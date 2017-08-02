using Slin.Facturacion.Electronica.Web.Views.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;

namespace Slin.Facturacion.Electronica.Web.Views.Home
{
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                Cargar();
            }
        }


        #region ENTITY

        ServiceWebFactController objController = new ServiceWebFactController();

        private WCFSeguridad.Usuario ousuarioLogeado;
        public WCFSeguridad.Usuario oUsuarioLogeado
        {
            get { return ousuarioLogeado; }
            set { ousuarioLogeado = value; }
        }

        #endregion


        #region METHOD

        void Cargar()
        {
            try
            {
                ObtenerUsuarioLogeado();
                logoEmpresa.Src = "../../Img/home/" + oUsuarioLogeado.Empresa.RUC + ".png";
                lblempresa.InnerText = oUsuarioLogeado.Empresa.RazonSocial.ToUpper();
            }
            catch (Exception ex)
            {

            }
            
        }

        void ObtenerUsuarioLogeado()
        {
            try
            {
                oUsuarioLogeado = objController.GetUserLogueado_Fact();

                txtusuario.Value = oUsuarioLogeado.Username;
                txtnombres.Value = oUsuarioLogeado.Empleado.Nombres;
                txtapellidos.Value = oUsuarioLogeado.Empleado.ApePaterno;
                txtdni.Value = oUsuarioLogeado.Empleado.DNI;
                txtdireccion.Value = oUsuarioLogeado.Empleado.Direccion;
                txtemail.Value = oUsuarioLogeado.Empleado.Email;
                txttelefono.Value = oUsuarioLogeado.Empleado.Telefono;
                txtruc.Value = oUsuarioLogeado.Empresa.RUC;
                txtempresa.Value = oUsuarioLogeado.Empresa.RazonSocial;
            }
            catch (Exception ex)
            {

            }
            
        }

        #endregion
    }
}