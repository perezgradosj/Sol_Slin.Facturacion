using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
//using System.ServiceModel;
using System.Text;

using Slin.Facturacion.BusinessEntities;
using Slin.Facturacion.ServiceImplementation;

using WCF = global::System.ServiceModel;

namespace Host.IIS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ServicioMantenimiento" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ServicioMantenimiento.svc or ServicioMantenimiento.svc.cs at the Solution Explorer and start debugging.

    [WCF::ServiceBehavior(Name = "ServicioMantenimiento",
        Namespace = "http://www.slin.com.pe",
        InstanceContextMode = WCF::InstanceContextMode.PerSession,
        ConcurrencyMode = WCF::ConcurrencyMode.Single)]
    public class ServicioMantenimiento : IServicioMantenimiento
    {
        //Singleton_SI objMethod = new Singleton_SI();

        ServicioMantenimientoSOA objMethod = new ServicioMantenimientoSOA();
        #region LISTAS

        public ListaSexo GetListaSexo()
        {
            return objMethod.GetListaSexo();
        }

        public ListaEstado GetListaEstado()
        {
            return objMethod.GetListaEstado();
        }

        public ListaEmpresa GetListaEmpresa()
        {
            return objMethod.GetListaEmpresa();
        }

        public ListaTipoDocumentoIdentidad GetListaTipoDocumentoIdentidad()
        {
            return objMethod.GetListaTipoDocumentoIdentidad();
        }

        public ListaPais GetListaPais()
        {
            return objMethod.GetListaPais();
        }

        public ListaDepartamento GetListaDepartamento(Int32 IdPais)
        {
            return objMethod.GetListaDepartamento(IdPais);
        }

        public ListaProvincia GetListaProvincia(Int32 IdDepartamento)
        {
            return objMethod.GetListaProvincia(IdDepartamento);
        }

        public ListaDistrito GetListaDistrito(Int32 IdProvincia)
        {
            return objMethod.GetListaDistrito(IdProvincia);
        }
        #endregion

        #region LISTA DATA

        public ListaEmpleados GetListaEmpleado(Empleado oEmpleado)
        {
            return objMethod.GetListaEmpleado(oEmpleado);
        }

        public ListaEmpresa GetListadoEmpresa(Empresa oEmpresa)
        {
            return objMethod.GetListadoEmpresa(oEmpresa);
        }
        #endregion

        #region REGISTRO

        public String RegistrarEmpleado(Empleado oEmpleado)
        {
            return objMethod.RegistrarEmpleado(oEmpleado);
        }

        public String ActualizarEmpleado(Empleado oEmpleado)
        {
            return objMethod.ActualizarEmpleado(oEmpleado);
        }


        public Int32 ValidarDniRuc(String Dni_Ruc)
        {
            return objMethod.ValidarDniRuc(Dni_Ruc);
        }

        public ListaEmpresa ValidarEmpresaRuc(String Ruc_Empresa)
        {
            return objMethod.ValidarEmpresaRuc(Ruc_Empresa);
        }

        public String RegistrarEmpresa(Empresa oEmpresa)
        {
            return objMethod.RegistrarEmpresa(oEmpresa);
        }

        public String ActualizarEmpresa(Empresa oEmpresa)
        {
            return objMethod.ActualizarEmpresa(oEmpresa);
        }

        public Ubigeo GetUbigeo(String CodigoUbigeo)
        {
            return objMethod.GetUbigeo(CodigoUbigeo);
        }
        #endregion


        #region MANTENIMIENTO DE CLIENTE

        public ListaCliente ValidarNroClienteExiste(String NroDocumento)
        {
            return objMethod.ValidarNroClienteExiste(NroDocumento);
        }

        public String InsertarCliente(Cliente oCliente)
        {
            return objMethod.InsertarCliente(oCliente);
        }

        public String ActualizarCliente(Cliente oCliente)
        {
            return objMethod.ActualizarCliente(oCliente);
        }

        public ListaCliente GetListaCliente(Cliente oCliente)
        {
            return objMethod.GetListaCliente(oCliente);
        }


        #region MANT BANK

        public string Insert_CtaBank(Bank objbank)
        {
            return objMethod.Insert_CtaBank(objbank);
        }

        public string Update_CtaBank(Bank objbank)
        {
            return objMethod.Update_CtaBank(objbank);
        }

        public ListBank GetListBank(string RucEntity)
        {
            return objMethod.GetListBank(RucEntity);
        }

        #endregion END MANT BANK

        #endregion

        #region SETTINGS COMPANY

        public ListaEmpresa Get_ListCompanyGroup(int Grp)
        {
            return objMethod.Get_ListCompanyGroup(Grp);
        }
        #endregion
    }
}
