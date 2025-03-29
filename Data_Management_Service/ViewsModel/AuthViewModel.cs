using Data_Management_Service.Command;
using Data_Management_Service.OtherViews;
using Database.Context;
using Database.Service;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Data_Management_Service.ViewsModel
{
    public class AuthViewModel : BaseViewModel
    {
        private readonly SqlServerContext _context;
        private readonly AuthService _authService;

        public string Login { get; set; }
        public string Password { get; set; }

        public ICommand LoginCommand { get; }

        // События для уведомления View
        public event Action<string> OnError;
        public event Action<string> OnSuccess;
        public event Action OpenAdminWindow;
        public event Action OpenUserWindow;
        public event Action<Guid, string> OnLoginSuccess;


        public AuthViewModel()
        {
            _context = new SqlServerContext();
            _authService = new AuthService();
            LoginCommand = new RelayCommand(LoginExecute);
        }

        private void LoginExecute(object obj)
        {
            if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password))
            {
                OnError?.Invoke("Ошибка! Логин и пароль не могут быть пустыми.");
                return;
            }

            if (!_authService.Login(Login, Password, out User user))
            {
                OnError?.Invoke("Ошибка! Неверный логин или пароль.");
                return;
            }

            // Сохраняем логин и ID пользователя
            SessionService.CurrentUserLogin = user.Login;
            SessionService.CurrentUserId = user.Id;

            // Получаем текущего пользователя из БД
            var currentUser = _context.User.FirstOrDefault(u => u.Login == SessionService.CurrentUserLogin);
            if (currentUser == null)
            {
                OnError?.Invoke("Ошибка! Не удалось найти данные пользователя.");
                return;
            }

            // Ищем гостя по ID или создаем нового
            var guest = _context.Guests.FirstOrDefault(g => g.Id == currentUser.Id);
            if (guest == null)
            {
                guest = new Guests
                {
                    Id = currentUser.Id,
                    FirstName = currentUser.Firstname,
                    MiddleName = currentUser.Middlename,
                    LastName = currentUser.Lastname,
                    DateOfBirth = DateTime.MinValue,
                    PassportNumber = 0,
                    ContactDetails = string.Empty,
                    RegistrationDate = DateTime.Now,
                    Preferences = string.Empty
                };
                _context.Guests.Add(guest);
            }
            else
            {
                guest.FirstName = currentUser.Firstname;
                guest.MiddleName = currentUser.Middlename;
                guest.LastName = currentUser.Lastname;
                _context.Guests.Update(guest);
            }

            _context.SaveChanges();

            // Проверяем роль пользователя и уведомляем View об открытии соответствующего окна
            if (currentUser.Role == "Admin")
            {
                OnSuccess?.Invoke("Добро пожаловать, Админ!");
                OnLoginSuccess?.Invoke(currentUser.Id, currentUser.Login);
            }
            else if (currentUser.Role == "User")
            {
                OnSuccess?.Invoke("Добро пожаловать, Пользователь!");
                OnLoginSuccess?.Invoke(currentUser.Id, currentUser.Login);
            }
            else
            {
                OnError?.Invoke("Ошибка! Неизвестная роль пользователя.");
            }
        }
    }
}
