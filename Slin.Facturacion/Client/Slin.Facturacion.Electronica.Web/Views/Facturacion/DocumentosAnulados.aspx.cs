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
using Slin.Facturacion.Electronica.Web.Views.Util;
using Slin.Facturacion.Common.Method;

namespace Slin.Facturacion.Electronica.Web.Views.Facturacion
{
    public partial class DocumentosAnulados : System.Web.UI.Page
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

        private ListaFacturaElectronica olistaDocumentoAnulado;
        public ListaFacturaElectronica oListaDocumentoAnulado
        {
            get { return olistaDocumentoAnulado; }
            set { olistaDocumentoAnulado = value; }
        }

        private ListaTipoDocumento listatipoDocumento;
        public ListaTipoDocumento ListaTipoDocumento
        {
            get { return listatipoDocumento; }
            set { listatipoDocumento = value; }
        }

        private FacturaElectronica odocumentoAnulado;
        public FacturaElectronica oDocumentoAnulado
        {
            get { return odocumentoAnulado; }
            set { odocumentoAnulado = value; }
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
                btnNuevo.Visible = false;
                ObtenerUsuarioLogeado();
                lblempresa.InnerText = oUsuarioLogeado.Empresa.RazonSocial.ToUpper();

                CargarLista();
                CapturarUserLogRoles();

                logoEmpresa.Src = "../../Img/home/" + oUsuarioLogeado.Empresa.RUC + ".png";
            }
            catch (Exception ex) { }
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

        public Int32 val = Constantes.ValorCero;
        //public DateTime fecha1;
        //public DateTime fecha2;

        void ValidarParametros()
        {
            try
            {
                //if (txtfechadesde.Value.Length > 0)
                //{
                //    fecha1 = Convert.ToDateTime(txtfechadesde.Value);
                //    fecha2 = Convert.ToDateTime(txtfechahasta.Value);
                //    int result = DateTime.Compare(fecha1, fecha2);
                //    val = result < 0 ? 2 : result;
                //}
                //else
                //{
                //    val = Constantes.ValorDos;
                //}
                val = Singleton.Instance.Validate_RangeDate(txtfechadesde.Value, txtfechahasta.Value);
            }
            catch (Exception ex) { }
        }


        void CargarLista()
        {
            try
            {
                ListaTipoDocumento = ServiceFacturacionController.Instance.ListarTipoDocumento();

                cbotipodocumento.DataSource = ListaTipoDocumento;
                cbotipodocumento.DataValueField = "IdTipoDocumento";
                cbotipodocumento.DataTextField = "Descripcion";
                cbotipodocumento.DataBind();

                txtfechadesde.Value = DateTime.Now.AddDays(-7).ToShortDateString();
                txtfechahasta.Value = DateTime.Now.ToShortDateString();
            }
            catch (Exception ex) { }
        }

        void LlenarObjetoBusqueda()
        {
            try
            {
                ObtenerUsuarioLogeado();

                oDocumentoAnulado = new FacturaElectronica();
                oDocumentoAnulado.Serie = new Serie();
                oDocumentoAnulado.Empresa = new Empresa();
                oDocumentoAnulado.TipoDocumento = new TipoDocumento();


                oDocumentoAnulado.FechaInicio = txtfechadesde.Value.Length == Constantes.ValorCero ? string.Empty : txtfechadesde.Value;
                oDocumentoAnulado.FechaFin = txtfechahasta.Value.Length == Constantes.ValorCero ? string.Empty : txtfechahasta.Value;
                oDocumentoAnulado.Serie.NumeroSerie = txtserie.Value == null ? string.Empty : txtserie.Value;
                oDocumentoAnulado.TipoDocumento.IdTipoDocumento = Convert.ToInt32(cbotipodocumento.Value) == Constantes.ValorCero ? Constantes.ValorCero : Convert.ToInt32(cbotipodocumento.Value);
                oDocumentoAnulado.Empresa.RUC = oUsuarioLogeado.Empresa.RUC;
            }
            catch (Exception ex) { }
        }

        void CargarListaDocumendoAnulado()
        {
            try
            {
                ObtenerUsuarioLogeado();

                Session.Remove("ListaDocumentoAnulado");

                ValidarParametros();

                if (val == Constantes.ValorUno)
                {
                    Response.Write(Constantes.MensajeFecha);
                }
                else if(val == Constantes.ValorDos)
                {
                    LlenarObjetoBusqueda();
                    oListaDocumentoAnulado = ServiceFacturacionController.Instance.GetListaDocumentoAnulado(oDocumentoAnulado);
                    GVListaAnulados.DataSource = oListaDocumentoAnulado;
                    GVListaAnulados.DataBind();
                }
                else if (val == Constantes.ValorCero)
                {
                    LlenarObjetoBusqueda();
                    oListaDocumentoAnulado = ServiceFacturacionController.Instance.GetListaDocumentoAnulado(oDocumentoAnulado);
                    GVListaAnulados.DataSource = oListaDocumentoAnulado;
                    GVListaAnulados.DataBind();
                }
                Session["ListaDocumentoAnulado"] = oListaDocumentoAnulado;
            }
            catch (Exception ex) { }
        }


        void BuscarDocumentosAnulados()
        {
            try
            {
                GVListaAnulados.PageIndex = 0;
                GVListaAnulados.DataSourceID = "";
                GVListaAnulados.DataBind();
                CargarListaDocumendoAnulado();
            }
            catch (Exception ex) { }
        }

        #endregion


        protected void btnBuscar_ServerClick(object sender, EventArgs e)
        {
            BuscarDocumentosAnulados();
        }

        protected void GVListaAnulados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string desc = e.Row.Cells[7].Text;
                    string desc2 = e.Row.Cells[7].Text;
                    string desc3 = e.Row.Cells[7].Text;

                    if (desc.Contains("pendiente") || desc.Contains("Pendiente"))
                    {
                        e.Row.ForeColor = System.Drawing.Color.Blue;
                    }

                    if (desc2.Contains("enviado a Sunat") || desc2.Contains("enviado a sunat"))
                    {
                        e.Row.ForeColor = System.Drawing.Color.DarkRed;
                    }

                    if (desc2.Contains("aceptado por Sunat") || desc2.Contains("aceptado por sunat"))
                    {
                        e.Row.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            catch (Exception ex) { }
        }
    }
}