using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Slin.Facturacion.Proxies.ServicioSeguridad;

namespace Slin.Facturacion.ServiceController
{
    public sealed class SeguridadServiceController
    {
        private static readonly SeguridadServiceController instance = new SeguridadServiceController();
        static SeguridadServiceController() { }
        private SeguridadServiceController() { }
        public static SeguridadServiceController Instance { get { return instance; } }


        #region SEGURIDAD

        public Int32 ValidarAcceso(Usuario oUsuario)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.ValidarAcceso(oUsuario);
            }
        }

        public String ActualizarContrasenia(Usuario oUsuario)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.ActualizarContrasenia(oUsuario);
            }
        }

        public Usuario GetUsuarioLogeado(Usuario oUsuario)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.GetUsuarioLogeado(oUsuario);
            }
        }

        

        #endregion

        #region USUARIO DATAACCESS

        public String RegistrarUsuario(Usuario oUsuario)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.RegistrarUsuario(oUsuario);
            }
        }

        public String InsertarUsuario(Usuario oUsuario)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.InsertarUsuario(oUsuario);
            }
        }

        public String ActualizarUsuario(Usuario oUsuario)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.ActualizarUsuario(oUsuario);
            }
        }

        public ListaUsuario GetListaUsuario(Usuario oUsuario)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.GetListaUsuario(oUsuario);
            }
        }

        public ListaUsuario ValidarUsername(String Username)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.ValidarUsername(Username);
            }
        }

        public ListaUsuario ValidarDniUsuario(String Dni_Ruc)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.ValidarDniUsuario(Dni_Ruc);
            }
        }

        public ListaUsuario InsertarUsuario_ForExcel(ListaUsuario olistausuario)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.InsertarUsuario_ForExcel(olistausuario);
            }
        }


        public ListaMenu GetListaMenu()
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.GetListaMenu();
            }
        }

        public ListaMenu GetListarMenuPerfil(Perfil oPerfil)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.GetListarMenuPerfil(oPerfil);
            }
        }

        public ListaPerfil GetListaPerfiles(string RucEntity)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.GetListaPerfiles(RucEntity);
            }
        }


        public ListaMenu GetListarMenu(Usuario oUsuario)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.GetListarMenu(oUsuario);
            }
        }



        public String RegistrarUsuarioPerfil(Usuario oUsuario)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.RegistrarUsuarioPerfil(oUsuario);
            }
        }


        public String DeleteUsuarioPerfil(Usuario oUsuario)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.DeleteUsuarioPerfil(oUsuario);
            }
        }



        public ListaRol GetListadoRol()
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.GetListadoRol();
            }
        }

        public ListaRol GetListaRolesUsuario(Usuario oUsuario)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.GetListaRolesUsuario(oUsuario);
            }
        }

        public String RegistrarUsuarioRol(Usuario oUsuario)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.RegistrarUsuarioRol(oUsuario);
            }
        }

        public String DeleteUsuarioRol(Usuario oUsuario)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.DeleteUsuarioRol(oUsuario);
            }
        }

        public Int32 ObtenerNuevoIdUsuario()
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.ObtenerNuevoIdUsuario();
            }
        }


        public String RegistrarNuevoPerfil(Perfil oPerfil)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.RegistrarNuevoPerfil(oPerfil);
            }
        }

        public String Delete_ProfileComp(Perfil profile)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.Delete_ProfileComp(profile);
            }
        }


        public ListaPerfil GetListaPerfil(Perfil oPerfil)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.GetListaPerfil(oPerfil);
            }
        }


        public String InsertarMenuPerfil(Perfil oPerfil)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.InsertarMenuPerfil(oPerfil);
            }
        }


        public String DeleteMenuPerfil(Perfil oPerfil)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.DeleteMenuPerfil(oPerfil);
            }
        }

        public ListaEmpleados ValidarExisteEmpleado(String DNI)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.ValidarExisteEmpleado(DNI);
            }
        }

        public String BloquearUsuario(Usuario oUsuario)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.BloquearUsuario(oUsuario);
            }
        }


        //Correo
        public String InsertarCorreo(Correo oCorreo)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.InsertarCorreo(oCorreo);
            }
        }

        public String ActualizarCorreo(Correo oCorreo)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.ActualizarCorreo(oCorreo);
            }
        }


        public String DeleteCorreo(Correo oCorreo)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.DeleteCorreo(oCorreo);
            }
        }

        public ListaCorreo GetListaCorreo(Empresa oEmpresa)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.GetListaCorreo(oEmpresa);
            }
        }

        public ListaCorreo ValidarExistsCorreoEmpresa(Correo oCorreo)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.ValidarExistsCorreoEmpresa(oCorreo);
            }
        }

        #endregion

        #region Ky

        public ListaSum41 GetKySum41ToDeep(string value1, string value2, string value3, string value4, string value5)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.GetKySum41ToDeep(value1, value2, value3, value4, value5);
            }
        }


        public ListaEmpresa GetobjEntity(string value)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.GetobjEntity(value);
            }
        }


        public Empresa GetobjEntitySingle(string value)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.GetobjEntitySingle(value);
            }
        }

        public ListaEmpresa GetEntityEmpresa(string entityId)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.GetEntityEmpresa(entityId);
            }
        }


        public AmbienteTrabjActual GetAmbienteTrabjActual(string RucEntity)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.GetAmbienteTrabjActual(RucEntity);
            }
        }

        public String UpdateAmbTrabjActual(AmbienteTrabjActual objAmb)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.UpdateAmbTrabjActual(objAmb);
            }
        }

        public ListaAmbienteSunat GetListAmbTrabj()
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.GetListAmbTrabj();
            }
        }

        #endregion

        #region Get Info Entity

        public Empresa GetCredentialEntity(string RucEmpresa)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.GetCredentialEntity(RucEmpresa);
            }
        }

        public ListaEmail GetListTypeMailEntity()
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.GetListTypeMailEntity();
            }
        }

        public ListSSL GetListUseProt_SSL()
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.GetListUseProt_SSL();
            }
        }

        #endregion

        #region AUDITORIA

        public Usuario GetDataFromUserLogueo(string Username)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.GetDataFromUserLogueo(Username);
            }
        }

        public Int32 InsertRegistroLogueo(LogueoClass ObjLogeo)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.InsertRegistroLogueo(ObjLogeo);
            }
        }

        public String UpdateRegistroLogueo(LogueoClass ObjLogeo)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.UpdateRegistroLogueo(ObjLogeo);
            }
        }

        public String DeleteRegistroLogueox3M(string FechaDesde, string RucEntity)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.DeleteRegistroLogueox3M(FechaDesde, RucEntity);
            }
        }

        public ListaLogueoClass GetListLogueoClass(LogueoClass objLogueo)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.GetListLogueoClass(objLogueo);
            }
        }

        public Int32 InsertLogLogueo(LogueoClass ObjLogeo)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.InsertLogLogueo(ObjLogeo);
            }
        }

        public ListaLogueoClass GetListLogLogueo(LogueoClass objlog)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.GetListLogLogueo(objlog);
            }
        }

        #endregion

        #region LOG ADE

        public Int32 InsertLogADE(LogAde objlog)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.InsertLogADE(objlog);
            }
        }

        public ListaLogAde GetListLogADE(LogAde objlog)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.GetListLogADE(objlog);
            }
        }

        #endregion

        #region UTIL LOG

        public ListaTipoLog GetListTipoLog()
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.GetListTipoLog();
            }
        }

        #endregion

        #region SEDE

        public ListaSede GetListSede(string RucEntity)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.GetListSede(RucEntity);
            }
        }

        #endregion

        #region user change company
        public String Update_UserCompany(Usuario ouser)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.Update_UserCompany(ouser);
            }
        }

        public ListaPerfil GetList_ProfileCompany(int IdComp)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.GetList_ProfileCompany(IdComp);
            }
        }

        public ListAuthenticate Get_ListAuthenticate()
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.Get_ListAuthenticate();
            }
        }

        #endregion

        #region mail to alert

        public ListaCorreo GetList_NotificationsMail(string ruccomp)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.GetList_NotificationsMail(ruccomp);
            }
        }

        public String Insert_MailToAlert(Correo oCorreo)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.Insert_MailToAlert(oCorreo);
            }
        }

        public String Update_MailToAlert(Correo oCorreo)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.Update_MailToAlert(oCorreo);
            }
        }

        public String Delete_MailToAlert(Correo oCorreo)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.Delete_MailToAlert(oCorreo);
            }
        }

        #endregion

        #region get id
        public Empresa GetId(string ruccomp)
        {
            using (ServicioSeguridadClient Client = new ServicioSeguridadClient())
            {
                return Client.GetId(ruccomp);
            }
        }
        #endregion
    }
}


