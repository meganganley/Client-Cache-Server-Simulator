using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using Server.Service;

namespace Server.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Step 1 Create a URI to serve as the base address.
            Uri baseAddress = new Uri("http://localhost:8000/Server/");

            // Step 2 Create a ServiceHost instance
<<<<<<< Updated upstream
            ServiceHost selfHost = new ServiceHost(typeof(Service.Service), baseAddress);
=======
            ServiceHost selfHost = new ServiceHost(typeof(Service.FileService), baseAddress);
>>>>>>> Stashed changes

            try
            {
                // Step 3 Add a service endpoint.
<<<<<<< Updated upstream
                selfHost.AddServiceEndpoint(typeof(IService), new WSHttpBinding(), "Service");
=======
                selfHost.AddServiceEndpoint(typeof(IFileService), new WSHttpBinding(), "FileService");
>>>>>>> Stashed changes

                // Step 4 Enable metadata exchange.
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                selfHost.Description.Behaviors.Add(smb);

                // Step 5 Start the service.
                selfHost.Open();
                Console.WriteLine("The service is ready.");
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
            }
        }
    }
}
