using System;
using System.ComponentModel;
using System.Windows;
using FtpFileController.Configs;
using FtpFileController.Extensions;

namespace FtpFileController.Windows {
    public partial class EditFileNameWindow : Window {

        private readonly FileSettings _fileSettings;
        
        public EditFileNameWindow(FileSettings fileSettings) {
            InitializeComponent();
            DataContext = this;
            
            _fileSettings = fileSettings;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            FileNameTextBox.Text = _fileSettings.FileName;
        }

        private void Window_Closing(object sender, CancelEventArgs e) {
            var newFileName = FileNameTextBox.Text.Trim();
            if (newFileName.Equals(_fileSettings.FileName, StringComparison.Ordinal)) {
                return;
            }

            var questionResult = MessageBox.Show($"Установить новое имя файла? [{newFileName}]",
                                                 "Имя файла было изменено:",
                                                 MessageBoxButton.YesNo,
                                                 MessageBoxImage.Question,
                                                 MessageBoxResult.No);
            if (questionResult == MessageBoxResult.No) {
                return;
            }

            _fileSettings.FileName = newFileName;
            _fileSettings.OverrideConfigurationFile("Configurations");
        }

        private void Window_Closed(object sender, EventArgs e) {
            FileNameTextBox.Text = "";
        }
    }
}