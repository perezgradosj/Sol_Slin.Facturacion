using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace Slin.Facturacion.Electronica.Web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Session["CurrentError"] = "Global: " + Server.GetLastError().Message;
        }


        void Application_Error(object sender, EventArgs e)
        {
            try
            {
                Session["CurrentError"] = "Global: " + Server.GetLastError().Message;
                Server.Transfer("../../ErrorPage/ErrorPage.aspx");
            }
            catch (Exception ex)
            {
                try
                {
                    Server.Transfer("../../ErrorPage/ErrorPage.aspx");
                }
                catch (Exception exx)
                {
                    Response.Redirect("~/ErrorPage/ErrorPage.aspx");
                }
            }
            
        }


    }
}