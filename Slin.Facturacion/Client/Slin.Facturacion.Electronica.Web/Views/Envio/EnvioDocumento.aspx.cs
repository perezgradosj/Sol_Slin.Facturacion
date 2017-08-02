using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Slin.Facturacion.Proxies.ServicioFacturacion;
using Slin.Facturacion.Electronica.Web.ServiceFacturacion;
using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;
using Slin.Facturacion.Common;
using System.Xml;
using System.Text;
using System.IO;


using iTextSharp.text.pdf;
using System.Drawing;
using Microsoft.Reporting.WebForms;

using xmlFac = Slin.Facturacion.Common.CE;
using xmlCre = Slin.Facturacion.Common.CRE;

using xmlNotDeb = Slin.Facturacion.Common.CEND;
using xmlNotCred = Slin.Facturacion.Common.CENC;
using System.Xml.Serialization;
using Slin.Facturacion.Common.Method;
using Slin.Facturacion.Electronica.Web.Views.Util;

namespace Slin.Facturacion.Electronica.Web.Views.Envio
{
    public partial class EnvioDocumento : System.Web.UI.Page
    {
        public string EmailClienteSeleccionado = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                Cargar();
                //Mensaje();
                MensajeFail();

                if (Session["EmailClienteSeleccionado"] == null)
                {
                    Session.Remove("EmailClienteSeleccionado");
                }
                else
                {
                    EmailClienteSeleccionado = (String)Session["EmailClienteSeleccionado"];

                    txtemaildestino.Value = EmailClienteSeleccionado.Length == Constantes.ValorCero ? string.Empty : EmailClienteSeleccionado;

                    Session.Remove("EmailClienteSeleccionado");
                }
            }

            if (this.FileUpload1.HasFile)
            {
                listadjunt = (List<string>)Session["listSendAdjFiles"];
                FileUpload f = FileUpload1;

                this.FileUpload1.SaveAs(Server.MapPath("~/DocumentoXML/" + FileUpload1.FileName));

                ListItem item = new ListItem();
                item.Value = f.FileName;
                //item.Text = f.FileName + " (" + f.FileContent.Length.ToString("NO") + " bytes).";

                var bytess = decimal.Round(decimal.Parse((f.FileContent.Length * 0.001) + string.Empty), 3);
                item.Text = f.FileName + "                                       " + bytess + string.Empty + "KB";
                //f.SaveAs(Server.MapPath("~/DocumentoXML/" + item.Value));
                lstFiles.Items.Add(item);
                listadjunt.Add(f.FileName);
                Session["listSendAdjFiles"] = listadjunt;

                lstFiles.Visible = lstFiles.Items.Count > Constantes.ValorCero ? true : false;
                btnDeleteSelected.Visible = lstFiles.Items.Count > Constantes.ValorCero ? true : false;              
            }
        }




        void Mensaje()
        {
            try
            {
                string respuesta = (string)Session["RespuestaEnvio"];
                if (respuesta == Constantes.msjEnviadoCorrectamente)
                {
                    lblmensajerespuesta.InnerText = (string)Session["RespuestaEnvio"];
                    lblmensajerespuesta.Visible = true;
                }
                else
                {
                    lblmensajerespuesta.InnerText = (string)Session["RespuestaEnvio"];
                    lblmensajerespuesta.Visible = true;
                }
                Session.Remove("RespuestaEnvio");
                Session.Remove("MsjErrorSerializerXML");
            }
            catch (Exception ex)
            {

            }
        }

        void MensajeFail()
        {
            try
            {
                string respuesta = (string)Session["MsjErrorSerializerXML"];
                if (respuesta != null && respuesta.Length > 0 && Session["RespuestaEnvio"] == null)
                {
                    Session.Remove("MsjErrorSerializerXML");
                    lblmensajerespuesta.InnerText = respuesta;
                    lblmensajerespuesta.Visible = true;
                }
                else //if (Session["RespuestaEnvio"] != null)z
                {
                    //lblmensajerespuesta.InnerText = (string)Session["RespuestaEnvio"];
                    //lblmensajerespuesta.Visible = true;
                    Mensaje();
                }

            }
            catch (Exception ex)
            {

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

        private FacturaElectronica facturaElectronica;
        public FacturaElectronica oFacturaElectronica
        {
            get { return facturaElectronica; }
            set { facturaElectronica = value; }
        }

        private FacturaElectronica generafacturaElectronica;
        public FacturaElectronica oGeneraFacturaElectronica
        {
            get { return generafacturaElectronica; }
            set { generafacturaElectronica = value; }
        }

        private ListaFacturaElectronica listaFacturaElectronica;
        public ListaFacturaElectronica oListaFacturaElectronica
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

        public string destino, asunto, cuerpo, usuario, password, referente;

        private Documento odocumentoenvio;
        public Documento oDocumentoEnvio
        {
            get { return odocumentoenvio; }
            set { odocumentoenvio = value; }
        }

        #endregion


        #region METHOD

        void Cargar()
        {
            string Url = HttpContext.Current.Request.Url.AbsoluteUri;
            int result = objController.Process_UrlPage(Url);
            if (result == Constantes.ValorCero)
            { Response.Redirect("~/Views/Home/Inicio.aspx"); }

            Singleton.Instance.CreateDirectory(Server.MapPath("~/DocumentoXML/"));
            try
            {
                ObtenerUsuarioLogeado();
                lblempresa.InnerText = oUsuarioLogeado.Empresa.RazonSocial.ToUpper();
                CargarListas();
                CapturarUserLogRoles();
                logoEmpresa.Src = "../../Img/home/" + oUsuarioLogeado.Empresa.RUC + ".png";
                listadjunt = new List<string>();
                Session["listSendAdjFiles"] = listadjunt;
            }
            catch (Exception ex) { }
        }


        WCFSeguridad.ListaRol oListaRolesUserLog = new WCFSeguridad.ListaRol();
        void CapturarUserLogRoles()
        {
            try
            {
                oListaRolesUserLog = objController.GetList_RolUserLog();

                btnEnviar.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnEnviar.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);

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

                var lista = new ListaTipoDocumento();

                lista.Insert(0, new TipoDocumento() { IdTipoDocumento = 0, Descripcion = Constantes.ValorSeleccione });
                foreach (var tpodoc in oListaTipoDocumento)
                {
                    switch (tpodoc.CodigoDocumento)
                    {
                        case Constantes.Factura:
                        case Constantes.Boleta:
                        case Constantes.NotaCredito:
                        case Constantes.NotaDebito:
                        case Constantes.Retencion:
                            {
                                lista.Add(tpodoc);
                                break;
                            }
                    }
                }

                //cbotipodocumento.DataSource = oListaTipoDocumento;

                cbotipodocumento.DataSource = lista;
                cbotipodocumento.DataValueField = "IdTipoDocumento";
                cbotipodocumento.DataTextField = "Descripcion";
                cbotipodocumento.DataBind();

                Session["AllListaTipoDocumento"] = oListaTipoDocumento;

                txtasunto.Value = string.Empty;
                txtemaildestino.Value = string.Empty;
                txtmensaje.Value = string.Empty;
                txtnrodocumento.Value = string.Empty;
                txtnroserie.Value = string.Empty;

                Singleton.Instance.CreateDirectory(Server.MapPath("~/DocumentoXML/"));
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
                oFacturaElectronica.Serie = new Serie();
                oFacturaElectronica.TipoDocumento = new TipoDocumento();
                oFacturaElectronica.Empresa = new Empresa();

                oFacturaElectronica.Serie.NumeroSerie = txtnroserie.Value.Length == 0 ? string.Empty : txtnroserie.Value;
                oFacturaElectronica.TipoDocumento.CodigoDocumento = cbotipodocumento.Value;
                //oFacturaElectronica.NumeroDocumento = txtnrodocumento.Value.Length > 0 ? string.Format("{0:00000000}", int.Parse(txtnrodocumento.Value)) : string.Empty;
                oFacturaElectronica.NumeroDocumento = string.Format("{0:00000000}", int.Parse(txtnrodocumento.Value));
                oFacturaElectronica.Empresa.RUC = oUsuarioLogeado.Empresa.RUC;
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

        void ObtenerDatosDocumentoDB()
        {
            try
            {
                ObtenerDatos();
                oGeneraFacturaElectronica = ServiceFacturacionController.Instance.GetObtenerDocumentoUnico(oFacturaElectronica);
            }
            catch (Exception ex) { }
        }

        void GenerarArchivos()
        {
            try
            {
                ObtenerDatosDocumentoDB();
                if (oGeneraFacturaElectronica.NombreArchivoXML == null)
                {
                    //lblmensajerespuesta.Visible = false;
                    lblmensajerespuesta.InnerText = "El Documento no Existe o los Datos son Incorrectos";
                    lblmensajerespuesta.Visible = true;
                    //Response.Write("<script language=javascript>alert('El Documento no Existe o los Datos son Incorrectos');</script>");
                    return;
                }
                else
                {
                    CrearArchivoXML();
                    CrearArchivoPDF();
                    Session["RespuestaEnvio"] = Constantes.msjEnviadoCorrectamente;
                    Limpiar();
                    LimpiarCampos();
                    Session.Remove("DatosEmail");
                    Session.Remove("EnvioFacturaPDF");
                    Session.Remove("EnvioFacturaDetallePDF");
                }
            }
            catch (Exception ex) { }
        }

        void LimpiarCampos()
        {
            txtnroserie.Value = string.Empty;
            txtnrodocumento.Value = string.Empty;
            txtasunto.Value = string.Empty;
            txtemaildestino.Value = string.Empty;
            txtmensaje.Value = string.Empty;
            cbotipodocumento.SelectedIndex = Constantes.ValorCero;
        }

        void CrearArchivoXML()
        {
            try
            {
                ObtenerUsuarioLogeado();//para el ruc de la empresa del usuario logeado
                string xmlGenerar = string.Empty;
                NombreArchivo = oGeneraFacturaElectronica.NombreArchivoXML;
                xmlGenerar = oGeneraFacturaElectronica.XML;

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

            }
            catch (Exception ex) { }
        }


        

        void CrearArchivoPDF()
        {
            try
            {
                ObtenerUsuarioLogeado();

                string xmlGenerar = string.Empty;
                NombreArchivo = oGeneraFacturaElectronica.NombreArchivoXML.TrimEnd();
                moneda = oGeneraFacturaElectronica.Moneda.Descripcion;
                xmlGenerar = oGeneraFacturaElectronica.XML;
                tpodocumento = oGeneraFacturaElectronica.TipoDocumento.CodigoDocumento;


                if (oGeneraFacturaElectronica.Campo1.Length < Constantes.ValorUno)
                {
                    OtrosArchivosForSend = "-";// archivos fisicos ejemplo orden de compra
                }
                else
                {
                    OtrosArchivosForSend = oGeneraFacturaElectronica.Campo1; // archivos fisicos ejemplo orden de compra
                }

                Singleton.Instance.CreateDirectory(Server.MapPath("~/DocumentoXML/"));

                //var xmldoc = new XmlDocument();
                //xmldoc.InnerXml = xmlGenerar;
                //xmldoc.Save(Server.MapPath("~/DocumentoXML/" + NombreArchivo + ".xml"));

                Response.Clear();

                switch (tpodocumento)
                {
                    case Constantes.Factura:
                    case Constantes.Boleta:
                        {
                            ObtenerValoresXMLCE(xmlGenerar); break;
                        }
                    case Constantes.NotaCredito: { ObtenerValoresXMLCENotCredit(xmlGenerar); break; }
                    case Constantes.NotaDebito: { ObtenerValoresXMLCENotDebit(xmlGenerar); break; }
                    case Constantes.Retencion: { ObtenerValoresXMLCRE(xmlGenerar); break; }
                    case Constantes.Percepcion: { break; }
                }

                tpodocumento = string.Empty;


                LlenarObjetoEmail();
                Response.Redirect("../Envio/UtilPDF");
                //Response.End();
            }
            catch (Exception ex) { }
        }

        #endregion


        #region SERIALIZAR XML

        public string moneda = string.Empty;
        public string tpodocumento = string.Empty;
        public string OtrosArchivosForSend = string.Empty;

        XmlDocument XMLArchive = new XmlDocument();
        public string NombreArchivo = String.Empty;
        FacturaElectronica oFactura = new FacturaElectronica();

        #region GET DATA FROM XML FACTURA, BOLETA

        void ObtenerValoresXMLCE(string xml_line)
        {            
            try
            {
                oFactura = new FacturaElectronica();
                XMLArchive.InnerXml = xml_line;

                var res = Singleton.Instance.GetDataXml_AllTypeDocument(xml_line, Constantes.Factura);
                var inv = (xmlFac.InvoiceType)(res);

                oFactura = Common.Util.UsefullClass.Instance.GetDataFromXMLCE(inv, XMLArchive, NombreArchivo);

                oFactura.Campo1 = OtrosArchivosForSend;
                //oFactura.TypeFormat = oGeneraFacturaElectronica.TypeFormat;
                oFactura.TypeFormat = 2;

                Session["EnvioFacturaPDF"] = oFactura;
                Session["EnvioFacturaDetallePDF"] = oFactura.ListaDetalleFacturaElectronica;
            }
            catch (Exception ex) { }
        }
        #endregion

        #region NOTA DE CREDITO
        void ObtenerValoresXMLCENotCredit(string xml_line)
        {
            try
            {
                oFactura = new FacturaElectronica();
                XMLArchive.InnerXml = xml_line;
                var res = Singleton.Instance.GetDataXml_AllTypeDocument(xml_line, Constantes.NotaCredito);
                var nc = (xmlNotCred.CreditNoteType)(res);

                oFactura = Common.Util.UsefullClass.Instance.GetDataFromCNOTECredit(nc, XMLArchive, NombreArchivo, tpodocumento);
                oFactura.Campo1 = OtrosArchivosForSend;
                //oFactura.TypeFormat = oGeneraFacturaElectronica.TypeFormat;

                oFactura.TypeFormat = 2;

                Session["EnvioFacturaPDF"] = oFactura;
                Session["EnvioFacturaDetallePDF"] = oFactura.ListaDetalleFacturaElectronica;
            }
            catch (Exception ex) { }
        }
        #endregion END NOTA DE CREDITO

        #region NOTA DE DEBITO
        void ObtenerValoresXMLCENotDebit(string xml_line)
        {
            try
            {
                oFactura = new FacturaElectronica();
                XMLArchive.InnerXml = xml_line;

                var res = Singleton.Instance.GetDataXml_AllTypeDocument(xml_line, Constantes.NotaDebito);
                var nd = (xmlNotDeb.DebitNoteType)(res);

                oFactura = Common.Util.UsefullClass.Instance.GetDataFromCNOTEDebit(nd, XMLArchive, NombreArchivo, tpodocumento);
                oFactura.Campo1 = OtrosArchivosForSend;
                //oFactura.TypeFormat = oGeneraFacturaElectronica.TypeFormat;

                oFactura.TypeFormat = 2;

                Session["EnvioFacturaPDF"] = oFactura;
                Session["EnvioFacturaDetallePDF"] = oFactura.ListaDetalleFacturaElectronica;
            }
            catch (Exception ex)
            {
                
            }
        }
        #endregion END NOTA DE DEBITO

        #region GET DATA FROM XML CRE

        void ObtenerValoresXMLCRE(string xml_line)
        {
            try
            {
                oFactura = new FacturaElectronica();
                XMLArchive.InnerXml = xml_line;
                var res = Singleton.Instance.GetDataXml_AllTypeDocument(xml_line, Constantes.Retencion);
                var ret = (xmlCre.RetentionType)(res);
                oFactura = Common.Util.UsefullClass.Instance.GetDataFromXMLCRE(ret, XMLArchive, NombreArchivo, tpodocumento);
                oFactura.Campo1 = OtrosArchivosForSend;
                //oFactura.TypeFormat = oGeneraFacturaElectronica.TypeFormat;

                oFactura.TypeFormat = 2;

                Session["EnvioFacturaPDF"] = oFactura;
                Session["EnvioFacturaDetallePDF"] = oFactura.ListaDocCRECPE;
            }
            catch (Exception ex) { }
        }

        #endregion

        #endregion


        #region ENVIO DE DOCUMENTO


        public WCFSeguridad.Usuario usuariologeado = new WCFSeguridad.Usuario();
        string cad1, cad2, cad3, cad4;
        void ObtenerDatosxDefecto()
        {
            try
            {
                oListaTipoDocumento = (ListaTipoDocumento)Session["AllListaTipoDocumento"];

                for (int i = 0; i <= oListaTipoDocumento.Count - 1; i++)
                {
                    if (cbotipodocumento.Value == oListaTipoDocumento[i].IdTipoDocumento.ToString())
                    {
                        cad1 = oListaTipoDocumento[i].Descripcion.ToUpper() + " ELECTRÓNICA";
                        break;
                    }
                }
                cad2 = " - " + oFactura.SerieCorrelativo + " - ";
                cad3 = usuariologeado.Empresa.RazonSocial.ToString();
                cad4 = cad1 + cad2 + cad3;
            }
            catch (Exception ex) { }
        }

        void LlenarObjetoEmail()
        {
            try
            {
                usuariologeado = (WCFSeguridad.Usuario)Session["UsuarioLogueadoFact"];
                referente = usuariologeado.Empresa.RazonSocial;
                destino = txtemaildestino.Value;
                if (txtasunto.Value.Length == 0)
                {
                    ObtenerDatosxDefecto();
                    asunto = cad4;
                }
                else
                {
                    asunto = txtasunto.Value;
                }

                cuerpo = txtmensaje.Value.Length == 0 ? string.Empty : txtmensaje.Value;

                Documento oDoEmail = new Documento();
                oDoEmail.Empresa = new Empresa();
                oDoEmail.Destino = destino;
                oDoEmail.Asunto = asunto;
                oDoEmail.Mensaje = cuerpo;
                oDoEmail.NombreArchivo = NombreArchivo.TrimEnd();
                oDoEmail.Referente = usuariologeado.Empresa.RazonSocial;
                oDoEmail.Empresa.RUC = usuariologeado.Empresa.RUC;

                Session["DatosEmail"] = oDoEmail;
                LlenarDocumentoEnvioInsert();
            }
            catch (Exception ex) { }
        }

        void LlenarDocumentoEnvioInsert()
        {
            try
            {
                oDocumentoEnvio = new Documento();
                oDocumentoEnvio.Empresa = new Empresa();
                oDocumentoEnvio.TipoDocumento = new TipoDocumento();

                string fechaenvio = string.Empty;

                oDocumentoEnvio.TipoDocumento.IdTipoDocumento = Convert.ToInt32(cbotipodocumento.Value);
                oDocumentoEnvio.Serie = txtnroserie.Value.Length == Constantes.ValorCero ? string.Empty : txtnroserie.Value.ToUpper();
                oDocumentoEnvio.NumeroDocumento = string.Format("{0:00000000}", int.Parse(txtnrodocumento.Value));

                oDocumentoEnvio.Destino = txtemaildestino.Value.Length == Constantes.ValorCero ? string.Empty : txtemaildestino.Value;
                oDocumentoEnvio.Asunto = txtasunto.Value.Length == Constantes.ValorCero ? asunto : txtasunto.Value;
                oDocumentoEnvio.Mensaje = txtmensaje.Value.Length == Constantes.ValorCero ? string.Empty : txtmensaje.Value;
                oDocumentoEnvio.Referente = usuario;
                oDocumentoEnvio.Remitente = oFactura.Empresa.RazonSocial;
                oDocumentoEnvio.Fecha_Cad = Convert.ToDateTime(DateTime.Now);
                oDocumentoEnvio.FechaEnvio = DateTime.Now.ToString();
                oDocumentoEnvio.Empresa.RUC = oUsuarioLogeado.Empresa.RUC;
                oDocumentoEnvio.TipoDocumento.CodigoDocumento = oFactura.TipoDocumento.CodigoDocumento;

                Session["ObjDocumentoEnviado"] = oDocumentoEnvio;
            }
            catch (Exception ex) { }
        }

        #endregion

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtemaildestino.Value.Contains("@"))
                {
                    GenerarArchivos();
                }
                else
                {
                    txtemaildestino.Focus();
                    Response.Write("<script language=javascript>alert('Debe Ingresar un Correo Electronico valido, caracter faltante @ ');</script>");
                    txtemaildestino.Focus();
                }
            }
            catch (Exception ex) { }
        }

        void Limpiar()
        {
            try
            {
                var file = Path.Combine(HttpContext.Current.Server.MapPath("../../DocumentoXML/" + NombreArchivo + ".xml"));
                if (System.IO.File.Exists(file))
                    System.IO.File.Delete(file);

                var filepdf = Path.Combine(HttpContext.Current.Server.MapPath("../../DocumentoXML/" + NombreArchivo + ".pdf"));
                if (System.IO.File.Exists(filepdf))
                    System.IO.File.Delete(filepdf);

                txtemaildestino.Value = string.Empty;
                txtmensaje.Value = string.Empty;
                txtasunto.Value = string.Empty;
                txtnroserie.Value = string.Empty;
                txtnrodocumento.Value = string.Empty;
                cbotipodocumento.Value = Constantes.ValorCero.ToString();
            }
            catch (Exception ex)
            {

            }
            
        }

        List<string> listadjunt = new List<string>();

        protected void btnAddFiles_Click(object sender, EventArgs e)
        {
        }

        protected void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            listadjunt = (List<string>)Session["listSendAdjFiles"];

            var itemSelect = lstFiles.SelectedIndex;
            lstFiles.Items.RemoveAt(itemSelect);

            var indexItemSelect = itemSelect;
            var nameSelected = listadjunt[indexItemSelect].ToString();
            listadjunt.Remove(nameSelected);

            if (System.IO.File.Exists(Server.MapPath("~/DocumentoXML/" + nameSelected)))
            {
                System.IO.File.Delete(Server.MapPath("~/DocumentoXML/" + nameSelected));
            }
            Session["listSendAdjFiles"] = listadjunt;
            lstFiles.Visible = lstFiles.Items.Count > Constantes.ValorCero ? true : false;

            btnDeleteSelected.Visible = lstFiles.Items.Count > Constantes.ValorCero ? true : false;
        }
    }
}