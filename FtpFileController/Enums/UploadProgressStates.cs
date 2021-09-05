using System.ComponentModel;

namespace FtpFileController.Enums {
    public enum UploadProgressStates {
        [Description("Получение файлового потока")]
        StreamReceiving = 0,
        
        [Description("Загрузка файла")]
        Downloading = 1,
     
        [Description("Файл загружен")]
        Downloaded = 2
    }
}