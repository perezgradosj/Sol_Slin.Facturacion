using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using System.IO;
using System.Xml;
using System.Drawing;

using Slin.Facturacion.Common;
using Slin.Facturacion.Proxies.ServicioFacturacion;
using Slin.Facturacion.Electronica.Web.ServiceFacturacion;
using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;
using Slin.Facturacion.Electronica.Web.Helper.Print;
using System.Xml.Serialization;

using xmlFac = Slin.Facturacion.Common.CE;
using Slin.Facturacion.Electronica.Web.Views.Util;

namespace Slin.Facturacion.Electronica.Web.Views.Facturacion
{
    public partial class ConsultaDocAnuladoSUNAT : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                Cargar();
            }
        }


        #region SESION USUARIO LOGEADO

        #endregion

        #region ENTITY
        ServiceWebFactController objController = new ServiceWebFactController();
        FacturaElectronica oFacturaElectronica = new FacturaElectronica();
        DetalleFacturaElectronica oDetalleFacturaElectronica = new DetalleFacturaElectronica();

        Serie oSerie = new Serie();
        ListaSerie oListaSerie = new ListaSerie();
        ListaTipoDocumento oListaTipoDocumento = new ListaTipoDocumento();
        ListaEstado oListaEstado = new ListaEstado();

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
                //btnexportarexcel.Visible = false;

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
                btnExportarExcel.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnExportarExcel.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);

            }
            catch (Exception ex)
            {

            }
            
        }

        void ObtenerDatosSerie()
        {
            try
            {
                oSerie = new Serie();
                oSerie.TipoDocumento = new TipoDocumento();
                oSerie.Empresa = new Empresa();

                oSerie.TipoDocumento.IdTipoDocumento = Constantes.ValorCero;
                //oSerie.Empresa.IdEmpresa = txtestado.Text.Length == 0 ? Constantes.ValorCero : Convert.ToInt32(txtestado.Text);
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
                oListaTipoDocumento = ServiceFacturacionController.Instance.ListarTipoDocumento();
                oListaTipoDocumento.RemoveAt(0);
                oListaTipoDocumento.Insert(0, new TipoDocumento() { IdTipoDocumento = Constantes.ValorCero, Descripcion = Constantes.ValorTodos });

                var listatpodoc = new ListaTipoDocumento();
                foreach (var tpo in oListaTipoDocumento)
                {
                    if (tpo.CodigoDocumento != Constantes.Retencion && tpo.CodigoDocumento != Constantes.Percepcion)
                    {
                        listatpodoc.Add(tpo);
                    }
                }
                cbotipodocumento.DataSource = listatpodoc;
                //cbotipodocumento.DataSource = oListaTipoDocumento;
                cbotipodocumento.DataValueField = "IdTipoDocumento";
                cbotipodocumento.DataTextField = "Descripcion";
                cbotipodocumento.DataBind();

                oListaEstado = ServiceFacturacionController.Instance.ListarEstadoDocumento();
                oListaEstado.RemoveAt(0);
                oListaEstado.Insert(0, new Estado() { IdEstado = Constantes.ValorCero, Descripcion = Constantes.ValorTodos });

                cboestado.DataSource = oListaEstado;
                cboestado.DataValueField = "IdEstado";
                cboestado.DataTextField = "Descripcion";
                cboestado.DataBind();

                ObtenerDatosSerie();

                oListaSerie = ServiceFacturacionController.Instance.ListarSerie(oSerie);
                oListaSerie.Insert(0, new Serie() { IdSerie = Constantes.ValorCero, NumeroSerie = Constantes.ValorTodos });


                var listserie = new ListaSerie();

                foreach (var serie in oListaSerie)
                {
                    if (!serie.NumeroSerie.Contains("R") && !serie.NumeroSerie.Contains("P"))
                    {
                        listserie.Add(serie);
                    }
                }

                cboserie.DataSource = listserie;
                //cboserie.DataSource = oListaSerie;
                cboserie.DataValueField = "IdSerie";
                cboserie.DataValueField = "NumeroSerie";
                cboserie.DataBind();

                txtfechadesde.Value = DateTime.Now.AddDays(-7).ToShortDateString();
                txtfechahasta.Value = DateTime.Now.ToString("dd/MM/yyyy");
            }
            catch (Exception ex)
            {

            }
            
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
                    if (!serie.NumeroSerie.Contains("R") && !serie.NumeroSerie.Contains("P"))
                    {
                        listserie.Add(serie);
                    }
                }
                cboserie.DataSource = listserie;

                //cboserie.DataSource = oListaSerie;
                cboserie.DataValueField = "IdSerie";
                cboserie.DataValueField = "NumeroSerie";
                cboserie.DataBind();
            }
            catch (Exception ex)
            {

            }
            
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
                oFacturaElectronica.NumeroDocumentoInicio = txtnuminicio.Value == null ? String.Empty : txtnuminicio.Value;
                oFacturaElectronica.NumeroDocumentoFin = txtnumfin.Value == null ? String.Empty : txtnumfin.Value;
                oFacturaElectronica.Estado.IdEstado = Convert.ToInt32(cboestado.Value);

                oFacturaElectronica.Cliente.ClienteRuc = txtclienteruc.Value.Length == 0 ? string.Empty : txtclienteruc.Value;
                oFacturaElectronica.Cliente.Nombres = txtclientenombres.Value.Length == 0 ? string.Empty : txtclientenombres.Value;

                oFacturaElectronica.Empresa.RUC = oUsuarioLogeado.Empresa.RUC;

                oFacturaElectronica.Estado.IdEstadoSUNAT = Constantes.ValorUno;
            }
            catch (Exception ex)
            {

            }
            
            
        }

        public Int32 Inicio = Constantes.ValorCero;
        public Int32 Fin = Constantes.ValorCero;

        public Int32 val = Constantes.ValorCero;
        public DateTime fecha1;
        public DateTime fecha2;

        void ValidarParametros()
        {
            try
            {
                if (txtfechadesde.Value.Length > 0)
                {
                    fecha1 = Convert.ToDateTime(txtfechadesde.Value);
                    fecha2 = Convert.ToDateTime(txtfechahasta.Value);
                    int result = DateTime.Compare(fecha1, fecha2);
                    val = result < 0 ? 2 : result;
                }
                else
                {
                    val = Constantes.ValorDos;
                }
                Inicio = txtnuminicio.Value == String.Empty ? Constantes.ValorCero : Convert.ToInt32(txtnuminicio.Value);
                Fin = txtnumfin.Value == String.Empty ? Constantes.ValorCero : Convert.ToInt32(txtnumfin.Value);
            }
            catch (Exception ex)
            {

            }
            
        }


        void CargarData()
        {
            try
            {
                ObtenerUsuarioLogeado();//para la empresa del usuario logeado

                Session.Remove("ListaCabecera");
                //Session.Remove("ListaCabecera2");
                ValidarParametros();
                if (Inicio > Fin)
                {
                    Response.Write(Constantes.MensajeNumDoc);

                }
                else if (val == Constantes.ValorUno)
                {
                    Response.Write(Constantes.MensajeFecha);
                }
                else if (val == Constantes.ValorDos)
                {
                    ObtenerDatos();
                    ListaFacturaElectronica = ServiceFacturacionController.Instance.ListarDocumentoCabecera(oFacturaElectronica);

                    for (int i = 0; i <= ListaFacturaElectronica.Count - 1; i++)
                    {
                        if (ListaFacturaElectronica[i].Estado.Descripcion.Contains("En"))
                        {

                        }
                    }

                    GridView1.DataSource = ListaFacturaElectronica;
                    GridView1.DataBind();
                }
                Session["ListaCabecera"] = ListaFacturaElectronica;
                //Session["ListaCabecera2"] = ListaFacturaElectronica;
            }
            catch (Exception ex)
            {

            }
            
        }


        private ListaFacturaElectronica listaFacturaElectronica;
        public ListaFacturaElectronica ListaFacturaElectronica
        {
            get { return listaFacturaElectronica; }
            set { listaFacturaElectronica = value; }
        }

        void LimpiarCampos()
        {
            txtnuminicio.Value = String.Empty;
            txtnumfin.Value = String.Empty;
            cboestado.SelectedIndex = 0;
            cboserie.SelectedIndex = 0;
            cbotipodocumento.SelectedIndex = 0;
            txtfechadesde.Value = String.Empty;
            txtfechahasta.Value = String.Empty;
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
            catch (Exception ex)
            {

            }
            
        }

        #region ENTITTY XML_PDF
        public string NombreArchivo = String.Empty;
        public string moneda = string.Empty;
        XmlDocument XMLDocumentPDF = new XmlDocument();
        //Slin.Facturacion.BusinessEntities.ArchivoXML oArchivoXML = new Slin.Facturacion.BusinessEntities.ArchivoXML();

        public string tpodocumento = string.Empty;
        public String serie = string.Empty;
        public String nrodocumento = string.Empty;
        public String MontoTotal = string.Empty;
        public string tipoDocumento = string.Empty;
        #endregion


        #region METHOD XML_PDF
        void CrearNuevaCarpeta()
        {
            try
            {
                string newfolder = Server.MapPath("~/DocumentoXML/");
                Console.WriteLine("New Folder:" + newfolder);
                if (!Directory.Exists(newfolder))
                {
                    Directory.CreateDirectory(newfolder);
                    Console.WriteLine("Folder created: " + newfolder);
                }
            }
            catch (Exception ex)
            {

            }
            
        }

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
                tpodocumento = GridView1.Rows[gvrow.RowIndex].Cells[5].Text;
                serie = GridView1.Rows[gvrow.RowIndex].Cells[6].Text;
                nrodocumento = GridView1.Rows[gvrow.RowIndex].Cells[7].Text;

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
                {
                    Response.Write("<script language=javascript>alert('Este Doc. no tiene Xml Firmado');</script>");
                    return;
                }

                CrearNuevaCarpeta();

                var xmldoc = new XmlDocument();
                xmldoc.InnerXml = xmlGenerar;
                xmldoc.Save(Server.MapPath("~/DocumentoXML/" + NombreArchivo + ".xml"));
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
        //XmlDocument ArchivoPDF = new XmlDocument();
        //ListaFacturaElectronica oListaFacturaPDF = new ListaFacturaElectronica();
        //FacturaElectronica oFacturaPDF = new FacturaElectronica();

        ListaDetalleFacturaElectronica oListaDetalle = new ListaDetalleFacturaElectronica();
        ListaFacturaElectronica ListaMontoCab = new ListaFacturaElectronica();

        ListaDetalleFacturaElectronica objlistadetalle = new ListaDetalleFacturaElectronica();
        DetalleFacturaElectronica objdetalle = new DetalleFacturaElectronica();

        StreamReader sr;
        FacturaElectronica oFactura = new FacturaElectronica();
        XmlDocument XMLArchive = new XmlDocument();

        protected void btnImgPdf_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ObtenerUsuarioLogeado();
                ListaFacturaElectronica = (ListaFacturaElectronica)Session["ListaCabecera"];

                ImageButton btnDetalles = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btnDetalles.NamingContainer;

                string xmlGenerar = string.Empty;
                tpodocumento = GridView1.Rows[gvrow.RowIndex].Cells[5].Text;
                serie = GridView1.Rows[gvrow.RowIndex].Cells[6].Text;
                nrodocumento = GridView1.Rows[gvrow.RowIndex].Cells[7].Text;
                MontoTotal = Convert.ToDecimal(GridView1.Rows[gvrow.RowIndex].Cells[12].Text).ToString();

                for (int i = 0; i <= ListaFacturaElectronica.Count - 1; i++)
                {
                    if (serie == ListaFacturaElectronica[i].Serie.NumeroSerie && nrodocumento == ListaFacturaElectronica[i].NumeroDocumento && tpodocumento == ListaFacturaElectronica[i].TipoDocumento.CodigoDocumento)
                    {
                        NombreArchivo = ListaFacturaElectronica[i].NombreArchivoXML.TrimEnd();
                        moneda = ListaFacturaElectronica[i].Moneda.Descripcion;
                        tipoDocumento = ListaFacturaElectronica[i].TipoDocumento.IdTipoDocumento.ToString();
                        xmlGenerar = ListaFacturaElectronica[i].XML;

                        //oListaDetalle = ServiceFacturacionController.ListarDocumentoDetalle(tipoDocumento, serie, nrodocumento, oUsuarioLogeado.Empresa.RUC);
                        //ListaMontoCab = ServiceFacturacionController.GetListaMontoCab(tipoDocumento, serie, nrodocumento, oUsuarioLogeado.Empresa.RUC);
                        break;
                    }
                }

                if (xmlGenerar.Length < 50)
                {
                    Response.Write("<script language=javascript>alert('Este Doc. no tiene Xml Firmado');</script>");
                    return;
                }

                CrearNuevaCarpeta();

                var xmldoc = new XmlDocument();
                xmldoc.InnerXml = xmlGenerar;
                xmldoc.Save(Server.MapPath("~/DocumentoXML/" + NombreArchivo + ".xml"));

                Response.Clear();

                switch (tpodocumento)
                {
                    case "01":
                    case "03":
                        {
                            ObtenerValoresXMLCE();
                            sr.Close();
                            break;
                        }
                    case "07":
                    case "08":
                        {
                            sr.Close();
                            break;
                        }
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

        // 01, 03
        void ObtenerValoresXMLCE()
        {
            try
            {
                XMLArchive.Load(Server.MapPath("~/DocumentoXML/" + NombreArchivo + ".xml"));//ALL

                XmlNodeList xmlnodelist; //ALL

                objlistadetalle = new ListaDetalleFacturaElectronica();
                objdetalle = new DetalleFacturaElectronica();
                oFactura = new FacturaElectronica();
                oFactura.Empresa = new Empresa();
                oFactura.Cliente = new Cliente();
                oFactura.TipoDocumento = new TipoDocumento();
                oFactura.Empresa.TipoDocumentiIdentidad = new TipoDocumentoIdentidad();
                oFactura.Cliente.TipoDocumentoIdentidad = new TipoDocumentoIdentidad();



                XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlFac.InvoiceType));
                //StreamReader sr = new StreamReader(Server.MapPath("~/DocumentoXML/" + NombreArchivo + ".xml"));
                sr = new StreamReader(Server.MapPath("~/DocumentoXML/" + NombreArchivo + ".xml"));
                xmlFac.InvoiceType inv = (xmlFac.InvoiceType)xmlSerial.Deserialize(sr);


                oFactura.NombreArchivoXML = NombreArchivo;
                oFactura.SerieCorrelativo = inv.ID.Value; //SERIE CORRELATIVO

                string[] array = oFactura.SerieCorrelativo.Split('-');
                oFactura.NumeroSerie = array[0];
                oFactura.NumeroDocumento = array[1];


                oFactura.FechaEmision2 = inv.IssueDate.Value.ToString("yyyy-MM-dd");
                oFactura.FechaEmision = inv.IssueDate.Value;

                oFactura.TipoDocumento.CodigoDocumento = inv.InvoiceTypeCode.Value; //TIPO CE

                //oFactura.TipoMoneda = inv.DocumentCurrencyCode.Value; //TIPO MONEDA
                oFactura.TipoMoneda = moneda;


                oFactura.Empresa.RUC = inv.AccountingSupplierParty.CustomerAssignedAccountID.Value; //RUC EMISOR

                oFactura.Empresa.TipoDocumentiIdentidad.Codigo = inv.AccountingSupplierParty.AdditionalAccountID.First().Value; //TPO DOC EMISOR

                oFactura.Empresa.RazonSocial = inv.AccountingSupplierParty.Party.PartyLegalEntity.ToList().First().RegistrationName.Value; // RAZON SOCIAL EMISOR
                oFactura.Empresa.RazonComercial = inv.AccountingSupplierParty.Party.PartyName.First().Name.Value; //RAZON COMERCIAL

                oFactura.Empresa.CodigoUbigeo = inv.AccountingSupplierParty.Party.PostalAddress.ID.Value; //UBIGEO EMISOR
                oFactura.Empresa.Direccion = inv.AccountingSupplierParty.Party.PostalAddress.StreetName.Value; //DIRECCION EMISOR



                oFactura.Cliente.ClienteRuc = inv.AccountingCustomerParty.CustomerAssignedAccountID.Value; //NRO DOC CLIENTE
                oFactura.Cliente.TipoDocumentoIdentidad.Codigo = inv.AccountingCustomerParty.AdditionalAccountID.First().Value; // TPO DOC CLIENTE


                string dir = inv.AccountingCustomerParty.Party.PostalAddress.StreetName.Value;
                oFactura.Cliente.Direccion = dir != null ? dir : "-"; //DIRECCION CLIENTE

                oFactura.Cliente.RazonSocial = inv.AccountingCustomerParty.Party.PartyLegalEntity.First().RegistrationName.Value; //RAZON SOCIAL CLIENTE



                //TOTALES

                oFactura.MontoIgvCad = string.Empty + inv.TaxTotal.First().TaxAmount.Value; //TOTAL IGV
                //oFactura.MontoIgvCad = string.Empty + inv.TaxTotal.First().TaxSubtotal.First().TaxAmount.Value; //TOTAL IGV
                oFactura.MontoTotalCad = string.Empty + inv.LegalMonetaryTotal.PayableAmount.Value; //IMPORTE TOTAL

                xmlnodelist = XMLArchive.GetElementsByTagName("sac:AdditionalMonetaryTotal"); //FOR SUB TOTAL AND OTHER TOTALES
                for (int i = 0; i <= xmlnodelist.Count - 1; i++)
                {
                    switch (xmlnodelist.Item(i).ChildNodes[0].InnerText)
                    {
                        #region CASE

                        case "1001":
                            {
                                oFactura.TotalGravado = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "1002":
                            {
                                oFactura.TotalInafecto = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "1003":
                            {
                                oFactura.TotalExonerado = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "1004":
                            {
                                oFactura.TotalGratuito = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "1005":
                            {
                                oFactura.SubTotalVenta = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "2001":
                            {
                                oFactura.TotalPercepciones = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "2002":
                            {
                                oFactura.TotalRetenciones = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "2003":
                            {
                                oFactura.TotalDetracciones = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "2004":
                            {
                                oFactura.TotalBonificaciones = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "2005":
                            {
                                oFactura.TotalDescuento = xmlnodelist.Item(i).ChildNodes[1].InnerText; ;
                                break;
                            }

                        #endregion
                    }
                }

                if (oFactura.TotalDescuento == null || oFactura.TotalDescuento.Length == Constantes.ValorCero)
                    oFactura.TotalDescuento = Constantes.ValorCeroMonto;


                xmlnodelist = XMLArchive.GetElementsByTagName("sac:AdditionalProperty"); // MONTO LITERAL
                oFactura.MontoTotalLetras = xmlnodelist.Item(0).ChildNodes[1].InnerText;


                foreach (var line in inv.InvoiceLine)
                {
                    objdetalle = new DetalleFacturaElectronica();
                    objdetalle.NroOrden = int.Parse(line.ID.Value); //NRO DE ORDEN
                    objdetalle.Cantidad = line.InvoicedQuantity.Value; //CANTIDAD

                    objdetalle.ValorVenta = line.LineExtensionAmount.Value; //VALOR VENTA

                    objdetalle.PrecioVenta = line.PricingReference.AlternativeConditionPrice.First().PriceAmount.Value; //PRECIO VENTA

                    objdetalle.SubTotalTexto = string.Empty + objdetalle.PrecioVenta * objdetalle.Cantidad; // SUB TOTAL X DETALLE

                    objdetalle.IGV = line.TaxTotal.First().TaxAmount.Value; // TOTAL IGV * DETALLE

                    //objdetalle.ValorUnitario = line.Price.PriceAmount.Value; //VALOR UNITARIO
                    objdetalle.ValorUnitario = line.PricingReference.AlternativeConditionPrice.First().PriceAmount.Value; //VALOR UNITARIO
                    objdetalle.ValorUnitario = decimal.Round(objdetalle.ValorUnitario - (objdetalle.IGV / objdetalle.Cantidad), 2);

                    objdetalle.Descripcion = line.Item.Description.First().Value; //DESCRIPCION PRODUCTO
                    objdetalle.CodigoProducto = line.Item.SellersItemIdentification.ID.Value; //CODIGO PRODUCTO
                    objdetalle.Unidad = line.InvoicedQuantity.unitCode.ToString();// UNIDAD PRODUCTO

                    objdetalle.NombreArchivoXML = oFactura.NombreArchivoXML;

                    objdetalle.NumeroSerie = oFactura.NumeroSerie;
                    objdetalle.NumeroDocumento = oFactura.NumeroDocumento;

                    //objdetalle.Importe = decimal.Round(decimal.Parse(objdetalle.SubTotalTexto), 2);
                    objdetalle.Importe = decimal.Round(objdetalle.ValorUnitario * objdetalle.Cantidad, 2); // SUB TOTAL X DETALLE


                    objlistadetalle.Add(objdetalle);
                }

                xmlnodelist = XMLArchive.GetElementsByTagName("DigestValue"); //VALOR RESUMEN 
                oFactura.ValorResumen = xmlnodelist.Item(0).InnerText.Length == 0 ? string.Empty : xmlnodelist.Item(0).InnerText; ;

                xmlnodelist = XMLArchive.GetElementsByTagName("SignatureValue"); //VALOR FIRMA
                oFactura.ValorFirma = xmlnodelist.Item(0).InnerText.Length == 0 ? string.Empty : xmlnodelist.Item(0).InnerText;


                //XmlNodeList xmlnodelist;


                xmlnodelist = XMLArchive.GetElementsByTagName("Extra");

                for (int i = 0; i <= xmlnodelist.Count - 1; i++)
                {
                    oFactura.Extra.ExLinea = xmlnodelist.Item(i).ChildNodes[0].InnerText;
                    oFactura.Extra.ExDato = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                    oFactura.Extra.ExTipo = xmlnodelist.Item(i).ChildNodes[2].InnerText;
                    oFactura.ListaExtra.Add(oFactura.Extra);
                }

                Session["FacturaPDF"] = oFactura;
                Session["FacturaDetallePDF"] = objlistadetalle;

                sr.Close();
            }
            catch (Exception ex)
            {
                sr.Close();
            }
            
        }


        void Inicializa()
        {

            objlistadetalle = new ListaDetalleFacturaElectronica();
            objdetalle = new DetalleFacturaElectronica();
            oFactura = new FacturaElectronica();
            oFactura.Empresa = new Empresa();
            oFactura.Cliente = new Cliente();
            oFactura.TipoDocumento = new TipoDocumento();
            oFactura.Empresa.TipoDocumentiIdentidad = new TipoDocumentoIdentidad();
            oFactura.Cliente.TipoDocumentoIdentidad = new TipoDocumentoIdentidad();

            oFactura.DocCRECPE = new DocCRECPE();

            oFactura.Extra = new Extra();
            oFactura.ListaExtra = new ListaExtra();
        }

        #endregion

        #region EXPORT TO EXCEL

        void EnviarToExcel()
        {
            try
            {
                CrearNuevaCarpeta();
                ObtenerDatos();
                var lista = new ListaDocumento();
                //lista = ServiceFacturacionController.GetListaDocumentoCabExcel(oFacturaElectronica);
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

                tpodocumento = GridView1.Rows[gvrow.RowIndex].Cells[5].Text;
                serie = GridView1.Rows[gvrow.RowIndex].Cells[6].Text;
                nrodocumento = GridView1.Rows[gvrow.RowIndex].Cells[7].Text;
                MontoTotal = Convert.ToDecimal(GridView1.Rows[gvrow.RowIndex].Cells[12].Text).ToString();

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
                        oDocumentoCab.Empresa.RazonSocial = ListaFacturaElectronica[i].Empresa.RazonSocial;
                        oDocumentoCab.Empresa.Direccion = ListaFacturaElectronica[i].Empresa.Direccion;
                        oDocumentoCab.Empresa.Ubigeo.CodigoUbigeo = ListaFacturaElectronica[i].Empresa.Ubigeo.CodigoUbigeo;
                        oDocumentoCab.Empresa.Ubigeo.Descripcion = ListaFacturaElectronica[i].Empresa.Ubigeo.Descripcion;

                        oDocumentoCab.Cliente.NumeroDocumentoIdentidad = ListaFacturaElectronica[i].Cliente.NumeroDocumentoIdentidad;
                        oDocumentoCab.Cliente.Nombres = ListaFacturaElectronica[i].Cliente.Nombres;
                        oDocumentoCab.Cliente.Direccion = ListaFacturaElectronica[i].Cliente.Direccion;
                        oDocumentoCab.TipoMoneda = ListaFacturaElectronica[i].Moneda.Descripcion;

                        tipoDocumento = ListaFacturaElectronica[i].TipoDocumento.IdTipoDocumento.ToString();

                        //oListaDetalle = ServiceFacturacionController.ListarDocumentoDetalle(tipoDocumento, serie, nrodocumento, oUsuarioLogeado.Empresa.RUC);
                        //listamontos = ServiceFacturacionController.GetListaMontoCab(tipoDocumento, serie, nrodocumento, oUsuarioLogeado.Empresa.RUC);
                        break;
                    }
                }
                Session["DocumentoCab"] = oDocumentoCab;
                Session["DocumentoDet"] = oListaDetalle;
                Session["ListaMontos"] = listamontos;

                tpodocumento = string.Empty;

                //string form = "../../Views/Facturacion/DetalleDocumento";
                //string script = @"<script type='text/javascript'>";
                //script += "window.open('" + form + "',null,'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=1,width=1200,height=500,left=100,top=100');";
                //script += @"</script>";
                //Response.Write(script);

                string form = "../../Views/Facturacion/DetalleDoc";
                //string form = "../../Views/Facturacion/DetalleDocumento";
                string script = @"<script type='text/javascript'>";
                script += "window.open('" + form + "',null,'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=1,width=889,height=500,left=100,top=100');";
                script += @"</script>";
                Response.Write(script);
            }
            catch (Exception ex)
            {

            }
            
        }


        #endregion


        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string desc = e.Row.Cells[10].Text;

                    if (desc.Contains("ANUL") || desc.Contains("Anul") || desc.Contains("anul") || desc.Contains("X-") || desc.Contains("x") || desc.Contains("x-"))
                    {
                        e.Row.ForeColor = Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}