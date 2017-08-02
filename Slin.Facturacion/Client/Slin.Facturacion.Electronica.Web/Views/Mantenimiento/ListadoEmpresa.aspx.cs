using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

using Slin.Facturacion.Common;
using Slin.Facturacion.Electronica.Web.ServiceFacturacion;
using Slin.Facturacion.Proxies.ServicioMantenimiento;

using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;
using Slin.Facturacion.Electronica.Web.Views.Util;

namespace Slin.Facturacion.Electronica.Web.Views.Mantenimiento
{
    public partial class ListadoEmpresa : System.Web.UI.Page
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


        void Mensaje()
        {
            try
            {
                string respuesta = (string)Session["MensajeEmpresa"];
                if (respuesta == Constantes.msjActualizado)
                {
                    Session.Remove("MensajeEmpresa");
                    Response.Write("<script language=javascript>alert('" + respuesta + "');</script>");
                }
                
            }
            catch (Exception ex) { }
        }


        void MensajeRegistrado()
        {
            try
            {
                string respuesta = (string)Session["MensajeEmpresaRegistrada"];
                if (respuesta == Constantes.msjRegistrado)
                {
                    Session.Remove("MensajeEmpresaRegistrada");
                    Response.Write("<script language=javascript>alert('" + respuesta + "');</script>");
                }
                
            }
            catch (Exception ex) { }
        }


        #region ENTITY

        ServiceWebFactController objController = new ServiceWebFactController();

        private ListaEstado olistaEstado;
        public ListaEstado oListaEstado
        {
            get { return olistaEstado; }
            set { olistaEstado = value; }
        }

        private Empresa empresa;
        public Empresa oEmpresa
        {
            get { return empresa; }
            set { empresa = value; }
        }

        private ListaEmpresa olistaEmpresa;
        public ListaEmpresa oListaEmpresa
        {
            get { return olistaEmpresa; }
            set { olistaEmpresa = value; }
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
                CargarLista();
                CapturarUserLogRoles();
                ObtenerUsuarioLogeado();
                txtbuscarruc.Value = oUsuarioLogeado.Empresa.RUC;

                lblempresa.InnerText = oUsuarioLogeado.Empresa.RazonSocial.ToUpper();
                logoEmpresa.Src = "../../Img/home/" + oUsuarioLogeado.Empresa.RUC + ".png";

                txtbuscarrsocial.Value = oUsuarioLogeado.Empresa.RazonSocial;
                ValidarBusqueda();
            }
            catch (Exception ex) { }
        }

        WCFSeguridad.ListaRol oListaRolesUserLog = new WCFSeguridad.ListaRol();
        void CapturarUserLogRoles()
        {
            try
            {
                oListaRolesUserLog = objController.GetList_RolUserLog();
                btnbuscar.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
            }
            catch (Exception ex) { }
        }


        void CargarLista()
        {
            try
            {
                oListaEstado = new ListaEstado();

                oListaEstado = ServiceMantenimientoController.Instance.GetListaEstado();
                oListaEstado.Insert(0, new Estado() { IdEstado = Constantes.ValorCero, Descripcion = Constantes.ValorTodos });

                cbobuscarestado.DataSource = oListaEstado;
                cbobuscarestado.DataValueField = "IdEstado";
                cbobuscarestado.DataTextField = "Descripcion";
                cbobuscarestado.DataBind();
            }
            catch (Exception ex) { }
        }


        void ObtenerDatosBusqueda()
        {
            try
            {
                oEmpresa = new Empresa();
                oEmpresa.Estado = new Estado();
                oEmpresa.Ubigeo = new Ubigeo();
                oEmpresa.RUC = txtbuscarruc.Value.Length == Constantes.ValorCero ? String.Empty : txtbuscarruc.Value;                
                oEmpresa.RazonSocial = txtbuscarrsocial.Value.Length == Constantes.ValorCero ? String.Empty : txtbuscarrsocial.Value;                
                oEmpresa.RazonComercial = string.Empty;
                oEmpresa.Telefono = string.Empty;
                oEmpresa.Estado.IdEstado = Constantes.ValorUno;
                oEmpresa.Ubigeo.IdUbigeo = Constantes.ValorCero;
            }
            catch (Exception ex) { }
        }

        void ValidarBusqueda()
        {
            try
            {
                Session.Remove("ListadoEmpresas");
                ObtenerDatosBusqueda();
                oListaEmpresa = new ListaEmpresa();
                oListaEmpresa = ServiceMantenimientoController.Instance.GetListadoEmpresa(oEmpresa);
                GVListadoEmpresa.DataSource = oListaEmpresa;
                GVListadoEmpresa.DataBind();

                Session["ListadoEmpresas"] = oListaEmpresa;
            }
            catch (Exception ex) { }
        }

        #endregion

        protected void btnbuscar_ServerClick(object sender, EventArgs e)
        {
            try
            {
                GVListadoEmpresa.PageIndex = 0;
                GVListadoEmpresa.DataSourceID = "";
                GVListadoEmpresa.DataBind();
                ValidarBusqueda();
            }
            catch (Exception ex) { }            
        }

        #region ENVIARDATOS
        private Ubigeo oubigeo;
        public Ubigeo oUbigeo
        {
            get { return oubigeo; }
            set { oubigeo = value; }
        }

        public string RucEmpresa = String.Empty;

        void PasarDatosEmpresa()
        {
            try
            {
                oEmpresa = new Empresa();
                oEmpresa.Estado = new Estado();
                oEmpresa.TipoDocumentiIdentidad = new TipoDocumentoIdentidad();
                oEmpresa.Ubigeo = new Ubigeo();

                for (int i = 0; i <= oListaEmpresa.Count - 1; i++)
                {
                    if (RucEmpresa == oListaEmpresa[i].RUC)
                    {
                        oEmpresa.IdEmpresa = oListaEmpresa[i].IdEmpresa;
                        oEmpresa.CodEmpresa = oListaEmpresa[i].CodEmpresa;
                        oEmpresa.RazonSocial = oListaEmpresa[i].RazonSocial;
                        oEmpresa.RazonComercial = oListaEmpresa[i].RazonComercial;
                        oEmpresa.Ubigeo.IdUbigeo = oListaEmpresa[i].Ubigeo.IdUbigeo;
                        oEmpresa.Ubigeo.CodigoUbigeo = oListaEmpresa[i].Ubigeo.CodigoUbigeo;
                        oEmpresa.TipoDocumentiIdentidad.IdTipoDocumentoIdentidad = oListaEmpresa[i].TipoDocumentiIdentidad.IdTipoDocumentoIdentidad;
                        oEmpresa.RUC = oListaEmpresa[i].RUC;
                        oEmpresa.Direccion = oListaEmpresa[i].Direccion;
                        oEmpresa.DomicilioFiscal = oListaEmpresa[i].DomicilioFiscal;
                        oEmpresa.Urbanizacion = oListaEmpresa[i].Urbanizacion;
                        oEmpresa.Telefono = oListaEmpresa[i].Telefono;
                        oEmpresa.Fax = oListaEmpresa[i].Fax;
                        oEmpresa.PaginaWeb = oListaEmpresa[i].PaginaWeb;
                        oEmpresa.Email = oListaEmpresa[i].Email;
                        oEmpresa.FechaRegistro = oListaEmpresa[i].FechaRegistro;
                        oEmpresa.Estado.IdEstado = oListaEmpresa[i].Estado.IdEstado;

                        oEmpresa.Url_CompanyLogo = oListaEmpresa[i].Url_CompanyLogo;
                        oEmpresa.Url_CompanyConsult = oListaEmpresa[i].Url_CompanyConsult;
                    }
                }
                Session["EmpresaSeleccionada"] = oEmpresa;
                Response.Redirect("ActualizarEmpresa");
            }
            catch (Exception ex) { }
        }
        #endregion
        
        protected void btnImgEditar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                oListaEmpresa = (ListaEmpresa)Session["ListadoEmpresas"];
                ImageButton btnDetalles = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btnDetalles.NamingContainer;
                //index = gvrow.RowIndex;
                RucEmpresa = GVListadoEmpresa.Rows[gvrow.RowIndex].Cells[2].Text;
                PasarDatosEmpresa();
            }
            catch (Exception ex) { }
        }
    }
}