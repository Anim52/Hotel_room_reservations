using Data_Management_Service.PageViewModel;
using Database.Service;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AplicationApp.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для AccauntPage.xaml
    /// </summary>
    public partial class AccauntPage : UserControl
    {

        public AccauntPage(Guid userId)
        {
            InitializeComponent();
            AccountViewModel accountViewModel = new AccountViewModel(userId);
            this.DataContext = accountViewModel;
            
            accountViewModel.OnError += ViewModel_OnError;
            accountViewModel.OnSuccess += ViewModel_OnSuccess;
            
        }

        // Обработчик события ошибки
        private void ViewModel_OnError(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        // Обработчик события успеха
        private void ViewModel_OnSuccess(string message)
        {
            MessageBox.Show(message, "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

       
    }
}
