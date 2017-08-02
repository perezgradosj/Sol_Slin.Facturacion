using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Slin.Facturacion.Proxies.ServicioMantenimiento;

namespace Slin.Facturacion.ServiceController
{
    public sealed class MantenimientoServiceController
    {
        private static readonly MantenimientoServiceController instance = new MantenimientoServiceController();
        static MantenimientoServiceController() { }
        private MantenimientoServiceController() { }
        public static MantenimientoServiceController Instance { get { return instance; } }



        #region LISTAS

        public ListaSexo GetListaSexo()
        {
            using (ServicioMantenimientoClient Client = new ServicioMantenimientoClient())
            {
                return Client.GetListaSexo();
            }
        }

        public ListaEstado GetListaEstado()
        {
            using (ServicioMantenimientoClient Client = new ServicioMantenimientoClient())
            {
                return Client.GetListaEstado();
            }
        }

        public ListaEmpresa GetListaEmpresa()
        {
            using (ServicioMantenimientoClient Client = new ServicioMantenimientoClient())
            {
                return Client.GetListaEmpresa();
            }
        }

        public ListaTipoDocumentoIdentidad GetListaTipoDocumentoIdentidad()
        {
            using (ServicioMantenimientoClient Client = new ServicioMantenimientoClient())
            {
                return Client.GetListaTipoDocumentoIdentidad();
            }
        }

        public ListaPais GetListaPais()
        {
            using (ServicioMantenimientoClient Client = new ServicioMantenimientoClient())
            {
                return Client.GetListaPais();
            }
        }

        public ListaDepartamento GetListaDepartamento(Int32 IdPais)
        {
            using (ServicioMantenimientoClient Client = new ServicioMantenimientoClient())
            {
                return Client.GetListaDepartamento(IdPais);
            }
        }

        public ListaProvincia GetListaProvincia(Int32 IdDepartamento)
        {
            using (ServicioMantenimientoClient Client = new ServicioMantenimientoClient())
            {
                return Client.GetListaProvincia(IdDepartamento);
            }
        }

        public ListaDistrito GetListaDistrito(Int32 IdProvincia)
        {
            using (ServicioMantenimientoClient Client = new ServicioMantenimientoClient())
            {
                return Client.GetListaDistrito(IdProvincia);
            }
        }


        #endregion

        #region LISTA DATA

        public ListaEmpleados GetListaEmpleado(Empleado oEmpleado)
        {
            using (ServicioMantenimientoClient Client = new ServicioMantenimientoClient())
            {
                return Client.GetListaEmpleado(oEmpleado);
            }
        }

        public ListaEmpresa GetListadoEmpresa(Empresa oEmpresa)
        {
            using (ServicioMantenimientoClient Client = new ServicioMantenimientoClient())
            {
                return Client.GetListadoEmpresa(oEmpresa);
            }
        }

        #endregion

        #region REGISTRO

        public String RegistrarEmpleado(Empleado oEmpleado)
        {
            using (ServicioMantenimientoClient Client = new ServicioMantenimientoClient())
            {
                return Client.RegistrarEmpleado(oEmpleado);
            }
        }

        public String ActualizarEmpleado(Empleado oEmpleado)
        {
            using (ServicioMantenimientoClient Client = new ServicioMantenimientoClient())
            {
                return Client.ActualizarEmpleado(oEmpleado);
            }
        }

        public Int32 ValidarDniRuc(String Dni_Ruc)
        {
            using (ServicioMantenimientoClient Client = new ServicioMantenimientoClient())
            {
                return Client.ValidarDniRuc(Dni_Ruc);
            }
        }

        public ListaEmpresa ValidarEmpresaRuc(String Ruc_Empresa)
        {
            using (ServicioMantenimientoClient Client = new ServicioMantenimientoClient())
            {
                return Client.ValidarEmpresaRuc(Ruc_Empresa);
            }
        }

        public String RegistrarEmpresa(Empresa oEmpresa)
        {
            using (ServicioMantenimientoClient Client = new ServicioMantenimientoClient())
            {
                return Client.RegistrarEmpresa(oEmpresa);
            }
        }

        public String ActualizarEmpresa(Empresa oEmpresa)
        {
            using (ServicioMantenimientoClient Client = new ServicioMantenimientoClient())
            {
                return Client.ActualizarEmpresa(oEmpresa);
            }
        }

        public Ubigeo GetUbigeo(String CodigoUbigeo)
        {
            using (ServicioMantenimientoClient Client = new ServicioMantenimientoClient())
            {
                return Client.GetUbigeo(CodigoUbigeo);
            }
        }

        #endregion

        #region MANTENIMIENTO DE CLIENTE

        public ListaCliente ValidarNroClienteExiste(String NroDocumento)
        {
            using (ServicioMantenimientoClient Client = new ServicioMantenimientoClient())
            {
                return Client.ValidarNroClienteExiste(NroDocumento);
            }
        }

        public String InsertarCliente(Cliente oCliente)
        {
            using (ServicioMantenimientoClient Client = new ServicioMantenimientoClient())
            {
                return Client.InsertarCliente(oCliente);
            }
        }

        public String ActualizarCliente(Cliente oCliente)
        {
            using (ServicioMantenimientoClient Client = new ServicioMantenimientoClient())
            {
                return Client.ActualizarCliente(oCliente);
            }
        }

        public ListaCliente GetListaCliente(Cliente oCliente)
        {
            using (ServicioMantenimientoClient Client = new ServicioMantenimientoClient())
            {
                return Client.GetListaCliente(oCliente);
            }
        }

        #endregion

        #region MANT BANK

        public string Insert_CtaBank(Bank objbank)
        {
            using (ServicioMantenimientoClient Client = new ServicioMantenimientoClient())
            {
                return Client.Insert_CtaBank(objbank);
            }
        }

        public string Update_CtaBank(Bank objbank)
        {
            using (ServicioMantenimientoClient Client = new ServicioMantenimientoClient())
            {
                return Client.Update_CtaBank(objbank);
            }
        }

        public ListBank GetListBank(string RucEntity)
        {
            using (ServicioMantenimientoClient Client = new ServicioMantenimientoClient())
            {
                return Client.GetListBank(RucEntity);
            }
        }

        #endregion END MANT BANK

        #region SETTINGS COMPANY
        public ListaEmpresa Get_ListCompanyGroup(int Grp)
        {
            using (ServicioMantenimientoClient Client = new ServicioMantenimientoClient())
            {
                return Client.Get_ListCompanyGroup(Grp);
            }
        }
        #endregion
    }
}
