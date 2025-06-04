using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Service
{
    /// <summary>
    /// Модель услуги, запрашиваемой пользователем.
    /// </summary>
    public class Services
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Services"/>.
        /// </summary>
        public Services(Guid id, Guid userId, string description, DateTime requestDate, string status)
        {
            Id = id;
            UserId = userId;
            Description = description;
            RequestDate = requestDate;
            Status = status;
        }
        /// <summary>
        /// Инициализирует новый пустой экземпляр класса <see cref="Services"/>.
        /// </summary>
        public Services()
        {

        }
        /// <summary>
        /// Уникальный идентификатор услуги.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();
        /// <summary>
        /// Идентификатор пользователя, запросившего услугу.
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Описание услуги.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Дата запроса услуги.
        /// </summary>
        public DateTime RequestDate { get; set; } = DateTime.Now;
        /// <summary>
        /// Статус выполнения услуги.
        /// </summary>
        public string Status { get; set; } = "В обработке";
        /// <summary>
        /// Пользователь, связанный с услугой.
        /// </summary>
        public User User { get; set; }
        /// <summary>
        /// Полное имя пользователя, с сокращением имени и отчества.
        /// </summary>
        public string Fullname => $"{User.Lastname} {User.Firstname[0]}. {User.Middlename[0]}.";

    }
}
