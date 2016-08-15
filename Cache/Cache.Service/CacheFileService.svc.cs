using System;
using System.Collections.Generic;
using System.IO;
using Cache.ServerClient;

namespace Cache.Service
{
    public class CacheFileService : ICacheFileService
    {
        private const string CacheFilesLocation = @"C:\Users\Megan\Documents\S2 2016\CS 711\CacheFiles";
        private const string CacheLogFile = @"C:\Users\Megan\Documents\S2 2016\CS 711\log.txt";

        private string[] fullServerPaths = { };                 // TODO: FIX
        private string[] fileNames = { };

        public IEnumerable<string> GetFileNames()
        {
            WriteToLog("user request: get list of files available on server at " + DateTime.Now);

            Client c = new Client();
            WriteToLog("response: list of files returned");
            return c.GetFileNames();
        }
    
        public byte[] GetFile(string path)
        {
            string file = Path.GetFileName(path);

            WriteToLog("user request: file " + file + " at " + DateTime.Now);

            if (File.Exists(Path.Combine(CacheFilesLocation, file)))
            {
                WriteToLog("response: cached file " + file);

                return File.ReadAllBytes(path);     // todo: breaks with nested dirs -- don't know if full path required here
            }
            else
            {
                Client c = new Client();
                byte[] b = c.GetFile(path);
                File.WriteAllBytes(System.IO.Path.Combine(CacheFilesLocation, file), b);

                WriteToLog("response: file " + file + " downloaded from the server");

                return b;
            }


            // TODO: CHECK FILE NAME, IF IN CACHE RETURN. ELSE GET FROM SERVER SERVICEREFERENCE

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
