using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using Server.Service;

namespace Server.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Server";

            SetUpDirectory("ServerFiles");

            Uri baseAddress = new Uri("http://localhost:8082/Server/");

            ServiceHost selfHost = new ServiceHost(typeof(Service.FileService), baseAddress);

            try
            {
                selfHost.AddServiceEndpoint(typeof(IFileService), new WSHttpBinding(), "Service");

                ServiceMetadataBehavior smb = new ServiceMetadataBehavior { HttpGetEnabled = true };
                selfHost.Description.Behaviors.Add(smb);

                selfHost.Open();
                Console.WriteLine("The service is ready.");
                Console.WriteLine("Press <ENTER> to terminate service.");
                Console.WriteLine();

                string fullPath = @"C:\Users\Megan\Desktop\711 Files - Megan Ganley\ServerFiles\cute-golden-retriever-happy-puppies EDITS.jpg";
                PerformancMetrics(fullPath);

                Console.ReadLine();

                selfHost.Close();
            }
            catch (CommunicationException ce)
            {
                Console.WriteLine("An exception occurred: {0}", ce.Message);
                selfHost.Abort();
                Console.ReadLine();
            }
        }

        public static void SetUpDirectory(string directoryName)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string parentDirectoryPath = Path.Combine(desktopPath, "711 Files - Megan Ganley");
            Directory.CreateDirectory(parentDirectoryPath);

            string childDirectoryPath = Path.Combine(parentDirectoryPath, directoryName);
            Directory.CreateDirectory(childDirectoryPath);
        }

        public static void PerformancMetrics(string fullPath)
        {
            byte[] b = File.ReadAllBytes(fullPath);
            List<byte[]> chunks = Common.RabinKarp.Slice(b, 0x01FFF);
            //    List<byte[]> chunks = Common.RabinKarp.Slice(b, 0x03FF);


            Console.WriteLine("Size of first chunk: " + chunks[0].Length);
            double average = chunks.Average(x => x.Length);
            Console.WriteLine("Average size of chunk: " + average);
            Console.WriteLine("Number of chunks: " + chunks.Count);
            
            Console.ReadLine();
        }

    }
}
