using Slin.Facturacion.BusinessEntities.Helper;
using Slin.Facturacion.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slin.Facturacion.ServiceImplementation
{
    public class ServicioInterfaceSOA
    {
        //private static readonly ServicioInterfaceSOA instance = new ServicioInterfaceSOA();
        //static ServicioInterfaceSOA() { }
        //private ServicioInterfaceSOA() { }
        //public static ServicioInterfaceSOA Instance { get { return instance; } }



        InterfaceDataAccess oInterfaceDataAccess = new InterfaceDataAccess();

        public List<ERegex> getRegex()
        {
            return oInterfaceDataAccess.getRegex();
        }
    }
}
