using System;
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

            Uri baseAddress = new Uri("http://localhost:8082/Server/");

            ServiceHost selfHost = new ServiceHost(typeof(Service.FileService), baseAddress);

            try
            {
                selfHost.AddServiceEndpoint(typeof(IFileService), new WSHttpBinding(), "Service");

                ServiceMetadataBehavior smb = new ServiceMetadataBehavior {HttpGetEnabled = true};
                selfHost.Description.Behaviors.Add(smb);

                selfHost.Open();
                Console.WriteLine("The service is ready.");
                Console.WriteLine("Press <ENTER> to terminate service.");
                Console.WriteLine();
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
    }
}
