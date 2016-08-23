using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
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

        public byte[] GetFile(string serverPath)
        {
            string file = Path.GetFileName(serverPath);

            WriteToLog("user request: file " + file + " at " + DateTime.Now);

            Client c = new Client();

            if (File.Exists(Path.Combine(CacheFilesLocation, file)))
            {
                if (c.FileIsUpToDate(serverPath, GetHashCode(Path.Combine(CacheFilesLocation, file))))
                {
                    WriteToLog("response: cached file " + file);

                    return File.ReadAllBytes(serverPath);     // todo: breaks with nested dirs -- don't know if full serverPath required here

                }

                WriteToLog("file " + file + " has an updated version available on server");

            }
            byte[] b = c.GetFile(serverPath);
            File.WriteAllBytes(System.IO.Path.Combine(CacheFilesLocation, file), b);

            WriteToLog("response: file " + file + " downloaded from the server");

            return b;
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
