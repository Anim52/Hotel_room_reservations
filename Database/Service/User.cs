using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Service
{
    /// <summary>
    /// Представляет пользователя системы.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Инициализирует нового пользователя с параметрами.
        /// </summary>
        public User(Guid id, string lastname, string firstname, string middlename, string login, string password)
        {
            Id = id;
            Lastname = lastname;
            Firstname = firstname;
            Middlename = middlename;
            Login = login;
            Password = password;
        }
        /// <summary>
        /// Конструктор без параметров.
        /// </summary>
        public User()
        {
        }
        /// <summary>
        /// Уникальный идентификатор пользователя.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Фамилия пользователя.
        /// </summary>
        public string Lastname { get; set; } = null!;
        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string Firstname { get; set; } = null!;
        /// <summary>
        /// Отчество пользователя.
        /// </summary>
        public string Middlename { get; set; } = null!;
        /// <summary>
        /// Логин для входа.
        /// </summary>
        public string Login { get; set; } = null!;
        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        public string Password { get; set; } = null!;
        /// <summary>
        /// Роль пользователя в системе.
        /// </summary>
        public string Role { get; set; } = null!;
        /// <summary>
        /// Полное имя пользователя (ФИО).
        /// </summary>
        [NotMapped]
        public string FullName => $"{Lastname},{Firstname},{Middlename}";
        /// <summary>
        /// Преобразование в строку (для отображения).
        /// </summary>
        public override string ToString()
        {
            return $"{Lastname},{Firstname},{Middlename},{Login},{Password}";
        }
        /// <summary>
        /// Создаёт нового пользователя с автоматически сгенерированным Id.
        /// </summary>
        public static User Create(string lastName, string firstName, string middleName, string login, string password)
        {
            return new User(Guid.NewGuid(), lastName, firstName, middleName, login, password);
        }

    }
}
