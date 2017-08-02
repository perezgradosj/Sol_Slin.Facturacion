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
using Slin.Facturacion.Electronica.Web.Views.Util;
using Slin.Facturacion.Common.Method;

namespace Slin.Facturacion.Electronica.Web.Views.Facturacion
{
    public partial class ConsultaDocumentoRecibido : System.Web.UI.Page
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

        private ListaFacturaElectronica listdocelectr;
        public ListaFacturaElectronica ListDocElectr
        {
            get { return listdocelectr; }
            set { listdocelectr = value; }
        }

        private FacturaElectronica objdocelectronico;
        public FacturaElectronica objDocElectronico
        {
            get { return objdocelectronico; }
            set { objdocelectronico = value; }
        }

        private DetalleFacturaElectronica objdetdocelectronico;
        public DetalleFacturaElectronica objDetDocElectronico
        {
            get { return objdetdocelectronico; }
            set { objdetdocelectronico = value; }
        }

        private WCFSeguridad.Usuario ousuarioLogeado;
        public WCFSeguridad.Usuario oUsuarioLogeado
        {
            get { return ousuarioLogeado; }
            set { ousuarioLogeado = value; }
        }

        private ListaTipoDocumento objlistatipodocumento;
        public ListaTipoDocumento objListaTipoDocumento
        {
            get { return objlistatipodocumento; }
            set { objlistatipodocumento = value; }
        }

        private ListaSerie objlistaserie;
        public ListaSerie objListaSerie
        {
            get { return objlistaserie; }
            set { objlistaserie = value; }
        }

        private ListaEstado objlistaestado;
        public ListaEstado objListaEstado
        {
            get { return objlistaestado; }
            set { objlistaestado = value; }
        }

        private Serie objserie;
        public Serie objSerie
        {
            get { return objserie; }
            set { objserie = value; }
        }

        #endregion


        #region METHOD

        WCFSeguridad.ListaRol oListaRolesUserLog = new WCFSeguridad.ListaRol();
        void CapturarUserLogRoles()
        {
            try
            {
                oListaRolesUserLog = objController.GetList_RolUserLog();
                btnBuscar.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnExportarExcel.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnExportarExcel.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
            }
            catch (Exception ex) { }
        }

        private void ObtenerUsuarioLogeado()
        {
            try
            {
                oUsuarioLogeado = objController.GetUserLogueado_Fact();
            }
            catch (Exception ex) { }
        }

        private void Cargar()
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

        private void CargarListas()
        {
            try
            {
                objListaTipoDocumento = ServiceFacturacionController.Instance.ListarTipoDocumento();
                objListaTipoDocumento.RemoveAt(0);
                objListaTipoDocumento.Insert(0, new TipoDocumento() { IdTipoDocumento = Constantes.ValorCero, Descripcion = Constantes.ValorTodos });

                cbotipodocumento.DataSource = objListaTipoDocumento;
                cbotipodocumento.DataValueField = "IdTipoDocumento";
                cbotipodocumento.DataTextField = "Descripcion";
                cbotipodocumento.DataBind();

                objListaEstado = ServiceFacturacionController.Instance.ListarEstadoDocumento();
                objListaEstado.RemoveAt(0);
                objListaEstado.Insert(0, new Estado() { IdEstado = Constantes.ValorCero, Descripcion = Constantes.ValorTodos });

                cboestado.DataSource = objListaEstado;
                cboestado.DataValueField = "IdEstado";
                cboestado.DataTextField = "Descripcion";
                cboestado.DataBind();

                ObtenerDatosSerie();

                objListaSerie = ServiceFacturacionController.Instance.ListarSerie(objSerie);
                objListaSerie.Insert(0, new Serie() { IdSerie = Constantes.ValorCero, NumeroSerie = Constantes.ValorTodos });

                cboserie.DataSource = objListaSerie;
                cboserie.DataValueField = "IdSerie";
                cboserie.DataValueField = "NumeroSerie";
                cboserie.DataBind();

                txtfechadesde.Value = DateTime.Now.AddDays(-7).ToShortDateString();
                txtfechahasta.Value = DateTime.Now.ToString("dd/MM/yyyy");
            }
            catch (Exception ex) { }
        }

        private void ObtenerDatosSerie()
        {
            try
            {
                objSerie = new Serie();
                objSerie.Empresa = new Empresa();
                objSerie.TipoDocumento = new TipoDocumento();

                objSerie.TipoDocumento.IdTipoDocumento = Constantes.ValorCero;
                //objSerie.TipoDocumento.CodigoDocumento = string.Empty;
                objSerie.Empresa.RUC = oUsuarioLogeado.Empresa.RUC;
            }
            catch (Exception ex) { }
        }

        private void ListarSerie()
        {
            try
            {
                ObtenerUsuarioLogeado();

                objSerie = new Serie();
                objSerie.Empresa = new Empresa();
                objSerie.TipoDocumento = new TipoDocumento();

                objSerie.TipoDocumento.CodigoDocumento = cbotipodocumento.Value;
                objSerie.Empresa.RUC = oUsuarioLogeado.Empresa.RUC;

                objListaSerie = ServiceFacturacionController.Instance.ListarSerie(objSerie);
                objListaSerie.Insert(0, new Serie() { IdSerie = Constantes.ValorCero, NumeroSerie = Constantes.ValorTodos });

                cboserie.DataSource = objListaSerie;
                cboserie.DataValueField = "IdSerie";
                cboserie.DataTextField = "NumeroSerie";
                cboserie.DataBind();
            }
            catch (Exception ex) { }
        }

        private void ObtenerDatos()
        {
            try
            {
                ObtenerUsuarioLogeado();

                objDocElectronico = new FacturaElectronica();
                objDocElectronico.Serie = new Serie();
                objDocElectronico.Estado = new Estado();
                objDocElectronico.Empresa = new Empresa();
                objDocElectronico.Cliente = new Cliente();
                objDocElectronico.TipoDocumento = new TipoDocumento();

                objDocElectronico.FechaInicio = txtfechadesde.Value;
                objDocElectronico.FechaFin = txtfechahasta.Value;
                objDocElectronico.TipoDocumento.CodigoDocumento = cbotipodocumento.Value;
                objDocElectronico.Serie.NumeroSerie = cboserie.Value == Constantes.ValorTodos ? string.Empty : cboserie.Value;

                objDocElectronico.NumeroDocumentoInicio = txtnuminicio.Value != null ? txtnuminicio.Value : string.Empty;
                objDocElectronico.NumeroDocumentoFin = txtnumfin.Value != null ? txtnumfin.Value : string.Empty;
                objDocElectronico.Estado.IdEstado = int.Parse(cboestado.Value);
                objDocElectronico.Empresa.RUC = txtrucEmisor.Value != null ? txtrucEmisor.Value : string.Empty;
                objDocElectronico.Empresa.RazonSocial = txtrazonsocialEmisor.Value != null ? txtrazonsocialEmisor.Value : string.Empty;
                objDocElectronico.Cliente.ClienteRuc = oUsuarioLogeado.Empresa.RUC;
            }
            catch (Exception ex) { }
        }


        private int Inicio = Constantes.ValorCero;
        private int Fin = Constantes.ValorCero;
        private int val = Constantes.ValorCero;

        private void ValidarParametros()
        {
            try
            {
                val = Singleton.Instance.Validate_RangeDate(txtfechadesde.Value, txtfechahasta.Value);
                Inicio = txtnuminicio.Value == string.Empty ? Constantes.ValorCero : Convert.ToInt32(txtnuminicio.Value);
                Fin = txtnumfin.Value == string.Empty ? Constantes.ValorCero : Convert.ToInt32(txtnumfin.Value);
            }
            catch (Exception ex) { }
        }

        private void CargarData()
        {
            try
            {
                ObtenerUsuarioLogeado();

                Session.Remove("ListaDocumentoReceibed");
                ValidarParametros();
                if (Inicio > Fin)
                { Response.Write(Constantes.MensajeNumDoc); }
                else if (val == Constantes.ValorUno)
                { Response.Write(Constantes.MensajeFecha); }
                else if (val == Constantes.ValorDos)
                {
                    ObtenerDatos();
                    ListDocElectr = ServiceFacturacionController.Instance.GetListaDocumentReceived(objDocElectronico);
                    GVDocElectronico.DataSource = ListDocElectr;
                    GVDocElectronico.DataBind();
                }
                else if (val == Constantes.ValorCero)
                {
                    ObtenerDatos();
                    ListDocElectr = ServiceFacturacionController.Instance.GetListaDocumentReceived(objDocElectronico);
                    GVDocElectronico.DataSource = ListDocElectr;
                    GVDocElectronico.DataBind();
                }
                Session["ListaDocumentoReceibed"] = ListDocElectr;
            }
            catch (Exception ex) { }
        }

        #endregion

        protected void GVDocElectronico_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void btnBuscar_ServerClick(object sender, EventArgs e)
        {
            try
            {
                GVDocElectronico.PageIndex = Constantes.ValorCero;
                GVDocElectronico.DataSource = string.Empty;
                GVDocElectronico.DataBind();
                CargarData();

                string script = @"<script type='text/javascript'></script>";
                Response.Write(script);
            }
            catch (Exception ex) { }
        }

        protected void cbotipodocumento_ServerChange(object sender, EventArgs e)
        {
            try
            {
                ListarSerie();
                string script = @"<script type='text/javascript'></script>";
                Response.Write(script);
            }
            catch (Exception ex) { }
        }


        string tpodocumento = string.Empty;
        string serie = string.Empty;
        string nrodocumento = string.Empty;
        string NombreArchivo = string.Empty;

        string EmisorRuc = string.Empty;

        protected void btnImgXML_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ObtenerUsuarioLogeado();

                ListDocElectr = (ListaFacturaElectronica)Session["ListaDocumentoReceibed"];
                ImageButton btn = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btn.NamingContainer;

                string xmltoGenerar = string.Empty;

                tpodocumento = GVDocElectronico.Rows[gvrow.RowIndex].Cells[2].Text;
                serie = GVDocElectronico.Rows[gvrow.RowIndex].Cells[3].Text;
                nrodocumento = GVDocElectronico.Rows[gvrow.RowIndex].Cells[4].Text;
                EmisorRuc = GVDocElectronico.Rows[gvrow.RowIndex].Cells[6].Text;
                for (int i = 0; i <= ListDocElectr.Count - 1; i++)
                {
                    if (EmisorRuc == ListDocElectr[i].Empresa.RUC && serie == ListDocElectr[i].Serie.NumeroSerie && nrodocumento == ListDocElectr[i].NumeroDocumento && tpodocumento == ListDocElectr[i].TipoDocumento.CodigoDocumento)
                    {
                        NombreArchivo = ListDocElectr[i].NombreArchivoXML.TrimEnd();
                        xmltoGenerar = ListDocElectr[i].XML;
                        break;
                    }
                }

                if (xmltoGenerar.Length < 50)
                {
                    Response.Write("<script language=javascript>alert('Este Doc. no tiene Xml Firmado');</script>");
                    return;
                }
                Singleton.Instance.CreateDirectory(Server.MapPath("~/DocumentoXML/"));

                var xmldoc = new XmlDocument();
                xmldoc.InnerXml = xmltoGenerar;
                xmldoc.Save(Server.MapPath("~/DocumentoXML/" + NombreArchivo + ".xml"));

                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + NombreArchivo + ".xml");
                Response.WriteFile("../../DocumentoXML/" + NombreArchivo + ".xml");
                Response.Flush();

                var file = Path.Combine(HttpContext.Current.Server.MapPath("../../DocumentoXML/" + NombreArchivo + ".xml"));
                if (System.IO.File.Exists(file))
                    System.IO.File.Delete(file);
                Response.End();
            }
            catch (Exception ex) { }
        }

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {

        }

    }
}