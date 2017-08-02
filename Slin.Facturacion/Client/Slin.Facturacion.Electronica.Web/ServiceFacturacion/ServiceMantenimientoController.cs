using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Slin.Facturacion.Proxies.ServicioMantenimiento;

using Slin.Facturacion.ServiceController;

namespace Slin.Facturacion.Electronica.Web.ServiceFacturacion
{
    public sealed class ServiceMantenimientoController
    {
        private static readonly ServiceMantenimientoController instance = new ServiceMantenimientoController();
        static ServiceMantenimientoController() { }
        private ServiceMantenimientoController() { }
        public static ServiceMantenimientoController Instance { get { return instance; } }


        #region LISTAS

        public  ListaSexo GetListaSexo()
        {
            return MantenimientoServiceController.Instance.GetListaSexo();
        }

        public  ListaEstado GetListaEstado()
        {
            return MantenimientoServiceController.Instance.GetListaEstado();
        }

        public  ListaTipoDocumentoIdentidad GetListaTipoDocumentoIdentidad()
        {
            return MantenimientoServiceController.Instance.GetListaTipoDocumentoIdentidad();
        }

        public  ListaEmpresa GetListaEmpresa()
        {
            return MantenimientoServiceController.Instance.GetListaEmpresa();
        }


        public  ListaPais GetListaPais()
        {
            return MantenimientoServiceController.Instance.GetListaPais();
        }

        public  ListaDepartamento GetListaDepartamento(Int32 IdPais)
        {
            return MantenimientoServiceController.Instance.GetListaDepartamento(IdPais);
        }

        public  ListaProvincia GetListaProvincia(Int32 IdDepartamento)
        {
            return MantenimientoServiceController.Instance.GetListaProvincia(IdDepartamento);
        }

        public  ListaDistrito GetListaDistrito(Int32 IdProvincia)
        {
            return MantenimientoServiceController.Instance.GetListaDistrito(IdProvincia);
        }

        #endregion


        #region LISTADO ENTITY

        public  ListaEmpresa GetListadoEmpresa(Empresa oEmpresa)
        {
            return MantenimientoServiceController.Instance.GetListadoEmpresa(oEmpresa);
        }

        public  ListaEmpleados GetListaEmpleado(Empleado oEmpleado)
        {
            return MantenimientoServiceController.Instance.GetListaEmpleado(oEmpleado);
        }

        #endregion

        #region REGISTRO


        public  String RegistrarEmpleado(Empleado oEmpleado)
        {
            return MantenimientoServiceController.Instance.RegistrarEmpleado(oEmpleado);
        }

        public  String ActualizarEmpleado(Empleado oEmpleado)
        {
            return MantenimientoServiceController.Instance.ActualizarEmpleado(oEmpleado);
        }

        public  Int32 ValidarDniRuc(string Dni_Ruc)
        {
            return MantenimientoServiceController.Instance.ValidarDniRuc(Dni_Ruc);
        }

        public  String RegistrarEmpresa(Empresa oEmpresa)
        {
            return MantenimientoServiceController.Instance.RegistrarEmpresa(oEmpresa);
        }

        public  String ActualizarEmpresa(Empresa oEmpresa)
        {
            return MantenimientoServiceController.Instance.ActualizarEmpresa(oEmpresa);
        }

        public  ListaEmpresa ValidarEmpresaRuc(String Ruc_Empresa)
        {
            return MantenimientoServiceController.Instance.ValidarEmpresaRuc(Ruc_Empresa);
        }

        public  Ubigeo GetUbigeo(String CodigoUbigeo)
        {
            return MantenimientoServiceController.Instance.GetUbigeo(CodigoUbigeo);
        }

        #endregion

        #region MANTENIMIENTO DE CLIENTE

        public  ListaCliente ValidarNroClienteExiste(String NroDocumento)
        {
            return MantenimientoServiceController.Instance.ValidarNroClienteExiste(NroDocumento);
        }

        public  String InsertarCliente(Cliente oCliente)
        {
            return MantenimientoServiceController.Instance.InsertarCliente(oCliente);
        }

        public  String ActualizarCliente(Cliente oCliente)
        {
            return MantenimientoServiceController.Instance.ActualizarCliente(oCliente);
        }

        public  ListaCliente GetListaCliente(Cliente oCliente)
        {
            return MantenimientoServiceController.Instance.GetListaCliente(oCliente);
        }

        #endregion


        #region MANT BANK

        public  string Insert_CtaBank(Bank objbank)
        {
            return MantenimientoServiceController.Instance.Insert_CtaBank(objbank);
        }

        public  string Update_CtaBank(Bank objbank)
        {
            return MantenimientoServiceController.Instance.Update_CtaBank(objbank);
        }

        public  ListBank GetListBank(string RucEntity)
        {
            return MantenimientoServiceController.Instance.GetListBank(RucEntity);
        }

        #endregion END MANT BANK

        #region SETTINGS COMPANY
        public ListaEmpresa Get_ListCompanyGroup(int Grp)
        {
            return MantenimientoServiceController.Instance.Get_ListCompanyGroup(Grp);
        }
        #endregion
    }
}