using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using FtpFileController.Configs;
using FtpFileController.Enums;
using FtpFileController.Extensions;

namespace FtpFileController.Servicies.Ftp {
    public class UploadFtpService : BaseFtpService<UploadProgressStates> {
        
        /// <summary>
        /// Разрешено ли выполнять операцию
        /// </summary>
        internal override bool IsInvokeAllowed => !Worker.IsBusy && TempFileExists();
        
        public UploadFtpService(ClientService clientService, FileSettings fileSettings) : base(clientService, fileSettings) { }
        
        /// <summary>
        /// Обработчик отправки файла 
        /// </summary>
        protected override void DoWorkHandled(object sender, DoWorkEventArgs eventArgs) {
            var tempFileName = FileSettings.GetTempFileName(WithMacros);
            var request = (FtpWebRequest) eventArgs.Argument;
            Worker.ReportProgress(0);
                
            WriteFileToStream(request, tempFileName).GetAwaiter().GetResult();
            Worker.ReportProgress(1);
                
            GetResponse(request).GetAwaiter().GetResult();
            Worker.ReportProgress(2);
                
            File.Delete(tempFileName);
            Worker.ReportProgress(3);
        }

        /// <summary>
        /// Возвращает объект запроса на отправку файла 
        /// </summary>
        protected override FtpWebRequest GetRequest() =>
            ClientService.GetRequest(WebRequestMethods.Ftp.UploadFile,
                                     FileSettings.FileName.ExcelFileName(WithMacros));
        
        /// <summary>
        /// Пишет файл в поток запроса 
        /// </summary>
        private async Task WriteFileToStream(FtpWebRequest request, string tempFileName) {
            await using (var responseStream = request.GetRequestStream()) {
                await using (var fs = new FileStream(tempFileName, FileMode.Open)) {
                    await fs.CopyToAsync(responseStream);
                }
            }
        }
    }
}