using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace Client.GUI
{
    public partial class MainWindow : Window
    {
        private const string ClientFilesLocation = @"C:\Users\Megan\Documents\S2 2016\CS 711\ClientFiles"; //todo

        private string[] _fullServerPaths = { };                 // TODO: FIX
        private string[] _fileNames = { };
        private string[] _fileStatuses = { };

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

            _fileStatuses = new string[_fileNames.Length];

            for (int i = 0; i < _fileStatuses.Length; i++)
            {
                _fileStatuses[i] = "downloaded";
            }


            client.Close();
        }

        private void DownloadFileButton_Click(object sender, RoutedEventArgs e)
        {
            // which file currently dealing with 
            int selectedIndex = FilesListBox.SelectedIndex;

            if (selectedIndex == -1)
            {
                return;
            }
            _fileStatuses[selectedIndex] = "Downloading...";
            //         FilesDownloadedItemsControl.ItemsSource = _fileStatuses;     // todo fix


            FileServiceReference.CacheFileServiceClient client = new FileServiceReference.CacheFileServiceClient();
            byte[] b = client.GetFile(_fullServerPaths[selectedIndex]);

            File.WriteAllBytes(System.IO.Path.Combine(ClientFilesLocation, _fileNames[selectedIndex]), b);

            client.Close();

            _fileStatuses[selectedIndex] = "Downloaded";
            //          FilesDownloadedItemsControl.ItemsSource = _fileStatuses;


        }

        private void DisplayContentsButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = FilesListBox.SelectedIndex;

            if (selectedIndex == -1)
            {
                return;
            }

            string file = System.IO.Path.Combine(ClientFilesLocation, _fileNames[selectedIndex]);
            
            // use windows default program to open file 
            try
            {
                System.Diagnostics.Process.Start(file);
            }
            catch (Exception ex)
            {
                // todo 
                System.Windows.Forms.MessageBox.Show("That file has not been downloaded yet");
            }
        }

    }
}
