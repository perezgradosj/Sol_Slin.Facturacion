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
using Slin.Facturacion.Electronica.Web.Helper.Print;

//using System.Globalization;

using xmlFac = Slin.Facturacion.Common.CE;
using xmlCre = Slin.Facturacion.Common.CRE;

using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Configuration;
using System.Text;
using Slin.Facturacion.Common.Helper;
using Slin.Facturacion.Common.Method;
using Slin.Facturacion.Electronica.Web.Views.Util;

namespace Slin.Facturacion.Electronica.Web.Views.Facturacion
{
    public partial class ConsultaDocumentoCRE : System.Web.UI.Page
    {
        private string PathLogSLINADE = ConfigurationManager.AppSettings["PathLogSLINADE"];
        private string PathWriteFile = string.Empty;

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

        Serie oSerie = new Serie();
        ListaSerie oListaSerie = new ListaSerie();
        ListaTipoDocumento oListaTipoDocumento = new ListaTipoDocumento();
        ListaEstado oListaEstado = new ListaEstado();

        private ListaFacturaElectronica listaFacturaElectronica;
        public ListaFacturaElectronica ListaFacturaElectronica
        {
            get { return listaFacturaElectronica; }
            set { listaFacturaElectronica = value; }
        }

        private WCFSeguridad.Usuario ousuarioLogeado;
        public WCFSeguridad.Usuario oUsuarioLogeado
        {
            get { return ousuarioLogeado; }
            set { ousuarioLogeado = value; }
        }

        #endregion

        #region METHOD

        private void Message()
        {
            try
            {
                string respuesta = (string)Session["MsjeErrorCreatedPDF"];
                if (respuesta.Length > Constantes.ValorCero)
                {
                    Session.Remove("MsjeErrorCreatedPDF");
                    Response.Write("<script language=javascript>alert('" + respuesta + "');</script>");
                }
                
            }
            catch (Exception ex) { }
        }

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

                //cboimpresoras.Items.Add(Constantes.ValorSeleccione);
                foreach (string strPrinter in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    cboimpresoras.Items.Add(strPrinter);
                }
            }
            catch (Exception ex) { }
        }

        WCFSeguridad.ListaRol oListaRolesUserLog = new WCFSeguridad.ListaRol();
        void CapturarUserLogRoles()
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

        void ObtenerDatosSerie()
        {
            try
            {
                oSerie = new Serie();
                oSerie.TipoDocumento = new TipoDocumento();
                oSerie.Empresa = new Empresa();

                //oSerie.TipoDocumento.IdTipoDocumento = Constantes.ValorCero;
                oSerie.TipoDocumento.IdTipoDocumento = listanewtpodoc[0].IdTipoDocumento;

                //oSerie.Empresa.IdEmpresa = txtestado.Text.Length == 0 ? Constantes.ValorCero : Convert.ToInt32(txtestado.Text);
                oSerie.Empresa.IdEmpresa = Constantes.ValorCero;
            }
            catch (Exception ex) { }
        }


        ListaTipoDocumento listanewtpodoc = new ListaTipoDocumento();
        void CargarListas()
        {
            try
            {

                cbotipodocumento.DataSource = Singleton.Instance.GetList_TypeDocument_CRE();
                cbotipodocumento.DataValueField = "IdTipoDocumento";
                cbotipodocumento.DataTextField = "Descripcion";
                cbotipodocumento.DataBind();

                cboestado.DataSource = Singleton.Instance.Get_ListStatus();
                cboestado.DataValueField = "IdEstado";
                cboestado.DataTextField = "Descripcion";
                cboestado.DataBind();

                cboserie.DataSource = Singleton.Instance.Get_ListSerieDocument_CRE(oUsuarioLogeado, int.Parse(cbotipodocumento.Value));
                cboserie.DataValueField = "IdSerie";
                cboserie.DataValueField = "NumeroSerie";
                cboserie.DataBind();

                txtfechadesde.Value = DateTime.Now.AddDays(-7).ToShortDateString();
                txtfechahasta.Value = DateTime.Now.ToString("dd/MM/yyyy");
            }
            catch (Exception ex) { }
        }

        void ListarSerie()
        {
            try
            {
                oSerie = new Serie();
                oSerie.TipoDocumento = new TipoDocumento();
                oSerie.Empresa = new Empresa();

                oSerie.TipoDocumento.IdTipoDocumento = Convert.ToInt32(cbotipodocumento.Value);
                oSerie.Empresa.IdEmpresa = Constantes.ValorCero;

                oListaSerie = ServiceFacturacionController.Instance.ListarSerie(oSerie);
                oListaSerie.Insert(0, new Serie() { IdSerie = Constantes.ValorCero, NumeroSerie = Constantes.ValorTodos });


                var listserie = new ListaSerie();

                foreach (var serie in oListaSerie)
                {
                    if (serie.NumeroSerie.Contains("R"))
                    {
                        listserie.Add(serie);
                    }
                }
                listserie.Insert(0, new Serie() { IdSerie = Constantes.ValorCero, NumeroSerie = Constantes.ValorTodos });
                cboserie.DataSource = listserie;

                cboserie.DataValueField = "IdSerie";
                cboserie.DataValueField = "NumeroSerie";
                cboserie.DataBind();
            }
            catch (Exception ex) { }
        }

        void ObtenerDatos()
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
                string numserie = cboserie.Value == Constantes.ValorTodos ? String.Empty : cboserie.Value;
                oFacturaElectronica.Serie.NumeroSerie = numserie;

                oFacturaElectronica.NumeroDocumentoInicio = txtnuminicio.Value == string.Empty ? string.Empty : string.Format("{0:00000000}", int.Parse(txtnuminicio.Value));
                oFacturaElectronica.NumeroDocumentoFin = txtnumfin.Value == string.Empty ? string.Empty : string.Format("{0:00000000}", int.Parse(txtnumfin.Value));


                oFacturaElectronica.Estado.IdEstado = Convert.ToInt32(cboestado.Value);

                oFacturaElectronica.Cliente.ClienteRuc = txtclienteruc.Value.Length == 0 ? string.Empty : txtclienteruc.Value;
                oFacturaElectronica.Cliente.Nombres = txtclientenombres.Value.Length == 0 ? string.Empty : txtclientenombres.Value;

                oFacturaElectronica.Empresa.RUC = oUsuarioLogeado.Empresa.RUC;
                oFacturaElectronica.Estado.IdEstadoSUNAT = Constantes.ValorCero;

                //oFacturaElectronica.Sede = oUsuarioLogeado.Sede.Name;
                oFacturaElectronica.Sede = oUsuarioLogeado.Sede.Name == null ? string.Empty : oUsuarioLogeado.Sede.Name;
            }
            catch (Exception ex) { }
        }

        public int Inicio = Constantes.ValorCero;
        public int Fin = Constantes.ValorCero;

        public int val = Constantes.ValorCero;

        void ValidarParametros()
        {
            try
            {
                val = Singleton.Instance.Validate_RangeDate(txtfechadesde.Value, txtfechahasta.Value);
                Inicio = txtnuminicio.Value == string.Empty ? Constantes.ValorCero : Convert.ToInt32(txtnuminicio.Value);
                Fin = txtnumfin.Value == string.Empty ? Constantes.ValorCero : Convert.ToInt32(txtnumfin.Value);
            }
            catch (Exception ex) { }
        }


        void CargarData()
        {
            try
            {
                ObtenerUsuarioLogeado();//para la empresa del usuario logeado

                Session.Remove("ListaCabeceraCRE");
                ValidarParametros();
                if (Inicio > Fin)
                {
                    Response.Write(Constantes.MensajeNumDoc);

                }
                else if (val == Constantes.ValorUno)
                {
                    Response.Write(Constantes.MensajeFecha);
                }
                else if (val == Constantes.ValorDos || val == Constantes.ValorCero)
                {
                    ObtenerDatos();
                    ListaFacturaElectronica = ServiceFacturacionController.Instance.ListarDocumentoCabeceraCRECPE(oFacturaElectronica);

                    GridView1.DataSource = ListaFacturaElectronica;
                    GridView1.DataBind();
                }
                Session["ListaCabeceraCRE"] = ListaFacturaElectronica;
            }
            catch (Exception ex) { }
        }

        void LimpiarCampos()
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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                GridView1.PageIndex = 0;
                GridView1.DataSourceID = "";
                GridView1.DataBind();
                CargarData();

                if (GridView1.Rows.Count > Constantes.ValorCero)
                {
                    //chkAllChecked.Visible = true;
                    idSelectAll.Visible = true;
                    txtCopies.Visible = true;
                    lblcopies.Visible = true;
                    lblseleccionar.Visible = true;

                    idSelectAlltoSend.Visible = true;
                    lblseleccionar_send.Visible = true;
                }
                else
                {
                    //chkAllChecked.Visible = false;
                    lblseleccionar.Visible = false;
                    idSelectAll.Visible = false;
                    txtCopies.Visible = false;
                    lblcopies.Visible = false;

                    idSelectAlltoSend.Visible = false;
                    lblseleccionar_send.Visible = false;
                }

                string script = @"<script type='text/javascript'>";
                script += @"</script>";
                Response.Write(script);
            }
            catch (Exception ex)
            {

            }
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
        public string montoLiteral = string.Empty;

        int typeFormat = Constantes.ValorCero;

        #endregion


        #region METHOD XML_PDF
        //void CrearNuevaCarpeta()
        //{
        //    try
        //    {
        //        string newfolder = Server.MapPath("~/DocumentoXML/");
        //        if (!Directory.Exists(newfolder))
        //        {
        //            Directory.CreateDirectory(newfolder);
        //        }
        //    } catch (Exception ex) { }
        //}

        //private void CreateDirectory(string path)
        //{
        //    if (!System.IO.Directory.Exists(path))
        //    {
        //        System.IO.Directory.CreateDirectory(path);
        //    }
        //}

        #endregion

        #region GENERAR XML
        protected void btnImgXML_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ObtenerUsuarioLogeado();
                ListaFacturaElectronica = (ListaFacturaElectronica)Session["ListaCabeceraCRE"];
                ImageButton btnDetalles = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btnDetalles.NamingContainer;

                string xmlGenerar = string.Empty;

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
                        montoLiteral = ListaFacturaElectronica[i].Campo1;
                        break;
                    }
                }

                if (xmlGenerar.Length < 50)
                {
                    Response.Write("<script language=javascript>alert('Este Doc. no tiene Xml Firmado');</script>");
                    return;
                }

                Singleton.Instance.CreateDirectory(Server.MapPath("~/DocumentoXML/"));

                //var xmldoc = new XmlDocument();
                //xmldoc.InnerXml = xmlGenerar;
                //xmldoc.Save(Server.MapPath("~/DocumentoXML/" + NombreArchivo + ".xml"));


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

                var file = Path.Combine(HttpContext.Current.Server.MapPath("../../DocumentoXML/" + NombreArchivo + ".xml"));
                if (System.IO.File.Exists(file))
                    System.IO.File.Delete(file);
                Response.End();
            }
            catch (Exception ex)
            {

            }
            
        }

        #endregion

        #region GENERAR PDF

        XmlDocument XMLArchive = new XmlDocument();

        FacturaElectronica oFactura = new FacturaElectronica();

        //StreamReader sr;

        public int contadorP = Constantes.ValorCero;
        public int contadorV = Constantes.ValorCero;
        public int nroOrden = Constantes.ValorUno;

        protected void btnImgPdf_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                typeFormat = Constantes.ValorCero;
                ObtenerUsuarioLogeado();
                ListaFacturaElectronica = (ListaFacturaElectronica)Session["ListaCabeceraCRE"];

                ImageButton btnDetalles = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btnDetalles.NamingContainer;

                string xmlGenerar = string.Empty;

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

                        if (ListaFacturaElectronica[i].Campo1.Length < Constantes.ValorUno)
                        {
                            montoLiteral = "-";
                        }
                        else
                        {
                            montoLiteral = ListaFacturaElectronica[i].Campo1;
                        }
                        break;
                    }
                }

                if (xmlGenerar.Length < 50)
                {
                    Response.Write("<script language=javascript>alert('Este Doc. no tiene Xml Firmado');</script>");
                    return;
                }

                Singleton.Instance.CreateDirectory(Server.MapPath("~/DocumentoXML/"));

                //var xmldoc = new XmlDocument();
                //xmldoc.InnerXml = xmlGenerar;
                //xmldoc.Save(Server.MapPath("~/DocumentoXML/" + NombreArchivo + ".xml"));

                Response.Clear();


                switch (tpodocumento)
                {
                    case Constantes.Retencion:
                        { ObtenerValoresXMLCRE(xmlGenerar); break; }
                    case Constantes.Percepcion: { break; }
                }

                tpodocumento = string.Empty;

                var file = Path.Combine(HttpContext.Current.Server.MapPath("../../DocumentoXML/" + NombreArchivo + ".xml"));
                if (System.IO.File.Exists(file))
                    System.IO.File.Delete(file);
                Response.Redirect("../Informes/DocumentoPdf");
                Response.End();
            }
            catch (Exception ex)
            {

            }
            
        }

        #endregion


        #region SERIALIZAR XML


        void ObtenerValoresXMLCRE(string xml_line)
        {
            try
            {
                oFactura = new FacturaElectronica();
                XMLArchive.InnerXml = xml_line;

                var res = Singleton.Instance.GetDataXml_AllTypeDocument(xml_line, Constantes.Retencion);
                var ret = (xmlCre.RetentionType)(res);

                //oFactura = new Slin.Facturacion.Common.Util.UsefullClass().GetDataFromXMLCRE(ret, XMLArchive, NombreArchivo, tpodocumento);
                oFactura = Common.Util.UsefullClass.Instance.GetDataFromXMLCRE(ret, XMLArchive, NombreArchivo, tpodocumento);
                oFactura.TypeFormat = typeFormat;

                Session["FacturaPDF"] = oFactura;
                Session["FacturaDetallePDF"] = oFactura.ListaDocCRECPE;
                //Session["FacturaDetallePDF"] = objlistaDocCRECPE;
            }
            catch (Exception ex) { }
        }


        void ObtenerValoresXMLCREDET(string xml_line)
        {
            try
            {
                XMLArchive.InnerXml = xml_line; //XML
                var res = Singleton.Instance.GetDataXml_AllTypeDocument(xml_line, Constantes.Retencion);
                var ret = (xmlCre.RetentionType)(res);

                //oFactura = new Slin.Facturacion.Common.Util.UsefullClass().GetDataFromXMLCRE(ret, XMLArchive, NombreArchivo, tpodocumento);
                oFactura = Common.Util.UsefullClass.Instance.GetDataFromXMLCRE(ret, XMLArchive, NombreArchivo, tpodocumento);
            }
            catch (Exception ex) { }
        }

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
            catch (Exception ex)
            {

            }
        }

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (GridView1.Rows.Count == 0)
                {
                    Response.Write("<script language=javascript>alert('No se ha realizado una consulta previa');</script>");
                }
                else
                {
                    EnviarToExcel();
                }
            }
            catch (Exception ex)
            {

            }
            
        }

        #endregion

        #region VER DETALLE DOCUMENTO

        FacturaElectronica oDocumentoCab = new FacturaElectronica();
        //ListaFacturaElectronica listamontos = new ListaFacturaElectronica();

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
                oDocumentoCab.DocCRECPE = new DocCRECPE();

                ListaFacturaElectronica = (ListaFacturaElectronica)Session["ListaCabeceraCRE"];

                ImageButton btnDetalles = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btnDetalles.NamingContainer;

                string xmlGenerar = string.Empty;

                tpodocumento = GridView1.Rows[gvrow.RowIndex].Cells[7].Text;
                serie = GridView1.Rows[gvrow.RowIndex].Cells[8].Text;
                nrodocumento = GridView1.Rows[gvrow.RowIndex].Cells[9].Text;
                MontoTotal = Convert.ToDecimal(GridView1.Rows[gvrow.RowIndex].Cells[14].Text).ToString();
                string montotot = GridView1.Rows[gvrow.RowIndex].Cells[14].Text;

                //tpodocumento = GridView1.Rows[gvrow.RowIndex].Cells[8].Text;
                //serie = GridView1.Rows[gvrow.RowIndex].Cells[9].Text;
                //nrodocumento = GridView1.Rows[gvrow.RowIndex].Cells[10].Text;
                //MontoTotal = Convert.ToDecimal(GridView1.Rows[gvrow.RowIndex].Cells[15].Text).ToString();
                //string montotot = GridView1.Rows[gvrow.RowIndex].Cells[15].Text;


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
                        //oDocumentoCab.Empresa.RazonSocial = ListaFacturaElectronica[i].Empresa.RazonSocial;
                        oDocumentoCab.Empresa.RazonSocial = oUsuarioLogeado.Empresa.RazonSocial;

                        oDocumentoCab.Empresa.Direccion = ListaFacturaElectronica[i].Empresa.Direccion;
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

                        oDocumentoCab.Estado.IdEstado = ListaFacturaElectronica[i].Estado.IdEstado;
                        oDocumentoCab.Estado.Descripcion = ListaFacturaElectronica[i].Estado.Descripcion;

                        xmlGenerar = ListaFacturaElectronica[i].XML;

                        oDocumentoCab.MontoTotalCad = montotot;

                        NombreArchivo = ListaFacturaElectronica[i].NombreArchivoXML;
                        break;
                    }
                }
                Singleton.Instance.CreateDirectory(Server.MapPath("~/DocumentoXML/"));

                //var xmldoc = new XmlDocument();
                //xmldoc.InnerXml = xmlGenerar;
                //xmldoc.Save(Server.MapPath("~/DocumentoXML/" + NombreArchivo + ".xml"));

                Response.Clear();

                switch (tpodocumento)
                {
                    case Constantes.Retencion:
                        {
                            ObtenerValoresXMLCREDET(xmlGenerar);

                            //oDocumentoCab.TasaRetencion = oFactura.DocCRECPE.TasaRetencion;
                            //oDocumentoCab.DocCRECPE.ImporteTotalRetenido = oFactura.DocCRECPE.ImporteTotalRetenido;
                            oDocumentoCab.TasaRetencion = oFactura.TasaRetencion;
                            oDocumentoCab.DocCRECPE.ImporteTotalRetenido = decimal.Parse(oFactura.ImporteTotalRetenido);
                            break;
                        }
                    case Constantes.Percepcion:
                        { break; }
                }

                var file = Path.Combine(HttpContext.Current.Server.MapPath("../../DocumentoXML/" + NombreArchivo + ".xml"));
                if (System.IO.File.Exists(file))
                    System.IO.File.Delete(file);


                tpodocumento = string.Empty;

                //Session["DocumentoDet"] = oListaDetalle;
                //Session["ListaMontos"] = listamontos;


                Session["DocumentoCabCRE"] = oDocumentoCab;
                Session["DocumentoDetCRE"] = oFactura.ListaDocCRECPE;
                //Session["DocumentoDetCRE"] = objlistaDocCRECPE;

                tpodocumento = string.Empty;

                string form = "../../Views/Facturacion/DetDocumentoCRE";                
                string script = @"<script type='text/javascript'>";
                script += "window.open('" + form + "',null,'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=1,width=889,height=500,left=100,top=100');";
                script += @"</script>";
                Response.Write(script);
            }
            catch (Exception ex) { }
        }
        #endregion

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string desc = e.Row.Cells[14].Text;
                    //string desc2 = e.Row.Cells[14].Text;
                    //string desc3 = e.Row.Cells[14].Text;

                    if (desc.Contains("pendiente") || desc.Contains("Pendiente"))
                    { e.Row.ForeColor = System.Drawing.Color.Blue; }

                    if (desc.Contains("enviado a Sunat") || desc.Contains("enviado a sunat"))
                    { e.Row.ForeColor = System.Drawing.Color.DarkRed; }

                    if (desc.Contains("aceptado por Sunat") || desc.Contains("aceptado por sunat"))
                    { e.Row.ForeColor = System.Drawing.Color.Red; }
                }
            }
            catch (Exception ex) { }
        }



        #region METHOD FOR PRINT

        

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            List<string> listDocPrint = new List<string>();
            listDocPrint = new List<string>();


            PathWriteFile = Singleton.Instance.Get_PathWriteOrder(PathLogSLINADE);

            for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
            {
                CheckBox check = ((CheckBox)GridView1.Rows[i].Cells[0].FindControl("chkSelImp"));

                if (check.Checked)
                {
                    listDocPrint.Add("Print: " + (i + 1));
                }
            }

            try
            {
                if (GridView1.Rows.Count < Constantes.ValorUno)
                {
                    Response.Write("<script language=javascript>alert('No hay Registros');</script>");
                }
                else if (listDocPrint.Count < Constantes.ValorUno)
                {
                    Response.Write("<script language=javascript>alert('Seleccione Documentos para imprimir');</script>");
                }
                else
                {
                    #region PRINT

                    var list_Write = new List<string>();

                    ObtenerUsuarioLogeado();

                    Encoding encoding = Encoding.GetEncoding("ISO-8859-1");

                    for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
                    {

                        CheckBox check = ((CheckBox)GridView1.Rows[i].Cells[1].FindControl("chkSelImp"));

                        string valueDoc = string.Empty;
                        string TypeDoc = GridView1.Rows[i].Cells[7].Text;
                        string SerieDoc = GridView1.Rows[i].Cells[8].Text;
                        string Num_Doc = GridView1.Rows[i].Cells[9].Text;

                        string Copies = txtCopies.Value.Length < Constantes.ValorUno ? Constantes.ValorUno + string.Empty : txtCopies.Value;

                        string num_ce = oUsuarioLogeado.Empresa.RUC + "-" + TypeDoc + "-" + SerieDoc + "-" + Num_Doc; ;


                        ListaFacturaElectronica = (ListaFacturaElectronica)Session["ListaCabeceraCRE"];
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
                            myXml.Estado_Desc = "Doc For Print";
                            myXml.TypeFormat = typeFormat + string.Empty;
                            myXml.PrintName = cboimpresoras.Value;
                            myXml.Copies = Copies;

                            //OK
                            Singleton.Instance.CreateDirectory(PathWriteFile + @"smp\prt\");

                            XmlSerializer xml = new XmlSerializer(typeof(DocumentState));

                            using (StreamWriter sw = new StreamWriter(PathWriteFile + @"smp\prt\" + num_ce + ".xml", false, Encoding.GetEncoding("ISO-8859-1")))
                            {
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

                    //string script = @"<script type='text/javascript'>";
                    //script += @"</script>";
                    //Response.Write(script);
                }
            }
            catch (System.Exception ex) { }
        }

        #endregion METHOD FOR PRINT

        protected void btnSend_Click(object sender, EventArgs e)
        {
            var listSelect = new List<string>();

            PathWriteFile = Singleton.Instance.Get_PathWriteOrder(PathLogSLINADE);

            for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
            {
                CheckBox check = ((CheckBox)GridView1.Rows[i].Cells[0].FindControl("chkSelSend"));
                if (check.Checked)
                {
                    listSelect.Add("Select: " + (i + 1));
                }
            }

            try
            {
                if (GridView1.Rows.Count < Constantes.ValorUno)
                {
                    Response.Write("<script language=javascript>alert('No hay Registros');</script>");
                }
                if (listSelect.Count < Constantes.ValorUno)
                {
                    Response.Write("<script language=javascript>alert('Seleccione registros para enviar');</script>");
                }

                #region method generate xml for send

                ObtenerUsuarioLogeado();

                var list_Write = new List<string>();

                for (int i = 0; i <= GridView1.Rows.Count - 1; i++)
                {
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


                        XmlSerializer xml = new XmlSerializer(typeof(DocumentSend));

                        Singleton.Instance.CreateDirectory(PathWriteFile + @"smc\cds\");

                        using (StreamWriter sw = new StreamWriter(PathWriteFile + @"smc\cds\" + num_ce + ".xml", false, Encoding.GetEncoding("ISO-8859-1")))
                        {
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

        protected void btnImgCDR_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ObtenerUsuarioLogeado();
                ListaFacturaElectronica = (ListaFacturaElectronica)Session["ListaCabecera"];
                ImageButton btnDetalles = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btnDetalles.NamingContainer;

                string xmlGenerar = string.Empty;

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
                Response.AddHeader("Content-Disposition", "attachment; filename=" + "R-" + NombreArchivo + ".xml");
                Response.ContentType = "application/octet-stream";
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