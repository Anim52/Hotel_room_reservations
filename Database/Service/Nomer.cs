using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Service
{
    /// <summary>
    /// Типы номеров в отеле.
    /// </summary>
    public enum TypeNumder
    {
        Standart,
        Studio,
        Suite,
        Apartment
    };
    /// <summary>
    /// Модель номера отеля.
    /// </summary>
    public class Nomer
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Nomer"/>.
        /// </summary>
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
        /// <summary>
        /// Инициализирует новый пустой экземпляр класса <see cref="Nomer"/>.
        /// </summary>
        public Nomer()
        {
        }
        /// <summary>
        /// Уникальный идентификатор номера.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Тип номера.
        /// </summary>
        public TypeNumder TypeNumder { get; set; }
        /// <summary>
        /// Номер комнаты.
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// Этаж, на котором расположен номер.
        /// </summary>
        public int Floor { get; set; }
        /// <summary>
        /// Статус занятости номера (true - занят, false - свободен).
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// Стоимость номера.
        /// </summary>
        public decimal Cost { get; set; }
        /// <summary>
        /// Описание номера.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Путь к изображению номера.
        /// </summary>
        public string? ImagePath { get; set; }

        /// <summary>
        /// Строковое представление типа номера.
        /// </summary>
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
        /// <summary>
        /// Возвращает строковое представление объекта.
        /// </summary>
        /// <returns>Строка с параметрами номера.</returns>
        public override string ToString()
        {

            return $"{Number},{TypeNumder},{Floor},{Status},{Cost},{Description}";
        }
    }
}
