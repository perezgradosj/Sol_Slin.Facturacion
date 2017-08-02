using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Slin.Facturacion.Common
{
    public static class BasePage
    {
        static string PagLogin = ConfigurationManager.AppSettings["PagLogin"].ToString();
        public static void RegisterRedirectOnSessionEndScript(this Page page)
        {
            /// Login Page, We can retrieve for configuration file (Web.Config)
            /// 



            //Session.RemoveAll();
            //Default.SessionActiva = false;

            //Default.valorsession = string.Empty;
            //Default.valorsession = null;
            //Session.Remove("UsuarioExisteFact");

            //Session.Abandon();
            //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));


            //string loginPage = "../../Default.aspx"; //REVISAR SI AQUI FUNCA O NO
            //string loginPage = "../../Login.aspx";

            string loginPage = "~/" + PagLogin + ".aspx";
            /// Session Timeout (Default 20 minutes)
            int sessionTimeout = HttpContext.Current.Session.Timeout;
            /// Timeout for Redirect to Login Page (10 milliseconds before)
            int redirectTimeout = (sessionTimeout * 60000) - 10;

            /// JavaScript Code
            StringBuilder javascript = new StringBuilder();
            javascript.Append("var redirectTimeout;");
            javascript.Append("clearTimeout(redirectTimeout);");
            javascript.Append(String.Format("setTimeout(\"window.location.href='{0}'\",{1});", loginPage, redirectTimeout));

            /// Register JavaScript Code on WebPage
            page.ClientScript.RegisterStartupScript(page.GetType(),
                                                    "RegisterRedirectOnSessionEndScript",
                                                    javascript.ToString(),
                                                    true);
        }
    }
}
