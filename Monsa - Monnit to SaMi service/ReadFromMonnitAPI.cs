using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Diagnostics;

namespace Monsa___Monnit_to_SaMi_service.SaMi
{
    class ReadFromMonnitAPI
    {
        
        public string GetAuthToken()
        {
            string address = String.Format("https://www.imonnit.com/xml/GetAuthToken?username={0}&password={1}", Storage._instance.Username, Storage._instance.Password);
            XDocument doc = XDocument.Load(address);

            Debug.WriteLine(doc.Descendants("Result").Single().Value.ToString());

            return doc.Descendants("Result").Single().Value;

        }

        public void GetData(string StartDate, string EndDate)
        {

            try
            {

                List<int> IDList = new List<int>();
                List<MonnitDataType> DataList = new List<MonnitDataType>();

                string AuthToken = GetAuthToken();

                string address = string.Format("https://www.imonnit.com/xml/SensorList/{0}?NetworkID={1}", AuthToken, Storage._instance.NetworkID);
                XDocument doc = XDocument.Load(address);


                foreach (var data in doc.Descendants("APISensor"))
                {
                    int id = (Convert.ToInt32(data.Attribute("SensorID").Value));
                    IDList.Add(id);
                    Storage._instance.IDList.Add(id);


                }

                foreach (int Id in IDList)
                {

                    address = string.Format("https://www.imonnit.com/xml/SensorDataMessages/{0}?sensorID={1}&fromDate={2}&toDate={3}", AuthToken, Id.ToString(), StartDate, EndDate);
                    doc = XDocument.Load(address);

                    foreach (var data in doc.Descendants("APIDataMessage"))
                    {
                        Debug.WriteLine(data.ToString());
                        MonnitDataType MonnitData = new MonnitDataType(Convert.ToInt32(data.Attribute("SensorID").Value), data.Attribute("MessageDate").Value, data.Attribute("Data").Value, data.Attribute("DataTypes").Value);
                        Storage._instance.MonnitDataList.Add(MonnitData);
                        Debug.WriteLine(MonnitData.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceLogger._instance.WriteToLog(ex.ToString());
            }
        }

    
    }
}
