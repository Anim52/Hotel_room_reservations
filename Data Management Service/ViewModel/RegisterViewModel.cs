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
using System.Windows;


namespace Data_Management_Service.ViewModel
{
    public class RegisterViewModel : BaseViewModel
    {
        private readonly SqlServerContext _context;

        // Свойства для привязки в XAML
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        // Команда для регистрации
        public ICommand RegisterCommand { get; set; }

        public RegisterViewModel()
        {
            _context = new SqlServerContext();
            RegisterCommand = new RelayCommand(RegisterExecute); // Исправление команды
        }
        public event Action<string> OnError;
        public event Action<string> OnSuccess;

        // Метод регистрации
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
            Firstname = string.Empty;
            Middlename = string.Empty;
            Lastname = string.Empty;
            Login = string.Empty;
            Password = string.Empty;
        }
  
        // Метод для проверки логина на наличие только букв и цифр
        private bool IsValidLogin(string login)
        {
            var regex = new Regex(@"^[a-zA-Z0-9]+$"); // Регулярное выражение для букв и цифр
            return regex.IsMatch(login);
        }

        // Метод для проверки пароля на наличие только букв и цифр
        private bool IsValidPassword(string password)
        {
            var regex = new Regex(@"^[a-zA-Z0-9]+$"); // Регулярное выражение для букв и цифр
            return regex.IsMatch(password);
        }

        // Метод для проверки имени, фамилии и отчества на наличие только букв
        private bool IsValidName(string name)
        {
            var regex = new Regex(@"^[a-zA-Zа-яА-Я]+$"); // Регулярное выражение для букв (кириллица и латиница)
            return regex.IsMatch(name);
        }
    }
}
