using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Slin.Facturacion.Proxies.ServicioSeguridad;

using Slin.Facturacion.ServiceController;

namespace Slin.Facturacion.Electronica.Web.ServiceFacturacion
{
    public sealed class ServiceSeguridadController
    {
        private static readonly ServiceSeguridadController instance = new ServiceSeguridadController();
        static ServiceSeguridadController() { }
        private ServiceSeguridadController() { }
        public static ServiceSeguridadController Instance { get { return instance; } }


        #region SEGURIDAD

        public  Int32 ValidarAcceso(Usuario oUsuario)
        {
            return SeguridadServiceController.Instance.ValidarAcceso(oUsuario);
        }

        public  String ActualizarContrasenia(Usuario oUsuario)
        {
            return SeguridadServiceController.Instance.ActualizarContrasenia(oUsuario);
        }

        public  Usuario GetUsuarioLogeado(Usuario oUsuario)
        {
            return SeguridadServiceController.Instance.GetUsuarioLogeado(oUsuario);
        }

        #endregion



        #region Usuario

        public  ListaUsuario GetListaUsuario(Usuario oUsuario)
        {
            return SeguridadServiceController.Instance.GetListaUsuario(oUsuario);
        }


        public  String RegistrarUsuario(Usuario oUsuario)
        {
            return SeguridadServiceController.Instance.RegistrarUsuario(oUsuario);
        }

        public  String InsertarUsuario(Usuario oUsuario)
        {
            return SeguridadServiceController.Instance.InsertarUsuario(oUsuario);
        }


        public  String ActualizarUsuario(Usuario oUsuario)
        {
            return SeguridadServiceController.Instance.ActualizarUsuario(oUsuario);
        }

        public  ListaUsuario ValidarUsername(String Username)
        {
            return SeguridadServiceController.Instance.ValidarUsername(Username);
        }


        public  ListaUsuario ValidarDniUsuario(String Dni_Ruc)
        {
            return SeguridadServiceController.Instance.ValidarDniUsuario(Dni_Ruc);
        }

        public  ListaUsuario InsertarUsuario_ForExcel(ListaUsuario olistausuario)
        {
            return SeguridadServiceController.Instance.InsertarUsuario_ForExcel(olistausuario);
        }

        #endregion


        #region MENU

        public  ListaMenu GetListaMenu()
        {
            return SeguridadServiceController.Instance.GetListaMenu();
        }

        public  ListaMenu GetListarMenuPerfil(Perfil oPerfil)
        {
            return SeguridadServiceController.Instance.GetListarMenuPerfil(oPerfil);
        }

        public  ListaPerfil GetListaPerfiles(string RucEntity)
        {
            return SeguridadServiceController.Instance.GetListaPerfiles(RucEntity);
        }

        public  ListaMenu GetListarMenu(Usuario oUsuario)
        {
            return SeguridadServiceController.Instance.GetListarMenu(oUsuario);
        }



        public  String RegistrarUsuarioPerfil(Usuario oUsuario)
        {
            return SeguridadServiceController.Instance.RegistrarUsuarioPerfil(oUsuario);
        }

        public  String DeleteUsuarioPerfil(Usuario oUsuario)
        {
            return SeguridadServiceController.Instance.DeleteUsuarioPerfil(oUsuario);
        }





        public  ListaRol GetListadoRol()
        {
            return SeguridadServiceController.Instance.GetListadoRol();
        }

        public  ListaRol GetListaRolesUsuario(Usuario oUsuario)
        {
            return SeguridadServiceController.Instance.GetListaRolesUsuario(oUsuario);
        }

        public  String RegistrarUsuarioRol(Usuario oUsuario)
        {
            return SeguridadServiceController.Instance.RegistrarUsuarioRol(oUsuario);
        }

        public  String DeleteUsuarioRol(Usuario oUsuario)
        {
            return SeguridadServiceController.Instance.DeleteUsuarioRol(oUsuario);
        }

        public  Int32 ObtenerNuevoIdUsuario()
        {
            return SeguridadServiceController.Instance.ObtenerNuevoIdUsuario();
        }

        public  String RegistrarNuevoPerfil(Perfil oPerfil)
        {
            return SeguridadServiceController.Instance.RegistrarNuevoPerfil(oPerfil);
        }

        public  String Delete_ProfileComp(Perfil profile)
        {
            return SeguridadServiceController.Instance.Delete_ProfileComp(profile);
        }

        public  ListaPerfil GetListaPerfil(Perfil oPerfil)
        {
            return SeguridadServiceController.Instance.GetListaPerfil(oPerfil);
        }

        public  String InsertarMenuPerfil(Perfil oPerfil)
        {
            return SeguridadServiceController.Instance.InsertarMenuPerfil(oPerfil);
        }


        public  String DeleteMenuPerfil(Perfil oPerfil)
        {
            return SeguridadServiceController.Instance.DeleteMenuPerfil(oPerfil);
        }

        public  ListaEmpleados ValidarExisteEmpleado(String DNI)
        {
            return SeguridadServiceController.Instance.ValidarExisteEmpleado(DNI);
        }


        public  String BloquearUsuario(Usuario oUsuario)
        {
            return SeguridadServiceController.Instance.BloquearUsuario(oUsuario);
        }




        //Correo
        public  String InsertarCorreo(Correo oCorreo)
        {
            return SeguridadServiceController.Instance.InsertarCorreo(oCorreo);
        }

        public  String ActualizarCorreo(Correo oCorreo)
        {
            return SeguridadServiceController.Instance.ActualizarCorreo(oCorreo);
        }


        public  String DeleteCorreo(Correo oCorreo)
        {
            return SeguridadServiceController.Instance.DeleteCorreo(oCorreo);
        }

        public  ListaCorreo GetListaCorreo(Empresa oEmpresa)
        {
            return SeguridadServiceController.Instance.GetListaCorreo(oEmpresa);
        }

        public  ListaCorreo ValidarExistsCorreoEmpresa(Correo oCorreo)
        {
            return SeguridadServiceController.Instance.ValidarExistsCorreoEmpresa(oCorreo);
        }
        #endregion


        #region Ky

        public  ListaSum41 GetKySum41ToDeep(string value1, string value2, string value3, string value4, string value5)
        {
            return SeguridadServiceController.Instance.GetKySum41ToDeep(value1, value2, value3, value4, value5);
        }


        public  ListaEmpresa GetobjEntity(string value)
        {
            return SeguridadServiceController.Instance.GetobjEntity(value);
        }


        public  Empresa GetobjEntitySingle(string value)
        {
            return SeguridadServiceController.Instance.GetobjEntitySingle(value);
        }

        public  ListaEmpresa GetEntityEmpresa(string entityId)
        {
            return SeguridadServiceController.Instance.GetEntityEmpresa(entityId);
        }

        public  AmbienteTrabjActual GetAmbienteTrabjActual(string RucEntity)
        {
            return SeguridadServiceController.Instance.GetAmbienteTrabjActual(RucEntity);
        }

        public  String UpdateAmbTrabjActual(AmbienteTrabjActual objAmb)
        {
            return SeguridadServiceController.Instance.UpdateAmbTrabjActual(objAmb);
        }

        public  ListaAmbienteSunat GetListAmbTrabj()
        {
            return SeguridadServiceController.Instance.GetListAmbTrabj();
        }

        #endregion

        #region Get Info Entity

        public  Empresa GetCredentialEntity(string RucEmpresa)
        {
            return SeguridadServiceController.Instance.GetCredentialEntity(RucEmpresa);
        }

        public  ListaEmail GetListTypeMailEntity()
        {
            return SeguridadServiceController.Instance.GetListTypeMailEntity();
        }

        public  ListSSL GetListUseProt_SSL()
        {
            return SeguridadServiceController.Instance.GetListUseProt_SSL();
        }

        #endregion

        #region AUDITORIA

        public  Usuario GetDataFromUserLogueo(string Username)
        {
            return SeguridadServiceController.Instance.GetDataFromUserLogueo(Username);
        }

        public Int32 InsertRegistroLogueo(LogueoClass ObjLogeo)
        {
            return SeguridadServiceController.Instance.InsertRegistroLogueo(ObjLogeo);
        }

        public  String UpdateRegistroLogueo(LogueoClass ObjLogeo)
        {
            return SeguridadServiceController.Instance.UpdateRegistroLogueo(ObjLogeo);
        }

        public  String DeleteRegistroLogueox3M(string FechaDesde, string RucEntity)
        {
            return SeguridadServiceController.Instance.DeleteRegistroLogueox3M(FechaDesde, RucEntity);
        }

        public  ListaLogueoClass GetListLogueoClass(LogueoClass objLogueo)
        {
            return SeguridadServiceController.Instance.GetListLogueoClass(objLogueo);
        }

        public  Int32 InsertLogLogueo(LogueoClass ObjLogeo)
        {
            return SeguridadServiceController.Instance.InsertLogLogueo(ObjLogeo);
        }

        public  ListaLogueoClass GetListLogLogueo(LogueoClass objlog)
        {
            return SeguridadServiceController.Instance.GetListLogLogueo(objlog);
        }

        #endregion

        #region LOG ADE

        public  Int32 InsertLogADE(LogAde objlog)
        {
            return SeguridadServiceController.Instance.InsertLogADE(objlog);
        }

        public  ListaLogAde GetListLogADE(LogAde objlog)
        {
            return SeguridadServiceController.Instance.GetListLogADE(objlog);
        }

        #endregion

        #region UTIL LOG

        public  ListaTipoLog GetListTipoLog()
        {
            return SeguridadServiceController.Instance.GetListTipoLog();
        }

        #endregion

        #region SEDE

        public  ListaSede GetListSede(string RucEntity)
        {
            return SeguridadServiceController.Instance.GetListSede(RucEntity);
        }

        #endregion

        #region user change company
        public  String Update_UserCompany(Usuario ouser)
        {
            return SeguridadServiceController.Instance.Update_UserCompany(ouser);
        }

        public  ListaPerfil GetList_ProfileCompany(int IdComp)
        {
            return SeguridadServiceController.Instance.GetList_ProfileCompany(IdComp);
        }

        public  ListAuthenticate Get_ListAuthenticate()
        {
            return SeguridadServiceController.Instance.Get_ListAuthenticate();
        }

        #endregion

        #region mail to alert

        public  ListaCorreo GetList_NotificationsMail(string ruccomp)
        {
            return SeguridadServiceController.Instance.GetList_NotificationsMail(ruccomp);
        }

        public  String Insert_MailToAlert(Correo oCorreo)
        {
            return SeguridadServiceController.Instance.Insert_MailToAlert(oCorreo);
        }

        public  String Update_MailToAlert(Correo oCorreo)
        {
            return SeguridadServiceController.Instance.Update_MailToAlert(oCorreo);
        }

        public  String Delete_MailToAlert(Correo oCorreo)
        {
            return SeguridadServiceController.Instance.Delete_MailToAlert(oCorreo);
        }

        #endregion

        #region get id
        public Empresa GetId(string ruccomp)
        {
            return SeguridadServiceController.Instance.GetId(ruccomp);
        }
        #endregion
    }
}