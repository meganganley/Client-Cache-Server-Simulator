using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Client.GUI
{
    public partial class MainWindow : Window
    {
        private const string ClientFilesLocation = @"C:\Users\Megan\Documents\S2 2016\CS 711\ClientFiles"; //todo

        private string[] _fullServerPaths = {};                 // TODO: FIX
        private string[] _fileNames = {};

        public MainWindow()
        {
            InitializeComponent();
        }

        private void QueryFileNamesButton_Click(object sender, RoutedEventArgs e)
        {
            FileServiceReference.CacheFileServiceClient client = new FileServiceReference.CacheFileServiceClient();
            
            // want to preserve full path name in case of nested directories
            _fullServerPaths = client.GetFileNames();

            // Conflict in System.IO.Path vs some graphical element
            _fileNames = _fullServerPaths.Select(System.IO.Path.GetFileName).ToArray();

            FilesListBox.ItemsSource = _fileNames;

            client.Close(); 
        }

        private void DownloadFilesButton_Click(object sender, RoutedEventArgs e)
        {
            // which file currently dealing with 
            int selectedIndex = FilesListBox.SelectedIndex;

            FileServiceReference.CacheFileServiceClient client = new FileServiceReference.CacheFileServiceClient();
            byte[] b = client.GetFile(_fullServerPaths[selectedIndex]);        

            File.WriteAllBytes(System.IO.Path.Combine(ClientFilesLocation, _fileNames[selectedIndex]), b);

            client.Close();
        }

        private void DisplayContentsButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = FilesListBox.SelectedIndex;
            string file = System.IO.Path.Combine(ClientFilesLocation, _fileNames[selectedIndex]);
            // use windows default program to open file 
            System.Diagnostics.Process.Start(file); // some exception handling todo 
        }

    }
}
