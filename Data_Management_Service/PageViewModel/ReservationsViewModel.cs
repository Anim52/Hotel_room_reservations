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
using Aspose.Pdf;
using Aspose.Pdf.Text;
using System;
using System.IO;

namespace Data_Management_Service.PageViewModel
{
    /// <summary>
    /// Модель представления для управления бронированиями.
    /// </summary>
    public class ReservationsViewModel : BaseViewModel
    {
        private readonly SqlServerContext _context;

        private Nomer _selectedNomer;
        private Reservations _selectedReservation;
        private DateTime? _arrivalDate;
        private DateTime? _departureDate;
        private int _numberOfPersons;
        private decimal _calculatedPrice;
        private string _selectedRoomType; 
        private ObservableCollection<Nomer> _filteredNomerList;
        /// <summary>
        /// Список всех доступных номеров.
        /// </summary>
        public ObservableCollection<Nomer> NomerList { get; set; }
        /// <summary>
        /// Список всех бронирований.
        /// </summary>
        public ObservableCollection<Reservations> ReservationList { get; set; }
        /// <summary>
        /// Список бронирований текущего пользователя.
        /// </summary>
        public ObservableCollection<Reservations> UserReservationList { get; set; }
        /// <summary>
        /// Типы комнат.
        /// </summary>
        public ObservableCollection<TypeNumder> RoomTypes { get; set; }
        /// <summary>
        /// Отфильтрованный список номеров по типу.
        /// </summary>
        public ObservableCollection<Nomer> FilteredNomerList 
        {
            get => _filteredNomerList;
            set
            {
                _filteredNomerList = value;
                OnPropertyChanged(nameof(FilteredNomerList));
            }
        }


    
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
            SelectedRoomType = "Все"; 
            FilteredNomerList = new ObservableCollection<Nomer>(NomerList); 
            ReservationList = new ObservableCollection<Reservations>(
                _context.Reservations
                    .Include(r => r.Nomer)
                    .Include(r => r.Guests)
                    .ToList());

            ReservationList = new ObservableCollection<Reservations>(_context.Reservations
                    .Include(r => r.Guests)
                    .Include(r => r.Nomer)
                     .ToList());

            var currentUserId = SessionService.CurrentUserId;

            UserReservationList = new ObservableCollection<Reservations>(
                _context.Reservations
                    .Include(r => r.Guests)
                    .Include(r => r.Nomer)
                    .Where(r => r.Guests.Id == currentUserId)
                    .ToList()
            );
            RoomTypes = new ObservableCollection<TypeNumder>(
    Enum.GetValues(typeof(TypeNumder)).Cast<TypeNumder>());


        }
        /// <summary>
        /// Команда добавления бронирования.
        /// </summary>
        public ICommand AddReservationCommand { get; }
        /// <summary>
        /// Команда отмены бронирования.
        /// </summary>
        public ICommand CancelReservationCommand { get; }
        /// <summary>
        /// Команда заселения гостя.
        /// </summary>
        public ICommand SetPopulatedCommand { get; }
        /// <summary>
        /// Команда подтверждения бронирования.
        /// </summary>
        public ICommand ConfirmReservationCommand { get; }


        #region BildingToXaml
        /// <summary>
        /// Вычисленная цена проживания.
        /// </summary>
        public decimal CalculatedPrice
        {
            get => _calculatedPrice;
            set
            {
                _calculatedPrice = value;
                OnPropertyChanged(nameof(CalculatedPrice));
            }
        }
        /// <summary>
        /// Выбранный номер.
        /// </summary>
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
        /// <summary>
        /// Выбранное бронирование.
        /// </summary>
        public Reservations SelectedReservation
        {
            get => _selectedReservation;
            set
            {
                _selectedReservation = value;
                OnPropertyChanged(nameof(SelectedReservation));
            }
        }
        /// <summary>
        /// Дата прибытия.
        /// </summary>
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
        /// <summary>
        /// Дата отъезда.
        /// </summary>
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
        /// <summary>
        /// Количество человек.
        /// </summary>
        public int NumberOfPersons
        {
            get => _numberOfPersons;
            set
            {
                _numberOfPersons = value;
                OnPropertyChanged(nameof(NumberOfPersons));
            }
        }
        /// <summary>
        /// Выбранный тип номера.
        /// </summary>  
        public string SelectedRoomType
        {
            get => _selectedRoomType;
            set
            {
                _selectedRoomType = value;
                OnPropertyChanged(nameof(SelectedRoomType));
                FilterNomerList(); // Фильтруем список номеров при изменении типа
            }
        }
        private void FilterNomerList()
        {
            if (SelectedRoomType == "Все")
            {
                FilteredNomerList = new ObservableCollection<Nomer>(NomerList);
            }
            else if (Enum.TryParse<TypeNumder>(SelectedRoomType, out var selectedType))
            {
                FilteredNomerList = new ObservableCollection<Nomer>(
                    NomerList.Where(n => n.TypeNumder == selectedType)
                );
            }

            OnPropertyChanged(nameof(FilteredNomerList));
        }


        #endregion
        /// <summary>
        /// Добавляет новое бронирование.
        /// </summary>
        private void AddReservation(object obj)
        {
            
            if (SelectedNomer == null || !ArrivalDate.HasValue || !DepartureDate.HasValue || NumberOfPersons <= 0)
            {
                OnError?.Invoke("Все поля должны быть заполнены корректно.");
                return;
            }

        
            if (ArrivalDate >= DepartureDate)
            {
                OnError?.Invoke("Дата приезда должна быть раньше даты отъезда.");
                return;
            }


            var CurrentUserId = SessionService.CurrentUserId;
            if (CurrentUserId == Guid.Empty)
            {
                OnError?.Invoke("Ошибка: Пользователь не авторизован.");
                return;
            }

           
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
                Nomer = SelectedNomer,  
                Guests = currentUser,    
                DateReservations = DateTime.Now, 
                ArrivalDate = ArrivalDate.Value, 
                DepartureDate = DepartureDate.Value, 
                NumberOfPersons = NumberOfPersons, 
                Status = Status.New, 
                TotalPrice = CalculatedPrice
            };

       
            if (newReservation.Nomer == null)
            {
                OnError?.Invoke("Ошибка: Номер не выбран.");
                return;
            }

     
            _context.Reservations.Add(newReservation);
            _context.SaveChanges(); 

      
            ReservationList.Add(newReservation);
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Бронь.pdf");
            GenerateReservationPdf(newReservation, filePath);


     
            OnReservationAdded?.Invoke(newReservation);
            OnSuccess?.Invoke("Бронирование успешно добавлено!");
            


        
            SelectedNomer = null;
            ArrivalDate = null;
            DepartureDate = null;
            NumberOfPersons = 0;
        }
        /// <summary>
        /// Подтверждает выбранное бронирование.
        /// </summary>
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
        /// <summary>
        /// Устанавливает статус бронирования как "Заселен".
        /// </summary>
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

        /// <summary>
        /// Отменяет выбранное бронирование.
        /// </summary>
        private void CancelReservation(object obj)
        {
            if (SelectedReservation == null)
            {
                OnError?.Invoke("Пожалуйста, выберите бронирование.");
                return;
            }

            
            if (SelectedReservation.Nomer == null)
            {
                OnError?.Invoke("Номер не найден для этого бронирования.");
                return;
            }

         
            SelectedReservation.Nomer.Status = true; 
            _context.Nomers.Update(SelectedReservation.Nomer);
            _context.Reservations.Remove(SelectedReservation);
            _context.SaveChanges();

            ReservationList.Remove(SelectedReservation);
            OnSuccess?.Invoke("Бронирование отменено!");
        }
        /// <summary>
        /// Обновляет расчет общей стоимости проживания.
        /// </summary>
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
        /// <summary>
        /// Генерирует PDF с деталями бронирования.
        /// </summary>
        /// <param name="reservation">Данные бронирования.</param>
        /// <param name="filePath">Путь сохранения PDF-файла.</param>
        public void GenerateReservationPdf(Reservations reservation, string filePath)
        {
            var doc = new Document();
            var page = doc.Pages.Add();

           
            var title = new TextFragment("Подтверждение бронирования")
            {
                TextState = { FontSize = 20, FontStyle = FontStyles.Bold },
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = { Bottom = 20 }
            };
            page.Paragraphs.Add(title);

        
            page.Paragraphs.Add(new TextFragment("--------------------------------------------------------"));

            AddField(page, "Номер комнаты:", reservation.Nomer?.Number.ToString() ?? "N/A");
            AddField(page, "Гость:", $"{reservation.Guests?.FirstName ?? "N/A"} {reservation.Guests?.LastName ?? ""}");
            AddField(page, "Дата бронирования:", reservation.DateReservations.ToString("dd.MM.yyyy HH:mm"));
            AddField(page, "Дата заезда:", reservation.ArrivalDate.ToString("dd.MM.yyyy"));
            AddField(page, "Дата выезда:", reservation.DepartureDate.ToString("dd.MM.yyyy"));
            AddField(page, "Количество гостей:", reservation.NumberOfPersons.ToString());
            AddField(page, "Статус:", reservation.Status.ToString());
            AddField(page, "Общая стоимость:", reservation.TotalPrice.ToString("N0") + " руб." ?? "Не указано");

         
            page.Paragraphs.Add(new TextFragment("--------------------------------------------------------"));
            page.Paragraphs.Add(new TextFragment("Если возникнут какие-то вопросы обратитесь к администратору"));

        
            var dateGenerated = new TextFragment($"Документ создан: {DateTime.Now:dd.MM.yyyy HH:mm}")
            {
                TextState = { FontSize = 10, FontStyle = FontStyles.Italic },
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = { Top = 20 }
            };
            page.Paragraphs.Add(dateGenerated);

     
            doc.Save(filePath);
        }

        /// <summary>
        /// Добавляет текстовое поле в PDF.
        /// </summary>
        private void AddField(Page page, string label, string value)
        {
            var text = new TextFragment($"{label} {value}")
            {
                TextState = { FontSize = 12, Font = FontRepository.FindFont("Arial") },
                Margin = { Top = 8, Bottom = 4 }
            };
            page.Paragraphs.Add(text);
        }


    }

}
