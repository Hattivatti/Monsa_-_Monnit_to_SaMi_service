using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Win32;
using System.Configuration;

namespace Monsa___Monnit_to_SaMi_service.SaMi
{
    public partial class MainService : ServiceBase
    {

        public MainService()
        {
            InitializeComponent();
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            
            base.OnStart(args);

            Storage._instance.Username = ConfigurationManager.AppSettings["UName"];
            Storage._instance.Password = ConfigurationManager.AppSettings["PWord"];
            Storage._instance.NetworkID = Int32.Parse(ConfigurationManager.AppSettings["NetworkID"]);
            // Set timer interval in minutes
            int TimerInterval = Int32.Parse(ConfigurationManager.AppSettings["MeasurementInterval"]);

            Storage._instance.Key = ConfigurationManager.AppSettings["SaMiKey"];

            Timer ServiceRunner = new Timer();

            ServiceRunner.Interval = 1000 * 60 * TimerInterval;
            ServiceRunner.Elapsed += new ElapsedEventHandler(TimerRun);
            ServiceRunner.Start();

            

        }

        protected override void OnStop()
        {
        }

        private static void TimerRun(object sender, ElapsedEventArgs e)
        {
            
            string LogResult = "";
            string DateString_End = "2000/1/1";
            string DateString_Start = "2000/1/1";


            try
            {

                try
                {
                    Storage._instance.LastMeasurementDate = Newtonsoft.Json.JsonConvert.DeserializeObject<SaMiStatusInfo>(System.IO.File.ReadAllText(ConfigurationManager.AppSettings["JsonConfigPath"])).LatestSavedMeasurement;

                }
                catch (Exception ex)
                {
                    Storage._instance.LastMeasurementDate = DateTime.Now.AddDays(-7);

                }

                //var LastMeasurementDate = Newtonsoft.Json.JsonConvert.DeserializeObject<SaMiStatusInfo>(System.IO.File.ReadAllText(ConfigurationManager.AppSettings["JsonConfigPath"]));

                if (Storage._instance.LastMeasurementDate != DateTime.Now.AddDays(-1))
                {
                    Debug.WriteLine("Diy day");

                    if (Storage._instance.LastMeasurementDate > DateTime.Now.AddDays(-7))
                    {
                        DateString_End = String.Format("{0}/{1}/{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        DateString_Start = String.Format("{0}/{1}/{2}", Storage._instance.LastMeasurementDate.Year, Storage._instance.LastMeasurementDate.Month, Storage._instance.LastMeasurementDate.Day);

                        Debug.WriteLine("Viimeinen mittaus oli viikon sisään.");
                    }
                    else
                    {
                        DateTime dt = DateTime.Now.AddDays(-7);

                        DateString_End = String.Format("{0}/{1}/{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        DateString_Start = String.Format("{0}/{1}/{2}", dt.Year, dt.Month, dt.Day);

                        Debug.WriteLine("Viimeinen mittaus ei ollut viikon sisään.");

                        Storage._instance.LastMeasurementDate = DateTime.Now.AddDays(-7);

                    }

                    ReadFromMonnitAPI RFMA = new ReadFromMonnitAPI();
                    RFMA.GetData(DateString_Start, DateString_End);

                    SendToSaMi STSM = new SendToSaMi();

                    //STSM.TestSend(); //Test function to write monnit a data to a file

                    int Result = STSM.SendData();

                    Storage._instance.MonnitDataList.Clear();

                    Debug.WriteLine("All done");
                }
               
            }
            catch (Exception ex)
            {
                LogResult = ex.ToString();
            }

            string LogData = String.Format("Date: {0}, Row Count: {1}", DateTime.Now.ToString(), LogResult);


            ServiceLogger._instance.WriteToLog(LogData);
        }
    }
}
