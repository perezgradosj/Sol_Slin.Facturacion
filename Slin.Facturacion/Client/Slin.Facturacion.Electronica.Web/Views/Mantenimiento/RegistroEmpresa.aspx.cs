using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Runtime.Caching;

using Slin.Facturacion.Common;
using Slin.Facturacion.Proxies.ServicioMantenimiento;
using Slin.Facturacion.Electronica.Web.ServiceFacturacion;
using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;
using Slin.Facturacion.Electronica.Web.Views.Util;

namespace Slin.Facturacion.Electronica.Web.Views.Mantenimiento
{
    public partial class RegistroEmpresa : System.Web.UI.Page
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

        private Empresa oempresa;
        public Empresa oEmpresa
        {
            get { return oempresa; }
            set { oempresa = value; }
        }

        private ListaTipoDocumentoIdentidad olistatipoDocumentoIdentidad;
        public ListaTipoDocumentoIdentidad oListaTipoDocumentoIdentidad
        {
            get { return olistatipoDocumentoIdentidad; }
            set { olistatipoDocumentoIdentidad = value; }
        }

        private ListaPais olistaPais;
        public ListaPais oListaPais
        {
            get { return olistaPais; }
            set { olistaPais = value; }
        }

        private ListaDepartamento olistaDepartamento;
        public ListaDepartamento oListaDepartamento
        {
            get { return olistaDepartamento; }
            set { olistaDepartamento = value; }
        }

        private ListaProvincia olistaProvincia;
        public ListaProvincia oListaProvincia
        {
            get { return olistaProvincia; }
            set { olistaProvincia = value; }
        }

        private ListaDistrito olistaDistrito;
        public ListaDistrito oListaDistrito
        {
            get { return olistaDistrito; }
            set { olistaDistrito = value; }
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
                CargarListas();
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
        

        void CargarListas()
        {
            try
            {
                oListaEstado = new ListaEstado();
                oListaEstado = ServiceMantenimientoController.Instance.GetListaEstado();
                cboestado.DataSource = oListaEstado;
                cboestado.DataValueField = "IdEstado";
                cboestado.DataTextField = "Descripcion";
                cboestado.DataBind();

                txtfecharegistro.Value = DateTime.Now.ToShortDateString();


                oListaTipoDocumentoIdentidad = ServiceMantenimientoController.Instance.GetListaTipoDocumentoIdentidad();
                oListaTipoDocumentoIdentidad.Insert(0, new TipoDocumentoIdentidad() { IdTipoDocumentoIdentidad = Constantes.ValorCero, Descripcion = Constantes.ValorSeleccione });
                cbotipodocumento.DataSource = oListaTipoDocumentoIdentidad;
                cbotipodocumento.DataValueField = "IdTipoDocumentoIdentidad";
                cbotipodocumento.DataTextField = "Descripcion";
                cbotipodocumento.DataBind();

                oListaPais = ServiceMantenimientoController.Instance.GetListaPais();
                oListaPais.Insert(0, new Pais() { IdPais = Constantes.ValorCero, Descripcion = Constantes.ValorSeleccione });
                cbopais.DataSource = oListaPais;
                cbopais.DataValueField = "IdPais";
                cbopais.DataTextField = "Descripcion";
                cbopais.DataBind();

                CargarListaUbigeo();
            }
            catch (Exception ex)
            {

            }
            
        }

        void CargarListaUbigeo()
        {
            try
            {
                int IdPais = Constantes.ValorUno;
                oListaDepartamento = ServiceMantenimientoController.Instance.GetListaDepartamento(IdPais);
                oListaDepartamento.Insert(0, new Departamento() { IdDepartamento = Constantes.ValorCero, Descripcion = Constantes.ValorSeleccione });
                cbodepartamento.DataSource = oListaDepartamento;
                cbodepartamento.DataValueField = "IdDepartamento";
                cbodepartamento.DataTextField = "Descripcion";
                cbodepartamento.DataBind();

                oListaProvincia = ServiceMantenimientoController.Instance.GetListaProvincia(Convert.ToInt32(cbodepartamento.Value));
                oListaProvincia.Insert(0, new Provincia() { IdProvincia = Constantes.ValorCero, Descripcion = Constantes.ValorSeleccione });
                cboprovincia.DataSource = oListaProvincia;
                cboprovincia.DataValueField = "IdProvincia";
                cboprovincia.DataTextField = "Descripcion";
                cboprovincia.DataBind();

                oListaDistrito = ServiceMantenimientoController.Instance.GetListaDistrito(Convert.ToInt32(cboprovincia.Value));
                oListaDistrito.Insert(0, new Distrito() { IdDistrito = Constantes.ValorCero, Descripcion = Constantes.ValorSeleccione });
                cbodistrito.DataSource = oListaDistrito;
                cbodistrito.DataValueField = "IdDistrito";
                cbodistrito.DataTextField = "Descripcion";
                cbodistrito.DataBind();
            }
            catch (Exception ex)
            {

            }
            
        }

        void ObtenerDatosEmpresa()
        {
            try
            {
                oEmpresa = new Empresa();
                oEmpresa.Ubigeo = new Ubigeo();
                oEmpresa.Estado = new Estado();
                oEmpresa.TipoDocumentiIdentidad = new TipoDocumentoIdentidad();

                oempresa.CodEmpresa = txtruc.Value.Length == Constantes.ValorCero ? String.Empty : txtruc.Value;
                oEmpresa.RUC = txtruc.Value.Length == Constantes.ValorCero ? String.Empty : txtruc.Value;
                oEmpresa.RazonSocial = txtrazonsocial.Value.Length == Constantes.ValorCero ? String.Empty : txtrazonsocial.Value;
                oEmpresa.RazonComercial = txtrazoncomercial.Value.Length == Constantes.ValorCero ? String.Empty : txtrazoncomercial.Value;
                oEmpresa.Telefono = txttelefono.Value.Length == Constantes.ValorCero ? String.Empty : txttelefono.Value;
                oEmpresa.Fax = txtfax.Value.Length == Constantes.ValorCero ? String.Empty : txtfax.Value;
                oEmpresa.Direccion = txtdireccion.Value.Length == Constantes.ValorCero ? String.Empty : txtdireccion.Value;
                oEmpresa.DomicilioFiscal = txtdomiciliofiscal.Value.Length == Constantes.ValorCero ? String.Empty : txtdomiciliofiscal.Value;
                oEmpresa.Urbanizacion = txturbanizacion.Value.Length == Constantes.ValorCero ? String.Empty : txturbanizacion.Value;
                oEmpresa.FechaRegistro = Convert.ToDateTime(txtfecharegistro.Value);
                oEmpresa.PaginaWeb = txtpaginaweb.Value.Length == Constantes.ValorCero ? String.Empty : txtpaginaweb.Value;
                oEmpresa.Email = txtemail.Value.Length == Constantes.ValorCero ? String.Empty : txtemail.Value;
                oEmpresa.Estado.IdEstado = Convert.ToInt32(cboestado.Value);
                oEmpresa.TipoDocumentiIdentidad.IdTipoDocumentoIdentidad = Convert.ToInt32(cbotipodocumento.Value);

                oEmpresa.Ubigeo.IdUbigeo = Constantes.ValorUno;
                oEmpresa.Ubigeo.CodigoUbigeo = txtubigeo.Value.Length == Constantes.ValorCero ? String.Empty : txtubigeo.Value;
            }
            catch (Exception ex)
            {

            }
            
        }


        void ValidarDatos()
        {
            try
            {
                if (txtubigeo.Value.Length == Constantes.ValorCero)
                {
                    Response.Write("<script language=javascript>alert('Seleccione Distrito');</script>");
                }
                else if (cbotipodocumento.SelectedIndex == Constantes.ValorCero)
                {
                    Response.Write("<script language=javascript>alert('Seleccione Tipo Documento');</script>");
                }
                else if (txtruc.Value.Length < 11)
                {
                    Response.Write("<script language=javascript>alert('El Ruc debe Tener 11 Digitos');</script>");
                }
                else
                {

                    var listaresult = new ListaEmpresa();
                    listaresult = ServiceMantenimientoController.Instance.ValidarEmpresaRuc(txtruc.Value);

                    if (listaresult.Count > 0)
                    {
                        Response.Write("<script language=javascript>alert('El Ruc ya esta registrado');</script>");
                    }
                    else
                    {
                        ValidarRegistro();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            
        }

        void ValidarRegistro()
        {
            try
            {
                ObtenerDatosEmpresa();
                string respuesta = string.Empty;
                respuesta = ServiceMantenimientoController.Instance.RegistrarEmpresa(oEmpresa);

                Session["MensajeEmpresaRegistrada"] = Constantes.msjRegistrado;
                Response.Redirect("ListadoEmpresa");
            }
            catch (Exception ex)
            {

            }
            
        }

        void LimpiarCampos()
        {
            txtrazonsocial.Value = String.Empty;
            txtrazoncomercial.Value = String.Empty;

            cbotipodocumento.SelectedIndex = Constantes.ValorCero;
            txtruc.Value = String.Empty;
            txtdireccion.Value = String.Empty;
            txtdomiciliofiscal.Value = String.Empty;
            txturbanizacion.Value = String.Empty;
            cbopais.SelectedIndex = Constantes.ValorCero;
            cbodepartamento.SelectedIndex = Constantes.ValorCero;
            cboprovincia.SelectedIndex = Constantes.ValorCero;
            cbodistrito.SelectedIndex = Constantes.ValorCero;
            txtubigeo.Value = String.Empty;
            txttelefono.Value = String.Empty;
            txtfax.Value = String.Empty;
            txtpaginaweb.Value = String.Empty;
            txtemail.Value = String.Empty;
            cboestado.SelectedIndex = Constantes.ValorUno;
        }

        #endregion

        void SessionGuardar()
        {
            try
            {
                var empresaTemporal = new Empresa();
                empresaTemporal.RazonSocial = txtrazonsocial.Value;

                Session["empresatemporal"] = empresaTemporal;
            }
            catch (Exception ex)
            {

            }
            
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                //ValidarDatos();
                LimpiarCampos();
            }
            catch (Exception ex) { }
        }



        protected void cbopais_ServerChange(object sender, EventArgs e)
        {
            try
            {
                int IdPais = Constantes.ValorUno;
                oListaDepartamento = ServiceMantenimientoController.Instance.GetListaDepartamento(IdPais);
                oListaDepartamento.Insert(0, new Departamento() { IdDepartamento = Constantes.ValorCero, Descripcion = Constantes.ValorSeleccione });
                cbodepartamento.DataSource = oListaDepartamento;
                cbodepartamento.DataValueField = "IdDepartamento";
                cbodepartamento.DataTextField = "Descripcion";
                cbodepartamento.DataBind();
            }
            catch (Exception ex)
            {
            }
        }
        protected void cbodepartamento_ServerChange(object sender, EventArgs e)
        {
            try
            {
                oListaProvincia = ServiceMantenimientoController.Instance.GetListaProvincia(Convert.ToInt32(cbodepartamento.Value));
                oListaProvincia.Insert(0, new Provincia() { IdProvincia = Constantes.ValorCero, Descripcion = Constantes.ValorSeleccione });
                cboprovincia.DataSource = oListaProvincia;
                cboprovincia.DataValueField = "IdProvincia";
                cboprovincia.DataTextField = "Descripcion";
                cboprovincia.DataBind();
            }
            catch (Exception ex)
            {

            }
            
        }

        ListaDistrito olista = new ListaDistrito();
        protected void cboprovincia_ServerChange(object sender, EventArgs e)
        {
            try
            {
                oListaDistrito = ServiceMantenimientoController.Instance.GetListaDistrito(Convert.ToInt32(cboprovincia.Value));
                oListaDistrito.Insert(0, new Distrito() { IdDistrito = Constantes.ValorCero, Descripcion = Constantes.ValorSeleccione });

                Session["SListaDistrito"] = oListaDistrito;

                cbodistrito.DataSource = oListaDistrito;
                cbodistrito.DataValueField = "IdDistrito";
                cbodistrito.DataTextField = "Descripcion";
                cbodistrito.DataBind();
            }
            catch (Exception ex)
            {
            }
        }

        protected void cbodistrito_ServerChange(object sender, EventArgs e)
        {
            try
            {
                var lista = (ListaDistrito)Session["SListaDistrito"];
                int sel = Convert.ToInt32(cbodistrito.Value);

                for (int i = 0; i <= lista.Count - 1; i++)
                {
                    if (lista[i].IdDistrito == sel)
                    {
                        txtubigeo.Value = lista[i].CodigoUbigeo;
                    }
                }
            }
            catch (Exception ex)
            {

            }
           
        }

        protected void cbotipodocumento_ServerChange(object sender, EventArgs e)
        {
            try
            {
                if (cbotipodocumento.SelectedIndex == Constantes.ValorDos)
                {
                    txtruc.MaxLength = Constantes.ValorOcho;
                }
                else if (cbotipodocumento.SelectedIndex == Constantes.ValorCuatro)
                {
                    txtruc.MaxLength = 11;

                }
            }
            catch (Exception ex)
            {

            }
           
        }
    }
}