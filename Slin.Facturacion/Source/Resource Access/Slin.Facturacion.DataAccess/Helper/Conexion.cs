using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slin.Facturacion.DataAccess.Helper
{
    public class Conexion
    {
        public static string cnsSql = "cnsFacturacion";

        public static string PathLogSLINADE = ConfigurationManager.AppSettings["PathLogSLINADE"].ToString();
        public static string Cadena = PathLogSLINADE + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\logade\";

        public static string Cadena_Nl = PathLogSLINADE + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\notlic\";

        public static string Host = ConfigurationManager.AppSettings["Host"].ToString();
        public static string BD = ConfigurationManager.AppSettings["BD"].ToString();
        public static string USER = new Helper.Encrypt().DecryptKey(ConfigurationManager.AppSettings["USER"].ToString());
        public static string PWD = new Helper.Encrypt().DecryptKey(ConfigurationManager.AppSettings["PWD"].ToString());

        //static string cadena = "Server=" + Host + ";Database=" + BD + ";User=" + USER + ";pwd=" + PWD;
    }
}
