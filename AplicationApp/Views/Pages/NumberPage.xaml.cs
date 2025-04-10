using AplicationApp.Assets.Recources;
using AplicationApp.Views.Windows;
using Data_Management_Service.Inerfaces;
using Data_Management_Service.ViewsModel;
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
        AddNomerViewModel viewModel;
        public NumberPage()
        {
            InitializeComponent();
             viewModel = new AddNomerViewModel(new FileDialogService())
            {
                OnError = message => MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error),
                OnSuccess = message => MessageBox.Show(message, "Успех", MessageBoxButton.OK, MessageBoxImage.Information),
                OnConfirm = (message, action) =>
                {
                    var result = MessageBox.Show(message, "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        action.Invoke(); // Выполняем действие после подтверждения
                    }
                }
            };

            DataContext = viewModel;
        }
        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Effect = new BlurEffect { Radius = 10 };

            // Показываем затемнитель
            Overlay.Visibility = Visibility.Visible;

            // Открываем окно добавления номера
            var addNomerWindow = new NumberAdd();
            addNomerWindow.Owner = Window.GetWindow(this);
            addNomerWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            addNomerWindow.ShowDialog();

            // Убираем размытие и затемнитель после закрытия окна
            MainGrid.Effect = null;
            Overlay.Visibility = Visibility.Collapsed;
        }


      

        private void btn2_Click(object sender, RoutedEventArgs e)
        {

            if (viewModel.SelectedNomer == null)
            {
                MessageBox.Show("Выберите номер для редактирования: " + (viewModel.SelectedNomer == null ? "null" : "не null"),
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MainGrid.Effect = new BlurEffect { Radius = 10 };

            // Показываем затемнитель
            Overlay.Visibility = Visibility.Visible;

            // Открываем окно редактирования номера с передачей SelectedNomer
            var editNomerWindow = new EditNomerView(viewModel.SelectedNomer);
            editNomerWindow.Owner = Window.GetWindow(this);
            editNomerWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            editNomerWindow.ShowDialog();

            // Убираем размытие и затемнитель после закрытия окна
            MainGrid.Effect = null;
            Overlay.Visibility = Visibility.Collapsed;
        }
    }
}
