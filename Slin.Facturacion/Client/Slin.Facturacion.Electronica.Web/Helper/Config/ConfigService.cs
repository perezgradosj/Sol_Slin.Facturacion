using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using SW = System.ServiceProcess;

namespace Slin.Facturacion.Electronica.Web.Helper.Config
{
    public class ConfigService
    {
        static string PathLogSLINADE = ConfigurationManager.AppSettings["PathLogSLINADE"].ToString();
        //static string PathDirectoryError = PathLogSLINADE + DateTime.Now.Year + "_" + DateTime.Now.Month + @"\";

        static string PathDirectoryError = PathLogSLINADE + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\logade\";

        public string Iniciar_Service(string nameService)
        {
            string msje = string.Empty;
            string nombreServicio = nameService;
            SW.ServiceController service = new SW.ServiceController(nombreServicio);
            try
            {
                //int timeoutMilisegundos = 5000;
                //TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilisegundos);
                //service.WaitForStatus(SW.ServiceControllerStatus.Running, timeout);
                CreateDirectory(PathDirectoryError);

                if (service != null && service.Status == SW.ServiceControllerStatus.Stopped)
                {
                    service.Start();

                    using (StreamWriter sw = new StreamWriter(PathDirectoryError + "Seg_logADEService.log", true, Encoding.UTF8))
                    {
                        sw.WriteLine("[ " + DateTime.Now  + " ]" + " El Servicio se " + nameService + " se a Iniciado Correctamente.");
                    }
                    msje = "El Servicio " + nameService + ", se a Iniciado Correctamente.";
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(PathDirectoryError + "Seg_logADEService.log", true, Encoding.UTF8))
                {
                    sw.WriteLine("[ " + DateTime.Now + " ]" + " Error al Iniciar el Sercivio: " + ex.Message + ", " + ex.InnerException);
                }
                msje = "Error al Iniciar el Sercivio: " + ex.Message;
            }            
            return msje;
        }



        public string Detener_Service(string nameService)
        {
            string msje = string.Empty;
            string serviceName = nameService;
            SW.ServiceController service = new SW.ServiceController(serviceName);
            try
            {
                CreateDirectory(PathDirectoryError);
                if (service != null && service.Status == SW.ServiceControllerStatus.Running)
                {
                    service.Stop();
                    using (StreamWriter sw = new StreamWriter(PathDirectoryError + "Seg_logADEService.log", true, Encoding.UTF8))
                    {
                        sw.WriteLine("[ " + DateTime.Now + " ]" + " El Servicio se " + nameService + " se a Detenido Correctamente.");
                    }

                    msje = "El Servicio " + nameService + ", se a Detenido Correctamente.";
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(PathDirectoryError + "Seg_logADEService.log", true, Encoding.UTF8))
                {
                    sw.WriteLine("[ " + DateTime.Now + " ]" + " Error al Detener el Sercivio: " +  ex.Message + ", " + ex.InnerException);
                }
                msje = "Error al Detener el Sercivio: " + ex.Message;
            }
            return msje;
        }

        private void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}