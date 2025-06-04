using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Service
{
    /// <summary>
    /// Статусы бронирования.
    /// </summary>

    public enum Status
    {
        New,
        Verified,
        Populated,

    };
    /// <summary>
    /// Модель бронирования номера.
    /// </summary>
    public class Reservations
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Reservations"/>.
        /// </summary>
        public Reservations(Guid id, Nomer nomer, Guests guests, DateTime dateReservations, DateTime arrivalDate, DateTime departureDate, int numberOfPersons,decimal totalPrice)
        {
            Id = id;
            Nomer = nomer;
            Guests = guests;
            DateReservations = dateReservations;
            ArrivalDate = arrivalDate;
            DepartureDate = departureDate;
            NumberOfPersons = numberOfPersons;
            TotalPrice = totalPrice;
        }
        /// <summary>
        /// Инициализирует новый пустой экземпляр класса <see cref="Reservations"/>.
        /// </summary>
        public Reservations()
        {
        }
        /// <summary>
        /// Уникальный идентификатор бронирования.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Номер, который бронируется.
        /// </summary>
        public Nomer Nomer { get; set; }
        /// <summary>
        /// Гость, который бронирует номер.
        /// </summary>
        public Guests Guests { get; set; }
        /// <summary>
        /// Дата создания бронирования.
        /// </summary>
        public DateTime DateReservations { get; set; }
        /// <summary>
        /// Дата заезда.
        /// </summary>
        public DateTime ArrivalDate { get; set; }
        /// <summary>
        /// Дата выезда.
        /// </summary>
        public DateTime DepartureDate { get; set; }
        
       
        public enum StatusReservations;
        /// <summary>
        /// Количество человек для бронирования.
        /// </summary>
        public int NumberOfPersons { get; set; }
        /// <summary>
        ///Статус бронирования.
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Итоговая стоимость бронирования.
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Читаемое строковое представление статуса бронирования.
        /// </summary>
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

        /// <summary>
        /// Возвращает строковое представление объекта бронирования.
        /// </summary>
        /// <returns>Строка с деталями бронирования.</returns>
        public override string ToString()
        {
            return $"{Nomer},{Guests},{DateReservations},{ArrivalDate},{DepartureDate},{NumberOfPersons},{StatusRegister}";
        }
    }
}
