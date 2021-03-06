using Autofac;
using FtpFileController.Configs;
using FtpFileController.Enums;
using FtpFileController.Middlewares;
using FtpFileController.Servicies;
using FtpFileController.Servicies.Ftp;
using FtpFileController.Windows;
using ConfigReader = FtpFileController.Extensions.ConfigurationsExtensions;

namespace FtpFileController.IoC {
    internal static class AutofacKernel {
        internal static readonly IContainer Container;
        
        static AutofacKernel() {
            var builder = new ContainerBuilder();

            SetUpConfigurationFiles(builder, "Configurations");

            builder.RegisterType<ClientService>().AsSelf().SingleInstance();
            builder.RegisterType<DownloadFtpService>().As<BaseFtpService<DownloadProgressStates>>().SingleInstance();
            builder.RegisterType<UploadFtpService>().As<BaseFtpService<UploadProgressStates>>().SingleInstance();
            
            builder.RegisterType<MenuMiddleware>().AsSelf().SingleInstance();
            builder.RegisterType<EditFileNameWindow>();
            
            builder.RegisterType<MainWindow>().AsSelf().SingleInstance();
            
            builder.RegisterType<App>().AsSelf().SingleInstance();

            Container = builder.Build();
        }
        private static void SetUpConfigurationFiles(ContainerBuilder builder, string directory) {
            builder.RegisterInstance(ConfigReader.GetConfiguration<ConnectionSettings>(directory)).SingleInstance();
            builder.RegisterInstance(ConfigReader.GetConfiguration<FileSettings>(directory)).SingleInstance();
            builder.RegisterInstance(ConfigReader.GetConfiguration<UserCredentials>(directory)).SingleInstance();
        }
    }
}