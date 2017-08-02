using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Slin.Facturacion.BusinessEntities;
using Slin.Facturacion.DataAccess;
using Slin.Facturacion.BusinessEntities.Configuracion;

namespace Slin.Facturacion.ServiceImplementation
{
    public class ServicioSeguridadSOA
    {
        //private static readonly ServicioSeguridadSOA instance = new ServicioSeguridadSOA();
        //static ServicioSeguridadSOA() { }
        //private ServicioSeguridadSOA() { }
        //public static ServicioSeguridadSOA Instance { get { return instance; } }


        public SeguridadDataAccess objSeguridadDataAccess = new SeguridadDataAccess();


        #region SEGURIDAD
        public Int32 ValidarAcceso(Usuario oUsuario)
        {
            return objSeguridadDataAccess.ValidarAcceso(oUsuario);
        }

        public String ActualizarContrasenia(Usuario oUsuario)
        {
            return objSeguridadDataAccess.ActualizarContrasenia(oUsuario);
        }

        public Usuario GetUsuarioLogeado(Usuario oUsuario)
        {
            return objSeguridadDataAccess.GetUsuarioLogeado(oUsuario);
        }



        #endregion

        #region USUARIO DATAACCESS

        UsuarioDataAccess objUsuarioDataAccess = new UsuarioDataAccess();

        public string RegistrarUsuario(Usuario oUsuario)
        {
            return objUsuarioDataAccess.RegistrarUsuario(oUsuario);
        }

        public string InsertarUsuario(Usuario oUsuario)
        {
            return objUsuarioDataAccess.InsertarUsuario(oUsuario);
        }

        public string ActualizarUsuario(Usuario oUsuario)
        {
            return objUsuarioDataAccess.ActualizarUsuario(oUsuario);
        }

        public ListaUsuario GetListaUsuario(Usuario oUsuario)
        {
            return objUsuarioDataAccess.GetListaUsuario(oUsuario);
        }

        public ListaUsuario ValidarUsername(String Username)
        {
            return objUsuarioDataAccess.ValidarUsername(Username);
        }

        public ListaUsuario ValidarDniUsuario(String Dni_Ruc)
        {
            return objUsuarioDataAccess.ValidarDniUsuario(Dni_Ruc);
        }

        public ListaUsuario InsertarUsuario_ForExcel(ListaUsuario olistausuario)
        {
            return objUsuarioDataAccess.InsertarUsuario_ForExcel(olistausuario);
        }

        public ListaMenu GetListaMenu()
        {
            return objSeguridadDataAccess.GetListaMenu();
        }

        public ListaPerfil GetListaPerfiles(string RucEntity)
        {
            return objSeguridadDataAccess.GetListaPerfiles(RucEntity);
        }

        public ListaMenu GetListarMenu(Usuario oUsuario)
        {
            return objSeguridadDataAccess.GetListarMenu(oUsuario);
        }


        public ListaMenu GetListarMenuPerfil(Perfil oPerfil)
        {
            return objSeguridadDataAccess.GetListarMenuPerfil(oPerfil);
        }


        public string RegistrarUsuarioPerfil(Usuario oUsuario)
        {
            return objSeguridadDataAccess.RegistrarUsuarioPerfil(oUsuario);
        }

        public string DeleteUsuarioPerfil(Usuario oUsuario)
        {
            return objSeguridadDataAccess.DeleteUsuarioPerfil(oUsuario);
        }



        public ListaRol GetListadoRol()
        {
            return objSeguridadDataAccess.GetListadoRol();
        }

        public ListaRol GetListaRolesUsuario(Usuario oUsuario)
        {
            return objSeguridadDataAccess.GetListaRolesUsuario(oUsuario);
        }

        public string RegistrarUsuarioRol(Usuario oUsuario)
        {
            return objSeguridadDataAccess.RegistrarUsuarioRol(oUsuario);
        }

        public string DeleteUsuarioRol(Usuario oUsuario)
        {
            return objSeguridadDataAccess.DeleteUsuarioRol(oUsuario);
        }

        public Int32 ObtenerNuevoIdUsuario()
        {
            return objSeguridadDataAccess.ObtenerNuevoIdUsuario();
        }


        public string RegistrarNuevoPerfil(Perfil oPerfil)
        {
            return objSeguridadDataAccess.RegistrarNuevoPerfil(oPerfil);
        }

        public string Delete_ProfileComp(Perfil profile)
        {
            return objSeguridadDataAccess.Delete_ProfileComp(profile);
        }

        public ListaPerfil GetListaPerfil(Perfil oPerfil)
        {
            return objSeguridadDataAccess.GetListaPerfil(oPerfil);
        }

        public string InsertarMenuPerfil(Perfil oPerfil)
        {
            return objSeguridadDataAccess.InsertarMenuPerfil(oPerfil);
        }

        public string DeleteMenuPerfil(Perfil oPerfil)
        {
            return objSeguridadDataAccess.DeleteMenuPerfil(oPerfil);
        }

        public ListaEmpleados ValidarExisteEmpleado(String DNI)
        {
            return objUsuarioDataAccess.ValidarExisteEmpleado(DNI);
        }

        public string BloquearUsuario(Usuario oUsuario)
        {
            return objSeguridadDataAccess.BloquearUsuario(oUsuario);
        }




        //Correo
        public string InsertarCorreo(Correo oCorreo)
        {
            return objSeguridadDataAccess.InsertarCorreo(oCorreo);
        }

        public string ActualizarCorreo(Correo oCorreo)
        {
            return objSeguridadDataAccess.ActualizarCorreo(oCorreo);
        }

        public string DeleteCorreo(Correo oCorreo)
        {
            return objSeguridadDataAccess.DeleteCorreo(oCorreo);
        }

        public ListaCorreo GetListaCorreo(Empresa oEmpresa)
        {
            return objSeguridadDataAccess.GetListaCorreo(oEmpresa);
        }

        public ListaCorreo ValidarExistsCorreoEmpresa(Correo oCorreo)
        {
            return objSeguridadDataAccess.ValidarExistsCorreoEmpresa(oCorreo);
        }
        #endregion

        #region Ky

        public ListaSum41 GetKySum41ToDeep(string value1, string value2, string value3, string value4, string value5)
        {
            return objSeguridadDataAccess.GetKySum41ToDeep(value1, value2, value3, value4, value5);
        }


        public ListaEmpresa GetobjEntity(string value)
        {
            return objSeguridadDataAccess.GetobjEntity(value);
        }


        public Empresa GetobjEntitySingle(string value)
        {
            return objSeguridadDataAccess.GetobjEntitySingle(value);
        }

        public ListaEmpresa GetEntityEmpresa(string entityId)
        {
            return objSeguridadDataAccess.GetEntityEmpresa(entityId);
        }

        public AmbienteTrabjActual GetAmbienteTrabjActual(string RucEntity)
        {
            return objSeguridadDataAccess.GetAmbienteTrabjActual(RucEntity);
        }

        public String UpdateAmbTrabjActual(AmbienteTrabjActual objAmb)
        {
            return objSeguridadDataAccess.UpdateAmbTrabjActual(objAmb);
        }

        public ListaAmbienteSunat GetListAmbTrabj()
        {
            return objSeguridadDataAccess.GetListAmbTrabj();
        }

        #endregion

        #region Get Info Entity

        public Empresa GetCredentialEntity(string RucEmpresa)
        {
            return objSeguridadDataAccess.GetCredentialEntity(RucEmpresa);
        }

        public Empresa GetCredentialEntity_Received(string RucEmpresa)
        {
            return objSeguridadDataAccess.GetCredentialEntity_Received(RucEmpresa);
        }


        public ListaEmail GetListTypeMailEntity()
        {
            return objSeguridadDataAccess.GetListTypeMailEntity();
        }

        public ListSSL GetListUseProt_SSL()
        {
            return objSeguridadDataAccess.GetListUseProt_SSL();
        }

        #endregion

        #region AUDITORIA

        public Usuario GetDataFromUserLogueo(string Username)
        {
            return objSeguridadDataAccess.GetDataFromUserLogueo(Username);
        }

        public Int32 InsertRegistroLogueo(LogueoClass ObjLogeo)
        {
            return objSeguridadDataAccess.InsertRegistroLogueo(ObjLogeo);
        }

        public String UpdateRegistroLogueo(LogueoClass ObjLogeo)
        {
            return objSeguridadDataAccess.UpdateRegistroLogueo(ObjLogeo);
        }

        public String DeleteRegistroLogueox3M(string FechaDesde, string RucEntity)
        {
            return objSeguridadDataAccess.DeleteRegistroLogueox3M(FechaDesde, RucEntity);
        }

        public ListaLogueoClass GetListLogueoClass(LogueoClass objLogueo)
        {
            return objSeguridadDataAccess.GetListLogueoClass(objLogueo);
        }

        public Int32 InsertLogLogueo(LogueoClass ObjLogeo)
        {
            return objSeguridadDataAccess.InsertLogLogueo(ObjLogeo);
        }

        public ListaLogueoClass GetListLogLogueo(LogueoClass objlog)
        {
            return objSeguridadDataAccess.GetListLogLogueo(objlog);
        }

        #endregion

        #region LOG ADE

        public Int32 InsertLogADE(LogAde objlog)
        {
            return objSeguridadDataAccess.InsertLogADE(objlog);
        }

        public ListaLogAde GetListLogADE(LogAde objlog)
        {
            return objSeguridadDataAccess.GetListLogADE(objlog);
        }

        #endregion END LOG ADE

        #region UTIL LOG

        public ListaTipoLog GetListTipoLog()
        {
            return objSeguridadDataAccess.GetListTipoLog();
        }

        #endregion

        #region SEDE

        public ListaSede GetListSede(string RucEntity)
        {
            return objSeguridadDataAccess.GetListSede(RucEntity);
        }

        #endregion

        #region valida statuc service for company
        public ListService GetStatus_WService(string entityId, string NameService)
        {
            return objSeguridadDataAccess.GetStatus_WService(entityId, NameService);
        }
        #endregion

        #region user change company
        public String Update_UserCompany(Usuario ouser)
        {
            return objSeguridadDataAccess.Update_UserCompany(ouser);
        }

        public ListaPerfil GetList_ProfileCompany(int IdComp)
        {
            return objSeguridadDataAccess.GetList_ProfileCompany(IdComp);
        }

        public ListAuthenticate Get_ListAuthenticate()
        {
            return objSeguridadDataAccess.Get_ListAuthenticate();
        }
        #endregion

        #region mail to alert

        public ListaCorreo GetList_NotificationsMail(string ruccomp)
        {
            return objSeguridadDataAccess.GetList_NotificationsMail(ruccomp);
        }

        public String Insert_MailToAlert(Correo oCorreo)
        {
            return objSeguridadDataAccess.Insert_MailToAlert(oCorreo);
        }

        public String Update_MailToAlert(Correo oCorreo)
        {
            return objSeguridadDataAccess.Update_MailToAlert(oCorreo);
        }

        public String Delete_MailToAlert(Correo oCorreo)
        {
            return objSeguridadDataAccess.Delete_MailToAlert(oCorreo);
        }

        #endregion

        #region get id
        public Empresa GetId(string ruccomp)
        {
            return objSeguridadDataAccess.GetId(ruccomp);
        }
        #endregion
    }
}
