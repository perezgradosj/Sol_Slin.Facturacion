using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Slin.Facturacion.BusinessEntities;
using System.ServiceModel.Web;

namespace Host.IIS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IServicioSeguridad" in both code and config file together.
    [ServiceContract]
    public interface IServicioSeguridad
    {
        #region SEGURIDAD

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "show/json/{name}")]
        Int32 ValidarAcceso(Usuario oUsuario);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "show/xml/{name}")]
        String ActualizarContrasenia(Usuario oUsuario);

        [OperationContract]
        Usuario GetUsuarioLogeado(Usuario oUsuario);


        #endregion


        #region USUARIO DATAACCESS

        [OperationContract]
        String RegistrarUsuario(Usuario oUsuario);

        [OperationContract]
        String InsertarUsuario(Usuario oUsuario);

        [OperationContract]
        String ActualizarUsuario(Usuario oUsuario);

        [OperationContract]
        ListaUsuario GetListaUsuario(Usuario oUsuario);

        [OperationContract]
        ListaUsuario ValidarUsername(String Username);

        [OperationContract]
        ListaUsuario ValidarDniUsuario(String Dni_Ruc);

        [OperationContract]
        ListaUsuario InsertarUsuario_ForExcel(ListaUsuario olistausuario);

        [OperationContract]
        ListaMenu GetListaMenu();

        [OperationContract]
        ListaMenu GetListarMenuPerfil(Perfil oPerfil);

        [OperationContract]
        ListaPerfil GetListaPerfiles(string RucEntity);

        [OperationContract]
        ListaMenu GetListarMenu(Usuario oUsuario);

        [OperationContract]
        String RegistrarUsuarioPerfil(Usuario oUsuario);

        [OperationContract]
        String DeleteUsuarioPerfil(Usuario oUsuario);

        [OperationContract]
        ListaRol GetListadoRol();

        [OperationContract]
        ListaRol GetListaRolesUsuario(Usuario oUsuario);

        [OperationContract]
        String RegistrarUsuarioRol(Usuario oUsuario);

        [OperationContract]
        String DeleteUsuarioRol(Usuario oUsuario);

        [OperationContract]
        Int32 ObtenerNuevoIdUsuario();

        [OperationContract]
        String RegistrarNuevoPerfil(Perfil oPerfil);

        [OperationContract]
        String Delete_ProfileComp(Perfil profile);

        [OperationContract]
        ListaPerfil GetListaPerfil(Perfil oPerfil);

        [OperationContract]
        String InsertarMenuPerfil(Perfil oPerfil);

        [OperationContract]
        String DeleteMenuPerfil(Perfil oPerfil);

        [OperationContract]
        ListaEmpleados ValidarExisteEmpleado(String DNI);

        [OperationContract]
        String BloquearUsuario(Usuario oUsuario);


        //Correo
        [OperationContract]
        String InsertarCorreo(Correo oCorreo);

        [OperationContract]
        String ActualizarCorreo(Correo oCorreo);

        [OperationContract]
        String DeleteCorreo(Correo oCorreo);

        [OperationContract]
        ListaCorreo GetListaCorreo(Empresa oEmpresa);

        [OperationContract]
        ListaCorreo ValidarExistsCorreoEmpresa(Correo oCorreo);

        #endregion

        #region Ky

        [OperationContract]
        ListaSum41 GetKySum41ToDeep(string value1, string value2, string value3, string value4, string value5);

        [OperationContract]
        ListaEmpresa GetobjEntity(string value);

        [OperationContract]
        Empresa GetobjEntitySingle(string value);

        [OperationContract]
        ListaEmpresa GetEntityEmpresa(string entityId);

        [OperationContract]
        AmbienteTrabjActual GetAmbienteTrabjActual(string RucEntity);

        [OperationContract]
        String UpdateAmbTrabjActual(AmbienteTrabjActual objAmb);

        [OperationContract]
        ListaAmbienteSunat GetListAmbTrabj();

        #endregion

        #region Get Info Entity

        [OperationContract]
        Empresa GetCredentialEntity(string RucEmpresa);

        [OperationContract]
        ListaEmail GetListTypeMailEntity();

        [OperationContract]
        ListSSL GetListUseProt_SSL();

        #endregion

        #region AUDITORIA

        [OperationContract]
        Usuario GetDataFromUserLogueo(string Username);

        [OperationContract]
        Int32 InsertRegistroLogueo(LogueoClass ObjLogeo);

        [OperationContract]
        String UpdateRegistroLogueo(LogueoClass ObjLogeo);

        [OperationContract]
        String DeleteRegistroLogueox3M(string FechaDesde, string RucEntity);

        [OperationContract]
        ListaLogueoClass GetListLogueoClass(LogueoClass objLogueo);

        [OperationContract]
        Int32 InsertLogLogueo(LogueoClass ObjLogeo);

        [OperationContract]
        ListaLogueoClass GetListLogLogueo(LogueoClass objlog);

        #endregion

        #region LOG ADE

        [OperationContract]
        Int32 InsertLogADE(LogAde objlog);

        [OperationContract]
        ListaLogAde GetListLogADE(LogAde objlog);

        #endregion END LOG ADE

        #region UTIL LOG

        [OperationContract]
        ListaTipoLog GetListTipoLog();

        #endregion

        #region SEDE

        [OperationContract]
        ListaSede GetListSede(string RucEntity);

        #endregion

        #region user change company
        [OperationContract]
        String Update_UserCompany(Usuario ouser);

        [OperationContract]
        ListaPerfil GetList_ProfileCompany(int IdComp);

        [OperationContract]
        ListAuthenticate Get_ListAuthenticate();
        #endregion

        #region mail to alert
        [OperationContract]
        ListaCorreo GetList_NotificationsMail(string ruccomp);

        [OperationContract]
        String Insert_MailToAlert(Correo oCorreo);

        [OperationContract]
        String Update_MailToAlert(Correo oCorreo);

        [OperationContract]
        String Delete_MailToAlert(Correo oCorreo);
        #endregion

        #region get id
        [OperationContract]
        Empresa GetId(string ruccomp);
        #endregion
    }
}
