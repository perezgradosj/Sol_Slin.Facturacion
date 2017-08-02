using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Slin.Facturacion.Common;
using Slin.Facturacion.Electronica.Web.ServiceFacturacion;
using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;
using Slin.Facturacion.Electronica.Web.Views.Util;

namespace Slin.Facturacion.Electronica.Web.Views.Seguridad
{
    public partial class CambiarClave : System.Web.UI.Page
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

        private WCFSeguridad.Usuario ousuario2;
        public WCFSeguridad.Usuario oUsuario2
        {
            get { return ousuario2; }
            set { ousuario2 = value; }
        }




        public string ClaveDesencryptada;
        #endregion


        #region METHOD

        void Cargar()
        {
            try
            {
                ObtenerUsuarioLogeado();
                lblempresa.InnerText = oUsuarioLogeado.Empresa.RazonSocial.ToUpper();
                logoEmpresa.Src = "../../Img/home/" + oUsuarioLogeado.Empresa.RUC + ".png";
            }
            catch (Exception ex)
            {

            }
            
        }

        private void ObtenerUsuarioLogeado()
        {
            try
            {
                oUsuarioLogeado = objController.GetUserLogueado_Fact();
            }
            catch (Exception ex) { }
        }

        void LlenarObjetoUsuario()
        {
            try
            {
                ObtenerUsuarioLogeado();

                oUsuario2 = new WCFSeguridad.Usuario();
                oUsuario2.IdUsuario = oUsuarioLogeado.IdUsuario;
                oUsuario2.Password = oUsuarioLogeado.Password;
                //oUsuario2.NuevoPassword = new Helper.Encrypt().EncryptKey(txtnuevaclave.Value);
                oUsuario2.NuevoPassword = new Helper.Encrypt().HashPassword(txtnuevaclave.Value);
            }
            catch (Exception ex)
            {

            }
            
        }

        void CambiarContrasenia()
        {
            try
            {
                ObtenerUsuarioLogeado();

                //ClaveDesencryptada = new Helper.Encrypt().DecryptKey(oUsuarioLogeado.Password);

                //if (ClaveDesencryptada == txtantiguaclave.Value)
                if(new Helper.Encrypt().HashPassword(txtantiguaclave.Value) == oUsuarioLogeado.Password)
                {
                    if (txtnuevaclave.Value == txtconfirmarclave.Value)
                    {
                        LlenarObjetoUsuario();

                        string msj = ServiceSeguridadController.Instance.ActualizarContrasenia(oUsuario2);

                        Session["MensajeCambiarClave"] = msj;

                        //Response.Write("<script language=javascript>alert('"+ msj+"');</script>");

                        Response.Redirect("../../Views/Home/Inicio");

                    }
                    else
                    {
                        Response.Write("<script language=javascript>alert('Las Contraseñas no coinciden');</script>");
                    }
                }
                else
                {
                    Response.Write("<script language=javascript>alert('Contraseña Antigua Incorrecta');</script>");
                }
            }
            catch (Exception ex)
            {

            }
            
        }

        

        #endregion

        protected void btnCambiarClave_Click(object sender, EventArgs e)
        {
            CambiarContrasenia();
        }

    }
}