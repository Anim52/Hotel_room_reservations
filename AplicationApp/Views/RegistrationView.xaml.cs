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

namespace AplicationApp.Views
{
    /// <summary>
    /// Логика взаимодействия для RegistrationView.xaml
    /// </summary>
    public partial class RegistrationView : Window
    {
        private readonly RegisterViewModel _viewModel;
        public RegistrationView()
        {
            InitializeComponent();
            _viewModel = new RegisterViewModel();
            DataContext = _viewModel;

            // Подписка на события
            _viewModel.OnError += ShowError;
            _viewModel.OnSuccess += ShowSuccess;
            _viewModel.CloseRegistrationWindow += CloseRegistrationWindow;
        }
        private void ShowError(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        // Отображение сообщения об успехе
        private void ShowSuccess(string message)
        {
            MessageBox.Show(message, "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Закрытие окна
        private void CloseRegistrationWindow()
        {
            this.Close();
        }
        private void Login_Btn(object sender, RoutedEventArgs e)
        {
            AutorizationView autorizationView = new AutorizationView();
            autorizationView.Show();
            this.Close();
        }
    }
}
