using Data_Management_Service.Command;
using Database.Context;
using Database.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using Data_Management_Service.Inerfaces;



namespace Data_Management_Service.ViewsModel
{
    /// <summary>
    /// ViewModel для добавления, изменения и удаления номеров.
    /// </summary>
    public class AddNomerViewModel : BaseViewModel
    {
        // <summary>
        /// Контекст базы данных.
        /// </summary>
        private readonly SqlServerContext _context;
        /// <summary>
        /// Сервис выбора файлов (изображений).
        /// </summary>
        private readonly IFileDialogService _fileDialogService;

        // Поля для ввода данных
        private int _number;
        private int _floor;
        private decimal _cost;
        private string _description;
        private TypeNumder _selectedTypeNumder;
        private string _imagePath;

        // Коллекция номеров
        private ObservableCollection<Nomer> _nomerList;
        private Nomer _selectedNomer;

        public Action<string> OnError { get; set; }
        public Action<string> OnSuccess { get; set; }
        public Action<string, Action> OnConfirm { get; set; }

        // Конструктор
        public AddNomerViewModel(IFileDialogService fileDialogService)
        {
            _context = new SqlServerContext();
            _fileDialogService = fileDialogService;
            AddNomerCommand = new RelayCommand(AddNomer);
            DeleteNomerCommand = new RelayCommand(DeleteNomer);
            EditNomerCommand = new RelayCommand(EditNomer);
            SelectImageCommand = new RelayCommand(SelectImage); // Добавили команду выбора изображения

            TypeNumderList = new ObservableCollection<TypeNumder>
            {
                TypeNumder.Standart,
                TypeNumder.Studio,
                TypeNumder.Suite,
                TypeNumder.Apartment
            };

            // Загружаем список номеров из базы
            NomerList = new ObservableCollection<Nomer>(_context.Nomers.ToList());
        }

        // Свойства для привязки данных
        public int Number
        {
            get => _number;
            set
            {
                _number = value;
                OnPropertyChanged(nameof(Number));
            }
        }

        public int Floor
        {
            get => _floor;
            set
            {
                _floor = value;
                OnPropertyChanged(nameof(Floor));
            }
        }

        public decimal Cost
        {
            get => _cost;
            set
            {
                _cost = value;
                OnPropertyChanged(nameof(Cost));
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public TypeNumder SelectedTypeNumder
        {
            get => _selectedTypeNumder;
            set
            {
                _selectedTypeNumder = value;
                OnPropertyChanged(nameof(SelectedTypeNumder));
            }
        }

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged(nameof(ImagePath));
            }
        }
        /// <summary>
        /// Коллекция типов номеров.
        /// </summary>
        public ObservableCollection<TypeNumder> TypeNumderList { get; set; }

        public ObservableCollection<Nomer> NomerList
        {
            get => _nomerList;
            set
            {
                _nomerList = value;
                OnPropertyChanged(nameof(NomerList));
            }
        }
        /// <summary>
        /// Выбранный номер для редактирования.
        /// </summary>
        public Nomer SelectedNomer
        {
            get => _selectedNomer;
            set
            {
                _selectedNomer = value;
                OnPropertyChanged(nameof(SelectedNomer));

                // Если выбран номер — подставляем его значения в поля
                if (_selectedNomer != null)
                {
                    Number = _selectedNomer.Number;
                    Floor = _selectedNomer.Floor;
                    Cost = _selectedNomer.Cost;
                    Description = _selectedNomer.Description;
                    SelectedTypeNumder = _selectedNomer.TypeNumder;
                    ImagePath = _selectedNomer.ImagePath;
                }
            }
        }



        /// <summary>
        /// Команда добавления нового номера.
        /// </summary>
        public ICommand AddNomerCommand { get; }
        /// <summary>
        /// Команда удаления номера.
        /// </summary>
        public ICommand DeleteNomerCommand { get; }
        /// <summary>
        /// Команда редактирования номера.
        /// </summary>
        public ICommand EditNomerCommand { get; }
        /// <summary>
        /// Команда выбора изображения.
        /// </summary>
        public ICommand SelectImageCommand { get; }

        /// <summary>
        /// Выбор изображения с помощью диалогового окна.
        /// </summary>
        private void SelectImage(object obj)
        {
            ImagePath = _fileDialogService.OpenFileDialog();
            OnPropertyChanged(nameof(ImagePath));
        }

        /// <summary>
        /// Добавление нового номера в базу данных.
        /// </summary>
        private void AddNomer(object obj)
        {
            if (Number <= 0 || Floor <= 0 || Cost <= 0 || string.IsNullOrWhiteSpace(Description))
            {
                OnError?.Invoke("Все поля должны быть заполнены корректно.");
                return;
            }

            var newNomer = new Nomer
            {
                Id = Guid.NewGuid(),
                Number = Number,
                Floor = Floor,
                Status = true,
                Cost = Cost,
                Description = Description,
                TypeNumder = SelectedTypeNumder,
                ImagePath = ImagePath 
            };

            _context.Nomers.Add(newNomer);
            _context.SaveChanges();

            OnSuccess?.Invoke("Номер успешно добавлен!");

            NomerList.Add(newNomer);

            // Очищаем поля
            Number = 0;
            Floor = 0;
            Cost = 0;
            Description = string.Empty;
            SelectedTypeNumder = TypeNumder.Standart;
            ImagePath = null;
        }

        /// <summary>
        /// Редактирование существующего номера.
        /// </summary>
        private void EditNomer(object obj)
        {
            if (SelectedNomer == null)
            {
                OnError?.Invoke("Пожалуйста, выберите номер для изменения.");
                return;
            }

            var nomerToEdit = _context.Nomers.FirstOrDefault(n => n.Id == SelectedNomer.Id);
            if (nomerToEdit != null)
            {
                nomerToEdit.Number = Number;
                nomerToEdit.Floor = Floor;
                nomerToEdit.Cost = Cost;
                nomerToEdit.Description = Description;
                nomerToEdit.TypeNumder = SelectedTypeNumder;
                nomerToEdit.ImagePath = ImagePath;

                _context.SaveChanges();

                // ✅ Обновляем только измененный объект в коллекции
                var index = NomerList.IndexOf(SelectedNomer);
                if (index >= 0)
                {
                    NomerList[index] = nomerToEdit;
                    OnPropertyChanged(nameof(NomerList));
                }

                OnSuccess?.Invoke("Изменения успешно сохранены!");
            }

        }


            private bool CanEditNomer(object obj)
            {
            return SelectedNomer != null;
            }

        /// <summary>
        /// Удаление выбранного номера.
        /// </summary>
        private void DeleteNomer(object obj)
        {
            if(SelectedNomer == null)
            {
                OnError?.Invoke("Пожалуйста, выберите номер для удаления.");
                return;
            }

            OnConfirm?.Invoke($"Вы уверены, что хотите удалить номер {SelectedNomer.Number}?", () =>
            {
                _context.Nomers.Remove(SelectedNomer);
                _context.SaveChanges();
                NomerList.Remove(SelectedNomer);
                OnSuccess?.Invoke("Номер успешно удален!");
                SelectedNomer = null;
            });
        }

        private bool CanDeleteNomer(object obj)
        {
            return SelectedNomer != null;
        }
    }
}
