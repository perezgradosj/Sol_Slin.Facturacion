using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slin.Facturacion.InstallCommon
{
    public class Procedimientos
    {
        public const string usp_registrCompany = "[Mtro].[Usp_InsertarEmpresa]";
        public const string usp_registrUserRoot = "[Seg].[Usp_InsertarUsuarios]";
        public const string usp_getlistmenu = "[Seg].[Usp_ListarMenu]";
        public const string usp_registrRolUserRoot = "[Seg].[Usp_RegistrarUsuarioRol]";
        public const string usp_registrmenuperfil = "[Seg].[Usp_InsertarMenuPerfil]";
        public const string usp_registrambienttrabj = "[Seg].[Usp_UpdateAmbTrabjActual]";
        public const string usp_getlistambient = "[Seg].[Usp_GetListAmbiente]";
        public const string usp_getlisttypedocument = "[Ctl].[Usp_ListaTipoDocumento]";
        public const string usp_insertcredentialcertificate = "[Conf].[Usp_InsertCredentialCertificateAmb]";
        public const string usp_getlistroles = "[Seg].[Usp_ListadoRoles]";
        public const string usp_getlistperfil = "[Seg].[Usp_ListadoPerfiles]";
        public const string usp_insert_configmain = "[Fact].[Usp_Insert_ConfigMain]";
        public const string usp_insertmail = "[Seg].[Usp_InsertarCorreo]";
         
        public const string usp_registrcompany_ok = "[Seg].[Usp_RegistrCompany_Ok]";

        public const string usp_registerComp = "[Seg].[Usp_RegisterComp]";
        public const string usp_registerUserComp = "[Seg].[Usp_RegisterUserComp]";
        public const string usp_registrperfilComp = "[Seg].[Usp_RegistrPerfilComp]";

        public const string usp_getlist_timeservice = "[Conf].[Usp_GetList_TimeService]";
        public const string usp_register_timeservicecompany = "[Conf].[Usp_Register_TimeServiceCompany]";

        public const string usp_registercertificateinformation = "[Conf].[Usp_RegisterCertificateInformation]";
    }
}
