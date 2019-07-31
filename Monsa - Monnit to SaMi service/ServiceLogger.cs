using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monsa___Monnit_to_SaMi_service.SaMi
{
    class ServiceLogger
    {
        public static readonly ServiceLogger _instance = new ServiceLogger();

        public void WriteToLog(string LogData)
        {
            System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "Monsa.log", LogData + Environment.NewLine);
        }     
    }
}
