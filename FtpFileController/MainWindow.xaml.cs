using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
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
using FtpFileController.Commands;
using FtpFileController.Servicies;

namespace FtpFileController {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        
        public ICommand DownloadFileCommand { get;  }
        public ICommand UploadFileCommand { get;  }
        public ICommand OpenFileNameWindowCommand { get; }

        private readonly FtpService _ftpService;
        
        public MainWindow(FtpService ftpService) {
            InitializeComponent();
            DataContext = this;

            _ftpService = ftpService;
            
            DownloadFileCommand = new DefaultCommand(_ftpService.DownloadFile, _ => !_ftpService.InProgress && !_ftpService.TempFileExists());
            UploadFileCommand = new DefaultCommand(_ftpService.UploadFile, _ => !_ftpService.InProgress && _ftpService.TempFileExists());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            
        }

        private void Test()
        {
            //var uri = "ftp://91.201.253.216:40021";
            //var credentials = new NetworkCredential("Bakery", "@#Bake755");
            //var request = (FtpWebRequest)WebRequest.Create(uri);
            //request.Method = WebRequestMethods.Ftp.ListDirectory;
            //request.Credentials = credentials;

            //try
            //{
            //    var response = (FtpWebResponse)request.GetResponse();

            //    var codeParagraph = new Paragraph(new Run(response.StatusCode.ToString()))
            //    {
            //        Foreground = Brushes.Gold
            //    };
            //    rtb.Document.Blocks.Add(codeParagraph);

            //    string responseString;
            //    using (var sr = new StreamReader(response.GetResponseStream()))
            //    {
            //        responseString = sr.ReadToEnd();
            //    }

            //    var paragraph = new Paragraph(new Run(responseString))
            //    {
            //        Foreground = Brushes.Green
            //    };
            //    rtb.Document.Blocks.Add(paragraph);

            //}
            //catch (Exception ex)
            //{
            //    var paragraph = new Paragraph(new Run(ex.Message))
            //    {
            //        Foreground = Brushes.Red
            //    };
            //    rtb.Document.Blocks.Add(paragraph);
            //}
        }
    }
}