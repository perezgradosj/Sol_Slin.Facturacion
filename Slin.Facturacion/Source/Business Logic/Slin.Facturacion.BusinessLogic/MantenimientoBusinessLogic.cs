using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Slin.Facturacion.DataAccess;
using Slin.Facturacion.BusinessEntities;

namespace Slin.Facturacion.BusinessLogic
{
    public class MantenimientoBusinessLogic
    {
        MantenimientoDataAccess objMantenimientoDataAccess = new MantenimientoDataAccess();

        #region LISTAS

        public ListaEmpresa GetListaEmpresa()
        {
            return objMantenimientoDataAccess.GetListaEmpresa();
        }


        public ListaSexo GetListaSexo()
        {
            return objMantenimientoDataAccess.GetListaSexo();
        }

        public ListaEstado GetListaEstado()
        {
            return objMantenimientoDataAccess.GetListaEstado();
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


        #region LISTADOS ENTITY


        public ListaEmpresa GetListadoEmpresa(Empresa oEmpresa)
        {
            return objMantenimientoDataAccess.GetListadoEmpresa(oEmpresa);
        }


        public ListaEmpleados GetListaEmpleado(Empleado oEmpleado)
        {
            return objMantenimientoDataAccess.GetListaEmpleado(oEmpleado);
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


        public String RegistrarEmpresa(Empresa oEmpresa)
        {
            return objMantenimientoDataAccess.RegistrarEmpresa(oEmpresa);
        }

        public String ActualizarEmpresa(Empresa oEmpresa)
        {
            return objMantenimientoDataAccess.ActualizarEmpresa(oEmpresa);
        }

        public ListaEmpresa ValidarEmpresaRuc(String Ruc_Empresa)
        {
            return objMantenimientoDataAccess.ValidarEmpresaRuc(Ruc_Empresa);
        }

        public Int32 ValidarDniRuc(string Dni_Ruc)
        {
            return objMantenimientoDataAccess.ValidarDniRuc(Dni_Ruc);
        }

        public Ubigeo GetUbigeo(String CodigoUbigeo)
        {
            return objMantenimientoDataAccess.GetUbigeo(CodigoUbigeo);
        }
        #endregion

    }
}
