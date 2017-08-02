using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Slin.Facturacion.BusinessEntities;
using Slin.Facturacion.ServiceImplementation;

namespace Host.IIS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ServicioSeguridad" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ServicioSeguridad.svc or ServicioSeguridad.svc.cs at the Solution Explorer and start debugging.
    public class ServicioSeguridad : IServicioSeguridad
    {
        ServicioSeguridadSOA objMethod = new ServicioSeguridadSOA();

        #region SEGURIDAD
        public Int32 ValidarAcceso(Usuario oUsuario)
        {
            return objMethod.ValidarAcceso(oUsuario);
        }

        public String ActualizarContrasenia(Usuario oUsuario)
        {
            return objMethod.ActualizarContrasenia(oUsuario);
        }

        public Usuario GetUsuarioLogeado(Usuario oUsuario)
        {
            return objMethod.GetUsuarioLogeado(oUsuario);
        }

        #endregion


        #region USUARIO DATAACCESS

        public String RegistrarUsuario(Usuario oUsuario)
        {
            return objMethod.RegistrarUsuario(oUsuario);
        }

        public String InsertarUsuario(Usuario oUsuario)
        {
            return objMethod.InsertarUsuario(oUsuario);
        }

        public String ActualizarUsuario(Usuario oUsuario)
        {
            return objMethod.ActualizarUsuario(oUsuario);
        }

        public ListaUsuario GetListaUsuario(Usuario oUsuario)
        {
            return objMethod.GetListaUsuario(oUsuario);
        }

        public ListaUsuario ValidarUsername(String Username)
        {
            return objMethod.ValidarUsername(Username);
        }

        public ListaUsuario ValidarDniUsuario(String Dni_Ruc)
        {
            return objMethod.ValidarDniUsuario(Dni_Ruc);
        }

        public ListaUsuario InsertarUsuario_ForExcel(ListaUsuario olistausuario)
        {
            return objMethod.InsertarUsuario_ForExcel(olistausuario);
        }


        public ListaMenu GetListaMenu()
        {
            return objMethod.GetListaMenu();
        }

        public ListaPerfil GetListaPerfiles(string RucEntity)
        {
            return objMethod.GetListaPerfiles(RucEntity);
        }

        public ListaMenu GetListarMenu(Usuario oUsuario)
        {
            return objMethod.GetListarMenu(oUsuario);
        }


        public ListaMenu GetListarMenuPerfil(Perfil oPerfil)
        {
            return objMethod.GetListarMenuPerfil(oPerfil);
        }


        public String RegistrarUsuarioPerfil(Usuario oUsuario)
        {
            return objMethod.RegistrarUsuarioPerfil(oUsuario);
        }

        public String DeleteUsuarioPerfil(Usuario oUsuario)
        {
            return objMethod.DeleteUsuarioPerfil(oUsuario);
        }


        public ListaRol GetListadoRol()
        {
            return objMethod.GetListadoRol();
        }

        public ListaRol GetListaRolesUsuario(Usuario oUsuario)
        {
            return objMethod.GetListaRolesUsuario(oUsuario);
        }

        public String RegistrarUsuarioRol(Usuario oUsuario)
        {
            return objMethod.RegistrarUsuarioRol(oUsuario);
        }

        public String DeleteUsuarioRol(Usuario oUsuario)
        {
            return objMethod.DeleteUsuarioRol(oUsuario);
        }

        public Int32 ObtenerNuevoIdUsuario()
        {
            return objMethod.ObtenerNuevoIdUsuario();
        }

        public String RegistrarNuevoPerfil(Perfil oPerfil)
        {
            return objMethod.RegistrarNuevoPerfil(oPerfil);
        }

        public String Delete_ProfileComp(Perfil profile)
        {
            return objMethod.Delete_ProfileComp(profile);
        }

        public ListaPerfil GetListaPerfil(Perfil oPerfil)
        {
            return objMethod.GetListaPerfil(oPerfil);
        }


        public String InsertarMenuPerfil(Perfil oPerfil)
        {
            return objMethod.InsertarMenuPerfil(oPerfil);
        }

        public String DeleteMenuPerfil(Perfil oPerfil)
        {
            return objMethod.DeleteMenuPerfil(oPerfil);
        }

        public ListaEmpleados ValidarExisteEmpleado(String DNI)
        {
            return objMethod.ValidarExisteEmpleado(DNI);
        }

        public String BloquearUsuario(Usuario oUsuario)
        {
            return objMethod.BloquearUsuario(oUsuario);
        }


        //Correo
        public String InsertarCorreo(Correo oCorreo)
        {
            return objMethod.InsertarCorreo(oCorreo);
        }

        public String ActualizarCorreo(Correo oCorreo)
        {
            return objMethod.ActualizarCorreo(oCorreo);
        }

        public String DeleteCorreo(Correo oCorreo)
        {
            return objMethod.DeleteCorreo(oCorreo);
        }

        public ListaCorreo GetListaCorreo(Empresa oEmpresa)
        {
            return objMethod.GetListaCorreo(oEmpresa);
        }

        public ListaCorreo ValidarExistsCorreoEmpresa(Correo oCorreo)
        {
            return objMethod.ValidarExistsCorreoEmpresa(oCorreo);
        }
        #endregion

        #region Ky

        public ListaSum41 GetKySum41ToDeep(string value1, string value2, string value3, string value4, string value5)
        {
            return objMethod.GetKySum41ToDeep(value1, value2, value3, value4, value5);
        }


        public ListaEmpresa GetobjEntity(string value)
        {
            return objMethod.GetobjEntity(value);
        }

        public ListaEmpresa GetEntityEmpresa(string entityId)
        {
            return objMethod.GetEntityEmpresa(entityId);
        }


        public Empresa GetobjEntitySingle(string value)
        {
            return objMethod.GetobjEntitySingle(value);
        }

        public AmbienteTrabjActual GetAmbienteTrabjActual(string RucEntity)
        {
            return objMethod.GetAmbienteTrabjActual(RucEntity);
        }

        public String UpdateAmbTrabjActual(AmbienteTrabjActual objAmb)
        {
            return objMethod.UpdateAmbTrabjActual(objAmb);
        }

        public ListaAmbienteSunat GetListAmbTrabj()
        {
            return objMethod.GetListAmbTrabj();
        }

        #endregion

        #region Get Info Entity

        public Empresa GetCredentialEntity(string RucEmpresa)
        {
            return objMethod.GetCredentialEntity(RucEmpresa);
        }


        public ListaEmail GetListTypeMailEntity()
        {
            return objMethod.GetListTypeMailEntity();
        }

        public ListSSL GetListUseProt_SSL()
        {
            return objMethod.GetListUseProt_SSL();
        }

        #endregion

        #region AUDITORIA

        public Usuario GetDataFromUserLogueo(string Username)
        {
            return objMethod.GetDataFromUserLogueo(Username);
        }

        public Int32 InsertRegistroLogueo(LogueoClass ObjLogeo)
        {
            return objMethod.InsertRegistroLogueo(ObjLogeo);
        }

        public String UpdateRegistroLogueo(LogueoClass ObjLogeo)
        {
            return objMethod.UpdateRegistroLogueo(ObjLogeo);
        }

        public String DeleteRegistroLogueox3M(string FechaDesde, string RucEntity)
        {
            return objMethod.DeleteRegistroLogueox3M(FechaDesde, RucEntity);
        }

        public ListaLogueoClass GetListLogueoClass(LogueoClass objLogueo)
        {
            return objMethod.GetListLogueoClass(objLogueo);
        }

        public Int32 InsertLogLogueo(LogueoClass ObjLogeo)
        {
            return objMethod.InsertLogLogueo(ObjLogeo);
        }

        public ListaLogueoClass GetListLogLogueo(LogueoClass objlog)
        {
            return objMethod.GetListLogLogueo(objlog);
        }

        #endregion

        #region LOG ADE

        public Int32 InsertLogADE(LogAde objlog)
        {
            return objMethod.InsertLogADE(objlog);
        }

        public ListaLogAde GetListLogADE(LogAde objlog)
        {
            return objMethod.GetListLogADE(objlog);
        }

        #endregion END LOG ADE

        #region UTIL LOG

        public ListaTipoLog GetListTipoLog()
        {
            return objMethod.GetListTipoLog();
        }

        #endregion


        #region SEDE

        public ListaSede GetListSede(string RucEntity)
        {
            return objMethod.GetListSede(RucEntity);
        }

        #endregion

        #region user change company
        public String Update_UserCompany(Usuario ouser)
        {
            return objMethod.Update_UserCompany(ouser);
        }

        public ListaPerfil GetList_ProfileCompany(int IdComp)
        {
            return objMethod.GetList_ProfileCompany(IdComp);
        }

        public ListAuthenticate Get_ListAuthenticate()
        {
            return objMethod.Get_ListAuthenticate();
        }
        #endregion

        #region mail to alert

        public ListaCorreo GetList_NotificationsMail(string ruccomp)
        {
            return objMethod.GetList_NotificationsMail(ruccomp);
        }

        public String Insert_MailToAlert(Correo oCorreo)
        {
            return objMethod.Insert_MailToAlert(oCorreo);
        }

        public String Update_MailToAlert(Correo oCorreo)
        {
            return objMethod.Update_MailToAlert(oCorreo);
        }

        public String Delete_MailToAlert(Correo oCorreo)
        {
            return objMethod.Delete_MailToAlert(oCorreo);
        }

        #endregion

        #region get id
        public Empresa GetId(string ruccomp)
        {
            return objMethod.GetId(ruccomp);
        }
        #endregion
    }
}
