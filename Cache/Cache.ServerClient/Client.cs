using System.Collections.Generic;

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
    }

}
