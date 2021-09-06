using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using FtpFileController.Configs;
using FtpFileController.Enums;
using FtpFileController.Extensions;

namespace FtpFileController.Servicies.Ftp {
    public class DownloadFtpService : BaseFtpService<DownloadProgressStates> {
        
        /// <summary>
        /// Разрешено ли выполнять операцию
        /// </summary>
        internal override bool IsInvokeAllowed => !Worker.IsBusy && !TempFileExists();
        
        public DownloadFtpService(ClientService clientService, FileSettings fileSettings) : base(clientService, fileSettings) {
            FileExtensions.CreateTempDirectory(FileSettings.TempDirectory);
        }

        /// <summary>
        /// Обработчик загрузки файла 
        /// </summary>
        protected override void DoWorkHandled(object sender, DoWorkEventArgs eventArgs) {
            var tempFileName = FileSettings.GetTempFileName(WithMacros);
            Worker.ReportProgress(0);
                
            var response = GetResponse((FtpWebRequest) eventArgs.Argument).GetAwaiter().GetResult();
            Worker.ReportProgress(1);
                
            WriteFileFromStream(response, tempFileName).GetAwaiter().GetResult();
            Worker.ReportProgress(2);
                
            ExcelExtensions.OpenBook(tempFileName);
        }

        /// <summary>
        /// Возвращает объект запроса на загрузку файла 
        /// </summary>
        protected override FtpWebRequest GetRequest() =>
            ClientService.GetRequest(WebRequestMethods.Ftp.DownloadFile,
                                      FileSettings.FileName.ExcelFileName(WithMacros));
        
        /// <summary>
        /// Пишет файл из потока ответа 
        /// </summary>
        private async Task WriteFileFromStream(FtpWebResponse response, string tempFileName) {
            await using (var responseStream = response.GetResponseStream()) {
                await using (var fs = new FileStream(tempFileName, FileMode.Create)) {
                    byte[] buffer = new byte[64];
                    int size = 0;

                    while ((size = await responseStream.ReadAsync(buffer, 0, buffer.Length)) > 0) {
                        await fs.WriteAsync(buffer.AsMemory(0, size));
                    }
                }
            }
        }
    }
}