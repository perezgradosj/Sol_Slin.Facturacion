using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Slin.Facturacion.BusinessEntities;

namespace Host.IIS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IServicioMantenimiento" in both code and config file together.
    [ServiceContract]
    public interface IServicioMantenimiento
    {

        #region LISTAS

        [OperationContract]
        ListaSexo GetListaSexo();

        [OperationContract]
        ListaEstado GetListaEstado();

        [OperationContract]
        ListaEmpresa GetListaEmpresa();

        [OperationContract]
        ListaTipoDocumentoIdentidad GetListaTipoDocumentoIdentidad();

        [OperationContract]
        ListaPais GetListaPais();

        [OperationContract]
        ListaDepartamento GetListaDepartamento(Int32 IdPais);

        [OperationContract]
        ListaProvincia GetListaProvincia(Int32 IdDepartamento);

        [OperationContract]
        ListaDistrito GetListaDistrito(Int32 IdProvincia);
        #endregion

        #region LISTA DATA

        [OperationContract]
        ListaEmpleados GetListaEmpleado(Empleado oEmpleado);

        [OperationContract]
        ListaEmpresa GetListadoEmpresa(Empresa oEmpresa);
        #endregion


        #region REGISTRO

        [OperationContract]
        String RegistrarEmpleado(Empleado oEmpleado);

        [OperationContract]
        String ActualizarEmpleado(Empleado oEmpleado);

        [OperationContract]
        Int32 ValidarDniRuc(String Dni_Ruc);

        [OperationContract]
        ListaEmpresa ValidarEmpresaRuc(String Ruc_Empresa);

        [OperationContract]
        String RegistrarEmpresa(Empresa oEmpresa);

        [OperationContract]
        String ActualizarEmpresa(Empresa oEmpresa);

        [OperationContract]
        Ubigeo GetUbigeo(String CodigoUbigeo);

        #endregion

        #region MANTENIMIENTO DE CLIENTE

        [OperationContract]
        ListaCliente ValidarNroClienteExiste(String NroDocumento);

        [OperationContract]
        String InsertarCliente(Cliente oCliente);

        [OperationContract]
        String ActualizarCliente(Cliente oCliente);

        [OperationContract]
        ListaCliente GetListaCliente(Cliente oCliente);

        #endregion

        #region MANT BANK

        [OperationContract]
        String Insert_CtaBank(Bank objbank);

        [OperationContract]
        String Update_CtaBank(Bank objbank);

        [OperationContract]
        ListBank GetListBank(string RucEntity);
        #endregion END MANT BANK

        #region SETTINGS COMPANY
        [OperationContract]
        ListaEmpresa Get_ListCompanyGroup(int Grp);
        #endregion
    }
}
