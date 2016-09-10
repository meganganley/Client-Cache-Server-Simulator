using System.ComponentModel;
using System.Runtime.CompilerServices;
using Client.GUI.Annotations;

namespace Client.GUI
{
    public class FileDisplayed : INotifyPropertyChanged
    {
        private string _fileName;
        private string _status;

        public string Filename {
            get { return _fileName; }
            set
            {
                _fileName = value;
                OnPropertyChanged(nameof(Filename));
            }
        }

        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}