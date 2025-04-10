using AplicationApp.Assets.Recources;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AplicationApp.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditNomerView.xaml
    /// </summary>
    public partial class EditNomerView : Window
    {
        public EditNomerView(Nomer SelectedNomer)
        {
            InitializeComponent();
            if (SelectedNomer != null)
            {
                var editNomerViewModel = new AddNomerViewModel(new FileDialogService())
                {
                    SelectedNomer = SelectedNomer,
                    Number = SelectedNomer.Number,
                    Floor = SelectedNomer.Floor,
                    Cost = SelectedNomer.Cost,
                    Description = SelectedNomer.Description,
                    SelectedTypeNumder = SelectedNomer.TypeNumder,
                    ImagePath = SelectedNomer.ImagePath
                };

                this.DataContext = editNomerViewModel;
            }
            else
            {
                MessageBox.Show("Выберите номер для редактирования.");
            }
        }
    }
}
