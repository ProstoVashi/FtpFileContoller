using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using FtpFileController.Configs;
using FtpFileController.Enums;
using FtpFileController.Extensions;
using Microsoft.Office.Interop.Excel;

namespace FtpFileController.Servicies {
    public class FtpService {

        internal event Action<string> onProgressChanged;
        
        internal bool InProgress => _worker.IsBusy;

        private readonly bool _withMacros = false;
        private readonly FileSettings _fileSettings;
        private readonly BackgroundWorker _worker;
        private readonly ClientService _clientService;
        
        public FtpService(ClientService clientService, FileSettings fileSettings) {
            _worker = new BackgroundWorker {
                WorkerReportsProgress = true
            };
            
            _clientService = clientService;
            _fileSettings = fileSettings;

            FileExtensions.CreateTempDirectory(_fileSettings.TempDirectory);
        }

        internal void DownloadFile(object args) {
            void DoWork(object sender, DoWorkEventArgs eventArgs) {
                var tempFileName = _fileSettings.GetTempFileName(_withMacros);
                _worker.ReportProgress(0);
                
                var response = GetResponse((FtpWebRequest) eventArgs.Argument).GetAwaiter().GetResult();
                _worker.ReportProgress(1);
                
                WriteFileFromStream(response, tempFileName).GetAwaiter().GetResult();
                _worker.ReportProgress(2);
                
                //ExcelExtensions.OpenBook(tempFileName);
                Application excel = new ApplicationClass();
                var wb = excel.Workbooks.Open(tempFileName);
            }

            void HandleProgress(object sender, ProgressChangedEventArgs eventArgs) {
                var state = (DownloadProgressStates) eventArgs.ProgressPercentage;
                onProgressChanged?.Invoke(state.GetDescription());
            }

            void HandleResult(object sender, RunWorkerCompletedEventArgs eventArgs) {
                _worker.DoWork -= DoWork;
                _worker.ProgressChanged -= HandleProgress;
                _worker.RunWorkerCompleted -= HandleResult;
            }

            _worker.DoWork += DoWork;
            _worker.ProgressChanged += HandleProgress;
            _worker.RunWorkerCompleted += HandleResult;
            _worker.RunWorkerAsync(_clientService.GetRequest(WebRequestMethods.Ftp.DownloadFile, _fileSettings.FileName.ExcelFileName(_withMacros)));
        }
        
        internal void UploadFile(object args) {
            
            void DoWork(object sender, DoWorkEventArgs eventArgs) {
                var tempFileName = _fileSettings.GetTempFileName(_withMacros);
                var request = (FtpWebRequest) eventArgs.Argument;
                _worker.ReportProgress(0);
                
                WriteFileToStream(request, tempFileName).GetAwaiter().GetResult();
                _worker.ReportProgress(1);
                
                var response = GetResponse(request).GetAwaiter().GetResult();
                _worker.ReportProgress(2);
                
                File.Delete(tempFileName);
                _worker.ReportProgress(3);
            }

            void HandleProgress(object sender, ProgressChangedEventArgs eventArgs) {
                var state = (UploadProgressStates) eventArgs.ProgressPercentage;
                onProgressChanged?.Invoke(state.GetDescription());
            }

            void HandleResult(object sender, RunWorkerCompletedEventArgs eventArgs) {
                _worker.DoWork -= DoWork;
                _worker.ProgressChanged -= HandleProgress;
                _worker.RunWorkerCompleted -= HandleResult;
            }

            _worker.DoWork += DoWork;
            _worker.ProgressChanged += HandleProgress;
            _worker.RunWorkerCompleted += HandleResult;
            _worker.RunWorkerAsync(_clientService.GetRequest(WebRequestMethods.Ftp.UploadFile, _fileSettings.FileName.ExcelFileName(_withMacros)));
        }

        internal bool TempFileExists() => File.Exists(_fileSettings.GetTempFileName(_withMacros));
        
        private async Task<FtpWebResponse> GetResponse(FtpWebRequest request) {
            var webResponse = await request.GetResponseAsync();
            return (FtpWebResponse) webResponse;
        }
        
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
        
        private async Task WriteFileToStream(FtpWebRequest request, string tempFileName) {
            await using (var responseStream = request.GetRequestStream()) {
                await using (var fs = new FileStream(tempFileName, FileMode.Open)) {
                    await fs.CopyToAsync(responseStream);
                }
            }
        }
    }
}