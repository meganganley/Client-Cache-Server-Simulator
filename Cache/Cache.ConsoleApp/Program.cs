using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Cache.Service;

namespace Cache.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri baseAddress = new Uri("http://localhost:8081/Cache/");     //TODO 

            ServiceHost selfHost = new ServiceHost(typeof(CacheFileService), baseAddress);

            try
            {
                selfHost.AddServiceEndpoint(typeof(ICacheFileService), new WSHttpBinding(), "Service");// change from ws to basic

                ServiceMetadataBehavior smb = new ServiceMetadataBehavior {HttpGetEnabled = true};
                selfHost.Description.Behaviors.Add(smb);

                Console.WriteLine("The cache service is ready.");
                Console.WriteLine("Press <ENTER> to terminate service.");
                Console.WriteLine();
                Console.ReadLine();

                // Close the ServiceHostBase to shutdown the service.
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
