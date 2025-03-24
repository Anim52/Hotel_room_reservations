using Application.Views;
using Data_Management_Service.ViewsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Application
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        private readonly AuthViewModel _viewModel;

        public AuthWindow()
        {
            InitializeComponent();
            _viewModel = new AuthViewModel();
            this.DataContext = _viewModel;

            // Подписываемся на события
            _viewModel.OnError += ShowError;
            _viewModel.OnSuccess += ShowSuccess;
            _viewModel.OpenAdminWindow += OpenAdminWindow;
            _viewModel.OpenUserWindow += OpenUserWindow;
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowSuccess(string message)
        {
            MessageBox.Show(message, "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OpenAdminWindow()
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void OpenUserWindow()
        {
            var userWindow = new UserMainWindow();
            userWindow.Show();
            this.Close();
        }
    }
}
