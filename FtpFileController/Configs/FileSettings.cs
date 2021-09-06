using System;

namespace FtpFileController.Configs {
    public class FileSettings {
        public string FileName { get; set; }

        public string TempDirectory => $"C:\\Users\\{Environment.UserName}\\Documents\\FtpFileController\\Temp";
    }
}