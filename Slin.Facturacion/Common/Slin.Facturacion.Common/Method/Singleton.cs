using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WCFFacturacion = Slin.Facturacion.Proxies.ServicioFacturacion;
using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;

using SCont = Slin.Facturacion.ServiceController;

using xmlFac = Slin.Facturacion.Common.CE;
using xmlNotCred = Slin.Facturacion.Common.CENC;
using xmlNotDeb = Slin.Facturacion.Common.CEND;
using xmlCre = Slin.Facturacion.Common.CRE;

using System.IO;
using System.Xml;
using System.Web.UI.WebControls;
using System.Xml.Serialization;

namespace Slin.Facturacion.Common.Method
{
    public sealed class Singleton 
    {
        private static readonly Singleton instance = new Singleton();
        static Singleton() {}
        private Singleton() {}
        public static Singleton Instance { get { return instance; } }


        #region validation filters

        public int Validate_RangeDate(string stardate, string enddate)
        {
            int val = Constantes.ValorCero;
            try
            {
                DateTime fecha1; DateTime fecha2;

                if (stardate.Length > 0)
                {
                    fecha1 = Convert.ToDateTime(stardate);
                    fecha2 = Convert.ToDateTime(enddate);
                    int result = DateTime.Compare(fecha1, fecha2);
                    val = result < 0 ? 2 : result;
                }
                else { val = Constantes.ValorDos; }
            }
            catch (Exception ex) { }
            return val;
        }

        #endregion



        #region charger list objects

        public WCFFacturacion.ListaSerie Get_ListSerieDocument(WCFSeguridad.Usuario ouser, int id_typedoc)
        {
            WCFFacturacion.Serie oSerie = new WCFFacturacion.Serie();
            WCFFacturacion.ListaSerie oListaSerie = new WCFFacturacion.ListaSerie();

            var listserie = new WCFFacturacion.ListaSerie();
            try
            {
                oSerie = new WCFFacturacion.Serie();
                oSerie.TipoDocumento = new WCFFacturacion.TipoDocumento();
                oSerie.Empresa = new WCFFacturacion.Empresa();

                oSerie.TipoDocumento.IdTipoDocumento = id_typedoc;
                oSerie.Empresa.IdEmpresa = ouser.Empresa.IdEmpresa;

                oListaSerie = SCont.FacturacionServiceController.Instance.ListarSerie(oSerie);
                oListaSerie.Insert(0, new WCFFacturacion.Serie() { IdSerie = Constantes.ValorCero, NumeroSerie = Constantes.ValorTodos });
                listserie = new WCFFacturacion.ListaSerie();

                foreach (var serie in oListaSerie)
                {
                    if (!serie.NumeroSerie.Contains("R") && !serie.NumeroSerie.Contains("P"))
                    {
                        listserie.Add(serie);
                    }
                }
            }
            catch (Exception ex) { }
            return listserie;
        }

        public WCFFacturacion.ListaSerie Get_ListSerieDocument_CRE(WCFSeguridad.Usuario ouser, int id_typedoc)
        {
            var oSerie = new WCFFacturacion.Serie();
            oSerie = new WCFFacturacion.Serie();
            oSerie.TipoDocumento = new WCFFacturacion.TipoDocumento();
            oSerie.Empresa = new WCFFacturacion.Empresa();

            var listserie = new WCFFacturacion.ListaSerie();

            try
            {
                oSerie.TipoDocumento.IdTipoDocumento = id_typedoc;
                oSerie.Empresa.IdEmpresa = ouser.Empresa.IdEmpresa;

                var oListSerie = SCont.FacturacionServiceController.Instance.ListarSerie(oSerie);
                oListSerie.Insert(0, new WCFFacturacion.Serie() { IdSerie = Constantes.ValorCero, NumeroSerie = Constantes.ValorTodos });

                foreach (var serie in oListSerie)
                {
                    if (serie.NumeroSerie.Contains("R"))
                    {
                        listserie.Add(serie);
                    }
                }
                listserie.Insert(0, new WCFFacturacion.Serie() { IdSerie = Constantes.ValorCero, NumeroSerie = Constantes.ValorTodos });
            }
            catch (Exception ex) { }
            return listserie;
        }

        public WCFFacturacion.ListaTipoDocumento GetList_TypeDocument_CE()
        {
            var oListaTipoDocumento = SCont.FacturacionServiceController.Instance.ListarTipoDocumento();
            var listatpodoc = new WCFFacturacion.ListaTipoDocumento();

            oListaTipoDocumento.RemoveAt(0);
            oListaTipoDocumento.Insert(0, new WCFFacturacion.TipoDocumento() { IdTipoDocumento = Constantes.ValorCero, Descripcion = Constantes.ValorTodos });

            foreach (var tpo in oListaTipoDocumento)
            {
                if (tpo.CodigoDocumento != Constantes.Retencion && tpo.CodigoDocumento != Constantes.Percepcion)
                {
                    listatpodoc.Add(tpo);
                }
            }
            return listatpodoc;
        }

        public WCFFacturacion.ListaTipoDocumento GetList_TypeDocument_CRE()
        {
            var listanewtpodoc = new WCFFacturacion.ListaTipoDocumento();
            var oListaTipoDocumento = SCont.FacturacionServiceController.Instance.ListarTipoDocumento();

            oListaTipoDocumento.RemoveAt(0);
            oListaTipoDocumento.Insert(0, new WCFFacturacion.TipoDocumento() { IdTipoDocumento = Constantes.ValorCero, Descripcion = Constantes.ValorTodos });

            foreach (var tpodoc in oListaTipoDocumento)
            {
                if (tpodoc.Descripcion.Contains(Constantes.RetencionDesc))
                {
                    listanewtpodoc.Add(tpodoc);
                }
            }
            return listanewtpodoc;
        }

        public WCFFacturacion.ListaEstado Get_ListStatus()
        {
            var oListEstado = SCont.FacturacionServiceController.Instance.ListarEstadoDocumento();
            oListEstado.RemoveAt(0);
            oListEstado.Insert(0, new WCFFacturacion.Estado() { IdEstado = Constantes.ValorCero, Descripcion = Constantes.ValorTodos });

            return oListEstado;
        }

        #endregion


        #region UTIL

        public void CreateDirectory(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }

        public object GetDataXml_AllTypeDocument(string xml_line, string typedocument)
        {
            object obj = new object();

            switch (typedocument)
            {
                case Constantes.Factura:
                case Constantes.Boleta:
                    {
                        obj = new xmlFac.InvoiceType();

                        XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlFac.InvoiceType));
                        using (TextReader reader = new StringReader(xml_line))
                        {
                            obj = (xmlFac.InvoiceType)xmlSerial.Deserialize(reader);
                        }
                        break;
                    }
                case Constantes.NotaCredito:
                    {
                        obj = new xmlNotCred.CreditNoteType();
                        XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlNotCred.CreditNoteType));
                        using (TextReader reader = new StringReader(xml_line))
                        {
                            obj = (xmlNotCred.CreditNoteType)xmlSerial.Deserialize(reader);
                        }
                        break;
                    }
                case Constantes.NotaDebito:
                    {
                        obj = new xmlNotDeb.DebitNoteType();
                        XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlNotDeb.DebitNoteType));
                        using (TextReader reader = new StringReader(xml_line))
                        {
                            obj = (xmlNotDeb.DebitNoteType)xmlSerial.Deserialize(reader);
                        }
                        break;
                    }
                case Constantes.Retencion:
                    {
                        obj = new xmlCre.RetentionType();
                        XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlCre.RetentionType));
                        using (TextReader reader = new StringReader(xml_line))
                        {
                            obj = (xmlCre.RetentionType)xmlSerial.Deserialize(reader);
                        }
                        break;
                    }
                case Constantes.Percepcion:
                    {
                        
                        break;
                    }
            }
            #region old method
            //if (typedocument == Constantes.Factura)
            //{
            //    obj = new xmlFac.InvoiceType();

            //    XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlFac.InvoiceType));
            //    using (TextReader reader = new StringReader(xml_line))
            //    {
            //        obj = (xmlFac.InvoiceType)xmlSerial.Deserialize(reader);
            //    }
            //    return obj;
            //}
            //else if (typedocument == Constantes.NotaCredito)
            //{
            //    obj = new xmlNotCred.CreditNoteType();
            //    XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlNotCred.CreditNoteType));
            //    using (TextReader reader = new StringReader(xml_line))
            //    {
            //        obj = (xmlNotCred.CreditNoteType)xmlSerial.Deserialize(reader);
            //    }
            //    return obj;
            //}
            //else if (typedocument == Constantes.NotaDebito)
            //{
            //    obj = new xmlNotDeb.DebitNoteType();
            //    XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlNotDeb.DebitNoteType));
            //    using (TextReader reader = new StringReader(xml_line))
            //    {
            //        obj = (xmlNotDeb.DebitNoteType)xmlSerial.Deserialize(reader);
            //    }
            //    return obj;
            //}
            //else if (typedocument == Constantes.Retencion)
            //{
            //    obj = new xmlCre.RetentionType();
            //    XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlCre.RetentionType));
            //    using (TextReader reader = new StringReader(xml_line))
            //    {
            //        obj = (xmlCre.RetentionType)xmlSerial.Deserialize(reader);
            //    }
            //    return obj;
            //}
            //else if (typedocument == Constantes.Percepcion)
            //{
            //    return obj;
            //}
            #endregion
            return obj;
        }

        #endregion


        #region  Write log

        public void WriteLog_Service_Comp(string path, string log)
        {
            using (StreamWriter sw = new StreamWriter(path, true, Encoding.GetEncoding("ISO-8859-1")))
            {
                sw.WriteLine(log);
            }
        }

        #endregion



        #region 
        public bool Validate_Exists_File(string path)
        {
            if (System.IO.File.Exists(path))
            {
                return true;
            }
            else { return false; }
        }

        #endregion


        #region Others

        public List<string> Execute_List_PrintSend(string type_checkbox, GridView grid)
        {
            List<string> listDoc = new List<string>();
            listDoc = new List<string>();

            for (int i = 0; i <= grid.Rows.Count - 1; i++)
            {
                CheckBox check = ((CheckBox)grid.Rows[i].Cells[1].FindControl(type_checkbox));

                if (check.Checked)
                {
                    listDoc.Add("Document: " + (i + 1));
                }
            }
            return listDoc;
        }

        public string Get_PathWriteOrder(string path)
        {
            return Path.GetFullPath(Path.Combine(path, "../")) + @"_ADE\";
        }


        public void Create_FileXml(string xml_line, string path)
        {
            using (StreamWriter sw = new StreamWriter(File.Open(path, FileMode.Create), Encoding.GetEncoding("ISO-8859-1")))
            {
                sw.Write(xml_line);
            }
        }

        #endregion




        #region TYPE NC AND ND

        public string Get_Desc_TypeNC(string Code)
        {
            string response = string.Empty;
            switch (Code)
            {
                case Constantes.Cod_NC_01: { response = Constantes.Desc_NC_01; break; }
                case Constantes.Cod_NC_02: { response = Constantes.Desc_NC_02; break; }
                case Constantes.Cod_NC_03: { response = Constantes.Desc_NC_03; break; }
                case Constantes.Cod_NC_04: { response = Constantes.Desc_NC_04; break; }
                case Constantes.Cod_NC_05: { response = Constantes.Desc_NC_05; break; }
                case Constantes.Cod_NC_06: { response = Constantes.Desc_NC_06; break; }
                case Constantes.Cod_NC_07: { response = Constantes.Desc_NC_07; break; }
                case Constantes.Cod_NC_08: { response = Constantes.Desc_NC_08; break; }
                case Constantes.Cod_NC_09: { response = Constantes.Desc_NC_09; break; }
            }
            return response;
        }

        public string Get_Desc_TypeND(string Code)
        {
            string response = string.Empty;
            switch (Code)
            {
                case Constantes.Cod_ND_01: { response = Constantes.Desc_ND_01; break; }
                case Constantes.Cod_ND_02: { response = Constantes.Desc_ND_02; break; }
                case Constantes.Cod_ND_03: { response = Constantes.Desc_ND_03; break; }
            }
            return response;
        }




        #endregion

    }
}
