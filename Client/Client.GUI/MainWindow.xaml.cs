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

        private string[] fullServerPaths = {};                 // TODO: FIX
        private string[] fileNames = {};

        public MainWindow()
        {
            InitializeComponent();
        }

        private void QueryFileNamesButton_Click(object sender, RoutedEventArgs e)
        {
            FileServiceReference.CacheFileServiceClient client = new FileServiceReference.CacheFileServiceClient();
            fullServerPaths = client.GetFileNames(); // want to preserve full path name in case of nested directories

            fileNames = fullServerPaths.Select(System.IO.Path.GetFileName).ToArray();    // Conflict in System.IO.Path vs some graphical element

            FilesListBox.ItemsSource = fileNames;

          //  FirstFileTextBox.Text = fileNames[0];

            client.Close(); // don't know where this is appropriate 
        }

        private void DownloadFilesButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = FilesListBox.SelectedIndex;

            FileServiceReference.CacheFileServiceClient client = new FileServiceReference.CacheFileServiceClient();
            byte[] b = client.GetFile(fullServerPaths[selectedIndex]);        // breaks when file too big -- change binding 


            File.WriteAllBytes(System.IO.Path.Combine(ClientFilesLocation, fileNames[selectedIndex]), b);

            Console.WriteLine("test");

            client.Close();
        }

        private void DisplayContentsButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = FilesListBox.SelectedIndex;
            string file = System.IO.Path.Combine(ClientFilesLocation, fileNames[selectedIndex]);
            System.Diagnostics.Process.Start(file); // some exception handling todo 
        }

        /*
        async void DefaultLaunch()
        {
            // Path to the file in the app package to launch
            string imageFile = @"images\test.png";

            System.Diagnostics.Process.Start(@"c:\textfile.txt");
            
            /*

            var file = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(imageFile);



            if (file != null)
            {
                // Launch the retrieved file
                var success = await Windows.System.Launcher.LaunchFileAsync(file);

                if (success)
                {
                    // File launched
                }
                else
                {
                    // File launch failed
                }
            }
            else
            {
                // Could not find file
            }
        }
        */
    }
}
