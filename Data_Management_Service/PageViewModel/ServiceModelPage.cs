using Data_Management_Service.Command;
using Data_Management_Service.ViewsModel;
using Database.Context;
using Database.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Data_Management_Service.PageViewModel
{
    /// <summary>
    /// ViewModel для управления заявками на услуги (Service).
    /// </summary>
    public class ServiceModelPage : BaseViewModel
    {
        /// <summary>
        /// Контекст базы данных.
        /// </summary>
        private readonly SqlServerContext _context;
        /// <summary>
        /// Выбранная заявка.
        /// </summary>
        private Services _selectedService;
        /// <summary>
        /// Описание новой заявки.
        /// </summary>
        private string _newRequestDescription;
        /// <summary>
        /// Признак, является ли пользователь администратором.
        /// </summary>
        private bool _isAdmin;
        /// <summary>
        /// ID текущего пользователя.
        /// </summary>
        private Guid _currentUserId;

        /// <summary>
        /// Текущий пользователь (для отображения ФИО).
        /// </summary>
        private User _currentUser;

        // Действия для отображения сообщений
        public Action<string> OnError { get; set; }
        public Action<string> OnSuccess { get; set; }
        public Action<string, Action> OnConfirm { get; set; }

        // Конструктор
        public ServiceModelPage(Guid userId, bool isAdmin)
        {
            _context = new SqlServerContext();
            ServiceRequests = new ObservableCollection<Services>();
            _isAdmin = isAdmin;
            _currentUserId = userId;

            // Загружаем данные
            LoadServiceRequests();

            // Получаем текущего пользователя для отображения ФИО
            _currentUser = _context.User.FirstOrDefault(u => u.Id == _currentUserId);

            // Команды
            CreateRequestCommand = new RelayCommand(CreateRequest);
            CompleteRequestCommand = new RelayCommand(CompleteRequest);
            DeleteRequestCommand = new RelayCommand(DeleteRequest);
        }

        // Свойство для новых заявок
        public string NewRequestDescription
        {
            get => _newRequestDescription;
            set
            {
                _newRequestDescription = value;
                OnPropertyChanged(nameof(NewRequestDescription));
            }
        }

        // Свойство для выбранной заявки
        public Services SelectedService
        {
            get => _selectedService;
            set
            {
                _selectedService = value;
                OnPropertyChanged(nameof(SelectedService));
            }
        }

        /// <summary>
        /// Коллекция заявок.
        /// </summary>
        public ObservableCollection<Services> ServiceRequests { get; set; }

        // Новый проперт для отображения ФИО с инициалами
        public string Fullname => _currentUser != null
            ? $"{_currentUser.Lastname} {_currentUser.Firstname[0]}. {_currentUser.Middlename[0]}."
            : string.Empty;

        /// <summary>
        /// Команда создания новой заявки.
        /// </summary>
        public ICommand CreateRequestCommand { get; }
        /// <summary>
        /// Команда завершения заявки.
        /// </summary>       
        public ICommand CompleteRequestCommand { get; }
        /// <summary>
        /// Команда удаления заявки.
        /// </summary>
        public ICommand DeleteRequestCommand { get; }

        /// <summary>
        /// Загрузка заявок из базы данных.
        /// </summary>
        private void LoadServiceRequests()
        {
            ServiceRequests.Clear();

            // Загружаем все заявки и связанные с ними пользователи
            var requests = _context.Services
                                  .Include(s => s.User) // Подгружаем пользователя вместе с заявкой
                                  .ToList();

            foreach (var request in requests)
            {
                // Добавляем заявку только если у нее есть связанный пользователь
                if (request.User != null)
                {
                    ServiceRequests.Add(request);
                }
            }
        }

        /// <summary>
        /// Создание новой заявки.
        /// </summary>
        private void CreateRequest(object obj)
        {
            if (string.IsNullOrWhiteSpace(NewRequestDescription))
            {
                OnError?.Invoke("Описание не может быть пустым.");
                return;
            }

            var newRequest = new Services
            {
                Id = Guid.NewGuid(),
                UserId = _currentUserId, // Привязываем заявку к текущему пользователю
                Description = NewRequestDescription,
                RequestDate = DateTime.Now,
                Status = "В обработке"
            };

            _context.Services.Add(newRequest);
            _context.SaveChanges();

            ServiceRequests.Add(newRequest);
            NewRequestDescription = string.Empty;

            OnSuccess?.Invoke("Заявка успешно создана!");
        }

        /// <summary>
        /// Завершение заявки (только админ).
        /// </summary>
        private void CompleteRequest(object obj)
        {
            if (SelectedService != null && _isAdmin)
            {
                SelectedService.Status = "Выполнено";
                _context.Services.Update(SelectedService);
                _context.SaveChanges();
                LoadServiceRequests();
                OnSuccess?.Invoke("Заявка помечена как выполненная.");
            }
            else
            {
                OnError?.Invoke("Вы должны быть администратором для выполнения этой операции.");
            }
        }

        /// <summary>
        /// Удаление заявки (только админ).
        /// </summary>
        private void DeleteRequest(object obj)
        {
            if (SelectedService != null && _isAdmin)
            {
                OnConfirm?.Invoke($"Вы уверены, что хотите удалить заявку {SelectedService.Description}?", () =>
                {
                    _context.Services.Remove(SelectedService);
                    _context.SaveChanges();
                    LoadServiceRequests();
                    OnSuccess?.Invoke("Заявка успешно удалена!");
                });
            }
            else
            {
                OnError?.Invoke("Вы должны быть администратором для выполнения этой операции.");
            }
        }

        /// <summary>
        /// Проверка возможности редактирования заявки.
        /// </summary>
        private bool CanModifyRequest(object obj)
        {
            return SelectedService != null && _isAdmin;
        }
    }

}
