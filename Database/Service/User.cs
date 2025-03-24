using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Service
{
    public class User
    {
        public User(Guid id, string lastname, string firstname, string middlename, string login, string password)
        {
            Id = id;
            Lastname = lastname;
            Firstname = firstname;
            Middlename = middlename;
            Login = login;
            Password = password;
        }
        public User()
        {
        }
        public Guid Id { get; set; }
        public string Lastname { get; set; } = null!;
        public string Firstname { get; set; } = null!;
        public string Middlename { get; set; } = null!;
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;

        [NotMapped]
        public string FullName => $"{Lastname},{Firstname},{Middlename}";

        public override string ToString()
        {
            return $"{Lastname},{Firstname},{Middlename},{Login},{Password}";
        }
        public static User Create(string lastName, string firstName, string middleName, string login, string password)
        {
            return new User(Guid.NewGuid(), lastName, firstName, middleName, login, password);
        }

    }
}
