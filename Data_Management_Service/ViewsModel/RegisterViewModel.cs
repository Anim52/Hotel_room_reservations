using Data_Management_Service.Command;
using Database.Context;
using Database.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Data_Management_Service.ViewsModel
{
    /// <summary>
    /// ViewModel для регистрации нового пользователя.
    /// Обрабатывает ввод данных, валидацию и добавление записи в базу данных.
    /// </summary>
    public class RegisterViewModel : BaseViewModel
    {
        private readonly SqlServerContext _context;

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string Firstname { get; set; }
        /// <summary>
        /// Отчество пользователя.
        /// </summary>
        public string Middlename { get; set; }
        /// <summary>
        /// Фамилия пользователя.
        /// </summary>
        public string Lastname { get; set; }
        /// <summary>
        /// Логин для входа.
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Пароль для входа.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Команда для выполнения регистрации.
        /// </summary>
        public ICommand RegisterCommand { get; set; }
        /// <summary>
        /// Команда для закрытия окна регистрации.
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        /// Событие, вызываемое при ошибке регистрации.
        /// </summary>
        public event Action<string> OnError;
        /// <summary>
        /// Событие, вызываемое при успешной регистрации.
        /// </summary>
        public event Action<string> OnSuccess;
        /// <summary>
        /// Событие для закрытия окна регистрации.
        /// </summary>
        public event Action CloseRegistrationWindow;
        /// <summary>
        /// Конструктор. Инициализирует команды и контекст базы данных.
        /// </summary>
        public RegisterViewModel()
        {
            _context = new SqlServerContext();
            RegisterCommand = new RelayCommand(RegisterExecute);
            CloseCommand = new RelayCommand(CloseRegistrationView);
        }

        /// <summary>
        /// Выполняет регистрацию нового пользователя после валидации.
        /// </summary>
        /// <param name="obj">Не используется.</param>
        private void RegisterExecute(object obj)
        {
            // Проверка на пустые поля
            if (string.IsNullOrEmpty(Firstname) || string.IsNullOrEmpty(Middlename) || string.IsNullOrEmpty(Lastname) ||
                string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password))
            {
                OnError?.Invoke("Пожалуйста, заполните все поля.");
                return;
            }

            // Проверка на наличие букв в имени, фамилии и отчества
            if (!IsValidName(Firstname) || !IsValidName(Middlename) || !IsValidName(Lastname))
            {
                OnError?.Invoke("Имя, фамилия и отчество должны содержать только буквы.");
                return;
            }

            // Проверка на наличие букв и цифр в логине
            if (!IsValidLogin(Login))
            {
                OnError?.Invoke("Логин должен содержать только буквы и цифры.");
                return;
            }

            // Проверка на наличие букв и цифр в пароле
            if (!IsValidPassword(Password))
            {
                OnError?.Invoke("Пароль должен содержать только буквы и цифры.");
                return;
            }

            // Проверка на уникальность логина
            var existingUser = _context.User.FirstOrDefault(u => u.Login == Login);
            if (existingUser != null)
            {
                OnError?.Invoke("Пользователь с таким логином уже существует.");
                return;
            }

            // Создаем нового пользователя с ролью User по умолчанию
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Firstname = Firstname,
                Middlename = Middlename,
                Lastname = Lastname,
                Login = Login,
                Password = Password,
                Role = "User"  // Роль по умолчанию - User
            };

            // Добавляем нового пользователя в базу данных
            _context.User.Add(newUser);
            _context.SaveChanges();

            OnSuccess?.Invoke("Регистрация прошла успешно!");

            // Очистка полей после успешной регистрации
            ClearFields();
        }

        /// <summary>
        /// Закрывает окно регистрации.
        /// </summary>
        /// <param name="obj">Не используется.</param>
        private void CloseRegistrationView(object obj)
        {
            CloseRegistrationWindow?.Invoke(); // Делегируем закрытие окна в представление
        }

        /// <summary>
        /// Очищает все поля ввода.
        /// </summary>
        private void ClearFields()
        {
            Firstname = string.Empty;
            Middlename = string.Empty;
            Lastname = string.Empty;
            Login = string.Empty;
            Password = string.Empty;
        }

        // <summary>
        /// Проверяет логин на допустимость (только буквы и цифры).
        /// </summary>
        private bool IsValidLogin(string login)
        {
            var regex = new Regex(@"^[a-zA-Z0-9]+$"); // Регулярное выражение для букв и цифр
            return regex.IsMatch(login);
        }

        /// <summary>
        /// Проверяет пароль на допустимость (только буквы и цифры).
        /// </summary>
        private bool IsValidPassword(string password)
        {
            var regex = new Regex(@"^[a-zA-Z0-9]+$"); // Регулярное выражение для букв и цифр
            return regex.IsMatch(password);
        }

        /// <summary>
        /// Проверяет имя/фамилию/отчество на допустимость (только буквы, кириллица и латиница).
        /// </summary>
        private bool IsValidName(string name)
        {
            var regex = new Regex(@"^[a-zA-Zа-яА-Я]+$"); // Регулярное выражение для букв (кириллица и латиница)
            return regex.IsMatch(name);
        }
    }
}
