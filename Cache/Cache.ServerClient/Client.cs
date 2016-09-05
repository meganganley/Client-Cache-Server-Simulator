using System.Collections.Generic;
using System.Linq;
using Cache.ServerClient.FileServiceReference;

namespace Cache.ServerClient
{
    public class Client
    {
        public byte[] GetFile(string path)
        {
            FileServiceReference.FileServiceClient client = new FileServiceReference.FileServiceClient();
            return client.GetFile(path);
        }

        public IEnumerable<string> GetFileNames()
        {
            FileServiceReference.FileServiceClient client = new FileServiceReference.FileServiceClient();
            return client.GetFileNames();
        }

        public bool FileIsUpToDate(string path, byte[] hash)
        {
            FileServiceReference.FileServiceClient client = new FileServiceReference.FileServiceClient();
            return client.FileIsUpToDate(path, hash);
        }

        public IEnumerable<ChunkContent> GetModifiedChunks(string fullPath, IEnumerable<ChunkHash> hashSet)
        {
            FileServiceReference.FileServiceClient client = new FileServiceReference.FileServiceClient();
            return client.GetModifiedChunks(fullPath, hashSet.ToArray());
        }
    }

}
