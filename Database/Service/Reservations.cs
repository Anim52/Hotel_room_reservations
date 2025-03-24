using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Service
{
    public enum Status
    {
        New,
        Verified,
        Populated,

    };

    public class Reservations
    {
        public Reservations(Guid id, Nomer nomer, Guests guests, DateTime dateReservations, DateTime arrivalDate, DateTime departureDate, int numberOfPersons)
        {
            Id = id;
            Nomer = nomer;
            Guests = guests;
            DateReservations = dateReservations;
            ArrivalDate = arrivalDate;
            DepartureDate = departureDate;
            NumberOfPersons = numberOfPersons;
        }
        public Reservations()
        {
        }

        public Guid Id { get; set; }
        public Nomer Nomer { get; set; }
        public Guests Guests { get; set; }
        public DateTime DateReservations { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public enum StatusReservations;
        public int NumberOfPersons { get; set; }
        public Status Status { get; set; }
        [NotMapped]

        public string StatusRegister
        {
            get
            {
                switch (Status)
                {
                    case Status.New:
                        return "Новое";
                    case Status.Verified:
                        return "Проверено";
                    case Status.Populated:
                        return "Заселен";
                    default:
                        return "Не выбран";
                }
            }
        }


        public override string ToString()
        {
            return $"{Nomer},{Guests},{DateReservations},{ArrivalDate},{DepartureDate},{NumberOfPersons},{StatusRegister}";
        }
    }
}
