using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monsa___Monnit_to_SaMi_service.SaMi
{
    class MonnitDataType
    {
        public int SensorID { get; set; }
        public string Date { get; set; }
        public string Data { get; set; }
        public string DataType { get; set; }


        public MonnitDataType(int SensorID, string Date, string Data, string DataType)
        {
            this.SensorID = SensorID;
            this.Date = Date;
            this.Data = Data;
            this.DataType = DataType;
        }

        override public string ToString()
        {
            return String.Format("ID: {0}, Date: {1}, Data: {2}, DataType {3}", this.SensorID, this.Date, this.Data, this.DataType);
        }
    }
}
