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
using System.IO;
using Slin.Facturacion.Electronica.Web.Views.Util;

namespace Slin.Facturacion.Electronica.Web.Views.Mantenimiento
{
    public partial class ListadoCliente : System.Web.UI.Page
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

        private ListaEstado olistaestado;
        public ListaEstado oListaEstado
        {
            get { return olistaestado; }
            set { olistaestado = value; }
        }

        private Cliente ocliente;
        public Cliente oCliente
        {
            get { return ocliente; }
            set { ocliente = value; }
        }

        private ListaCliente olistacliente;
        public ListaCliente oListaCliente
        {
            get { return olistacliente; }
            set { olistacliente = value; }
        }

        private WCFSeguridad.Usuario ousuarioLogeado;
        public WCFSeguridad.Usuario oUsuarioLogeado
        {
            get { return ousuarioLogeado; }
            set { ousuarioLogeado = value; }
        }

        #endregion


        #region METHOD

        void Mensaje()
        {
            try
            {
                string respuesta = (string)Session["MensajeCliente"];
                if (respuesta == Constantes.msjActualizado)
                {
                    Session.Remove("MensajeCliente");
                    Response.Write("<script language=javascript>alert('" + respuesta + "');</script>");
                }
                
            }
            catch (Exception ex)
            {

            }
            
        }

        void MensajeRegistrado()
        {
            try
            {
                string respuesta = (string)Session["MensajeClienteRegistrado"];
                if (respuesta == Constantes.msjRegistrado)
                {
                    Session.Remove("MensajeClienteRegistrado");
                    Response.Write("<script language=javascript>alert('" + respuesta + "');</script>");
                }
                
            }
            catch (Exception ex)
            {

            }
            
        }

        void Cargar()
        {
            string Url = HttpContext.Current.Request.Url.AbsoluteUri;
            int result = objController.Process_UrlPage(Url);
            if (result == Constantes.ValorCero)
            { Response.Redirect("~/Views/Home/Inicio.aspx"); }

            try
            {
                btnNuevo.Visible = false;

                ObtenerUsuarioLogueado();
                lblempresa.InnerText = oUsuarioLogeado.Empresa.RazonSocial.ToUpper();
                CargarLista();
                CapturarUserLogRoles();

                logoEmpresa.Src = "../../Img/home/" + oUsuarioLogeado.Empresa.RUC + ".png";
            }
            catch (Exception ex)
            { }
        }

        WCFSeguridad.ListaRol oListaRolesUserLog = new WCFSeguridad.ListaRol();
        void CapturarUserLogRoles()
        {
            try
            {
                oListaRolesUserLog = objController.GetList_RolUserLog();

                btnBuscar.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);

                btnNuevo.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);

                btnExportarExcel.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnExportarExcel.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);

                btnExportarPDF.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnExportarPDF.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);

            }
            catch (Exception ex)
            {

            }
            
        }

        private void ObtenerUsuarioLogueado()
        {
            try
            {
                oUsuarioLogeado = objController.GetUserLogueado_Fact();
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
            catch (Exception ex)
            {

            }
            
        }



        void LlenarObjetoBusqueda()
        {
            try
            {
                ObtenerUsuarioLogueado();

                oCliente = new Cliente();
                oCliente.Estado = new Estado();
                oCliente.Empresa = new Empresa();

                oCliente.RazonSocial = txtrazonsocialcliente.Value.Length == Constantes.ValorCero ? string.Empty : txtrazonsocialcliente.Value;
                oCliente.NroDocumento = txtnrodocumentocliente.Value.Length == Constantes.ValorCero ? string.Empty : txtnrodocumentocliente.Value;
                oCliente.Estado.IdEstado = Convert.ToInt32(cbobuscarestado.Value);
                oCliente.Empresa.IdEmpresa = oUsuarioLogeado.Empresa.IdEmpresa;
                oCliente.Empresa.RUC = oUsuarioLogeado.Empresa.RUC;
            }
            catch (Exception ex)
            {

            }
            
        }

        void CargarListaClientes()
        {
            try
            {
                Session.Remove("ListadoClientes");
                LlenarObjetoBusqueda();
                oListaCliente = ServiceMantenimientoController.Instance.GetListaCliente(oCliente);
                GVListaClientes.DataSource = oListaCliente;
                GVListaClientes.DataBind();
                Session["ListadoClientes"] = oListaCliente;
            }
            catch (Exception ex)
            {

            }
            
        }

        #endregion

        protected void btnBuscar_ServerClick(object sender, EventArgs e)
        {
            try
            {
                GVListaClientes.PageIndex = 0;
                GVListaClientes.DataSourceID = "";
                GVListaClientes.DataBind();
                CargarListaClientes();
            }
            catch (Exception ex) { }
        }

        public Cliente oClienteSeleccionado = new Cliente();
        void PasarDatosCliente()
        {
            try
            {
                oClienteSeleccionado = new Cliente();
                oClienteSeleccionado.Estado = new Estado();
                oClienteSeleccionado.Empresa = new Empresa();

                for (int i = 0; i <= oListaCliente.Count - 1; i++)
                {
                    if (NroDocCliente == oListaCliente[i].NroDocumento)
                    {
                        oClienteSeleccionado.IdCliente = oListaCliente[i].IdCliente;
                        oClienteSeleccionado.RazonSocial = oListaCliente[i].RazonSocial;
                        oClienteSeleccionado.NroDocumento = oListaCliente[i].NroDocumento;
                        oClienteSeleccionado.Email = oListaCliente[i].Email;
                        oClienteSeleccionado.Telefono = oListaCliente[i].Telefono;
                        oClienteSeleccionado.Direccion = oListaCliente[i].Direccion;
                        oClienteSeleccionado.Estado.IdEstado = oListaCliente[i].Estado.IdEstado;
                        oClienteSeleccionado.Empresa.IdEmpresa = oListaCliente[i].Empresa.IdEmpresa;

                        Session["EditClienteSeleccionado"] = oClienteSeleccionado;
                        break;
                    }
                }
                Response.Redirect("ActualizarCliente");
            }
            catch (Exception ex) { }
        }

        public string NroDocCliente = string.Empty;
        protected void btnImgEditar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                oListaCliente = (ListaCliente)Session["ListadoClientes"];

                ImageButton btnDetalles = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btnDetalles.NamingContainer;
                NroDocCliente = GVListaClientes.Rows[gvrow.RowIndex].Cells[4].Text;

                PasarDatosCliente();
            }
            catch (Exception ex) { }            
        }

        protected void btnEnviarCorreo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                oListaCliente = (ListaCliente)Session["ListadoClientes"];

                ImageButton btnSendEmail = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btnSendEmail.NamingContainer;
                NroDocCliente = GVListaClientes.Rows[gvrow.RowIndex].Cells[4].Text;
                string EmailClienteSeleccionado = GVListaClientes.Rows[gvrow.RowIndex].Cells[5].Text;


                Session["EmailClienteSeleccionado"] = EmailClienteSeleccionado;
                Response.Redirect("../Envio/EnvioDocumento");
            }
            catch (Exception ex) { }
        }

        #region EXPORT EXCEL

        private ListaCliente listclient;
        public ListaCliente ListClient
        {
            get { return listclient; }
            set { listclient = value; }
        }

        void EnviarToExcel()
        {
            try
            {
                Common.Method.Singleton.Instance.CreateDirectory(Server.MapPath("~/DocumentoXML/"));
                var lista = new ListaCliente();
                lista = (ListaCliente)Session["ListadoClientes"];

                Session["ExcelListClient"] = lista;
                Session["TipoExport"] = Constantes.ValorExportExcel;
                Response.Redirect("../Informes/ExportReportListClient");
            }
            catch (Exception ex) { }
        }

        void EnviarToPDF()
        {
            try
            {
                Common.Method.Singleton.Instance.CreateDirectory(Server.MapPath("~/DocumentoXML/"));

                var lista = new ListaCliente();
                lista = (ListaCliente)Session["ListadoClientes"];

                Session["ExcelListClient"] = lista;
                Session["TipoExport"] = Constantes.ValorExportPDF;
                Response.Redirect("../Informes/ExportReportListClient");
            }
            catch (Exception ex) { }
        }

        #endregion END EXPORT EXCEL

        protected void btnExportarPDF_Click(object sender, EventArgs e)
        {
            try
            {
                if (GVListaClientes.Rows.Count == 0)
                {
                    Response.Write("<script language=javascript>alert('No se ha realizado una consulta previa');</script>");
                }
                else
                { EnviarToPDF(); }
            }
            catch (Exception ex) { }
        }

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (GVListaClientes.Rows.Count == 0)
                {
                    Response.Write("<script language=javascript>alert('No se ha realizado una consulta previa');</script>");
                }
                else
                { EnviarToExcel(); }
            }
            catch (Exception ex) { }
        }

    }
}