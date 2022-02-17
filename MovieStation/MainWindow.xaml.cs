using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Forms;


namespace MovieStation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //I think I am brilliant!
        //basica functions
        /// <summary>
        /// load favorite songs by default
        /// scan music folder
        /// search for music by file name
        /// display the file
        /// open folder, open file
        /// add to favorite
        /// </summary>
        /// 

        public List<FileInfo> movieFiles = new List<FileInfo>();
        public List<SimpleFile> sfsmusic = new List<SimpleFile>();


        public List<SimpleFile> sfsMovie = new List<SimpleFile>();

        string musicPath = @"\\Xeon\d\Music";
        public bool isFlacOnly { get; set; }
        public bool isDirOnly { get; set; }

        public int CountFile { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            GetAllFolders();

            //    if (Directory.Exists(musicPath))
            //    {
            //        LoadMusicInFolder(musicPath);
            //    }
        }



        private void GetAllFolders()
        {
            List<string> allsharedDirs = new List<string>() {@"\\M6400\emule",
                                                            @"\\M6400\movie",
                                                            @"\\M6400\emule",
                                                            @"\\M6400\BaiduNetdiskDownload",
                                                            @"\\Quadcore\d\TV",
                                                            @"\\Quadcore\e\TV",
                                                            @"\\Quadcore\e\Movies",
                                                            @"\\Quadcore\g\TV",
                                                            @"\\Quadcore\g\New Movies",
                                                            @"\\Quadcore\h\BBC",
                                                            @"\\tp-share\I\Movies",
                                                            @"\\tp-share\H\TF",
                                                            @"\\MSI\Movies"
                                                            //@"\\tp-share\G\TV"
                                                             };

            foreach (var dir in allsharedDirs)
            {
                if (Directory.Exists(dir))
                {
                    LoadMovieFolders(dir);
                }
            }

            Maindtg.ItemsSource = sfsMovie;

            
        }



        private void LoadMovieFolders(string dir)
        {
            string[] allfiles = Directory.GetFiles(dir, "*.mkv", SearchOption.AllDirectories);


            sfsmusic = new List<SimpleFile>();
            foreach (var f in allfiles)
            {
                FileInfo fi = new FileInfo(f);
                movieFiles.Add(fi);
                sfsMovie.Add(new SimpleFile()
                {
                    FileDir = fi.DirectoryName,
                    FileFormat = fi.Extension,
                    FileName = fi.Name,
                    fullname = f
                });
            }

        }

    


        private void LoadMusicInFolder(string dir)
        {
            string[] allfiles = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);

            sfsmusic = new List<SimpleFile>();
            foreach (var f in allfiles)
            {
                FileInfo fi = new FileInfo(f);
                sfsmusic.Add(new SimpleFile()
                {
                    FileDir = fi.DirectoryName,
                    FileFormat = fi.Extension,
                    FileName = fi.Name,
                    fullname = f
                });
            }

            Maindtg.ItemsSource = sfsmusic;
        }

        private void folderSelection_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result.ToString() != string.Empty)
                {
                    musicPath = dialog.SelectedPath;
                }
            }
            if (Directory.Exists(musicPath))
            {
                LoadMusicInFolder(musicPath);
            }

        }





        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextBox.Text != "")
            {
                e.Handled = true;

                if (isDirOnly)
                {
                    if (isFlacOnly)
                    {
                        Maindtg.ItemsSource = from x in sfsmusic
                                              where x.FileDir.Contains(SearchTextBox.Text, StringComparison.OrdinalIgnoreCase)
                                              && x.FileFormat == ".flac"
                                              select x;
                    }
                    else
                    {
                        Maindtg.ItemsSource = from x in sfsmusic
                                              where x.FileDir.Contains(SearchTextBox.Text, StringComparison.OrdinalIgnoreCase)
                                              select x;
                    }
                }
                else
                {
                    if (isFlacOnly)
                    {
                        Maindtg.ItemsSource = from x in sfsmusic
                                              where x.FileName.Contains(SearchTextBox.Text, StringComparison.OrdinalIgnoreCase)
                                              && x.FileFormat == ".flac"
                                              select x;
                    }
                    else
                    {
                        Maindtg.ItemsSource = from x in sfsmusic
                                              where x.FileName.Contains(SearchTextBox.Text, StringComparison.OrdinalIgnoreCase)
                                              select x;
                    }

                }

            }

            if (SearchTextBox.Text == "")
            {
                e.Handled = true;

                Maindtg.ItemsSource = sfsmusic;
            }
        }



        private void Play_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(((SimpleFile)Maindtg.SelectedItem).fullname);
        }

        private void OpenFolder_Click(object sender, RoutedEventArgs e)
        {

            System.Diagnostics.Process.Start(((SimpleFile)Maindtg.SelectedItem).FileDir);
        }

        private void Maindtg_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(((SimpleFile)Maindtg.SelectedItem).fullname);
        }

     

        private void PlayMovie_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(((SimpleFile)Maindtg.SelectedItem).fullname);
        }

        private void OpenMvFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(((SimpleFile)Maindtg.SelectedItem).FileDir);
        }

        private void MovieMaindtg_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(((SimpleFile)Maindtg.SelectedItem).fullname);
        }


    }

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

