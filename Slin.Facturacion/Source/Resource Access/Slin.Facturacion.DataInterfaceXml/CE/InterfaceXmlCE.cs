using Slin.Facturacion.BusinessEntities;
using Slin.Facturacion.BusinessEntities.Helper;
using Slin.Facturacion.BusinessEntities.InterfaceXMLCE;
using Slin.Facturacion.Common;
using Slin.Facturacion.Common.Method;
using Slin.Facturacion.DataAccess.Helper;
using Slin.Facturacion.ServiceImplementation;
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

namespace Slin.Facturacion.DataInterfaceXml.CE
{
    public class InterfaceXmlCE
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
        public string nombreArchivoXmlInterfaceError = string.Empty;

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
        public string msjCABECERAPRINCIPAL = string.Empty;
        public string msjCABCABECERAEMISOR = string.Empty;
        public string msjCABCABECERARECEPTOR = string.Empty;
        public string msjTOTAL = string.Empty;
        public string msjLINEA = string.Empty;
        public string msjREFERENCIA = string.Empty;
        public string msjANTICIPO = string.Empty;
        public string msjEXTRAS = string.Empty;
        public string msjMAIL = string.Empty;
        public string msjADICIONAL = string.Empty;

        public string msjDETRACCION = string.Empty;
        public string msjLEYENDA = string.Empty;









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



        public string GetObjInterfaceXML_CE(string xmlLine)
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
                        #region SI EL DOCUMENTO RECIBIO ESTÁ HABILITADO PARA EL COSNUMO DE ESTA WEB SERVICE

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
                        if (listaError.Count > 0)
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
                            foreach (var error in listaError)
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
                                #endregion END SI EL DOCUMENTO NO ESTA EN BD
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


                                //rutaWriter = string.Empty;
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
                    if (msjCABCABECERAEMISOR.Length > 2)
                    {
                        msj = msjCABCABECERAEMISOR;
                    }
                    else if (msjCABCABECERARECEPTOR.Length > 2)
                    {
                        msj = msjCABCABECERARECEPTOR;
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
                WriteLog_Process(Constantes.Msj_DocProcess + nombreArchivoXmlInterface);
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
                //XmlSerializer xmlSerial = new XmlSerializer(typeof(Invoice));
                //sr = new StreamReader(filename);
                //sr = new StreamReader(filename);
                //Invoice inv = (Invoice)xmlSerial.Deserialize(sr);

                //System.Text.Encoding Encoding = Encoding.GetEncoding("ISO-8859-1");

                var inv = new Invoice();
                XmlSerializer xmlSerial = new XmlSerializer(typeof(Invoice));
                using (TextReader reader = new StringReader(xml_line))
                {
                    inv = (Invoice)xmlSerial.Deserialize(reader);
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
                if (inv.CABECERAPRINCIPAL != null)
                {
                    if (inv.CABECERAPRINCIPAL.TipoCE != null)
                    {
                        lineaPrincipal += inv.CABECERAPRINCIPAL.TipoCE + "|";
                    }
                    else
                    {
                        lineaPrincipal += guion + "|";
                    }

                    if (inv.CABECERAPRINCIPAL.ID_CE != null)
                    {
                        lineaPrincipal += inv.CABECERAPRINCIPAL.ID_CE + "|";
                    }
                    else
                    {
                        lineaPrincipal += guion + "|";
                    }

                    if (inv.CABECERAPRINCIPAL.Toperacion != null)
                    {
                        lineaPrincipal += inv.CABECERAPRINCIPAL.Toperacion + "|";
                    }
                    else
                    {
                        lineaPrincipal += guion + "|";
                    }

                    if (inv.CABECERAPRINCIPAL.FEmision != null)
                    {
                        lineaPrincipal += inv.CABECERAPRINCIPAL.FEmision + "|";
                    }
                    else
                    {
                        lineaPrincipal += guion + "|";
                    }

                    if (inv.CABECERAPRINCIPAL.TMoneda != null)
                    {
                        lineaPrincipal += inv.CABECERAPRINCIPAL.TMoneda;
                    }
                    else
                    {
                        lineaPrincipal += guion;
                    }
                }
                else
                {
                    msjCABECERAPRINCIPAL = "Existen Errores en el TAG CABECERAPRINCIPAL";
                }
                #endregion END CABECERA PRINCIPAL

                #region CABECERA CABECERAEMISOR
                string lineaCABECERAEMISOR = "CABECERA-EMISOR|";
                if (inv.CABECERAEMISOR != null)
                {
                    if (inv.CABECERAEMISOR.EmiTpoDoc != null)
                    {
                        lineaCABECERAEMISOR += inv.CABECERAEMISOR.EmiTpoDoc + "|";
                    }
                    else
                    {
                        lineaCABECERAEMISOR += guion + "|";
                    }

                    if (inv.CABECERAEMISOR.EmiNumDoc != null) lineaCABECERAEMISOR += inv.CABECERAEMISOR.EmiNumDoc + "|"; else lineaCABECERAEMISOR += guion + "|";

                    if (inv.CABECERAEMISOR.EmiNombre != null) lineaCABECERAEMISOR += inv.CABECERAEMISOR.EmiNombre + "|"; else lineaCABECERAEMISOR += guion + "|";

                    if (inv.CABECERAEMISOR.EmiNComer != null) lineaCABECERAEMISOR += inv.CABECERAEMISOR.EmiNComer + "|"; else lineaCABECERAEMISOR += guion + "|";

                    if (inv.CABECERAEMISOR.EmiUbigeo != null) lineaCABECERAEMISOR += inv.CABECERAEMISOR.EmiUbigeo + "|"; else lineaCABECERAEMISOR += guion + "|";

                    if (inv.CABECERAEMISOR.EmiDirFiscal != null) lineaCABECERAEMISOR += inv.CABECERAEMISOR.EmiDirFiscal + "|"; else lineaCABECERAEMISOR += guion + "|";

                    if (inv.CABECERAEMISOR.EmiDirUrbani != null) lineaCABECERAEMISOR += inv.CABECERAEMISOR.EmiDirUrbani + "|"; else lineaCABECERAEMISOR += guion + "|";

                    if (inv.CABECERAEMISOR.EmiDirProvin != null) lineaCABECERAEMISOR += inv.CABECERAEMISOR.EmiDirProvin + "|"; else lineaCABECERAEMISOR += guion + "|";

                    if (inv.CABECERAEMISOR.EmiDirDepart != null) lineaCABECERAEMISOR += inv.CABECERAEMISOR.EmiDirDepart + "|"; else lineaCABECERAEMISOR += guion + "|";

                    if (inv.CABECERAEMISOR.EmiDirDistrito != null) lineaCABECERAEMISOR += inv.CABECERAEMISOR.EmiDirDistrito + "|"; else lineaCABECERAEMISOR += guion + "|";

                    if (inv.CABECERAEMISOR.EmiCodPais != null) lineaCABECERAEMISOR += inv.CABECERAEMISOR.EmiCodPais; else lineaCABECERAEMISOR += guion;
                }
                else
                {
                    msjCABCABECERAEMISOR = "Existen Errores en el TAG CABECERA EMISOR";
                }

                #endregion END CABECERA CABECERAEMISOR

                #region CABECERA CABECERARECEPTOR
                string lineaCABECERARECEPTOR = "CABECERA-RECEPTOR|";
                if (inv.CABECERARECEPTOR != null)
                {
                    if (inv.CABECERARECEPTOR.RecTpoDoc != null && inv.CABECERARECEPTOR.RecTpoDoc.Length > 0)
                    {
                        lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.RecTpoDoc + "|";
                    }
                    else
                    {
                        lineaCABECERARECEPTOR += "0" + "|";
                    }

                    if (inv.CABECERARECEPTOR.RecNumDocu != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.RecNumDocu + "|"; else lineaCABECERARECEPTOR += guion + "|";

                    if (inv.CABECERARECEPTOR.RecNombre != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.RecNombre + "|"; else lineaCABECERARECEPTOR += guion + "|";

                    if (inv.CABECERARECEPTOR.RecNComer != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.RecNComer + "|"; else lineaCABECERARECEPTOR += "|";

                    if (inv.CABECERARECEPTOR.RecUbigeo != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.RecUbigeo + "|"; else lineaCABECERARECEPTOR += "|";

                    if (inv.CABECERARECEPTOR.RecDirFiscal != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.RecDirFiscal + "|"; else lineaCABECERARECEPTOR += "|";

                    if (inv.CABECERARECEPTOR.RecDirUrbani != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.RecDirUrbani + "|"; else lineaCABECERARECEPTOR += "|";

                    if (inv.CABECERARECEPTOR.RecDirProvin != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.RecDirProvin + "|"; else lineaCABECERARECEPTOR +=  "|";

                    if (inv.CABECERARECEPTOR.RecDirDepart != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.RecDirDepart + "|"; else lineaCABECERARECEPTOR += "|";

                    if (inv.CABECERARECEPTOR.RecDirDistrito != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.RecDirDistrito + "|"; else lineaCABECERARECEPTOR += "|";

                    if (inv.CABECERARECEPTOR.RecCodPais != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.RecCodPais + "|"; else lineaCABECERARECEPTOR += "|";
                    //if (inv.CABECERARECEPTOR.RecCodPais != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.RecCodPais; else lineaCABECERARECEPTOR += guion;

                    if (inv.CABECERARECEPTOR.LugUbigeo != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.LugUbigeo + "|"; else lineaCABECERARECEPTOR += "|";
                    if (inv.CABECERARECEPTOR.LugDirFiscal != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.LugDirFiscal + "|"; else lineaCABECERARECEPTOR += "|";
                    if (inv.CABECERARECEPTOR.LugDirUrbani != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.LugDirUrbani + "|"; else lineaCABECERARECEPTOR += "|";
                    if (inv.CABECERARECEPTOR.LugDirProvin != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.LugDirProvin + "|"; else lineaCABECERARECEPTOR += "|";
                    if (inv.CABECERARECEPTOR.LugDirDepart != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.LugDirDepart + "|"; else lineaCABECERARECEPTOR += "|";
                    if (inv.CABECERARECEPTOR.LugDirDistrito != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.LugDirDistrito + "|"; else lineaCABECERARECEPTOR += "|";
                    if (inv.CABECERARECEPTOR.LugCodPais != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.LugCodPais + "|"; else lineaCABECERARECEPTOR += "|";

                    if (inv.CABECERARECEPTOR.LocalAnexo != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.LocalAnexo + "|"; else lineaCABECERARECEPTOR += guion;




                    #region OTHER VIEW

                    //if (inv.CABECERARECEPTOR.RecNumDocu != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.RecNumDocu + "|"; else lineaCABECERARECEPTOR += guion + "|";

                    //if (inv.CABECERARECEPTOR.RecNombre != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.RecNombre + "|"; else lineaCABECERARECEPTOR += guion + "|";

                    //if (inv.CABECERARECEPTOR.RecNComer != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.RecNComer + "|"; else lineaCABECERARECEPTOR += guion + "|";

                    //if (inv.CABECERARECEPTOR.RecUbigeo != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.RecUbigeo + "|"; else lineaCABECERARECEPTOR += guion + "|";

                    //if (inv.CABECERARECEPTOR.RecDirFiscal != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.RecDirFiscal + "|"; else lineaCABECERARECEPTOR += guion + "|";

                    //if (inv.CABECERARECEPTOR.RecDirUrbani != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.RecDirUrbani + "|"; else lineaCABECERARECEPTOR += guion + "|";

                    //if (inv.CABECERARECEPTOR.RecDirProvin != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.RecDirProvin + "|"; else lineaCABECERARECEPTOR += guion + "|";

                    //if (inv.CABECERARECEPTOR.RecDirDepart != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.RecDirDepart + "|"; else lineaCABECERARECEPTOR += guion + "|";

                    //if (inv.CABECERARECEPTOR.RecDirDistrito != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.RecDirDistrito + "|"; else lineaCABECERARECEPTOR += guion + "|";

                    //if (inv.CABECERARECEPTOR.RecCodPais != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.RecCodPais + "|"; else lineaCABECERARECEPTOR += guion + "|";
                    ////if (inv.CABECERARECEPTOR.RecCodPais != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.RecCodPais; else lineaCABECERARECEPTOR += guion;

                    //if (inv.CABECERARECEPTOR.LugUbigeo != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.LugUbigeo + "|"; else lineaCABECERARECEPTOR += guion + "|";
                    //if (inv.CABECERARECEPTOR.LugDirFiscal != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.LugDirFiscal + "|"; else lineaCABECERARECEPTOR += guion + "|";
                    //if (inv.CABECERARECEPTOR.LugDirUrbani != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.LugDirUrbani + "|"; else lineaCABECERARECEPTOR += guion + "|";
                    //if (inv.CABECERARECEPTOR.LugDirProvin != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.LugDirProvin + "|"; else lineaCABECERARECEPTOR += guion + "|";
                    //if (inv.CABECERARECEPTOR.LugDirDepart != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.LugDirDepart + "|"; else lineaCABECERARECEPTOR += guion + "|";
                    //if (inv.CABECERARECEPTOR.LugDirDistrito != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.LugDirDistrito + "|"; else lineaCABECERARECEPTOR += guion + "|";
                    //if (inv.CABECERARECEPTOR.LugCodPais != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.LugCodPais + "|"; else lineaCABECERARECEPTOR += guion += "|";

                    //if (inv.CABECERARECEPTOR.LocalAnexo != null) lineaCABECERARECEPTOR += inv.CABECERARECEPTOR.LocalAnexo + "|"; else lineaCABECERARECEPTOR += guion;

                    #endregion

                }
                else
                {
                    msjCABCABECERARECEPTOR = msjCABCABECERARECEPTOR = "Existen Errores en el TAG CABECERACABECERARECEPTOR";
                }

                #endregion END CABECERA CABECERARECEPTOR



                #region TOTAL
                string lineaTOTAL = string.Empty;

                if (inv.TOTAL != null)
                {
                    lineaTOTAL = "TOTAL|";
                    if (inv.TOTAL.TotVtaGrab != null)
                    {
                        lineaTOTAL += inv.TOTAL.TotVtaGrab + "|";
                    }
                    else if (inv.TOTAL.TotVtaGrab == null)
                    {
                        msjTOTAL = "Existes Campos que son Necesarios en el TAG TOTAL";
                    }
                    else
                    {
                        lineaTOTAL += guion + "|";
                    }

                    if (inv.TOTAL.TotVtaInaf != null) lineaTOTAL += inv.TOTAL.TotVtaInaf + "|"; else lineaTOTAL += guion + "|";

                    if (inv.TOTAL.TotVtaExon != null) lineaTOTAL += inv.TOTAL.TotVtaExon + "|"; else lineaTOTAL += guion + "|";

                    if (inv.TOTAL.TotVtaGrat != null) lineaTOTAL += inv.TOTAL.TotVtaGrat + "|"; else lineaTOTAL += guion + "|";

                    if (inv.TOTAL.TotTotDscto != null) lineaTOTAL += inv.TOTAL.TotTotDscto + "|"; else lineaTOTAL += guion + "|";


                    if (inv.TOTAL.TotSumIGV != null)
                    {
                        lineaTOTAL += inv.TOTAL.TotSumIGV + "|";
                    }
                    else if (inv.TOTAL.TotSumIGV == null)
                    {
                        msjTOTAL = "Existes Campos que son Necesarios en el TAG TOTAL";
                    }
                    else
                    {
                        lineaTOTAL += guion + "|";
                    }


                    if (inv.TOTAL.TotSumISC != null) lineaTOTAL += inv.TOTAL.TotSumISC + "|"; else lineaTOTAL += guion + "|";

                    if (inv.TOTAL.TotSumOTrib != null) lineaTOTAL += inv.TOTAL.TotSumOTrib + "|"; else lineaTOTAL += guion + "|";

                    if (inv.TOTAL.TotDctoGlobal != null) lineaTOTAL += inv.TOTAL.TotDctoGlobal + "|"; else lineaTOTAL += guion + "|";

                    if (inv.TOTAL.TotSumOCargo != null) lineaTOTAL += inv.TOTAL.TotSumOCargo + "|"; else lineaTOTAL += guion + "|";

                    if (inv.TOTAL.TotAnticipo != null) lineaTOTAL += inv.TOTAL.TotAnticipo + "|"; else lineaTOTAL += guion + "|";

                    if (inv.TOTAL.TotImporTotal != null)
                    {
                        lineaTOTAL += inv.TOTAL.TotImporTotal + "|";
                    }
                    else if (inv.TOTAL.TotImporTotal == null)
                    {
                        msjTOTAL = "Existes Campos que son Necesarios en el TAG TOTAL";
                    }
                    else
                    {
                        lineaTOTAL += guion + "|";
                    }

                    if (inv.TOTAL.MontoLiteral != null) lineaTOTAL += inv.TOTAL.MontoLiteral; else lineaTOTAL += guion;
                }
                else
                {
                    msjTOTAL = "No existe el TAG TOTAL o es Incorrecto";
                }

                #endregion END TOTAL



                #region LINEAS

                List<string> listaLinea = new List<string>();

                if (inv.LINEAS != null)
                {
                    if (inv.LINEAS.LINEA.Count > 0)
                    {
                        if (inv.LINEAS.LINEA[0].LnNroOrden != null && inv.LINEAS.LINEA[0].LnUndMed != null && inv.LINEAS.LINEA[0].LnCantidad != null && inv.LINEAS.LINEA[0].LnDescrip != null && inv.LINEAS.LINEA[0].LnValUnit != null &&
                        inv.LINEAS.LINEA[0].LnMntPrcVta != null && inv.LINEAS.LINEA[0].LnValVta != null && inv.LINEAS.LINEA[0].LnMntIGV != null && inv.LINEAS.LINEA[0].LnCodAfectIGV != null &&
                        inv.LINEAS.LINEA[0].LnMntISC != null && inv.LINEAS.LINEA[0].LnCodSisISC != null && inv.LINEAS.LINEA[0].LnDescMnto != null)
                        //inv.LINEAS.LINEA[0].LnMntISC != null && inv.LINEAS.LINEA[0].LnCodSisISC != null && inv.LINEAS.LINEA[0].LnDescMnto != null && inv.LINEAS.LINEA[0].LnPorcIGV != null)
                        {
                            for (int i = 0; i <= inv.LINEAS.LINEA.Count - 1; i++)
                            {
                                string codProduc = inv.LINEAS.LINEA[i].LnCodProd;

                                if (codProduc != null && codProduc.Length > 0)
                                {
                                    listaLinea.Add((i + 1) + "|" + inv.LINEAS.LINEA[i].LnUndMed + "|" + inv.LINEAS.LINEA[i].LnCantidad + "|" + inv.LINEAS.LINEA[i].LnCodProd + "|" + inv.LINEAS.LINEA[i].LnDescrip + "|" + inv.LINEAS.LINEA[i].LnValUnit + "|" +
                                        inv.LINEAS.LINEA[i].LnMntPrcVta + "|" + inv.LINEAS.LINEA[i].LnValVta + "|" + inv.LINEAS.LINEA[i].LnMntIGV + "|" + inv.LINEAS.LINEA[i].LnCodAfectIGV + "|" +
                                        inv.LINEAS.LINEA[i].LnMntISC + "|" + inv.LINEAS.LINEA[i].LnCodSisISC + "|" + inv.LINEAS.LINEA[i].LnDescMnto);
                                        //inv.LINEAS.LINEA[i].LnMntISC + "|" + inv.LINEAS.LINEA[i].LnCodSisISC + "|" + inv.LINEAS.LINEA[i].LnDescMnto + "|" + inv.LINEAS.LINEA[i].LnPorcIGV);
                                }
                                else
                                {
                                    listaLinea.Add((i + 1) + "|" + inv.LINEAS.LINEA[i].LnUndMed + "|" + inv.LINEAS.LINEA[i].LnCantidad + "|" + string.Empty + "|" + inv.LINEAS.LINEA[i].LnDescrip + "|" + inv.LINEAS.LINEA[i].LnValUnit + "|" +
                                        inv.LINEAS.LINEA[i].LnMntPrcVta + "|" + inv.LINEAS.LINEA[i].LnValVta + "|" + inv.LINEAS.LINEA[i].LnMntIGV + "|" + inv.LINEAS.LINEA[i].LnCodAfectIGV + "|" +
                                        inv.LINEAS.LINEA[i].LnMntISC + "|" + inv.LINEAS.LINEA[i].LnCodSisISC + "|" + inv.LINEAS.LINEA[i].LnDescMnto);
                                        //inv.LINEAS.LINEA[i].LnMntISC + "|" + inv.LINEAS.LINEA[i].LnCodSisISC + "|" + inv.LINEAS.LINEA[i].LnDescMnto + "|" + inv.LINEAS.LINEA[i].LnPorcIGV);
                                }
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

                #region REFERENCIA

                List<string> listaReferencia = new List<string>();
                if (inv.REFERENCIAS != null)
                {
                    if (inv.REFERENCIAS.REFERENCIA.Count > 0)
                    {
                        if (inv.REFERENCIAS.REFERENCIA[0].RefID != null && inv.REFERENCIAS.REFERENCIA[0].RefTpoDoc != null)
                        {
                            for (int i = 0; i <= inv.REFERENCIAS.REFERENCIA.Count - 1; i++)
                            {
                                listaReferencia.Add((i + 1) + "|" + inv.REFERENCIAS.REFERENCIA[i].RefID + "|" + inv.REFERENCIAS.REFERENCIA[i].RefTpoDoc);
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
                if (inv.ANTICIPOS != null)
                {
                    if (inv.ANTICIPOS.ANTICIPO.Count > 0)
                    {
                        if (inv.ANTICIPOS.ANTICIPO[0].AntMonto != null && inv.ANTICIPOS.ANTICIPO[0].AntTpoDocAnt != null && inv.ANTICIPOS.ANTICIPO[0].AntIdDocAnt != null &&
                        inv.ANTICIPOS.ANTICIPO[0].AntNumDocEmi != null && inv.ANTICIPOS.ANTICIPO[0].AntTpoDocEmi != null)
                        {
                            for (int i = 0; i <= inv.ANTICIPOS.ANTICIPO.Count - 1; i++)
                            {
                                listaAnticipo.Add((i + 1) + "|" + inv.ANTICIPOS.ANTICIPO[i].AntMonto + "|" + inv.ANTICIPOS.ANTICIPO[i].AntTpoDocAnt + "|" +
                                    inv.ANTICIPOS.ANTICIPO[i].AntIdDocAnt + "|" + inv.ANTICIPOS.ANTICIPO[i].AntNumDocEmi + "|" + inv.ANTICIPOS.ANTICIPO[i].AntTpoDocEmi);
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

                #region DETRACCION

                string lineaDETRACCION = string.Empty;
                if (inv.DETRACCION != null)
                {

                    //lineaDETRACCION = "DETRACCION|";
                    //if (inv.DETRACCION.DetValBBSS != null) lineaDETRACCION += inv.DETRACCION.DetValBBSS + "|"; else lineaDETRACCION += guion + "|";
                    //if (inv.DETRACCION.DetCtaBN != null) lineaDETRACCION += inv.DETRACCION.DetCtaBN + "|"; else lineaDETRACCION += guion + "|";
                    //if (inv.DETRACCION.DetPorcent != null) lineaDETRACCION += inv.DETRACCION.DetPorcent + "|"; else lineaDETRACCION += guion + "|";
                    //if (inv.DETRACCION.DetMonto != null) lineaDETRACCION += inv.DETRACCION.DetMonto + "|"; else lineaDETRACCION += guion;

                    if (inv.DETRACCION.DetValBBSS != null && inv.DETRACCION.DetValBBSS.Length > 0)
                    {
                        if (inv.DETRACCION.DetCtaBN != null && inv.DETRACCION.DetCtaBN.Length > 0)
                        {
                            lineaDETRACCION = "DETRACCION|";
                            if (inv.DETRACCION.DetValBBSS != null) lineaDETRACCION += inv.DETRACCION.DetValBBSS + "|"; else lineaDETRACCION += guion + "|";
                            if (inv.DETRACCION.DetCtaBN != null) lineaDETRACCION += inv.DETRACCION.DetCtaBN + "|"; else lineaDETRACCION += guion + "|";
                            if (inv.DETRACCION.DetPorcent != null) lineaDETRACCION += inv.DETRACCION.DetPorcent + "|"; else lineaDETRACCION += guion + "|";
                            if (inv.DETRACCION.DetMonto != null) lineaDETRACCION += inv.DETRACCION.DetMonto + "|"; else lineaDETRACCION += guion;
                        }
                        else
                        {
                            msjDETRACCION += "Existen Campos Obligatorios que son Incorrectos en el TAG DETRACCION";
                        }
                    }


                    #region OTHER
                    //if (inv.DETRACCION.DetValBBSS == null && inv.DETRACCION.DetCtaBN == null)
                    //{
                    //    msjDETRACCION += "Existen Campos Obligatorios que son Incorrectos en el TAG DETRACCION";
                    //}
                    //else if (inv.DETRACCION.DetValBBSS.Length == 0 && inv.DETRACCION.DetCtaBN.Length == 0)
                    //{
                    //    msjDETRACCION += "Existen Campos Obligatorios que son Incorrectos en el TAG DETRACCION";
                    //}
                    //else
                    //{
                    //    lineaDETRACCION = "DETRACCION|";
                    //    lineaDETRACCION += inv.DETRACCION.DetValBBSS + "|";

                    //    if (inv.DETRACCION.DetCtaBN != null) lineaDETRACCION += inv.DETRACCION.DetCtaBN + "|"; else lineaDETRACCION += guion + "|";
                    //    if (inv.DETRACCION.DetPorcent != null) lineaDETRACCION += inv.DETRACCION.DetPorcent + "|"; else lineaDETRACCION += guion + "|";
                    //    if (inv.DETRACCION.DetMonto != null) lineaDETRACCION += inv.DETRACCION.DetMonto + "|"; else lineaDETRACCION += guion;
                    //}
                    #endregion
                }

                #endregion END DETRACCION


                #region FACT GUIA
                //string lineaFACTGUIA = string.Empty;

                string linea_FG_DATOS = string.Empty;
                string linea_FG_DESTI = string.Empty;
                string linea_FG_TRASL = string.Empty;
                string linea_FG_TRANS = string.Empty;
                string linea_FG_CONDU = string.Empty;
                string linea_FG_VEHIC = string.Empty;
                string linea_FG_PLLEG = string.Empty;
                string linea_FG_PPART = string.Empty;

                if (inv.FACTGUIA != null)
                {
                    if (inv.FACTGUIA.DatoNroDoc != null)
                    {
                        #region NEW METHOD FACTGUIA

                        linea_FG_DATOS += "FG-DATOS|" + inv.FACTGUIA.DatoNroDoc + "|";

                        linea_FG_DATOS += inv.FACTGUIA.DatoCodTpoDoc != null ? inv.FACTGUIA.DatoCodTpoDoc + "|" : guion + "|";
                        linea_FG_DATOS += inv.FACTGUIA.DatoTpoDoc != null ? inv.FACTGUIA.DatoTpoDoc + "|" : guion + "|";
                        linea_FG_DATOS += inv.FACTGUIA.DatoNumDocRem != null ? inv.FACTGUIA.DatoNumDocRem + "|" : guion;

                        linea_FG_DESTI += "FG-DESTI|";
                        linea_FG_DESTI += inv.FACTGUIA.DestNumDoc != null ? inv.FACTGUIA.DestNumDoc + "|" : guion + "|";
                        linea_FG_DESTI += inv.FACTGUIA.DestTpoDoc != null ? inv.FACTGUIA.DestTpoDoc + "|" : guion + "|";
                        linea_FG_DESTI += inv.FACTGUIA.DestRazSoc != null ? inv.FACTGUIA.DestRazSoc + "|" : guion;

                        linea_FG_TRASL += "FG-TRASL|";
                        linea_FG_TRASL += inv.FACTGUIA.TrasMotivo != null ? inv.FACTGUIA.TrasMotivo + "|" : guion + "|";
                        linea_FG_TRASL += inv.FACTGUIA.TrasPeso != null ? inv.FACTGUIA.TrasPeso + "|" : guion + "|";
                        linea_FG_TRASL += inv.FACTGUIA.TrasUndMed != null ? inv.FACTGUIA.TrasUndMed + "|" : guion + "|";
                        linea_FG_TRASL += inv.FACTGUIA.TrasModalidad != null ? inv.FACTGUIA.TrasModalidad + "|" : guion + "|";
                        linea_FG_TRASL += inv.FACTGUIA.TrasFecInicio != null ? inv.FACTGUIA.TrasFecInicio + "|" : guion;

                        linea_FG_TRANS += "FG-TRANS|";
                        linea_FG_TRANS += inv.FACTGUIA.TranIDDoc != null ? inv.FACTGUIA.TranIDDoc + "|" : guion + "|";
                        linea_FG_TRANS += inv.FACTGUIA.TranTpoDoc != null ? inv.FACTGUIA.TranTpoDoc + "|" : guion + "|";
                        linea_FG_TRANS += inv.FACTGUIA.TranRazSoc != null ? inv.FACTGUIA.TranRazSoc + "|" : guion;

                        linea_FG_CONDU += "FG-CONDU";
                        linea_FG_CONDU += inv.FACTGUIA.CondIDDoc != null ? inv.FACTGUIA.CondIDDoc + "|" : guion + "|";
                        linea_FG_CONDU += inv.FACTGUIA.CondTpoDoc != null ? inv.FACTGUIA.CondTpoDoc + "|" : guion;

                        linea_FG_VEHIC += "FG-VEHIC";
                        linea_FG_VEHIC += inv.FACTGUIA.VehiConstancia != null ? inv.FACTGUIA.VehiConstancia + "|" : guion + "|";
                        linea_FG_VEHIC += inv.FACTGUIA.VehiPlaca != null ? inv.FACTGUIA.VehiPlaca + "|" : guion;

                        linea_FG_PLLEG += "FG-PLLEG|";
                        linea_FG_PLLEG += inv.FACTGUIA.DirLlegUbigeo != null ? inv.FACTGUIA.DirLlegUbigeo + "|" : guion + "|";
                        linea_FG_PLLEG += inv.FACTGUIA.DirLlegDireccion != null ? inv.FACTGUIA.DirLlegDireccion + "|" : guion;

                        linea_FG_PPART += "FG-PPART|";
                        linea_FG_PPART += inv.FACTGUIA.DirParUbigeo != null ? inv.FACTGUIA.DirParUbigeo + "|" : guion + "|";
                        linea_FG_PPART += inv.FACTGUIA.DirParDireccion != null ? inv.FACTGUIA.DirParDireccion + "|" : guion;

                        #endregion END NEW METHOD FACTGUIA

                        #region AFTER METHOD FACTGUIA CHANGE AQUI
                        //lineaFACTGUIA += "FACT-GUIA";
                        //if (inv.FACTGUIA.DatoNroDoc != null)
                        //{
                        //    lineaFACTGUIA += inv.FACTGUIA.DatoNroDoc + "|";
                        //}
                        //else
                        //{
                        //    lineaFACTGUIA += guion + "|";
                        //}

                        //if (inv.FACTGUIA.DatoCodTpoDoc != null) lineaFACTGUIA += inv.FACTGUIA.DatoCodTpoDoc + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.DatoTpoDoc != null) lineaFACTGUIA += inv.FACTGUIA.DatoTpoDoc + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.DatoNumDocRem != null) lineaFACTGUIA += inv.FACTGUIA.DatoNumDocRem + "|"; else lineaFACTGUIA += guion + "|";


                        //if (inv.FACTGUIA.DestNumDoc != null) lineaFACTGUIA += inv.FACTGUIA.DestNumDoc + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.DestTpoDoc != null) lineaFACTGUIA += inv.FACTGUIA.DestTpoDoc + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.DestRazSoc != null) lineaFACTGUIA += inv.FACTGUIA.DestRazSoc + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.TrasMotivo != null) lineaFACTGUIA += inv.FACTGUIA.TrasMotivo + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.TrasPeso != null) lineaFACTGUIA += inv.FACTGUIA.TrasPeso + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.TrasUndMed != null) lineaFACTGUIA += inv.FACTGUIA.TrasUndMed + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.TrasModalidad != null) lineaFACTGUIA += inv.FACTGUIA.TrasModalidad + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.TrasFecInicio != null) lineaFACTGUIA += inv.FACTGUIA.TrasFecInicio + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.TranIDDoc != null) lineaFACTGUIA += inv.FACTGUIA.TranIDDoc + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.TranTpoDoc != null) lineaFACTGUIA += inv.FACTGUIA.TranTpoDoc + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.TranRazSoc != null) lineaFACTGUIA += inv.FACTGUIA.TranRazSoc + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.CondIDDoc != null) lineaFACTGUIA += inv.FACTGUIA.CondIDDoc + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.CondTpoDoc != null) lineaFACTGUIA += inv.FACTGUIA.CondTpoDoc + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.VehiConstancia != null) lineaFACTGUIA += inv.FACTGUIA.VehiConstancia + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.VehiPlaca != null) lineaFACTGUIA += inv.FACTGUIA.VehiPlaca + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.DirLlegUbigeo != null) lineaFACTGUIA += inv.FACTGUIA.DatoCodTpoDoc + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.DirLlegDireccion != null) lineaFACTGUIA += inv.FACTGUIA.DatoCodTpoDoc + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.DirParUbigeo != null) lineaFACTGUIA += inv.FACTGUIA.DirParUbigeo + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.DirParDireccion != null) lineaFACTGUIA += inv.FACTGUIA.DirParDireccion + "|"; else lineaFACTGUIA += guion;

                        #endregion END METHOD FACTGUIA
                    }
                    //else if(inv.FACTGUIA.DatoNroDoc.Length > 0)
                    //{
                        #region NEW METHOD FACTGUIA

                        //linea_FG_DATOS += "FG-DATOS|" + inv.FACTGUIA.DatoNroDoc + "|";

                        //linea_FG_DATOS += inv.FACTGUIA.DatoCodTpoDoc != null ? inv.FACTGUIA.DatoCodTpoDoc + "|" : guion + "|";
                        //linea_FG_DATOS += inv.FACTGUIA.DatoTpoDoc != null ? inv.FACTGUIA.DatoTpoDoc + "|" : guion + "|";
                        //linea_FG_DATOS += inv.FACTGUIA.DatoNumDocRem != null ? inv.FACTGUIA.DatoNumDocRem + "|" : guion;

                        //linea_FG_DESTI += "FG-DESTI|";
                        //linea_FG_DESTI += inv.FACTGUIA.DestNumDoc != null ? inv.FACTGUIA.DestNumDoc + "|" : guion + "|";
                        //linea_FG_DESTI += inv.FACTGUIA.DestTpoDoc != null ? inv.FACTGUIA.DestTpoDoc + "|" : guion + "|";
                        //linea_FG_DESTI += inv.FACTGUIA.DestRazSoc != null ? inv.FACTGUIA.DestRazSoc + "|" : guion;

                        //linea_FG_TRASL += "FG-TRASL|";
                        //linea_FG_TRASL += inv.FACTGUIA.TrasMotivo != null ? inv.FACTGUIA.TrasMotivo + "|" : guion + "|";
                        //linea_FG_TRASL += inv.FACTGUIA.TrasPeso != null ? inv.FACTGUIA.TrasPeso + "|" : guion + "|";
                        //linea_FG_TRASL += inv.FACTGUIA.TrasUndMed != null ? inv.FACTGUIA.TrasUndMed + "|" : guion + "|";
                        //linea_FG_TRASL += inv.FACTGUIA.TrasModalidad != null ? inv.FACTGUIA.TrasModalidad + "|" : guion + "|";
                        //linea_FG_TRASL += inv.FACTGUIA.TrasFecInicio != null ? inv.FACTGUIA.TrasFecInicio + "|" : guion;

                        //linea_FG_TRANS += "FG-TRANS|";
                        //linea_FG_TRANS += inv.FACTGUIA.TranIDDoc != null ? inv.FACTGUIA.TranIDDoc + "|" : guion + "|";
                        //linea_FG_TRANS += inv.FACTGUIA.TranTpoDoc != null ? inv.FACTGUIA.TranTpoDoc + "|" : guion + "|";
                        //linea_FG_TRANS += inv.FACTGUIA.TranRazSoc != null ? inv.FACTGUIA.TranRazSoc + "|" : guion;

                        //linea_FG_CONDU += "FG-CONDU";
                        //linea_FG_CONDU += inv.FACTGUIA.CondIDDoc != null ? inv.FACTGUIA.CondIDDoc + "|" : guion + "|";
                        //linea_FG_CONDU += inv.FACTGUIA.CondTpoDoc != null ? inv.FACTGUIA.CondTpoDoc + "|" : guion;

                        //linea_FG_VEHIC += "FG-VEHIC";
                        //linea_FG_VEHIC += inv.FACTGUIA.VehiConstancia != null ? inv.FACTGUIA.VehiConstancia + "|" : guion + "|";
                        //linea_FG_VEHIC += inv.FACTGUIA.VehiPlaca != null ? inv.FACTGUIA.VehiPlaca + "|" : guion;

                        //linea_FG_PLLEG += "FG-PLLEG|";
                        //linea_FG_PLLEG += inv.FACTGUIA.DirLlegUbigeo != null ? inv.FACTGUIA.DirLlegUbigeo + "|" : guion + "|";
                        //linea_FG_PLLEG += inv.FACTGUIA.DirLlegDireccion != null ? inv.FACTGUIA.DirLlegDireccion + "|" : guion;

                        //linea_FG_PPART += "FG-PPART|";
                        //linea_FG_PPART += inv.FACTGUIA.DirParUbigeo != null ? inv.FACTGUIA.DirParUbigeo + "|" : guion + "|";
                        //linea_FG_PPART += inv.FACTGUIA.DirParDireccion != null ? inv.FACTGUIA.DirParDireccion + "|" : guion;

                        #endregion END NEW METHOD FACTGUIA

                        #region AFTER METHOD FACTGUIA
                        //lineaFACTGUIA += "FACT-GUIA";
                        //if (inv.FACTGUIA.DatoNroDoc != null)
                        //{
                        //    lineaFACTGUIA += inv.FACTGUIA.DatoNroDoc + "|";
                        //}
                        //else
                        //{
                        //    lineaFACTGUIA += guion + "|";
                        //}

                        //if (inv.FACTGUIA.DatoCodTpoDoc != null) lineaFACTGUIA += inv.FACTGUIA.DatoCodTpoDoc + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.DatoTpoDoc != null) lineaFACTGUIA += inv.FACTGUIA.DatoTpoDoc + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.DatoNumDocRem != null) lineaFACTGUIA += inv.FACTGUIA.DatoNumDocRem + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.DestNumDoc != null) lineaFACTGUIA += inv.FACTGUIA.DestNumDoc + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.DestTpoDoc != null) lineaFACTGUIA += inv.FACTGUIA.DestTpoDoc + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.DestRazSoc != null) lineaFACTGUIA += inv.FACTGUIA.DestRazSoc + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.TrasMotivo != null) lineaFACTGUIA += inv.FACTGUIA.TrasMotivo + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.TrasPeso != null) lineaFACTGUIA += inv.FACTGUIA.TrasPeso + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.TrasUndMed != null) lineaFACTGUIA += inv.FACTGUIA.TrasUndMed + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.TrasModalidad != null) lineaFACTGUIA += inv.FACTGUIA.TrasModalidad + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.TrasFecInicio != null) lineaFACTGUIA += inv.FACTGUIA.TrasFecInicio + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.TranIDDoc != null) lineaFACTGUIA += inv.FACTGUIA.TranIDDoc + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.TranTpoDoc != null) lineaFACTGUIA += inv.FACTGUIA.TranTpoDoc + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.TranRazSoc != null) lineaFACTGUIA += inv.FACTGUIA.TranRazSoc + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.CondIDDoc != null) lineaFACTGUIA += inv.FACTGUIA.CondIDDoc + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.CondTpoDoc != null) lineaFACTGUIA += inv.FACTGUIA.CondTpoDoc + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.VehiConstancia != null) lineaFACTGUIA += inv.FACTGUIA.VehiConstancia + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.VehiPlaca != null) lineaFACTGUIA += inv.FACTGUIA.VehiPlaca + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.DirLlegUbigeo != null) lineaFACTGUIA += inv.FACTGUIA.DatoCodTpoDoc + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.DirLlegDireccion != null) lineaFACTGUIA += inv.FACTGUIA.DatoCodTpoDoc + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.DirParUbigeo != null) lineaFACTGUIA += inv.FACTGUIA.DirParUbigeo + "|"; else lineaFACTGUIA += guion + "|";
                        //if (inv.FACTGUIA.DirParDireccion != null) lineaFACTGUIA += inv.FACTGUIA.DirParDireccion + "|"; else lineaFACTGUIA += guion;

                        #endregion END AFTER METHOD FACTGUIA
                    //}
                    else
                    {
                        //lineaFACTGUIA = string.Empty;
                    }
                }


                #endregion


                #region LEYENDA

                List<string> listaLEYENDA = new List<string>();
                if (inv.LEYENDAS != null)
                {
                    if (inv.LEYENDAS.LEYENDA.Count > 0)
                    {

                        if (inv.LEYENDAS.LEYENDA[0].LeyCodigo != null && inv.LEYENDAS.LEYENDA[0].LeyDescrip != null)
                        {
                            for (int i = 0; i <= inv.LEYENDAS.LEYENDA.Count - 1; i++)
                            {
                                listaLEYENDA.Add((i + 1) + "|" + inv.LEYENDAS.LEYENDA[i].LeyCodigo + "|" + inv.LEYENDAS.LEYENDA[i].LeyDescrip);
                            }
                        }
                        else
                        {
                            msjLEYENDA += "Existen Campos Obligatorios que son Incorrectos en el TAG LEYENDA";
                        }
                    }
                }
                else
                {
                    msjLEYENDA += "No existe el TAG LEYENDA o es Incorrecto";
                }
                #endregion END LEYENDA






                #region EXTRAS

                List<string> listaExtras = new List<string>();
                if (inv.EXTRAS != null)
                {
                    if (inv.EXTRAS.EXTRA.Count > 0)
                    {
                        if (inv.EXTRAS.EXTRA[0].ExID != null && inv.EXTRAS.EXTRA[0].ExDato != null && inv.EXTRAS.EXTRA[0].ExTpoDato != null)
                        {
                            for (int i = 0; i <= inv.EXTRAS.EXTRA.Count - 1; i++)
                            {
                                listaExtras.Add(inv.EXTRAS.EXTRA[i].ExID + "|" + inv.EXTRAS.EXTRA[i].ExDato + "|" + inv.EXTRAS.EXTRA[i].ExTpoDato);
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
                if (inv.MAIL != null)
                {
                    lineaCorreo = "MAIL|";
                    if (inv.MAIL.Para != null) lineaCorreo += inv.MAIL.Para + "|"; else lineaCorreo += guion + "|";

                    if (inv.MAIL.CC != null) lineaCorreo += inv.MAIL.CC + "|"; else lineaCorreo += guion + "|";

                    if (inv.MAIL.CCO != null) lineaCorreo += inv.MAIL.CCO; else lineaCorreo += guion;

                    msjMAIL += "Existen Campos Obligatorios que son Incorrectos";
                }
                else
                {
                    msjMAIL += "No existe el TAG CORREO o es Incorrecto";
                }

                #endregion END MAIL

                #region ADICIONAL

                string lineaAdicional = string.Empty;
                if (inv.ADICIONAL != null)
                {
                    lineaAdicional = "ADICIONAL|";
                    if (inv.ADICIONAL.Sede != null) lineaAdicional += inv.ADICIONAL.Sede + "|"; else lineaAdicional += guion + "|";
                    if (inv.ADICIONAL.Usuario != null) lineaAdicional += inv.ADICIONAL.Usuario + "|"; else lineaAdicional += guion + "|";
                    if (inv.ADICIONAL.Impresora != null) lineaAdicional += inv.ADICIONAL.Impresora + "|"; else lineaAdicional += guion + "|";
                    if (inv.ADICIONAL.Campo1 != null) lineaAdicional += inv.ADICIONAL.Campo1 + "|"; else lineaAdicional += guion + "|";
                    if (inv.ADICIONAL.Campo2 != null) lineaAdicional += inv.ADICIONAL.Campo2 + "|"; else lineaAdicional += guion + "|";
                    if (inv.ADICIONAL.Campo3 != null) lineaAdicional += inv.ADICIONAL.Campo3 + "|"; else lineaAdicional += guion + "|";
                    if (inv.ADICIONAL.Campo4 != null) lineaAdicional += inv.ADICIONAL.Campo4 + "|"; else lineaAdicional += guion + "|";
                    if (inv.ADICIONAL.Campo5 != null) lineaAdicional += inv.ADICIONAL.Campo5 + "|"; else lineaAdicional += guion + "|";
                    if (inv.ADICIONAL.Campo6 != null) lineaAdicional += inv.ADICIONAL.Campo6 + "|"; else lineaAdicional += guion + "|";
                    if (inv.ADICIONAL.Campo7 != null) lineaAdicional += inv.ADICIONAL.Campo7 + "|"; else lineaAdicional += guion + "|";
                    if (inv.ADICIONAL.Campo8 != null) lineaAdicional += inv.ADICIONAL.Campo8 + "|"; else lineaAdicional += guion + "|";
                    if (inv.ADICIONAL.Campo9 != null) lineaAdicional += inv.ADICIONAL.Campo9 + "|"; else lineaAdicional += guion + "|";
                    if (inv.ADICIONAL.Campo10 != null) lineaAdicional += inv.ADICIONAL.Campo10; else lineaAdicional += guion;
                }
                else
                {
                    msjADICIONAL += "No existe el TAG ADICIONAL o es Incorrecto";
                }

                #endregion END ADICIONAL

                #region LISTA INTERFACE
                listaInterface.Add(lineaCE);
                listaInterface.Add(lineaPrincipal);


                if (inv.CABECERAPRINCIPAL.TipoCE != null)
                {
                    #region CASE TIPO CE

                    switch (inv.CABECERAPRINCIPAL.TipoCE)
                    {
                        case "01":
                        case "03":
                            {
                                listaInterface.Add(lineaCABECERAEMISOR);
                                listaInterface.Add(lineaCABECERARECEPTOR);
                                listaInterface.Add(lineaTOTAL);

                                foreach (var li in listaLinea)
                                {
                                    listaInterface.Add("LINEA|" + li);
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

                                if (lineaDETRACCION.Length > 12)
                                {
                                    listaInterface.Add(lineaDETRACCION);
                                }

                                //AFTER FACT GUIA
                                //if (lineaFACTGUIA.Length > 12)
                                //{
                                //    listaInterface.Add(lineaFACTGUIA);
                                //}

                                if (linea_FG_DATOS.Length > 15)
                                {
                                    listaInterface.Add(linea_FG_DATOS);
                                    listaInterface.Add(linea_FG_DESTI);
                                    listaInterface.Add(linea_FG_TRASL);
                                    listaInterface.Add(linea_FG_TRANS);
                                    listaInterface.Add(linea_FG_CONDU);
                                    listaInterface.Add(linea_FG_VEHIC);
                                    listaInterface.Add(linea_FG_PLLEG);
                                    listaInterface.Add(linea_FG_PPART);
                                }

                                if (listaLEYENDA.Count > 0)
                                {
                                    foreach (var ley in listaLEYENDA)
                                    {
                                        listaInterface.Add("LEYENDA|" + ley);
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

                if (inv.CABECERAEMISOR != null && inv.CE != null && inv.CABECERAPRINCIPAL != null)
                {
                    if (inv.CABECERAPRINCIPAL.TipoCE != null && inv.CABECERAEMISOR.EmiNumDoc != null && inv.CE.ID != null && inv.CABECERAPRINCIPAL.ID_CE != null)
                    {
                        ObjectEmpresa.RUC = inv.CABECERAEMISOR.EmiNumDoc;
                        //entityRUCId1 = inv.CE.ID.Substring(0, 11);

                        //IdDocumentoReceib = inv.CE.ID;
                        //IdDocumentoCreated = inv.CABECERAEMISOR.EmiNumDoc + "-" + inv.CABECERAPRINCIPAL.TipoCE + "-" + inv.CABECERAPRINCIPAL.ID_CE;

                        TPO_CE = inv.CABECERAPRINCIPAL.TipoCE;
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
        public int it, li, re, an, da, ex, nroLINEAS = 0, de, ley;




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

        #region OTHER
        //public string[] getSizedTmp(string[] tmp, List<ERegex> Validaciones)
        //{
        //    int nlines;
        //    nlines = Validaciones.Where(o => o.TAB == tmp[0]).Count() + 1;
        //    List<string> mod = tmp.OfType<string>().ToList();
        //    if (mod.Count < nlines)
        //    {
        //        int a = nlines - mod.Count;
        //        for (int i = 0; i < a; ++i)
        //        {
        //            mod.Add("");
        //        }
        //    }
        //    string[] res = mod.ToArray();
        //    return res;
        //}
        #endregion

        public List<string> ValidaInterfaceNew(List<string> txt, string td)
        {
            List<string> listerror = new List<string>();

            listerror = new List<string>();

            List<ERegex> Validaciones = new List<ERegex>();
            Validaciones = new ServicioInterfaceSOA().getRegex();
            if (Validaciones.Count == 0)
            {
            }
            bool valido = true;
            int i = 0, j = 0;



            #region ADD VALIDATION

            foreach (string line in txt)
            {
                string[] temx = line.Split('|');
                string[] temp = getSizedTmp(temx, Validaciones, td);
                //string[] temp = getSizedTmp(temx, Validaciones);
                if (temp[0].StartsWith("DOCUMENTO-AFECTADO") || temp[0].StartsWith("ITEM") || temp[0].StartsWith("LINEA") || temp[0].StartsWith("REFERENCIA") || temp[0].StartsWith("ANTICIPO") || temp[0].StartsWith("EXTRAS") || temp[0].StartsWith("LEYENDA"))
                {
                    fDict.Add(temp[0] + temp[1], temp);
                    nroLINEAS++;
                    if (temp[0].StartsWith("LINEA")) li++;
                    if (temp[0].StartsWith("ITEM")) it++;
                    if (temp[0].StartsWith("DOCUMENTO-AFECTADO")) it++;
                    if (temp[0].StartsWith("REFERENCIA")) re++;
                    if (temp[0].StartsWith("ANTICIPO")) an++;

                    if (temp[0].StartsWith("LEYENDA")) ley++;

                    if (temp[0].StartsWith("EXTRAS")) ex++;
                }
                else
                {
                    fDict.Add(temp[0], temp);
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

                                    if (temp[0].StartsWith("DOCUMENTO-AFECTADO") || temp[0].StartsWith("ITEM") || temp[0].StartsWith("LINEA") || temp[0].StartsWith("REFERENCIA") || temp[0].StartsWith("ANTICIPO") || temp[0].StartsWith("EXTRAS") || temp[0].StartsWith("LEYENDA"))
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

            //List<ERegex> emp = new List<ERegex>();
            //emp = Validaciones.Where(o => o._USE == 0 && o.DOC.Contains(td)).ToList();
            //if (emp.Count > 0)
            //{
            //    foreach (ERegex valid in emp)
            //    {
            //        listerror.Add(valid.NOM + "-" + valid.MSG);
            //    }
            //    //valido = false;
            //}
            ////return valido;



            // Busqueda de los elemento que no se encuentran en la interface:
            List<ERegex> emp = new List<ERegex>();
            //emp = Validaciones.Where(o => o._USE == 0 && o.DOC.Contains(td)).ToList();
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
