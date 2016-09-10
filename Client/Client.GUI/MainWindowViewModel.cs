using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Client.GUI.Annotations;

namespace Client.GUI
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly string ClientFilesLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "711 Files - Megan Ganley", "ClientFiles");

        private string[] _fullServerPaths = { };                 // TODO: FIX
        private string[] _fileNames = { };

        private int _selectedIndex;

        private ObservableCollection<FileDisplayed> _listOfFilesToDisplay;
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public async Task QueryFileNames()
        {
            using (var client = new FileServiceReference.CacheFileServiceClient())
            {
                // want to preserve full path name in case of nested directories
                _fullServerPaths = await client.GetFileNamesAsync();

                // Conflict in System.IO.Path vs some graphical element
                _fileNames = _fullServerPaths.Select(Path.GetFileName).ToArray();

                ListOfFilesToDisplay.Clear();
                foreach (string file in _fileNames)
                {
                    ListOfFilesToDisplay.Add(new FileDisplayed { Filename = file, Status = string.Empty });
                }
                // update ui
                NotifyPropertyChanged(nameof(ListOfFilesToDisplay));

            }
        }

        public async Task DownloadFile()
        {
            if (SelectedIndex == -1 || ListOfFilesToDisplay.Count <= 0)
            {
                return;
            }

            // alert user that download is in progress
            ListOfFilesToDisplay[SelectedIndex].Status = "Downloading...";

            using (var client = new FileServiceReference.CacheFileServiceClient())
            {
                byte[] b = await client.GetFileAsync(_fullServerPaths[SelectedIndex]);

                File.WriteAllBytes(System.IO.Path.Combine(ClientFilesLocation, _fileNames[SelectedIndex]), b);

            }
            // alert user that file has been downloaded
            ListOfFilesToDisplay[SelectedIndex].Status = "Downloaded";

        }

        public void DisplayFile()
        {
            if (SelectedIndex == -1 || ListOfFilesToDisplay.Count <= 0)
            {
                return;
            }

            string file = System.IO.Path.Combine(ClientFilesLocation, _fileNames[SelectedIndex]);

            // use windows default program to open file 
            try
            {
                System.Diagnostics.Process.Start(file);
            }
            catch (Exception ex)
            {
                // file must have been deleted from client side or equivalent -- update ui to our best knowledge of situation
                ListOfFilesToDisplay[SelectedIndex].Status = String.Empty;
                System.Windows.Forms.MessageBox.Show("That file has not been downloaded yet");
            }
        }

        public ObservableCollection<FileDisplayed> ListOfFilesToDisplay
        {
            get
            {
                if (_listOfFilesToDisplay == null)
                {
                    _listOfFilesToDisplay = new ObservableCollection<FileDisplayed>();
                }
                return _listOfFilesToDisplay;
            }

            set
            {
                _listOfFilesToDisplay = value;
                NotifyPropertyChanged();
            }
        }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                NotifyPropertyChanged(nameof(SelectedIndex));
            }
        }
    }
}
