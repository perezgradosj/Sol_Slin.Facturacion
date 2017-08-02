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
using System.IO;
using System.Xml;
using System.Text;
using Slin.Facturacion.Electronica.Web.Views.Util;
using Slin.Facturacion.Common.Method;

namespace Slin.Facturacion.Electronica.Web.Views.Facturacion
{
    public partial class ConsultaRA : System.Web.UI.Page
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

        private Serie oserie;
        public Serie oSerie
        {
            get { return oserie; }
            set { oserie = value; }
        }

        private ListaTipoDocumento olistaTipoDocumento;
        public ListaTipoDocumento oListaTipoDocumento
        {
            get { return olistaTipoDocumento; }
            set { olistaTipoDocumento = value; }
        }


        private ListaSerie olistaSerie;
        public ListaSerie oListaSerie
        {
            get { return olistaSerie; }
            set { olistaSerie = value; }
        }


        private FacturaElectronica odocumentoRA;
        public FacturaElectronica oDocumentoRA
        {
            get { return odocumentoRA; }
            set { odocumentoRA = value; }
        }

        private ListaFacturaElectronica olistaDocumentoRA;
        public ListaFacturaElectronica oListaDocumentoRA
        {
            get { return olistaDocumentoRA; }
            set { olistaDocumentoRA = value; }
        }

        private ListaEstado olistaTipoFecha;
        public ListaEstado oListaTipoFecha
        {
            get { return olistaTipoFecha; }
            set { olistaTipoFecha = value; }
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
                ObtenerUsuarioLogeado();
                lblempresa.InnerText = oUsuarioLogeado.Empresa.RazonSocial.ToUpper();
                CargarListas();
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

        void ObtenerDatosSerie()
        {
            try
            {
                oSerie = new Serie();
                oSerie.TipoDocumento = new TipoDocumento();
                oSerie.Empresa = new Empresa();

                oSerie.TipoDocumento.IdTipoDocumento = Constantes.ValorCero;
                oSerie.Empresa.IdEmpresa = Constantes.ValorCero;
            }
            catch (Exception ex)
            {

            }
            
        }

        void CargarListas()
        {
            try
            {
                cboestado.DataSource = ServiceFacturacionController.Instance.ListarEstadoDocumento();
                cboestado.DataValueField = "IdEstado";
                cboestado.DataTextField = "Descripcion";
                cboestado.DataBind();

                oListaTipoFecha = ServiceFacturacionController.Instance.GetListaTipoFecha();
                oListaTipoFecha.RemoveAt(0);
                oListaTipoFecha.Insert(0, new Estado() { IdEstado = Constantes.ValorUno, Descripcion = "Fecha Anulado" });
                cbotipofecha.DataSource = oListaTipoFecha;
                cbotipofecha.DataValueField = "IdEstado";
                cbotipofecha.DataTextField = "Descripcion";
                cbotipofecha.DataBind();

                txtfechadesde.Value = DateTime.Now.AddDays(-7).ToShortDateString();
                txtfechahasta.Value = DateTime.Now.ToShortDateString();
                //txtfechahasta.Value = DateTime.Now.AddDays(7).ToString("dd/MM/yyyy");
            }
            catch (Exception ex) { }
        }

        public Int32 val = Constantes.ValorCero;
        void ValidarParametros()
        {
            try
            {
                val = Singleton.Instance.Validate_RangeDate(txtfechadesde.Value, txtfechahasta.Value);
            }
            catch (Exception ex) { }
        }

        void LlenarObjetoBusqueda()
        {
            try
            {
                ObtenerUsuarioLogeado();

                oDocumentoRA = new FacturaElectronica();
                oDocumentoRA.Estado = new Estado();
                oDocumentoRA.Empresa = new Empresa();

                oDocumentoRA.FechaInicio = txtfechadesde.Value.Length == Constantes.ValorCero ? string.Empty : txtfechadesde.Value;
                oDocumentoRA.FechaFin = txtfechahasta.Value.Length == Constantes.ValorCero ? string.Empty : txtfechahasta.Value;
                //oDocumentoRC.Estado.IdEstado = Convert.ToInt32(cboestado.Value);
                oDocumentoRA.Estado.IdEstado = Constantes.ValorCero;
                oDocumentoRA.TipoFecha = Convert.ToInt32(cbotipofecha.Value);

                oDocumentoRA.Empresa.RUC = oUsuarioLogeado.Empresa.RUC;
            }
            catch (Exception ex)
            {

            }
            
        }

        void CargarListaRA()
        {
            try
            {
                Session.Remove("ListadoResumenRA");

                ValidarParametros();

                if (val == Constantes.ValorUno)
                {
                    Response.Write(Constantes.MensajeFecha);
                }
                else if(val == Constantes.ValorDos)
                {
                    LlenarObjetoBusqueda();

                    oListaDocumentoRA = ServiceFacturacionController.Instance.GetListaCabeceraRA(oDocumentoRA);

                    GVListadoRA.DataSource = oListaDocumentoRA;
                    GVListadoRA.DataBind();
                }
                else if(val == Constantes.ValorCero)
                {
                    LlenarObjetoBusqueda();
                    oListaDocumentoRA = ServiceFacturacionController.Instance.GetListaCabeceraRA(oDocumentoRA);

                    GVListadoRA.DataSource = oListaDocumentoRA;
                    GVListadoRA.DataBind();
                }
                Session["ListadoResumenRA"] = oListaDocumentoRA;                
            }
            catch (Exception ex)
            {

            }
            
        }

        void BuscarResumenRA()
        {
            try
            {
                GVListadoRA.PageIndex = 0;
                GVListadoRA.DataSourceID = "";
                GVListadoRA.DataBind();
                CargarListaRA();
            }
            catch (Exception ex) { }
        }

        #endregion


        protected void btnBuscar_ServerClick(object sender, EventArgs e)
        {
            BuscarResumenRA();
        }

        protected void btnImgXML_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                oListaDocumentoRA = (ListaFacturaElectronica)Session["ListadoResumenRA"];
                ImageButton btnDetalles = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btnDetalles.NamingContainer;

                string ArchivoXmlRA = string.Empty;
                string NombreArchivoXML = GVListadoRA.Rows[gvrow.RowIndex].Cells[2].Text.TrimEnd();

                //int IdRac = Convert.ToInt32(GVListadoRA.Rows[gvrow.RowIndex].Cells[2].Text.ToString());
                for (int i = 0; i <= oListaDocumentoRA.Count - 1; i++)
                {
                    if (NombreArchivoXML == oListaDocumentoRA[i].NombreArchivoXML.TrimEnd())
                    {
                        ArchivoXmlRA = oListaDocumentoRA[i].XML;
                        //NombreArchivo = oListaDocumentoRA[i].NombreArchivoXML.TrimEnd();
                        break;
                    }
                }
                if (ArchivoXmlRA.Length < 50)
                {
                    Response.Write("<script language=javascript>alert('Este Doc. no tiene Xml Firmado');</script>");
                    return;
                }

                Singleton.Instance.CreateDirectory(Server.MapPath("~/DocumentoXML/"));

                var xmldoc = new XmlDocument();
                xmldoc.InnerXml = ArchivoXmlRA;
                xmldoc.Save(Server.MapPath("~/DocumentoXML/" + NombreArchivoXML + ".xml"));

                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + NombreArchivoXML + ".xml");
                Response.WriteFile("../../DocumentoXML/" + NombreArchivoXML + ".xml");
                Response.Flush();

                var file = Path.Combine(HttpContext.Current.Server.MapPath("../../DocumentoXML/" + NombreArchivoXML + ".xml"));
                if (System.IO.File.Exists(file))
                    System.IO.File.Delete(file);
                Response.End();
            }
            catch (Exception ex) { }
        }

        #region Ver Detalle

        private FacturaElectronica odetalleRA;
        public FacturaElectronica oDetalleRA
        {
            get { return odetalleRA; }
            set { odetalleRA = value; }
        }

        private ListaDetalleFacturaElectronica olistaDetalleRA;
        public ListaDetalleFacturaElectronica oListaDetalleRA
        {
            get { return olistaDetalleRA; }
            set { olistaDetalleRA = value; }
        }

        
        #endregion

        string nombreArchivo = string.Empty;
        protected void btnVerDetalle_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                oDetalleRA = new FacturaElectronica();
                oDetalleRA.Empresa = new Empresa();
                oDetalleRA.Estado = new Estado();

                oListaDocumentoRA = (ListaFacturaElectronica)Session["ListadoResumenRA"];

                ImageButton btnDetalleRA = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btnDetalleRA.NamingContainer;

                nombreArchivo = GVListadoRA.Rows[gvrow.RowIndex].Cells[2].Text.TrimEnd();

                for (int i = 0; i <= oListaDocumentoRA.Count - 1; i++)
                {
                    if (nombreArchivo == oListaDocumentoRA[i].NombreArchivoXML.TrimEnd())
                    {
                        oDetalleRA.IdFactura = oListaDocumentoRA[i].IdFactura;
                        oDetalleRA.NumeroAtencion = oListaDocumentoRA[i].NumeroAtencion;
                        oDetalleRA.Secuencia = oListaDocumentoRA[i].Secuencia;
                        //oDetalleRA.Empresa.IdEmpresa = oListaDocumentoRA[i].Empresa.IdEmpresa;
                        //oDetalleRA.Empresa.RUC = oListaDocumentoRA[i].Empresa.RUC;

                        oDetalleRA.Estado.IdEstado = oListaDocumentoRA[i].Estado.IdEstado;
                        oDetalleRA.Estado.Descripcion = oListaDocumentoRA[i].Estado.Descripcion;
                        oDetalleRA.FechaEnvio = oListaDocumentoRA[i].FechaEnvio;

                        oDetalleRA.FechaInicio = oListaDocumentoRA[i].FechaInicio2.ToShortDateString();


                        oDetalleRA.MensajeEnvio = oListaDocumentoRA[i].MensajeEnvio;

                        oDetalleRA.MensajeEnvioDetalle = oListaDocumentoRA[i].MensajeEnvioDetalle;

                        oListaDetalleRA = ServiceFacturacionController.Instance.GetListaDetalleRA(oDetalleRA);
                        break;
                    }
                }
                Session["objDetalleRA"] = oDetalleRA;
                Session["objListaDetalleRA"] = oListaDetalleRA;

                string form = "../../Views/Facturacion/DetResumenRA";
                string script = @"<script type='text/javascript'>";
                script += "window.open('" + form + "',null,'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=1,width=875,height=430,left=100,top=100');";
                script += @"</script>";
                Response.Write(script);
            }
            catch (Exception ex) { }
        }

    }
}