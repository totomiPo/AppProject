using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllLabs
{
    // static - инициализация 1 раз, живет до конца программы (в сегменте данных)
    // public - возможно использование другими классами или сборками, в целом в программе
    static class Data
    {
        public static string Exct = "";
    }
}
