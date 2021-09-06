using System.IO;
using Autofac;
using FtpFileController.Configs;
using FtpFileController.Windows;

namespace FtpFileController.Middlewares {
    public class MenuMiddleware {
        private readonly DirectoryInfo _tempFolderInfo;
        private readonly ILifetimeScope _scope;

        public MenuMiddleware(ILifetimeScope scope, FileSettings fileSettings) {
            _scope = scope;
            _tempFolderInfo = new DirectoryInfo(fileSettings.TempDirectory);
        }

        internal void OpenEditFileNameWindowCommand(object args) {
            _scope.Resolve<EditFileNameWindow>().ShowDialog();
        }

        internal void DeleteTempFiles(object args) {
            foreach (var fileInfo in _tempFolderInfo.GetFiles()) {
                fileInfo.Delete();
            }
        }
        
    }
}