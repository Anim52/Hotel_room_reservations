using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Data_Management_Service.ViewsModel
{
    /// <summary>
    /// Базовый класс ViewModel, реализующий интерфейс INotifyPropertyChanged для поддержки привязки данных.
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Событие, вызываемое при изменении значения свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Вызывает событие PropertyChanged для обновления привязанных свойств в View.
        /// </summary>
        /// <param name="propertyName">Имя измененного свойства. Определяется автоматически, если не указано явно.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
