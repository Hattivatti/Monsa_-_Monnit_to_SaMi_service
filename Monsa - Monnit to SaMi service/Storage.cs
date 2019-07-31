using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monsa___Monnit_to_SaMi_service.SaMi
{
    class Storage
    {
        public static readonly Storage _instance = new Storage();

        public string Username { get; set; }
        public string Password { get; set; }
        public string DatabaseUsername { get; set; }
        public string DatabasePassword { get; set; }
        public string Key { get; set; }
        public int NetworkID { get; set; }
        public List<MonnitDataType> MonnitDataList = new List<MonnitDataType>();
        public List<int> IDList = new List<int>();
        public DateTime LastMeasurementDate { get; set; }
        

    }
}
