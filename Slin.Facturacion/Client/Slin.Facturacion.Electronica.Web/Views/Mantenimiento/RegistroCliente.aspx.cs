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
    public partial class RegistroCliente : System.Web.UI.Page
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

        private ListaEmpresa olistaEmpresa;
        public ListaEmpresa oListaEmpresa
        {
            get { return olistaEmpresa; }
            set { olistaEmpresa = value; }
        }

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
                CargarLista();
                CapturarUserLogRoles();
                logoEmpresa.Src = "../../Img/home/" + oUsuarioLogeado.Empresa.RUC + ".png";
            }
            catch (Exception ex)
            {

            }
            
        }

        WCFSeguridad.ListaRol oListaRolesUserLog = new WCFSeguridad.ListaRol();
        void CapturarUserLogRoles()
        {
            try
            {
                oListaRolesUserLog = objController.GetList_RolUserLog();

                btnGuardar.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnGuardar.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);

            }
            catch (Exception ex)
            {

            }
            
        }

        void CargarLista()
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



        void LlenarObjeto()
        {
            try
            {
                ObtenerUsuarioLogeado();
                oCliente = new Cliente();
                oCliente.Estado = new Estado();
                oCliente.Empresa = new Empresa();

                oCliente.Estado.IdEstado = Convert.ToInt32(cboestado.Value);
                oCliente.Empresa.IdEmpresa = oUsuarioLogeado.Empresa.IdEmpresa;
                oCliente.RazonSocial = txtnombreRazonSocial.Value.Length == Constantes.ValorCero ? string.Empty : txtnombreRazonSocial.Value;
                oCliente.NroDocumento = txtnrodocumentocliente.Value.Length == Constantes.ValorCero ? string.Empty : txtnrodocumentocliente.Value;
                oCliente.Email = txtemail.Value.Length == Constantes.ValorCero ? string.Empty : txtemail.Value;
                oCliente.Telefono = txttelefono.Value.Length == Constantes.ValorCero ? string.Empty : txttelefono.Value;
                oCliente.Direccion = txtdireccion.Value.Length == Constantes.ValorCero ? string.Empty : txtdireccion.Value;
            }
            catch (Exception ex)
            {

            }
            
        }

        public ListaCliente olistaResultado;
        void ValidarNroDocumentoCliente()
        {
            try
            {
                olistaResultado = new ListaCliente();

                olistaResultado = ServiceMantenimientoController.Instance.ValidarNroClienteExiste(txtnrodocumentocliente.Value);

                if (olistaResultado.Count > 0)
                {
                    txtnrodocumentocliente.Focus();
                    Response.Write("<script language=javascript>alert('El Nro Documento ya Existe');</script>");
                }
                else
                {
                    InsertarCliente();
                }
            }
            catch (Exception ex)
            {

            }
            
        }

        public string respuesta = string.Empty;
        void InsertarCliente()
        {
            try
            {
                LlenarObjeto();
                respuesta = ServiceMantenimientoController.Instance.InsertarCliente(oCliente);
                LimpiarCampos();
                Session["MensajeClienteRegistrado"] = Constantes.msjRegistrado;
                Response.Redirect("ListadoCliente");
            }
            catch (Exception ex)
            {

            }
            
        }

        void LimpiarCampos()
        {
            txtnombreRazonSocial.Value = string.Empty;
            txtnrodocumentocliente.Value = string.Empty;
            txtemail.Value = string.Empty;
            txttelefono.Value = string.Empty;
            txtdireccion.Value = string.Empty;
            cboestado.SelectedIndex = Constantes.ValorCero;
        }

        #endregion

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            ValidarNroDocumentoCliente();
        }

    }
}