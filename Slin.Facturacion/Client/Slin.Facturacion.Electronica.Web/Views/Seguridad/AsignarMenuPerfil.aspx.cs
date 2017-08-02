using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Slin.Facturacion.Common;
using Slin.Facturacion.Proxies.ServicioMantenimiento;
using Slin.Facturacion.Electronica.Web.ServiceFacturacion;
using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;
using Slin.Facturacion.Electronica.Web.Views.Util;

namespace Slin.Facturacion.Electronica.Web.Views.Seguridad
{
    public partial class AsignarMenuPerfil : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                Cargar();
                Message();
            }
        }

        #region ENTITY
        ServiceWebFactController objController = new ServiceWebFactController();

        private WCFSeguridad.Perfil operfil;
        public WCFSeguridad.Perfil oPerfil
        {
            get { return operfil; }
            set { operfil = value; }
        }

        private WCFSeguridad.ListaPerfil olistaPerfil;
        public WCFSeguridad.ListaPerfil oListaPerfil
        {
            get { return olistaPerfil; }
            set { olistaPerfil = value; }
        }

        private WCFSeguridad.Usuario ousuarioLogeado;
        public WCFSeguridad.Usuario oUsuarioLogeado
        {
            get { return ousuarioLogeado; }
            set { ousuarioLogeado = value; }
        }

        #endregion


        #region METHOD

        private void ObtenerUsuarioLogeado()
        {
            try
            {
                oUsuarioLogeado = objController.GetUserLogueado_Fact();
            }
            catch (Exception ex) { }
        }
        void Cargar()
        {
            string Url = HttpContext.Current.Request.Url.AbsoluteUri;
            int result = objController.Process_UrlPage(Url);
            if (result == Constantes.ValorCero)
            { Response.Redirect("~/Views/Home/Inicio.aspx"); }

            try
            {
                ObtenerUsuarioLogeado();
                lblempresa.InnerText = oUsuarioLogeado.Empresa.RazonSocial.ToUpper();
                CapturarUserLogRoles();

                CargaListPerfiles();

                logoEmpresa.Src = "../../Img/home/" + oUsuarioLogeado.Empresa.RUC + ".png";
            }
            catch (Exception ex)
            {

            }
            
        }

        private void Message()
        {
            try
            {
                string respuesta = (string)Session["MessageProcessProfile"];
                if (respuesta.Length > Constantes.ValorCero)
                {
                    Session.Remove("MessageProcessProfile");
                    Response.Write("<script language=javascript>alert('" + respuesta + "');</script>");
                }
                
            }
            catch (Exception ex) { }
        }





        WCFSeguridad.ListaRol oListaRolesUserLog = new WCFSeguridad.ListaRol();
        void CapturarUserLogRoles()
        {
            try
            {
                oListaRolesUserLog = objController.GetList_RolUserLog();

                btnGuardar.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnGuardar.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
            }
            catch (Exception ex)
            {

            }        
        }

        void CargarListaPerfiles()
        {
            try
            {
                Session.Remove("ListaPerfiles");
                LlenarObjeto();

                oListaPerfil = ServiceSeguridadController.Instance.GetListaPerfil(oPerfil);

                GVListaPerfiles.DataSource = oListaPerfil;
                GVListaPerfiles.DataBind();

                Session["ListaPerfiles"] = oListaPerfil;
            }
            catch (Exception ex)
            {

            }
            
        }
        #endregion


        public string idperfil = string.Empty;
        protected void btnImgSelect_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btnSelect = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btnSelect.NamingContainer;

                oListaPerfil = (WCFSeguridad.ListaPerfil)Session["ListaPerfiles"];

                oPerfil = new WCFSeguridad.Perfil();
                oPerfil.Empresa = new WCFSeguridad.Empresa();

                //CodigoPerfil = GVListaPerfiles.Rows[gvrow.RowIndex].Cells[2].Text;
                idperfil = GVListaPerfiles.Rows[gvrow.RowIndex].Cells[1].Text;

                for (int i = 0; i <= oListaPerfil.Count - 1; i++)
                {
                    //if (CodigoPerfil == oListaPerfil[i].NombrePerfil)
                    if (int.Parse(idperfil) == oListaPerfil[i].IdPerfil)
                        {
                        oPerfil.IdPerfil = oListaPerfil[i].IdPerfil;
                        oPerfil.NombrePerfil = oListaPerfil[i].NombrePerfil;
                        oPerfil.Codigo = oListaPerfil[i].Codigo;
                        oPerfil.Empresa.RUC = oListaPerfil[i].Empresa.RUC;
                        break;
                    }
                }
                Session["PerfilSeleccionado"] = oPerfil;
                Response.Redirect("AsignarPerfil");
            }
            catch (Exception ex)
            {

            }
            
        }

        protected void btnBuscar_ServerClick(object sender, EventArgs e)
        {
            CargaListPerfiles();
        }


        public void CargaListPerfiles()
        {
            try
            {
                GVListaPerfiles.PageIndex = 0;
                GVListaPerfiles.DataSourceID = "";
                GVListaPerfiles.DataBind();
                CargarListaPerfiles();
            }
            catch (Exception ex)
            {

            }
            
        }


        void LlenarObjeto()
        {
            try
            {
                ObtenerUsuarioLogeado();
                oPerfil = new WCFSeguridad.Perfil();
                oPerfil.Empresa = new WCFSeguridad.Empresa();

                oPerfil.NombrePerfil = txtnombreperfil.Value;
                //oPerfil.Codigo = txtcodigo.Value.Length == 0 ? string.Empty : txtcodigo.Value;
                oPerfil.Codigo = string.Empty;
                oPerfil.Empresa.RUC = oUsuarioLogeado.Empresa.RUC;
            }
            catch (Exception ex)
            {

            }

        }

        void GuardarPerfil()
        {
            try
            {
                string msj = string.Empty;
                LlenarObjeto();
                msj = ServiceSeguridadController.Instance.RegistrarNuevoPerfil(oPerfil);
                Session["MessageProcessProfile"] = msj;
                Response.Redirect("AsignarMenuPerfil");
            }
            catch (Exception ex) { }
        }

        private int ProfileValidateName()
        {
            try
            {
                var listProfile = new List<string>();
                var list = (WCFSeguridad.ListaPerfil)Session["ListaPerfiles"];

                for (int i = 0; i <= list.Count - 1; i++)
                {
                    listProfile.Add(list[i].NombrePerfil);
                }

                if (listProfile.Contains(txtnombreperfil.Value))
                {
                    return Constantes.ValorDos;
                }
                else { return Constantes.ValorUno; }
            }
            catch (Exception ex) { return Constantes.ValorCero; }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            var result = ProfileValidateName();

            if (result == Constantes.ValorDos)
            {
                Session["MessageProcessProfile"] = Constantes.msjRegistroExistente;
                Response.Redirect("AsignarMenuPerfil");
            }else if (result == Constantes.ValorUno)
            {

                if (txtnombreperfil.Value.Length > Constantes.ValorCero)
                {
                    GuardarPerfil();
                }
                else
                {
                    Response.Write("<script language=javascript>alert('Ingrese Nombre de Perfil');</script>");
                }
            }
        }

        protected void btnDelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                
                ObtenerUsuarioLogeado();
                operfil = new WCFSeguridad.Perfil();
                operfil.Empresa = new WCFSeguridad.Empresa();

                ImageButton btnDetalles = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btnDetalles.NamingContainer;

                operfil.IdPerfil = int.Parse(GVListaPerfiles.Rows[gvrow.RowIndex].Cells[1].Text);
                //operfil.Codigo = GVListaPerfiles.Rows[gvrow.RowIndex].Cells[2].Text;
                operfil.Codigo = string.Empty;
                operfil.Empresa.RUC = oUsuarioLogeado.Empresa.RUC;

                var result = ServiceSeguridadController.Instance.Delete_ProfileComp(operfil);

                Session["MessageProcessProfile"] = result;
                Response.Redirect("AsignarMenuPerfil");

            }
            catch (Exception ex)
            {

            }
        }

        protected void GVListaPerfiles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //try
            //{
            //    var list = new Common.Helper.ListClass().Return_ListCodProfileSetup();
            //    if (e.Row.RowType == DataControlRowType.DataRow)
            //    {
            //        ImageButton btnImg = (ImageButton)e.Row.FindControl("btnDelete");

            //        string cod = e.Row.Cells[3].Text;

            //        if (list.Contains(cod))
            //        {
            //            btnImg.Enabled = false;
            //            btnImg.Visible = false;
            //        }
            //    }
            //}
            //catch (Exception ex) { }
        }

        protected void btnImgEdit_Click(object sender, ImageClickEventArgs e)
        {

        }
    }
}