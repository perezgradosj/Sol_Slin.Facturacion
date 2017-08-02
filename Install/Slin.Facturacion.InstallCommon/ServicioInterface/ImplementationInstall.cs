using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slin.Facturacion.InstallCommon
{
    public class ImplementationInstall
    {
        static string HOST = string.Empty;
        static string BD = string.Empty;
        static string USER = string.Empty;
        static string PWD = string.Empty;

        InstallDataAccess ObjDataAccess;

        public ImplementationInstall(string host, string bd, string user, string pwd)
        {
            HOST = host;
            BD = bd;
            USER = user;
            PWD = pwd;
            ObjDataAccess = new InstallDataAccess(HOST, BD, USER, PWD);
        }        

        #region registr

        public int InsertCompany(Company objcompany)
        {
            return ObjDataAccess.InsertCompany(objcompany);
        }

        public int InsertUserRoot(User objUser)
        {
            return ObjDataAccess.InsertUserRoot(objUser);
        }

        public string Insert_RolUserRoot(UserRol objroluser)
        {
            return ObjDataAccess.Insert_RolUserRoot(objroluser);
        }


        public string Insert_MenuPerfil(ListMenuPerfil objlistmenuperf)
        {
            return ObjDataAccess.Insert_MenuPerfil(objlistmenuperf);
        }

        public ListMenu GetListMenu()
        {
            return ObjDataAccess.GetListMenu();
        }

        public string Insert_AmbientTrabj(int idamb, string ruccompany)
        {
            return ObjDataAccess.Insert_AmbientTrabj(idamb, ruccompany);
        }

        public ListAmbiente GetListAmbienteTrabaj()
        {
            return ObjDataAccess.GetListAmbienteTrabaj();
        }

        public ListTypeDocument GetListTypeDocument()
        {
            return ObjDataAccess.GetListTypeDocument();
        }

        public string Insert_CredentialCertificateAmb(CredentialCertificate objcred)
        {
            return ObjDataAccess.Insert_CredentialCertificateAmb(objcred);
        }

        public string Insert_MailCompany(Correo oMail)
        {
            return ObjDataAccess.Insert_MailCompany(oMail);
        }

        public ListRol GetList_Roles()
        {
            return ObjDataAccess.GetList_Roles();
        }

        public ListPerfil GetList_Perfil(string RucCompany)
        {
            return ObjDataAccess.GetList_Perfil(RucCompany);
        }


        public string Insert_ConfigMain(ConfigMain oConfigmain)
        {
            return ObjDataAccess.Insert_ConfigMain(oConfigmain);
        }

        #endregion


        public ListCompany GetListCompany(int id)
        {
            return ObjDataAccess.GetListCompany(id);
        }

        #region register company

        public string RegisterPerf_Comp(Perfil oPerf)
        {
            return ObjDataAccess.RegisterPerf_Comp(oPerf);
        }

        public string RegisterCompany_Portal(Company objcompany, Correo omail)
        {
            return ObjDataAccess.RegisterCompany_Portal(objcompany, omail);
        }

        public string RegisterUserCompany_Portal(User oUser)
        {
            return ObjDataAccess.RegisterUserCompany_Portal(oUser);
        }

        #endregion


        #region GET LIST SERVICES WINDOWS

        public ListSevices GetList_TimeService()
        {
            return ObjDataAccess.GetList_TimeService();
        }

        public string Register_TimeServiceCompany(Service obj)
        {
            return ObjDataAccess.Register_TimeServiceCompany(obj);
        }

        #endregion

        #region Certificate Digital

        public string Register_CertificateDigital(CredentialCertificate obj)
        {
            return ObjDataAccess.Register_CertificateDigital(obj);
        }

        #endregion
    }
}
