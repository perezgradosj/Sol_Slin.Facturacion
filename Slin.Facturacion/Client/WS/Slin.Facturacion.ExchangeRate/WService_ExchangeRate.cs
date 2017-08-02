using Slin.Facturacion.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
//using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Slin.Facturacion.ExchangeRate
{
    partial class WService_ExchangeRate : ServiceBase
    {
        string Intervale = ConfigurationManager.AppSettings["Intervale"].ToString();
        public static string PathLogSLINADE = ConfigurationManager.AppSettings["PathLogSLINADE"].ToString();
        //string pathlog = PathLogSLINADE + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\";
        string pathlog = PathLogSLINADE + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\ser";

        #region entity

        public Timer time;

        #endregion
        public WService_ExchangeRate()
        {
            InitializeComponent();

            CreateDirectory(pathlog);

            try
            {
                if (Intervale != null && Intervale.Length > 0)
                {
                    time = new Timer();
                    time.Interval = (int.Parse(Intervale) * 60) * 1000;
                    time.Elapsed += new ElapsedEventHandler(Time_Elapsed_ChangeType);
                }
                else
                {
                    time = new Timer();
                    //time.Interval = 600000;
                    //time.Interval = 1200000;
                    time.Interval = 82800000;
                    time.Elapsed += new ElapsedEventHandler(Time_Elapsed_ChangeType);
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(pathlog + "WS_ExchangeRate_Lisa.log", true, Encoding.UTF8))
                {
                    sw.WriteLine("[" + DateTime.Now + "] ---------------------------------------------------------------------");
                    sw.WriteLine("[" + DateTime.Now + "] Msje: " + ex.Message);
                    sw.WriteLine("[" + DateTime.Now + "] ---------------------------------------------------------------------");
                }
            }
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.

            CreateDirectory(pathlog);

            time.Enabled = true;

            using (StreamWriter sw = new StreamWriter(pathlog + "WS_ExchangeRate.log", true, Encoding.UTF8))
            {
                decimal cant_hours = int.Parse(Intervale) / 60;
                sw.WriteLine("[" + DateTime.Now + "] -------------------------------INICIO--------------------------------");
                sw.WriteLine("[" + DateTime.Now + "] El Servicio de Tipo de Cambio se a iniciado Correctamente.");
                //sw.WriteLine("[" + DateTime.Now + "] Se revisará el correo cada " + (int.Parse(Intervale) == Constantes.ValorCero ? (Constantes.ValorVeinte + string.Empty) : Intervale) + " minutos.");
                sw.WriteLine("[" + DateTime.Now + "] Se verificará el Tipo de cambio cada " + (cant_hours < Constantes.ValorUno ? (23 + string.Empty) : Intervale) + " horas.");
                sw.WriteLine("[" + DateTime.Now + "] --------------------------------FIN----------------------------------");
            }
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.

            CreateDirectory(pathlog);

            using (StreamWriter sw = new StreamWriter(pathlog + "WS_ExchangeRate.log", true, Encoding.UTF8))
            {
                sw.WriteLine("[" + DateTime.Now + "] -------------------------------INICIO--------------------------------");
                sw.WriteLine("[" + DateTime.Now + "] El Servicio de Tipo de Cambio se a detenido Correctamente.");
                sw.WriteLine("[" + DateTime.Now + "] --------------------------------FIN----------------------------------");
            }
        }


        private void CreateDirectory(string pathDirectory)
        {
            if (!Directory.Exists(pathDirectory))
            {
                Directory.CreateDirectory(pathDirectory);
            }
        }

        private void Time_Elapsed_ChangeType(object sender, EventArgs e)
        {
            CreateDirectory(pathlog);



            try
            {
                new ProcessClass().Process_WithCondition();
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(pathlog + "WS_ExchangeRate.log", true, Encoding.UTF8))
                {
                    sw.WriteLine("[" + DateTime.Now + "] -------------------------------INICIO--------------------------------");
                    sw.WriteLine("[" + DateTime.Now + "] Msje: " + ex.Message);
                    sw.WriteLine("[" + DateTime.Now + "] --------------------------------FIN----------------------------------");
                }
            }
        }
    }
}
