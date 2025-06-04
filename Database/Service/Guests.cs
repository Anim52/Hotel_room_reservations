using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Service
{
    /// <summary>
    /// Представляет гостя отеля.
    /// </summary>
    public class Guests
    {
        /// <summary>
        /// Инициализирует нового гостя с параметрами.
        /// </summary>
        public Guests(Guid id, string lastName, string firstName, string middleName, DateTime dateOfBirth, int passportNumber, string contactDetails, DateTime registrationDate, string preferences)
        {
            Id = id;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            PassportNumber = passportNumber;
            ContactDetails = contactDetails;
            RegistrationDate = DateTime.Now;
            Preferences = preferences;
        }
        /// <summary>
        /// Конструктор без параметров. Устанавливает дату регистрации в текущую дату.
        /// </summary>
        public Guests()
        {
            RegistrationDate = DateTime.Now;
        }
        /// <summary>
        /// Уникальный идентификатор гостя.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Фамилия гостя.
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Имя гостя.
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Отчество гостя.
        /// </summary>
        public string MiddleName { get; set; }
        /// <summary>
        /// Дата рождения.
        /// </summary>
        public DateTime DateOfBirth { get; set; }
        /// <summary>
        /// Номер паспорта.
        /// </summary>
        public int PassportNumber { get; set; }
        /// <summary>
        /// Контактная информация.
        /// </summary>
        public string ContactDetails { get; set; }
        /// <summary>
        /// Дата регистрации в системе.
        /// </summary>
        public DateTime RegistrationDate { get; set; }
        /// <summary>
        /// Предпочтения гостя.
        /// </summary>
        public string Preferences { get; set; }
        /// <summary>
        /// Полное имя (Фамилия Имя Отчество).
        /// </summary>
        [NotMapped]
        public string FullName => $"{LastName} {FirstName} {MiddleName}";
        /// <summary>
        /// Представление объекта в виде строки.
        /// </summary>
        public override string ToString()
        {
            return $"{FullName},{DateOfBirth},{PassportNumber},{ContactDetails},{RegistrationDate},{Preferences}";
        }
    }
}
