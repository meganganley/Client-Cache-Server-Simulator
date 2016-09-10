using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Server.Service
{
    public class FileService : IFileService
    {
    //     private static readonly string ServerFilesLocation = Path.Combine(Environment.CurrentDirectory, @"ServerFiles");
    //    static readonly string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        
  //      string folder = Path.Combine(desktopPath, @"Server Files");

      //  Directory.CreateDirectory(folder);

        private readonly string ServerFilesLocation =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "711 Files - Megan Ganley", "ServerFiles");

        public IEnumerable<string> GetFileNames()
        {
            string[] fullPaths = Directory.GetFiles(ServerFilesLocation);  
            return fullPaths;

        }
        // todo : byte[] to stream?
        public byte[] GetFile(string fullPath)
        {
            byte[] buff = File.ReadAllBytes(fullPath);
            return buff;
        }
        
        public bool FileIsUpToDate(string fullPath, byte[] hash)
        {
            return hash.SequenceEqual(GetFileHash(fullPath));
        }

        public IEnumerable<ChunkContent> GetModifiedChunks(string fullPath, List<ChunkHash> cacheChunkHashes)
        {
            byte[] b = File.ReadAllBytes(fullPath);
            List<byte[]> chunks = Common.RabinKarp.Slice(b, 0x01FFF);

            List<ChunkHash> serverChunkHashes = GenerateChunkHashSet(chunks);
            
            List<ChunkContent> latestContent = new List<ChunkContent>();

            bool foundCachedChunk = false;

            for (int i = 0; i < serverChunkHashes.Count; i++)
            {
                foundCachedChunk = false;

                var newHash = serverChunkHashes[i];

                foreach (var oldHash in cacheChunkHashes)
                {
                    if (oldHash.Hash.SequenceEqual(newHash.Hash))
                    {
                        // hash functions match indicates that chunk content is same, no need to pass chunk again
                        // but may need to update location/index of chunk within file on cache 
                        latestContent.Add(new ChunkContent
                        {
                            Content = null,
                            PreviousLocation = oldHash.Location,
                            UpdatedLocation = i,
                            UseUpdatedChunk = false 
                        });
                        foundCachedChunk = true;
                        break;
                    }
                }

                if (!foundCachedChunk)
                {
                    // found no matches, so chunk is modified and we should send the content to the cache  
                    latestContent.Add(new ChunkContent
                    {
                        Content = chunks[i],
                        PreviousLocation = -1,
                        UpdatedLocation = i,
                        UseUpdatedChunk = true
                    });

                }
            }
            return latestContent;
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
                    hashSet.Add(new ChunkHash {Hash = hash, Location = index});
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
                    return hash;
                }
            }
        }
    }
}
