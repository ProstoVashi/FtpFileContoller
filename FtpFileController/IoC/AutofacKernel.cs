using Autofac;
using FtpFileController.Configs;
using FtpFileController.Servicies;
using ConfigReader = FtpFileController.Extensions.ConfigurationsExtensions;

namespace FtpFileController.IoC {
    internal static class AutofacKernel {
        internal static readonly IContainer Container;
        
        static AutofacKernel() {
            var builder = new ContainerBuilder();

            SetUpConfigurationFiles(builder, "Configurations");

            builder.RegisterType<ClientService>().AsSelf().SingleInstance();
            builder.RegisterType<FtpService>().AsSelf().SingleInstance();
            
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