using WCFFacturacion = Slin.Facturacion.Proxies.ServicioFacturacion;
using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;

using Entity = Slin.Facturacion.BusinessEntities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCont = Slin.Facturacion.ServiceController;
using System.Xml.Serialization;

using xmlFac = Slin.Facturacion.Common.CE;
using xmlNotCred = Slin.Facturacion.Common.CENC;
using xmlNotDeb = Slin.Facturacion.Common.CEND;
using xmlCre = Slin.Facturacion.Common.CRE;

using System.IO;
using System.Xml;
using System.Web.UI.WebControls;

namespace Slin.Facturacion.Common.Method
{
    class RefactoryClass
    {
        
        #region validates filters

        public int Return_Validate_RangeToDate(string stardate, string enddate)
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


        #region Util

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

            if (typedocument == Constantes.Factura)
            {
                obj = new xmlFac.InvoiceType();

                XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlFac.InvoiceType));
                using (TextReader reader = new StringReader(xml_line))
                {
                    obj = (xmlFac.InvoiceType)xmlSerial.Deserialize(reader);
                }
                return obj;
            }
            else if (typedocument == Constantes.NotaCredito)
            {
                obj = new xmlNotCred.CreditNoteType();
                XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlNotCred.CreditNoteType));
                using (TextReader reader = new StringReader(xml_line))
                {
                    obj = (xmlNotCred.CreditNoteType)xmlSerial.Deserialize(reader);
                }
                return obj;
            }
            else if (typedocument == Constantes.NotaDebito)
            {
                obj = new xmlNotDeb.DebitNoteType();
                XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlNotDeb.DebitNoteType));
                using (TextReader reader = new StringReader(xml_line))
                {
                    obj = (xmlNotDeb.DebitNoteType)xmlSerial.Deserialize(reader);
                }
                return obj;
            }
            else if (typedocument == Constantes.Retencion)
            {
                obj = new xmlCre.RetentionType();
                XmlSerializer xmlSerial = new XmlSerializer(typeof(xmlCre.RetentionType));
                using (TextReader reader = new StringReader(xml_line))
                {
                    obj = (xmlCre.RetentionType)xmlSerial.Deserialize(reader);
                }
                return obj;
            }
            else if (typedocument == Constantes.Percepcion)
            {
                return obj;
            }
            return obj;
        }

        #endregion


        #region write log

        public void WriteLog_Service_Comp(string path, string log)
        {
            using (StreamWriter sw = new StreamWriter(path, true, Encoding.UTF8))
            {
                sw.WriteLine(log);
            }
        }

        #endregion

        #region files

        public bool Validate_Exists_File(string path)
        {
            if (System.IO.File.Exists(path))
            {
                return true;
            }
            else { return false; }
        }

        


        // estan bien estos metodos, pero esta complicado al separa un tipo entity de un tipo entity serirializado
        public object Get_Document(string xml_line, string typedocument, string pathxmlfile, string num_ce, string TypeObject)
        {
            object oFactura = new object();

            if (TypeObject == "WCF")
            {

            }
            else if (TypeObject == "WS")
            {
                //Entity.FacturaElectronica oFactura = new Entity.FacturaElectronica();

                oFactura = new object();

                var XMLArchive = new XmlDocument();
                if (xml_line.Length > Constantes.Valor50 + Constantes.Valor50)
                {
                    XMLArchive.InnerXml = xml_line;
                }
                else
                {
                    XMLArchive.Load(pathxmlfile);
                }

                var obj = GetDataXml_AllTypeDocument(XMLArchive.InnerXml, typedocument);

                switch (typedocument)
                {
                    case Constantes.Factura:
                    case Constantes.Boleta:
                        {
                            var inv = (xmlFac.InvoiceType)(obj);
                            oFactura = (Entity.FacturaElectronica)new Common.UtilCE.UsefullClassWS().GetDataFromXMLCE(inv, XMLArchive, num_ce);
                            break;
                        }
                }
            }


            return oFactura;
        }
        public WCFFacturacion.FacturaElectronica Get_Document_WCF(string xml_line, string typedocument, string pathxmlfile, string num_ce)
        {
            var XMLArchive = new XmlDocument();
            XMLArchive.Load(pathxmlfile);

            var obj = GetDataXml_AllTypeDocument(XMLArchive.InnerXml, typedocument);

            WCFFacturacion.FacturaElectronica oFactura = new WCFFacturacion.FacturaElectronica();

            switch (typedocument)
            {
                case Constantes.Factura:
                case Constantes.Boleta:
                    {
                        var inv = (xmlFac.InvoiceType)(obj);
                        //oFactura = new Common.Util.UsefullClass().GetDataFromXMLCE(inv, XMLArchive, num_ce);
                        oFactura = Util.UsefullClass.Instance.GetDataFromXMLCE(inv, XMLArchive, num_ce);
                        break;
                    }
            }

            return oFactura;
        }

        #endregion


        #region others

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


        #endregion
    }
}
