using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Slin.Facturacion.Common;
using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;
using Slin.Facturacion.Electronica.Web.ServiceFacturacion;
using WCFMantenimiento = Slin.Facturacion.Proxies.ServicioMantenimiento;

using System.DirectoryServices;
using System.Configuration;
using System.Net;
using System.Xml;
using System.IO;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Slin.Facturacion.Electronica.Web
{
    public partial class AccessCloud : System.Web.UI.Page
    {
        static string ADSERVER = ConfigurationManager.AppSettings["ADSERVER"].ToString();

        public static bool SessionActiva = false;
        public static bool ExisteSession = false;
        public static string valorsession;

        protected void Page_Load(object sender, EventArgs e)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };

            Response.AppendHeader("Cache-Control", "no-store");
            HttpContext.Current.Request.Cookies.Clear();
            valorsession = Session.Count == 0 ? string.Empty : (String)Session["UsuarioExisteFact"];

            //valorsession = Session["UsuarioExisteFact"] == null ? string.Empty : (String)Session["UsuarioExisteFact"];

            if (valorsession != string.Empty)
            {
                //Response.Redirect("Views/Home/Inicio");
            }
            Session["contadorFact"] = contador;
            contador = (int)Session["contadorFact"];
            if (Page.IsPostBack == false)
            {
                GetKeyFromBD();
                Cargar();
            }
        }

        #region ENTITY

        private WCFSeguridad.Usuario ousuario;
        public WCFSeguridad.Usuario oUsuario
        {
            get { return ousuario; }
            set { ousuario = value; }
        }

        private WCFMantenimiento.ListaEmpresa olistaEmpresa;
        public WCFMantenimiento.ListaEmpresa oListaEmpresa
        {
            get { return olistaEmpresa; }
            set { olistaEmpresa = value; }
        }

        private WCFSeguridad.ListaMenu olistamenu;
        public WCFSeguridad.ListaMenu oListaMenu
        {
            get { return olistamenu; }
            set { olistamenu = value; }
        }

        #endregion

        #region METHOD

        void Cargar()
        {
            txtruccomp.Focus();
            CargarLista();
            logoade.Src = "Img/logo/logoade.png";
        }


        int id_comp = Constantes.ValorCero;
        void CapturarDatosUsuario()
        {
            try
            {
                var comp = ServiceSeguridadController.Instance.GetId(txtruccomp.Value);

                if (comp != null && comp.IdEmpresa > 0)
                {
                    oUsuario = new WCFSeguridad.Usuario();
                    oUsuario.Empresa = new WCFSeguridad.Empresa();
                    oUsuario.Username = txtusuario.Value;
                    oUsuario.Empresa.IdEmpresa = comp.IdEmpresa;
                    oUsuario.Password = new Helper.Encrypt().HashPassword(txtpassword.Value);//PWC

                    id_comp = comp.IdEmpresa;

                    var xmlDoc = new XmlDocument();
                    xmlDoc.Load(Server.MapPath("Content\\EntityRegex.xml"));
                    XmlNodeList node;
                    node = xmlDoc.GetElementsByTagName("EntityRegex");
                    for (int i = 0; i <= node.Count - 1; i++)
                    {
                        if (node.Item(i).ChildNodes[0].InnerText == oUsuario.Empresa.IdEmpresa.ToString())
                        {
                            oUsuario.Empresa.TipoLogin = node.Item(i).ChildNodes[1].InnerText;
                            break;
                        }
                    }
                }
                else
                {
                    lblmensaje.InnerText = "RUC Invalido!";
                    lblmensaje.Visible = true;
                    return;
                }
            }
            catch (Exception ex) { }
        }



        void CargarLista()
        {
            oListaEmpresa = new WCFMantenimiento.ListaEmpresa();
            oListaEmpresa = ServiceMantenimientoController.Instance.GetListaEmpresa();

            //cboempresa.DataSource = oListaEmpresa;
            //cboempresa.DataValueField = "IdEmpresa";
            //cboempresa.DataTextField = "RazonSocial";
            //cboempresa.DataBind();

            Session["ListCboEmpresa"] = oListaEmpresa;//session para validar el tipo de logueo

            Singleton_wb.Instance.Create_EntityRegex(Server.MapPath("Content"), oListaEmpresa);
        }

        void LimpiarCampos()
        {
            txtpassword.Value = string.Empty;
            txtusuario.Value = string.Empty;
        }

        public static int contador = Constantes.ValorCero;
        public string usernameacceso = string.Empty;
        public int Acceso = Constantes.ValorCero;
        public string ultimoUsername = string.Empty;
        public static string usernameUno = string.Empty;

        void ValidarDatos()
        {
            try
            {
                usernameUno = (string)Session["UsernameUnoFact"];
                contador = (int)Session["contadorFact"];

                if (oUsuario.Empresa.TipoLogin == Constantes.LoginWithLDAP)
                {
                    #region ACCESS CLOUD WITH ACTIVE DIRECTORY
                    var res = ValidarActiveDirectory();
                    if (res == false)
                    {
                        #region USER NOT EXISTS IN ACTIVEDIRECTORY

                        var IdResult = GetDataUserLogLogueo();
                        string user = string.Empty;
                        if (usernameUno == txtusuario.Value)
                        {
                            contador++;
                            Session["UsernameUnoFact"] = txtusuario.Value;
                            Session["contadorFact"] = contador;
                        }
                        else
                        {
                            contador = Constantes.ValorCero;
                            Session["contadorFact"] = contador;
                            Session.Remove("UsernameUnoFact");
                        }
                        Session["UsernameUnoFact"] = txtusuario.Value;
                        lblmensaje.InnerText = "Datos Incorrectos";
                        lblmensaje.Visible = true;
                        LimpiarCampos();
                        if (contador == Constantes.ValorDos)
                        {
                            lblmensaje.InnerText = ServiceSeguridadController.Instance.BloquearUsuario(oUsuario);
                        }
                        #endregion
                    }
                    else
                    {
                        #region ACCESO

                        Acceso = ServiceSeguridadController.Instance.ValidarAcceso(oUsuario);
                        if (Acceso > 0)
                        {
                            ObtenerUsuarioLogeado();
                            var listentity = new Slin.Facturacion.BusinessSecurity.Entity.EntityClass().getsListEntity();

                            if (listentity.Contains(oUsuarioLogeado.Empresa.RUC))
                            {
                                lblmensaje.InnerText = String.Empty;
                                Session["UsuarioExisteFact"] = "HaySession";
                                Session.Remove("UsernameAccesoFact");

                                var IdSession = GetDataUserLogxInsert(id_comp);
                                Session["IdSession"] = IdSession;

                                Response.Redirect("Views/Banner/SecurityScreen.aspx");
                            }
                            else
                            {
                                var IdLog = GetDataUserLogxInsert(id_comp);
                                Session.Remove("UsuarioLogueadoFact");
                                Session.RemoveAll();
                                Response.Redirect("~/ErrorPage/AccessDenied.aspx");
                            }
                        }
                        else
                        {
                            var IdResult = GetDataUserLogLogueo();
                            string user = string.Empty;
                            if (usernameUno == txtusuario.Value)
                            {
                                contador++;
                                Session["UsernameUnoFact"] = txtusuario.Value;
                                Session["contadorFact"] = contador;
                            }
                            else
                            {
                                contador = Constantes.ValorCero;
                                Session["contadorFact"] = contador;
                                Session.Remove("UsernameUnoFact");
                            }
                            Session["UsernameUnoFact"] = txtusuario.Value;
                            lblmensaje.InnerText = "Datos Incorrectos";
                            lblmensaje.Visible = true;
                            LimpiarCampos();
                            if (contador == Constantes.ValorDos)
                            {
                                lblmensaje.InnerText = ServiceSeguridadController.Instance.BloquearUsuario(oUsuario);
                            }
                        }


                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region ACCESS CLOUD WITH BD

                    Acceso = ServiceSeguridadController.Instance.ValidarAcceso(oUsuario);


                    if (Acceso > 0)
                    {
                        ObtenerUsuarioLogeado();

                        if (oUsuarioLogeado.Estado.Descripcion == Constantes.StatusLocked)
                        {
                            Session.Remove("UsuarioLogueadoFact");
                            Session.RemoveAll();
                            lblmensaje.InnerText = Constantes.msjUsuarioBloqueado;
                            lblmensaje.Visible = true;
                            LimpiarCampos();
                        }
                        else
                        {
                            var listentity = new Slin.Facturacion.BusinessSecurity.Entity.EntityClass().getsListEntity();
                            listentity.Find(x => x.Contains(oUsuarioLogeado.Empresa.RUC));
                            var valor = listentity.Find(x => x.Contains(oUsuarioLogeado.Empresa.RUC));

                            if (listentity.Contains(oUsuarioLogeado.Empresa.RUC))
                            {
                                lblmensaje.InnerText = String.Empty;
                                Session["UsuarioExisteFact"] = "HaySession";
                                Session.Remove("UsernameAccesoFact");

                                var IdSession = GetDataUserLogxInsert(id_comp);
                                Session["IdSession"] = IdSession;
                                Response.Redirect("Views/Home/Inicio.aspx");
                            }
                            else
                            {
                                var IdLog = GetDataUserLogxInsert(id_comp);
                                Session.Remove("UsuarioLogueadoFact");
                                Session.RemoveAll();
                                Response.Redirect("~/ErrorPage/AccessDenied.aspx");
                            }
                        }
                    }
                    else
                    {
                        var IdResult = GetDataUserLogLogueo();
                        string user = string.Empty;
                        if (usernameUno == txtusuario.Value)
                        {
                            contador++;
                            Session["UsernameUnoFact"] = txtusuario.Value;
                            Session["contadorFact"] = contador;
                        }
                        else
                        {
                            contador = Constantes.ValorCero;
                            Session["contadorFact"] = contador;
                            Session.Remove("UsernameUnoFact");
                        }
                        Session["UsernameUnoFact"] = txtusuario.Value;
                        lblmensaje.InnerText = "Datos Incorrectos";
                        lblmensaje.Visible = true;
                        LimpiarCampos();
                        if (contador == Constantes.ValorDos)
                        {
                            lblmensaje.InnerText = ServiceSeguridadController.Instance.BloquearUsuario(oUsuario);
                        }
                    }


                    #endregion
                }
            }
            catch (Exception ex) { }
        }

        private WCFSeguridad.Usuario ousuarioLogeado;
        public WCFSeguridad.Usuario oUsuarioLogeado
        {
            get { return ousuarioLogeado; }
            set { ousuarioLogeado = value; }
        }

        void ObtenerUsuarioLogeado()
        {
            oUsuarioLogeado = ServiceSeguridadController.Instance.GetUsuarioLogeado(oUsuario);
            Session["UsuarioLogueadoFact"] = oUsuarioLogeado;

            oListaMenu = new WCFSeguridad.ListaMenu();
            oListaMenu = ServiceSeguridadController.Instance.GetListarMenu(oUsuarioLogeado);
            Session["SessionListMenuUserLog"] = oListaMenu;
        }
        #endregion

        #region BLOQUEAR POR INTENTO FALLIDOS
        public int intentos = Constantes.ValorCero;
        void BloquearUsuario()
        {
            Session["nroIntentoLogeo"] = intentos;
        }

        #endregion





        #region KY
        private void GetKeyFromBD()
        {
            var listentity = new Slin.Facturacion.BusinessSecurity.Entity.EntityClass().getsListEntity();
            int cont = Constantes.ValorCero;
            int emp = Constantes.ValorCero;

            foreach (var ent in listentity)
            {
                var result = ServiceSeguridadController.Instance.GetEntityEmpresa(ent);
                if (result.Count == Constantes.ValorCero) { cont++; }
                else { emp++; }
            }

            if (emp == Constantes.ValorCero)
            {
                Session["SessionListCompanyNull"] = "No se a podido obtener informacion de la Base de datos. revise el log de la aplicación.";
                Response.Redirect("~/ErrorPage/AccessDenied.aspx");
            }
        }

        #endregion

        #region REGISTER LOG ACCESS CLOUD

        string sIP = string.Empty;
        string HostName = string.Empty;

        private WCFSeguridad.LogueoClass objlogueo;
        public WCFSeguridad.LogueoClass ObjLogueo
        {
            get { return objlogueo; }
            set { objlogueo = value; }
        }

        private int GetDataUserLogxInsert(int id)
        {
            int IdSession = Constantes.ValorCero;
            try
            {
                GetHostNameClient();

                ObjLogueo = new WCFSeguridad.LogueoClass();
                ObjLogueo.Empleado = new WCFSeguridad.Empleado();
                ObjLogueo.Perfil = new WCFSeguridad.Perfil();
                ObjLogueo.TipoLog = new WCFSeguridad.TipoLog();
                ObjLogueo.Empresa = new WCFSeguridad.Empresa();

                ObjLogueo.Fecha = DateTime.Now;
                ObjLogueo.Empleado.DNI = oUsuarioLogeado.Empleado.DNI;
                ObjLogueo.Username = oUsuarioLogeado.Username;
                ObjLogueo.Perfil.IdPerfil = oUsuarioLogeado.Perfil.IdPerfil;
                ObjLogueo.FechaIngreso = DateTime.Now;
                ObjLogueo.HostName = HostName.Length > Constantes.ValorCero ? HostName : string.Empty;
                ObjLogueo.sIP = sIP.Length > Constantes.ValorCero ? sIP : string.Empty;
                ObjLogueo.TipoLog.ID = Constantes.ValorUno;
                ObjLogueo.Empresa.IdEmpresa = id;

                IdSession = ServiceSeguridadController.Instance.InsertRegistroLogueo(ObjLogueo);
            }
            catch (Exception ex)
            {
                IdSession = Constantes.ValorCero;
            }
            return IdSession;
        }


        private int GetDataUserLogLogueo()
        {
            int IdResult = Constantes.ValorCero;
            try
            {
                GetHostNameClient();
                ObjLogueo = new WCFSeguridad.LogueoClass();
                ObjLogueo.Empleado = new WCFSeguridad.Empleado();
                ObjLogueo.Perfil = new WCFSeguridad.Perfil();
                ObjLogueo.TipoLog = new WCFSeguridad.TipoLog();
                ObjLogueo.Empresa = new WCFSeguridad.Empresa();

                var ObjetoUsuario = ServiceSeguridadController.Instance.GetDataFromUserLogueo(txtusuario.Value);

                if (ObjetoUsuario.Empleado != null)
                {
                    ObjLogueo.Empleado.DNI = ObjetoUsuario.Empleado.DNI;
                    ObjLogueo.Username = txtusuario.Value;
                    ObjLogueo.Perfil.IdPerfil = ObjetoUsuario.Perfil.IdPerfil;
                }
                else
                {
                    ObjLogueo.Empleado.DNI = string.Empty;
                    ObjLogueo.Username = txtusuario.Value;
                    ObjLogueo.Perfil.IdPerfil = Constantes.ValorCero;
                }
                ObjLogueo.Fecha = DateTime.Now;
                ObjLogueo.FechaIngreso = DateTime.Now;
                ObjLogueo.HostName = HostName.Length > Constantes.ValorCero ? HostName : string.Empty;
                ObjLogueo.sIP = sIP.Length > Constantes.ValorCero ? sIP : string.Empty;
                ObjLogueo.TipoLog.ID = Constantes.ValorDos;
                ObjLogueo.Empresa.IdEmpresa = id_comp;

                IdResult = ServiceSeguridadController.Instance.InsertRegistroLogueo(ObjLogueo);
            }
            catch (Exception ex)
            {
                IdResult = Constantes.ValorCero;
            }
            return IdResult;
        }


        #endregion END REGISTER LOG ACCESS CLOUD

        #region GET DATA HOST NAME

        private void GetHostNameClient()
        {
            sIP = Request.UserHostName;
            HostName = DetermineCompName(sIP);
        }
        public static string DetermineCompName(string IP)
        {
            IPAddress myIP = IPAddress.Parse(IP);
            IPHostEntry GetIPHost = Dns.GetHostEntry(myIP);
            List<string> compName = GetIPHost.HostName.ToString().Split('.').ToList();
            return compName.First();
        }
        #endregion


        protected void btnIngreso_Click(object sender, EventArgs e)
        {
            CapturarDatosUsuario();
            ValidarDatos();
        }

        #region ACCESS CLOUD WITH ACTIVE DIRECTORY
        public bool ValidarActiveDirectory()
        {
            bool result = false;
            string server = ADSERVER;

            using (DirectoryEntry _entry = new DirectoryEntry())
            {
                _entry.Username = txtusuario.Value;
                _entry.Password = txtpassword.Value;
                _entry.Path = "LDAP://" + server + "/DC=test,DC=com";
                DirectorySearcher _searcher = new DirectorySearcher(_entry);
                _searcher.Filter = "(objectclass=user)";
                try
                {
                    SearchResult _sr = _searcher.FindOne();
                    string _name = _sr.Properties["displayname"][0].ToString();
                    result = true;
                }
                catch (Exception ex)
                {
                    #region

                    switch (ex.Message)
                    {
                        case Constantes.MsjLoginWithActiveDirectoryOK:
                        case Constantes.MsjLoginWithActiveDirectoryOK_ENG:
                        case Constantes.MsjLoginWithActiveDirectoryOK_ENG_sl:
                            {
                                result = true;
                                break;
                            }

                    }

                    #endregion
                }
            }
            return result;
        }
        #endregion END ACCESS CLOUD WITH ACTIVE DIRECTOTY
    }
}