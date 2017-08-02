using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Slin.Facturacion.Proxies.ServicioResumen;

namespace Slin.Facturacion.ServiceController
{
    public class ResumenServiceController
    {
        public string ProcesaA(string fecha, string RucEmpresa, string TipoRes)//EAnulados oAnulado
        {
            using (ServicioResumenClient Client = new ServicioResumenClient())
            {
                return Client.ProcesaResumen(fecha, RucEmpresa, TipoRes);
            }
        }

        public string ProcesaB(string fecha, string RucEmpresa, string TipoRes)//EAnulados oAnulado
        {
            using (ServicioResumenClient Client = new ServicioResumenClient())
            {
                return Client.ProcesaResumen(fecha, RucEmpresa, TipoRes);
            }
        }

        public string ProcesaR(string fecha, string RucEmpresa, string TipoRes)//EAnulados oAnulado
        {
            using (ServicioResumenClient Client = new ServicioResumenClient())
            {
                return Client.ProcesaResumen(fecha, RucEmpresa, TipoRes);
            }
        }


        //public string Mesaje(string msje)
        //{
        //    using (ServicioResumenClient Client = new ServicioResumenClient())
        //    {
        //        return Client.Mensaje(msje);
        //    }
        //}
    }
}
