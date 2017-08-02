using Slin.Facturacion.BusinessEntities.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using Slin.Facturacion.BusinessEntities.InterfaceXMLCRE;

using Slin.Facturacion.ServiceImplementation;
using Slin.Facturacion.BusinessEntities;
using Slin.Facturacion.DataAccess.Helper;
using Slin.Facturacion.Common;

namespace Slin.Facturacion.DataInterfaceXml.CRE
{
    public class InterfaceXmlCRE
    {

        static string PathLogSLINADE = Conexion.Cadena; //log para escribir cuando el documento no pertenezca a ninguna empresa licenciada
        string PathInterfaceTxT = ConfigurationManager.AppSettings["PathInterfaceTxT"].ToString();

        static string PathLog_NotLicense = Conexion.Cadena_Nl;




        List<string> ListEntityDesencrypt = new List<string>();
        private void LlenarListEntity()
        {
            ListEntityDesencrypt = new List<string>();
            ListEntityDesencrypt = new Slin.Facturacion.BusinessSecurity.Entity.EntityClass().getsListEntity();
        }

        List<string> listaInterface = new List<string>();
        List<string> listaError = new List<string>();

        public string nombreArchivoXmlInterface = string.Empty;
        public string itemListaRegistro = string.Empty;

        string PathCompany = string.Empty;
        string Path_TologConsult = string.Empty;
        string Path_ToWriteInterfXml = string.Empty;
        public bool ResultOfSerialize = false;


        #region ENTITY

        Empresa ObjectEmpresa = new Empresa();

        #endregion

        private void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public string msj = string.Empty;

        public string msjCABPRINCIPAL = string.Empty;
        public string msjCABNOTA = string.Empty;
        public string msjCABEMISOR = string.Empty;
        public string msjCABRECEPTOR = string.Empty;
        public string msjDATOCE = string.Empty;
        public string msjDOCAFECTADO = string.Empty;
        public string msjTOTAL = string.Empty;
        public string msjITEM = string.Empty;
        public string msjLINEA = string.Empty;
        public string msjPERCEPCION = string.Empty;
        public string msjREFERENCIA = string.Empty;
        public string msjANTICIPO = string.Empty;
        public string msjEXTRAS = string.Empty;
        public string msjMAIL = string.Empty;
        public string msjADICIONAL = string.Empty;

        


        private void WriteLog_Service(string msje)
        {
            using (StreamWriter sw = new StreamWriter(Path_TologConsult, true, Encoding.UTF8))
            {
                sw.WriteLine(msje);
            }
        }

        private void WriteLog_NotLicense(string msje)
        {
            using (StreamWriter sw = new StreamWriter(PathLog_NotLicense + "ws_interf_nl.log", true, Encoding.UTF8))
            {
                sw.WriteLine(msje);
            }
        }

        private void WriteLog_Process(string msje)
        {
            using (StreamWriter sw = new StreamWriter(PathLogSLINADE + @"Fact_Interface.log", true, Encoding.UTF8))
            {
                sw.WriteLine(msje);
            }
        }

        public string GetObjInterfaceXML(string xmlLine)
        {
            listaInterface = new List<string>();
            listaError = new List<string>();
            nombreArchivoXmlInterface = string.Empty;
            itemListaRegistro = string.Empty;
            PathCompany = string.Empty;
            Path_TologConsult = string.Empty;
            Path_ToWriteInterfXml = string.Empty;
            ResultOfSerialize = false;

            LlenarListEntity();
            CreateDirectory(PathLogSLINADE);
            CreateDirectory(PathLog_NotLicense);

            try
            {
               


                string TPO_CE = GetObjHeaderXML(xmlLine); // serializa el xml recibido

                if (ResultOfSerialize == true)
                {
                    #region IniciaValidaciones de Correspondencia

                    if (!ListEntityDesencrypt.Contains(ObjectEmpresa.RUC))
                    {

                        #region SI EL DOCUMENTO RECIBIO NO ESTÁ HABILITADO PARA EL COSNUMO DE ESTA WEB SERVICE
                        WriteLog_NotLicense(" ");
                        WriteLog_NotLicense(Constantes.MsjStart);
                        WriteLog_NotLicense(Constantes.Msj_DocProcess + nombreArchivoXmlInterface);
                        WriteLog_NotLicense(Constantes.Msj_TpoDoc + TPO_CE);
                        WriteLog_NotLicense(Constantes.MsjRucInvalid + ObjectEmpresa.RUC);
                        WriteLog_NotLicense(Constantes.MsjEndProcessDoc + nombreArchivoXmlInterface);
                        WriteLog_NotLicense(Constantes.MsjEnd);

                        return msj = "El Documento: " + nombreArchivoXmlInterface + "| RUC de Emisor Invalido ";

                        #endregion
                    }
                    else
                    {
                        #region SI EL DOCUMENTO RECIBIDO ESTÁ HABILITADO PARA EL CONSUMO DE ESTA WEB SERVICE

                        PathCompany = ConfigurationManager.AppSettings[ObjectEmpresa.RUC].ToString();

                        Path_TologConsult = PathCompany + @"Logs\" + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\ser_interf\";
                        CreateDirectory(Path_TologConsult);
                        Path_TologConsult = PathCompany + @"Logs\" + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\ser_interf\ws_interf.log";
                        Path_ToWriteInterfXml = PathCompany + @"EntradaCE\InterfXml\";

                        WriteLog_Service(" ");
                        WriteLog_Service(Constantes.MsjStart);
                        WriteLog_Service(Constantes.Msj_DocProcess + nombreArchivoXmlInterface);
                        WriteLog_Service(Constantes.Msj_TpoDoc + TPO_CE);


                        listaError = ValidaInterfaceNew(listaInterface, TPO_CE); //SE VALIDA SI EL DOCUMENTO RECIBIDO TIENE LA ESTRUCTURA CORRECTA
                        if (listaError.Count > 0)  // SI EL DOCUMENTO CONTIENE ERRORES
                        {
                            #region EL DOCUMENTO INTERFACE CONTIENE ERRORES

                            msj += "El Documento: " + nombreArchivoXmlInterface + " | Contiene Los Siguientes Errores";
                            foreach (var line in listaError)
                            {
                                string sep = "|";
                                msj += sep + line;
                                sep = string.Empty;
                            }

                            WriteLog_Service(Constantes.Value_DateToLog + "El Doc. Contiene los sgtes errores.");
                            foreach(var error in listaError)
                            {
                                WriteLog_Service(Constantes.Value_DateToLog + error);
                            }
                            WriteLog_Service(Constantes.Value_DateToLog + "El Doc. no se a validado.");
                            WriteLog_Service(Constantes.MsjEndProcessDoc + nombreArchivoXmlInterface);
                            WriteLog_Service(Constantes.MsjEnd);

                            return msj;

                            #endregion
                        }
                        else
                        {
                            #region SI EL DOCUMENTO NO CONTIENE ERRORES

                            Documento existe = new Documento();
                            existe = new ServicioFacturacionSOA().ValidarExisteDoc_WS(nombreArchivoXmlInterface, TPO_CE); // SE VALIDA SI EL DOCUMENTO YA ESTÁ EN BD

                            if (existe.Serie != null && existe.Serie.Length > 0)
                            {
                                #region SI EL DOCUMENTO ESTA EN BD

                                WriteLog_Service(Constantes.Value_DateToLog + "El Doc. Ya esta registrado en la BD.");

                                WriteLog_Service(Constantes.MsjEnd);

                                return msj = "El Documento Nro: " + nombreArchivoXmlInterface + "| Ya se está Registrado en la BD.";

                                #endregion
                            }
                            else
                            {
                                #region SI EL DOCUMENTO NO ESTA EN BD
                                CreateDirectory(PathInterfaceTxT);

                                if (System.IO.File.Exists(PathInterfaceTxT + @nombreArchivoXmlInterface + ".txt"))
                                    System.IO.File.Delete(PathInterfaceTxT + @nombreArchivoXmlInterface + ".txt");

                                //using (StreamWriter sw = new StreamWriter(PathInterfaceTxT + @nombreArchivoXmlInterface + ".txt", true, Encoding.UTF8))
                                using (StreamWriter sw = new StreamWriter(PathInterfaceTxT + @nombreArchivoXmlInterface + ".txt", true, Encoding.GetEncoding("ISO-8859-1")))
                                {
                                    string salto = string.Empty;
                                    string cadenaInterface = string.Empty;
                                    foreach (var line in listaInterface)
                                    {
                                        cadenaInterface += salto + line;
                                        salto = "\r\n";
                                    }
                                    sw.Write(cadenaInterface);

                                    //foreach (var line in listaInterface)
                                    //{
                                    //    sw.WriteLine(line);
                                    //}
                                }

                                CreateDirectory(Path_ToWriteInterfXml);

                                var xmlDoc = new XmlDocument();
                                xmlDoc.InnerXml = xmlLine;
                                xmlDoc.Save(Path_ToWriteInterfXml + nombreArchivoXmlInterface + ".xml");// crea el xml validado correctamente
                                WriteLog_Service(Constantes.Value_DateToLog + "Se escribio archivo Txt : " + PathInterfaceTxT + nombreArchivoXmlInterface + ".txt");
                                WriteLog_Service(Constantes.Value_DateToLog + "Se guardo el Doc. Path  : " + Path_ToWriteInterfXml + nombreArchivoXmlInterface + ".xml");
                                WriteLog_Service(Constantes.Value_DateToLog + "El Doc. Se a validado Correctamente!");
                                WriteLog_Service(Constantes.MsjEndProcessDoc + nombreArchivoXmlInterface);
                                WriteLog_Service(Constantes.MsjEnd);

                                return msj = "El Documento Nro: " + nombreArchivoXmlInterface + "| Se valido Correctamente";

                                #endregion
                            }
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region OTHER ERROR
                    if (msjCABPRINCIPAL.Length > 2)
                    {
                        msj = msjCABPRINCIPAL;
                    }
                    else if (msjCABEMISOR.Length > 2)
                    {
                        msj = msjCABEMISOR;
                    }
                    else if (msjCABRECEPTOR.Length > 2)
                    {
                        msj = msjCABRECEPTOR;
                    }
                    else if (msjEXTRAS.Contains("Campos"))
                    {
                        msj = msjEXTRAS;
                    }
                    else if (TPO_CE == string.Empty && messageErrorwhenDeserialize.Length > 0)
                    {
                        msj = messageErrorwhenDeserialize;
                    }
                    #endregion
                    //return msj;
                }
            }
            catch (Exception ex)
            {
                CreateDirectory(PathLogSLINADE);
                WriteLog_Process(" ");
                WriteLog_Process(Constantes.MsjStart);
                WriteLog_Process(Constantes.Msj_DocProcess);
                WriteLog_Process(Constantes.Msj_Error + ex.Message);
                WriteLog_Process(Constantes.MsjEndProcessDoc + nombreArchivoXmlInterface);
                WriteLog_Process(Constantes.MsjEnd);
                messageErrorwhenDeserialize = "Error: " + ex.Message;
            }
            return msj;
        }



        string messageErrorwhenDeserialize = string.Empty;
        public string GetObjHeaderXML(string xml_line)
        {

            string TPO_CE = string.Empty;
            listaInterface = new List<string>();
            try
            {
                //XmlSerializer xmlSerial = new XmlSerializer(typeof(Retention));
                //sr = new StreamReader(filename);
                ////sr = new StreamReader(filename);
                //Retention inv = (Retention)xmlSerial.Deserialize(sr);


                var inv = new Retention();
                XmlSerializer xmlSerial = new XmlSerializer(typeof(Retention));
                using (TextReader reader = new StringReader(xml_line))
                {
                    inv = (Retention)xmlSerial.Deserialize(reader);
                }

                string guion = "-";
                string lineaCE = "CE|";

                #region CE - LIST

                if (inv.CE.ID != null)
                {
                    lineaCE += inv.CE.ID;
                }
                else
                {
                    lineaCE += inv.CE.ID;
                }

                #region CABECERA PRINCIPAL
                string lineaPrincipal = "CABECERA-PRINCIPAL|";
                if (inv.CabPrincipal != null)
                {
                    if (inv.CabPrincipal.TipoCE != null)
                    {
                        lineaPrincipal += inv.CabPrincipal.TipoCE + "|";
                    }
                    else
                    {
                        lineaPrincipal += guion + "|";
                    }

                    if (inv.CabPrincipal.ID_CE != null)
                    {
                        lineaPrincipal += inv.CabPrincipal.ID_CE + "|";
                    }
                    else
                    {
                        lineaPrincipal += guion + "|";
                    }

                    lineaPrincipal += "|";


                    if (inv.CabPrincipal.FEmision != null)
                    {
                        lineaPrincipal += inv.CabPrincipal.FEmision + "|";
                    }
                    else
                    {
                        lineaPrincipal += guion + "|";
                    }

                    if (inv.CabPrincipal.TMoneda != null)
                    {
                        lineaPrincipal += inv.CabPrincipal.TMoneda;
                    }
                    else
                    {
                        lineaPrincipal += guion;
                    }
                }
                else
                {
                    msjCABPRINCIPAL = "Existen Errores en el TAG CABECERA PRINCIPAL";
                }
                #endregion END CABECERA PRINCIPAL

                #region CABECERA NOTA
                string lineaNota = string.Empty;
                if (inv.CabNota != null)
                {
                    lineaNota = "CABECERA-NOTA|";
                    if (inv.CabNota.TpoNota != null)
                    {
                        lineaNota += inv.CabNota.TpoNota + "|";
                    }
                    else if (inv.CabNota.TpoNota == null)
                    {
                        msjCABNOTA = "Existen Campos necesarios en el TAG CABECERANOTA";
                    }
                    else
                    {
                        lineaNota += guion + "|";
                    }

                    if (inv.CabNota.MotivoNota != null)
                    {
                        lineaNota += inv.CabNota.MotivoNota;
                    }
                    else
                    {
                        lineaNota += guion;
                    }
                }
                else
                {
                    msjCABNOTA = string.Empty;
                }
                #endregion END CABECERA NOTA

                #region CABECERA EMISOR
                string lineaEmisor = "CABECERA-EMISOR|";
                if (inv.Emisor != null)
                {
                    if (inv.Emisor.EmiTpoDoc != null)
                    {
                        lineaEmisor += inv.Emisor.EmiTpoDoc + "|";
                    }
                    else
                    {
                        lineaEmisor += guion + "|";
                    }

                    if (inv.Emisor.EmiNumDocu != null) lineaEmisor += inv.Emisor.EmiNumDocu + "|"; else lineaEmisor += guion + "|";

                    if (inv.Emisor.EmiNombre != null) lineaEmisor += inv.Emisor.EmiNombre + "|"; else lineaEmisor += guion + "|";

                    if (inv.Emisor.EmiNComer != null) lineaEmisor += inv.Emisor.EmiNComer + "|"; else lineaEmisor += guion + "|";

                    if (inv.Emisor.EmiUbigeo != null) lineaEmisor += inv.Emisor.EmiUbigeo + "|"; else lineaEmisor += guion + "|";

                    if (inv.Emisor.EmiDirFiscal != null) lineaEmisor += inv.Emisor.EmiDirFiscal + "|"; else lineaEmisor += guion + "|";

                    if (inv.Emisor.EmiDirUrbani != null) lineaEmisor += inv.Emisor.EmiDirUrbani + "|"; else lineaEmisor += guion + "|";

                    if (inv.Emisor.EmiDirProvin != null) lineaEmisor += inv.Emisor.EmiDirProvin + "|"; else lineaEmisor += guion + "|";

                    if (inv.Emisor.EmiDirDepart != null) lineaEmisor += inv.Emisor.EmiDirDepart + "|"; else lineaEmisor += guion + "|";

                    if (inv.Emisor.EmiDirDistrito != null) lineaEmisor += inv.Emisor.EmiDirDistrito + "|"; else lineaEmisor += guion + "|";

                    if (inv.Emisor.EmiCodPais != null) lineaEmisor += inv.Emisor.EmiCodPais; else lineaEmisor += guion;
                }
                else
                {
                    msjCABEMISOR = "Existen Errores en el TAG CABECERAEMISOR";
                }

                #endregion END CABECERA EMISOR

                #region CABECERA RECEPTOR
                string lineaReceptor = "CABECERA-RECEPTOR|";
                if (inv.Receptor != null)
                {
                    if (inv.Receptor.RecTpoDoc != null)
                    {
                        lineaReceptor += inv.Receptor.RecTpoDoc + "|";
                    }
                    else
                    {
                        lineaReceptor += guion + "|";
                    }

                    if (inv.Receptor.RecNumDocu != null) lineaReceptor += inv.Receptor.RecNumDocu + "|"; else lineaReceptor += guion + "|";

                    if (inv.Receptor.RecNombre != null) lineaReceptor += inv.Receptor.RecNombre + "|"; else lineaReceptor += guion + "|";

                    if (inv.Receptor.RecNComer != null) lineaReceptor += inv.Receptor.RecNComer + "|"; else lineaReceptor += guion + "|";

                    if (inv.Receptor.RecUbigeo != null) lineaReceptor += inv.Receptor.RecUbigeo + "|"; else lineaReceptor += guion + "|";

                    if (inv.Receptor.RecDirFiscal != null) lineaReceptor += inv.Receptor.RecDirFiscal + "|"; else lineaReceptor += guion + "|";

                    if (inv.Receptor.RecDirUrbani != null) lineaReceptor += inv.Receptor.RecDirUrbani + "|"; else lineaReceptor += guion + "|";

                    if (inv.Receptor.RecDirProvin != null) lineaReceptor += inv.Receptor.RecDirProvin + "|"; else lineaReceptor += guion + "|";

                    if (inv.Receptor.RecDirDepart != null) lineaReceptor += inv.Receptor.RecDirDepart + "|"; else lineaReceptor += guion + "|";

                    if (inv.Receptor.RecDirDistrito != null) lineaReceptor += inv.Receptor.RecDirDistrito + "|"; else lineaReceptor += guion + "|";

                    if (inv.Receptor.RecCodPais != null) lineaReceptor += inv.Receptor.RecCodPais; else lineaReceptor += guion;
                }
                else
                {
                    msjCABRECEPTOR = msjCABRECEPTOR = "Existen Errores en el TAG CABECERA RECEPTOR";
                }

                #endregion END CABECERA RECEPTOR

                #region DATO CE
                string lineaDatoCE = string.Empty;
                if (inv.DatosCE != null)
                {
                    lineaDatoCE = "DATO-CE|";
                    if (inv.DatosCE.RegimenCE != null)
                    {
                        lineaDatoCE += inv.DatosCE.RegimenCE + "|";
                    }
                    else
                    {
                        lineaDatoCE += guion + "|";
                    }

                    if (inv.DatosCE.TasaCE != null) lineaDatoCE += inv.DatosCE.TasaCE + "|"; else lineaDatoCE += guion + "|";

                    if (inv.DatosCE.ObsCE != null) lineaDatoCE += inv.DatosCE.ObsCE + "|"; else lineaDatoCE += guion + "|";

                    if (inv.DatosCE.ImpTotCE != null) lineaDatoCE += inv.DatosCE.ImpTotCE + "|"; else lineaDatoCE += guion + "|";

                    if (inv.DatosCE.MonImpTotCE != null) lineaDatoCE += inv.DatosCE.MonImpTotCE + "|"; else lineaDatoCE += guion + "|";

                    if (inv.DatosCE.ImpTot != null) lineaDatoCE += inv.DatosCE.ImpTot + "|"; else lineaDatoCE += guion + "|";

                    if (inv.DatosCE.MonImpTot != null) lineaDatoCE += inv.DatosCE.MonImpTot; else lineaDatoCE += guion;
                }
                else
                {
                    msjDATOCE = "Existen Errores en el TAG DATO CE";
                }

                #endregion

                #region AFECTADO
                List<string> listaDocAfectado = new List<string>();
                if (inv.Afectados != null)
                {
                    if (inv.Afectados.ListAfectado.Count > 0)
                    {
                        if (inv.Afectados.ListAfectado[0].DocuAfeID != null && inv.Afectados.ListAfectado[0].TipoCE != null && inv.Afectados.ListAfectado[0].ID_CE != null)
                        {
                            foreach (var afec in inv.Afectados.ListAfectado)
                            {
                                listaDocAfectado.Add(afec.DocuAfeID + "|" + afec.TipoCE + "|" + afec.ID_CE);
                            }
                        }
                    }
                }
                else
                {
                    msjDOCAFECTADO += "No existe el TAG DOCUMENTO AFECTADO o es Incorrecto";
                }
                #endregion END AFECTADO

                #region TOTAL
                string lineaTOTAL = string.Empty;

                if (inv.Total != null)
                {
                    lineaTOTAL = "TOTAL|";
                    if (inv.Total.TotVtaGrab != null)
                    {
                        lineaTOTAL += inv.Total.TotVtaGrab + "|";
                    }
                    else if (inv.Total.TotVtaGrab == null)
                    {
                        msjTOTAL = "Existes Campos que son Necesarios en el TAG TOTAL";
                    }
                    else
                    {
                        lineaTOTAL += guion + "|";
                    }

                    if (inv.Total.TotVtaInaf != null) lineaTOTAL += inv.Total.TotVtaInaf + "|"; else lineaTOTAL += guion + "|";

                    if (inv.Total.TotVtaExon != null) lineaTOTAL += inv.Total.TotVtaExon + "|"; else lineaTOTAL += guion + "|";

                    if (inv.Total.TotVtaGrat != null) lineaTOTAL += inv.Total.TotVtaGrat + "|"; else lineaTOTAL += guion + "|";

                    if (inv.Total.TotVtaDscto != null) lineaTOTAL += inv.Total.TotVtaDscto + "|"; else lineaTOTAL += guion + "|";


                    if (inv.Total.TotSumIGV != null)
                    {
                        lineaTOTAL += inv.Total.TotSumIGV + "|";
                    }
                    else if (inv.Total.TotSumIGV == null)
                    {
                        msjTOTAL = "Existes Campos que son Necesarios en el TAG TOTAL";
                    }
                    else
                    {
                        lineaTOTAL += guion + "|";
                    }


                    if (inv.Total.TotSumISC != null) lineaTOTAL += inv.Total.TotSumISC + "|"; else lineaTOTAL += guion + "|";

                    if (inv.Total.TotSumOTrib != null) lineaTOTAL += inv.Total.TotSumOTrib + "|"; else lineaTOTAL += guion + "|";

                    if (inv.Total.TotDsctoGlobal != null) lineaTOTAL += inv.Total.TotDsctoGlobal + "|"; else lineaTOTAL += guion + "|";

                    if (inv.Total.TotSumOCargo != null) lineaTOTAL += inv.Total.TotSumOCargo + "|"; else lineaTOTAL += guion + "|";

                    if (inv.Total.TotAnticipo != null) lineaTOTAL += inv.Total.TotAnticipo + "|"; else lineaTOTAL += guion + "|";

                    if (inv.Total.TotImporTotal != null)
                    {
                        lineaTOTAL += inv.Total.TotImporTotal + "|";
                    }
                    else if (inv.Total.TotImporTotal == null)
                    {
                        msjTOTAL = "Existes Campos que son Necesarios en el TAG TOTAL";
                    }
                    else
                    {
                        lineaTOTAL += guion + "|";
                    }

                    if (inv.Total.MontoLiteral != null) lineaTOTAL += inv.Total.MontoLiteral; else lineaTOTAL += guion;
                }
                else
                {
                    msjTOTAL = "No existe el TAG TOTAL o es Incorrecto";
                }

                #endregion END TOTAL

                #region ITEMS

                List<string> listaItem = new List<string>();

                if (inv.Items != null)
                {

                    if (inv.Items.ListItems.Count > 0)
                    {
                        if (inv.Items.ListItems[0].TpoDocRelac != null && inv.Items.ListItems[0].NumDocRelac != null && inv.Items.ListItems[0].FEmisDocRelac != null && inv.Items.ListItems[0].ITotDocRelac != null && inv.Items.ListItems[0].MDocRelac != null &&
                        inv.Items.ListItems[0].FecMovi != null && inv.Items.ListItems[0].NumMovi != null && inv.Items.ListItems[0].ImpSOperMov != null && inv.Items.ListItems[0].MonMovi != null &&
                        inv.Items.ListItems[0].ImpOper != null && inv.Items.ListItems[0].MonImpOper != null && inv.Items.ListItems[0].FecOper != null && inv.Items.ListItems[0].ImpTotOper != null && inv.Items.ListItems[0].MonOper != null)
                        {
                            for (int i = 0; i <= inv.Items.ListItems.Count - 1; i++)
                            {
                                if (inv.Items.ListItems[i].MDocRelac == "PEN")
                                {
                                    listaItem.Add((i + 1) + "|" + inv.Items.ListItems[i].TpoDocRelac + "|" + inv.Items.ListItems[i].NumDocRelac + "|" + inv.Items.ListItems[i].FEmisDocRelac + "|" + inv.Items.ListItems[i].ITotDocRelac + "|" + inv.Items.ListItems[i].MDocRelac + "|" +
                                        inv.Items.ListItems[i].FecMovi + "|" + inv.Items.ListItems[i].NumMovi + "|" + inv.Items.ListItems[i].ImpSOperMov + "|" + inv.Items.ListItems[i].MonMovi + "|" +
                                        inv.Items.ListItems[i].ImpOper + "|" + inv.Items.ListItems[i].MonImpOper + "|" + inv.Items.ListItems[i].FecOper + "|" + inv.Items.ListItems[i].ImpTotOper + "|" + inv.Items.ListItems[i].MonOper);
                                }
                                else
                                {
                                    listaItem.Add((i + 1) + "|" + inv.Items.ListItems[i].TpoDocRelac + "|" + inv.Items.ListItems[i].NumDocRelac + "|" + inv.Items.ListItems[i].FEmisDocRelac + "|" + inv.Items.ListItems[i].ITotDocRelac + "|" + inv.Items.ListItems[i].MDocRelac + "|" +
                                        inv.Items.ListItems[i].FecMovi + "|" + inv.Items.ListItems[i].NumMovi + "|" + inv.Items.ListItems[i].ImpSOperMov + "|" + inv.Items.ListItems[i].MonMovi + "|" +
                                        inv.Items.ListItems[i].ImpOper + "|" + inv.Items.ListItems[i].MonImpOper + "|" + inv.Items.ListItems[i].FecOper + "|" + inv.Items.ListItems[i].ImpTotOper + "|" + inv.Items.ListItems[i].MonOper + "|" +
                                        inv.Items.ListItems[i].MonRefeTC + "|" + inv.Items.ListItems[i].MonDestTC + "|" + inv.Items.ListItems[i].FactorTC + "|" + inv.Items.ListItems[i].FechaTC);//(inv.Items.ListItems[i].FactorTC.Length > 0 ? inv.Items.ListItems[i].FactorTC : "0.00")
                                }
                            }
                        }
                        else
                        {
                            msjITEM += "Existen Campos Obligatorios que son Incorrectos en el TAG ITEM";
                        }
                    }

                }
                else
                {
                    msjITEM += "No existe el TAG ITEM o es Incorrecto";
                }

                #endregion END ITEMS

                #region LINEAS

                List<string> listaLinea = new List<string>();

                if (inv.Lineas != null)
                {
                    if (inv.Lineas.ListLinea.Count > 0)
                    {
                        if (inv.Lineas.ListLinea[0].LnUndMed != null && inv.Lineas.ListLinea[0].LnCantidad != null && inv.Lineas.ListLinea[0].LnCodProd != null && inv.Lineas.ListLinea[0].LnDescrip != null && inv.Lineas.ListLinea[0].LnValUnit != null &&
                        inv.Lineas.ListLinea[0].LnMntPrcVta != null && inv.Lineas.ListLinea[0].LnValVta != null && inv.Lineas.ListLinea[0].LnMntIGV != null && inv.Lineas.ListLinea[0].LnCodAfecIGV != null &&
                        inv.Lineas.ListLinea[0].LnMntISC != null && inv.Lineas.ListLinea[0].LnCodSisISC != null && inv.Lineas.ListLinea[0].LnDescMnto != null)
                        {
                            for (int i = 0; i <= inv.Lineas.ListLinea.Count - 1; i++)
                            {

                                listaLinea.Add((i + 1) + "|" + inv.Lineas.ListLinea[i].LnUndMed + "|" + inv.Lineas.ListLinea[i].LnCantidad + "|" + inv.Lineas.ListLinea[i].LnCodProd + "|" + inv.Lineas.ListLinea[i].LnDescrip + "|" + inv.Lineas.ListLinea[i].LnValUnit + "|" +
                                        inv.Lineas.ListLinea[i].LnMntPrcVta + "|" + inv.Lineas.ListLinea[i].LnValVta + "|" + inv.Lineas.ListLinea[i].LnMntIGV + "|" + inv.Lineas.ListLinea[i].LnCodAfecIGV + "|" +
                                        inv.Lineas.ListLinea[i].LnMntISC + "|" + inv.Lineas.ListLinea[i].LnCodSisISC + "|" + inv.Lineas.ListLinea[i].LnDescMnto);
                            }
                        }
                        else
                        {
                            msjLINEA += "Existen Campos Obligatorios que son Incorrectos en el TAG LINEA";
                        }
                    }
                }
                else
                {
                    msjLINEA += "No existe el TAG LINEA o es Incorrecto";
                }

                #endregion END LINEAS

                #region PERCEPCION

                string lineaPercepcion = string.Empty;
                if (inv.Percepcion != null)
                {
                    lineaPercepcion = "PERCEPCION|";
                    if (inv.Percepcion.PerBaseImp != null && inv.Percepcion.PerMntoPer != null && inv.Percepcion.PerMntoTot != null)
                    {
                        lineaPercepcion += inv.Percepcion.PerBaseImp + "|" + inv.Percepcion.PerMntoTot + "|" + inv.Percepcion.PerMntoTot;
                    }
                    else
                    {
                        msjPERCEPCION += "Hay Campos Incorrectos en el TAG PERCEPCION";
                    }
                }
                else
                {
                    msjPERCEPCION += "No existe el TAG PERCEPCION o es Incorrecto";
                }

                #endregion END PERCEPCION

                #region REFERENCIA

                List<string> listaReferencia = new List<string>();
                if (inv.Referencias != null)
                {
                    if (inv.Referencias.ListReferencia.Count > 0)
                    {
                        if (inv.Referencias.ListReferencia[0].RefID != null && inv.Referencias.ListReferencia[0].RefTpoDoc != null)
                        {
                            for (int i = 0; i <= inv.Referencias.ListReferencia.Count - 1; i++)
                            {
                                listaReferencia.Add((i + 1) + "|" + inv.Referencias.ListReferencia[i].RefID + "|" + inv.Referencias.ListReferencia[i].RefTpoDoc);
                            }
                        }
                        else
                        {
                            msjREFERENCIA += "Existen Campos Obligatorios que son Incorrectos en el TAG REFERENCIA";
                        }
                    }
                }
                else
                {
                    msjREFERENCIA += "No existe el TAG REFERENCIA o es Incorrecto";
                }

                #endregion END REFERENCIA

                #region ANTICIPO

                List<string> listaAnticipo = new List<string>();
                if (inv.Anticipos != null)
                {
                    if (inv.Anticipos.ListAnticipo.Count > 0)
                    {
                        if (inv.Anticipos.ListAnticipo[0].AntMonto != null && inv.Anticipos.ListAnticipo[0].AntTpoDocAnt != null && inv.Anticipos.ListAnticipo[0].AntIdDocAnt != null &&
                        inv.Anticipos.ListAnticipo[0].AntNumDocEmi != null && inv.Anticipos.ListAnticipo[0].AntTpoDocEmi != null)
                        {
                            for (int i = 0; i <= inv.Anticipos.ListAnticipo.Count - 1; i++)
                            {
                                listaAnticipo.Add((i + 1) + "|" + inv.Anticipos.ListAnticipo[i].AntMonto + "|" + inv.Anticipos.ListAnticipo[i].AntTpoDocAnt + "|" +
                                    inv.Anticipos.ListAnticipo[i].AntIdDocAnt + "|" + inv.Anticipos.ListAnticipo[i].AntNumDocEmi + "|" + inv.Anticipos.ListAnticipo[i].AntTpoDocEmi);
                            }
                        }
                        else
                        {
                            msjANTICIPO += "Existen Campos Obligatorios que son Incorrectos en el TAG ANTICIPO";
                        }
                    }
                }
                else
                {
                    msjANTICIPO += "No existe el TAG ANTICIPO o es Incorrecto";
                }
                #endregion END ANTICIPO

                #region EXTRAS

                List<string> listaExtras = new List<string>();
                if (inv.Extras != null)
                {
                    if (inv.Extras.ListExtra.Count > 0)
                    {
                        if (inv.Extras.ListExtra[0].ExLinea != null && inv.Extras.ListExtra[0].ExDato != null && inv.Extras.ListExtra[0].ExTpoDato != null)
                        {
                            for (int i = 0; i <= inv.Extras.ListExtra.Count - 1; i++)
                            {
                                listaExtras.Add(inv.Extras.ListExtra[i].ExLinea + "|" + inv.Extras.ListExtra[i].ExDato + "|" + inv.Extras.ListExtra[i].ExTpoDato);
                            }
                        }
                        else
                        {
                            msjEXTRAS += "Existen Campos Obligatorios que son Incorrectos en el TAG EXTRAS";
                        }
                    }
                }
                else
                {
                    msjEXTRAS += " No existe el TAG EXTRAS o es Incorrecto";
                }

                #endregion END EXTRAS

                #region MAIL

                string lineaCorreo = string.Empty;
                if (inv.Mail != null)
                {
                    lineaCorreo = "MAIL|";
                    if (inv.Mail.Para != null) lineaCorreo += inv.Mail.Para + "|"; else lineaCorreo += guion + "|";

                    if (inv.Mail.CC != null) lineaCorreo += inv.Mail.CC + "|"; else lineaCorreo += guion + "|";

                    if (inv.Mail.CCO != null) lineaCorreo += inv.Mail.CCO; else lineaCorreo += guion;

                    msjMAIL += "Existen Campos Obligatorios que son Incorrectos";
                }
                else
                {
                    msjMAIL += "No existe el TAG CORREO o es Incorrecto";
                }

                #endregion END MAIL

                #region ADICIONAL

                string lineaAdicional = string.Empty;
                if (inv.Adicional != null)
                {
                    lineaAdicional = "ADICIONAL|";
                    if (inv.Adicional.Sede != null) lineaAdicional += inv.Adicional.Sede + "|"; else lineaAdicional += guion + "|";
                    if (inv.Adicional.Usuario != null) lineaAdicional += inv.Adicional.Usuario + "|"; else lineaAdicional += guion + "|";
                    if (inv.Adicional.Impresora != null) lineaAdicional += inv.Adicional.Impresora + "|"; else lineaAdicional += guion + "|";
                    if (inv.Adicional.Campo1 != null) lineaAdicional += inv.Adicional.Campo1 + "|"; else lineaAdicional += guion + "|";
                    if (inv.Adicional.Campo2 != null) lineaAdicional += inv.Adicional.Campo2 + "|"; else lineaAdicional += guion + "|";
                    if (inv.Adicional.Campo3 != null) lineaAdicional += inv.Adicional.Campo3 + "|"; else lineaAdicional += guion + "|";
                    if (inv.Adicional.Campo4 != null) lineaAdicional += inv.Adicional.Campo4 + "|"; else lineaAdicional += guion + "|";
                    if (inv.Adicional.Campo5 != null) lineaAdicional += inv.Adicional.Campo5 + "|"; else lineaAdicional += guion + "|";
                    if (inv.Adicional.Campo6 != null) lineaAdicional += inv.Adicional.Campo6 + "|"; else lineaAdicional += guion + "|";
                    if (inv.Adicional.Campo7 != null) lineaAdicional += inv.Adicional.Campo7 + "|"; else lineaAdicional += guion + "|";
                    if (inv.Adicional.Campo8 != null) lineaAdicional += inv.Adicional.Campo8 + "|"; else lineaAdicional += guion + "|";
                    if (inv.Adicional.Campo9 != null) lineaAdicional += inv.Adicional.Campo9 + "|"; else lineaAdicional += guion + "|";
                    if (inv.Adicional.Campo10 != null) lineaAdicional += inv.Adicional.Campo10; else lineaAdicional += guion;
                }
                else
                {
                    msjADICIONAL += "No existe el TAG ADICIONAL o es Incorrecto";
                }

                #endregion END ADICIONAL

                #region LISTA INTERFACE
                listaInterface.Add(lineaCE);
                listaInterface.Add(lineaPrincipal);


                if (inv.CabPrincipal.TipoCE != null)
                {
                    #region CASE TIPO CE

                    switch (inv.CabPrincipal.TipoCE)
                    {
                        case "01":
                        case "03":
                            {
                                listaInterface.Add(lineaEmisor);
                                listaInterface.Add(lineaReceptor);
                                listaInterface.Add(lineaTOTAL);

                                foreach (var li in listaLinea)
                                {
                                    listaInterface.Add("LINEA|" + li);
                                }

                                if (lineaPercepcion.Length > 5 && inv.CabPrincipal.TipoCE == "01")
                                {
                                    listaInterface.Add(lineaPercepcion);
                                }

                                if (listaReferencia.Count > 0)
                                {
                                    foreach (var refe in listaReferencia)
                                    {
                                        listaInterface.Add("REFERENCIA|" + refe);
                                    }
                                }

                                if (listaAnticipo.Count > 0)
                                {
                                    foreach (var ant in listaAnticipo)
                                    {
                                        listaInterface.Add("ANTICIPO|" + ant);
                                    }
                                }

                                if (listaExtras.Count > 0)
                                {
                                    foreach (var ext in listaExtras)
                                    {
                                        listaInterface.Add("EXTRAS|" + ext);
                                    }
                                }
                                break;
                            }
                        case "07":
                        case "08":
                            {
                                listaInterface.Add(lineaNota);
                                listaInterface.Add(lineaEmisor);
                                listaInterface.Add(lineaReceptor);

                                if (listaDocAfectado.Count > 0)
                                {
                                    foreach (var afec in listaDocAfectado)
                                    {
                                        listaInterface.Add("DOCUMENTO-AFECTADO|" + afec);
                                    }
                                }

                                listaInterface.Add(lineaTOTAL);

                                if (listaLinea.Count > 0)
                                {
                                    foreach (var li in listaLinea)
                                    {
                                        listaInterface.Add("LINEA|" + li);
                                    }
                                }

                                if (listaReferencia.Count > 0)
                                {
                                    foreach (var refe in listaReferencia)
                                    {
                                        listaInterface.Add("REFERENCIA|" + refe);
                                    }
                                }

                                if (listaExtras.Count > 0)
                                {
                                    foreach (var ext in listaExtras)
                                    {
                                        listaInterface.Add("EXTRAS|" + ext);
                                    }
                                }
                                break;
                            }
                        case "20":
                        case "40":
                            {
                                listaInterface.Add(lineaEmisor);
                                listaInterface.Add(lineaReceptor);

                                if (lineaDatoCE.Length > 10)
                                {
                                    listaInterface.Add(lineaDatoCE);
                                }

                                if (listaItem.Count > 0)
                                {
                                    foreach (var item in listaItem)
                                    {
                                        listaInterface.Add("ITEM|" + item);
                                    }
                                }

                                if (listaExtras.Count > 0)
                                {
                                    foreach (var ex in listaExtras)
                                    {
                                        listaInterface.Add("EXTRAS|" + ex);
                                    }
                                }
                                break;
                            }
                    }

                    #endregion END CASE TIPO CE
                }

                if (lineaCorreo.Length > 10)
                {
                    listaInterface.Add(lineaCorreo);
                }

                if (lineaAdicional.Length > 10)
                {
                    listaInterface.Add(lineaAdicional);
                }

                #endregion

                ObjectEmpresa = new Empresa();

                if (inv.Emisor != null && inv.CE != null && inv.CabPrincipal != null)
                {
                    if (inv.CabPrincipal.TipoCE != null && inv.Emisor.EmiNumDocu != null && inv.CE.ID != null && inv.CabPrincipal.ID_CE != null)
                    {
                        ObjectEmpresa.RUC = inv.Emisor.EmiNumDocu;
                        //entityRUCId2 = inv.Emisor.EmiNumDocu;
                        //entityRUCId1 = inv.CE.ID.Substring(0, 11);

                        //IdDocumentoReceib = inv.CE.ID;
                        //IdDocumentoCreated = inv.Emisor.EmiNumDocu + "-" + inv.CabPrincipal.TipoCE + "-" + inv.CabPrincipal.ID_CE;


                        TPO_CE = inv.CabPrincipal.TipoCE;
                        nombreArchivoXmlInterface = inv.CE.ID;
                    }
                }

                #endregion

                itemListaRegistro = "[" + DateTime.Now + "] " + nombreArchivoXmlInterface + " " + "Recibido Correctamente.";

                ResultOfSerialize = true;
            }
            catch (Exception ex)
            {
                ResultOfSerialize = false;
                messageErrorwhenDeserialize = "Error: " + ex.Message;

                CreateDirectory(PathLogSLINADE);
                WriteLog_Process(Constantes.MsjStart);
                WriteLog_Process(Constantes.Msj_DocProcess + nombreArchivoXmlInterface);
                WriteLog_Process(Constantes.Msj_Error + ex.Message);
                WriteLog_Process(Constantes.MsjEndProcessDoc + nombreArchivoXmlInterface);
                WriteLog_Process(Constantes.MsjEnd);
            }
            return TPO_CE;
        }

        private Dictionary<string, string[]> fDict = new Dictionary<string, string[]>();
        public int it, li, re, an, da, ex, nroItems = 0;

        public string[] getSizedTmp(string[] tmp, List<ERegex> Validaciones, string td)
        {
            int nlines;
            nlines = Validaciones.Where(o => o.TAB == tmp[0] && o.DOC.Contains(td)).Count() + 1;
            List<string> mod = tmp.OfType<string>().ToList();
            if (mod.Count < nlines)
            {
                int a = nlines - mod.Count;
                for (int i = 0; i < a; ++i)
                {
                    mod.Add("");
                }
            }
            string[] res = mod.ToArray();
            return res;
        }


        public List<string> ValidaInterfaceNew(List<string> txt, string td)
        {
            // listaError

            List<string> listerror = new List<string>();

            listerror = new List<string>();

            List<ERegex> Validaciones = new List<ERegex>();
            Validaciones = new ServicioInterfaceSOA().getRegex();
            if (Validaciones.Count == 0)
            {
                //log.Validaciones("No se han podido obtener la validaciones de la base de datos");
                //return false;
            }
            bool valido = true;
            int i = 0, j = 0;



            #region ADD VALIDATION

            foreach (string line in txt)
            {
                string[] temx = line.Split('|');
                string[] temp = getSizedTmp(temx, Validaciones, td);
                if (temp[0].StartsWith("DOCUMENTO-AFECTADO") || temp[0].StartsWith("ITEM") || temp[0].StartsWith("LINEA") || temp[0].StartsWith("REFERENCIA") || temp[0].StartsWith("ANTICIPO") || temp[0].StartsWith("EXTRAS"))
                {
                    fDict.Add(temp[0] + temp[1], temp);
                    nroItems++;
                    if (temp[0].StartsWith("LINEA")) li++;
                    if (temp[0].StartsWith("ITEM")) it++;
                    if (temp[0].StartsWith("DOCUMENTO-AFECTADO")) it++;
                    if (temp[0].StartsWith("REFERENCIA")) re++;
                    if (temp[0].StartsWith("ANTICIPO")) an++;
                    if (temp[0].StartsWith("EXTRAS")) ex++;
                }
                else
                {
                    fDict.Add(temp[0], temp);
                    //if (temp[0] == "CABECERA-PRINCIPAL")
                    //{
                    //    if (temp[5] == "PEN") _curr = CurrencyCodeContentType.PEN;
                    //    if (temp[5] == "EUR") _curr = CurrencyCodeContentType.EUR;
                    //    if (temp[5] == "USD") _curr = CurrencyCodeContentType.USD;
                    //}
                }
            }

            /// Validacion momentanea, mejorar luego

            for (int s = 0; s < it; ++s)
            {
                if (fDict.ContainsKey("ITEM" + (s + 1)))
                {
                    if (fDict["ITEM" + (s + 1)][6] != "PEN")
                    {
                        if (fDict["ITEM" + (s + 1)][16] == "" || fDict["ITEM" + (s + 1)][17] == "" || fDict["ITEM" + (s + 1)][18] == "" || fDict["ITEM" + (s + 1)][19] == "")
                        {
                            listerror.Add("ITEM(" + (s + 1) + ")-Los datos acerca del Tipo de Cambio son obligatorios");
                        }
                    }
                }
            }


            #endregion

            for (j = 0; j < Validaciones.Count; ++j)
            {
                Validaciones[j]._POS = j;
                Validaciones[j]._USE = 0;
            }


            foreach (string line in listaInterface)
            {
                i++;
                string[] temp = line.Split('|');
                try
                {
                    List<ERegex> tmp = new List<ERegex>();
                    string tab = temp[0];
                    tmp = Validaciones.Where(o => o.TAB == tab).ToList();

                    for (j = 0; j < (temp.Length - 1); ++j)
                    {
                        string key = tab + "-" + (j + 1);
                        List<ERegex> val = new List<ERegex>();
                        //val = Validaciones.Where(o => o.KEY == key).ToList();
                        val = Validaciones.Where(o => o.KEY == key && o.DOC.Contains(td)).ToList();
                        temp[j + 1] = temp[j + 1].Trim();
                        if (val[0].DOC.Contains(td))
                        {
                            Regex r = new Regex(@"" + val[0].VAL, RegexOptions.IgnoreCase);
                            Match m = r.Match(temp[j + 1]);
                            Validaciones[val[0]._POS]._USE = 1;

                            if (!m.Success)
                            {
                                if (val[0].MND == "S")
                                {
                                    valido = false;

                                    if (temp[0].StartsWith("DOCUMENTO-AFECTADO") || temp[0].StartsWith("ITEM") || temp[0].StartsWith("LINEA") || temp[0].StartsWith("REFERENCIA") || temp[0].StartsWith("ANTICIPO") || temp[0].StartsWith("EXTRAS"))
                                    {
                                        listerror.Add(val[0].NOM + "( " + temp[1] + ")" + "-" + val[0].MSG);
                                    }
                                    else
                                    {
                                        listerror.Add(val[0].NOM + "-" + val[0].MSG);
                                    }
                                }
                                else
                                {
                                    if (temp[j + 1] != "")
                                    {
                                        valido = false;
                                        listerror.Add(val[0].NOM + "-" + val[0].MSG);
                                    }
                                }
                            }
                        }
                    }

                    tmp = null;
                }
                catch (Exception ValidacionExcepcion)
                {

                }
            }

            // Busqueda de los elemento que no se encuentran en la interface:
            List<ERegex> emp = new List<ERegex>();
            emp = Validaciones.Where(o => o._USE == 0 && o.DOC.Contains(td) && o.MND == "S").ToList();
            if (emp.Count > 0)
            {
                //valido = false;
                listerror.Add(emp[0].NOM + "-" + emp[0].MSG);
            }
            //return valido;

            return listerror;
        }
    }
}
