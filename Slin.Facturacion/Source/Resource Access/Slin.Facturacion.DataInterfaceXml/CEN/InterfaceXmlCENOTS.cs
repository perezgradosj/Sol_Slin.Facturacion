using Slin.Facturacion.BusinessEntities;
using Slin.Facturacion.BusinessEntities.Helper;
using Slin.Facturacion.BusinessEntities.InterfaceXMLNOTE;
using Slin.Facturacion.Common;
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

namespace Slin.Facturacion.DataInterfaceXml.CEN
{
    public class InterfaceXmlCENOTS
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

        private string GetPathDirectoty(string RucEntityForPath, string NameConstante)
        {
            return ConfigurationManager.AppSettings[RucEntityForPath + "_" + NameConstante].ToString();
        }


        List<string> listaInterface = new List<string>();
        List<string> listaError = new List<string>();

        public string nombreArchivoXmlInterface = string.Empty;
        public string itemListaRegistro = string.Empty;

        string PathCompany = string.Empty;
        string Path_TologConsult = string.Empty;
        string Path_ToWriteInterfXml = string.Empty;
        public bool ResultOfSerialize = false;
        public string nombreArchivoXmlInterfaceError = string.Empty;


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

        public string GetObjInterfaceXML_CENOTE(string xmlLine)
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
                xmlLine = xmlLine.Replace("CreditNote", "NoteClass");
                xmlLine = xmlLine.Replace("DebitNote", "NoteClass");

                #region all process

                string TPO_CE = GetObjHeaderXML(xmlLine); //serializa el xml recibido

                if (ResultOfSerialize == true)
                {
                    #region IniciaValidaciones de Correspondencia

                    if (!ListEntityDesencrypt.Contains(ObjectEmpresa.RUC))
                    {
                        #region SI EL DOCUMENTO RECIBIDO NO ESTÁ HABILITADO PARA EL COSNUMO DE ESTA WEB SERVICE

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
                        #region SI EL DOCUMENTO RECIBIDO ESTÁ HABILITADO PARA EL COSNUMO DE ESTA WEB SERVICE

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

                            List<string> exts = new List<string>();
                            foreach (var doc in listaInterface)
                            {
                                if (doc.Contains("DOCUMENTO-AFECTADO"))
                                {
                                    exts.Add(doc);
                                    break;
                                }
                            }

                            if (exts.Count > 0) // SI LA NOTA CONTIENE DOCUMENTOS AFECTADOS
                            {
                                Documento existe = new Documento();
                                existe = new ServicioFacturacionSOA().ValidarExisteDoc_WS(nombreArchivoXmlInterface, TPO_CE); // SE VALIDA SI EL DOCUMENTO YA ESTÁ EN BD

                                if (existe.Serie != null && existe.Serie.Length > 0)
                                {
                                    #region SI EL DOCUMENTO ESTA EN BD

                                    WriteLog_Service(Constantes.Value_DateToLog + "El Doc. Ya esta registrado en la BD.");
                                    WriteLog_Service(Constantes.MsjEndProcessDoc + nombreArchivoXmlInterface);
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

                                    #region CASE TPO_CE
                                    switch (TPO_CE)
                                    {
                                        case Constantes.NotaCredito:
                                            { xmlLine = xmlLine.Replace("NoteClass", "CreditNote"); break; }
                                        case Constantes.NotaDebito:
                                            { xmlLine = xmlLine.Replace("NoteClass", "DebitNote"); break; }
                                    }
                                    #endregion

                                    xmlDoc.InnerXml = xmlLine;
                                    xmlDoc.Save(Path_ToWriteInterfXml + nombreArchivoXmlInterface + ".xml");

                                    WriteLog_Service(Constantes.Value_DateToLog + "Se escribio archivo Txt : " + PathInterfaceTxT + nombreArchivoXmlInterface + ".txt");
                                    WriteLog_Service(Constantes.Value_DateToLog + "Se guardo el Doc. Path  : " + Path_ToWriteInterfXml + nombreArchivoXmlInterface + ".xml");
                                    WriteLog_Service(Constantes.Value_DateToLog + "El Doc. Se a validado Correctamente!");
                                    WriteLog_Service(Constantes.MsjEndProcessDoc + nombreArchivoXmlInterface);
                                    WriteLog_Service(Constantes.MsjEnd);

                                    return msj = "El Documento Nro: " + nombreArchivoXmlInterface + "| Se valido Correctamente";
                                    #endregion
                                }


                            }
                            else
                            {
                                msj = "El Documento: " + nombreArchivoXmlInterface + " debe contener por lo menos un DOC AFECTADO";

                                WriteLog_Service(Constantes.Value_DateToLog + "El Doc. debe contener por lo menos un Doc. Afectado");
                                WriteLog_Service(Constantes.Value_DateToLog + "El Doc. no se a validado.");

                                WriteLog_Service(Constantes.MsjEndProcessDoc + nombreArchivoXmlInterface);
                                WriteLog_Service(Constantes.MsjEnd);
                                return msj;
                            }
                            #endregion
                        }

                        #endregion
                    }
                    #endregion End IniciaValidaciones de Correspondencia
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
                        //msj = "Hay Errores en Algunos de los TAG";
                        msj = messageErrorwhenDeserialize;
                    }
                    #endregion END OTHER ERROR
                    //return msj;
                }

                #endregion

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
                //XmlSerializer xmlSerial = new XmlSerializer(typeof(NoteClass));
                //sr = new StreamReader(filename);
                ////sr = new StreamReader(filename);
                //NoteClass note = (NoteClass)xmlSerial.Deserialize(sr);


                var note = new NoteClass();
                XmlSerializer xmlSerial = new XmlSerializer(typeof(NoteClass));
                using (TextReader reader = new StringReader(xml_line))
                {
                    note = (NoteClass)xmlSerial.Deserialize(reader);
                }

                //Encoding enco = Encoding.GetEncoding("iso-8859-1");
                //NoteClass note = new NoteClass();
                //using (StreamReader streamReader = new StreamReader(filename, enco, true))
                //{
                //    XmlSerializer serializer = new XmlSerializer(typeof(NoteClass));
                //    //obj = serializer.Deserialize(streamReader);
                //    note = (NoteClass)serializer.Deserialize(streamReader);
                //}


                #region

                //NoteClass note = new NoteClass();
                //var xml = new XmlSerializer(typeof(NoteClass));

                //var fname = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
                //var appendMode = false;
                //var encoding = Encoding.GetEncoding("ISO-8859-1");

                //using (StreamWriter sw = new StreamWriter(fname, appendMode, encoding))
                //{
                //    xml.Serialize(sw, note);
                //}


                #endregion

                string guion = "-";
                string lineaCE = "CE|";

                #region CE - LIST

                if (note.CE.ID != null)
                {
                    lineaCE += note.CE.ID;
                }
                else
                {
                    lineaCE += note.CE.ID;
                }

                #region CABECERA PRINCIPAL
                string lineaPrincipal = "CABECERA-PRINCIPAL|";
                if (note.CABECERAPRINCIPAL != null)
                {
                    if (note.CABECERAPRINCIPAL.TipoCE != null)
                    {
                        lineaPrincipal += note.CABECERAPRINCIPAL.TipoCE + "|";
                    }
                    //else if (note.CABECERAPRINCIPAL.TipoCE == null)
                    //{
                    //    msjCABECERAPRINCIPAL = "Existen Errores en el TAG CABECERAPRINCIPAL";
                    //}
                    else
                    {
                        lineaPrincipal += guion + "|";
                    }

                    if (note.CABECERAPRINCIPAL.ID_CE != null)
                    {
                        lineaPrincipal += note.CABECERAPRINCIPAL.ID_CE + "|";
                    }
                    else
                    {
                        lineaPrincipal += guion + "|";
                    }

                    if (note.CABECERAPRINCIPAL.Toperacion != null)
                    {
                        lineaPrincipal += note.CABECERAPRINCIPAL.Toperacion + "|";
                    }
                    else
                    {
                        lineaPrincipal += guion + "|";
                    }

                    if (note.CABECERAPRINCIPAL.FEmision != null)
                    {
                        lineaPrincipal += note.CABECERAPRINCIPAL.FEmision + "|";
                    }
                    else
                    {
                        lineaPrincipal += guion + "|";
                    }

                    if (note.CABECERAPRINCIPAL.TMoneda != null)
                    {
                        lineaPrincipal += note.CABECERAPRINCIPAL.TMoneda;
                    }
                    else
                    {
                        lineaPrincipal += guion;
                    }

                    //listaInterface.Add("CABECERA-PRINCIPAL|" + note.CABECERAPRINCIPAL.TipoCE + "|" + note.CABECERAPRINCIPAL.ID_CE + "|" + note.CABECERAPRINCIPAL.TipoCE + "|" + note.CABECERAPRINCIPAL.FEmision + "|" + note.CABECERAPRINCIPAL.TMoneda);
                }
                else
                {
                    msjCABECERAPRINCIPAL = "Existen Errores en el TAG CABECERAPRINCIPAL";
                }
                #endregion END CABECERA PRINCIPAL


                #region CABECERA NOTA
                string lineaCABECERANOTA = "CABECERA-NOTA|";
                if (note.CABECERANOTA != null)
                {
                    if (note.CABECERANOTA.TpoNota != null)
                    {
                        lineaCABECERANOTA += note.CABECERANOTA.TpoNota + "|";
                    }
                    else
                    {
                        lineaCABECERANOTA += guion + "|";
                    }

                    if (note.CABECERANOTA.MotivoNota != null)
                    {
                        lineaCABECERANOTA += note.CABECERANOTA.MotivoNota;
                    }
                    else
                    {
                        lineaCABECERANOTA += guion;
                    }
                }

                #endregion


                #region CABECERA CABECERAEMISOR
                string lineaCABECERAEMISOR = "CABECERA-EMISOR|";
                if (note.CABECERAEMISOR != null)
                {
                    if (note.CABECERAEMISOR.EmiTpoDoc != null)
                    {
                        lineaCABECERAEMISOR += note.CABECERAEMISOR.EmiTpoDoc + "|";
                    }
                    //else if (note.CABECERAEMISOR.EmiTpoDoc == null)
                    //{
                    //    msjCABCABECERAEMISOR = "Existen Errores en el TAG CABECERACABECERAEMISOR";
                    //}
                    else
                    {
                        lineaCABECERAEMISOR += guion + "|";
                    }

                    if (note.CABECERAEMISOR.EmiNumDoc != null) lineaCABECERAEMISOR += note.CABECERAEMISOR.EmiNumDoc + "|"; else lineaCABECERAEMISOR += guion + "|";

                    if (note.CABECERAEMISOR.EmiNombre != null) lineaCABECERAEMISOR += note.CABECERAEMISOR.EmiNombre + "|"; else lineaCABECERAEMISOR += guion + "|";

                    if (note.CABECERAEMISOR.EmiNComer != null) lineaCABECERAEMISOR += note.CABECERAEMISOR.EmiNComer + "|"; else lineaCABECERAEMISOR += guion + "|";

                    if (note.CABECERAEMISOR.EmiUbigeo != null) lineaCABECERAEMISOR += note.CABECERAEMISOR.EmiUbigeo + "|"; else lineaCABECERAEMISOR += guion + "|";

                    if (note.CABECERAEMISOR.EmiDirFiscal != null) lineaCABECERAEMISOR += note.CABECERAEMISOR.EmiDirFiscal + "|"; else lineaCABECERAEMISOR += guion + "|";

                    if (note.CABECERAEMISOR.EmiDirUrbani != null) lineaCABECERAEMISOR += note.CABECERAEMISOR.EmiDirUrbani + "|"; else lineaCABECERAEMISOR += guion + "|";

                    if (note.CABECERAEMISOR.EmiDirProvin != null) lineaCABECERAEMISOR += note.CABECERAEMISOR.EmiDirProvin + "|"; else lineaCABECERAEMISOR += guion + "|";

                    if (note.CABECERAEMISOR.EmiDirDepart != null) lineaCABECERAEMISOR += note.CABECERAEMISOR.EmiDirDepart + "|"; else lineaCABECERAEMISOR += guion + "|";

                    if (note.CABECERAEMISOR.EmiDirDistrito != null) lineaCABECERAEMISOR += note.CABECERAEMISOR.EmiDirDistrito + "|"; else lineaCABECERAEMISOR += guion + "|";

                    if (note.CABECERAEMISOR.EmiCodPais != null) lineaCABECERAEMISOR += note.CABECERAEMISOR.EmiCodPais; else lineaCABECERAEMISOR += guion;
                }
                else
                {
                    msjCABCABECERAEMISOR = "Existen Errores en el TAG CABECERA EMISOR";
                }

                #endregion END CABECERA CABECERAEMISOR

                #region CABECERA CABECERARECEPTOR
                string lineaCABECERARECEPTOR = "CABECERA-RECEPTOR|";
                if (note.CABECERARECEPTOR != null)
                {

                    if (note.CABECERARECEPTOR.RecTpoDoc != null && note.CABECERARECEPTOR.RecTpoDoc.Length > 0)
                    {
                        lineaCABECERARECEPTOR += note.CABECERARECEPTOR.RecTpoDoc + "|";
                    }
                    //else if (note.CABECERARECEPTOR.RecTpoDoc == null)
                    //{
                    //    msjCABCABECERARECEPTOR = "Existen Errores en el TAG CABECERACABECERARECEPTOR";
                    //}
                    else
                    {
                        lineaCABECERARECEPTOR += "0" + "|";
                    }

                    if (note.CABECERARECEPTOR.RecNumDocu != null) lineaCABECERARECEPTOR += note.CABECERARECEPTOR.RecNumDocu + "|"; else lineaCABECERARECEPTOR += guion + "|";

                    if (note.CABECERARECEPTOR.RecNombre != null) lineaCABECERARECEPTOR += note.CABECERARECEPTOR.RecNombre + "|"; else lineaCABECERARECEPTOR += guion + "|";

                    if (note.CABECERARECEPTOR.RecNComer != null) lineaCABECERARECEPTOR += note.CABECERARECEPTOR.RecNComer + "|"; else lineaCABECERARECEPTOR += "|";

                    if (note.CABECERARECEPTOR.RecUbigeo != null) lineaCABECERARECEPTOR += note.CABECERARECEPTOR.RecUbigeo + "|"; else lineaCABECERARECEPTOR += "|";

                    if (note.CABECERARECEPTOR.RecDirFiscal != null) lineaCABECERARECEPTOR += note.CABECERARECEPTOR.RecDirFiscal + "|"; else lineaCABECERARECEPTOR += "|";
                    //if (note.CABECERARECEPTOR.RecDirFiscal != null) lineaCABECERARECEPTOR += note.CABECERARECEPTOR.RecDirFiscal + "|"; else lineaCABECERARECEPTOR += guion;

                    if (note.CABECERARECEPTOR.RecDirUrbani != null) lineaCABECERARECEPTOR += note.CABECERARECEPTOR.RecDirUrbani + "|"; else lineaCABECERARECEPTOR += "|";

                    if (note.CABECERARECEPTOR.RecDirProvin != null) lineaCABECERARECEPTOR += note.CABECERARECEPTOR.RecDirProvin + "|"; else lineaCABECERARECEPTOR += "|";

                    if (note.CABECERARECEPTOR.RecDirDepart != null) lineaCABECERARECEPTOR += note.CABECERARECEPTOR.RecDirDepart + "|"; else lineaCABECERARECEPTOR += "|";

                    if (note.CABECERARECEPTOR.RecDirDistrito != null) lineaCABECERARECEPTOR += note.CABECERARECEPTOR.RecDirDistrito + "|"; else lineaCABECERARECEPTOR += "|";

                    if (note.CABECERARECEPTOR.RecCodPais != null) lineaCABECERARECEPTOR += note.CABECERARECEPTOR.RecCodPais; else lineaCABECERARECEPTOR += "|";
                    //if (note.CABECERARECEPTOR.RecCodPais != null) lineaCABECERARECEPTOR += note.CABECERARECEPTOR.RecCodPais; else lineaCABECERARECEPTOR += guion;

                    if (note.CABECERARECEPTOR.LugUbigeo != null) lineaCABECERARECEPTOR += note.CABECERARECEPTOR.LugUbigeo + "|"; else lineaCABECERARECEPTOR += "|";
                    if (note.CABECERARECEPTOR.LugDirFiscal != null) lineaCABECERARECEPTOR += note.CABECERARECEPTOR.LugDirFiscal + "|"; else lineaCABECERARECEPTOR += "|";
                    if (note.CABECERARECEPTOR.LugDirUrbani != null) lineaCABECERARECEPTOR += note.CABECERARECEPTOR.LugDirUrbani + "|"; else lineaCABECERARECEPTOR += "|";
                    if (note.CABECERARECEPTOR.LugDirProvin != null) lineaCABECERARECEPTOR += note.CABECERARECEPTOR.LugDirProvin + "|"; else lineaCABECERARECEPTOR += "|";
                    if (note.CABECERARECEPTOR.LugDirDepart != null) lineaCABECERARECEPTOR += note.CABECERARECEPTOR.LugDirDepart + "|"; else lineaCABECERARECEPTOR += "|";
                    if (note.CABECERARECEPTOR.LugDirDistrito != null) lineaCABECERARECEPTOR += note.CABECERARECEPTOR.LugDirDistrito + "|"; else lineaCABECERARECEPTOR += "|";
                    if (note.CABECERARECEPTOR.LugCodPais != null) lineaCABECERARECEPTOR += note.CABECERARECEPTOR.LugCodPais + "|"; else lineaCABECERARECEPTOR += "|";

                    if (note.CABECERARECEPTOR.LocalAnexo != null) lineaCABECERARECEPTOR += note.CABECERARECEPTOR.LocalAnexo + "|"; else lineaCABECERARECEPTOR += guion;
                }
                else
                {
                    msjCABCABECERARECEPTOR = msjCABCABECERARECEPTOR = "Existen Errores en el TAG CABECERACABECERARECEPTOR";
                }

                #endregion END CABECERA CABECERARECEPTOR



                #region TOTAL
                string lineaTOTAL = string.Empty;

                if (note.TOTAL != null)
                {
                    lineaTOTAL = "TOTAL|";
                    if (note.TOTAL.TotVtaGrab != null)
                    {
                        lineaTOTAL += note.TOTAL.TotVtaGrab + "|";
                    }
                    else if (note.TOTAL.TotVtaGrab == null)
                    {
                        msjTOTAL = "Existes Campos que son Necesarios en el TAG TOTAL";
                    }
                    else
                    {
                        lineaTOTAL += guion + "|";
                    }

                    if (note.TOTAL.TotVtaInaf != null) lineaTOTAL += note.TOTAL.TotVtaInaf + "|"; else lineaTOTAL += guion + "|";

                    if (note.TOTAL.TotVtaExon != null) lineaTOTAL += note.TOTAL.TotVtaExon + "|"; else lineaTOTAL += guion + "|";

                    //if (note.TOTAL.TotVtaGrat != null) lineaTOTAL += note.TOTAL.TotVtaGrat + "|"; else lineaTOTAL += guion + "|";

                    //if (note.TOTAL.TotTotDscto != null) lineaTOTAL += note.TOTAL.TotTotDscto + "|"; else lineaTOTAL += guion + "|";


                    if (note.TOTAL.TotSumIGV != null)
                    {
                        lineaTOTAL += note.TOTAL.TotSumIGV + "|";
                    }
                    else if (note.TOTAL.TotSumIGV == null)
                    {
                        msjTOTAL = "Existes Campos que son Necesarios en el TAG TOTAL";
                    }
                    else
                    {
                        lineaTOTAL += guion + "|";
                    }


                    if (note.TOTAL.TotSumISC != null) lineaTOTAL += note.TOTAL.TotSumISC + "|"; else lineaTOTAL += guion + "|";

                    if (note.TOTAL.TotSumOTrib != null) lineaTOTAL += note.TOTAL.TotSumOTrib + "|"; else lineaTOTAL += guion + "|";

                    //if (note.TOTAL.TotDctoGlobal != null) lineaTOTAL += note.TOTAL.TotDctoGlobal + "|"; else lineaTOTAL += guion + "|";

                    if (note.TOTAL.TotSumOCargo != null) lineaTOTAL += note.TOTAL.TotSumOCargo + "|"; else lineaTOTAL += guion + "|";

                    //if (note.TOTAL.TotAnticipo != null) lineaTOTAL += note.TOTAL.TotAnticipo + "|"; else lineaTOTAL += guion + "|";

                    if (note.TOTAL.TotImporTotal != null)
                    {
                        lineaTOTAL += note.TOTAL.TotImporTotal + "|";
                    }
                    else if (note.TOTAL.TotImporTotal == null)
                    {
                        msjTOTAL = "Existes Campos que son Necesarios en el TAG TOTAL";
                    }
                    else
                    {
                        lineaTOTAL += guion + "|";
                    }

                    if (note.TOTAL.MontoLiteral != null) lineaTOTAL += note.TOTAL.MontoLiteral; else lineaTOTAL += guion;
                }
                else
                {
                    msjTOTAL = "No existe el TAG TOTAL o es Incorrecto";
                }

                #endregion END TOTAL



                #region LINEAS

                List<string> listaLinea = new List<string>();

                if (note.LINEAS != null)
                {
                    if (note.LINEAS.LINEA.Count > 0)
                    {
                        if (note.LINEAS.LINEA[0].LnNroOrden != null && note.LINEAS.LINEA[0].LnUndMed != null && note.LINEAS.LINEA[0].LnCantidad != null && note.LINEAS.LINEA[0].LnDescrip != null && note.LINEAS.LINEA[0].LnValUnit != null &&
                            note.LINEAS.LINEA[0].LnMntPrcVta != null && note.LINEAS.LINEA[0].LnValVta != null && note.LINEAS.LINEA[0].LnMntIGV != null && note.LINEAS.LINEA[0].LnCodAfectIGV != null)
                        //note.LINEAS.LINEA[0].LnMntPrcVta != null && note.LINEAS.LINEA[0].LnValVta != null && note.LINEAS.LINEA[0].LnMntIGV != null && note.LINEAS.LINEA[0].LnCodAfectIGV != null && note.LINEAS.LINEA[0].LnPorcIGV != null)
                        {
                            for (int i = 0; i <= note.LINEAS.LINEA.Count - 1; i++)
                            {
                                string codProduc = note.LINEAS.LINEA[i].LnCodProd;

                                if (codProduc != null && codProduc.Length > 0)
                                {
                                    listaLinea.Add((i + 1) + "|" + note.LINEAS.LINEA[i].LnUndMed + "|" + note.LINEAS.LINEA[i].LnCantidad + "|" + note.LINEAS.LINEA[i].LnCodProd + "|" + note.LINEAS.LINEA[i].LnDescrip + "|" + note.LINEAS.LINEA[i].LnValUnit + "|" +
                                        note.LINEAS.LINEA[i].LnMntPrcVta + "|" + note.LINEAS.LINEA[i].LnValVta + "|" + note.LINEAS.LINEA[i].LnMntIGV + "|" + note.LINEAS.LINEA[i].LnCodAfectIGV + "|" +
                                        note.LINEAS.LINEA[i].LnMntISC + "|" + note.LINEAS.LINEA[i].LnCodSisISC);
                                        //note.LINEAS.LINEA[i].LnMntISC + "|" + note.LINEAS.LINEA[i].LnCodSisISC + "|" + note.LINEAS.LINEA[i].LnPorcIGV);
                                }
                                else
                                {
                                    listaLinea.Add((i + 1) + "|" + note.LINEAS.LINEA[i].LnUndMed + "|" + note.LINEAS.LINEA[i].LnCantidad + "|" + string.Empty + "|" + note.LINEAS.LINEA[i].LnDescrip + "|" + note.LINEAS.LINEA[i].LnValUnit + "|" +
                                        note.LINEAS.LINEA[i].LnMntPrcVta + "|" + note.LINEAS.LINEA[i].LnValVta + "|" + note.LINEAS.LINEA[i].LnMntIGV + "|" + note.LINEAS.LINEA[i].LnCodAfectIGV + "|" +
                                        note.LINEAS.LINEA[i].LnMntISC + "|" + note.LINEAS.LINEA[i].LnCodSisISC);
                                        //note.LINEAS.LINEA[i].LnMntISC + "|" + note.LINEAS.LINEA[i].LnCodSisISC + "|" + note.LINEAS.LINEA[i].LnPorcIGV);
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

                
                #region DOCUMENTO AFECTADO
                List<string> listaDocAfectado = new List<string>();
                if (note.DOCAFECTADO != null)
                {
                    if (note.DOCAFECTADO.DOCUMENTO.Count > 0)
                    {
                        if (note.DOCAFECTADO.DOCUMENTO[0].DocID != null && note.DOCAFECTADO.DOCUMENTO[0].DocTpoDoc != null)
                        {
                            for (int i = 0; i <= note.DOCAFECTADO.DOCUMENTO.Count - 1; i++)
                            {
                                listaDocAfectado.Add("DOCUMENTO-AFECTADO|" + (i + 1) + "|" + note.DOCAFECTADO.DOCUMENTO[i].DocID + "|" + note.DOCAFECTADO.DOCUMENTO[i].DocTpoDoc);
                            }
                        }
                    }
                }




                #endregion END DOCUMENTO AFECTADO


                #region REFERENCIA

                List<string> listaReferencia = new List<string>();
                if (note.REFERENCIAS != null)
                {
                    if (note.REFERENCIAS.REFERENCIA.Count > 0)
                    {
                        if (note.REFERENCIAS.REFERENCIA[0].RefID != null && note.REFERENCIAS.REFERENCIA[0].RefTpoDoc != null)
                        {
                            for (int i = 0; i <= note.REFERENCIAS.REFERENCIA.Count - 1; i++)
                            {
                                listaReferencia.Add((i + 1) + "|" + note.REFERENCIAS.REFERENCIA[i].RefID + "|" + note.REFERENCIAS.REFERENCIA[i].RefTpoDoc);
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
                if (note.ANTICIPOS != null)
                {
                    if (note.ANTICIPOS.ANTICIPO.Count > 0)
                    {
                        if (note.ANTICIPOS.ANTICIPO[0].AntMonto != null && note.ANTICIPOS.ANTICIPO[0].AntTpoDocAnt != null && note.ANTICIPOS.ANTICIPO[0].AntIdDocAnt != null &&
                        note.ANTICIPOS.ANTICIPO[0].AntNumDocEmi != null && note.ANTICIPOS.ANTICIPO[0].AntTpoDocEmi != null)
                        {
                            for (int i = 0; i <= note.ANTICIPOS.ANTICIPO.Count - 1; i++)
                            {
                                listaAnticipo.Add((i + 1) + "|" + note.ANTICIPOS.ANTICIPO[i].AntMonto + "|" + note.ANTICIPOS.ANTICIPO[i].AntTpoDocAnt + "|" +
                                    note.ANTICIPOS.ANTICIPO[i].AntIdDocAnt + "|" + note.ANTICIPOS.ANTICIPO[i].AntNumDocEmi + "|" + note.ANTICIPOS.ANTICIPO[i].AntTpoDocEmi);
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
                if (note.DETRACCION != null)
                {

                    //lineaDETRACCION = "DETRACCION|";
                    //if (note.DETRACCION.DetValBBSS != null) lineaDETRACCION += note.DETRACCION.DetValBBSS + "|"; else lineaDETRACCION += guion + "|";
                    //if (note.DETRACCION.DetCtaBN != null) lineaDETRACCION += note.DETRACCION.DetCtaBN + "|"; else lineaDETRACCION += guion + "|";
                    //if (note.DETRACCION.DetPorcent != null) lineaDETRACCION += note.DETRACCION.DetPorcent + "|"; else lineaDETRACCION += guion + "|";
                    //if (note.DETRACCION.DetMonto != null) lineaDETRACCION += note.DETRACCION.DetMonto + "|"; else lineaDETRACCION += guion;

                    if (note.DETRACCION.DetValBBSS != null && note.DETRACCION.DetValBBSS.Length > 0)
                    {
                        if (note.DETRACCION.DetCtaBN != null && note.DETRACCION.DetCtaBN.Length > 0)
                        {
                            lineaDETRACCION = "DETRACCION|";
                            if (note.DETRACCION.DetValBBSS != null) lineaDETRACCION += note.DETRACCION.DetValBBSS + "|"; else lineaDETRACCION += guion + "|";
                            if (note.DETRACCION.DetCtaBN != null) lineaDETRACCION += note.DETRACCION.DetCtaBN + "|"; else lineaDETRACCION += guion + "|";
                            if (note.DETRACCION.DetPorcent != null) lineaDETRACCION += note.DETRACCION.DetPorcent + "|"; else lineaDETRACCION += guion + "|";
                            if (note.DETRACCION.DetMonto != null) lineaDETRACCION += note.DETRACCION.DetMonto + "|"; else lineaDETRACCION += guion;
                        }
                        else
                        {
                            msjDETRACCION += "Existen Campos Obligatorios que son Incorrectos en el TAG DETRACCION";
                        }
                    }

                    //if (note.DETRACCION.DetValBBSS == null && note.DETRACCION.DetCtaBN == null)
                    //{
                    //    msjDETRACCION += "Existen Campos Obligatorios que son Incorrectos en el TAG DETRACCION";
                    //}
                    //else if (note.DETRACCION.DetValBBSS.Length == 0 && note.DETRACCION.DetCtaBN.Length == 0)
                    //{
                    //    msjDETRACCION += "Existen Campos Obligatorios que son Incorrectos en el TAG DETRACCION";
                    //}
                    //else
                    //{
                    //    lineaDETRACCION = "DETRACCION|";
                    //    lineaDETRACCION += note.DETRACCION.DetValBBSS + "|";

                    //    if (note.DETRACCION.DetCtaBN != null) lineaDETRACCION += note.DETRACCION.DetCtaBN + "|"; else lineaDETRACCION += guion + "|";
                    //    if (note.DETRACCION.DetPorcent != null) lineaDETRACCION += note.DETRACCION.DetPorcent + "|"; else lineaDETRACCION += guion + "|";
                    //    if (note.DETRACCION.DetMonto != null) lineaDETRACCION += note.DETRACCION.DetMonto + "|"; else lineaDETRACCION += guion;
                    //}
                }

                #endregion END DETRACCION





                #region LEYENDA

                List<string> listaLEYENDA = new List<string>();
                if (note.LEYENDAS != null)
                {
                    if (note.LEYENDAS.LEYENDA.Count > 0)
                    {

                        if (note.LEYENDAS.LEYENDA[0].LeyCodigo != null && note.LEYENDAS.LEYENDA[0].LeyDescrip != null)
                        {
                            for (int i = 0; i <= note.LEYENDAS.LEYENDA.Count - 1; i++)
                            {
                                listaLEYENDA.Add((i + 1) + "|" + note.LEYENDAS.LEYENDA[i].LeyCodigo + "|" + note.LEYENDAS.LEYENDA[i].LeyDescrip);
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
                if (note.EXTRAS != null)
                {
                    if (note.EXTRAS.EXTRA.Count > 0)
                    {
                        if (note.EXTRAS.EXTRA[0].ExID != null && note.EXTRAS.EXTRA[0].ExDato != null && note.EXTRAS.EXTRA[0].ExTpoDato != null)
                        {
                            for (int i = 0; i <= note.EXTRAS.EXTRA.Count - 1; i++)
                            {
                                listaExtras.Add((i + 1) + "|" + note.EXTRAS.EXTRA[i].ExDato + "|" + note.EXTRAS.EXTRA[i].ExTpoDato);
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
                if (note.MAIL != null)
                {
                    lineaCorreo = "MAIL|";
                    if (note.MAIL.Para != null) lineaCorreo += note.MAIL.Para + "|"; else lineaCorreo += guion + "|";

                    if (note.MAIL.CC != null) lineaCorreo += note.MAIL.CC + "|"; else lineaCorreo += guion + "|";

                    if (note.MAIL.CCO != null) lineaCorreo += note.MAIL.CCO; else lineaCorreo += guion;

                    msjMAIL += "Existen Campos Obligatorios que son Incorrectos";
                }
                else
                {
                    msjMAIL += "No existe el TAG CORREO o es Incorrecto";
                }

                #endregion END MAIL

                #region ADICIONAL

                string lineaAdicional = string.Empty;
                if (note.ADICIONAL != null)
                {
                    lineaAdicional = "ADICIONAL|";
                    if (note.ADICIONAL.Sede != null) lineaAdicional += note.ADICIONAL.Sede + "|"; else lineaAdicional += guion + "|";
                    if (note.ADICIONAL.Usuario != null) lineaAdicional += note.ADICIONAL.Usuario + "|"; else lineaAdicional += guion + "|";
                    if (note.ADICIONAL.Impresora != null) lineaAdicional += note.ADICIONAL.Impresora + "|"; else lineaAdicional += guion + "|";
                    if (note.ADICIONAL.Campo1 != null) lineaAdicional += note.ADICIONAL.Campo1 + "|"; else lineaAdicional += guion + "|";
                    if (note.ADICIONAL.Campo2 != null) lineaAdicional += note.ADICIONAL.Campo2 + "|"; else lineaAdicional += guion + "|";
                    if (note.ADICIONAL.Campo3 != null) lineaAdicional += note.ADICIONAL.Campo3 + "|"; else lineaAdicional += guion + "|";
                    if (note.ADICIONAL.Campo4 != null) lineaAdicional += note.ADICIONAL.Campo4 + "|"; else lineaAdicional += guion + "|";
                    if (note.ADICIONAL.Campo5 != null) lineaAdicional += note.ADICIONAL.Campo5 + "|"; else lineaAdicional += guion + "|";
                    if (note.ADICIONAL.Campo6 != null) lineaAdicional += note.ADICIONAL.Campo6 + "|"; else lineaAdicional += guion + "|";
                    if (note.ADICIONAL.Campo7 != null) lineaAdicional += note.ADICIONAL.Campo7 + "|"; else lineaAdicional += guion + "|";
                    if (note.ADICIONAL.Campo8 != null) lineaAdicional += note.ADICIONAL.Campo8 + "|"; else lineaAdicional += guion + "|";
                    if (note.ADICIONAL.Campo9 != null) lineaAdicional += note.ADICIONAL.Campo9 + "|"; else lineaAdicional += guion + "|";
                    if (note.ADICIONAL.Campo10 != null) lineaAdicional += note.ADICIONAL.Campo10; else lineaAdicional += guion;
                }
                else
                {
                    msjADICIONAL += "No existe el TAG ADICIONAL o es Incorrecto";
                }

                #endregion END ADICIONAL

                #region LISTA INTERFACE
                listaInterface.Add(lineaCE);
                listaInterface.Add(lineaCABECERANOTA);
                listaInterface.Add(lineaPrincipal);


                if (note.CABECERAPRINCIPAL.TipoCE != null)
                {
                    #region CASE TIPO CE

                    switch (note.CABECERAPRINCIPAL.TipoCE)
                    {
                        case "07":
                        case "08":
                            {
                                listaInterface.Add(lineaCABECERAEMISOR);
                                listaInterface.Add(lineaCABECERARECEPTOR);

                                listaInterface.Add(lineaTOTAL);

                                if (listaLinea.Count > 0)
                                {
                                    foreach (var li in listaLinea)
                                    {
                                        listaInterface.Add("LINEA|" + li);
                                    }
                                }

                                if (listaDocAfectado.Count > 0)
                                {
                                    foreach (var afec in listaDocAfectado)
                                    {
                                        listaInterface.Add(afec);
                                    }
                                }

                                if (listaReferencia.Count > 0)
                                {
                                    foreach (var refe in listaReferencia)
                                    {
                                        listaInterface.Add("REFERENCIA|" + refe);
                                    }
                                }

                                if (lineaDETRACCION.Length > 12)
                                {
                                    listaInterface.Add(lineaDETRACCION);
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

                if (note.CABECERAEMISOR != null && note.CE != null && note.CABECERAPRINCIPAL != null)
                {
                    if (note.CABECERAPRINCIPAL.TipoCE != null && note.CABECERAEMISOR.EmiNumDoc != null && note.CE.ID != null && note.CABECERAPRINCIPAL.ID_CE != null)
                    {
                        ObjectEmpresa.RUC = note.CABECERAEMISOR.EmiNumDoc;

                        //entityRUCId2 = note.CABECERAEMISOR.EmiNumDoc;
                        //entityRUCId1 = note.CE.ID.Substring(0, 11);

                        //IdDocumentoReceib = note.CE.ID;
                        //IdDocumentoCreated = note.CABECERAEMISOR.EmiNumDoc + "-" + note.CABECERAPRINCIPAL.TipoCE + "-" + note.CABECERAPRINCIPAL.ID_CE;

                        TPO_CE = note.CABECERAPRINCIPAL.TipoCE;
                        nombreArchivoXmlInterface = note.CE.ID;
                    }
                }

                #endregion

                itemListaRegistro = "[" + DateTime.Now + "]" + nombreArchivoXmlInterface + " " + "Recibido Correctamente.";

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
