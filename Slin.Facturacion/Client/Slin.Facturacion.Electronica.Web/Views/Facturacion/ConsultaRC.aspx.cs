using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Slin.Facturacion.Common;
using Slin.Facturacion.Proxies.ServicioFacturacion;
using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;
using Slin.Facturacion.Electronica.Web.ServiceFacturacion;
using System.IO;
using System.Xml;
using System.Text;
using Slin.Facturacion.Electronica.Web.Views.Util;
using Slin.Facturacion.Common.Method;

namespace Slin.Facturacion.Electronica.Web.Views.Facturacion
{
    public partial class ConsultaRC : System.Web.UI.Page
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

        Serie oSerie = new Serie();

        ListaSerie oListaSerie = new ListaSerie();
        ListaEstado oListaEstado = new ListaEstado();

        private FacturaElectronica odocumentoRC;
        public FacturaElectronica oDocumentoRC
        {
            get { return odocumentoRC; }
            set { odocumentoRC = value; }
        }

        private ListaFacturaElectronica olistaDocumentoRC;
        public ListaFacturaElectronica oListaDocumentoRC
        {
            get { return olistaDocumentoRC; }
            set { olistaDocumentoRC = value; }
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
                btnNuevo.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);

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
                
                oDocumentoRC = new FacturaElectronica();
                oDocumentoRC.Estado = new Estado();
                oDocumentoRC.Empresa = new Empresa();

                oDocumentoRC.FechaInicio = txtfechadesde.Value.Length == Constantes.ValorCero ? string.Empty : txtfechadesde.Value;
                //oDocumentoRC.FechaInicio = txtfechadesde.InnerText.Length == Constantes.ValorCero ? string.Empty : txtfechadesde.InnerText;

                oDocumentoRC.FechaFin = txtfechahasta.Value.Length == Constantes.ValorCero ? string.Empty : txtfechahasta.Value;
                //oDocumentoRC.Estado.IdEstado = Convert.ToInt32(cboestado.Value);
                oDocumentoRC.Estado.IdEstado = Constantes.ValorCero;
                oDocumentoRC.TipoFecha = Convert.ToInt32(cbotipofecha.Value);

                oDocumentoRC.Empresa.RUC = oUsuarioLogeado.Empresa.RUC;
            }
            catch (Exception ex)
            {

            }
            
        }

        void CargarListaRC()
        {
            try
            {
                Session.Remove("ListadoResumenRC");

                ValidarParametros();

                if (val == Constantes.ValorUno)
                {
                    Response.Write(Constantes.MensajeFecha);
                }
                else if (val == Constantes.ValorDos)
                {
                    LlenarObjetoBusqueda();

                    oListaDocumentoRC = ServiceFacturacionController.Instance.GetListaCabeceraRC(oDocumentoRC);

                    GVListadoRC.DataSource = oListaDocumentoRC;
                    GVListadoRC.DataBind();
                }
                else if (val == Constantes.ValorCero)
                {
                    LlenarObjetoBusqueda();
                    oListaDocumentoRC = ServiceFacturacionController.Instance.GetListaCabeceraRC(oDocumentoRC);

                    GVListadoRC.DataSource = oListaDocumentoRC;
                    GVListadoRC.DataBind();
                }
                Session["ListadoResumenRC"] = oListaDocumentoRC;
            }
            catch (Exception ex) { }
        }

        void BuscarResumenRC()
        {
            try
            {
                GVListadoRC.PageIndex = 0;
                GVListadoRC.DataSourceID = "";
                GVListadoRC.DataBind();
                CargarListaRC();
            }
            catch (Exception ex) { }   
        }

        #endregion

        protected void btnBuscar_ServerClick(object sender, EventArgs e)
        {
            BuscarResumenRC();
        }

        protected void btnImgXML_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                oListaDocumentoRC = (ListaFacturaElectronica)Session["ListadoResumenRC"];
                ImageButton btnDetalles = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btnDetalles.NamingContainer;

                //string correlativo = GVListadoRC.Rows[gvrow.RowIndex].Cells[2].Text.ToString();
                string ArchivoXmlRC = string.Empty;
                string NombreArchivoXML = GVListadoRC.Rows[gvrow.RowIndex].Cells[2].Text.TrimEnd();

                for (int i = 0; i <= oListaDocumentoRC.Count - 1; i++)
                {
                    if (NombreArchivoXML == oListaDocumentoRC[i].NombreArchivoXML.TrimEnd())
                    {
                        ArchivoXmlRC = oListaDocumentoRC[i].XML;
                        //NombreArchivoXML = oListaDocumentoRC[i].NombreArchivoXML.TrimEnd();
                        break;
                    }
                }
                if (ArchivoXmlRC.Length < 50)
                {
                    Response.Write("<script language=javascript>alert('Este Doc. no tiene Xml Firmado');</script>");
                    return;
                }
                Singleton.Instance.CreateDirectory(Server.MapPath("~/DocumentoXML/"));

                var xmldoc = new XmlDocument();
                xmldoc.InnerXml = ArchivoXmlRC;
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

        private FacturaElectronica odetalleRC;
        public FacturaElectronica oDetalleRC
        {
            get { return odetalleRC; }
            set { odetalleRC = value; }
        }

        private ListaDetalleFacturaElectronica olistaDetalleRC;
        public ListaDetalleFacturaElectronica oListaDetalleRC
        {
            get { return olistaDetalleRC; }
            set { olistaDetalleRC = value; }
        }
        #endregion

        string nombreArchivo = string.Empty;
        protected void btnVerDetalle_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                oDetalleRC = new FacturaElectronica();
                oDetalleRC.Empresa = new Empresa();
                oDetalleRC.Estado = new Estado();

                oListaDocumentoRC = (ListaFacturaElectronica)Session["ListadoResumenRC"];

                ImageButton btnDetalleRC = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btnDetalleRC.NamingContainer;

                nombreArchivo = GVListadoRC.Rows[gvrow.RowIndex].Cells[2].Text.TrimEnd();


                for (int i = 0; i <= oListaDocumentoRC.Count - 1; i++)
                {
                    if (nombreArchivo == oListaDocumentoRC[i].NombreArchivoXML.TrimEnd())
                    {
                        oDetalleRC.IdFactura = oListaDocumentoRC[i].IdFactura;
                        oDetalleRC.FechaEnvio = oListaDocumentoRC[i].FechaEnvio;
                        oDetalleRC.Secuencia = oListaDocumentoRC[i].Secuencia;
                        oDetalleRC.NumeroAtencion = oListaDocumentoRC[i].NumeroAtencion;
                        oDetalleRC.Empresa.RUC = oListaDocumentoRC[i].Empresa.RUC;
                        oDetalleRC.FechaInicio = oListaDocumentoRC[i].FechaInicio2.ToShortDateString();
                        oDetalleRC.Estado.IdEstado = oListaDocumentoRC[i].Estado.IdEstado;
                        oDetalleRC.Estado.Descripcion = oListaDocumentoRC[i].Estado.Descripcion;
                        oDetalleRC.MensajeEnvio = oListaDocumentoRC[i].MensajeEnvio;

                        oDetalleRC.MensajeEnvioDetalle = oListaDocumentoRC[i].MensajeEnvioDetalle;

                        oListaDetalleRC = ServiceFacturacionController.Instance.GetListaDetalleRC(oDetalleRC);
                        break;
                    }
                }
                Session["objDetalleRC"] = oDetalleRC;
                Session["objListaDetalleRC"] = oListaDetalleRC;

                string form = "../../Views/Facturacion/DetResumenRC";
                string script = @"<script type='text/javascript'>";
                script += "window.open('" + form + "',null,'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=1,width=890,height=430,left=100,top=100');";
                script += @"</script>";
                Response.Write(script);
            }
            catch (Exception ex) { }   
        }
    }

}