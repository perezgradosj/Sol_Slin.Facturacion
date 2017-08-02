using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Slin.Facturacion.Common;
using Slin.Facturacion.Proxies.ServicioFacturacion;
using Slin.Facturacion.Electronica.Web.ServiceFacturacion;
using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;
using Slin.Facturacion.Common.Method;
using Slin.Facturacion.Electronica.Web.Views.Util;

namespace Slin.Facturacion.Electronica.Web.Views.Envio
{
    public partial class DocumentosEnviados : System.Web.UI.Page
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

        private ListaTipoDocumento listaTipoDocumento;
        public ListaTipoDocumento oListaTipoDocumento
        {
            get { return listaTipoDocumento; }
            set { listaTipoDocumento = value; }
        }


        private Documento odocumentoenviado;
        public Documento oDocumentoEnviado
        {
            get { return odocumentoenviado; }
            set { odocumentoenviado = value; }
        }

        private ListaDocumento olistadocumentoenviado;
        public ListaDocumento oListaDocumentoEnviado
        {
            get { return olistadocumentoenviado; }
            set { olistadocumentoenviado = value; }
        }

        private WCFSeguridad.Usuario ousuarioLogeado;
        public WCFSeguridad.Usuario oUsuarioLogeado
        {
            get { return ousuarioLogeado; }
            set { ousuarioLogeado = value; }
        }

        #endregion

        #region METHOD

        void ObtenerUsuarioLogeado()
        {
            oUsuarioLogeado = objController.GetUserLogueado_Fact();
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

                btnBuscar.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnNuevo.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
            }
            catch (Exception ex) { }
        }

        void CargarListas()
        {
            try
            {
                oListaTipoDocumento = ServiceFacturacionController.Instance.ListarTipoDocumento();
                cbotipodocumento.DataSource = oListaTipoDocumento;
                cbotipodocumento.DataValueField = "IdTipoDocumento";
                cbotipodocumento.DataTextField = "Descripcion";
                cbotipodocumento.DataBind();

                txtfechadesde.Value = DateTime.Now.AddDays(-7).ToShortDateString();
                txtfechahasta.Value = DateTime.Now.ToShortDateString();
            }
            catch (Exception ex)
            {

            }
            

        }

        void LlenaoObjetoBusqueda()
        {
            try
            {
                oDocumentoEnviado = new Documento();
                oDocumentoEnviado.Empresa = new Empresa();
                oDocumentoEnviado.TipoDocumento = new TipoDocumento();

                oDocumentoEnviado.FechaDesde = txtfechadesde.Value.Length == Constantes.ValorCero ? string.Empty : txtfechadesde.Value;
                oDocumentoEnviado.FechaHasta = txtfechahasta.Value.Length == Constantes.ValorCero ? string.Empty : txtfechahasta.Value;
                oDocumentoEnviado.Serie = txtserie.Value.Length == Constantes.ValorCero ? string.Empty : txtserie.Value;
                oDocumentoEnviado.TipoDocumento.IdTipoDocumento = Convert.ToInt32(cbotipodocumento.Value);
                oDocumentoEnviado.Empresa.RUC = oUsuarioLogeado.Empresa.RUC;
            }
            catch (Exception ex)
            {

            }
            
        }


        void CargarListaDocumentoEnviado()
        {
            try
            {
                ObtenerUsuarioLogeado();

                Session.Remove("ListaDocumentoEnviado");

                ValidarParametros();

                if (val == Constantes.ValorUno)
                {
                    Response.Write(Constantes.MensajeFecha);
                }
                else if (val == Constantes.ValorDos)
                {
                    LlenaoObjetoBusqueda();
                    oListaDocumentoEnviado = ServiceFacturacionController.Instance.GetListaDocumentoEnviado(oDocumentoEnviado);
                    GVDocumentoEnviado.DataSource = oListaDocumentoEnviado;
                    GVDocumentoEnviado.DataBind();
                }
                else if (val == Constantes.ValorCero)
                {
                    LlenaoObjetoBusqueda();
                    oListaDocumentoEnviado = ServiceFacturacionController.Instance.GetListaDocumentoEnviado(oDocumentoEnviado);
                    GVDocumentoEnviado.DataSource = oListaDocumentoEnviado;
                    GVDocumentoEnviado.DataBind();
                }
                Session["ListaDocumentoEnviado"] = oListaDocumentoEnviado;
            }
            catch (Exception ex)
            {

            }
            
        }

        public int val = Constantes.ValorCero;
        void ValidarParametros()
        {
            try
            {
                val = Singleton.Instance.Validate_RangeDate(txtfechadesde.Value, txtfechahasta.Value);
            }
            catch (Exception ex) { }
        }

        void BuscarDocumentosEnviados()
        {
            try
            {
                GVDocumentoEnviado.PageIndex = 0;
                GVDocumentoEnviado.DataSourceID = "";
                GVDocumentoEnviado.DataBind();
                CargarListaDocumentoEnviado();
            }
            catch (Exception ex) { }
        }

        #endregion

        protected void btnBuscar_ServerClick(object sender, EventArgs e)
        {
            BuscarDocumentosEnviados();
        }
    }
}