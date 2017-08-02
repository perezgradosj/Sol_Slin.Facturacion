using Slin.Facturacion.Common;
using Slin.Facturacion.ServiceImplementation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using static Slin.Facturacion.ExchangeRate.ExchangeRateClass;

namespace Slin.Facturacion.ExchangeRate
{
    public class ProcessClass
    {
        public static string PathLogSLINADE = ConfigurationManager.AppSettings["PathLogSLINADE"].ToString();
        public static string NameWS = ConfigurationManager.AppSettings["NameWS"].ToString();

        string pathlog = PathLogSLINADE + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month)  + @"\" + NameWS;

        public string Process_WithCondition()
        {
            //var result = new ServiceLisaConnectionSOA().Update_ccdcc_ChangeStatus();
            //Get_ChangeType_Sunat();

            string msj = string.Empty;

            CreateDirectory(pathlog);
            
            try
            {
                var mounth = DateTime.Now.Month;
                var year = DateTime.Now.Year;
                var listReturn = new ExchangeRateClass().GetList_ChangeType(mounth + string.Empty, year + string.Empty);

                if (listReturn.Count <= 1)
                {
                    if (mounth == 1)
                    {
                        mounth = 12;
                        year--;
                    }
                    else { mounth--; }
                    listReturn = new ExchangeRateClass().GetList_ChangeType(mounth + string.Empty, year + string.Empty);
                }
                int CantItems = listReturn.Count;

                var DayValue = listReturn[CantItems - 3];
                var ChangeType_purchase = listReturn[CantItems - 2];
                var ChangeType_Sale = listReturn[CantItems - 1];

                var result = "Dia Tipo Cambio: " + DayValue + ", Compra: " + ChangeType_purchase + ", Venta: " + ChangeType_Sale;

                Obj_ExchangeRate = new BusinessEntities.ExchangeRate();




                Obj_ExchangeRate.fech = DateTime.Now.Date;
                Obj_ExchangeRate.fech_str = DateTime.Now.ToString("dd-MM-yyyy");
                Obj_ExchangeRate.value = Convert.ToDecimal(ChangeType_Sale, CultureInfo.CreateSpecificCulture("es-PE"));


                var msjeResult = new ServicioFacturacionSOA().Insert_ExchangeRate_Value(Obj_ExchangeRate);

                if (msjeResult == Constantes.msjRegistrado)
                {
                    msj = "Se a Ingresado el tipo de cambio para el dia " + DateTime.Now.ToString("dd / MM / yyyy") + " TP: " + Obj_ExchangeRate.value;
                    using (StreamWriter sw = new StreamWriter(pathlog + @"\ExchangeRate_BD.log", true, Encoding.UTF8))
                    {
                        sw.WriteLine("[" + DateTime.Now + "] ---------------------------------------------------------------------");
                        sw.WriteLine("[" + DateTime.Now + "] Se a Ingresado el tipo de cambio para el dia " + DateTime.Now.ToString("dd/MM/yyyy") + " TP: " + Obj_ExchangeRate.value);
                        sw.WriteLine("[" + DateTime.Now + "] ---------------------------------------------------------------------");
                    }
                }
            }
            catch (Exception ex)
            {
                msj = "Msje: " + ex.Message;
                using (StreamWriter sw = new StreamWriter(pathlog + @"\ExchangeRate_BD.log", true, Encoding.UTF8))
                {
                    sw.WriteLine("[" + DateTime.Now + "] ---------------------------------------------------------------------");
                    sw.WriteLine("[" + DateTime.Now + "] Msje: " + ex.Message);
                    sw.WriteLine("[" + DateTime.Now + "] ---------------------------------------------------------------------");
                }
            }
            return msj;
        }


        #region

        private BusinessEntities.ExchangeRate obj_exchangerate;
        public BusinessEntities.ExchangeRate Obj_ExchangeRate
        {
            get { return obj_exchangerate; }
            set
            {
                obj_exchangerate = value;
            }
        }


        #endregion

        #region method

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
