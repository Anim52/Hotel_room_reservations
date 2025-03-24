using Database.Context;
using Database.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Management_Service.OtherViews
{
    public class AuthService
    {
        private readonly SqlServerContext _context;

        // Конструктор по умолчанию
        public AuthService()
        {
            _context = new SqlServerContext();
        }

        // Конструктор с параметром (если нужно передавать контекст извне)
        public AuthService(SqlServerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Проверяет учетные данные при входе
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <param name="user">Возвращает найденного пользователя</param>
        /// <returns>Возвращает true, если пользователь найден, иначе false</returns>
        User? user = null;
        public bool Login(string login, string password, out User? user)
        {
            user = _context.User.FirstOrDefault(u => u.Login == login && u.Password == password);
            return user != null;
        }

        /// <summary>
        /// Регистрирует нового пользователя
        /// </summary>
        /// <param name="lastname">Фамилия</param>
        /// <param name="firstname">Имя</param>
        /// <param name="middlename">Отчество</param>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <returns>Возвращает true, если регистрация успешна, иначе false</returns>
        public bool Register(string lastname, string firstname, string middlename, string login, string password)
        {
            // Проверяем, есть ли уже пользователь с таким логином
            if (_context.User.Any(u => u.Login == login))
                return false; // Логин уже занят

            // Создаем нового пользователя
            var newUser = User.Create(lastname, firstname, middlename, login, password);
            _context.User.Add(newUser);
            _context.SaveChanges();
            return true;
        }
    }
}
