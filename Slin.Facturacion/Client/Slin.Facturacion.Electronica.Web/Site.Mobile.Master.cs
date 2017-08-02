using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Slin.Facturacion.Common;
using Slin.Facturacion.Proxies.ServicioFacturacion;
using Slin.Facturacion.Electronica.Web.ServiceFacturacion;
using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Configuration;

namespace Slin.Facturacion.Electronica.Web
{
    public partial class Site_Mobile : System.Web.UI.MasterPage
    {
        string PagLogin = ConfigurationManager.AppSettings["PagLogin"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };

            if (Session["UsuarioLogueadoFact"] == null)
            {
                //Response.Redirect("../../" + PagLogin + ".aspx");
                Response.Redirect("~/" + PagLogin + ".aspx");
            }

            Cargar();
            Page.RegisterRedirectOnSessionEndScript();
            //Default.valorsession = Session["UsuarioExisteFact"] == null ? string.Empty : (String)Session["UsuarioExisteFact"];
            Login.valorsession = Session["UsuarioExisteFact"] == null ? string.Empty : (String)Session["UsuarioExisteFact"];
        }

        #region SESSION USUARIO LOGEADO

        #endregion



        #region ENTITY

        //public bool SessionActiva = false;GetAmbienteTrabjActual

        private WCFSeguridad.Usuario ousuarioLogeado;
        public WCFSeguridad.Usuario oUsuarioLogeado
        {
            get { return ousuarioLogeado; }
            set { ousuarioLogeado = value; }
        }

        private WCFSeguridad.AmbienteTrabjActual oambtrbjActual;
        public WCFSeguridad.AmbienteTrabjActual oAmbTrbjActual
        {
            get { return oambtrbjActual; }
            set { oambtrbjActual = value; }
        }



        #endregion

        #region METHOD

        void Cargar()
        {
            ObtenerUsuarioLogeado();
            oAmbTrbjActual = new WCFSeguridad.AmbienteTrabjActual();
            oAmbTrbjActual = ServiceSeguridadController.Instance.GetAmbienteTrabjActual(oUsuarioLogeado.Empresa.RUC);

            //ObtenerUsuarioLogeado();
            //txtuser.InnerText = oUsuarioLogeado.Username + " - " + oUsuarioLogeado.Empleado.Nombres;

            if (oAmbTrbjActual.ID == Constantes.ValorCero)
            {
                txtuser.InnerText = "Bienvenido " + oUsuarioLogeado.Empleado.Nombres + " - Ambiente Trabajo: " + "Sin Ambiente";
            }
            else
            {
                txtuser.InnerText = "Bienvenido " + oUsuarioLogeado.Empleado.Nombres + " - Ambiente Trabajo: " + oAmbTrbjActual.DESCRIPCION;
            }


            logoade.Src = "Img/logo/logoade.png";
            

            Session["AmbTrabjActual"] = oAmbTrbjActual;
            //menu
            CrearMenu();
            CapturarRolesUsuarioLogeado();

            var list = ServiceConfiguracionController.Instance.Get_CertificateDigitalInformation(oUsuarioLogeado.Empresa.RUC);

            if (list.Count == Constantes.ValorUno)
            {
                var Dief = Convert.ToDateTime(list[0].ExpirationDate).Subtract(DateTime.Now);
                int cant = Dief.Days;
                if (cant < Constantes.ValorQuince + Constantes.ValorUno)
                {
                    lblExpiratoionDate_Cert_red.InnerText = "Expiración Cert. Dig.: " + Convert.ToDateTime(list[0].ExpirationDate).ToString("dd/MM/yyyy");
                    lblExpiratoionDate_Cert_red.Visible = true;
                    lblExpiratoionDate_Cert.Visible = false;
                }
                else
                {
                    lblExpiratoionDate_Cert.InnerText = "Expiración Cert. Dig.: " + Convert.ToDateTime(list[0].ExpirationDate).ToString("dd/MM/yyyy");
                    lblExpiratoionDate_Cert.Visible = true;
                    lblExpiratoionDate_Cert_red.Visible = false;
                }
            }
            else { lblExpiratoionDate_Cert.InnerText = string.Empty; }
        }

        private void ObtenerUsuarioLogeado()
        {
            oUsuarioLogeado = (WCFSeguridad.Usuario)Session["UsuarioLogueadoFact"];
        }

        #endregion

        #region REGISTER LOG LOGIN

        private WCFSeguridad.LogueoClass objlogueo;
        public WCFSeguridad.LogueoClass ObjLogueo
        {
            get { return objlogueo; }
            set { objlogueo = value; }
        }

        private string GetDataUserLogxUpdateDateDeaperture()
        {
            string Result = string.Empty;
            int IdSession = Constantes.ValorCero;

            IdSession = (int)Session["IdSession"];

            if (IdSession > Constantes.ValorCero)
            {
                try
                {
                    ObjLogueo = new WCFSeguridad.LogueoClass();
                    ObjLogueo.Empleado = new WCFSeguridad.Empleado();
                    ObjLogueo.Perfil = new WCFSeguridad.Perfil();

                    ObjLogueo.ID = IdSession;
                    ObjLogueo.Empleado.DNI = oUsuarioLogeado.Empleado.DNI;
                    ObjLogueo.Username = oUsuarioLogeado.Username;
                    ObjLogueo.FechaSalida = DateTime.Now;

                    Result = ServiceSeguridadController.Instance.UpdateRegistroLogueo(ObjLogueo);
                }
                catch (Exception ex)
                {
                    Result = string.Empty;
                }
            }
            return Result;
        }

        #endregion END REGISTER LOG LOGIN

        protected void CerrarSesion_ServerClick(object sender, EventArgs e)
        {
            //Session.Abandon();

            //Session.RemoveAll();
            //Default.SessionActiva = false;

            GetDataUserLogxUpdateDateDeaperture();

            Session.Remove("SessionListMenuUserLog");


            //Default.valorsession = string.Empty;
            Login.valorsession = string.Empty;

            Session.Remove("UsuarioExisteFact");

            Session.Remove("UsuarioLogueadoFact");//SESSION USUAARIO LOGEADO

            //HttpContext.Current.Request.Cookies.Clear();

            Session.Abandon();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));

            string path = ConfigurationManager.AppSettings["PagLogin"].ToString();

            Response.Redirect("~/" + path + ".aspx");
        }

        #region MENU

        WCFSeguridad.ListaMenu oListaMenu = new WCFSeguridad.ListaMenu();
        WCFSeguridad.ListaMenu oListaMenuPadre = new WCFSeguridad.ListaMenu();

        void IdentificarUsuario()
        {
            ObtenerUsuarioLogeado();
            if (oUsuarioLogeado.Empleado.DNI == Constantes.ValorRoot)
            { //AsignarMenuRoot(); 
            }
            else { CrearMenu(); }
        }

        void AsignarMenuRoot()
        {
            MenuConfiguracion.Visible = true;
            MenuEnvio.Visible = true;
            MenuConsultas.Visible = true;
            MenuRegistro.Visible = true;
            MenuSeguridad.Visible = true;
            MenuMantenimiento.Visible = true;
            EnvioDocumento.Visible = true;
            DocumentosEnviados.Visible = true;
            ConsultaDocumento.Visible = true;
            DocumentosAnulados.Visible = true;
            ConsultaRC.Visible = true;
            AnularDocumento.Visible = true;
            //CrearPerfil.Visible = true;
            AsignarMenuPerfil.Visible = true;
            //RegistroEmpresa.Visible = true;
            RegistroUsuario.Visible = true;
            ListadoEmpresa.Visible = true;
            ListaUsuarios.Visible = true;
            //RegistroCorreo.Visible = true;
            ListadoCorreo.Visible = true;
            ConsultaRA.Visible = true;
            ConsultaRR.Visible = true;
            //RegistroCliente.Visible = true;
            ListadoCliente.Visible = true;
            ConsultaDocAnuladoSUNAT.Visible = true;

            ConfiguracionURL.Visible = true;
            ConfiguracionImpresion.Visible = true;
            ConfiguracionEnvio.Visible = true;
            ConsultaDocumentoCRE.Visible = true;
        }



        private void GetMenuSessionUserLog()
        {
            oListaMenu = new WCFSeguridad.ListaMenu();
            oListaMenu = (WCFSeguridad.ListaMenu)Session["SessionListMenuUserLog"];
        }

        void CrearMenu()
        {
            //oListaMenu = ServiceSeguridadController.GetListarMenu(oUsuarioLogeado);


            GetMenuSessionUserLog();

            //MENU PRINCIPAL


            //REEMPLAZAR TODO ESTO POR UN SWITCH

            //MENU CONFIGURACION
            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(MenuConfiguracion.ID))
                {
                    MenuConfiguracion.Visible = true; break;
                    //MenuConfiguracion.InnerText = oListaMenu[i].NombreMenu;
                }
            }

            //MENU ENVIO
            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(MenuEnvio.ID))
                { MenuEnvio.Visible = true; break; }
            }


            //MENU CONSULTAS
            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(MenuConsultas.ID))
                { MenuConsultas.Visible = true; break; }
            }

            //MENU REGISTRO
            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(MenuRegistro.ID))
                { MenuRegistro.Visible = true; break; }
            }

            //MENU SEGURIDAD
            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(MenuSeguridad.ID))
                { MenuSeguridad.Visible = true; break; }
            }

            //MENU MANTENIMIENTO
            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(MenuMantenimiento.ID))
                { MenuMantenimiento.Visible = true; break; }
            }

            
            //MENU AJUSTE EMPRESA
            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(MenuAjusteEmpresa.ID))
                { MenuAjusteEmpresa.Visible = true; break; }
            }


            //SUB MENUS ALL

            //SUB MENU CONFIGURACION
            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(ConfiguracionURL.ID))
                {
                    ConfiguracionURL.Visible = true;
                    SpanConfiguracionURL.InnerHtml = oListaMenu[i].NombreMenu;
                    break;
                }
            }

            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(ConfiguracionImpresion.ID))
                {
                    ConfiguracionImpresion.Visible = true;
                    SpanConfiguracionImpresion.InnerHtml = oListaMenu[i].NombreMenu;
                    break;
                }
            }

            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(ConfiguracionEnvio.ID))
                {
                    ConfiguracionEnvio.Visible = true;
                    SpanConfiguracionEnvio.InnerHtml = oListaMenu[i].NombreMenu;
                    break;
                }
            }

            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(ConfiguracionAmbienteTrabj.ID))
                {
                    ConfiguracionAmbienteTrabj.Visible = true;
                    SpanConfiguracionAmbienteTrabajo.InnerHtml = oListaMenu[i].NombreMenu;
                    break;
                }
            }


            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(ConfiguracionTimeService.ID))
                {
                    ConfiguracionTimeService.Visible = true;
                    SpanConfiguracionTimeService.InnerHtml = oListaMenu[i].NombreMenu;
                    break;
                }
            }


            //for (int i = 0; i <= oListaMenu.Count - 1; i++)
            //{
            //    if (oListaMenu[i].CodigoMenu.Equals(ConfiguracionTipoCambio.ID))
            //    {
            //        ConfiguracionTipoCambio.Visible = true;
            //        SpanConfiguracionTipoCambio.InnerHtml = oListaMenu[i].NombreMenu;
            //        break;
            //    }
            //}








            //-------------------------------------INICIO ALTER

            //SUB MENU - ENVIO
            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(EnvioDocumento.ID))
                {
                    EnvioDocumento.Visible = true;
                    SpanEnvioDocumento.InnerHtml = oListaMenu[i].NombreMenu;
                    break;
                }
            }

            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(DocumentosEnviados.ID))
                {
                    DocumentosEnviados.Visible = true;
                    SpanDocumentoEnviado.InnerHtml = oListaMenu[i].NombreMenu;
                    break;
                }
            }


            //SUB MENU CONSULTAS
            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(ConsultaDocumento.ID))
                {
                    ConsultaDocumento.Visible = true;
                    SpanConsultaDocumento.InnerHtml = oListaMenu[i].NombreMenu;
                    break;
                }
            }

            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(DocumentosAnulados.ID))
                {
                    DocumentosAnulados.Visible = true;
                    SpanConsultaDocumentoAnulado.InnerHtml = oListaMenu[i].NombreMenu;
                    break;
                }
            }

            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(ConsultaDocumentoCRE.ID))
                {
                    ConsultaDocumentoCRE.Visible = true;
                    SpanConsultaDocumentoCRE.InnerHtml = oListaMenu[i].NombreMenu;
                    break;
                }
            }

            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(ConsultaRC.ID))
                {
                    ConsultaRC.Visible = true;
                    SpanConsultaResumenRC.InnerHtml = oListaMenu[i].NombreMenu;
                    break;
                }
            }

            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(ConsultaRA.ID))
                {
                    ConsultaRA.Visible = true;
                    SpanConsultaResumenRA.InnerHtml = oListaMenu[i].NombreMenu;
                    break;
                }
            }

            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(ConsultaRR.ID))
                {
                    ConsultaRR.Visible = true;
                    SpanConsultaResumenRR.InnerHtml = oListaMenu[i].NombreMenu;
                    break;
                }
            }

            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(ConsultaDocAnuladoSUNAT.ID))
                {
                    ConsultaDocAnuladoSUNAT.Visible = true;
                    SpanConsultaDocAnuladoADE.InnerHtml = oListaMenu[i].NombreMenu;
                    break;
                }
            }

            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(ConsultaDocumentoRecibido.ID))
                {
                    ConsultaDocumentoRecibido.Visible = true;
                    SpanConsultaDocumentoReceived.InnerHtml = oListaMenu[i].NombreMenu;
                    break;
                }
            }


            //SUB MENU REGISTRO 
            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(AnularDocumento.ID))
                {
                    AnularDocumento.Visible = true;
                    SpanAnularDocumento.InnerHtml = oListaMenu[i].NombreMenu;
                    break;
                }
            }

            //SUB MENU SEGURIDAD
            //for (int i = 0; i <= oListaMenu.Count - 1; i++)
            //{
            //    if (oListaMenu[i].CodigoMenu.Equals(CrearPerfil.ID))
            //    {
            //        CrearPerfil.Visible = true;
            //        break;
            //    }
            //}

            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(AsignarMenuPerfil.ID))
                {
                    AsignarMenuPerfil.Visible = true;
                    SpanAsignarPerfiles.InnerHtml = oListaMenu[i].NombreMenu;
                    break;
                }
            }

            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(ListLogSeguridad.ID))
                {
                    ListLogSeguridad.Visible = true;
                    SpanListLogSeguridad.InnerHtml = oListaMenu[i].NombreMenu;
                    break;
                }
            }


            //for (int i = 0; i <= oListaMenu.Count - 1; i++)
            //{
            //    if (oListaMenu[i].CodigoMenu.Equals(CambiarEmpresa.ID))
            //    {
            //        CambiarEmpresa.Visible = true;
            //        SpanChangeCompany.InnerHtml = oListaMenu[i].NombreMenu;
            //        break;
            //    }
            //}



            //SUB MENU MANTENIMIENTO

            //registroempresa
            //for (int i = 0; i <= oListaMenu.Count - 1; i++)
            //{
            //    if (oListaMenu[i].CodigoMenu.Equals(RegistroEmpresa.ID))
            //    {
            //        RegistroEmpresa.Visible = true;
            //        break;
            //    }
            //}

            //registrousuario
            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(RegistroUsuario.ID))
                {
                    RegistroUsuario.Visible = true;
                    SpanRegistroUsuario.InnerHtml = oListaMenu[i].NombreMenu;
                    break;
                }
            }



            //SUN MENU MANTENIMIENTO
            //listadoempresa
            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(ListadoEmpresa.ID))
                {
                    ListadoEmpresa.Visible = true;
                    SpanListadoEmpresa.InnerHtml = oListaMenu[i].NombreMenu;
                    break;
                }
            }

            //listadousuario
            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(ListaUsuarios.ID))
                {
                    ListaUsuarios.Visible = true;
                    SpanListadoUsuario.InnerHtml = oListaMenu[i].NombreMenu;
                    break;
                }
            }

            //registrocorreo
            //for (int i = 0; i <= oListaMenu.Count - 1; i++)
            //{
            //    if (oListaMenu[i].CodigoMenu.Equals(RegistroCorreo.ID))
            //    {
            //        RegistroCorreo.Visible = true;
            //        break;
            //    }
            //}

            //listadocorreo
            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(ListadoCorreo.ID))
                {
                    ListadoCorreo.Visible = true;
                    SpanListadoCorreo.InnerHtml = oListaMenu[i].NombreMenu;
                    break;
                }
            }

            //registrocliente
            //for (int i = 0; i <= oListaMenu.Count - 1; i++)
            //{
            //    if (oListaMenu[i].CodigoMenu.Equals(RegistroCliente.ID))
            //    {
            //        RegistroCliente.Visible = true;
            //        break;
            //    }
            //}

            //listadocliente
            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(ListadoCliente.ID))
                {
                    ListadoCliente.Visible = true;
                    SpanListadoCliente.InnerHtml = oListaMenu[i].NombreMenu;
                    break;
                }
            }




            //SUN MENU AJUSTE
            //Cambio Empresa
            for (int i = 0; i <= oListaMenu.Count - 1; i++)
            {
                if (oListaMenu[i].CodigoMenu.Equals(ChangeCompany.ID))
                {
                    ChangeCompany.Visible = true;
                    SpanChangeCompany.InnerHtml = oListaMenu[i].NombreMenu;
                    break;
                }
            }

        }


        WCFSeguridad.ListaRol oListaRol = new WCFSeguridad.ListaRol();
        void CapturarRolesUsuarioLogeado()
        {
            oListaRol = ServiceSeguridadController.Instance.GetListaRolesUsuario(oUsuarioLogeado);
            Session["RolesUsuarioLogeado"] = oListaRol;
        }

        #endregion
    }
}