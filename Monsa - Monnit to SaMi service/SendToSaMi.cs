using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Globalization;

namespace Monsa___Monnit_to_SaMi_service.SaMi
{
    class SendToSaMi
    {
        MeasurementsServiceClient Client = new MeasurementsServiceClient();
        MeasurementPackage Package = new MeasurementPackage();
        List<MeasurementModel> All_measurements = new List<MeasurementModel>();


    public void TestSend()
        {

            if (Storage._instance.MonnitDataList.Count > 0)
            {
                foreach (MonnitDataType Monnit in Storage._instance.MonnitDataList)
                {
                    System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "TEST.txt", Monnit.ToString() + Environment.NewLine);

                    SaMiStatusInfo STI = new SaMiStatusInfo();

                    MonnitDataType MDT = Storage._instance.MonnitDataList.First();
                    STI.LatestSavedMeasurement = ParseMonnitDateStringToIso(MDT.Date);

                    System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "Config.json", Newtonsoft.Json.JsonConvert.SerializeObject(STI));
                }
            }
            else
            {
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "TEST.txt", "TYHJÄ" + Environment.NewLine);
            }
        }

        public void SortMonnitDataList()
        {
            //Storage._instance.MonnitDataList = Storage._instance.MonnitDataList.OrderByDescending(d => d.Date).ToList();
            //Storage._instance.MonnitDataList.Reverse();

        }

        public int SendData()
        {


            SaMiStatusInfo STI = new SaMiStatusInfo();

            foreach (MonnitDataType Monnit in Storage._instance.MonnitDataList)
            {

                if (Monnit.Data != null || Monnit.Data != "")
                {
                    MeasurementModel SensorDataset = new MeasurementModel();
                    List<DataModel> Sensors = new List<DataModel>();

                    DataModel sensorValue = new DataModel();
                    sensorValue.Tag = Monnit.SensorID.ToString();
                    sensorValue.Value = Double.Parse(Monnit.Data, System.Globalization.CultureInfo.InvariantCulture);

                    Sensors.Add(sensorValue);

                    SensorDataset.Data = Sensors.ToArray();
                    SensorDataset.Object = "Betonilabra";
                    SensorDataset.Timestamp = ParseMonnitDateStringToIso(Monnit.Date);
                    SensorDataset.Tag = Monnit.SensorID.ToString();

                    All_measurements.Add(SensorDataset);
                }

            }

            MonnitDataType MDT = Storage._instance.MonnitDataList.First();
            STI.LatestSavedMeasurement = ParseMonnitDateStringToIso(MDT.Date);

            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "Config.json", Newtonsoft.Json.JsonConvert.SerializeObject(STI));

            Package.Measurements = All_measurements.ToArray();

            Package.Key = Storage._instance.Key;
            //var result = Client.SaveMeasurementPackage(Package);

            Client.Close();

            return 0;
        }

        public DateTime ParseMonnitDateStringToIso(string DateString)
        {

            System.Globalization.CultureInfo cultureinfo =
            new System.Globalization.CultureInfo("en-US");
            DateTime dt = DateTime.Parse(DateString, cultureinfo);


            return dt;
        }
    }
}
 