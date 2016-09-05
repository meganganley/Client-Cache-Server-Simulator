using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using Common;
using Server.Service;

namespace Server.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Server";

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

             //   string fullPath = @"C:\Users\Megan\Documents\S2 2016\CS 711\ServerFiles\wiki.txt";
           //     TestAlg(fullPath);

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

        public static void TestAlg(string fullPath)
        {
            byte[] b = File.ReadAllBytes(fullPath);
            List<byte[]> chunks = Common.RabinKarp.Slice(b, 0x01FFF);

            Console.WriteLine(chunks[0].Length);
            double average = chunks.Average(x => x.Length);
            Console.WriteLine("Av:" + average);
            Console.WriteLine(chunks[0].Length);

            Console.ReadLine();
        }

    }
}
