using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;

namespace Slin.Facturacion.Common.Helper
{
    public sealed class RolClass
    {
        private static readonly RolClass instance = new RolClass();
        static RolClass() { }
        private RolClass() { }
        public static RolClass Instance { get { return instance; } }

        public bool ChargeUserRol(WCFSeguridad.ListaRol list_rol)
        {
            bool result = false;
            try
            {
                for (int i = 0; i <= list_rol.Count - 1; i++)
                {
                    if (list_rol[i].CodigoRol.Contains(Constantes.RolGuardar))
                    { result = true; break; }
                    else if (list_rol[i].CodigoRol.Contains(Constantes.RolModificar))
                    { result = true; break; }
                    else if (list_rol[i].CodigoRol.Contains(Constantes.RolBuscar))
                    { result = true; break; }
                    else if (list_rol[i].CodigoRol.Contains(Constantes.RolExportar))
                    { result = true; break; }
                    else if (list_rol[i].CodigoRol.Contains(Constantes.RolEnviar))
                    { result = true; break; }
                    else if (list_rol[i].CodigoRol.Contains(Constantes.RolNuevo))
                    { result = true; break; }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return result;
        }
    }
}
