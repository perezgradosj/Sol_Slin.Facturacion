using Slin.Facturacion.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;

namespace Slin.Facturacion.Electronica.Web.Views.Util
{


    [Export(typeof(IServiceWebFacController))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ServiceWebFactController : System.Web.UI.Page, IServiceWebFacController
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        #region for session

        public WCFSeguridad.ListaRol GetList_RolUserLog()
        {
            var list = new WCFSeguridad.ListaRol();
            list = (WCFSeguridad.ListaRol)Session["RolesUsuarioLogeado"];
            return list;
        }

        public int Valida_MainSession(string url)
        {
            int result = Constantes.ValorCero;
            try
            {
                List<string> list = new List<string>();
                var oListaMenu = (WCFSeguridad.ListaMenu)Session["SessionListMenuUserLog"];
                foreach (var obj in oListaMenu)
                { list.Add(obj.CodigoMenu); }

                if (list.Contains(url))
                { result = Constantes.ValorUno; }
                else { result = Constantes.ValorCero; }
            }
            catch (Exception ex) { result = Constantes.ValorCero; }
            return result;
        }

        public int Process_UrlPage(string Url)
        {
            int result = Constantes.ValorCero;
            try
            {
                string[] array = Url.Split('/');
                int size = array.Length;
                string uri = array[size - 1];
                result = Valida_MainSession(uri);

                //string[] array = Url.Split('/');
                //int size = array.Length;
                //string uri = array[size - 1];
                //var result = objController.Valida_MainSession(uri);
                //if (result == Constantes.ValorCero)
                //{
                //    Response.Redirect("~/Views/Home/Inicio.aspx");
                //}
            }
            catch (Exception ex)
            { result = Constantes.ValorCero; }
            return result;
        }


        #endregion

        #region user login

        public WCFSeguridad.Usuario GetUserLogueado_Fact()
        {
            WCFSeguridad.Usuario ouser = new WCFSeguridad.Usuario();
            ouser = (WCFSeguridad.Usuario)Session["UsuarioLogueadoFact"];
            return ouser;
        }

        #endregion


        #region list



        #endregion
    }


    public interface IServiceWebFacController
    {

    }
}