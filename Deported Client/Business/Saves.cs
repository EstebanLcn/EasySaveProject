using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Deported_Client.Business
{
    public class Saves : INotifyPropertyChanged
    {
        public string SaveName { get; set; }
        public string SourceRepository { get; set; }
        public string DestinationRepository { get; set; }
        public string SaveType { get; set; }
        public bool status;

        private int _nbFilesSourceRepository { get; set; }
        private int _nbFilesDestinationRepository { get; set; }

        public int NbFilesSourceRepository
        {
            get
            {
                return _nbFilesSourceRepository;
            }
            set
            {
                _nbFilesSourceRepository = value;
                OnPropertyChanged("NbFilesSourceRepository");
            }
        }

        public int NbFilesDestinationRepository
        {
            get
            {
                return _nbFilesDestinationRepository;
            }
            set
            {
                _nbFilesDestinationRepository = value;
                OnPropertyChanged("NbFilesDestinationRepository");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string paramName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(paramName));
            }
        }
    }
}
