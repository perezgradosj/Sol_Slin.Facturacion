using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using System.Xml;
using System.Text;

using Slin.Facturacion.Common;
using Slin.Facturacion.Proxies.ServicioFacturacion;
using Slin.Facturacion.Electronica.Web.ServiceFacturacion;
using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;
using Slin.Facturacion.Electronica.Web.Views.Util;
using Slin.Facturacion.Common.Method;

namespace Slin.Facturacion.Electronica.Web.Views.Facturacion
{
    public partial class ConsultaRR : System.Web.UI.Page
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


        private FacturaElectronica odocumentoRR;
        public FacturaElectronica oDocumentoRR
        {
            get { return odocumentoRR; }
            set { odocumentoRR = value; }
        }

        private ListaFacturaElectronica olistaDocumentoRR;
        public ListaFacturaElectronica oListaDocumentoRR
        {
            get { return olistaDocumentoRR; }
            set { olistaDocumentoRR = value; }
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
                btnNuevoRR.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                
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
            catch (Exception ex) { }
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

                oDocumentoRR = new FacturaElectronica();
                oDocumentoRR.Estado = new Estado();
                oDocumentoRR.Empresa = new Empresa();

                oDocumentoRR.FechaInicio = txtfechadesde.Value.Length == Constantes.ValorCero ? string.Empty : txtfechadesde.Value;
                oDocumentoRR.FechaFin = txtfechahasta.Value.Length == Constantes.ValorCero ? string.Empty : txtfechahasta.Value;
                //oDocumentoRC.Estado.IdEstado = Convert.ToInt32(cboestado.Value);
                oDocumentoRR.Estado.IdEstado = Constantes.ValorCero;
                oDocumentoRR.TipoFecha = Convert.ToInt32(cbotipofecha.Value);

                oDocumentoRR.Empresa.RUC = oUsuarioLogeado.Empresa.RUC;
            }
            catch (Exception ex) { }
        }

        void CargarListaRA()
        {
            try
            {
                Session.Remove("ListadoResumenRR");

                ValidarParametros();

                if (val == Constantes.ValorUno)
                {
                    Response.Write(Constantes.MensajeFecha);
                }
                else if (val == Constantes.ValorDos)
                {
                    LlenarObjetoBusqueda();
                    oListaDocumentoRR = ServiceFacturacionController.Instance.GetListaCabeceraRR(oDocumentoRR);
                    GVListadoRR.DataSource = oListaDocumentoRR;
                    GVListadoRR.DataBind();
                }
                else if (val == Constantes.ValorCero)
                {
                    LlenarObjetoBusqueda();
                    oListaDocumentoRR = ServiceFacturacionController.Instance.GetListaCabeceraRR(oDocumentoRR);
                    GVListadoRR.DataSource = oListaDocumentoRR;
                    GVListadoRR.DataBind();
                }
                Session["ListadoResumenRR"] = oListaDocumentoRR;
            }
            catch (Exception ex) { }
        }

        void BuscarResumenRA()
        {
            try
            {
                GVListadoRR.PageIndex = 0;
                GVListadoRR.DataSourceID = "";
                GVListadoRR.DataBind();
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
                oListaDocumentoRR = (ListaFacturaElectronica)Session["ListadoResumenRR"];
                ImageButton btnDetalles = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btnDetalles.NamingContainer;

                string ArchivoXmlRA = string.Empty;
                string NombreArchivoXML = GVListadoRR.Rows[gvrow.RowIndex].Cells[2].Text.TrimEnd(); ;

                //int IdRac = Convert.ToInt32(GVListadoRA.Rows[gvrow.RowIndex].Cells[2].Text.ToString());
                for (int i = 0; i <= oListaDocumentoRR.Count - 1; i++)
                {
                    if (NombreArchivoXML == oListaDocumentoRR[i].NombreArchivoXML.TrimEnd())
                    {
                        ArchivoXmlRA = oListaDocumentoRR[i].XML;
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

                oListaDocumentoRR = (ListaFacturaElectronica)Session["ListadoResumenRR"];

                ImageButton btnDetalleRA = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btnDetalleRA.NamingContainer;

                nombreArchivo = GVListadoRR.Rows[gvrow.RowIndex].Cells[2].Text.TrimEnd();

                for (int i = 0; i <= oListaDocumentoRR.Count - 1; i++)
                {
                    if (nombreArchivo == oListaDocumentoRR[i].NombreArchivoXML.TrimEnd())
                    {
                        oDetalleRA.IdFactura = oListaDocumentoRR[i].IdFactura;
                        oDetalleRA.NumeroAtencion = oListaDocumentoRR[i].NumeroAtencion;
                        oDetalleRA.Secuencia = oListaDocumentoRR[i].Secuencia;
                        //oDetalleRA.Empresa.IdEmpresa = oListaDocumentoRA[i].Empresa.IdEmpresa;
                        //oDetalleRA.Empresa.RUC = oListaDocumentoRA[i].Empresa.RUC;

                        oDetalleRA.Estado.IdEstado = oListaDocumentoRR[i].Estado.IdEstado;
                        oDetalleRA.Estado.Descripcion = oListaDocumentoRR[i].Estado.Descripcion;
                        oDetalleRA.FechaEnvio = oListaDocumentoRR[i].FechaEnvio;

                        oDetalleRA.FechaInicio = oListaDocumentoRR[i].FechaInicio2.ToShortDateString();


                        oDetalleRA.MensajeEnvio = oListaDocumentoRR[i].MensajeEnvio;

                        oDetalleRA.MensajeEnvioDetalle = oListaDocumentoRR[i].MensajeEnvioDetalle;

                        oListaDetalleRA = ServiceFacturacionController.Instance.GetListaDetalleRA(oDetalleRA);
                        break;
                    }
                }
                Session["objDetalleRR"] = oDetalleRA;
                Session["objListaDetalleRR"] = oListaDetalleRA;

                string form = "../../Views/Facturacion/DetResumenRR";
                string script = @"<script type='text/javascript'>";
                script += "window.open('" + form + "',null,'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=1,width=875,height=430,left=100,top=100');";
                script += @"</script>";
                Response.Write(script);
            }
            catch (Exception ex) { }
        }
    }
}