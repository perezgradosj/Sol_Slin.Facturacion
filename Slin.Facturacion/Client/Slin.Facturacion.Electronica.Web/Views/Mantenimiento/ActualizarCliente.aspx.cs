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

namespace Slin.Facturacion.Electronica.Web.Views.Mantenimiento
{
    public partial class ActualizarCliente : System.Web.UI.Page
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

        private ListaEstado olistaEstado;
        public ListaEstado oListaEstado
        {
            get { return olistaEstado; }
            set { olistaEstado = value; }
        }

        private Cliente oclienteedit;
        public Cliente oClienteEdit
        {
            get { return oclienteedit; }
            set { oclienteedit = value; }
        }

        private Cliente ocliente;
        public Cliente oCliente
        {
            get { return ocliente; }
            set { ocliente = value; }
        }

        private WCFSeguridad.Usuario ousuarioLogeado;
        public WCFSeguridad.Usuario oUsuarioLogeado
        {
            get { return ousuarioLogeado; }
            set { ousuarioLogeado = value; }
        }
        #endregion

        #region METHOD

        WCFSeguridad.ListaRol oListaRolesUserLog = new WCFSeguridad.ListaRol();
        void CapturarUserLogRoles()
        {
            try
            {
                oListaRolesUserLog = objController.GetList_RolUserLog();

                btnActualizar.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnActualizar.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);

            }
            catch (Exception ex)
            {

            }
            
        }

        void ObtenerUsuarioLogueado()
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
                ObtenerUsuarioLogueado();

                lblempresa.InnerText = oUsuarioLogeado.Empresa.RazonSocial.ToUpper();
                CargarListas();
                RecibirObjeto();
                CapturarUserLogRoles();

                logoEmpresa.Src = "../../Img/home/" + oUsuarioLogeado.Empresa.RUC + ".png";
            }
            catch (Exception ex)
            {

            }
            
        }

        void CargarListas()
        {
            try
            {
                oListaEstado = ServiceMantenimientoController.Instance.GetListaEstado();

                cboestado.DataSource = oListaEstado;
                cboestado.DataValueField = "IdEstado";
                cboestado.DataTextField = "Descripcion";
                cboestado.DataBind();
            }
            catch (Exception ex)
            {

            }
            
        }

        void RecibirObjeto()
        {
            try
            {
                oClienteEdit = new Cliente();
                oClienteEdit = (Cliente)Session["EditClienteSeleccionado"];

                txtnombreRazonSocial.Value = oClienteEdit.RazonSocial;
                txtnroDocumentocliente.Value = oClienteEdit.NroDocumento;
                txtemail.Value = oClienteEdit.Email;
                txttelefono.Value = oClienteEdit.Telefono;
                txtdireccion.Value = oClienteEdit.Direccion;

                cboestado.Value = oClienteEdit.Estado.IdEstado.ToString();
            }
            catch (Exception ex)
            {

            }
            
        }

        void LlenarClienteEdit()
        {
            try
            {
                oClienteEdit = (Cliente)Session["EditClienteSeleccionado"];

                oCliente = new Cliente();
                oCliente.Estado = new Estado();
                oCliente.Empresa = new Empresa();

                oCliente.IdCliente = oClienteEdit.IdCliente;
                oCliente.RazonSocial = txtnombreRazonSocial.Value.Length == Constantes.ValorCero ? string.Empty : txtnombreRazonSocial.Value;
                oCliente.NroDocumento = oClienteEdit.NroDocumento;
                oCliente.Email = txtemail.Value.Length == Constantes.ValorCero ? string.Empty : txtemail.Value;
                oCliente.Telefono = txttelefono.Value.Length == Constantes.ValorCero ? string.Empty : txttelefono.Value;
                oCliente.Direccion = txtdireccion.Value.Length == Constantes.ValorCero ? string.Empty : txtdireccion.Value;
                oCliente.Estado.IdEstado = Convert.ToInt32(cboestado.Value);
                oCliente.Empresa.IdEmpresa = oClienteEdit.Empresa.IdEmpresa;
            }
            catch (Exception ex)
            {

            }
            
        }

        void ActualizarRegistro()
        {
            try
            {
                LlenarClienteEdit();

                string msje = string.Empty;

                msje = ServiceMantenimientoController.Instance.ActualizarCliente(oCliente);

                Session["MensajeCliente"] = Constantes.msjActualizado;
                Response.Redirect("ListadoCliente");
            }
            catch (Exception ex)
            {

            }
            
        }

        #endregion

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            ActualizarRegistro();
        }
    }
}