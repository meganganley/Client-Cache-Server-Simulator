using System.Collections.Generic;
using System.IO;

namespace Server.Service
{
    public class FileService : IFileService
    {
        // private static readonly string ServerFilesLocation = Path.Combine(Environment.CurrentDirectory, @"ServerFiles");

        private const string ServerFilesLocation = @"C:\Users\Megan\Documents\S2 2016\CS 711\ServerFiles";

        public IEnumerable<string> GetFileNames()
        {
            string[] fullPaths = Directory.GetFiles(ServerFilesLocation);   //  .Select(Path.GetFileName)
                                                                              // .ToArray()  
            return fullPaths;

        }
        // todo : byte[] to string?
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
