using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

using WCFMantenimiento = Slin.Facturacion.Proxies.ServicioMantenimiento;

namespace Slin.Facturacion.Electronica.Web
{
    public class Singleton_wb
    {
        private static readonly Singleton_wb instance = new Singleton_wb();
        static Singleton_wb() { }
        private Singleton_wb() { }
        public static Singleton_wb Instance { get { return instance; } }


        public bool Create_EntityRegex(string path, WCFMantenimiento.ListaEmpresa list)
        {
            try
            {
                string cadenaentity = string.Empty;
                cadenaentity += "<Utilxml>";
                foreach (var ent in list)
                {
                    cadenaentity += "<EntityRegex><ID>" + ent.IdEmpresa + "</ID><Val>" + ent.TipoLogin + "</Val></EntityRegex>";
                }
                cadenaentity += "</Utilxml>";

                var xmldoc = new XmlDocument();
                xmldoc.InnerXml = cadenaentity;
                xmldoc.Save(path + "\\EntityRegex.xml");
                return true;
            }
            catch (Exception ex) { return false; }
        }
    }
}