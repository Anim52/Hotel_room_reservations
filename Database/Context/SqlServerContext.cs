using Database.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Context
{
    public class SqlServerContext : DbContext
    {
        public DbSet<Guests> Guests { get; set; }
        public DbSet<Nomer> Nomers { get; set; }
        public DbSet<Reservations> Reservations { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = (localdb)\\MSSQLLocalDB;Database = HotelDb");
        }
    }
}
