using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Server.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FileService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select FileService.svc or FileService.svc.cs at the Solution Explorer and start debugging.
    public class FileService : IFileService
    {
        // private static readonly string ServerFilesLocation = Path.Combine(Environment.CurrentDirectory, @"ServerFiles");

        private static readonly string ServerFilesLocation = @"C:\Users\Megan\Documents\S2 2016\CS 711\ServerFiles";

        public IEnumerable<string> GetFileNames()
        {
            string[] fullPaths = Directory.GetFiles(ServerFilesLocation);   //  .Select(Path.GetFileName)
                                                                              // .ToArray();

            

            return fullPaths;
            /*
                
            return Path.GetFileName(ServerFilesLocation);
            try
            {
                Console.WriteLine(ServerFilesLocation);

                // Only get files that begin with the letter "c."
                
                length = dirs.Length;
                Console.WriteLine("The number of files starting with e is {0}.", dirs.Length);

                foreach (string dir in dirs)
                {
                    Console.WriteLine(dir);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }

            List<string> l = new List<string>{ ""+length };


            return dirs;
            */

        }

        public byte[] GetFile(string fullPath)
        {

            byte[] buff = File.ReadAllBytes(fullPath);
            return buff;
            /*
          //  FileStream fs = new FileStream(fullPath, FileMode.Open);
            try
            {
                FileStream imageFile = File.OpenRead(fullPath);
                return imageFile;
            }
            catch (IOException ex)
            {
                Console.WriteLine(
                    String.Format("An exception was thrown while trying to open file {0}", fullPath));
                Console.WriteLine("Exception is: ");
                Console.WriteLine(ex.ToString());
                throw ex;
            }
           // return fs;
           */
        }
    }
}
