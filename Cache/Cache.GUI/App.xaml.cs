using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Windows;
using Cache.Service;

namespace Cache.GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceHost _selfHost;

        protected override void OnStartup(StartupEventArgs e)
        {
            SetUpDirectory("Cache Files");

            Uri baseAddress = new Uri("http://localhost:8081/Cache/");    

            _selfHost = new ServiceHost(typeof(CacheFileService), baseAddress);

            try
            {
                _selfHost.AddServiceEndpoint(typeof(ICacheFileService), new WSHttpBinding(), "Service");

                ServiceMetadataBehavior smb = new ServiceMetadataBehavior {HttpGetEnabled = true};
                _selfHost.Description.Behaviors.Add(smb);

                _selfHost.Open();
            }
            catch (CommunicationException ce)
            {
                // todo: capture message somewhere
                _selfHost.Abort();
            }
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _selfHost.Close();
        }


        public static void SetUpDirectory(string directoryName)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string directoryPath = Path.Combine(desktopPath, directoryName);

            Directory.CreateDirectory(directoryPath);
        }
    }
}
