using System.Collections.Generic;
using System.IO;

namespace Cache.Service
{
    public class CacheFileService : ICacheFileService
    {
        private const string CacheFilesLocation = @"C:\Users\Megan\Documents\S2 2016\CS 711\CacheFiles";

        private string[] fullServerPaths = { };                 // TODO: FIX
        private string[] fileNames = { };

        public IEnumerable<string> GetFileNames()
        {
            // TODO: GET FILENAMES FROM SERVER SERVICEREFERENCE

            string[] fullPaths = Directory.GetFiles(CacheFilesLocation);   //  .Select(Path.GetFileName)
                                                                           // .ToArray()  
            return fullPaths;

        }
    
        public byte[] GetFile(string path)
        {

            // TODO: CHECK FILE NAME, IF IN CACHE RETURN. ELSE GET FROM SERVER SERVICEREFERENCE

            byte[] buff = File.ReadAllBytes(path);
            return buff;
        }
    }
}
