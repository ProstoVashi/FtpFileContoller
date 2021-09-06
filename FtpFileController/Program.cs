using System;
using Autofac;
using FtpFileController.IoC;

namespace FtpFileController {
    static class Program {
        [STAThread]
        static void Main() {
            using (ILifetimeScope scope = AutofacKernel.Container.BeginLifetimeScope()) {
                var app = scope.Resolve<App>();
                var mainWindow = scope.Resolve<MainWindow>();

                app.Run(mainWindow);
            }
        }
    }
}