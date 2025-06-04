using Data_Management_Service.Command;
using Data_Management_Service.ViewsModel;
using Database.Context;
using Database.Service;
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
    /// ViewModel для управления списком пользователей.
    /// </summary>

    public class UsersViewModel : BaseViewModel
    {
        /// <summary>
        /// Контекст базы данных.
        /// </summary>
        private readonly SqlServerContext _context;

        /// <summary>
        /// Коллекция всех пользователей.
        /// </summary>
        public ObservableCollection<Guests> UsersList { get; set; }

        /// <summary>
        /// Выбранный пользователь.
        /// </summary>
        private Guests _selectedUser;
        public Guests SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }

        /// <summary>
        /// Команда удаления пользователя.
        /// </summary>
        public ICommand DeleteUserCommand { get; }

        /// <summary>
        /// Событие отображения успеха.
        /// </summary>
        public Action<string> OnError { get; set; }
        /// <summary>
        /// Событие подтверждения действия.
        /// </summary>
        public Action<string> OnSuccess { get; set; }
        
        public Action<string, Action> OnConfirm { get; set; }

        // Конструктор
        public UsersViewModel()
        {
            _context = new SqlServerContext();

            // Загружаем список пользователей из базы данных
            UsersList = new ObservableCollection<Guests>(_context.Guests.ToList());

            // Инициализируем команду удаления
            DeleteUserCommand = new RelayCommand(DeleteUser);
        }

        /// <summary>
        /// Удаление пользователя и соответствующего объекта Guest.
        /// </summary>
        private async void DeleteUser(object obj)
        {
            if (SelectedUser == null)
            {
                OnError?.Invoke("Пожалуйста, выберите пользователя для удаления.");
                return;
            }

            // Используем OnConfirm для подтверждения
            OnConfirm?.Invoke($"Вы уверены, что хотите удалить пользователя: {SelectedUser.FirstName} {SelectedUser.LastName}?", () =>
            {
                try
                {
                    // Находим пользователя (User), связанного с этим гостем
                    var userToDelete = _context.User.FirstOrDefault(u => u.Id == SelectedUser.Id);

                    if (userToDelete != null)
                    {
                        _context.User.Remove(userToDelete);
                    }

                    // Удаляем гостя из базы данных
                    _context.Guests.Remove(SelectedUser);

                    // Асинхронно сохраняем изменения
                    _context.SaveChanges();

                    // Обновляем список
                    UsersList.Remove(SelectedUser);
                    OnSuccess?.Invoke("Пользователь успешно удален!");
                }
                catch (Exception ex)
                {
                    OnError?.Invoke($"Произошла ошибка при удалении пользователя: {ex.Message}");
                }
            });
        }
    }

}
