using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Service
{
    public enum TypeNumder
    {
        Standart,
        Studio,
        Suite,
        Apartment
    };

    public class Nomer
    {


        public Nomer(Guid id, int number, int floor, bool status, decimal cost, string description, TypeNumder typeNumder)
        {
            Id = id;
            Number = number;
            Floor = floor;
            Status = status;
            Cost = cost;
            Description = description;
            TypeNumder = typeNumder;
        }
        public Nomer()
        {
        }

        public Guid Id { get; set; }

        public TypeNumder TypeNumder { get; set; }
        public int Number { get; set; }
        public int Floor { get; set; }
        public bool Status { get; set; }
        public decimal Cost { get; set; }
        public string Description { get; set; }
        public string? ImagePath { get; set; }

        [NotMapped]

        public string TypeNumberString
        {
            get
            {
                switch (TypeNumder)
                {
                    case TypeNumder.Standart:
                        return "Стандарт";
                    case TypeNumder.Studio:
                        return "Студия";
                    case TypeNumder.Suite:
                        return "Люкс";
                    case TypeNumder.Apartment:
                        return "Аппартаменты";
                    default:
                        return "Не выбрано";
                }
            }
        }

        public override string ToString()
        {

            return $"{Number},{TypeNumder},{Floor},{Status},{Cost},{Description}";
        }
    }
}
