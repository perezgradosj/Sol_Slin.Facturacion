using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using Slin.Facturacion.BusinessEntities;
using System.Xml;
using System.IO;
using Slin.Facturacion.Common;


using xmlFac = Slin.Facturacion.Common.CE;
using xmlCre = Slin.Facturacion.Common.CRE;
using xmlNotCred = Slin.Facturacion.Common.CENC;
using xmlNotDeb = Slin.Facturacion.Common.CEND;
using System.Xml.Serialization;
using Slin.Facturacion.ServiceImplementation;

namespace Slin.Facturacion.WS_FileDown.Proccess
{
    public class RegistrProcess
    {

        #region entity

        private FacturaElectronica odoicReceived;
        public FacturaElectronica oDocReceived
        {
            get { return odoicReceived; }
            set
            {
                odoicReceived = value;
            }
        }


        private ListaDetalleFacturaElectronica objlistadetalle;
        public ListaDetalleFacturaElectronica objListDetalle
        {
            get { return objlistadetalle; }
            set
            {
                objlistadetalle = value;
            }
        }

        private DetalleFacturaElectronica objdetalle;
        public DetalleFacturaElectronica objDetalle
        {
            get { return objdetalle; }
            set
            {
                objdetalle = value;
            }
        }

        private DocCRECPE objdetallecrepre;
        public DocCRECPE objDetalleCRECPE
        {
            get { return objdetallecrepre; }
            set
            {
                objdetallecrepre = value;
            }
        }

        private ListaDocCRECPE objlistadoccrecpe;
        public ListaDocCRECPE objlistaDocCRECPE
        {
            get { return objlistadoccrecpe; }
            set
            {
                objlistadoccrecpe = value;
            }
        }

        string tipoDocumento = string.Empty;

        XmlDocument XMLArchive = new XmlDocument();


        string pathlog_Company = string.Empty;
        string pathfile_Company = string.Empty;
        string Path_MoveDocInserted_OK = string.Empty;
        string Path_MoveDoc_NotInserted = string.Empty;

        List<string> logprocess = new List<string>();


        #endregion

        #region method

        //public void Execute_ProcessRegistr(string xml_line, string pathxml, string ruccompany)
        public void Execute_ProcessRegistr(string pathxml, string ruccompany)
        {
            logprocess = new List<string>();

            pathlog_Company = string.Empty;
            pathlog_Company = ConfigurationManager.AppSettings[ruccompany].ToString();
            pathlog_Company = pathlog_Company + @"Logs\" + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\smd\";

            pathfile_Company = string.Empty;
            pathfile_Company = ConfigurationManager.AppSettings[ruccompany].ToString();
            pathfile_Company = pathfile_Company + @"ReceivedCE\";

            Path_MoveDocInserted_OK = string.Empty;
            Path_MoveDocInserted_OK = pathfile_Company + @"ins\";

            Path_MoveDoc_NotInserted = string.Empty;
            Path_MoveDoc_NotInserted = pathfile_Company + @"nins\";


            Inicializa();
            oDocReceived = new FacturaElectronica();
            XMLArchive = new XmlDocument();



            //Registr_XmlDocument(xml_line, pathxml);
            //iniciando el proceso para un documento xml - escribir log del procesamiento


            string[] array = @pathxml.Split('\\');
            int index = array.Length;

            string num_ce = array[index - 1].Replace(".xml", "");


            logprocess.Add("[" + DateTime.Now + "] ----------------------------------INICIO----------------------------------");
            logprocess.Add("[" + DateTime.Now + "] Documento en Proceso      : " + num_ce);


            var result = Serialize_XmlDocument(pathxml);

            if (result == false)
            {
                logprocess.Add("[" + DateTime.Now + "] Error al Leer el archivo  : " + pathxml);
            }
            else
            {
                logprocess.Add("[" + DateTime.Now + "] Se a leido correctamente  : " + pathxml);


                if (oDocReceived.Cliente.ClienteRuc == ruccompany)
                {
                    var listExists = new ServicioFacturacionSOA().GetListaIfExistsDocumentoCabecera(oDocReceived.NombreArchivoXML);

                    if (listExists.Count == Constantes.ValorCero)
                    {
                        #region ONE
                        Encoding encoding = Encoding.GetEncoding("ISO-8859-1");
                        byte[] XmlVarBinary = encoding.GetBytes(XMLArchive.InnerXml);

                        var result_ins = InsertDocumentReceived_BD(oDocReceived, XmlVarBinary, tipoDocumento);

                        if (result_ins == Constantes.msjRegistrado)
                        {
                            #region INSERTED OK
                            var file = Path.Combine(pathxml);
                            if (System.IO.File.Exists(file))
                            {
                                logprocess.Add("[" + DateTime.Now + "] Se a registrado en la BD  ");


                                if (!System.IO.File.Exists(Path_MoveDocInserted_OK + oDocReceived.NombreArchivoXML + ".xml"))
                                {
                                    System.IO.File.Move(pathxml, Path_MoveDocInserted_OK + oDocReceived.NombreArchivoXML + ".xml");
                                    logprocess.Add("[" + DateTime.Now + "] Se movio el archivo a     : " + Path_MoveDocInserted_OK + oDocReceived.NombreArchivoXML + ".xml");
                                }
                                else
                                {
                                    System.IO.File.Delete(Path_MoveDocInserted_OK + oDocReceived.NombreArchivoXML + ".xml");
                                    System.IO.File.Move(pathxml, Path_MoveDocInserted_OK + oDocReceived.NombreArchivoXML + ".xml");
                                    logprocess.Add("[" + DateTime.Now + "] Se movio el archivo a     : " + Path_MoveDocInserted_OK + oDocReceived.NombreArchivoXML + ".xml");
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            #region NOT INSERT
                            var file = Path.Combine(pathxml);
                            if (System.IO.File.Exists(file))
                            {
                                if (!System.IO.File.Exists(Path_MoveDoc_NotInserted + oDocReceived.NombreArchivoXML + ".xml"))
                                {
                                    System.IO.File.Move(pathxml, Path_MoveDoc_NotInserted + oDocReceived.NombreArchivoXML + ".xml");

                                    //using (StreamWriter sw = new StreamWriter(Path_List_ErrorLog + DateTime.Now.Year + "_" + DateTime.Now.Month + "_WS.log", true, Encoding.UTF8))
                                    //using (StreamWriter sw = new StreamWriter(pathlog_Company + "WS_Registr_log.log", true, Encoding.UTF8))
                                    //{
                                    //    //sw.WriteLine("[" + DateTime.Now + "] --------------------------------------------------------------------");
                                    //    //sw.WriteLine("[" + DateTime.Now + "] El Archivo se a movido a la ruta: " + Path_MoveDoc_NotInserted + oDocReceived.NombreArchivoXML + ".xml");
                                    //    //sw.WriteLine("[" + DateTime.Now + "] ---------------------------------------------------------------------");
                                    //}

                                    logprocess.Add("[" + DateTime.Now + "] Se movio el archivo a     : " + Path_MoveDoc_NotInserted + oDocReceived.NombreArchivoXML + ".xml");
                                }
                                else
                                {
                                    System.IO.File.Delete(Path_MoveDoc_NotInserted + oDocReceived.NombreArchivoXML + ".xml");
                                    System.IO.File.Move(pathxml, Path_MoveDoc_NotInserted + oDocReceived.NombreArchivoXML + ".xml");

                                    logprocess.Add("[" + DateTime.Now + "] Se movio el archivo a     : " + Path_MoveDoc_NotInserted + oDocReceived.NombreArchivoXML + ".xml");
                                }
                            }
                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        #region SI EL DOCUMENTO YA EXISTE EN LA BD

                        var file = Path.Combine(pathxml);
                        if (System.IO.File.Exists(file))
                        {
                            if (!System.IO.File.Exists(Path_MoveDoc_NotInserted + oDocReceived.NombreArchivoXML + ".xml"))
                            {
                                System.IO.File.Move(pathxml, Path_MoveDoc_NotInserted + oDocReceived.NombreArchivoXML + ".xml");
                            }
                            else
                            {
                                System.IO.File.Delete(Path_MoveDoc_NotInserted + oDocReceived.NombreArchivoXML + ".xml");
                                System.IO.File.Move(pathxml, Path_MoveDoc_NotInserted + oDocReceived.NombreArchivoXML + ".xml");
                            }

                            logprocess.Add("[" + DateTime.Now + "] El Doc. ya existe en la BD");
                            logprocess.Add("[" + DateTime.Now + "] Se movio el archivo a     : " + Path_MoveDoc_NotInserted + oDocReceived.NombreArchivoXML + ".xml");
                        }

                        #endregion
                    }
                }
                else
                {
                    if (System.IO.File.Exists(Path_MoveDoc_NotInserted + oDocReceived.NombreArchivoXML + ".xml"))
                        System.IO.File.Delete(Path_MoveDoc_NotInserted + oDocReceived.NombreArchivoXML + ".xml");

                    System.IO.File.Move(pathxml, Path_MoveDoc_NotInserted + oDocReceived.NombreArchivoXML + ".xml");

                    logprocess.Add("[" + DateTime.Now + "] Ruc de receptor invalido  : " + num_ce);
                    logprocess.Add("[" + DateTime.Now + "] El Doc. no se registro    : ");
                    logprocess.Add("[" + DateTime.Now + "] Se movio el archivo a     : " + Path_MoveDoc_NotInserted + oDocReceived.NombreArchivoXML + ".xml");
                }
                logprocess.Add("[" + DateTime.Now + "] -----------------------------------FIN------------------------------------");
            }


            if (logprocess.Count > 0)
            {
                foreach (var objlog in logprocess)
                {
                    using (StreamWriter sw = new StreamWriter(pathlog_Company + "Log_Proccess.log", true, Encoding.UTF8))
                    {
                        sw.WriteLine(objlog);
                    }
                }
            }

            //end proccess
        }

        public bool Serialize_XmlDocument(string pathxml)
        {
            try
            {
                #region
                XMLArchive = new XmlDocument();
                //var xmldoc = new XmlDocument();
                XMLArchive.Load(pathxml);

                XmlNodeList nodelist; // ALL

                nodelist = XMLArchive.GetElementsByTagName("cbc:InvoiceTypeCode");

                if (nodelist.Count > Constantes.ValorCero)
                {
                    #region SWITCH TYPE DOC OF SALES
                    switch (nodelist.Item(0).InnerText)
                    {
                        case Constantes.Factura:
                            {
                                tipoDocumento = Constantes.Factura;
                                GetDataFromXmlCE(pathxml, XMLArchive.InnerXml);
                                break;
                            }
                        case Constantes.Boleta:
                            {
                                //SERIALIZA CON InvoiceType
                                tipoDocumento = Constantes.Boleta;
                                GetDataFromXmlCE(pathxml, XMLArchive.InnerXml);
                                break;
                            }
                    }

                    #endregion END SWITCH TYPE DOC OF SALES
                }
                else
                {
                    #region DOC TYPE DOC RETETION OR PERCEPTION

                    if (XMLArchive.InnerXml.Contains("CreditNote"))
                    {
                        //SERIALIZA CON CreditNoteType
                        tipoDocumento = Constantes.NotaCredito;
                        GetDataFromXmlCENC(pathxml, XMLArchive.InnerXml);
                    }
                    else if (XMLArchive.InnerXml.Contains("DebitNote"))
                    {
                        // SERIALIZA CON DebitNoteType
                        tipoDocumento = Constantes.NotaDebito;
                        GetDataFromXmlCEND(pathxml, XMLArchive.InnerXml);
                    }
                    else if (XMLArchive.InnerXml.Contains("Retention"))
                    {

                        tipoDocumento = Constantes.Retencion;
                        GetDataFromXmlCRE(pathxml, XMLArchive.InnerXml);
                    }
                    else if (XMLArchive.InnerXml.Contains("Perception"))
                    {
                        tipoDocumento = Constantes.Percepcion;
                        GetDataFromXmlCRE(pathxml, XMLArchive.InnerXml);
                    }


                    #endregion END DOC TYPE DOC RETENTION OR PERCEPTION
                }

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                logprocess.Add("[" + DateTime.Now + "] Error                     : " + ex.Message);
                return false;
            }
        }


        private string InsertDocumentReceived_BD(FacturaElectronica Idocument, byte[] xmlVarBinary, string TypeDoc)
        {
            string msje = string.Empty;
            try
            {
                int Id_Head = new ServicioFacturacionSOA().InsertarDocumentoCabecera_Rec(Idocument, xmlVarBinary);


                switch (TypeDoc)
                {
                    case Constantes.Factura:
                    case Constantes.Boleta:
                    case Constantes.NotaCredito:
                    case Constantes.NotaDebito:
                        {
                            msje = new ServicioFacturacionSOA().InsertarDocumentoDetalle_Rec(objListDetalle, Id_Head);
                            break;
                        }
                    case Constantes.Retencion:
                        {
                            msje = new ServicioFacturacionSOA().InsertarDocumentoDetalle_CRE_CPE(objlistaDocCRECPE, Id_Head);
                            break;
                        }
                    case Constantes.Percepcion:
                        {
                            break;
                        }
                }



            }
            catch (Exception ex)
            {
                logprocess.Add("[" + DateTime.Now + "] Error al Registrar el Doc.: " + ex.Message);
                return ex.Message;
            }
            return msje;
        }






        #endregion

        #region seriealize

        void Inicializa()
        {

            oDocReceived = new FacturaElectronica();
            oDocReceived.Cliente = new Cliente();
            oDocReceived.Empresa = new Empresa();
            oDocReceived.TipoDocumento = new TipoDocumento();
            oDocReceived.ListaAfectado = new ListaAfectado();
            oDocReceived.ListaExtra = new ListaExtra();

            oDocReceived.Empresa.TipoDocumentiIdentidad = new TipoDocumentoIdentidad();
            oDocReceived.Cliente.TipoDocumentoIdentidad = new TipoDocumentoIdentidad();

            objListDetalle = new ListaDetalleFacturaElectronica();
            objlistaDocCRECPE = new ListaDocCRECPE();

            oDocReceived.DocCRECPE = new DocCRECPE();
        }


        #region FACT OR BOL CE
        private void GetDataFromXmlCE(string pathXML, string xml_line)
        {
            //var XMLArchive = new XmlDocument();
            //StreamReader sr;

            try
            {
                var inv = new xmlFac.InvoiceType();
                XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlFac.InvoiceType));
                using (TextReader reader = new StringReader(xml_line))
                {
                    inv = (xmlFac.InvoiceType)xmlSerial.Deserialize(reader);
                }



                //XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlCE.InvoiceType));
                ////sr = new StreamReader(pathXML + ".xml"); //ALL TYPE DOCUMENT
                //sr = new StreamReader(pathXML); //ALL TYPE DOCUMENT
                //xmlCE.InvoiceType inv = (xmlCE.InvoiceType)xmlSerial.Deserialize(sr);

                XmlNodeList xmlnodelist; //ALL

                Inicializa();

                //oDocReceived.NombreArchivoXML = NombreArchivo;
                oDocReceived.SerieCorrelativo = inv.ID.Value; //SERIE CORRELATIVO

                string[] array = oDocReceived.SerieCorrelativo.Split('-');
                oDocReceived.NumeroSerie = array[0];
                oDocReceived.NumeroDocumento = array[1];


                oDocReceived.FechaEmision2 = inv.IssueDate.Value.ToString("yyyy-MM-dd");
                oDocReceived.FechaEmision = inv.IssueDate.Value;

                oDocReceived.TipoDocumento.CodigoDocumento = inv.InvoiceTypeCode.Value; //TIPO CE

                oDocReceived.TipoMoneda = inv.DocumentCurrencyCode.Value; //TIPO MONEDA
                //oFactura.TipoMoneda = moneda;


                oDocReceived.Empresa.RUC = inv.AccountingSupplierParty.CustomerAssignedAccountID.Value; //RUC EMISOR

                oDocReceived.Empresa.TipoDocumentiIdentidad.Codigo = inv.AccountingSupplierParty.AdditionalAccountID.First().Value; //TPO DOC EMISOR

                oDocReceived.Empresa.RazonSocial = inv.AccountingSupplierParty.Party.PartyLegalEntity.ToList().First().RegistrationName.Value; // RAZON SOCIAL EMISOR
                oDocReceived.Empresa.RazonComercial = inv.AccountingSupplierParty.Party.PartyName.First().Name.Value; //RAZON COMERCIAL


                //oDocReceived.Empresa.RazonSocial = inv.AccountingSupplierParty.Party.PartyName.First().Name.Value; //RAZON SOCIAL EMISOR
                //oDocReceived.Empresa.RazonComercial = inv.AccountingSupplierParty.Party.PartyLegalEntity.First().RegistrationName.Value; //RAZON COMERCIAL EMISOR

                oDocReceived.Empresa.CodigoUbigeo = inv.AccountingSupplierParty.Party.PostalAddress.ID.Value; //UBIGEO EMISOR
                oDocReceived.Empresa.Direccion = inv.AccountingSupplierParty.Party.PostalAddress.StreetName.Value; //DIRECCION EMISOR


                if (inv.AccountingSupplierParty.Party.PostalAddress.CitySubdivisionName != null)
                {
                    oDocReceived.Empresa.Urbanizacion = inv.AccountingSupplierParty.Party.PostalAddress.CitySubdivisionName.Value == null ? string.Empty : inv.AccountingSupplierParty.Party.PostalAddress.CitySubdivisionName.Value;
                }
                else
                {
                    oDocReceived.Empresa.Urbanizacion = string.Empty;
                }



                oDocReceived.Empresa.Departamento = inv.AccountingSupplierParty.Party.PostalAddress.CityName.Value;
                oDocReceived.Empresa.Provincia = inv.AccountingSupplierParty.Party.PostalAddress.CountrySubentity.Value;
                oDocReceived.Empresa.Distrito = inv.AccountingSupplierParty.Party.PostalAddress.District.Value;
                oDocReceived.Empresa.CodPais = inv.AccountingSupplierParty.Party.PostalAddress.Country.IdentificationCode.Value;






                //oDocReceived.Cliente.ClienteRuc = ": " + inv.AccountingCustomerParty.CustomerAssignedAccountID.Value; //NRO DOC CLIENTE
                oDocReceived.Cliente.ClienteRuc = inv.AccountingCustomerParty.CustomerAssignedAccountID.Value; //NRO DOC CLIENTE
                oDocReceived.Cliente.TipoDocumentoIdentidad.Codigo = inv.AccountingCustomerParty.AdditionalAccountID.First().Value; // TPO DOC CLIENTE



                //CREAR EL NUM_CE DEL DOCUMENTO
                oDocReceived.NombreArchivoXML += oDocReceived.Empresa.RUC + Constantes.Guion;
                oDocReceived.NombreArchivoXML += oDocReceived.TipoDocumento.CodigoDocumento + Constantes.Guion;
                oDocReceived.NombreArchivoXML += oDocReceived.SerieCorrelativo;





                #region DIRECCION CLIENTE

                string depart = string.Empty, prov = string.Empty, distr = string.Empty, urb = string.Empty;

                if (inv.AccountingCustomerParty.Party != null)
                {
                    if (inv.AccountingCustomerParty.Party.PostalAddress != null)
                    {

                        #region
                        if (inv.AccountingCustomerParty.Party.PostalAddress.StreetName != null)
                        {
                            string dir = inv.AccountingCustomerParty.Party.PostalAddress.StreetName.Value;

                            if (dir == null)
                            {
                                //oDocReceived.Cliente.Direccion = ": ";
                                oDocReceived.Cliente.Direccion = string.Empty;
                            }
                            else if (dir == string.Empty)
                            {
                                //oDocReceived.Cliente.Direccion = ": ";
                                oDocReceived.Cliente.Direccion = string.Empty;
                            }
                            else
                            {
                                //oDocReceived.Cliente.Direccion = ": " + dir;
                                oDocReceived.Cliente.Direccion = dir;
                            }
                        }
                        else
                        {
                            oDocReceived.Cliente.Direccion = string.Empty;
                        }
                        #endregion



                        if (inv.AccountingCustomerParty.Party.PostalAddress.CitySubdivisionName != null)
                        {
                            oDocReceived.Cliente.Urbanizacion = inv.AccountingCustomerParty.Party.PostalAddress.CitySubdivisionName.Value == null ? string.Empty : inv.AccountingCustomerParty.Party.PostalAddress.CitySubdivisionName.Value;
                        }
                        else
                        {
                            oDocReceived.Cliente.Urbanizacion = string.Empty;
                        }

                        if (inv.AccountingCustomerParty.Party.PostalAddress.CityName != null)
                        {
                            oDocReceived.Cliente.Departamento = inv.AccountingCustomerParty.Party.PostalAddress.CityName.Value == null ? string.Empty : inv.AccountingCustomerParty.Party.PostalAddress.CityName.Value;
                        }
                        else
                        {
                            oDocReceived.Cliente.Departamento = string.Empty;
                        }

                        if (inv.AccountingCustomerParty.Party.PostalAddress.CountrySubentity != null)
                        {
                            oDocReceived.Cliente.Provincia = inv.AccountingCustomerParty.Party.PostalAddress.CountrySubentity.Value == null ? string.Empty : inv.AccountingCustomerParty.Party.PostalAddress.CountrySubentity.Value;
                        }
                        else
                        {
                            oDocReceived.Cliente.Provincia = string.Empty;
                        }

                        if (inv.AccountingCustomerParty.Party.PostalAddress.CountrySubentity != null)
                        {
                            oDocReceived.Cliente.Distrito = inv.AccountingCustomerParty.Party.PostalAddress.District.Value == null ? string.Empty : inv.AccountingCustomerParty.Party.PostalAddress.District.Value;
                        }
                        else
                        {
                            oDocReceived.Cliente.Distrito = string.Empty;
                        }

                        if (inv.AccountingCustomerParty.Party.PostalAddress.CountrySubentity != null)
                        {
                            oDocReceived.Cliente.CodPais = inv.AccountingCustomerParty.Party.PostalAddress.Country.IdentificationCode.Value == null ? string.Empty : inv.AccountingCustomerParty.Party.PostalAddress.Country.IdentificationCode.Value;
                        }
                        else
                        {
                            oDocReceived.Cliente.CodPais = string.Empty;
                        }
                    }
                    else
                    {
                        oDocReceived.Cliente.Direccion = string.Empty;
                        oDocReceived.Cliente.CodPais = string.Empty;
                        oDocReceived.Cliente.Distrito = string.Empty;
                        oDocReceived.Cliente.Provincia = string.Empty;
                        oDocReceived.Cliente.Departamento = string.Empty;
                        oDocReceived.Cliente.Urbanizacion = string.Empty;
                    }
                }
                else
                {
                    oDocReceived.Cliente.Direccion = string.Empty;
                    oDocReceived.Cliente.CodPais = string.Empty;
                    oDocReceived.Cliente.Distrito = string.Empty;
                    oDocReceived.Cliente.Provincia = string.Empty;
                    oDocReceived.Cliente.Departamento = string.Empty;
                    oDocReceived.Cliente.Urbanizacion = string.Empty;
                }


                #endregion END DIRECCION CLIENTE


                //oDocReceived.Cliente.RazonSocial = ": " + inv.AccountingCustomerParty.Party.PartyLegalEntity.First().RegistrationName.Value; //RAZON SOCIAL CLIENTE

                if (inv.AccountingCustomerParty.Party.PartyLegalEntity != null)
                {
                    if (inv.AccountingCustomerParty.Party.PartyLegalEntity.First().RegistrationName != null)
                    {
                        oDocReceived.Cliente.RazonSocial = inv.AccountingCustomerParty.Party.PartyLegalEntity.First().RegistrationName.Value; //RAZON SOCIAL CLIENTE
                    }
                }
                else
                {
                    oDocReceived.Cliente.RazonSocial = string.Empty;
                }




                #region ORDEN DE COMPRA
                string orderRef = string.Empty;
                if (inv.OrderReference == null)
                {
                    //orderRef = ": ";
                    orderRef = string.Empty;
                }
                else
                {
                    if (inv.OrderReference.ID == null)
                    {
                        //orderRef = ": ";
                        orderRef = string.Empty;
                    }
                    else
                    {
                        if (inv.OrderReference.ID.Value == null)
                        {
                            //orderRef = ": ";
                            orderRef = string.Empty;
                        }
                        else
                        {
                            orderRef = inv.OrderReference.ID.Value;
                        }
                    }

                    if (orderRef.Length == Constantes.ValorCero)
                    {
                        //orderRef = ": ";
                        orderRef = string.Empty;
                    }
                }
                oDocReceived.NroOrdCompra = orderRef;
                #endregion END ORDEN DE COMPRA

                if (inv.TaxTotal == null)
                {
                    oDocReceived.MontoIgvCad = string.Empty + 0.00m; //TOTAL IGV
                    oDocReceived.MontoTotalCad = string.Empty + 0.00m; //IMPORTE TOTAL
                    oDocReceived.MontoTotal = 0.00m;
                }
                else
                {
                    oDocReceived.MontoIgvCad = string.Empty + inv.TaxTotal.First().TaxAmount.Value; //TOTAL IGV
                    oDocReceived.MontoTotalCad = string.Empty + inv.LegalMonetaryTotal.PayableAmount.Value; //IMPORTE TOTAL
                    oDocReceived.MontoTotal = inv.LegalMonetaryTotal.PayableAmount.Value;
                }

                xmlnodelist = XMLArchive.GetElementsByTagName("sac:AdditionalMonetaryTotal"); //FOR SUB TOTAL AND OTHER TOTALES
                for (int i = 0; i <= xmlnodelist.Count - 1; i++)
                {
                    switch (xmlnodelist.Item(i).ChildNodes[0].InnerText)
                    {
                        #region CASE

                        case "1001":
                            {
                                oDocReceived.TotalGravado = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "1002":
                            {
                                oDocReceived.TotalInafecto = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "1003":
                            {
                                oDocReceived.TotalExonerado = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "1004":
                            {
                                oDocReceived.TotalGratuito = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "1005":
                            {
                                oDocReceived.SubTotalVenta = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "2001":
                            {
                                oDocReceived.TotalPercepciones = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "2002":
                            {
                                oDocReceived.TotalRetenciones = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "2003":
                            {
                                oDocReceived.TotalDetracciones = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "2004":
                            {
                                oDocReceived.TotalBonificaciones = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "2005":
                            {
                                oDocReceived.TotalDescuento = xmlnodelist.Item(i).ChildNodes[1].InnerText; ;
                                break;
                            }

                            #endregion
                    }
                }

                if (oDocReceived.TotalDescuento == null || oDocReceived.TotalDescuento.Length == Constantes.ValorCero)
                    oDocReceived.TotalDescuento = Constantes.ValorCeroMonto;


                xmlnodelist = XMLArchive.GetElementsByTagName("sac:AdditionalProperty"); // MONTO LITERAL
                for (int j = 0; j <= xmlnodelist.Count - 1; j++)
                {
                    switch (xmlnodelist.Item(j).ChildNodes[0].InnerText)
                    {
                        #region CASE

                        case "1000":
                            {
                                oDocReceived.MontoTotalLetras = xmlnodelist.Item(j).ChildNodes[1].InnerText;
                                break;
                            }

                            #endregion
                    }
                }


                xmlnodelist = XMLArchive.GetElementsByTagName("DigestValue"); //VALOR RESUMEN 
                oDocReceived.ValorResumen = xmlnodelist.Item(0).InnerText.Length == 0 ? string.Empty : xmlnodelist.Item(0).InnerText; ;

                xmlnodelist = XMLArchive.GetElementsByTagName("SignatureValue"); //VALOR FIRMA
                oDocReceived.ValorFirma = xmlnodelist.Item(0).InnerText.Length == 0 ? string.Empty : xmlnodelist.Item(0).InnerText;

                foreach (var line in inv.InvoiceLine)
                {
                    objDetalle = new DetalleFacturaElectronica();
                    objDetalle.NroOrden = int.Parse(line.ID.Value); //NRO DE ORDEN
                    objDetalle.Cantidad = line.InvoicedQuantity.Value; //CANTIDAD

                    objDetalle.ValorVenta = line.LineExtensionAmount.Value; //VALOR VENTA //total

                    objDetalle.PrecioVenta = line.PricingReference.AlternativeConditionPrice.First().PriceAmount.Value; //PRECIO VENTA

                    objDetalle.SubTotalTexto = string.Empty + (objdetalle.PrecioVenta * objdetalle.Cantidad); // SUB TOTAL X DETALLE


                    objDetalle.IGV = line.TaxTotal.First().TaxAmount.Value; // TOTAL IGV * DETALLE

                    objDetalle.ValorUnitario = line.Price.PriceAmount.Value; //VALOR UNITARIO
                    //objdetalle.ValorUnitario = line.PricingReference.AlternativeConditionPrice.First().PriceAmount.Value; //VALOR UNITARIO
                    //objdetalle.ValorUnitario = decimal.Round(objdetalle.ValorUnitario - (objdetalle.IGV / objdetalle.Cantidad), 2);

                    objDetalle.Descripcion = line.Item.Description.First().Value; //DESCRIPCION PRODUCTO
                    objDetalle.CodigoProducto = line.Item.SellersItemIdentification.ID.Value; //CODIGO PRODUCTO
                    objDetalle.Unidad = line.InvoicedQuantity.unitCode.ToString();// UNIDAD PRODUCTO

                    //NO APLICA EN ESTE CASO
                    //if (objDetalle.Unidad.Contains("NIU"))
                    //{
                    //    objDetalle.Unidad = "UND";
                    //}
                    //else
                    //{
                    //    objDetalle.Unidad = line.InvoicedQuantity.unitCode.ToString();
                    //}
                    //NO APLICA EN ESTE CASO

                    objDetalle.NombreArchivoXML = oDocReceived.NombreArchivoXML;

                    objDetalle.NumeroSerie = oDocReceived.NumeroSerie;
                    objDetalle.NumeroDocumento = oDocReceived.NumeroDocumento;

                    //objdetalle.Importe = decimal.Round(decimal.Parse(objdetalle.SubTotalTexto), 2);
                    //objdetalle.Importe = decimal.Round(objdetalle.ValorUnitario * objdetalle.Cantidad, 2); // SUB TOTAL X DETALLE
                    objDetalle.Importe = line.LineExtensionAmount.Value;

                    if (line.AllowanceCharge != null)
                    {
                        objDetalle.Dscto = line.AllowanceCharge.First().Amount.Value;
                    }
                    else
                    {
                        objDetalle.Dscto = 0.00m;
                    }

                    objDetalle.CodigoAfectoIGV = line.TaxTotal.First().TaxSubtotal.First().TaxCategory.TaxExemptionReasonCode.Value;

                    objListDetalle.Add(objDetalle);
                }

                //XmlNodeList xmlnodelist;
                xmlnodelist = XMLArchive.GetElementsByTagName("Extra");

                for (int i = 0; i <= xmlnodelist.Count - 1; i++)
                {
                    oDocReceived.Extra = new Extra();
                    oDocReceived.Extra.ExLinea = xmlnodelist.Item(i).ChildNodes[0].InnerText;
                    oDocReceived.Extra.ExDato = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                    oDocReceived.Extra.ExTipo = xmlnodelist.Item(i).ChildNodes[2].InnerText;
                    oDocReceived.ListaExtra.Add(oDocReceived.Extra);
                }

            }
            catch (Exception ex)
            {
                logprocess.Add("[" + DateTime.Now + "] Error al leer el XML msje : " + ex.Message);
            }

        }
        #endregion

        #region CREDIT NOTE
        void GetDataFromXmlCENC(string pathXML, string xml_line)
        {
            try
            {
                //XMLArchive.Load(pathXML);

                XmlNodeList xmlnodelist; //ALL

                Inicializa();


                oDocReceived.MontoTotalCad = "0.00";
                oDocReceived.MontoIgvCad = "0.00";
                oDocReceived.TotalDescuento = "0.00";
                oDocReceived.TotalExonerado = "0.00";
                oDocReceived.TotalInafecto = "0.00";
                oDocReceived.TotalGravado = "0.00";
                oDocReceived.TotalDetracciones = "0.00";
                oDocReceived.NroOrdCompra = ": ";

                //XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlCENC.CreditNoteType));
                //sr = new StreamReader(pathXML);
                //xmlCENC.CreditNoteType nc = (xmlCENC.CreditNoteType)xmlSerial.Deserialize(sr);


                var nc = new xmlNotCred.CreditNoteType();
                XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlNotCred.CreditNoteType));
                using (TextReader reader = new StringReader(xml_line))
                {
                    nc = (xmlNotCred.CreditNoteType)xmlSerial.Deserialize(reader);
                }










                //oDocReceived.NombreArchivoXML = NombreArchivo;
                oDocReceived.SerieCorrelativo = nc.ID.Value;

                string[] array = oDocReceived.SerieCorrelativo.Split('-');

                oDocReceived.NumeroSerie = array[0];
                oDocReceived.NumeroDocumento = array[1];

                oDocReceived.FechaEmision2 = nc.IssueDate.Value.ToString("yyyy-MM-dd");//FECHA DEL DOCUMENTO
                oDocReceived.FechaEmision = nc.IssueDate.Value;//FECHA EMISION

                //oFactura.TipoDocumento.CodigoDocumento = nc.DiscrepancyResponse.First().ResponseCode.Value;//TIPO DOCUMENTO
                //oDocReceived.TipoDocumento.CodigoDocumento = tpodocumento;//TIPO DOCUMENTO CHECKEAR
                oDocReceived.TipoDocumento.CodigoDocumento = Constantes.NotaCredito;

                oDocReceived.TipoMoneda = nc.DocumentCurrencyCode.Value;// TIPO MONEDA

                //switch (oDocReceived.TipoMoneda)  // NO APLICA PARA ESTE CASO
                //{
                //    case "USD":
                //        {
                //            oDocReceived.TipoMoneda = ": DÓLARES AMERICANOS";  // NO APLICA PARA ESTE CASO
                //            break;
                //        }
                //    case "PEN":
                //        {
                //            oDocReceived.TipoMoneda = ": SOLES";  // NO APLICA PARA ESTE CASO
                //            break;
                //        }
                //}

                //oFactura.Discr_ReferenceID = nc.DiscrepancyResponse.First().ReferenceID.Value;
                //oFactura.Discr_ResponseCode = nc.DiscrepancyResponse.First().ResponseCode.Value;
                //oFactura.Discr_Description = nc.DiscrepancyResponse.First().Description.ToList().First().Value;

                oDocReceived.Empresa.RUC = nc.AccountingSupplierParty.CustomerAssignedAccountID.Value;
                oDocReceived.Empresa.TipoDocumentiIdentidad.Codigo = nc.AccountingSupplierParty.AdditionalAccountID.First().Value;

                oDocReceived.Empresa.RazonSocial = nc.AccountingSupplierParty.Party.PartyLegalEntity.ToList().First().RegistrationName.Value; // RAZON SOCIAL EMISOR
                oDocReceived.Empresa.RazonComercial = nc.AccountingSupplierParty.Party.PartyName.First().Name.Value; //RAZON COMERCIAL

                //oDocReceived.Empresa.RazonSocial = nc.AccountingSupplierParty.Party.PartyName.First().Name.Value; //RAZON SOCIAL EMISOR
                //oDocReceived.Empresa.RazonComercial = nc.AccountingSupplierParty.Party.PartyLegalEntity.First().RegistrationName.Value;

                oDocReceived.Empresa.CodigoUbigeo = nc.AccountingSupplierParty.Party.PostalAddress.ID.Value;
                oDocReceived.Empresa.Direccion = nc.AccountingSupplierParty.Party.PostalAddress.StreetName.Value;

                oDocReceived.Empresa.Urbanizacion = nc.AccountingSupplierParty.Party.PostalAddress.CitySubdivisionName.Value == null ? string.Empty : nc.AccountingSupplierParty.Party.PostalAddress.CitySubdivisionName.Value;
                oDocReceived.Empresa.Departamento = nc.AccountingSupplierParty.Party.PostalAddress.CityName.Value;
                oDocReceived.Empresa.Provincia = nc.AccountingSupplierParty.Party.PostalAddress.CountrySubentity.Value;
                oDocReceived.Empresa.Distrito = nc.AccountingSupplierParty.Party.PostalAddress.District.Value;
                oDocReceived.Empresa.CodPais = nc.AccountingSupplierParty.Party.PostalAddress.Country.IdentificationCode.Value;


                //oDocReceived.Cliente.ClienteRuc = ": " + nc.AccountingCustomerParty.CustomerAssignedAccountID.Value; //NRO DOC CLIENTE
                oDocReceived.Cliente.ClienteRuc = nc.AccountingCustomerParty.CustomerAssignedAccountID.Value; //NRO DOC CLIENTE
                oDocReceived.Cliente.TipoDocumentoIdentidad.Codigo = nc.AccountingCustomerParty.AdditionalAccountID.First().Value;



                //CREAR EL NUM_CE DEL DOCUMENTO
                oDocReceived.NombreArchivoXML += oDocReceived.Empresa.RUC + Constantes.Guion;
                oDocReceived.NombreArchivoXML += oDocReceived.TipoDocumento.CodigoDocumento + Constantes.Guion;
                oDocReceived.NombreArchivoXML += oDocReceived.SerieCorrelativo;


                #region DIRECCION CLIENTE
                string dir = nc.AccountingCustomerParty.Party.PostalAddress.StreetName.Value;
                if (dir == null)
                {
                    //oDocReceived.Cliente.Direccion = ": "; 
                    oDocReceived.Cliente.Direccion = string.Empty;
                }
                else if (dir == string.Empty)
                {
                    //oDocReceived.Cliente.Direccion = ": ";
                    oDocReceived.Cliente.Direccion = string.Empty;
                }
                else
                {
                    //oDocReceived.Cliente.Direccion = ": " + dir;
                    oDocReceived.Cliente.Direccion = dir;
                }


                oDocReceived.Cliente.Urbanizacion = nc.AccountingCustomerParty.Party.PostalAddress.CitySubdivisionName.Value == null ? string.Empty : nc.AccountingCustomerParty.Party.PostalAddress.CitySubdivisionName.Value;
                oDocReceived.Cliente.Departamento = nc.AccountingCustomerParty.Party.PostalAddress.CityName.Value == null ? string.Empty : nc.AccountingCustomerParty.Party.PostalAddress.CityName.Value;
                oDocReceived.Cliente.Provincia = nc.AccountingCustomerParty.Party.PostalAddress.CountrySubentity.Value == null ? string.Empty : nc.AccountingCustomerParty.Party.PostalAddress.CountrySubentity.Value;
                oDocReceived.Cliente.Distrito = nc.AccountingCustomerParty.Party.PostalAddress.District.Value == null ? string.Empty : nc.AccountingCustomerParty.Party.PostalAddress.District.Value;
                oDocReceived.Cliente.CodPais = nc.AccountingCustomerParty.Party.PostalAddress.Country.IdentificationCode.Value == null ? string.Empty : nc.AccountingCustomerParty.Party.PostalAddress.Country.IdentificationCode.Value;


                #endregion

                //oDocReceived.Cliente.RazonSocial = ": " + nc.AccountingCustomerParty.Party.PartyLegalEntity.First().RegistrationName.Value; //RAZON SOCIAL CLIENTE
                oDocReceived.Cliente.RazonSocial = nc.AccountingCustomerParty.Party.PartyLegalEntity.First().RegistrationName.Value; //RAZON SOCIAL CLIENTE


                #region ORDEN DE COMPRA
                string orderRef = string.Empty;
                if (nc.OrderReference == null)
                {
                    //orderRef = ": ";
                    orderRef = string.Empty;
                }
                else
                {
                    if (nc.OrderReference.ID == null)
                    {
                        //orderRef = ": ";
                        orderRef = string.Empty;
                    }
                    else
                    {
                        if (nc.OrderReference.ID.Value == null)
                        {
                            //orderRef = ": ";
                            orderRef = string.Empty;
                        }
                        else
                        {
                            orderRef = nc.OrderReference.ID.Value;
                        }
                    }

                    if (orderRef.Length == Constantes.ValorCero)
                    {
                        //orderRef = ": ";
                        orderRef = string.Empty;
                    }
                }
                oDocReceived.NroOrdCompra = orderRef;
                #endregion END ORDEN DE COMPRA


                #region TOTALES
                xmlnodelist = XMLArchive.GetElementsByTagName("sac:AdditionalMonetaryTotal"); //FOR SUB TOTAL AND OTHER TOTALES
                for (int i = 0; i <= xmlnodelist.Count - 1; i++)
                {
                    switch (xmlnodelist.Item(i).ChildNodes[0].InnerText)
                    {
                        #region CASE

                        case "1001":
                            {
                                oDocReceived.TotalGravado = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                //oFactura.TotalGravadoSinIGV = decimal.Parse(xmlnodelist.Item(i).ChildNodes[1].InnerText);
                                break;
                            }
                        case "1002":
                            {
                                oDocReceived.TotalInafecto = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "1003":
                            {
                                oDocReceived.TotalExonerado = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "1004":
                            {
                                oDocReceived.TotalGratuito = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "1005":
                            {
                                oDocReceived.SubTotalVenta = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "2001":
                            {
                                oDocReceived.TotalPercepciones = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "2002":
                            {
                                oDocReceived.TotalRetenciones = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "2003":
                            {
                                oDocReceived.TotalDetracciones = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "2004":
                            {
                                oDocReceived.TotalBonificaciones = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "2005":
                            {
                                oDocReceived.TotalDescuento = xmlnodelist.Item(i).ChildNodes[1].InnerText; ;
                                break;
                            }

                            #endregion
                    }
                }
                #endregion

                #region OTHER TOTALES
                if (nc.TaxTotal == null)
                {
                    oDocReceived.MontoIgvCad = string.Empty + 0.00m; //TOTAL IGV
                    //oDocReceived.MontoTotalCad = string.Empty + 0.00m; //IMPORTE TOTAL

                    //oDocReceived.MontoTotal = 0.00m;
                }
                else
                {
                    oDocReceived.MontoIgvCad = string.Empty + nc.TaxTotal.First().TaxAmount.Value; //TOTAL IGV
                    //oDocReceived.MontoTotalCad = string.Empty + nc.LegalMonetaryTotal.PayableAmount.Value; //IMPORTE TOTAL

                    //oDocReceived.MontoTotal = nc.LegalMonetaryTotal.PayableAmount.Value;
                }


                if (nc.LegalMonetaryTotal != null)
                {
                    oDocReceived.MontoTotalCad = string.Empty + nc.LegalMonetaryTotal.PayableAmount.Value; //IMPORTE TOTAL
                    oDocReceived.MontoTotal = nc.LegalMonetaryTotal.PayableAmount.Value;
                }
                else
                {
                    oDocReceived.MontoTotalCad = string.Empty + 0.00m; //IMPORTE TOTAL
                    oDocReceived.MontoTotal = 0.00m;
                }


                if (nc.TaxTotal == null)
                {
                    oDocReceived.MontoIGV = 0.00m;
                    oDocReceived.MontoIGVText = 0.00m + string.Empty;
                    oDocReceived.MontoISC = 0.00m;
                    oDocReceived.MontoISCText = 0.00m + string.Empty;
                    oDocReceived.MontoOtros = 0.00m;
                    oDocReceived.MontoOtrosText = 0.00m + string.Empty;
                }
                else
                {
                    #region
                    for (int j = 0; j <= nc.TaxTotal.ToList().Count - 1; j++)
                    {
                        switch (nc.TaxTotal[j].TaxSubtotal.First().TaxCategory.TaxScheme.ID.Value)
                        {
                            case "1000":
                                {
                                    oDocReceived.MontoIGV = nc.TaxTotal[j].TaxSubtotal.First().TaxAmount.Value;
                                    oDocReceived.MontoIGV = nc.TaxTotal[j].TaxAmount.Value;

                                    oDocReceived.MontoIGVText = nc.TaxTotal[j].TaxSubtotal.First().TaxAmount.Value + string.Empty;
                                    oDocReceived.MontoIGVText = nc.TaxTotal[j].TaxAmount.Value + string.Empty;
                                    break;
                                }
                            case "2000":
                                {
                                    oDocReceived.MontoISC = nc.TaxTotal[j].TaxSubtotal.First().TaxAmount.Value;
                                    oDocReceived.MontoISC = nc.TaxTotal[j].TaxAmount.Value;

                                    oDocReceived.MontoISCText = nc.TaxTotal[j].TaxSubtotal.First().TaxAmount.Value + string.Empty;
                                    oDocReceived.MontoISCText = nc.TaxTotal[j].TaxAmount.Value + string.Empty;
                                    break;
                                }
                            case "9999":
                                {
                                    oDocReceived.MontoOtros = nc.TaxTotal[j].TaxSubtotal.First().TaxAmount.Value;
                                    oDocReceived.MontoOtros = nc.TaxTotal[j].TaxAmount.Value;

                                    oDocReceived.MontoOtrosText = nc.TaxTotal[j].TaxSubtotal.First().TaxAmount.Value + string.Empty;
                                    oDocReceived.MontoOtrosText = nc.TaxTotal[j].TaxAmount.Value + string.Empty;
                                    break;
                                }

                        }
                    }
                    #endregion
                }



                if (oDocReceived.TotalDescuento == null || oDocReceived.TotalDescuento.Length == Constantes.ValorCero)
                    oDocReceived.TotalDescuento = Constantes.ValorCeroMonto;

                #endregion END TOTALES

                xmlnodelist = XMLArchive.GetElementsByTagName("DigestValue"); //VALOR RESUMEN 
                oDocReceived.ValorResumen = xmlnodelist.Item(0).InnerText.Length == 0 ? string.Empty : xmlnodelist.Item(0).InnerText; ;

                xmlnodelist = XMLArchive.GetElementsByTagName("SignatureValue"); //VALOR FIRMA
                oDocReceived.ValorFirma = xmlnodelist.Item(0).InnerText.Length == 0 ? string.Empty : xmlnodelist.Item(0).InnerText;

                xmlnodelist = XMLArchive.GetElementsByTagName("sac:AdditionalProperty"); // MONTO LITERAL
                oDocReceived.MontoTotalLetras = xmlnodelist.Item(0).ChildNodes[1].InnerText;

                #region DETALLE
                foreach (var line in nc.CreditNoteLine)
                {
                    objDetalle = new DetalleFacturaElectronica();
                    objDetalle.NroOrden = int.Parse(line.ID.Value);

                    objDetalle.Unidad = line.CreditedQuantity.unitCode + string.Empty;
                    objDetalle.Cantidad = line.CreditedQuantity.Value;

                    objDetalle.SubTotal = line.LineExtensionAmount.Value; //SUB TOTAL SIN IGV

                    //bjDetalle.ValorVenta = line.LineExtensionAmount.Value; //VALOR VENTA //total

                    //objDetalle.ValorVenta = decimal.Parse(objDetalle.SubTotalTexto); //VALOR VENTA //total
                    objDetalle.ValorVenta = objDetalle.SubTotal; //VALOR VENTA //total

                    objDetalle.PrecioVenta = line.PricingReference.AlternativeConditionPrice.First().PriceAmount.Value; // PRECIO DE VENTA

                    objDetalle.SubTotalTexto = string.Empty + decimal.Round(objdetalle.PrecioVenta * objdetalle.Cantidad, 2); //SUB TOTAL + IGV 

                    objDetalle.DetMontoIGV = line.TaxTotal.First().TaxAmount.Value; // TOTAL IGV 

                    objDetalle.ValorUnitario = line.Price.PriceAmount.Value;

                    objDetalle.Descripcion = line.Item.Description.First().Value;
                    objDetalle.CodigoProducto = line.Item.SellersItemIdentification.ID.Value;

                    objDetalle.Importe = line.LineExtensionAmount.Value;



                    //if (objDetalle.Unidad.Contains("NIU")) //NO APLICA PARA ESTE CASO
                    //{
                    //    objDetalle.Unidad = "UND";  //NO APLICA PARA ESTE CASO
                    //}
                    //else
                    //{
                    //    objDetalle.Unidad = line.CreditedQuantity.unitCode.ToString();  //NO APLICA PARA ESTE CASO
                    //}

                    objDetalle.NombreArchivoXML = oDocReceived.NombreArchivoXML;

                    objDetalle.NumeroSerie = oDocReceived.NumeroSerie;
                    objDetalle.NumeroDocumento = oDocReceived.NumeroDocumento;

                    //oFactura.MontoTotal = oFactura.MontoTotal + objdetalle.Importe;

                    objDetalle.CodigoAfectoIGV = line.TaxTotal.First().TaxSubtotal.First().TaxCategory.TaxExemptionReasonCode.Value;

                    objListDetalle.Add(objDetalle);
                }

                #endregion END DETALLE

                #region DOCUMENTO AFECTADO
                oDocReceived.ListaAfectado = new ListaAfectado();
                foreach (var afec in nc.BillingReference)
                {
                    oDocReceived.Afectado = new Afectado();


                    oDocReceived.Afectado.ID = afec.InvoiceDocumentReference.ID.Value;
                    oDocReceived.Afectado.CodigoTipoDocumento = afec.InvoiceDocumentReference.DocumentTypeCode.Value;
                    oDocReceived.ListaAfectado.Add(oDocReceived.Afectado);
                }

                oDocReceived.MotivoAnulado = nc.DiscrepancyResponse.First().Description.First().Value;

                #endregion

                xmlnodelist = XMLArchive.GetElementsByTagName("Extra");

                for (int i = 0; i <= xmlnodelist.Count - 1; i++)
                {
                    oDocReceived.Extra = new Extra();
                    oDocReceived.Extra.ExLinea = xmlnodelist.Item(i).ChildNodes[0].InnerText;
                    oDocReceived.Extra.ExDato = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                    oDocReceived.Extra.ExTipo = xmlnodelist.Item(i).ChildNodes[2].InnerText;
                    oDocReceived.ListaExtra.Add(oDocReceived.Extra);
                }

                //Session["FacturaPDF"] = oFactura;
                //Session["FacturaDetallePDF"] = objlistadetalle;

                //sr.Close();
            }
            catch (Exception ex)
            {
                logprocess.Add("[" + DateTime.Now + "] Error al leer el XML msje : " + ex.Message);
            }
        }
        #endregion

        #region DEBIT NOTE
        private void GetDataFromXmlCEND(string pathXML, string xml_line)
        {
            try
            {
                XmlNodeList xmlnodelist; //ALL

                Inicializa();

                oDocReceived.MontoTotalCad = "0.00";
                oDocReceived.MontoIgvCad = "0.00";
                oDocReceived.TotalDescuento = "0.00";
                oDocReceived.TotalExonerado = "0.00";
                oDocReceived.TotalInafecto = "0.00";
                oDocReceived.TotalGravado = "0.00";
                oDocReceived.TotalDetracciones = "0.00";
                oDocReceived.NroOrdCompra = ": ";

                //XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlCEND.DebitNoteType));
                //sr = new StreamReader(pathXML);
                //xmlCEND.DebitNoteType nd = (xmlCEND.DebitNoteType)xmlSerial.Deserialize(sr);


                var nd = new xmlNotDeb.DebitNoteType();
                XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlNotDeb.DebitNoteType));
                using (TextReader reader = new StringReader(xml_line))
                {
                    nd = (xmlNotDeb.DebitNoteType)xmlSerial.Deserialize(reader);
                }


                //oDocReceived.NombreArchivoXML = NombreArchivo;
                oDocReceived.SerieCorrelativo = nd.ID.Value;

                string[] array = oDocReceived.SerieCorrelativo.Split('-');

                oDocReceived.NumeroSerie = array[0];
                oDocReceived.NumeroDocumento = array[1];

                oDocReceived.FechaEmision2 = nd.IssueDate.Value.ToString("yyyy-MM-dd");//FECHA DEL DOCUMENTO
                oDocReceived.FechaEmision = nd.IssueDate.Value;//FECHA EMISION

                //oFactura.TipoDocumento.CodigoDocumento = nc.DiscrepancyResponse.First().ResponseCode.Value;//TIPO DOCUMENTO
                //oDocReceived.TipoDocumento.CodigoDocumento = tpodocumento;//TIPO DOCUMENTO
                oDocReceived.TipoDocumento.CodigoDocumento = Constantes.NotaDebito;

                oDocReceived.TipoMoneda = nd.DocumentCurrencyCode.Value;// TIPO MONEDA

                //switch (oDocReceived.TipoMoneda)
                //{
                //    case "USD":
                //        {
                //            oDocReceived.TipoMoneda = ": DÓLARES AMERICANOS";
                //            break;
                //        }
                //    case "PEN":
                //        {
                //            oDocReceived.TipoMoneda = ": SOLES";
                //            break;
                //        }
                //}

                //oFactura.Discr_ReferenceID = nc.DiscrepancyResponse.First().ReferenceID.Value;
                //oFactura.Discr_ResponseCode = nc.DiscrepancyResponse.First().ResponseCode.Value;
                //oFactura.Discr_Description = nc.DiscrepancyResponse.First().Description.ToList().First().Value;

                oDocReceived.Empresa.RUC = nd.AccountingSupplierParty.CustomerAssignedAccountID.Value;
                oDocReceived.Empresa.TipoDocumentiIdentidad.Codigo = nd.AccountingSupplierParty.AdditionalAccountID.First().Value;

                oDocReceived.Empresa.RazonSocial = nd.AccountingSupplierParty.Party.PartyLegalEntity.ToList().First().RegistrationName.Value; // RAZON SOCIAL EMISOR
                oDocReceived.Empresa.RazonComercial = nd.AccountingSupplierParty.Party.PartyName.First().Name.Value; //RAZON COMERCIAL

                oDocReceived.Empresa.CodigoUbigeo = nd.AccountingSupplierParty.Party.PostalAddress.ID.Value;
                oDocReceived.Empresa.Direccion = nd.AccountingSupplierParty.Party.PostalAddress.StreetName.Value;

                oDocReceived.Empresa.Urbanizacion = nd.AccountingSupplierParty.Party.PostalAddress.CitySubdivisionName.Value == null ? string.Empty : nd.AccountingSupplierParty.Party.PostalAddress.CitySubdivisionName.Value;
                oDocReceived.Empresa.Departamento = nd.AccountingSupplierParty.Party.PostalAddress.CityName.Value;
                oDocReceived.Empresa.Provincia = nd.AccountingSupplierParty.Party.PostalAddress.CountrySubentity.Value;
                oDocReceived.Empresa.Distrito = nd.AccountingSupplierParty.Party.PostalAddress.District.Value;
                oDocReceived.Empresa.CodPais = nd.AccountingSupplierParty.Party.PostalAddress.Country.IdentificationCode.Value;


                //oDocReceived.Cliente.ClienteRuc = ": " + nd.AccountingCustomerParty.CustomerAssignedAccountID.Value; //NRO DOC CLIENTE
                oDocReceived.Cliente.ClienteRuc = nd.AccountingCustomerParty.CustomerAssignedAccountID.Value; //NRO DOC CLIENTE
                oDocReceived.Cliente.TipoDocumentoIdentidad.Codigo = nd.AccountingCustomerParty.AdditionalAccountID.First().Value;


                //CREAR EL NUM_CE DEL DOCUMENTO
                oDocReceived.NombreArchivoXML += oDocReceived.Empresa.RUC + Constantes.Guion;
                oDocReceived.NombreArchivoXML += oDocReceived.TipoDocumento.CodigoDocumento + Constantes.Guion;
                oDocReceived.NombreArchivoXML += oDocReceived.SerieCorrelativo;


                #region DIRECCION CLIENTE
                string dir = nd.AccountingCustomerParty.Party.PostalAddress.StreetName.Value;
                if (dir == null)
                {
                    //oDocReceived.Cliente.Direccion = ": ";
                    oDocReceived.Cliente.Direccion = string.Empty;
                }
                else if (dir == string.Empty)
                {
                    //oDocReceived.Cliente.Direccion = ": ";
                    oDocReceived.Cliente.Direccion = string.Empty;
                }
                else
                {
                    //oDocReceived.Cliente.Direccion = ": " + dir;
                    oDocReceived.Cliente.Direccion = dir;
                }





                #endregion


                oDocReceived.Cliente.Urbanizacion = nd.AccountingCustomerParty.Party.PostalAddress.CitySubdivisionName.Value == null ? string.Empty : nd.AccountingCustomerParty.Party.PostalAddress.CitySubdivisionName.Value;
                oDocReceived.Cliente.Departamento = nd.AccountingCustomerParty.Party.PostalAddress.CityName.Value == null ? string.Empty : nd.AccountingCustomerParty.Party.PostalAddress.CityName.Value;
                oDocReceived.Cliente.Provincia = nd.AccountingCustomerParty.Party.PostalAddress.CountrySubentity.Value == null ? string.Empty : nd.AccountingCustomerParty.Party.PostalAddress.CountrySubentity.Value;
                oDocReceived.Cliente.Distrito = nd.AccountingCustomerParty.Party.PostalAddress.District.Value == null ? string.Empty : nd.AccountingCustomerParty.Party.PostalAddress.District.Value;
                oDocReceived.Cliente.CodPais = nd.AccountingCustomerParty.Party.PostalAddress.Country.IdentificationCode.Value == null ? string.Empty : nd.AccountingCustomerParty.Party.PostalAddress.Country.IdentificationCode.Value;


                //oDocReceived.Cliente.RazonSocial = ": " + nd.AccountingCustomerParty.Party.PartyLegalEntity.First().RegistrationName.Value; //RAZON SOCIAL CLIENTE
                oDocReceived.Cliente.RazonSocial = nd.AccountingCustomerParty.Party.PartyLegalEntity.First().RegistrationName.Value; //RAZON SOCIAL CLIENTE


                #region ORDEN DE COMPRA
                string orderRef = string.Empty;
                if (nd.OrderReference == null)
                {
                    //orderRef = ": ";
                    orderRef = string.Empty;
                }
                else
                {
                    if (nd.OrderReference.ID == null)
                    {
                        //orderRef = ": ";
                        orderRef = string.Empty;
                    }
                    else
                    {
                        if (nd.OrderReference.ID.Value == null)
                        {
                            //orderRef = ": ";
                            orderRef = string.Empty;
                        }
                        else
                        {
                            orderRef = nd.OrderReference.ID.Value;
                        }
                    }

                    if (orderRef.Length == Constantes.ValorCero)
                    {
                        //orderRef = ": ";
                        orderRef = string.Empty;
                    }
                }
                oDocReceived.NroOrdCompra = orderRef;
                #endregion END ORDEN DE COMPRA


                #region TOTALES
                xmlnodelist = XMLArchive.GetElementsByTagName("sac:AdditionalMonetaryTotal"); //FOR SUB TOTAL AND OTHER TOTALES
                for (int i = 0; i <= xmlnodelist.Count - 1; i++)
                {
                    switch (xmlnodelist.Item(i).ChildNodes[0].InnerText)
                    {
                        #region CASE

                        case "1001":
                            {
                                oDocReceived.TotalGravado = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                //oFactura.TotalGravadoSinIGV = decimal.Parse(xmlnodelist.Item(i).ChildNodes[1].InnerText);
                                break;
                            }
                        case "1002":
                            {
                                oDocReceived.TotalInafecto = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "1003":
                            {
                                oDocReceived.TotalExonerado = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "1004":
                            {
                                oDocReceived.TotalGratuito = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "1005":
                            {
                                oDocReceived.SubTotalVenta = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "2001":
                            {
                                oDocReceived.TotalPercepciones = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "2002":
                            {
                                oDocReceived.TotalRetenciones = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "2003":
                            {
                                oDocReceived.TotalDetracciones = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "2004":
                            {
                                oDocReceived.TotalBonificaciones = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                                break;
                            }
                        case "2005":
                            {
                                oDocReceived.TotalDescuento = xmlnodelist.Item(i).ChildNodes[1].InnerText; ;
                                break;
                            }

                            #endregion
                    }
                }
                #endregion

                #region OTHER TOTALES

                //xmlnodelist = XMLArchive.GetElementsByTagName("");

                if (nd.TaxTotal == null)
                {
                    oDocReceived.MontoIgvCad = string.Empty + 0.00m; //TOTAL IGV
                }
                else
                {
                    oDocReceived.MontoIgvCad = string.Empty + nd.TaxTotal.First().TaxAmount.Value; //TOTAL IGV
                }

                if (nd.RequestedMonetaryTotal == null)
                {
                    oDocReceived.MontoTotalCad = string.Empty + 0.00m;//IMPORTE TOTAL
                    oDocReceived.MontoTotal = 0.00m;//IMPORTE TOTAL
                }
                else
                {
                    oDocReceived.MontoTotalCad = string.Empty + nd.RequestedMonetaryTotal.PayableAmount.Value;//IMPORTE TOTAL
                    oDocReceived.MontoTotal = nd.RequestedMonetaryTotal.PayableAmount.Value;//IMPORTE TOTAL
                }

                for (int j = 0; j <= nd.TaxTotal.ToList().Count - 1; j++)
                {
                    switch (nd.TaxTotal[j].TaxSubtotal.First().TaxCategory.TaxScheme.ID.Value)
                    {
                        case "1000":
                            {
                                oDocReceived.MontoIGV = nd.TaxTotal[j].TaxSubtotal.First().TaxAmount.Value;
                                oDocReceived.MontoIGV = nd.TaxTotal[j].TaxAmount.Value;

                                oDocReceived.MontoIGVText = nd.TaxTotal[j].TaxSubtotal.First().TaxAmount.Value + string.Empty;
                                oDocReceived.MontoIGVText = nd.TaxTotal[j].TaxAmount.Value + string.Empty;
                                break;
                            }
                        case "2000":
                            {
                                oDocReceived.MontoISC = nd.TaxTotal[j].TaxSubtotal.First().TaxAmount.Value;
                                oDocReceived.MontoISC = nd.TaxTotal[j].TaxAmount.Value;

                                oDocReceived.MontoISCText = nd.TaxTotal[j].TaxSubtotal.First().TaxAmount.Value + string.Empty;
                                oDocReceived.MontoISCText = nd.TaxTotal[j].TaxAmount.Value + string.Empty;
                                break;
                            }
                        case "9999":
                            {
                                oDocReceived.MontoOtros = nd.TaxTotal[j].TaxSubtotal.First().TaxAmount.Value;
                                oDocReceived.MontoOtros = nd.TaxTotal[j].TaxAmount.Value;

                                oDocReceived.MontoOtrosText = nd.TaxTotal[j].TaxSubtotal.First().TaxAmount.Value + string.Empty;
                                oDocReceived.MontoOtrosText = nd.TaxTotal[j].TaxAmount.Value + string.Empty;
                                break;
                            }

                    }
                }

                if (oDocReceived.TotalDescuento == null || oDocReceived.TotalDescuento.Length == Constantes.ValorCero)
                    oDocReceived.TotalDescuento = Constantes.ValorCeroMonto;

                #endregion END TOTALES

                xmlnodelist = XMLArchive.GetElementsByTagName("DigestValue"); //VALOR RESUMEN 
                oDocReceived.ValorResumen = xmlnodelist.Item(0).InnerText.Length == 0 ? string.Empty : xmlnodelist.Item(0).InnerText; ;

                xmlnodelist = XMLArchive.GetElementsByTagName("SignatureValue"); //VALOR FIRMA
                oDocReceived.ValorFirma = xmlnodelist.Item(0).InnerText.Length == 0 ? string.Empty : xmlnodelist.Item(0).InnerText;

                xmlnodelist = XMLArchive.GetElementsByTagName("sac:AdditionalProperty"); // MONTO LITERAL
                oDocReceived.MontoTotalLetras = xmlnodelist.Item(0).ChildNodes[1].InnerText;

                #region DETALLE
                foreach (var line in nd.DebitNoteLine)
                {
                    objDetalle = new DetalleFacturaElectronica();
                    objDetalle.NroOrden = int.Parse(line.ID.Value);

                    objDetalle.Unidad = line.DebitedQuantity.unitCode + string.Empty;
                    objDetalle.Cantidad = line.DebitedQuantity.Value;

                    objDetalle.SubTotal = line.LineExtensionAmount.Value; //SUB TOTAL SIN IGV

                    objDetalle.PrecioVenta = line.PricingReference.AlternativeConditionPrice.First().PriceAmount.Value; // PRECIO DE VENTA

                    objDetalle.SubTotalTexto = string.Empty + decimal.Round(objdetalle.PrecioVenta * objdetalle.Cantidad, 2); //SUB TOTAL + IGV 

                    //objDetalle.ValorVenta = line.LineExtensionAmount.Value; //VALOR VENTA //total
                    //objDetalle.ValorVenta = decimal.Parse(objDetalle.SubTotalTexto); //VALOR VENTA //total
                    objDetalle.ValorVenta = objDetalle.SubTotal; //VALOR VENTA //total

                    objDetalle.DetMontoIGV = line.TaxTotal.First().TaxAmount.Value; // TOTAL IGV 

                    objDetalle.ValorUnitario = line.Price.PriceAmount.Value;

                    objDetalle.Descripcion = line.Item.Description.First().Value;
                    objDetalle.CodigoProducto = line.Item.SellersItemIdentification.ID.Value;

                    objDetalle.Importe = line.LineExtensionAmount.Value;

                    //if (objDetalle.Unidad.Contains("NIU"))
                    //{
                    //    objDetalle.Unidad = "UND";
                    //}
                    //else
                    //{
                    //    objDetalle.Unidad = line.DebitedQuantity.unitCode.ToString();
                    //}

                    objDetalle.NombreArchivoXML = oDocReceived.NombreArchivoXML;

                    objDetalle.NumeroSerie = oDocReceived.NumeroSerie;
                    objDetalle.NumeroDocumento = oDocReceived.NumeroDocumento;

                    //oFactura.MontoTotal = oFactura.MontoTotal + objdetalle.Importe;

                    objDetalle.CodigoAfectoIGV = line.TaxTotal.First().TaxSubtotal.First().TaxCategory.TaxExemptionReasonCode.Value;

                    objListDetalle.Add(objDetalle);
                }

                #endregion END DETALLE


                #region DOCUMENTO AFECTADO
                oDocReceived.ListaAfectado = new ListaAfectado();
                foreach (var afec in nd.BillingReference)
                {
                    oDocReceived.Afectado = new Afectado();


                    oDocReceived.Afectado.ID = afec.InvoiceDocumentReference.ID.Value;
                    oDocReceived.Afectado.CodigoTipoDocumento = afec.InvoiceDocumentReference.DocumentTypeCode.Value;
                    oDocReceived.ListaAfectado.Add(oDocReceived.Afectado);
                }

                oDocReceived.MotivoAnulado = nd.DiscrepancyResponse.First().Description.First().Value;

                #endregion


                xmlnodelist = XMLArchive.GetElementsByTagName("Extra");

                for (int i = 0; i <= xmlnodelist.Count - 1; i++)
                {
                    oDocReceived.Extra = new Extra();
                    oDocReceived.Extra.ExLinea = xmlnodelist.Item(i).ChildNodes[0].InnerText;
                    oDocReceived.Extra.ExDato = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                    oDocReceived.Extra.ExTipo = xmlnodelist.Item(i).ChildNodes[2].InnerText;
                    oDocReceived.ListaExtra.Add(oDocReceived.Extra);
                }

                //Session["FacturaPDF"] = oFactura;
                //Session["FacturaDetallePDF"] = objlistadetalle;

                //sr.Close();
            }
            catch (Exception ex)
            {
                logprocess.Add("[" + DateTime.Now + "] Error al leer el XML msje : " + ex.Message);
            }
        }

        #endregion

        #region RETENCION CRE
        private void GetDataFromXmlCRE(string pathXML, string xml_line)
        {
            try
            {
                //XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlCRE.RetentionType));
                //sr = new StreamReader(pathXML);
                //xmlCRE.RetentionType ret = (xmlCRE.RetentionType)xmlSerial.Deserialize(sr);


                var ret = new xmlCre.RetentionType();
                XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlCre.RetentionType));
                using (TextReader reader = new StringReader(xml_line))
                {
                    ret = (xmlCre.RetentionType)xmlSerial.Deserialize(reader);
                }

                XmlNodeList xmlnodelist;

                Inicializa();

                //oDocReceived.NombreArchivoXML = NombreArchivo;
                oDocReceived.SerieCorrelativo = ret.ID.Value; // SERIE Y CORRELATIVO
                oDocReceived.FechaEmision2 = ret.IssueDate.Value.ToString("yyyy-MM-dd"); // FECHA DE DOCUMENTO
                oDocReceived.FechaEmision = ret.IssueDate.Value;

                //oDocReceived.TipoDocumento.CodigoDocumento = tpodocumento; //TIPO CE
                oDocReceived.TipoDocumento.CodigoDocumento = Constantes.Retencion; //TIPO CE

                string[] array = oDocReceived.SerieCorrelativo.Split('-');
                oDocReceived.NumeroSerie = array[0]; // SERIE 
                oDocReceived.NumeroDocumento = array[1]; // CORRELATIVO

                oDocReceived.Empresa.TipoDocumentiIdentidad.Codigo = ret.AgentParty.PartyIdentification.First().ID.schemeID; //TPO DOCUMENTO EMISOR
                oDocReceived.Empresa.RUC = ret.AgentParty.PartyIdentification.First().ID.Value; // NRO DOC EMISOR
                oDocReceived.Empresa.RazonComercial = ret.AgentParty.PartyName.First().Name.Value; // RAZON COMERCIAL EMISOR
                oDocReceived.Empresa.RazonSocial = ret.AgentParty.PartyLegalEntity.First().RegistrationName.Value; // RAZON SOCIAL EMISOR
                oDocReceived.Empresa.CodigoUbigeo = ret.AgentParty.PostalAddress.ID.Value; // COD UBIGEO EMISOR
                oDocReceived.Empresa.Direccion = ret.AgentParty.PostalAddress.StreetName.Value; // DIRECCION EMISOR


                oDocReceived.Empresa.Urbanizacion = ret.AgentParty.PostalAddress.CitySubdivisionName.Value == null ? string.Empty : ret.AgentParty.PostalAddress.CitySubdivisionName.Value;
                oDocReceived.Empresa.Departamento = ret.AgentParty.PostalAddress.CityName.Value;
                oDocReceived.Empresa.Provincia = ret.AgentParty.PostalAddress.CountrySubentity.Value;
                oDocReceived.Empresa.Distrito = ret.AgentParty.PostalAddress.District.Value;
                oDocReceived.Empresa.CodPais = ret.AgentParty.PostalAddress.Country.IdentificationCode.Value;


                oDocReceived.Cliente.TipoDocumentoIdentidad.Codigo = ret.ReceiverParty.PartyIdentification.First().ID.schemeID; // TPO DOC CLIENTE
                oDocReceived.Cliente.ClienteRuc = ret.ReceiverParty.PartyIdentification.First().ID.Value; // NRO DOC CLIENTE
                oDocReceived.Cliente.RazonSocial = ret.ReceiverParty.PartyLegalEntity.First().RegistrationName.Value; // RAZON SOCIAL CLIENTE
                oDocReceived.Cliente.Nombres = ret.ReceiverParty.PartyName.First().Name.Value; // RAZON COMERCIAL CLIENTE
                oDocReceived.Cliente.Direccion = ret.ReceiverParty.PostalAddress.StreetName.Value; // DIRECCION CLIENTE

                oDocReceived.Cliente.Departamento = ret.ReceiverParty.PostalAddress.CityName.Value;
                oDocReceived.Cliente.Provincia = ret.ReceiverParty.PostalAddress.CountrySubentity.Value;
                oDocReceived.Cliente.Distrito = ret.ReceiverParty.PostalAddress.District.Value;
                oDocReceived.Cliente.CodPais = ret.ReceiverParty.PostalAddress.Country.IdentificationCode.Value;


                //CREAR EL NUM_CE DEL DOCUMENTO
                oDocReceived.NombreArchivoXML += oDocReceived.Empresa.RUC + Constantes.Guion;
                oDocReceived.NombreArchivoXML += oDocReceived.TipoDocumento.CodigoDocumento + Constantes.Guion;
                oDocReceived.NombreArchivoXML += oDocReceived.SerieCorrelativo;


                //oDocReceived.Cliente.Direccion += ", " + oDocReceived.Cliente.Departamento + ", " + oDocReceived.Cliente.Provincia + ", " + oDocReceived.Cliente.Distrito;
                //oDocReceived.Cliente.Direccion += ", " + oDocReceived.Cliente.Departamento + ", " + oDocReceived.Cliente.Provincia + ", " + oDocReceived.Cliente.Distrito;

                oDocReceived.Nota = ret.Note.First().Value; // NOTAS DE LA RETENCION


                oDocReceived.DocCRECPE.MonedaImpTotalRetenido = string.Empty + ret.TotalInvoiceAmount.currencyID; //MONEDA IMPTOTRET
                oDocReceived.DocCRECPE.ImporteTotalRetenido = ret.TotalInvoiceAmount.Value; // IMP TOT RET

                oDocReceived.TipoMoneda = oDocReceived.DocCRECPE.MonedaImpTotalRetenido;
                oDocReceived.MontoTotal = oDocReceived.DocCRECPE.ImporteTotalRetenido;
                oDocReceived.TotalGravado = string.Empty + 0.00m;
                oDocReceived.MontoIgvCad = string.Empty + 0.00m;

                oDocReceived.DocCRECPE.MonedaImpTotalPagado = string.Empty + ret.SUNATTotalPaid.currencyID; // MON TOT PAG
                oDocReceived.DocCRECPE.ImporteTotalPagado = ret.SUNATTotalPaid.Value; // IMP TOT PAG



                oDocReceived.DocCRECPE.TasaRetencion = string.Empty + ret.SUNATRetentionPercent.Value;

                //oFactura.MontoTotalLetras = ret.TotalInvoiceAmount.Value + string.Empty;
                //oFactura.MontoTotalLetras = new ConvertorNum().enletras(oFactura.MontoTotalLetras);

                //oFactura.MontoTotalLetras = montoLiteral;

                xmlnodelist = XMLArchive.GetElementsByTagName("DigestValue"); //VALOR RESUMEN 
                oDocReceived.ValorResumen = xmlnodelist.Item(0).InnerText.Length == 0 ? string.Empty : xmlnodelist.Item(0).InnerText; ;

                xmlnodelist = XMLArchive.GetElementsByTagName("SignatureValue"); //VALOR FIRMA
                oDocReceived.ValorFirma = xmlnodelist.Item(0).InnerText.Length == 0 ? string.Empty : xmlnodelist.Item(0).InnerText;

                //decimal ImpNetPagSoles = 0.00m;

                decimal tipoCambio = 0.00m;
                string fechaCambio = "-";
                DateTime fechacambiodate = new DateTime();

                int cont = Constantes.ValorCero;
                foreach (var reten in ret.SUNATRetentionDocumentReference)
                {
                    cont++;
                    objDetalleCRECPE = new DocCRECPE();


                    objDetalleCRECPE.NombreArchivoXML = oDocReceived.NombreArchivoXML;
                    objDetalleCRECPE.NumeroSerie = oDocReceived.NumeroSerie;
                    objDetalleCRECPE.NumeroDocumento = oDocReceived.NumeroDocumento;
                    objDetalleCRECPE.NroOrden = cont;


                    objDetalleCRECPE.TipoDocRelac = reten.ID.schemeID; // TPO DOC RELAC
                    objDetalleCRECPE.NroDocRelac = reten.ID.Value;  // NRO DOC RELAC
                    //objDetalleCRECPE.FechaEmisionDocRelac = reten.IssueDate.Value.ToString("yyyy-MM-dd"); // FECHA EMI DOC RELAC
                    objDetalleCRECPE.FechaEmiDocRelac = reten.IssueDate.Value;

                    objDetalleCRECPE.ImporteTotDocRelac = reten.TotalInvoiceAmount.Value; // IMP TOT DOC RELAC
                    objDetalleCRECPE.MonedaImpTotDocRelac = string.Empty + reten.TotalInvoiceAmount.currencyID; // MON IMP TOT RELAC

                    //objDetalleCRECPE.FechaPago = string.Empty + reten.Payment.PaidDate.Value.ToString("yyyy-MM-dd"); // FECHA PAGO
                    objDetalleCRECPE.FechaPagoDate = reten.Payment.PaidDate.Value;

                    objDetalleCRECPE.NumeroPago = reten.Payment.ID.Value; // NUMERO DE PAGO
                    objDetalleCRECPE.ImportePagoSinReten = reten.Payment.PaidAmount.Value; // IMP PAGO SIN RETENCION
                    objDetalleCRECPE.MonedaPago = string.Empty + reten.Payment.PaidAmount.currencyID; // MONEDA PAGO

                    objDetalleCRECPE.ImporteRetenido = reten.SUNATRetentionInformation.SUNATRetentionAmount.Value; //IMP RETENIDO
                    objDetalleCRECPE.MonedaImpRetenido = string.Empty + reten.SUNATRetentionInformation.SUNATRetentionAmount.currencyID; // MONEDA IMP RET

                    //objDetalleCRECPE.FechaRetencion = ret.SUNATRetentionDocumentReference.First().SUNATRetentionInformation.SUNATRetentionDate.Value.ToString("yyyy-MM-dd"); // FECHA DE RETENCION
                    objDetalleCRECPE.FechaRetencionDate = ret.SUNATRetentionDocumentReference.First().SUNATRetentionInformation.SUNATRetentionDate.Value;


                    objDetalleCRECPE.ImporteTotxPagoNeto = reten.SUNATRetentionInformation.SUNATNetTotalPaid.Value; // IMP TOT A PAGAR(NETO)
                    objDetalleCRECPE.MonedaTotxPagoNeto = string.Empty + reten.SUNATRetentionInformation.SUNATNetTotalPaid.currencyID; // MONEDA TOT A PAGAR (NETO)

                    objDetalleCRECPE.MonedaRefTpoCambio = string.Empty;

                    objDetalleCRECPE.TasaRetencion = string.Empty;

                    //objDetalleCRECPE.MonedaRefTpoCambio = string.Empty + reten.SUNATRetentionInformation.ExchangeRate.SourceCurrencyCode.Value; // MON REF PARA TPO CAMBIO
                    //objDetalleCRECPE.MonObjetivoTasaCambio = string.Empty + reten.SUNATRetentionInformation.ExchangeRate.TargetCurrencyCode.Value; // MON OBJ TASA CAMBIO

                    //objDetalleCRECPE.TipoCambio = string.Empty + reten.SUNATRetentionInformation.ExchangeRate.CalculationRate.Value; // TIPO CAMBIO
                    //objDetalleCRECPE.FechaCambio = reten.SUNATRetentionInformation.ExchangeRate.Date.Value.ToString("yyyy-MM-dd"); // FECHA CAMBIO


                    //NO APLICA PARA ESTE CASO
                    //if (objDetalleCRECPE.MonedaImpTotDocRelac == Constantes.USD) 
                    //{
                    //    objDetalleCRECPE.Simbolo = Constantes.DOLAR;
                    //}
                    //else if (objDetalleCRECPE.MonedaImpTotDocRelac == Constantes.PEN)
                    //{
                    //    objDetalleCRECPE.Simbolo = Constantes.SOLES;
                    //}
                    //objDetalleCRECPE.SimboloSol = Constantes.SOLES;
                    //NO APLICA PARA ESTE CASO



                    if (reten.SUNATRetentionInformation.ExchangeRate == null)
                    {
                        fechacambiodate = Convert.ToDateTime("2000-01-01");
                    }
                    else
                    {
                        if (reten.SUNATRetentionInformation.ExchangeRate.CalculationRate.Value > tipoCambio)
                        {
                            tipoCambio = reten.SUNATRetentionInformation.ExchangeRate.CalculationRate.Value;
                            fechaCambio = reten.SUNATRetentionInformation.ExchangeRate.Date.Value.ToString("yyyy-MM-dd");
                            fechacambiodate = reten.SUNATRetentionInformation.ExchangeRate.Date.Value;
                        }

                        tipoCambio = reten.SUNATRetentionInformation.ExchangeRate.CalculationRate.Value;
                        fechaCambio = reten.SUNATRetentionInformation.ExchangeRate.Date.Value.ToString("yyyy-MM-dd");
                        fechacambiodate = reten.SUNATRetentionInformation.ExchangeRate.Date.Value;
                    }

                    //ImpNetPagSoles = ImpNetPagSoles + objDetalleCRECPE.ImportTotxPagoNeto;
                    objDetalleCRECPE.FechaTipoCambio = fechacambiodate;

                    objlistaDocCRECPE.Add(objDetalleCRECPE);
                }

                //oFactura.ImpPagGlobSoles = ImpNetPagSoles;

                oDocReceived.TipoCambio = tipoCambio + string.Empty;
                oDocReceived.FechaCambio = fechaCambio;

                xmlnodelist = XMLArchive.GetElementsByTagName("Extra");

                if (xmlnodelist == null)
                {
                    oDocReceived.ImpGlobSoles = oDocReceived.DocCRECPE.ImporteTotalRetenido + oDocReceived.DocCRECPE.ImporteTotalPagado;
                }
                else if (xmlnodelist.Count > 0)
                {
                    for (int i = 0; i <= xmlnodelist.Count - 1; i++)
                    {
                        oDocReceived.Extra = new Extra();
                        oDocReceived.Extra.ExLinea = xmlnodelist.Item(i).ChildNodes[0].InnerText;
                        oDocReceived.Extra.ExDato = xmlnodelist.Item(i).ChildNodes[1].InnerText;
                        oDocReceived.Extra.ExTipo = xmlnodelist.Item(i).ChildNodes[2].InnerText;
                        oDocReceived.ListaExtra.Add(oDocReceived.Extra);
                    }
                    //oFactura.ImpGlobSoles = decimal.Parse(oFactura.ListaExtra.First().ExDato);
                    oDocReceived.MontoTotalLetras = oDocReceived.ListaExtra[0].ExDato;
                    oDocReceived.ImpGlobSoles = decimal.Parse(oDocReceived.ListaExtra[1].ExDato);
                }
                else
                {
                    oDocReceived.ImpGlobSoles = oDocReceived.DocCRECPE.ImporteTotalRetenido + oDocReceived.DocCRECPE.ImporteTotalPagado;
                }

                //sr.Close();
            }
            catch (Exception ex)
            {
                logprocess.Add("[" + DateTime.Now + "] Error al leer el XML msje : " + ex.Message);
            }
        }

        #endregion

        #region PERCEPTION CPE

        private void GetDataFromXmlCPE(string pathXML)
        {

        }

        #endregion 

        #endregion

    }
}
