using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Cache.ServerClient;
using Cache.ServerClient.FileServiceReference;

namespace Cache.Service
{
    public class CacheFileService : ICacheFileService
    {
        private readonly string CacheFilesLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "711 Files - Megan Ganley", "CacheFiles");
        private readonly string CacheLogFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "711 Files - Megan Ganley", "log.txt");

        public IEnumerable<string> GetFileNames()
        {
            WriteToLog("user request: get list of files available on server at " + DateTime.Now);

            Client c = new Client();
            WriteToLog("response: list of files returned\n");
            return c.GetFileNames();
        }

        public byte[] GetFile(string serverPath)
        {
            string file = Path.GetFileName(serverPath);

            WriteToLog("user request: file " + file + " at " + DateTime.Now);

            Client c = new Client();
            byte[] b;
            
            if (File.Exists(Path.Combine(CacheFilesLocation, file)))
            {
                if (c.FileIsUpToDate(serverPath, GetFileHash(Path.Combine(CacheFilesLocation, file))))
                {
                    // File exists and up-to-date, can return full cached file 
                    return GetFileFromCache(serverPath, file);
                }

                /*  PART 2  */

                // there is an updated version of the file on the cache
                // query for only relevant chunks 
            //    return GetChunksFromServer(c, serverPath, file);
            }

            // File does not exist on cache, or update available in Part 1 - request from server
            return GetFileFromServer(c, serverPath, file);
        }

        public byte[] GetFileFromServer(Client c, string serverPath, string file)
        {
            byte[] b = c.GetFile(serverPath);

            WriteToLog("Cache received file at " + DateTime.Now.ToString("hh.mm.ss.ffffff"));

            // save file to cache for future use
            File.WriteAllBytes(System.IO.Path.Combine(CacheFilesLocation, file), b);

            WriteToLog("response: file " + file + " downloaded from the server\n");

            return b;
        }

        public byte[] GetFileFromCache(string serverPath, string file)
        {
            WriteToLog("response: cached file " + file + "\n");

            return File.ReadAllBytes(serverPath);     // todo: breaks with nested dirs -- don't know if full serverPath required here

        }

        public byte[] GetChunksFromServer(Client c, string serverPath, string file)
        {
            WriteToLog("file " + file + " has an updated version available on server");

            // Generate old chunks
            byte[] outOfDateBytes = File.ReadAllBytes(Path.Combine(CacheFilesLocation, file));
            List<byte[]> chunks = Common.RabinKarp.Slice(outOfDateBytes, 0x01FFF);

            List<ChunkHash> hashSet = GenerateChunkHashSet(chunks);

            // Send hashes of chunks to server to compare to their hashes 
            IEnumerable<ChunkContent> updatedContent = c.GetModifiedChunks(serverPath, hashSet);

            WriteToLog("Cache received chunks at " + DateTime.Now.ToString("hh.mm.ss.ffffff"));


            // Assemble full file
            byte[] b = OrderChunks(updatedContent, chunks);
            
            // save updated file to cache for future use, overwriting previous version
            File.WriteAllBytes(System.IO.Path.Combine(CacheFilesLocation, file), b);

            return b;
        }
        

        public byte[] OrderChunks(IEnumerable<ChunkContent> updatedContent, List<byte[]> cachedContent)
        {
            List<byte[]> orderedBytes = new List<byte[]>();

            double cacheChunksLength = 0.0;
            int serverChunksLength = 0;
            byte[] cachedChunk;


            // We don't have any guarantees on order the chunks are received in
            foreach (ChunkContent chunk in updatedContent)
            {
                if (chunk.UseUpdatedChunk)
                {
                    serverChunksLength += chunk.Content.Length;
                    orderedBytes.Insert(chunk.UpdatedLocation, chunk.Content);
                }
                else
                {
                    cachedChunk = cachedContent[chunk.PreviousLocation];
                    cacheChunksLength += cachedChunk.Length;
                    orderedBytes.Insert(chunk.UpdatedLocation, cachedChunk);
                }
            }
            int total = serverChunksLength + (int)cacheChunksLength;
            
            WriteToLog("response: " + (int)(cacheChunksLength*100/total) + "% was constructed with cached data\n");

            return AssembleChunks(orderedBytes, cachedContent, total);
        }

        public byte[] AssembleChunks(List<byte[]> orderedBytes, List<byte[]> cachedContent, int length)
        {
            byte[] final = new byte[length];
            int count = 0;

            // We have the correct order to arrange byte arrays in, now want to arrange in one large byte array
            foreach (byte[] chunk in orderedBytes)
            {
                chunk.CopyTo(final, count);
                count += chunk.Length;
            }

            return final;
        }


        public List<ChunkHash> GenerateChunkHashSet(List<byte[]> chunks)
        {
            List<ChunkHash> hashSet = new List<ChunkHash>();

            using (var cryptoService = new SHA256CryptoServiceProvider())
            {
                for (int index = 0; index < chunks.Count; index++)
                {
                    byte[] chunk = chunks[index];
                    var hash = cryptoService.ComputeHash(chunk);
                    hashSet.Add(new ChunkHash { Hash = hash, Location = index });
                }
            }

            return hashSet;
        }



        static byte[] GetFileHash(string filePath)
        {
            using (var cryptoService = new SHA256CryptoServiceProvider())
            {
                using (var fileStream = new FileStream(filePath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite))
                {
                    var hash = cryptoService.ComputeHash(fileStream);
                    var hashString = Convert.ToBase64String(hash);
                    return hash;
                }
            }
        }

        public void WriteToLog(string message)
        {
            using (System.IO.StreamWriter w = File.AppendText(CacheLogFile))
            {
                w.WriteLine(message);

            }
        }
    }
}
