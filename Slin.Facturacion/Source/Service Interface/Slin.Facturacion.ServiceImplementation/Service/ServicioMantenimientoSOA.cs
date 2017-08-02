using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Slin.Facturacion.BusinessEntities;
using Slin.Facturacion.DataAccess;

namespace Slin.Facturacion.ServiceImplementation
{
    public class ServicioMantenimientoSOA
    {
        //private static readonly ServicioMantenimientoSOA instance = new ServicioMantenimientoSOA();
        //static ServicioMantenimientoSOA() { }
        //private ServicioMantenimientoSOA() { }
        //public static ServicioMantenimientoSOA Instance { get { return instance; } }


        MantenimientoDataAccess objMantenimientoDataAccess = new MantenimientoDataAccess();

        #region LISTAS

        public ListaSexo GetListaSexo()
        {
            return objMantenimientoDataAccess.GetListaSexo();
        }

        public ListaEstado GetListaEstado()
        {
            return objMantenimientoDataAccess.GetListaEstado();
        }

        public ListaEmpresa GetListaEmpresa()
        {
            return objMantenimientoDataAccess.GetListaEmpresa();
        }

        public ListaTipoDocumentoIdentidad GetListaTipoDocumentoIdentidad()
        {
            return objMantenimientoDataAccess.GetListaTipoDocumentoIdentidad();
        }

        public ListaPais GetListaPais()
        {
            return objMantenimientoDataAccess.GetListaPais();
        }

        public ListaDepartamento GetListaDepartamento(Int32 IdPais)
        {
            return objMantenimientoDataAccess.GetListaDepartamento(IdPais);
        }

        public ListaProvincia GetListaProvincia(Int32 IdDepartamento)
        {
            return objMantenimientoDataAccess.GetListaProvincia(IdDepartamento);
        }

        public ListaDistrito GetListaDistrito(Int32 IdProvincia)
        {
            return objMantenimientoDataAccess.GetListaDistrito(IdProvincia);
        }
        #endregion

        #region LISTA DATA

        public ListaEmpleados GetListaEmpleado(Empleado oEmpleado)
        {
            return objMantenimientoDataAccess.GetListaEmpleado(oEmpleado);
        }

        public ListaEmpresa GetListadoEmpresa(Empresa oEmpresa)
        {
            return objMantenimientoDataAccess.GetListadoEmpresa(oEmpresa);
        }
        #endregion

        #region REGISTRO

        public String RegistrarEmpleado(Empleado oEmpleado)
        {
            return objMantenimientoDataAccess.RegistrarEmpleado(oEmpleado);
        }

        public String ActualizarEmpleado(Empleado oEmpleado)
        {
            return objMantenimientoDataAccess.ActualizarEmpleado(oEmpleado);
        }


        public Int32 ValidarDniRuc(String Dni_Ruc)
        {
            return objMantenimientoDataAccess.ValidarDniRuc(Dni_Ruc);
        }

        public ListaEmpresa ValidarEmpresaRuc(String Ruc_Empresa)
        {
            return objMantenimientoDataAccess.ValidarEmpresaRuc(Ruc_Empresa);
        }

        public String RegistrarEmpresa(Empresa oEmpresa)
        {
            return objMantenimientoDataAccess.RegistrarEmpresa(oEmpresa);
        }

        public String ActualizarEmpresa(Empresa oEmpresa)
        {
            return objMantenimientoDataAccess.ActualizarEmpresa(oEmpresa);
        }

        public Ubigeo GetUbigeo(String CodigoUbigeo)
        {
            return objMantenimientoDataAccess.GetUbigeo(CodigoUbigeo);
        }

        #endregion

        #region MANTENIMIENTO DE CLIENTE

        public ListaCliente ValidarNroClienteExiste(String NroDocumento)
        {
            return objMantenimientoDataAccess.ValidarNroClienteExiste(NroDocumento);
        }

        public String InsertarCliente(Cliente oCliente)
        {
            return objMantenimientoDataAccess.InsertarCliente(oCliente);
        }

        public String ActualizarCliente(Cliente oCliente)
        {
            return objMantenimientoDataAccess.ActualizarCliente(oCliente);
        }

        public ListaCliente GetListaCliente(Cliente oCliente)
        {
            return objMantenimientoDataAccess.GetListaCliente(oCliente);
        }

        #endregion

        #region MANT BANK

        public string Insert_CtaBank(Bank objbank)
        {
            return objMantenimientoDataAccess.Insert_CtaBank(objbank);
        }

        public string Update_CtaBank(Bank objbank)
        {
            return objMantenimientoDataAccess.Update_CtaBank(objbank);
        }

        public ListBank GetListBank(string RucEntity)
        {
            return objMantenimientoDataAccess.GetListBank(RucEntity);
        }

        #endregion END MANT BANK

        #region SETTINGS COMPANY
        public ListaEmpresa Get_ListCompanyGroup(int Grp)
        {
            return objMantenimientoDataAccess.Get_ListCompanyGroup(Grp);
        }
        #endregion
    }
}
