using System;
using System.IO;
using System.Windows;

namespace Client.GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            SetUpDirectory("Client Files");
        }

        public static void SetUpDirectory(string directoryName)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string directoryPath = Path.Combine(desktopPath, directoryName);

            Directory.CreateDirectory(directoryPath);
        }
    }
}
