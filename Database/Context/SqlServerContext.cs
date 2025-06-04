using Database.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Context
{
    /// <summary>
    /// Контекст базы данных для SQL Server, используемый для работы с сущностями отеля.
    /// </summary>
    public class SqlServerContext : DbContext
    {
        /// <summary>
        /// Набор данных гостей.
        /// </summary>
        public DbSet<Guests> Guests { get; set; }
        /// <summary>
        /// Набор данных номеров.
        /// </summary>
        public DbSet<Nomer> Nomers { get; set; }
        /// <summary>
        /// Набор данных бронирований.
        /// </summary>
        public DbSet<Reservations> Reservations { get; set; }
        /// <summary>
        /// Набор данных услуг.
        /// </summary>
        public DbSet<Services> Services { get; set; }
        /// <summary>
        /// Набор данных пользователей.
        /// </summary>
        public DbSet<User> User { get; set; }
        /// <summary>
        /// Конфигурирует параметры подключения к базе данных SQL Server.
        /// </summary>
        /// <param name="optionsBuilder">Параметры настройки контекста.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = (localdb)\\MSSQLLocalDB;Database = HotelDatabase");
        }
    }
}
