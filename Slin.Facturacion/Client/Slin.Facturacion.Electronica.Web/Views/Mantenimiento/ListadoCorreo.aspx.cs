using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Slin.Facturacion.Common;
using Slin.Facturacion.Electronica.Web.ServiceFacturacion;
using Slin.Facturacion.Proxies.ServicioMantenimiento;
using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;
using Slin.Facturacion.Electronica.Web.Views.Util;

namespace Slin.Facturacion.Electronica.Web.Views.Mantenimiento
{
    public partial class ListadoCorreo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                Cargar();
                Mensaje();
                MensajeRegistrado();
            }
        }

        #region ENTITY

        ServiceWebFactController objController = new ServiceWebFactController();

        private WCFSeguridad.ListaCorreo olistaCorreo;
        public WCFSeguridad.ListaCorreo oListaCorreo
        {
            get { return olistaCorreo; }
            set { olistaCorreo = value; }
        }

        private WCFSeguridad.Correo ocorreo;
        public WCFSeguridad.Correo oCorreo
        {
            get { return ocorreo; }
            set { ocorreo = value; }
        }

        private WCFSeguridad.Empresa oempresa;
        public WCFSeguridad.Empresa oEmpresa
        {
            get { return oempresa; }
            set { oempresa = value; }
        }

        private WCFSeguridad.Usuario ousuarioLogeado;
        public WCFSeguridad.Usuario oUsuarioLogeado
        {
            get { return ousuarioLogeado; }
            set { ousuarioLogeado = value; }
        }

        private ListaEstado olistaestado;
        public ListaEstado oListaEstado
        {
            get { return olistaestado; }
            set { olistaestado = value; }
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

        void Mensaje()
        {
            try
            {
                string respuesta = (string)Session["MensajeCorreo"];
                if (respuesta == Constantes.msjActualizado)
                {
                    Session.Remove("MensajeCorreo");
                    Response.Write("<script language=javascript>alert('" + respuesta + "');</script>");
                }
                
            }
            catch (Exception ex) { }
        }

        void MensajeRegistrado()
        {
            try
            {
                string respuesta = (string)Session["MensajeCorreoRegistrado"];
                if (respuesta == Constantes.msjRegistrado)
                {
                    Session.Remove("MensajeCorreoRegistrado");
                    Response.Write("<script language=javascript>alert('" + respuesta + "');</script>");
                }
                
            }
            catch (Exception ex) { } 
        }

        void LlenarObjeto()
        {
            try
            {
                ObtenerUsuarioLogeado();
                oEmpresa = new WCFSeguridad.Empresa();
                oEmpresa.Estado = new WCFSeguridad.Estado();

                oEmpresa.IdEmpresa = oUsuarioLogeado.Empresa.IdEmpresa;
                oEmpresa.Email = txtemail.Value.Length == Constantes.ValorCero ? string.Empty : txtemail.Value;
                oEmpresa.RazonSocial = string.Empty;
                oEmpresa.Estado.IdEstado = int.Parse(cboestado.Value);
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

                btnNuevo.Visible = false;
                CapturarUserLogRoles();
                logoEmpresa.Src = "../../Img/home/" + oUsuarioLogeado.Empresa.RUC + ".png";
                CargarLista();


                CargarListaCorreo();
            }
            catch (Exception ex) { }
        }

        void CargarLista()
        {
            oListaEstado = new ListaEstado();

            oListaEstado = ServiceMantenimientoController.Instance.GetListaEstado();

            oListaEstado.Insert(0, new Estado() { IdEstado = Constantes.ValorCero, Descripcion = Constantes.ValorTodos });

            cboestado.DataSource = oListaEstado;
            cboestado.DataValueField = "IdEstado";
            cboestado.DataTextField = "Descripcion";
            cboestado.DataBind();
        }

        WCFSeguridad.ListaRol oListaRolesUserLog = new WCFSeguridad.ListaRol();
        void CapturarUserLogRoles()
        {
            try
            {
                oListaRolesUserLog = objController.GetList_RolUserLog();

                btnBuscar.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);            
                btnNuevo.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);

            }
            catch (Exception ex) { }
        }

        void CargarListaCorreo()
        {
            try
            {
                Session.Remove("ListadoCorreo");

                LlenarObjeto();

                oListaCorreo = ServiceSeguridadController.Instance.GetListaCorreo(oEmpresa);

                GVListadoCorreo.DataSource = oListaCorreo;
                GVListadoCorreo.DataBind();

                Session["ListadoCorreo"] = oListaCorreo;
            }
            catch (Exception ex) { }
        }

        #endregion

        protected void btnBuscar_ServerClick(object sender, EventArgs e)
        {
            try
            {
                GVListadoCorreo.PageIndex = 0;
                GVListadoCorreo.DataSourceID = "";
                GVListadoCorreo.DataBind();
                CargarListaCorreo();
            }
            catch (Exception ex) { }   
        }


        void PasarDatosCorreo()
        {
            try
            {
                oCorreo = new WCFSeguridad.Correo();
                oCorreo.Estado = new WCFSeguridad.Estado();
                oCorreo.Empresa = new WCFSeguridad.Empresa();
                oCorreo.SSL = new WCFSeguridad.SSL();

                for (int i = 0; i <= oListaCorreo.Count - 1; i++)
                {
                    //if (CorreoSelect == oListaCorreo[i].Email)
                    if (CorreoSelect == oListaCorreo[i].Email && typeMail == oListaCorreo[i].TypeMail)
                    {
                        
                        oCorreo.IdEmail = oListaCorreo[i].IdEmail;
                        oCorreo.Empresa.IdEmpresa = oListaCorreo[i].Empresa.IdEmpresa;
                        oCorreo.Empresa.RazonSocial = oListaCorreo[i].Empresa.RazonSocial;
                        oCorreo.Email = oListaCorreo[i].Email;
                        oCorreo.Password = oListaCorreo[i].Password;

                        oCorreo.Domain = oListaCorreo[i].Domain;
                        oCorreo.IP = oListaCorreo[i].IP;
                        oCorreo.Port = oListaCorreo[i].Port;
                        oCorreo.Estado.IdEstado = oListaCorreo[i].Estado.IdEstado;
                        oCorreo.Empresa.RUC = oListaCorreo[i].Empresa.RUC;
                        oCorreo.TypeMail = oListaCorreo[i].TypeMail;
                        oCorreo.SSL.Id = oListaCorreo[i].SSL.Id;

                        Session["EditCorreoSeleccionado"] = oCorreo;
                        break;
                    }
                }
                Response.Redirect("ActualizarCorreo");
            }
            catch (Exception ex) { }
        }

        public string typeMail = string.Empty;
        public string CorreoSelect = string.Empty;
        protected void btnImgEditar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                oListaCorreo = (WCFSeguridad.ListaCorreo)Session["ListadoCorreo"];

                ImageButton btnDetalles = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btnDetalles.NamingContainer;
                //index = gvrow.RowIndex;
                CorreoSelect = GVListadoCorreo.Rows[gvrow.RowIndex].Cells[3].Text;
                typeMail = GVListadoCorreo.Rows[gvrow.RowIndex].Cells[9].Text;
                PasarDatosCorreo();
            }
            catch (Exception ex) { }
        }
    }
}