using AplicationApp.Views.Windows;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AplicationApp.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для NumberPage.xaml
    /// </summary>
    public partial class NumberPage : UserControl
    {
        public NumberPage()
        {
            InitializeComponent();
        }
        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            //MainGrid.Effect = new BlurEffect { Radius = 10 };

            //// Показываем затемнитель
            //Overlay.Visibility = Visibility.Visible;

            //// Открываем окно добавления номера
            //var addNomerWindow = new NumberAdd();
            //addNomerWindow.Owner = Window.GetWindow(this);
            //addNomerWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            //addNomerWindow.ShowDialog();

            //// Убираем размытие и затемнитель после закрытия окна
            //MainGrid.Effect = null;
            //Overlay.Visibility = Visibility.Collapsed;
        }


        private Nomer _selectedNomer;
        public Nomer SelectedNomer
        {
            get => _selectedNomer;
            set
            {
                _selectedNomer = value;
            }
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {

            if (SelectedNomer == null)
            {
                MessageBox.Show("Выберите номер для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MainGrid.Effect = new BlurEffect { Radius = 10 };

            // Показываем затемнитель
            Overlay.Visibility = Visibility.Visible;

            // Открываем окно редактирования номера с передачей SelectedNomer
            var editNomerWindow = new EditNomerView();
            editNomerWindow.Owner = Window.GetWindow(this);
            editNomerWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            editNomerWindow.ShowDialog();

            // Убираем размытие и затемнитель после закрытия окна
            MainGrid.Effect = null;
            Overlay.Visibility = Visibility.Collapsed;
        }
    }
}
