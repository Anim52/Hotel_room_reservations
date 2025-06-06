# Бронирование номеров в отелe

![Edge](https://img.shields.io/badge/Edge-0078D7?style=for-the-badge&logo=Microsoft-edge&logoColor=white)
	![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
 ![AMD](https://img.shields.io/badge/AMD-%23000000.svg?style=for-the-badge&logo=amd&logoColor=white)
 ![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)
![Windows 11](https://img.shields.io/badge/Windows%2011-%230079d5.svg?style=for-the-badge&logo=Windows%2011&logoColor=white)
![ChatGPT](https://img.shields.io/badge/chatGPT-74aa9c?style=for-the-badge&logo=openai&logoColor=white)

Бронирование номеров в отелe — это настольное приложение на платформе Windows, разработанное с использованием WPF (Windows Presentation Foundation) и языка программирования C#. Оно предназначено для автоматизации повседневных операций в отелях и гостиницах: от управления номерами и клиентами до ведения истории бронирований и анализа загруженности.

Приложение имеет интуитивно понятный графический интерфейс, реализованную логику по паттерну MVVM, и взаимодействует с базой данных через Entity Framework Core, обеспечивая надёжное хранение данных.

# 🔧 Основной функционал

🛏 Управление номерами

Добавление, редактирование и удаление номеров

Назначение типа (обычный, люкс и т.д)

Отображение текущей занятости

📅 Бронирования
Создание нового бронирования с выбором даты заезда/выезда

Проверка доступности номера в указанный период

Отображение активных и завершённых бронирований

👤 Работа с клиентами

Добавление новых клиентов с контактной информацией

Поиск по имени или телефону

Просмотр истории бронирований конкретного клиента

📈 Дополнительно
Экспорт данных о бронированиях (PDF)

# 🧩 Технологический стек
![image](https://github.com/user-attachments/assets/5228a30f-25fd-4e06-a91c-5611dc4ae293)

# 🧭 Архитектура приложения
Проект реализован в соответствии с архитектурным паттерном MVVM (Model-View-ViewModel), обеспечивая разделение логики и представления, что упрощает поддержку и тестирование кода.

Слои:

Models — бизнес-сущности

ViewModels — логика и команды, привязанные к UI

Views — XAML-интерфейсы для отображения данных

Services — работа с базой данных и логикой приложения

DataContext — взаимодействие с EF Core

# Erd Диаграмма базы данных


![image](https://github.com/user-attachments/assets/70744d83-d503-4771-ae90-4775b5179473)


# XML

[Xml for ApplicationApp](https://github.com/Anim52/Hotel_room_reservations/blob/Debug/AplicationApp.xml)

[Xml for Data Manegement Service](https://github.com/Anim52/Hotel_room_reservations/blob/Debug/Data_Management_Service.xml)

[Xml for Database](https://github.com/Anim52/Hotel_room_reservations/blob/Debug/Database.xml)
# 🧩 Структура проекта
 <pre lang="markdown">
Hotel room reservations/
├── AplicationApp/
│   ├── Assets/
│   │   ├── Resources/
│   │   │   ├── Fonts.xaml
│   │   │   ├── Logo.xaml
│   │   │   └── Style.xaml
│   ├── Views/
│   │   ├── Pages/
│   │   │   ├── AccountPage.xaml
│   │   │   ├── AdminServicePage.xaml
│   │   │   ├── GuestPage.xaml
│   │   │   ├── NumberPage.xaml
│   │   │   ├── ReservationPage.xaml
│   │   │   ├── ServicePage.xaml
│   │   │   └── UserViewReservation.xaml
│   │   └── Windows/
│   │       ├── AutorizationView.xaml
│   │       ├── EditNomerView.xaml
│   │       ├── MainWindow.xaml
│   │       ├── NumberAdd.xaml
│   │       ├── RegistrationView.xaml
│   │       └── UserMainWindow.xaml
│   ├── App.xaml
│   └── AssemblyInfo.cs
├── Data_Management_Service/
│   ├── Command/
│   ├── Interfaces/
│   ├── OtherViews/
│   ├── PageViewModel/
│   └── ViewsModel/
├── Database/
│   ├── Context/
│   ├── Migrations/
│   └── Service/

 </pre>
# 📈 Будущие улучшения
Ролевая модель (админ / ресепшен / гость)

Онлайн-резервирование через API

Подключение к облачной базе

Генерация отчетов и аналитика

Поддержка нескольких языков (локализация)
