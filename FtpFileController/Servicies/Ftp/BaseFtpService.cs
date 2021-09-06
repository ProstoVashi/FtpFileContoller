using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FtpFileController.Configs;
using FtpFileController.Extensions;

namespace FtpFileController.Servicies.Ftp {
    public abstract class BaseFtpService<T> where T : struct, Enum {
        private static readonly string[] ProgressMessages = Enum.GetValues<T>().Select(e => e.GetDescription()).ToArray();

        protected static readonly BackgroundWorker Worker = new() {
            WorkerReportsProgress = true
        };
        
        internal event Action<string> ProgressChanged;
        
        /// <summary>
        /// Разрешено ли выполнять операцию
        /// </summary>
        internal abstract bool IsInvokeAllowed { get; }
        
        protected readonly bool WithMacros = true;
        protected readonly FileSettings FileSettings;
        protected readonly ClientService ClientService;
        
        protected BaseFtpService(ClientService clientService, FileSettings fileSettings) {
            ClientService = clientService;
            FileSettings = fileSettings;
        }

        internal void Execute(object args) {
            Worker.DoWork += DoWorkHandled;
            Worker.ProgressChanged += ProgressChangedHandled;
            Worker.RunWorkerCompleted += RunWorkerCompletedHandled;
            Worker.RunWorkerAsync(GetRequest());
        }

#region Background worker

        protected abstract void DoWorkHandled(object sender, DoWorkEventArgs eventArgs);
        
        private void ProgressChangedHandled(object sender, ProgressChangedEventArgs eventArgs) {
            ProgressChanged?.Invoke(ProgressMessages[eventArgs.ProgressPercentage]);
        }

        private void RunWorkerCompletedHandled(object sender, RunWorkerCompletedEventArgs eventArgs) {
            Worker.DoWork -= DoWorkHandled;
            Worker.ProgressChanged -= ProgressChangedHandled;
            Worker.RunWorkerCompleted -= RunWorkerCompletedHandled;
        }

#endregion
        
        /// <summary>
        /// Возвращает FTP-запрос
        /// </summary>
        protected abstract FtpWebRequest GetRequest();
        
        /// <summary>
        /// Получает ответ на FTP-запрос
        /// </summary>
        protected async Task<FtpWebResponse> GetResponse(FtpWebRequest request) {
            var webResponse = await request.GetResponseAsync();
            return (FtpWebResponse) webResponse;
        }

#region Utils

        /// <summary>
        /// Проверяет, существует ли временный файл
        /// </summary>
        protected bool TempFileExists() => File.Exists(FileSettings.GetTempFileName(WithMacros));

#endregion
    }
}