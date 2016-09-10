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
            SetUpDirectory("ClientFiles");
        }

        public static void SetUpDirectory(string directoryName)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string parentDirectoryPath = Path.Combine(desktopPath, "711 Files - Megan Ganley");
            Directory.CreateDirectory(parentDirectoryPath);

            string childDirectoryPath = Path.Combine(parentDirectoryPath, directoryName);
            Directory.CreateDirectory(childDirectoryPath);
        }
    }
}
