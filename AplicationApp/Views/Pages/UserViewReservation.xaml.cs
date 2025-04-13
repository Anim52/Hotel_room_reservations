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
    /// Логика взаимодействия для UserViewReservation.xaml
    /// </summary>
    public partial class UserViewReservation : UserControl
    {
        private ReservationsViewModel viewModel;

        public UserViewReservation()
        {
            InitializeComponent();

            // Создаем экземпляр ViewModel
            viewModel = new ReservationsViewModel();
            this.DataContext = viewModel;

            // Подписка на события
            viewModel.OnError += ViewModel_OnError;
            viewModel.OnSuccess += ViewModel_OnSuccess;
            viewModel.OnReservationAdded += ViewModel_OnReservationAdded;
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

        // Обработчик события добавления бронирования
        private void ViewModel_OnReservationAdded(Reservations reservation)
        {
            // Например, уведомляем пользователя о новой броне
            MessageBox.Show($"Ваша бронь на номер {reservation.Nomer.Number} успешно создана. Дата прибытия: {reservation.ArrivalDate}",
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
