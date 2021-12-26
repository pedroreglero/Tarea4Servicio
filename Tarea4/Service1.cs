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

namespace Tarea4
{
    public partial class Service1 : ServiceBase
    {
        Timer myTimer;
        EventLog myEventLog;
        public Service1(string[] args)
        {
            InitializeComponent();
            string eventSourceName = "MySource";
            string logName = "MyNewLog";

            if (args.Length > 0)
                eventSourceName = args[0];
            if (args.Length > 1)
                logName = args[1];

            myEventLog = new EventLog();
            if (!EventLog.SourceExists(eventSourceName))
                EventLog.CreateEventSource(eventSourceName, logName);

            myEventLog.Source = eventSourceName;
            myEventLog.Log = logName;
        }

        protected override void OnStart(string[] args)
        {
            myTimer = new Timer(1500);
            myTimer.Elapsed += (sender, e) =>
            {
                myTimer.Enabled = false;
                com.w3schools.www.TempConvert WS = new com.w3schools.www.TempConvert();
                var cronometro = new Stopwatch();
                try
                {
                    // medimos el tiempo que tarda en realizarse la petición
                    cronometro.Start();
                    string res = WS.CelsiusToFahrenheit("22");
                    cronometro.Stop();
                    if (cronometro.Elapsed.TotalSeconds > 10)
                        myEventLog.WriteEntry($"La petición web ha sido lenta con {cronometro.Elapsed.TotalSeconds.ToString()} segundos");
                }
                catch (Exception ex)
                {
                    cronometro.Stop();
                    myEventLog.WriteEntry("Error al realizar la petición web");
                }
                myTimer.Enabled = true;
            };
            myTimer.Enabled = true;
        }

        protected override void OnStop()
        {
        }
    }
}
