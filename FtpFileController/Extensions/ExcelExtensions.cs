using Microsoft.Office.Interop.Excel;

namespace FtpFileController.Extensions {
    internal static class ExcelExtensions {
        internal static void OpenBook(string filePath) {
            Application excel = new Application();
            excel.Visible = true;
            excel.Workbooks.Open(filePath);
        }
    }
}