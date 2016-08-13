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

namespace Cache.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string CacheFilesLocation = @"C:\Users\Megan\Documents\S2 2016\CS 711\CacheFiles";

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
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(CacheFilesLocation);
            
            foreach (System.IO.FileInfo file in dir.GetFiles()) file.Delete();
          //  foreach (System.IO.DirectoryInfo subDirectory in dir.GetDirectories()) subDirectory.Delete(true);   // need to handle nested dirs

            QueryFileNamesButton_Click(sender, e);  // works?? todo 

            // make sure to refresh list of files as well todo 

        }

        private void ViewLogButton_Click(object sender, RoutedEventArgs e)
        {
//            int selectedIndex = FilesListBox.SelectedIndex;
//            string file = System.IO.Path.Combine(ClientFilesLocation, fileNames[selectedIndex]);
//            System.Diagnostics.Process.Start(file); // some exception handling todo 
        }
    }
}
