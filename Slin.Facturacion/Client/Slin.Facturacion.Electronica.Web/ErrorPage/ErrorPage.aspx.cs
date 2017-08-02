using Slin.Facturacion.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;

namespace Slin.Facturacion.Electronica.Web.ErrorPage
{
    public partial class ErrorPage : System.Web.UI.Page
    {

        private string PathLogSLINADE = ConfigurationManager.AppSettings["PathLogSLINADE"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                Cargar();
            }
        }

        #region ENTITY
        WCFSeguridad.Usuario oUsuario = new WCFSeguridad.Usuario();

        #endregion


        #region METHOD

        void Cargar()
        {
            Captura_Error();
            var result = ObtenerDatos(); //descomentar

            if (result == false)
            {
                txtuser.InnerText = "UserError";
            }
            else
            {
                txtuser.InnerText = (oUsuario.Username.Length == Constantes.ValorCero ? string.Empty : oUsuario.Username) + " - " + (oUsuario.Empleado.Nombres.Length == Constantes.ValorCero ? string.Empty : oUsuario.Empleado.Nombres);
            }
        }

        private bool ObtenerDatos()
        {
            try
            {
                oUsuario = new WCFSeguridad.Usuario();
                oUsuario = (WCFSeguridad.Usuario)Session["UsuarioLogueadoFact"];
                return true;
            }
            catch (Exception ex) { return false; }
            
        }
        //METHOD PARA CAPTURAR EL ERROR


        void Captura_Error()
        {
            System.Text.StringBuilder errMessage = new System.Text.StringBuilder();
            System.Exception appException = Server.GetLastError();

            if (appException is HttpException)
            {
                HttpException checkException = (HttpException)appException;
                switch (checkException.GetHashCode())
                {
                    case 403:
                        {
                            errMessage.Append("No se le permite ver a esa página.");
                            break;
                        }
                    case 404:
                        {
                            errMessage.Append("La página solicitada no se encuentra.");
                            break;
                        }
                    case 408:
                        {
                            errMessage.Append("Se ha agotado el tiempo para la solicitud");
                            break;
                        }
                    case 500:
                        {
                            errMessage.Append("El servidor no puede cumplir con su solicitud.");
                            break;
                        }
                    default:
                        {
                            errMessage.Append("El servidor ha presentado un error.");
                            break;
                        }
                }
            }
            else
            {
                //The exception was not a httpexception
                try
                {
                    errMessage.AppendFormat("Se ha producido el siguiente error<br />{0}", appException.ToString());
                }
                catch (Exception ex) { }
            }

            errMessage.Append("\n Por favor, póngase en contacto con el administrador del servidor.");
            lblmensaje.InnerText = errMessage.ToString();

            Server.ClearError();
        }


        #endregion


        protected void CerrarSesion_ServerClick(object sender, EventArgs e)
        {
            //Default.valorsession = string.Empty;
            Login.valorsession = string.Empty;
            Session.Remove("UsuarioExisteFact");
            Session.Abandon();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            Response.Redirect("~/Default");
        }
    }
}