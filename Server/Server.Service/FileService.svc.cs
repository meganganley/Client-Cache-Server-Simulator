using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Server.Service
{
    public class FileService : IFileService
    {
        // private static readonly string ServerFilesLocation = Path.Combine(Environment.CurrentDirectory, @"ServerFiles");

        private const string ServerFilesLocation = @"C:\Users\Megan\Documents\S2 2016\CS 711\ServerFiles";

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
            return hash.SequenceEqual(GetHashCode(fullPath));
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

                for (int j = 0; j < cacheChunkHashes.Count; j++)
                {
                    var oldHash = cacheChunkHashes[j];
                    if (oldHash.Hash.SequenceEqual(newHash.Hash))
                    {
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
                    latestContent.Add(new ChunkContent { Content = chunks[i], PreviousLocation = -1, UpdatedLocation = i, UseUpdatedChunk = true });

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



        static byte[] GetHashCode(string filePath)
        {
            using (var cryptoService = new MD5CryptoServiceProvider())
            {
                using (var fileStream = new FileStream(filePath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite))
                {
                    var hash = cryptoService.ComputeHash(fileStream);
                  //  var hashString = Convert.ToBase64String(hash);
                    return hash;
                }
            }
        }
    }
}
