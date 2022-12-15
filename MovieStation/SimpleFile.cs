using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStation
{
   
        [Serializable]
        public class SimpleFile
        {
            #region Members
            private string _fileDir;
            private string _fileName;
            private string _fileFormat;
            #endregion

            public string fullname { get; set; }
            //public string FileName { get; set; }
            #region Properties
            public string FileDir { get { return _fileDir; } set { _fileDir = value; OnPropertyChanged("FileDir"); } }
            public string FileName { get { return _fileName; } set { _fileName = value; OnPropertyChanged("FileName"); } }
            public string FileFormat { get { return _fileFormat; } set { _fileFormat = value; OnPropertyChanged("FileFormat"); } }
            #endregion


            // INotifyPropertyChanged interface
            [field: NonSerialized]
            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }
    }

