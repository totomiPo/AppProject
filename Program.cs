using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AllLabs
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        /// Все потоки выполняются в рамках одного процесса 
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            /// Для отображения текста графический класс на основе GDI 
            Application.SetCompatibleTextRenderingDefault(false);
            /// Запуск стартовой формы
            Application.Run(new Form1());
        }
    }
}
