using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Data_Management_Service.Command
{
    /// <summary>
    /// Реализация команды для MVVM.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;
        private event EventHandler _canExecuteChanged;
        /// <summary>
        /// Инициализирует новый экземпляр RelayCommand.
        /// </summary>
        /// <param name="execute">Действие, выполняемое командой</param>
        /// <param name="canExecute">Условие, при котором команда может выполняться</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }
        /// <summary>
        /// Выполняет команду.
        /// </summary>
        /// <param name="parameter">Параметр команды</param>
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        /// <summary>
        /// Событие, вызываемое при изменении возможности выполнения команды.
        /// </summary>
        public void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add => _canExecuteChanged += value;
            remove => _canExecuteChanged -= value;
        }
    }
}
