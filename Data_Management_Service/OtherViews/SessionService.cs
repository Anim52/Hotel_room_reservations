using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Management_Service.OtherViews
{
    /// <summary>
    /// Сервис сеанса, хранящий текущие данные пользователя.
    /// </summary>
    public static class SessionService
    {
        /// <summary>
        /// Логин текущего пользователя.
        /// </summary>
        public static string CurrentUserLogin { get; set; }
        /// <summary>
        /// Уникальный идентификатор текущего пользователя.
        /// </summary>
        public static Guid CurrentUserId { get; set; }
    }
}
