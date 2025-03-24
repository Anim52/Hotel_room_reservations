using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Service
{
    public class Guests
    {
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
        public Guests()
        {
            RegistrationDate = DateTime.Now;
        }

        public Guid Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int PassportNumber { get; set; }
        public string ContactDetails { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Preferences { get; set; }

        [NotMapped]
        public string FullName => $"{LastName} {FirstName} {MiddleName}";
        public override string ToString()
        {
            return $"{FullName},{DateOfBirth},{PassportNumber},{ContactDetails},{RegistrationDate},{Preferences}";
        }
    }
}
