using System.Windows;

namespace Client.GUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void QueryFileNamesButton_Click(object sender, RoutedEventArgs e)
        {
            await ((MainWindowViewModel)DataContext).QueryFileNames();
        }

        private async void DownloadFileButton_Click(object sender, RoutedEventArgs e)
        {
            await ((MainWindowViewModel)DataContext).DownloadFile();
        }

        private void DisplayContentsButton_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)DataContext).DisplayFile();
            
        }

    }
}
