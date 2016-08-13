using System.Collections.Generic;
using System.IO;
using Cache.ServerClient;

namespace Cache.Service
{
    public class CacheFileService : ICacheFileService
    {
        private const string CacheFilesLocation = @"C:\Users\Megan\Documents\S2 2016\CS 711\CacheFiles";

        private string[] fullServerPaths = { };                 // TODO: FIX
        private string[] fileNames = { };

        public IEnumerable<string> GetFileNames()
        {
            Client c = new Client();
            return c.GetFileNames();
        }
    
        public byte[] GetFile(string path)
        {
            string file = Path.GetFileName(path);

            if (File.Exists(Path.Combine(CacheFilesLocation, file)))
            {
                return File.ReadAllBytes(path);     // todo: breaks with nested dirs -- don't know if full path required here
            }
            else
            {
                Client c = new Client();
                byte[] b = c.GetFile(path);
                File.WriteAllBytes(System.IO.Path.Combine(CacheFilesLocation, file), b);

                return b;
            }


            // TODO: CHECK FILE NAME, IF IN CACHE RETURN. ELSE GET FROM SERVER SERVICEREFERENCE

        }
    }
}
