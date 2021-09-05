using System.Windows;
using System.Windows.Input;
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

            _ftpService.onProgressChanged += newStatus => StatusTextBlock.Text = newStatus;
        }
    }
}