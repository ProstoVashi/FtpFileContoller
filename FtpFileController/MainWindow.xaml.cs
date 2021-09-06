using System;
using System.Windows;
using System.Windows.Input;
using FtpFileController.Commands;
using FtpFileController.Enums;
using FtpFileController.Middlewares;
using FtpFileController.Servicies;
using FtpFileController.Servicies.Ftp;

namespace FtpFileController {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        
        public ICommand DownloadFileCommand { get;  }
        public ICommand UploadFileCommand { get;  }
        
        public ICommand OpenFileNameWindowCommand { get; }
        public ICommand DeleteTempFilesCommand { get; }

        public MainWindow(BaseFtpService<DownloadProgressStates> downloadFtpService, BaseFtpService<UploadProgressStates> uploadFtpService, MenuMiddleware menuMiddleware) {
            InitializeComponent();
            DataContext = this;

            DownloadFileCommand = InitializeFtpServiceCommand(downloadFtpService);
            UploadFileCommand = InitializeFtpServiceCommand(uploadFtpService);
                
            OpenFileNameWindowCommand = new DefaultCommand(menuMiddleware.OpenEditFileNameWindowCommand);
            DeleteTempFilesCommand = new DefaultCommand(menuMiddleware.DeleteTempFiles);
        }

        private ICommand InitializeFtpServiceCommand<T>(BaseFtpService<T> service) where T : struct, Enum {
            service.ProgressChanged += newStatus => StatusTextBlock.Text = newStatus;
            return new DefaultCommand(service.Execute, _ => service.IsInvokeAllowed);;
        } 
    }
}