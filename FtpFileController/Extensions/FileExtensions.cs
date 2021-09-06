using System.IO;
using FtpFileController.Configs;

namespace FtpFileController.Extensions {
    internal static class FileExtensions {
        internal static void CreateTempDirectory(string tempDirectory) {
            var di = new DirectoryInfo(tempDirectory);
            if (!di.Exists) {
                di.Create();
            }
        }

        internal static string ExcelFileName(this string fileName, bool withMacros) {
            return $"{fileName}.xls{(withMacros ? 'm' : 'x')}";
        }

        internal static string GetTempFileName(this FileSettings fileSettings, bool withMacros) {
            return Path.Combine(fileSettings.TempDirectory, ExcelFileName(fileSettings.FileName, withMacros));
        }
    }
}