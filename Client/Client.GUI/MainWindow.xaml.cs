using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client.GUI
{


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string ClientFilesLocation = @"C:\Users\Megan\Documents\S2 2016\CS 711\ClientFiles"; //todo

        private string[] fullPaths = {};                 // TODO: FIX
        private string[] fileNames = {};

        public MainWindow()
        {
            InitializeComponent();
        }

        private void QueryFileNamesButton_Click(object sender, RoutedEventArgs e)
        {
            FileServiceReference.FileServiceClient client = new FileServiceReference.FileServiceClient();
            fullPaths = client.GetFileNames(); // want to preserve full path name in case of nested directories

            fileNames = fullPaths.Select(System.IO.Path.GetFileName).ToArray();    // Conflict in System.IO.Path vs some graphical element

            FilesListBox.ItemsSource = fileNames;

            FirstFileTextBox.Text = fileNames[0];

            client.Close(); // don't know where this is appropriate 
        }

        private void DownloadFilesButton_Click(object sender, RoutedEventArgs e)
        {
            FileServiceReference.FileServiceClient client = new FileServiceReference.FileServiceClient();
            byte[] b = client.GetFile(fullPaths[0]);        // breaks when file too big -- change binding 

            File.WriteAllBytes(System.IO.Path.Combine(ClientFilesLocation, fileNames[0]), b);

            Console.WriteLine("test");

            client.Close();
        }
    }
}
