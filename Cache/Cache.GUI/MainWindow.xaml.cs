using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace Cache.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string CacheFilesLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "711 Files - Megan Ganley", "CacheFiles");
        private readonly string CacheLogFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "711 Files - Megan Ganley", "log.txt");

        public MainWindow()
        {
            InitializeComponent();
        }


        private void QueryFileNamesButton_Click(object sender, RoutedEventArgs e)
        {
            string[] fullPaths = Directory.GetFiles(CacheFilesLocation);   
            string[] fileNames = fullPaths.Select(System.IO.Path.GetFileName).ToArray();
            
            FilesListBox.ItemsSource = fileNames;
        }

        private void ClearCacheButton_Click(object sender, RoutedEventArgs e)
        {
            WriteToLog("cache cleared at " + DateTime.Now);

            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(CacheFilesLocation);
            
            foreach (System.IO.FileInfo file in dir.GetFiles()) { file.Delete();}
            // need to handle nested dirs
            //  foreach (System.IO.DirectoryInfo subDirectory in dir.GetDirectories()) subDirectory.Delete(true);   

            QueryFileNamesButton_Click(sender, e);  
        }

        private void ViewLogButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(CacheLogFile);
        }

        public void WriteToLog(string message)
        {
            using (System.IO.StreamWriter w = File.AppendText(CacheLogFile))
            {
                w.WriteLine(message);

            }
        }

    }
}
