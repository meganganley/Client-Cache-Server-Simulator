using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
