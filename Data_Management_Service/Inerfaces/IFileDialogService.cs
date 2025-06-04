using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Management_Service.Inerfaces
{
    /// <summary>
    /// Интерфейс для службы открытия файлового диалога.
    /// </summary>
    public interface IFileDialogService
    {
        /// <summary>
        /// Открывает диалог выбора файла и возвращает путь к выбранному файлу.
        /// </summary>
        /// <returns>Путь к выбранному файлу</returns>
        string OpenFileDialog();
    }
}
