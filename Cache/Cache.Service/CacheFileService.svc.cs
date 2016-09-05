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
        private const string CacheFilesLocation = @"C:\Users\Megan\Documents\S2 2016\CS 711\CacheFiles";
        private const string CacheLogFile = @"C:\Users\Megan\Documents\S2 2016\CS 711\log.txt";

        public IEnumerable<string> GetFileNames()
        {
            WriteToLog("user request: get list of files available on server at " + DateTime.Now);

            Client c = new Client();
            WriteToLog("response: list of files returned");
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
                // File exists and up-to-date, can return full cached file 
                if (c.FileIsUpToDate(serverPath, GetFileHash(Path.Combine(CacheFilesLocation, file))))
                {
                    WriteToLog("response: cached file " + file);

                    return File.ReadAllBytes(serverPath);     // todo: breaks with nested dirs -- don't know if full serverPath required here

                }

                // File exists but is not up-to-date, get only required chunks from server
                WriteToLog("file " + file + " has an updated version available on server");

                // Generate chunks
                byte[] outOfDateBytes = File.ReadAllBytes(Path.Combine(CacheFilesLocation, file));
                List<byte[]> chunks = Common.RabinKarp.Slice(outOfDateBytes, 0x01FFF);

                List<ChunkHash> hashSet = GenerateChunkHashSet(chunks);

                // Send hashes of chunks to server
                IEnumerable<ChunkContent> updatedContent = c.GetModifiedChunks(serverPath, hashSet);

                // Receive required chunks

                // Assemble full file
                b =  AssembleFile(updatedContent, chunks);
                // Return full file 

                File.WriteAllBytes(System.IO.Path.Combine(CacheFilesLocation, file), b);

                return b;

            }
            // File does not exist on cache, request from server
            b = c.GetFile(serverPath);
            File.WriteAllBytes(System.IO.Path.Combine(CacheFilesLocation, file), b);

            WriteToLog("response: file " + file + " downloaded from the server");

            return b;
        }


        public byte[] AssembleFile(IEnumerable<ChunkContent> updatedContent, List<byte[]> cachedContent)
        {
            List<byte[]> orderedBytes = new List<byte[]>();

            double cacheChunksLength = 0.0;
            int serverChunksLength = 0;
            byte[] cachedChunk;

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
            
            WriteToLog("response: " + (int)(cacheChunksLength*100/total) + "% was constructed with cached data");

            byte[] final = new byte[total];
            int count = 0; 

            foreach (byte[] chunk in orderedBytes)
            {
                chunk.CopyTo(final, count);
                count += chunk.Length;
            }

            return final;
        }


        public List<byte[]> OrderChunks(IEnumerable<ChunkContent> updatedContent, List<byte[]> cachedContent)
        {
            return null;
        }


        public List<ChunkHash> GenerateChunkHashSet(List<byte[]> chunks)
        {
            List<ChunkHash> hashSet = new List<ChunkHash>();

            using (var cryptoService = new MD5CryptoServiceProvider())
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
            using (var cryptoService = new MD5CryptoServiceProvider())
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
