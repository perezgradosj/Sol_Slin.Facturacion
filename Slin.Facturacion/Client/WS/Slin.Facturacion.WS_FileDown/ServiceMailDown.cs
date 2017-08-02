using Slin.Facturacion.BusinessEntities.Configuracion;
using Slin.Facturacion.Common;
using Slin.Facturacion.ServiceImplementation;
using Slin.Facturacion.WS_FileDown.Proccess;
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
using System.Threading.Tasks;
using System.Timers;

namespace Slin.Facturacion.WS_FileDown
{
    partial class ServiceMailDown : ServiceBase
    {
        public static string PathLogSLINADE = ConfigurationManager.AppSettings["PathLogSLINADE"].ToString();
        //public string PathProccessExe = ConfigurationManager.AppSettings["PathProccessExe"].ToString();
        public string NameService = ConfigurationManager.AppSettings["NameService"].ToString();


        string pathlog = PathLogSLINADE + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\smd\";



        #region ENTITY

        string entityIdRucDesencrypt = string.Empty;// get RucEntity desencrypt



        //public Services objservice;
        //public Services objService
        //{
        //    get { return objservice; }
        //    set { objservice = value; }
        //}


        public ListService objlistservices;
        public ListService ObjListServices
        {
            get { return objlistservices; }
            set { objlistservices = value; }
        }

        #endregion


        public Timer time;
        public int Intervale = Constantes.ValorCero;

        public ServiceMailDown()
        {
            InitializeComponent();


            CreateDirectory(pathlog);

            try
            {
                //entityIdRucDesencrypt = new Helper.Encrypt().DecryptKey(entityIdRucEncrypt);
                //entityIdRucDesencrypt = new Helper.Encrypt().DecryptKey(new Slin.Facturacion.BusinessSecurity.Entity.EntityClass().EntityId);
                Intervale = Constantes.ValorCero;
                ObjListServices = new ListService();
                ObjListServices = new ServicioConfiguracionSOA().GetTimeServicceForExeProccessMD(NameService);

                if(ObjListServices.Count > 0)
                {
                    if (ObjListServices[0].IntervaleValue.Length > 0)
                    {
                        Intervale = int.Parse(ObjListServices[0].IntervaleValue);
                        time = new Timer();
                        time.Interval = (int.Parse(ObjListServices[0].IntervaleValue) * 60) * 1000;
                        time.Elapsed += new ElapsedEventHandler(Time_Elapsed);
                    }
                    else
                    {
                        Intervale = Constantes.ValorVeinte;
                        time = new Timer();
                        //time.Interval = 600000;
                        time.Interval = 1200000;
                        time.Elapsed += new ElapsedEventHandler(Time_Elapsed);
                    }
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(pathlog + "ADEDownloadService.log", true, Encoding.UTF8))
                    {
                        sw.WriteLine("[" + DateTime.Now + "] --------------------------------------------------------------------");
                        sw.WriteLine("[" + DateTime.Now + "] Datos incorrectos, el nombre del Servicio es incorrecto: " + NameService);
                        sw.WriteLine("[" + DateTime.Now + "] ---------------------------------------------------------------------");
                    }
                }

                //if (objService.Empresa != null && objService.Empresa.RUC != null && objService.Empresa.RUC.Length == Constantes.ValorOnce)
                //{
                    
                //}
                //else
                //{
                    
                //}
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(pathlog + "ADEDownloadService.log", true, Encoding.UTF8))
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

            using (StreamWriter sw = new StreamWriter(pathlog + "ADEDownloadService.log", true, Encoding.UTF8))
            {
                sw.WriteLine("[" + DateTime.Now + "] -------------------------------INICIO--------------------------------");
                sw.WriteLine("[" + DateTime.Now + "] El Servicio de descarga de archivos se a iniciado Correctamente.");
                foreach (var obj in ObjListServices)
                {
                    sw.WriteLine("[" + DateTime.Now + "] Empresas a monitorear : " + obj.Empresa.RUC);
                }
                sw.WriteLine("[" + DateTime.Now + "] Se va a monitorear el correo cada " + (Intervale == Constantes.ValorCero ? Constantes.ValorVeinte + string.Empty : Intervale + " minutos."));
                sw.WriteLine("[" + DateTime.Now + "] --------------------------------FIN----------------------------------");
            }

            
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.

            CreateDirectory(pathlog);

            using (StreamWriter sw = new StreamWriter(pathlog + "ADEDownloadService.log", true, Encoding.UTF8))
            {
                sw.WriteLine("[" + DateTime.Now + "] ---------------------------------------------------------------------");
                sw.WriteLine("[" + DateTime.Now + "] El Servicio de descarga de archivos se a detenido Correctamente.");
                sw.WriteLine("[" + DateTime.Now + "] ---------------------------------------------------------------------");
            }
        }


        public void Time_Elapsed(object sender, EventArgs e)
        {
            CreateDirectory(pathlog);
            try
            {
                //Process p = new Process();
                //ProcessStartInfo psi = new ProcessStartInfo(PathProccessExe);
                //psi.Arguments = "/c \"first.exe -a -b -c | second.exe\"";
                //p.StartInfo = psi;
                //p.Start();

                //new CheckMail().getCredentialMail(entityIdRucEncrypt);
                new CheckMail().getCredentialMail();
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(pathlog + "ADEDownloadService.log", true, Encoding.UTF8))
                {
                    sw.WriteLine("[" + DateTime.Now + "] ---------------------------------------------------------------------");
                    sw.WriteLine("[" + DateTime.Now + "] Msje: " + ex.Message);
                    sw.WriteLine("[" + DateTime.Now + "] ---------------------------------------------------------------------");
                }
            }
        }



        #region CREATE DIRECTORY

        private void CreateDirectory(string pathDirectory)
        {
            if (!Directory.Exists(pathDirectory))
            {
                Directory.CreateDirectory(pathDirectory);
            }
        }

        #endregion
    }
}
