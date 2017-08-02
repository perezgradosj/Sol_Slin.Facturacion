using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using System.Xml;
//using System.Drawing;

using Slin.Facturacion.Common;
using Slin.Facturacion.Proxies.ServicioFacturacion;
using Slin.Facturacion.Electronica.Web.ServiceFacturacion;
using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;

//using System.Globalization;

using xmlFac = Slin.Facturacion.Common.CE;
using xmlNotCred = Slin.Facturacion.Common.CENC;
using xmlNotDeb = Slin.Facturacion.Common.CEND;

using System.Xml.Serialization;
using System.Text;
using System.Configuration;
using Slin.Facturacion.Common.Helper;
using Slin.Facturacion.Common.Method;
using Slin.Facturacion.Electronica.Web.Views.Util;
using System.ComponentModel;

namespace Slin.Facturacion.Electronica.Web.Views.Facturacion
{
    public partial class ConsultaDocumento : System.Web.UI.Page
    {
        //private string PathWriteFile = ConfigurationManager.AppSettings["PathWriteFile"];


        private string PathLogSLINADE = ConfigurationManager.AppSettings["PathLogSLINADE"];
        string PathWriteFile = string.Empty;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                Cargar();
                Message();
                
            }
        }

        #region ENTITY
        ServiceWebFactController objController = new ServiceWebFactController();
        FacturaElectronica oFacturaElectronica = new FacturaElectronica();
        DetalleFacturaElectronica oDetalleFacturaElectronica = new DetalleFacturaElectronica();

        private WCFSeguridad.Usuario ousuarioLogeado;
        public WCFSeguridad.Usuario oUsuarioLogeado
        {
            get { return ousuarioLogeado; }
            set { ousuarioLogeado = value; }
        }

        private ListaFacturaElectronica listaFacturaElectronica;
        public ListaFacturaElectronica ListaFacturaElectronica
        {
            get { return listaFacturaElectronica; }
            set { listaFacturaElectronica = value; }
        }
        #endregion

        #region METHOD

        private void Message()
        {
            try
            {
                string respuesta = (string)Session["MsjeErrorCreatedPDF"];
                //if (respuesta.Length > Constantes.ValorCero)
                if (respuesta == Constantes.msjPdfFailCreated)
                {
                    Session.Remove("MsjeErrorCreatedPDF");
                    Response.Write("<script language=javascript>alert('" + respuesta + "');</script>");
                }
                //Session.Remove("MsjeErrorCreatedPDF");
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



                //cboimpresoras.Items.Add(Constantes.ValorSeleccione);
                foreach (string strPrinter in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    cboimpresoras.Items.Add(strPrinter);
                }
            }
            catch (Exception ex) { }
        }

        WCFSeguridad.ListaRol oListaRolesUserLog = new WCFSeguridad.ListaRol();
        private void CapturarUserLogRoles()
        {
            try
            {
                oListaRolesUserLog = objController.GetList_RolUserLog();
                btnBuscar.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnExportarExcel.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnExportarExcel.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);

                btnPrint.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnPrint.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);

                btnSend.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnSend.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
            }
            catch (Exception ex) { }
        }


        private void CargarListas()
        {
            try
            {

                cbotipodocumento.DataSource = Singleton.Instance.GetList_TypeDocument_CE();
                cbotipodocumento.DataValueField = "IdTipoDocumento";
                cbotipodocumento.DataTextField = "Descripcion";
                cbotipodocumento.DataBind();


                cboestado.DataSource = Singleton.Instance.Get_ListStatus();
                cboestado.DataValueField = "IdEstado";
                cboestado.DataTextField = "Descripcion";
                cboestado.DataBind();

                cboserie.DataSource = Singleton.Instance.Get_ListSerieDocument(oUsuarioLogeado, Constantes.ValorCero);
                cboserie.DataValueField = "IdSerie";
                cboserie.DataValueField = "NumeroSerie";
                cboserie.DataBind();

                txtfechadesde.Value = DateTime.Now.AddDays(-7).ToShortDateString();
                txtfechahasta.Value = DateTime.Now.ToString("dd/MM/yyyy");
            }
            catch (Exception ex) { }
        }

        private void ListarSerie()
        {
            try
            {
                ObtenerUsuarioLogeado();

                cboserie.DataSource = Singleton.Instance.Get_ListSerieDocument(oUsuarioLogeado, int.Parse(cbotipodocumento.Value));
                cboserie.DataValueField = "IdSerie";
                cboserie.DataValueField = "NumeroSerie";
                cboserie.DataBind();
            }
            catch (Exception ex) { }
        }

        private void ObtenerDatos()
        {
            try
            {
                ObtenerUsuarioLogeado();

                oFacturaElectronica = new FacturaElectronica();
                oFacturaElectronica.TipoDocumento = new TipoDocumento();
                oFacturaElectronica.Estado = new Estado();
                oFacturaElectronica.Serie = new Serie();
                oFacturaElectronica.Empresa = new Empresa();
                oFacturaElectronica.Cliente = new Cliente();

                oFacturaElectronica.FechaInicio = txtfechadesde.Value;
                oFacturaElectronica.FechaFin = txtfechahasta.Value;
                oFacturaElectronica.TipoDocumento.CodigoDocumento = cbotipodocumento.Value;
                string numserie = cboserie.Value == Constantes.ValorTodos ? string.Empty : cboserie.Value;
                oFacturaElectronica.Serie.NumeroSerie = numserie;
                oFacturaElectronica.NumeroDocumentoInicio = txtnuminicio.Value == string.Empty ? string.Empty : string.Format("{0:00000000}", int.Parse(txtnuminicio.Value));
                oFacturaElectronica.NumeroDocumentoFin = txtnumfin.Value == string.Empty ? string.Empty : string.Format("{0:00000000}", int.Parse(txtnumfin.Value));
                oFacturaElectronica.Estado.IdEstado = Convert.ToInt32(cboestado.Value);
                oFacturaElectronica.Cliente.ClienteRuc = txtclienteruc.Value.Length == 0 ? string.Empty : txtclienteruc.Value;
                oFacturaElectronica.Cliente.Nombres = txtclientenombres.Value.Length == 0 ? string.Empty : txtclientenombres.Value;
                oFacturaElectronica.Empresa.RUC = oUsuarioLogeado.Empresa.RUC;
                oFacturaElectronica.Estado.IdEstadoSUNAT = Constantes.ValorCero;
                oFacturaElectronica.Sede = oUsuarioLogeado.Sede.Name == null ? string.Empty : oUsuarioLogeado.Sede.Name;
            }
            catch (Exception ex) { }
        }

        public int Inicio = Constantes.ValorCero;
        public int Fin = Constantes.ValorCero;
        public int val = Constantes.ValorCero;

        private void ValidarParametros()
        {
            try
            {
                val = Singleton.Instance.Validate_RangeDate(txtfechadesde.Value, txtfechahasta.Value);

                Inicio = txtnuminicio.Value == String.Empty ? Constantes.ValorCero : Convert.ToInt32(txtnuminicio.Value);
                Fin = txtnumfin.Value == String.Empty ? Constantes.ValorCero : Convert.ToInt32(txtnumfin.Value);
            }
            catch (Exception ex) { }
        }


        private void CargarData()
        {
            try
            {
                ObtenerUsuarioLogeado();//para la empresa del usuario logeado

                Session.Remove("ListaCabecera");
                ValidarParametros();

                if (Inicio > Fin)
                { Response.Write(Constantes.MensajeNumDoc); }

                else if (val == Constantes.ValorUno) { Response.Write(Constantes.MensajeFecha); }

                else if (val == Constantes.ValorDos || val == Constantes.ValorCero)
                {
                    ObtenerDatos();
                    ListaFacturaElectronica = ServiceFacturacionController.Instance.ListarDocumentoCabecera(oFacturaElectronica);

                    GridView1.DataSource = ListaFacturaElectronica;
                    GridView1.DataBind();
                }
                Session["ListaCabecera"] = ListaFacturaElectronica;
            }
            catch (Exception ex) { }
        }

        private void LimpiarCampos()
        {
            txtnuminicio.Value = string.Empty;
            txtnumfin.Value = string.Empty;
            cboestado.SelectedIndex = 0;
            cboserie.SelectedIndex = 0;
            cbotipodocumento.SelectedIndex = 0;
            txtfechadesde.Value = string.Empty;
            txtfechahasta.Value = string.Empty;
        }

        #endregion


        #region method this class

        private void Reset_Controls(int cant_item)
        {
            if (GridView1.Rows.Count > Constantes.ValorCero)
            {
                idSelectAll.Visible = true;
                txtCopies.Visible = true;
                lblcopies.Visible = true;
                lblseleccionar.Visible = true;

                idSelectAlltoSend.Visible = true;
                lblseleccionar_send.Visible = true;
            }
            else
            {
                lblseleccionar.Visible = false;
                idSelectAll.Visible = false;
                txtCopies.Visible = false;
                lblcopies.Visible = false;

                idSelectAlltoSend.Visible = false;
                lblseleccionar_send.Visible = false;
            }
        }

        #endregion

        [System.Web.Services.WebMethod]
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                GridView1.PageIndex = Constantes.ValorCero;
                GridView1.DataSourceID = string.Empty;
                GridView1.DataBind();
                CargarData();

                Reset_Controls(GridView1.Rows.Count);

                string script = @"<script type='text/javascript'>";
                script += @"</script>";
                Response.Write(script);
            }
            catch (Exception ex) { }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        protected void cbotipodocumento_ServerChange(object sender, EventArgs e)
        {
            try
            {
                ListarSerie();
                string script = @"<script type='text/javascript'>";
                script += @"</script>";
                Response.Write(script);
            }
            catch (Exception ex) { }
        }

        #region ENTITTY XML_PDF
        public string NombreArchivo = string.Empty;
        public string moneda = string.Empty;

        public string tpodocumento = string.Empty;
        public string serie = string.Empty;
        public string nrodocumento = string.Empty;
        public string MontoTotal = string.Empty;
        public string tipoDocumento = string.Empty;

        int typeFormat = Constantes.ValorCero;
        #endregion

        #region GENERAR XML
        protected void btnImgXML_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ObtenerUsuarioLogeado();
                ListaFacturaElectronica = (ListaFacturaElectronica)Session["ListaCabecera"];
                ImageButton btnDetalles = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btnDetalles.NamingContainer;

                string xmlGenerar = string.Empty;

                //tpodocumento = GridView1.Rows[gvrow.RowIndex].Cells[8].Text;
                //serie = GridView1.Rows[gvrow.RowIndex].Cells[9].Text;
                //nrodocumento = GridView1.Rows[gvrow.RowIndex].Cells[10].Text;

                tpodocumento = GridView1.Rows[gvrow.RowIndex].Cells[7].Text;
                serie = GridView1.Rows[gvrow.RowIndex].Cells[8].Text;
                nrodocumento = GridView1.Rows[gvrow.RowIndex].Cells[9].Text;

                for (int i = 0; i <= ListaFacturaElectronica.Count - 1; i++)
                {
                    if (serie == ListaFacturaElectronica[i].Serie.NumeroSerie && nrodocumento == ListaFacturaElectronica[i].NumeroDocumento && tpodocumento == ListaFacturaElectronica[i].TipoDocumento.CodigoDocumento)
                    {
                        NombreArchivo = ListaFacturaElectronica[i].NombreArchivoXML.TrimEnd();
                        tipoDocumento = ListaFacturaElectronica[i].TipoDocumento.IdTipoDocumento.ToString();
                        xmlGenerar = ListaFacturaElectronica[i].XML;
                        break;
                    }
                }

                if (xmlGenerar.Length < 50)
                { Response.Write("<script language=javascript>alert('Este Doc. no tiene Xml Firmado');</script>"); return; }

                Singleton.Instance.CreateDirectory(Server.MapPath("~/DocumentoXML/"));

                //var xmldoc = new XmlDocument();
                //xmldoc.InnerXml = xmlGenerar;
                //xmldoc.Save(Server.MapPath("~/DocumentoXML/" + NombreArchivo + ".xml"));

                //XmlSerializer xml = new XmlSerializer(typeof(xmlFac.InvoiceType));
                //using (StreamWriter sw = new StreamWriter(Server.MapPath("~/DocumentoXML/" + NombreArchivo + ".xml"), false, Encoding.GetEncoding("ISO-8859-1")))
                //{
                //    xml.Serialize(sw, xmlGenerar);
                //}


                using (StreamWriter sw = new StreamWriter(File.Open(Server.MapPath("~/DocumentoXML/" + NombreArchivo + ".xml"), FileMode.Create), Encoding.GetEncoding("ISO-8859-1")))
                {
                    sw.WriteLine(xmlGenerar);
                }



                tpodocumento = string.Empty;

                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + NombreArchivo + ".xml");
                Response.WriteFile("../../DocumentoXML/" + NombreArchivo + ".xml");
                Response.Flush();




                //Response.Clear();
                //Response.ClearContent();
                //Response.ClearHeaders();
                //Response.ContentType = "application/xml";
                //Response.TransmitFile("../../DocumentoXML/" + NombreArchivo + ".xml");
                //Response.End();



                //Response.Buffer = true;
                //Response.Clear();
                ////Response.ContentType = mimeType;
                //Response.ContentType = "application/octet-stream";
                ////response.AppendHeader("content-Disposition", "inline: filename=TrainingOfficialRecord." + extension);
                //Response.AppendHeader("content-Disposition", "inline: filename=" + NombreArchivo + ".xml");
                //Response.BinaryWrite(bytes);
                //Response.WriteFile("../../DocumentoXML/" + NombreArchivo + ".xml");
                ////Response.End();
                //Response.Clear();




                var file = Path.Combine(HttpContext.Current.Server.MapPath("../../DocumentoXML/" + NombreArchivo + ".xml"));
                if (System.IO.File.Exists(file))
                    System.IO.File.Delete(file);
                Response.End();
            }
            catch (Exception ex) { }
        }

        #endregion

        #region GENERAR PDF
        XmlDocument XMLArchive = new XmlDocument();
        FacturaElectronica oFactura = new FacturaElectronica();

        protected void btnImgPdf_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                typeFormat = Constantes.ValorCero;
                ObtenerUsuarioLogeado();
                ListaFacturaElectronica = (ListaFacturaElectronica)Session["ListaCabecera"];

                ImageButton btnDetalles = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btnDetalles.NamingContainer;

                string xmlGenerar = string.Empty;

                //tpodocumento = GridView1.Rows[gvrow.RowIndex].Cells[8].Text;
                //serie = GridView1.Rows[gvrow.RowIndex].Cells[9].Text;
                //nrodocumento = GridView1.Rows[gvrow.RowIndex].Cells[10].Text;
                //MontoTotal = Convert.ToDecimal(GridView1.Rows[gvrow.RowIndex].Cells[15].Text).ToString();

                tpodocumento = GridView1.Rows[gvrow.RowIndex].Cells[7].Text;
                serie = GridView1.Rows[gvrow.RowIndex].Cells[8].Text;
                nrodocumento = GridView1.Rows[gvrow.RowIndex].Cells[9].Text;
                MontoTotal = Convert.ToDecimal(GridView1.Rows[gvrow.RowIndex].Cells[14].Text).ToString();

                for (int i = 0; i <= ListaFacturaElectronica.Count - 1; i++)
                {
                    if (serie == ListaFacturaElectronica[i].Serie.NumeroSerie && nrodocumento == ListaFacturaElectronica[i].NumeroDocumento && tpodocumento == ListaFacturaElectronica[i].TipoDocumento.CodigoDocumento)
                    {
                        NombreArchivo = ListaFacturaElectronica[i].NombreArchivoXML.TrimEnd();
                        moneda = ListaFacturaElectronica[i].Moneda.Descripcion;
                        tipoDocumento = ListaFacturaElectronica[i].TipoDocumento.IdTipoDocumento.ToString();
                        xmlGenerar = ListaFacturaElectronica[i].XML;
                        //typeFormat = ListaFacturaElectronica[i].TypeFormat;

                        typeFormat = 2;
                        break;
                    }
                }

                if (xmlGenerar.Length < 50)
                { Response.Write("<script language=javascript>alert('Este Doc. no tiene Xml Firmado');</script>"); return; }

                Singleton.Instance.CreateDirectory(Server.MapPath("~/DocumentoXML/"));
                //var xmldoc = new XmlDocument();
                //xmldoc.InnerXml = xmlGenerar;
                //xmldoc.Save(Server.MapPath("~/DocumentoXML/" + NombreArchivo + ".xml"));

                Response.Clear();

                switch (tpodocumento)
                {
                    case Constantes.Factura:
                    case Constantes.Boleta:
                        { ObtenerValoresXMLCE(xmlGenerar); break; }
                    case Constantes.NotaCredito: { ObtenerValoresXMLCENotCredit(xmlGenerar); break; }
                    case Constantes.NotaDebito: { ObtenerValoresXMLCENotDebit(xmlGenerar); break; }
                }

                tpodocumento = string.Empty;

                Session["FacturaPDF"] = oFactura;
                Session["FacturaDetallePDF"] = oFactura.ListaDetalleFacturaElectronica;

                var file = Path.Combine(HttpContext.Current.Server.MapPath("../../DocumentoXML/" + NombreArchivo + ".xml"));
                if (System.IO.File.Exists(file))
                    System.IO.File.Delete(file);

                Response.Redirect("../Informes/DocumentoPdf");
                Response.End();
            }
            catch (Exception ex) { }
        }

        #endregion

        #region SERIALIZAR XML

        #region FACTURA, BOLETA
        void ObtenerValoresXMLCE(string xml_line)
        {
            try
            {
                XMLArchive.InnerXml = xml_line;

                var res = Singleton.Instance.GetDataXml_AllTypeDocument(xml_line, Constantes.Factura);
                var inv = (xmlFac.InvoiceType)(res);
                //oFactura = new Slin.Facturacion.Common.Util.UsefullClass().GetDataFromXMLCE(inv, XMLArchive, NombreArchivo);
                oFactura = Common.Util.UsefullClass.Instance.GetDataFromXMLCE(inv, XMLArchive, NombreArchivo);

                oFactura.TypeFormat = typeFormat;
            }
            catch (Exception ex) { }
        }
        #endregion END FACTURA, BOLETA

        #region NOTA DE CREDITO
        void ObtenerValoresXMLCENotCredit(string xml_line)
        {
            try
            {
                XMLArchive.InnerXml = xml_line;

                var res = Singleton.Instance.GetDataXml_AllTypeDocument(xml_line, Constantes.NotaCredito);
                var nc = (xmlNotCred.CreditNoteType)(res);

                //oFactura = new Slin.Facturacion.Common.Util.UsefullClass().GetDataFromCNOTECredit(nc, XMLArchive, NombreArchivo, tpodocumento);
                oFactura = Common.Util.UsefullClass.Instance.GetDataFromCNOTECredit(nc, XMLArchive, NombreArchivo, tpodocumento);
                oFactura.TypeFormat = typeFormat;
            }
            catch (Exception ex) { }
        }
        #endregion END NOTA DE CREDITO

        #region NOTA DE DEBITO
        void ObtenerValoresXMLCENotDebit(string xml_line)
        {
            try
            {
                XMLArchive.InnerXml = xml_line;

                var res = Singleton.Instance.GetDataXml_AllTypeDocument(xml_line, Constantes.NotaDebito);
                var nd = (xmlNotDeb.DebitNoteType)(res);

                //oFactura = new Slin.Facturacion.Common.Util.UsefullClass().GetDataFromCNOTEDebit(nd, XMLArchive, NombreArchivo, tpodocumento);
                oFactura = Common.Util.UsefullClass.Instance.GetDataFromCNOTEDebit(nd, XMLArchive, NombreArchivo, tpodocumento);
                oFactura.TypeFormat = typeFormat;
            }
            catch (Exception ex) { }
        }
        #endregion END NOTA DE DEBITO

        #endregion

        #region EXPORT TO EXCEL

        void EnviarToExcel()
        {
            try
            {
                Singleton.Instance.CreateDirectory(Server.MapPath("~/DocumentoXML/"));
                ObtenerDatos();
                var lista = new ListaDocumento();
                lista = ServiceFacturacionController.Instance.GetListaDocumentoCabExcel(oFacturaElectronica);
                Session["Excel"] = lista;
                Response.Redirect("../Informes/ExportarExcel");
            }
            catch (Exception ex) { }
        }

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {



            //var lista = (ListaFacturaElectronica)Session["ListaDocumentoCabecera"];
            //System.Data.DataTable dt = ConvertToDataTable(lista);
            //System.Data.DataSet ds = new System.Data.DataSet();
            //ds.Tables.Add(dt);
            //ExcelLibrary.DataSetHelper.CreateWorkbook("Myfile.xlsx",ds);


            try
            {
                if (GridView1.Rows.Count == 0)
                { Response.Write("<script language=javascript>alert('No se ha realizado una consulta previa');</script>"); }
                else
                { EnviarToExcel(); }
            }
            catch (Exception ex) { }
        }





        







        #endregion

        #region VER DETALLE DOCUMENTO

        FacturaElectronica oDocumentoCab = new FacturaElectronica();
        ListaFacturaElectronica listamontos = new ListaFacturaElectronica();

        protected void btnVerDetalle_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ObtenerUsuarioLogeado();

                oDocumentoCab = new FacturaElectronica();
                oDocumentoCab.TipoDocumento = new TipoDocumento();
                oDocumentoCab.Empresa = new Empresa();
                oDocumentoCab.Cliente = new Cliente();
                oDocumentoCab.Serie = new Serie();
                oDocumentoCab.Estado = new Estado();
                oDocumentoCab.Empresa.Ubigeo = new Ubigeo();

                ListaFacturaElectronica = (ListaFacturaElectronica)Session["ListaCabecera"];

                ImageButton btnDetalles = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btnDetalles.NamingContainer;

                string xmlGenerar = string.Empty;

                //tpodocumento = GridView1.Rows[gvrow.RowIndex].Cells[8].Text;
                //serie = GridView1.Rows[gvrow.RowIndex].Cells[9].Text;
                //nrodocumento = GridView1.Rows[gvrow.RowIndex].Cells[10].Text;
                //MontoTotal = Convert.ToDecimal(GridView1.Rows[gvrow.RowIndex].Cells[15].Text).ToString();

                tpodocumento = GridView1.Rows[gvrow.RowIndex].Cells[7].Text;
                serie = GridView1.Rows[gvrow.RowIndex].Cells[8].Text;
                nrodocumento = GridView1.Rows[gvrow.RowIndex].Cells[9].Text;
                MontoTotal = Convert.ToDecimal(GridView1.Rows[gvrow.RowIndex].Cells[14].Text).ToString();

                for (int i = 0; i <= ListaFacturaElectronica.Count - 1; i++)
                {
                    if (serie == ListaFacturaElectronica[i].Serie.NumeroSerie && nrodocumento == ListaFacturaElectronica[i].NumeroDocumento && tpodocumento == ListaFacturaElectronica[i].TipoDocumento.CodigoDocumento)
                    {
                        oDocumentoCab.TipoDocumento.CodigoDocumento = ListaFacturaElectronica[i].TipoDocumento.CodigoDocumento;
                        oDocumentoCab.TipoDocumento.Descripcion = ListaFacturaElectronica[i].TipoDocumento.Descripcion;
                        oDocumentoCab.Serie.NumeroSerie = ListaFacturaElectronica[i].Serie.NumeroSerie;
                        oDocumentoCab.NumeroDocumento = ListaFacturaElectronica[i].NumeroDocumento;
                        oDocumentoCab.FechaEmision = ListaFacturaElectronica[i].FechaEmision;

                        oDocumentoCab.Empresa.RUC = ListaFacturaElectronica[i].Empresa.RUC;

                        oDocumentoCab.Empresa.Ubigeo.CodigoUbigeo = ListaFacturaElectronica[i].Empresa.Ubigeo.CodigoUbigeo;
                        oDocumentoCab.Empresa.Ubigeo.Descripcion = ListaFacturaElectronica[i].Empresa.Ubigeo.Descripcion;

                        oDocumentoCab.Cliente.NumeroDocumentoIdentidad = ListaFacturaElectronica[i].Cliente.NumeroDocumentoIdentidad;
                        oDocumentoCab.Cliente.Nombres = ListaFacturaElectronica[i].Cliente.Nombres;
                        oDocumentoCab.Cliente.Direccion = ListaFacturaElectronica[i].Cliente.Direccion;
                        oDocumentoCab.TipoMoneda = ListaFacturaElectronica[i].Moneda.Descripcion;

                        tipoDocumento = ListaFacturaElectronica[i].TipoDocumento.IdTipoDocumento.ToString();

                        oDocumentoCab.CodeMessage = ListaFacturaElectronica[i].CodeMessage;
                        oDocumentoCab.DocMessage = ListaFacturaElectronica[i].DocMessage;

                        oDocumentoCab.CodeResponse = ListaFacturaElectronica[i].CodeResponse;
                        oDocumentoCab.NoteResponse = ListaFacturaElectronica[i].NoteResponse;

                        oDocumentoCab.Estado.Descripcion = ListaFacturaElectronica[i].Estado.Descripcion;

                        xmlGenerar = string.Empty;
                        xmlGenerar = ListaFacturaElectronica[i].XML;

                        NombreArchivo = ListaFacturaElectronica[i].NombreArchivoXML.TrimEnd();
                        break;
                    }
                }
                Singleton.Instance.CreateDirectory(Server.MapPath("~/DocumentoXML/"));

                //var xmldoc = new XmlDocument();
                //xmldoc.InnerXml = xmlGenerar;
                //xmldoc.Save(Server.MapPath("~/DocumentoXML/" + NombreArchivo + ".xml"));

                switch (tpodocumento)
                {
                    case Constantes.Boleta:
                    case Constantes.Factura:
                        {
                            ObtenerValoresXMLCE(xmlGenerar);
                            break;
                        }
                    case Constantes.NotaCredito:
                        {
                            ObtenerValoresXMLCENotCredit(xmlGenerar);
                            break;
                        }
                    case Constantes.NotaDebito:
                        {
                            ObtenerValoresXMLCENotDebit(xmlGenerar);
                            break;
                        }
                }

                Util_ViewDetailDocument(oDocumentoCab, oFactura);

                Session["DocumentoCab"] = oDocumentoCab;
                Session["DocumentoDet"] = oFactura.ListaDetalleFacturaElectronica;
                //Session["DocumentoDet"] = oListaDetalle = oFactura.ListaDetalleFacturaElectronica;
                //Session["ListaMontos"] = listamontos;
                tpodocumento = string.Empty;

                var file = Path.Combine(HttpContext.Current.Server.MapPath("../../DocumentoXML/" + NombreArchivo + ".xml"));
                if (System.IO.File.Exists(file))
                    System.IO.File.Delete(file);

                string form = "../../Views/Facturacion/DetalleDoc";
                //string form = "../../Views/Facturacion/DetalleDocumento";
                string script = @"<script type='text/javascript'>";
                script += "window.open('" + form + "',null,'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=1,width=889,height=550,left=100,top=100');";
                script += @"</script>";
                Response.Write(script);
            }
            catch (Exception ex) { }
        }



        private void Util_ViewDetailDocument(FacturaElectronica odoc, FacturaElectronica ofac)
        {
            oDocumentoCab.TotalGravado = oFactura.TotalGravado;
            oDocumentoCab.MontoTotalCad = oFactura.MontoTotalCad;
            oDocumentoCab.MontoIgvCad = oFactura.MontoIgvCad;
            oDocumentoCab.TotalDescuento = oFactura.TotalDescuento;
            oDocumentoCab.TotalnoGravado = oFactura.TotalnoGravado == null ? oFactura.TotalInafecto : oFactura.TotalnoGravado;
            oDocumentoCab.TotalExonerado = oFactura.TotalExonerado;

            oDocumentoCab.Empresa.RazonSocial = oFactura.Empresa.RazonSocial;
            oDocumentoCab.Empresa.Direccion = oFactura.Empresa.Direccion;
        }

        private void Execute_MessageGridview(int cant_items, List<string> list)
        {
            if (cant_items < Constantes.ValorUno)
            { Response.Write("<script language=javascript>alert('No hay Registros');</script>"); }

            if (list.Count < Constantes.ValorUno)
            { Response.Write("<script language=javascript>alert('Seleccione registros para enviar');</script>"); }
        }

        #endregion

        #region METHOD FOR PRINT

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            var listDocPrint = Singleton.Instance.Execute_List_PrintSend("chkSelImp", GridView1);

            PathWriteFile = Singleton.Instance.Get_PathWriteOrder(PathLogSLINADE);

            try
            {
                Execute_MessageGridview(GridView1.Rows.Count, listDocPrint);
                #region PRINT
                ObtenerUsuarioLogeado();
                var list_Write = new List<string>();

                Encoding encoding = Encoding.GetEncoding("ISO-8859-1");

                for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
                {
                    CheckBox check = ((CheckBox)GridView1.Rows[i].Cells[1].FindControl("chkSelImp"));

                    string valueDoc = string.Empty;
                    //string TypeDoc = GridView1.Rows[i].Cells[8].Text;
                    //string SerieDoc = GridView1.Rows[i].Cells[9].Text;
                    //string Num_Doc = GridView1.Rows[i].Cells[10].Text;

                    string TypeDoc = GridView1.Rows[i].Cells[7].Text;
                    string SerieDoc = GridView1.Rows[i].Cells[8].Text;
                    string Num_Doc = GridView1.Rows[i].Cells[9].Text;

                    string Copies = txtCopies.Value.Length < Constantes.ValorUno ? Constantes.ValorUno + string.Empty : txtCopies.Value;

                    string num_ce = oUsuarioLogeado.Empresa.RUC + "-" + TypeDoc + "-" + SerieDoc + "-" + Num_Doc;

                    ListaFacturaElectronica = (ListaFacturaElectronica)Session["ListaCabecera"];

                    for (int j = 0; j <= ListaFacturaElectronica.Count - 1; j++)
                    {
                        if (num_ce == ListaFacturaElectronica[i].NombreArchivoXML)
                        {
                            if (ListaFacturaElectronica[j].TypeFormat < Constantes.ValorUno)
                            {
                                typeFormat = Constantes.ValorDos;
                            }
                            else
                            {
                                typeFormat = ListaFacturaElectronica[j].TypeFormat;
                                break;
                            }
                        }
                    }


                    if (check.Checked)
                    {
                        var myXml = new DocumentState();

                        myXml.Tipo_CE = TypeDoc;
                        myXml.RucEmisor = oUsuarioLogeado.Empresa.RUC;
                        myXml.ID_CE = num_ce;
                        myXml.Estado_Doc = "0";
                        myXml.Estado_Desc = "Doc. for Print";
                        myXml.TypeFormat = typeFormat + string.Empty;
                        myXml.PrintName = cboimpresoras.Value;
                        myXml.Copies = Copies;

                        //CreateDirectory(PathWriteFile + @"smp\prt\");

                        Singleton.Instance.CreateDirectory(PathWriteFile + @"smp\prt\");

                        XmlSerializer xml = new XmlSerializer(typeof(DocumentState));

                        using (StreamWriter sw = new StreamWriter(PathWriteFile + @"smp\prt\" + num_ce + ".xml", false, Encoding.GetEncoding("ISO-8859-1")))
                        {
                            //xml.Serialize(sw, myXml, ns);
                            xml.Serialize(sw, myXml);

                            list_Write.Add(myXml.ID_CE);
                        }
                    }
                }


                if (list_Write.Count > Constantes.ValorCero)
                {
                    Response.Write("<script language=javascript>alert('Se Generaron " + list_Write.Count + " ordenes de Impresión');</script>");
                }

                #endregion END PRINT

            }
            catch (System.Exception ex) { }
        }


        #endregion END METHOD FOR PRINT

        protected void btnSend_Click(object sender, EventArgs e)
        {
            var listSelect = Singleton.Instance.Execute_List_PrintSend("chkSelSend", GridView1);

            PathWriteFile = Singleton.Instance.Get_PathWriteOrder(PathLogSLINADE);

            try
            {
                Execute_MessageGridview(GridView1.Rows.Count, listSelect);

                #region method generate xml for send

                ObtenerUsuarioLogeado();

                var list_Write = new List<string>();

                for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
                {
                    //string TypeDoc = GridView1.Rows[i].Cells[8].Text;
                    //string SerieDoc = GridView1.Rows[i].Cells[9].Text;
                    //string Num_Doc = GridView1.Rows[i].Cells[10].Text;

                    string TypeDoc = GridView1.Rows[i].Cells[7].Text;
                    string SerieDoc = GridView1.Rows[i].Cells[8].Text;
                    string Num_Doc = GridView1.Rows[i].Cells[9].Text;

                    string num_ce = oUsuarioLogeado.Empresa.RUC + "-" + TypeDoc + "-" + SerieDoc + "-" + Num_Doc;

                    CheckBox check = ((CheckBox)GridView1.Rows[i].Cells[0].FindControl("chkSelSend"));

                    if (check.Checked)
                    {
                        var myXml_Send = new DocumentSend();

                        myXml_Send.Tipo_CE = TypeDoc;
                        myXml_Send.RucEmisor = oUsuarioLogeado.Empresa.RUC;
                        myXml_Send.ID_CE = num_ce;
                        myXml_Send.Estado_Doc = "0";
                        myXml_Send.TypeFormat = typeFormat + string.Empty;

                        Singleton.Instance.CreateDirectory(PathWriteFile + @"smc\cds\");

                        XmlSerializer xml = new XmlSerializer(typeof(DocumentSend));

                        using (StreamWriter sw = new StreamWriter(PathWriteFile + @"smc\cds\" + num_ce + ".xml", false, Encoding.GetEncoding("ISO-8859-1")))
                        {
                            //xml.Serialize(sw, myXml, ns);
                            xml.Serialize(sw, myXml_Send);
                            list_Write.Add(myXml_Send.ID_CE);
                        }
                    }
                }

                if (list_Write.Count > Constantes.ValorCero)
                {
                    Response.Write("<script language=javascript>alert('Se Generaron " + list_Write.Count + " ordenes de Envio de correo');</script>");
                }
                #endregion
            }
            catch (Exception ex) { }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string desc = e.Row.Cells[15].Text;
                    //string desc2 = e.Row.Cells[15].Text;
                    //string desc3 = e.Row.Cells[15].Text;

                    if (desc.Contains("pendiente") || desc.Contains("Pendiente"))
                    { e.Row.ForeColor = System.Drawing.Color.Blue; }

                    if (desc.Contains("enviado a Sunat") || desc.Contains("enviado a sunat"))
                    { e.Row.ForeColor = System.Drawing.Color.DarkRed; }

                    if (desc.Contains("aceptado por Sunat") || desc.Contains("aceptado por sunat"))
                    { e.Row.ForeColor = System.Drawing.Color.Red; }   
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ImageButton btnImg = (ImageButton)e.Row.FindControl("btnImgCDR");
                    if (e.Row.Cells[8].Text.Contains("f") || e.Row.Cells[8].Text.Contains("F"))
                    {
                        btnImg.Visible = true;
                    }
                }
            }
            catch (Exception ex) { }
        }

        protected void btnImgCDR_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ObtenerUsuarioLogeado();
                ListaFacturaElectronica = (ListaFacturaElectronica)Session["ListaCabecera"];
                ImageButton btnDetalles = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btnDetalles.NamingContainer;

                string xmlGenerar = string.Empty;

                //tpodocumento = GridView1.Rows[gvrow.RowIndex].Cells[8].Text;
                //serie = GridView1.Rows[gvrow.RowIndex].Cells[9].Text;
                //nrodocumento = GridView1.Rows[gvrow.RowIndex].Cells[10].Text;

                tpodocumento = GridView1.Rows[gvrow.RowIndex].Cells[7].Text;
                serie = GridView1.Rows[gvrow.RowIndex].Cells[8].Text;
                nrodocumento = GridView1.Rows[gvrow.RowIndex].Cells[9].Text;

                for (int i = 0; i <= ListaFacturaElectronica.Count - 1; i++)
                {
                    if (serie == ListaFacturaElectronica[i].Serie.NumeroSerie && nrodocumento == ListaFacturaElectronica[i].NumeroDocumento && tpodocumento == ListaFacturaElectronica[i].TipoDocumento.CodigoDocumento)
                    {
                        NombreArchivo = ListaFacturaElectronica[i].NombreArchivoXML.TrimEnd();
                        tipoDocumento = ListaFacturaElectronica[i].TipoDocumento.IdTipoDocumento.ToString();
                        xmlGenerar = ListaFacturaElectronica[i].CDR;
                        break;
                    }
                }

                if (xmlGenerar.Length < 50)
                { Response.Write("<script language=javascript>alert('Este Doc. no tiene CDR');</script>"); return; }

                Singleton.Instance.CreateDirectory(Server.MapPath("~/DocumentoXML/"));

                var xmldoc = new XmlDocument();
                xmldoc.InnerXml = xmlGenerar;
                xmldoc.Save(Server.MapPath("~/DocumentoXML/" + "R-" + NombreArchivo + ".xml"));

                tpodocumento = string.Empty;

                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + "R-" + NombreArchivo + ".xml");
                Response.WriteFile("../../DocumentoXML/" + "R-" + NombreArchivo + ".xml");
                Response.Flush();

                var file = Path.Combine(HttpContext.Current.Server.MapPath("../../DocumentoXML/" + "R-" + NombreArchivo + ".xml"));
                if (System.IO.File.Exists(file))
                    System.IO.File.Delete(file);
                Response.End();
            }
            catch (Exception ex) { }
        }

    }
}