using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slin.Facturacion.InstallCommon.Util
{
    public class Util
    {
        public List<string> GetListAD()
        {
            List<string> listAD = new List<string>();

            listAD.Insert(0, "-SELECCIONE-");
            listAD.Insert(1, "1-SI");
            listAD.Insert(2, "2-NO");
            return listAD;
        }

        public List<string> GetListSSL()
        {
            List<string> listSSL = new List<string>();

            listSSL.Insert(0, "-SELECCIONE-");
            listSSL.Insert(1, "SI");
            listSSL.Insert(2, "NO");
            return listSSL;
        }

        public List<string> GetListVersionSQLInstalled()
        {
            List<string> listSqlVersion = new List<string>();

            listSqlVersion.Insert(0, "Sql Server 2008 R2");
            listSqlVersion.Insert(1, "Sql Server 2012");
            return listSqlVersion;
        }

        public List<string> GetListProtocolo()
        {
            List<string> listprotocolo = new List<string>();

            listprotocolo.Insert(0, "HTTP");
            listprotocolo.Insert(1, "HTTPS");
            return listprotocolo;
        }
    }
}
