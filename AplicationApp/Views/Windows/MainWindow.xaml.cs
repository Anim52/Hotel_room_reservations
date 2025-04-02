using AplicationApp.Views.Pages;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AplicationApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void LoadCurrentUserId()
        {
           // string currentUserLogin = App.CurrentUserLogin;

           // Получаем пользователя из базы данных по логину
           //var user = _context.User.FirstOrDefault(u => u.Login == currentUserLogin);

           // if (user != null)
           // {
           //     _currentUserId = user.Id;
           // }
           // else
           // {
           //     MessageBox.Show("Пользователь не найден.");
           // }
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            //string currentUserLogin = App.CurrentUserLogin;

            //// Получаем пользователя из базы данных по логину
            //var user = _context.User.FirstOrDefault(u => u.Login == currentUserLogin);

            //if (user != null)
            //{
            //    // Получаем userId из найденного пользователя
            //    Guid userId = user.Id;

            //    // Передаем userId в конструктор AccauntPage
            //    ContentControlFrame.Content = new AccauntPage(userId);  // Вместо this.Content =
            //}
            //else
            //{
            //    MessageBox.Show("Пользователь не найден.");
            //}
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            ContentControlFrame.Content = new NumberPage();
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            ContentControlFrame.Content = new GuestPage();
        }

        

        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            ContentControlFrame.Content = new AdminServicePage();
        }

        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            ContentControlFrame.Content = new ReservationPage();
        }

        private void Close_btn_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}