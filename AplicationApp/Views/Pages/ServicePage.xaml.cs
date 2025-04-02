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
    /// Логика взаимодействия для ServicePage.xaml
    /// </summary>
    public partial class ServicePage : UserControl
    {
        public ServicePage(Guid userId)
        {
            InitializeComponent();
            var serviceModelPage = new ServiceModelPage(userId, false);
            this.DataContext = serviceModelPage;

            // Пример для обработки ошибок:
            serviceModelPage.OnError = (message) => MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

            // Пример для успешных сообщений:
            serviceModelPage.OnSuccess = (message) => MessageBox.Show(message, "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            // Пример для подтверждений:
            serviceModelPage.OnConfirm = (message, action) =>
            {
                var result = MessageBox.Show(message, "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    action();
                }
            };

        }
    }
}
