using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Low = Slin.Facturacion.DataInterfaceXml.Serialize;

using Slin.Facturacion.BusinessEntities;
using Slin.Facturacion.ServiceImplementation;
using Slin.Facturacion.DataAccess.Helper;
using Slin.Facturacion.Common;

namespace Slin.Facturacion.DataInterfaceXml.LOW
{
    public class DocumentHelper
    {
        static string PathLogSLINADE = Conexion.Cadena; //log para escribir cuando el documento no pertenezca a ninguna empresa licenciada

        static string PathLog_NotLicense = Conexion.Cadena_Nl;

        private void CreateDirectory(string Path)
        {
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
        }
        
        string PathCompany = string.Empty;
        string Path_TologConsult = string.Empty;

        private void WriteLog_Service(string msje)
        {
            using (StreamWriter sw = new StreamWriter(Path_TologConsult, true, Encoding.UTF8))
            {
                sw.WriteLine(msje);
            }
        }

        private void WriteLog_NotLicense(string msje)
        {
            using (StreamWriter sw = new StreamWriter(PathLog_NotLicense + "ws_low_doc_nl.log", true, Encoding.UTF8))
            {
                sw.WriteLine(msje);
            }
        }

        private void WriteLog_Process(string msje)
        {
            using (StreamWriter sw = new StreamWriter(PathLogSLINADE + @"Fact_Interface_Low.log", true, Encoding.UTF8))
            {
                sw.WriteLine(msje);
            }
        }


        public string SendDocumentVoided(string xmlLine)
        {
            string rpta = string.Empty;

            try
            {
                string result = LeerDocumentVoidedXML(xmlLine);

                CreateDirectory(PathLog_NotLicense);
                CreateDirectory(PathLogSLINADE);

                if (ItsOkay == true)
                {
                    rpta = result;
                    oDocExists = new Documento();
                    oDocExists = new ServicioFacturacionSOA().ValidarExisteDoc_WS(DocVoieded.ID, DocVoieded.TipoCE);


                    var listEntity = new BusinessSecurity.Entity.EntityClass().getsListEntity();

                    if (listEntity.Contains(DocVoieded.EmiNumDoc))
                    {
                        #region company with license
                        PathCompany = ConfigurationManager.AppSettings[DocVoieded.EmiNumDoc].ToString();

                        Path_TologConsult = PathCompany + @"Logs\" + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\ser_interf\";
                        CreateDirectory(Path_TologConsult);
                        Path_TologConsult = PathCompany + @"Logs\" + DateTime.Now.Year + "-" + string.Format("{0:00}", DateTime.Now.Month) + @"\ser_interf\ws_low_doc.log";

                        WriteLog_Service(" ");
                        WriteLog_Service(Constantes.MsjStart);
                        WriteLog_Service(Constantes.Msj_DocProcess + DocVoieded.ID);
                        WriteLog_Service(Constantes.Msj_TpoDoc + DocVoieded.TipoCE);

                        if (oDocExists.Serie != null && oDocExists.Serie.Length > 0)
                        {
                            LlenarObjetoVoieded();
                            var listaResult = new ServicioFacturacionSOA().ValidarExisteDocAnulado(oDocAnulado);

                            if (listaResult.Count > 0)
                            {
                                rpta = "El Documento Nro: " + DocVoieded.ID + " ya se Encuentra ¡Anulado!";
                                WriteLog_Service(Constantes.Value_DateToLog + "El Doc. ya se encuentra ¡Anulado!");
                            }
                            else
                            {
                                rpta = "El Documento Nro: " + DocVoieded.ID + " se a ";
                                rpta += new ServicioFacturacionSOA().InsertarDocumentoAnulado(oDocAnulado);
                                rpta = rpta.Replace("Registrado", "Anulado");
                                //WriteLog_Service(Constantes.Value_DateToLog + "El Doc. se a Anulado Correctamente.");
                                WriteLog_Service(Constantes.Value_DateToLog + rpta);
                                WriteLog_Service(Constantes.Msj_DocAnul_Registr);
                            }
                        }
                        else
                        {
                            rpta = "El Documento Nro: " + DocVoieded.ID + " no ¡Existe!";
                            WriteLog_Service(Constantes.Value_DateToLog + "El Doc. no ¡Existe!.");
                        }
                        WriteLog_Service(Constantes.MsjEndProcessDoc + DocVoieded.ID);
                        WriteLog_Service(Constantes.MsjEnd);

                        #endregion
                    }
                    else
                    {
                        #region ruc invalid

                        rpta = "El Ruc del Doc. enviado es Invalido : " + DocVoieded.EmiNumDoc;

                        WriteLog_NotLicense(" ");
                        WriteLog_NotLicense(Constantes.MsjStart);
                        WriteLog_NotLicense(Constantes.Msj_DocProcess + DocVoieded.ID);
                        WriteLog_NotLicense(Constantes.Msj_TpoDoc + DocVoieded.TipoCE);
                        WriteLog_NotLicense(Constantes.MsjRucInvalid + DocVoieded.EmiNumDoc);
                        WriteLog_NotLicense(Constantes.MsjEndProcessDoc + DocVoieded.ID);
                        WriteLog_NotLicense(Constantes.MsjEnd);

                        #endregion
                    }
                }
                else
                {
                    rpta = "Error en Documento Enviado.";
                    WriteLog_Process(" ");
                    WriteLog_Process(Constantes.MsjStart);
                    WriteLog_Process(Constantes.Msj_DocProcess + DocVoieded.ID);
                    WriteLog_Process(Constantes.Value_DateToLog + "Error en Documento Enviado.");
                    WriteLog_Process(Constantes.Msj_Error + result);
                    WriteLog_Process(Constantes.MsjEndProcessDoc + DocVoieded.ID);
                    WriteLog_Process(Constantes.MsjEnd);
                }
            }
            catch (Exception ex)
            {
            }
            return rpta;
        }


        private bool ItsOkay = false;
        private string LeerDocumentVoidedXML(string xml_line)
        {
            string result = string.Empty;

            try
            {
                //XmlSerializer xmlSerial = new XmlSerializer(typeof(Low.DocumentVoided));
                //sr = new StreamReader(filename);
                ////sr = new StreamReader(filename);
                //Low.DocumentVoided low = (Low.DocumentVoided)xmlSerial.Deserialize(sr);


                var low = new Low.DocumentVoided();
                XmlSerializer xmlSerial = new XmlSerializer(typeof(Low.DocumentVoided));
                using (TextReader reader = new StringReader(xml_line))
                {
                    low = (Low.DocumentVoided)xmlSerial.Deserialize(reader);
                }

                if (low.Serie != null && low.Serie.Length > 0 && low.Correlativo != null && low.Correlativo.Length > 0 && low.EmiNumDoc != null && low.EmiNumDoc.Length > 0 && low.TipoCE != null && low.TipoCE.Length > 0)
                {
                    DocVoieded = new Low.DocumentVoided();
                    DocVoieded.ID = low.EmiNumDoc + "-" + low.TipoCE + "-" + low.Serie + "-" + low.Correlativo;
                    DocVoieded.TipoCE = low.TipoCE;
                    DocVoieded.EmiNumDoc = low.EmiNumDoc;
                    DocVoieded.Motivo = low.Motivo;

                    result = DocVoieded.ID;
                    ItsOkay = true;
                }
                else
                {
                    result = "Error en el Documento enviado.";
                    ItsOkay = false;
                }

            }
            catch (Exception ex)
            {
                result = ex.Message;
                ItsOkay = false;
            }

            return result;
        }


        #region ENTITY
        Low.DocumentVoided DocVoieded = new Low.DocumentVoided();
        Documento oDocExists = new Documento();
        FacturaElectronica oDocAnulado = new FacturaElectronica();

        #endregion

        void LlenarObjetoVoieded()
        {
            oDocAnulado = new FacturaElectronica();
            oDocAnulado.Empresa = new Empresa();
            oDocAnulado.Serie = new Serie();
            oDocAnulado.Estado = new Estado();
            oDocAnulado.TipoDocumento = new TipoDocumento();

            oDocAnulado.TipoDocumento.IdTipoDocumento = oDocExists.Nro;
            oDocAnulado.Serie.NumeroSerie = oDocExists.Serie;
            oDocAnulado.NumeroDocumento = oDocExists.NumeroDocumento;
            oDocAnulado.FechaAnulado = oDocExists.FechaAnulado;

            oDocAnulado.MotivoAnulado = DocVoieded.Motivo;
            oDocAnulado.Estado.IdEstado = 0;
            oDocAnulado.Empresa.RUC = DocVoieded.EmiNumDoc;
            oDocAnulado.TipoDocumento.CodigoDocumento = DocVoieded.TipoCE;

            oDocAnulado.Usuario = "WebService";
        }

        private void DeleteFileXmlReceib(string filename)
        {
            string pathdelete = filename + @"\TempVoided.xml";
            if (System.IO.File.Exists(pathdelete))
                System.IO.File.Delete(pathdelete);
        }
    }
}
