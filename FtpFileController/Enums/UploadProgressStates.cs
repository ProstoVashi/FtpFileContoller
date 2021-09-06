using System.ComponentModel;

namespace FtpFileController.Enums {
    public enum UploadProgressStates {
        [Description("Упаковка файла")]
        FileBoxing = 0,

        [Description("Отправка файла")]
        Uploading = 1,

        [Description("Файл выгружен")]
        Uploaded = 2,

        [Description("Временный файл удален")]
        TempRemoved = 3
    }
}