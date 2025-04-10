using Data_Management_Service.Command;
using Data_Management_Service.OtherViews;
using Data_Management_Service.ViewsModel;
using Database.Context;
using Database.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Data_Management_Service.PageViewModel
{
    public class ReservationsViewModel : BaseViewModel
    {
        private readonly SqlServerContext _context;

        private Nomer _selectedNomer;
        private Reservations _selectedReservation;
        private DateTime? _arrivalDate;
        private DateTime? _departureDate;
        private int _numberOfPersons;
        private decimal _calculatedPrice;

        public ObservableCollection<Nomer> NomerList { get; set; }
        public ObservableCollection<Reservations> ReservationList { get; set; }

        // Делегаты для сообщений
        public Action<string> OnError { get; set; }
        public Action<string> OnSuccess { get; set; }
        public Action<Reservations> OnReservationAdded { get; set; }

        public ReservationsViewModel()
        {
            _context = new SqlServerContext();
            AddReservationCommand = new RelayCommand(AddReservation);
            CancelReservationCommand = new RelayCommand(CancelReservation);
            SetPopulatedCommand = new RelayCommand(SetPopulated);
            ConfirmReservationCommand = new RelayCommand(ConfirmReservation);

            NomerList = new ObservableCollection<Nomer>(_context.Nomers.Where(n => n.Status).ToList());
            ReservationList = new ObservableCollection<Reservations>(
                _context.Reservations
                    .Include(r => r.Nomer)
                    .Include(r => r.Guests)
                    .ToList());
        }
        
        public ICommand AddReservationCommand { get; }
        public ICommand CancelReservationCommand { get; }
        public ICommand SetPopulatedCommand { get; }
        public ICommand ConfirmReservationCommand { get; }


        #region BildingToXaml
        public decimal CalculatedPrice
        {
            get => _calculatedPrice;
            set
            {
                _calculatedPrice = value;
                OnPropertyChanged(nameof(CalculatedPrice));
            }
        }

        public Nomer SelectedNomer
        {
            get => _selectedNomer;
            set
            {
                _selectedNomer = value;
                OnPropertyChanged(nameof(SelectedNomer));
                UpdateCalculatedPrice();
            }
        }

        public Reservations SelectedReservation
        {
            get => _selectedReservation;
            set
            {
                _selectedReservation = value;
                OnPropertyChanged(nameof(SelectedReservation));
            }
        }

        public DateTime? ArrivalDate
        {
            get => _arrivalDate;
            set
            {
                _arrivalDate = value;
                OnPropertyChanged(nameof(ArrivalDate));
                UpdateCalculatedPrice();
            }
        }

        public DateTime? DepartureDate
        {
            get => _departureDate;
            set
            {
                _departureDate = value;
                OnPropertyChanged(nameof(DepartureDate));
                UpdateCalculatedPrice();
            }
        }

        public int NumberOfPersons
        {
            get => _numberOfPersons;
            set
            {
                _numberOfPersons = value;
                OnPropertyChanged(nameof(NumberOfPersons));
            }
        }
        #endregion
        private void AddReservation(object obj)
        {
            // Проверяем обязательные поля
            if (SelectedNomer == null || !ArrivalDate.HasValue || !DepartureDate.HasValue || NumberOfPersons <= 0)
            {
                OnError?.Invoke("Все поля должны быть заполнены корректно.");
                return;
            }

            // Проверяем, что дата приезда раньше даты отъезда
            if (ArrivalDate >= DepartureDate)
            {
                OnError?.Invoke("Дата приезда должна быть раньше даты отъезда.");
                return;
            }

            // Получаем ID текущего пользователя
            var CurrentUserId = SessionService.CurrentUserId;
            if (CurrentUserId == Guid.Empty)
            {
                OnError?.Invoke("Ошибка: Пользователь не авторизован.");
                return;
            }

            // Получаем пользователя по ID
            var currentUser = _context.Guests.FirstOrDefault(g => g.Id == CurrentUserId);
            if (currentUser == null)
            {
                OnError?.Invoke("Ошибка: Пользователь не найден в базе данных.");
                return;
            }

            // Создаем новое бронирование
            var newReservation = new Reservations
            {
                Id = Guid.NewGuid(),
                Nomer = SelectedNomer,  // Здесь мы присваиваем номер
                Guests = currentUser,    // Здесь мы присваиваем пользователя
                DateReservations = DateTime.Now, // Текущая дата бронирования
                ArrivalDate = ArrivalDate.Value, // Дата приезда
                DepartureDate = DepartureDate.Value, // Дата отъезда
                NumberOfPersons = NumberOfPersons, // Количество человек
                Status = Status.New, // Статус нового бронирования
                TotalPrice = CalculatedPrice
            };

            // Проверяем, что номер не равен null (на всякий случай, но это не должно происходить, если все корректно)
            if (newReservation.Nomer == null)
            {
                OnError?.Invoke("Ошибка: Номер не выбран.");
                return;
            }

            // Добавляем бронирование в базу данных
            _context.Reservations.Add(newReservation);
            _context.SaveChanges(); // Сохраняем изменения

            // Добавляем бронирование в список на клиенте
            ReservationList.Add(newReservation);

            // Вызываем событие успешного добавления
            OnReservationAdded?.Invoke(newReservation);
            OnSuccess?.Invoke("Бронирование успешно добавлено!");

            // Сброс полей после добавления
            SelectedNomer = null;
            ArrivalDate = null;
            DepartureDate = null;
            NumberOfPersons = 0;
        }
        private void ConfirmReservation(object obj)
        {
            if (SelectedReservation == null)
            {
                OnError?.Invoke("Пожалуйста, выберите бронирование.");
                return;
            }
            SelectedReservation.Status = Status.Verified;
            SelectedReservation.Nomer.Status = false;

            _context.Reservations.Update(SelectedReservation);
            _context.Nomers.Update(SelectedReservation.Nomer);
            _context.SaveChanges();

            OnSuccess?.Invoke("Бронирование подтверждено!");
        }

        private void SetPopulated(object obj)
        {
            if (SelectedReservation == null)
            {
                OnError?.Invoke("Пожалуйста, выберите бронирование.");
                return;
            }

            SelectedReservation.Status = Status.Populated;
            SelectedReservation.Nomer.Status = false;

            _context.Reservations.Update(SelectedReservation);
            _context.Nomers.Update(SelectedReservation.Nomer);
            _context.SaveChanges();

            OnSuccess?.Invoke("Гость заселен!");
        }

        private void CancelReservation(object obj)
        {
            if (SelectedReservation == null)
            {
                OnError?.Invoke("Пожалуйста, выберите бронирование.");
                return;
            }

            // Проверяем, не null ли Nomer
            if (SelectedReservation.Nomer == null)
            {
                OnError?.Invoke("Номер не найден для этого бронирования.");
                return;
            }

            // Теперь можно безопасно работать с Nomer
            SelectedReservation.Nomer.Status = true; // Номер освобожден
            _context.Nomers.Update(SelectedReservation.Nomer);
            _context.Reservations.Remove(SelectedReservation);
            _context.SaveChanges();

            ReservationList.Remove(SelectedReservation);
            OnSuccess?.Invoke("Бронирование отменено!");
        }
        private void UpdateCalculatedPrice()
        {
            if (SelectedNomer != null && ArrivalDate.HasValue && DepartureDate.HasValue && ArrivalDate < DepartureDate)
            {
                CalculatedPrice = (decimal)(DepartureDate.Value - ArrivalDate.Value).TotalDays * SelectedNomer.Cost;
            }
            else
            {
                CalculatedPrice = 0;
            }
        }


    }

}
