using AplicationApp.Views;
using AplicationApp.Views.Pages;
using Database.Context;
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
using System.Windows.Shapes;

namespace AplicationApp
{
    /// <summary>
    /// Логика взаимодействия для UserMainWindow.xaml
    /// </summary>
    public partial class UserMainWindow : Window
    {
        private Guid _currentUserId;
        private string _currentUserLogin;
        private readonly SqlServerContext _context;

        public UserMainWindow(Guid userId, string login)
        {
            InitializeComponent();
            _currentUserId = userId;
            _currentUserLogin = login;
            _context = new SqlServerContext();
        }
        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            var user = _context.User.FirstOrDefault(u => u.Id == _currentUserId);

            if (user != null)
            {
                ContentControlFrame.Content = new AccauntPage(_currentUserId);
            }
            else
            {
                MessageBox.Show("Пользователь не найден.");
            }
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
            if (_currentUserId == Guid.Empty)
            {
                MessageBox.Show("Ошибка! UserId пустой.");
                return;
            }

            ContentControlFrame.Content = new ServicePage(_currentUserId);
        }

        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            ContentControlFrame.Content = new UserViewReservation();
        }
        private void Close_btn_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void UnLoginBtn_Click(object sender, RoutedEventArgs e)
        {
            AutorizationView autorizationView = new AutorizationView();
            autorizationView.Show();
            this.Close();

        }
    }
}
