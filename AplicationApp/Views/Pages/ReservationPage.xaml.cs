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
    /// Логика взаимодействия для ReservationPage.xaml
    /// </summary>
    public partial class ReservationPage : UserControl
    {
        private ReservationsViewModel viewModel;
        public ReservationPage()
        {
            InitializeComponent();
             viewModel = new ReservationsViewModel();
            this.DataContext = viewModel;
            viewModel.OnError += ViewModel_OnError;
            viewModel.OnSuccess += ViewModel_OnSuccess;
            viewModel.OnReservationAdded += ViewModel_OnReservationAdded;
        }
        private void ViewModel_OnError(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        // Обработчик события успеха
        private void ViewModel_OnSuccess(string message)
        {
            MessageBox.Show(message, "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Обработчик события добавления бронирования
        private void ViewModel_OnReservationAdded(Reservations reservation)
        {
            // Например, уведомляем администратора о новой броне
            MessageBox.Show($"Новая бронь: Номер {reservation.Nomer.Number}, Дата прибытия: {reservation.ArrivalDate}",
                            "Новая бронь", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Не забываем отписаться от событий, когда окно закрывается или больше не нужно слушать события
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            // Отписываемся от событий
            viewModel.OnError -= ViewModel_OnError;
            viewModel.OnSuccess -= ViewModel_OnSuccess;
            viewModel.OnReservationAdded -= ViewModel_OnReservationAdded;
        }
    }
}
