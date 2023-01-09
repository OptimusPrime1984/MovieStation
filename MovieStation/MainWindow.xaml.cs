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
using System.IO.Compression;

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
        

        public List<SimpleFile> sfsMovie = new List<SimpleFile>();

        List<string> moviePaths =new List<string> { @"\\Xeon\e\AllMovies", @"\\Xeon\e\NewTV", @"\\Xeon\e\TV" };
        string movieIndexFile = @"\\Xeon\e\AllMovies\index.sf";
        public bool isFlacOnly { get; set; }
        public bool isDirOnly { get; set; }

        public int CountFile { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            sfsMovie = new List<SimpleFile>();
            this.DataContext = this;


            if (File.Exists(movieIndexFile))
            {
                readSimpleFileList(movieIndexFile);
            }
            else
            {
                foreach(var mp in moviePaths)
                {
                    if (Directory.Exists(mp))
                    {
                        LoadMusicInFolder(mp);
                    }
                }                              

                saveSimpleFileList(sfsMovie);
            }

            Maindtg.ItemsSource = sfsMovie;
        }


    
        private void LoadMusicInFolder(string dir)
        {
            string[] allfiles = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);

            
            foreach (var f in allfiles)
            {
                FileInfo fi = new FileInfo(f);
                sfsMovie.Add(new SimpleFile()
                {
                    FileDir = fi.DirectoryName,
                    FileFormat = fi.Extension,
                    FileName = fi.Name,
                    fullname = f,
                    size= SizeConverter(fi.Length)
                });
            }

            sfsMovie = sfsMovie.Distinct().ToList();
        }

        public void saveSimpleFileList(List<SimpleFile> sfsmusic)
        {
            WriteToZipFile<List<SimpleFile>>(movieIndexFile, sfsmusic);
        }

        public void readSimpleFileList(string saveFilePath)
        {
            sfsMovie = ReadFromZipFile<List<SimpleFile>>(movieIndexFile);
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
                        Maindtg.ItemsSource = from x in sfsMovie
                                              where x.FileDir.Contains(SearchTextBox.Text, StringComparison.OrdinalIgnoreCase)
                                              && x.FileFormat == ".flac"
                                              select x;
                    }
                    else
                    {
                        Maindtg.ItemsSource = from x in sfsMovie
                                              where x.FileDir.Contains(SearchTextBox.Text, StringComparison.OrdinalIgnoreCase)
                                              select x;
                    }
                }
                else
                {
                    if (isFlacOnly)
                    {
                        Maindtg.ItemsSource = from x in sfsMovie
                                              where x.FileName.Contains(SearchTextBox.Text, StringComparison.OrdinalIgnoreCase)
                                              && x.FileFormat == ".flac"
                                              select x;
                    }
                    else
                    {
                        Maindtg.ItemsSource = from x in sfsMovie
                                              where x.FileName.Contains(SearchTextBox.Text, StringComparison.OrdinalIgnoreCase)
                                              select x;
                    }

                }

            }

            if (SearchTextBox.Text == "")
            {
                e.Handled = true;

                Maindtg.ItemsSource = sfsMovie;
            }
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(((SimpleFile)Maindtg.SelectedItem).fullname);
            }
            catch
            {
                System.Windows.MessageBox.Show("No Such File, please refresh");
            }
            
        }

        private void OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            try { 
            System.Diagnostics.Process.Start(((SimpleFile)Maindtg.SelectedItem).FileDir);
            }
            catch
            {
                System.Windows.MessageBox.Show("No Such Folder");
            }
        }

        private void Maindtg_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(((SimpleFile)Maindtg.SelectedItem).fullname);
            }
            catch
            {
                System.Windows.MessageBox.Show("No Such File, please refresh");
            }
        }


        public void WriteToZipFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.OpenOrCreate, FileAccess.ReadWrite))
            //using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            using (GZipStream gz = new GZipStream(stream, CompressionMode.Compress))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(gz, objectToWrite);
            }
        }


        public T ReadFromZipFile<T>(string filePath)
        {
            //using (Stream stream = File.Open(filePath, FileMode.Open))
            using (Stream stream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            using (GZipStream gz = new GZipStream(stream, CompressionMode.Decompress))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T)binaryFormatter.Deserialize(gz);
            }
        }

        private void RefreshClick(object sender, RoutedEventArgs e)
        {
            foreach(var mp in moviePaths)
            {
                if (Directory.Exists(mp))
                {
                    LoadMusicInFolder(mp);
                }
            }
            

            saveSimpleFileList(sfsMovie);
            Maindtg.ItemsSource = sfsMovie;
        }

        private string SizeConverter(long Length)
        {
            string sLen = Length.ToString();
            if (Length >= (1 << 30))
                sLen = string.Format("{0}Gb", Length >> 30);
            else
            if (Length >= (1 << 20))
                sLen = string.Format("{0}Mb", Length >> 20);
            else
            if (Length >= (1 << 10))
                sLen = string.Format("{0}Kb", Length >> 10);

            return sLen;
        }
    }

 
    public class IOFunctions
    {
        public static void WriteToZipFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.OpenOrCreate, FileAccess.ReadWrite))
            //using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            using (GZipStream gz = new GZipStream(stream, CompressionMode.Compress))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(gz, objectToWrite);
            }
        }


        public static T ReadFromZipFile<T>(string filePath)
        {
            //using (Stream stream = File.Open(filePath, FileMode.Open))
            using (Stream stream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            using (GZipStream gz = new GZipStream(stream, CompressionMode.Decompress))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T)binaryFormatter.Deserialize(gz);
            }
        }

    }
}

