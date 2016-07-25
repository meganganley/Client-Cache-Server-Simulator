using System;
using System.Collections.Generic;
using System.IO;

namespace Server.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service.svc or Service.svc.cs at the Solution Explorer and start debugging.
    public class Service : IService
    {
        private static readonly string ServerFilesLocation = Path.Combine(Environment.CurrentDirectory, @"ServerFiles");

        public IEnumerable<string> GetFileNames()
        {
            try
            {
                Console.WriteLine(ServerFilesLocation);

                // Only get files that begin with the letter "c."
                string[] dirs = Directory.GetFiles(@"c:\", "c*");
                Console.WriteLine("The number of files starting with c is {0}.", dirs.Length);

                foreach (string dir in dirs)
                {
                    Console.WriteLine(dir);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }

            List<string> l = new List<string>{ ServerFilesLocation };


            return l;

        }

        public Stream GetFile(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
