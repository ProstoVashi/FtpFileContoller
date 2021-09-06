using System.Windows;
using System.Windows.Input;
using FtpFileController.Commands;
using FtpFileController.Middlewares;
using FtpFileController.Servicies;

namespace FtpFileController {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        
        public ICommand DownloadFileCommand { get;  }
        public ICommand UploadFileCommand { get;  }
        
        public ICommand OpenFileNameWindowCommand { get; }
        public ICommand DeleteTempFilesCommand { get; }

        public MainWindow(FtpService ftpService, MenuMiddleware menuMiddleware) {
            InitializeComponent();
            DataContext = this;

            DownloadFileCommand = new DefaultCommand(ftpService.DownloadFile, _ => !ftpService.InProgress && !ftpService.TempFileExists());
            UploadFileCommand = new DefaultCommand(ftpService.UploadFile, _ => !ftpService.InProgress && ftpService.TempFileExists());
            OpenFileNameWindowCommand = new DefaultCommand(menuMiddleware.OpenEditFileNameWindowCommand);
            DeleteTempFilesCommand = new DefaultCommand(menuMiddleware.DeleteTempFiles);

            ftpService.onProgressChanged += newStatus => StatusTextBlock.Text = newStatus;
        }
    }
}