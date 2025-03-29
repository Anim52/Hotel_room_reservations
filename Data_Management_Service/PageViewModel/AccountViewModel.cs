using Data_Management_Service.Command;
using Data_Management_Service.ViewsModel;
using Database.Context;
using Database.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Data_Management_Service.PageViewModel
{
    public class AccountViewModel : BaseViewModel
    {
        private readonly SqlServerContext _context;
        private Guests _currentGuest;

        public event Action<string> OnError;
        public event Action<string> OnSuccess;

        // Свойства
        private string _lastname;
        public string Lastname
        {
            get => _lastname;
            set
            {
                if (_lastname != value)
                {
                    _lastname = value;
                    OnPropertyChanged(nameof(Lastname));
                    OnPropertyChanged(nameof(FullName));
                }
            }
        }

        private string _firstname;
        public string Firstname
        {
            get => _firstname;
            set
            {
                if (_firstname != value)
                {
                    _firstname = value;
                    OnPropertyChanged(nameof(Firstname));
                    OnPropertyChanged(nameof(FullName));
                }
            }
        }

        private string _middlename;
        public string Middlename
        {
            get => _middlename;
            set
            {
                if (_middlename != value)
                {
                    _middlename = value;
                    OnPropertyChanged(nameof(Middlename));
                    OnPropertyChanged(nameof(FullName));
                }
            }
        }

        private DateTime? _dateOfBirth;
        public DateTime? DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                if (_dateOfBirth != value)
                {
                    _dateOfBirth = value;
                    OnPropertyChanged(nameof(DateOfBirth));
                }
            }
        }

        private int _passportNumber;
        public int PassportNumber
        {
            get => _passportNumber;
            set
            {
                if (_passportNumber != value)
                {
                    _passportNumber = value;
                    OnPropertyChanged(nameof(PassportNumber));
                }
            }
        }

        private string _contactDetails;
        public string ContactDetails
        {
            get => _contactDetails;
            set
            {
                if (_contactDetails != value)
                {
                    _contactDetails = value;
                    OnPropertyChanged(nameof(ContactDetails));
                }
            }
        }

        private DateTime? _registrationDate;
        public DateTime? RegistrationDate
        {
            get => _registrationDate;
            set
            {
                if (_registrationDate != value)
                {
                    _registrationDate = value;
                    OnPropertyChanged(nameof(RegistrationDate));
                }
            }
        }

        private string _preferences;
        public string Preferences
        {
            get => _preferences;
            set
            {
                if (_preferences != value)
                {
                    _preferences = value;
                    OnPropertyChanged(nameof(Preferences));
                }
            }
        }

        public string FullName
        {
            get => $"{Lastname} {Firstname} {Middlename}".Trim();
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    var nameParts = value.Split(' ');
                    Lastname = nameParts.Length > 0 ? nameParts[0] : string.Empty;
                    Firstname = nameParts.Length > 1 ? nameParts[1] : string.Empty;
                    Middlename = nameParts.Length > 2 ? nameParts[2] : string.Empty;
                }
            }
        }

        // Команды
        public ICommand SaveCommand { get; }

        // Конструктор
        public AccountViewModel(Guid userId)
        {
            _context = new SqlServerContext();
            SaveCommand = new RelayCommand(SaveProfile);

            _currentGuest = _context.Guests.FirstOrDefault(g => g.Id == userId);

            if (_currentGuest != null)
            {
                LoadGuestData();
            }
            else
            {
                OnError?.Invoke("Ошибка загрузки профиля.");
            }
        }

        private void LoadGuestData()
        {
            if (_currentGuest == null)
            {
                OnError?.Invoke("Ошибка: данные пользователя отсутствуют.");
                return;
            }

            _context.Entry(_currentGuest).Reload();

            Lastname = _currentGuest.LastName ?? string.Empty;
            Firstname = _currentGuest.FirstName ?? string.Empty;
            Middlename = _currentGuest.MiddleName ?? string.Empty;
            DateOfBirth = _currentGuest.DateOfBirth;
            PassportNumber = _currentGuest.PassportNumber;
            ContactDetails = _currentGuest.ContactDetails ?? string.Empty;
            RegistrationDate = _currentGuest.RegistrationDate;
            Preferences = _currentGuest.Preferences ?? string.Empty;
        }

        private void SaveProfile(object obj)
        {
            if (string.IsNullOrEmpty(Firstname) || string.IsNullOrEmpty(Lastname))
            {
                OnError?.Invoke("ФИО не может быть пустым!");
                return;
            }

            _currentGuest.FirstName = Firstname;
            _currentGuest.MiddleName = Middlename;
            _currentGuest.LastName = Lastname;
            _currentGuest.DateOfBirth = DateOfBirth ?? DateTime.MinValue;
            _currentGuest.PassportNumber = PassportNumber;
            _currentGuest.ContactDetails = ContactDetails;
            _currentGuest.Preferences = Preferences;

            _context.Guests.Update(_currentGuest);
            _context.SaveChanges();

            LoadGuestData();
            OnSuccess?.Invoke("Данные сохранены!");
        }
    }

}
