using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Slin.Facturacion.Common;
using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;
using Slin.Facturacion.Electronica.Web.ServiceFacturacion;
using Slin.Facturacion.Electronica.Web.Views.Util;

namespace Slin.Facturacion.Electronica.Web.Views.Seguridad
{
    public partial class AsignarPerfil : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                Cargar();
            }
        }

        #region ENTITY

        ServiceWebFactController objController = new ServiceWebFactController();

        WCFSeguridad.Perfil oPerfil = new WCFSeguridad.Perfil();

        WCFSeguridad.ListaMenu oListaMenu = new WCFSeguridad.ListaMenu();
        WCFSeguridad.ListaMenu oListaMenuPerfil = new WCFSeguridad.ListaMenu();

        WCFSeguridad.Menu oMenu = new WCFSeguridad.Menu();


        private WCFSeguridad.Usuario ousuarioLogeado;
        public WCFSeguridad.Usuario oUsuarioLogeado
        {
            get { return ousuarioLogeado; }
            set { ousuarioLogeado = value; }
        }
        #endregion

        #region METHOD

        private void ObtenerUsuarioLogeado()
        {
            try
            {
                oUsuarioLogeado = objController.GetUserLogueado_Fact();
            }
            catch (Exception ex) { }
        }

        void Cargar()
        {
            //string Url = HttpContext.Current.Request.Url.AbsoluteUri;
            //int result = objController.Process_UrlPage(Url);
            //if (result == Constantes.ValorCero)
            //{ Response.Redirect("~/Views/Home/Inicio.aspx"); }

            try
            {
                ObtenerUsuarioLogeado();
                lblempresa.InnerText = oUsuarioLogeado.Empresa.RazonSocial.ToUpper();

                logoEmpresa.Src = "../../Img/home/" + oUsuarioLogeado.Empresa.RUC + ".png";

                oPerfil = (WCFSeguridad.Perfil)Session["PerfilSeleccionado"];

                oListaMenu = ServiceSeguridadController.Instance.GetListaMenu();
                Session["AllListaMenu"] = oListaMenu;

                txtnombreperfil.Value = oPerfil.NombrePerfil;
                //txtcodigo.Value = oPerfil.Codigo;

                oListaMenuPerfil = ServiceSeguridadController.Instance.GetListarMenuPerfil(oPerfil);
                Session["oListaMenuPerfil"] = oListaMenuPerfil;//

                HabilitarCheck();


                CreateeTreeView(oListaMenu, 0, null);


                HabilitarCheckBox();//treeview1

                CapturarUserLogRoles();
            }
            catch (Exception ex)
            {

            }
            
        }




        WCFSeguridad.ListaRol oListaRolesUserLog = new WCFSeguridad.ListaRol();
        void CapturarUserLogRoles()
        {
            try
            {
                oListaRolesUserLog = objController.GetList_RolUserLog();

                btnGuardar.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnGuardar.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);

            }
            catch (Exception ex)
            {

            }
            
        }


        void HabilitarCheck()
        {
            try
            {
                for (int i = 0; i <= oListaMenuPerfil.Count - 1; i++)
                {
                    if (oListaMenuPerfil[i].CodigoMenu.Contains(MenuConfiguracion.ID))
                    {
                        MenuConfiguracion.Checked = true;
                        break;
                    }
                }

                for (int i = 0; i <= oListaMenuPerfil.Count - 1; i++)
                {
                    if (oListaMenuPerfil[i].CodigoMenu == MenuEnvio.ID)
                    {
                        MenuEnvio.Checked = true;
                        break;
                    }
                }

                for (int i = 0; i <= oListaMenuPerfil.Count - 1; i++)
                {
                    if (oListaMenuPerfil[i].CodigoMenu == MenuConsultas.ID)
                    {
                        MenuConsultas.Checked = true;
                        break;
                    }
                }

                for (int i = 0; i <= oListaMenuPerfil.Count - 1; i++)
                {
                    if (oListaMenuPerfil[i].CodigoMenu == MenuRegistro.ID)
                    {
                        MenuRegistro.Checked = true;
                        break;
                    }
                }

                for (int i = 0; i <= oListaMenuPerfil.Count - 1; i++)
                {
                    if (oListaMenuPerfil[i].CodigoMenu == MenuSeguridad.ID)
                    {
                        MenuSeguridad.Checked = true;
                        break;
                    }
                }

                for (int i = 0; i <= oListaMenuPerfil.Count - 1; i++)
                {
                    if (oListaMenuPerfil[i].CodigoMenu == MenuMantenimiento.ID)
                    {
                        MenuMantenimiento.Checked = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            
        }


        #endregion

        protected void MenuConfiguracion_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                oListaMenu = (WCFSeguridad.ListaMenu)Session["AllListaMenu"];
                oPerfil = (WCFSeguridad.Perfil)Session["PerfilSeleccionado"];

                string msj = string.Empty;

                var perfil = new WCFSeguridad.Perfil();
                oMenu = new WCFSeguridad.Menu();

                for (int i = 0; i <= oListaMenu.Count - 1; i++)
                {
                    if (oListaMenu[i].CodigoMenu == MenuConfiguracion.ID)
                    {
                        //perfil.Perfil.IdPerfil = oListaPerfil[i].IdPerfil;
                        perfil.IdMenu = oListaMenu[i].IdMenu;
                        perfil.IdPerfil = oPerfil.IdPerfil;
                        break;
                    }
                }

                if (MenuConfiguracion.Checked == true)
                {
                    msj = ServiceSeguridadController.Instance.InsertarMenuPerfil(perfil);
                    Response.Write("<script language=javascript>alert('" + msj + "');</script>");
                }
                else
                {
                    msj = ServiceSeguridadController.Instance.DeleteMenuPerfil(perfil);
                    Response.Write("<script language=javascript>alert('" + msj + "');</script>");
                }
            }
            catch (Exception ex)
            {

            }
            
        }

        protected void MenuEnvio_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                oListaMenu = (WCFSeguridad.ListaMenu)Session["AllListaMenu"];
                oPerfil = (WCFSeguridad.Perfil)Session["PerfilSeleccionado"];

                string msj = string.Empty;

                var perfil = new WCFSeguridad.Perfil();
                oMenu = new WCFSeguridad.Menu();

                for (int i = 0; i <= oListaMenu.Count - 1; i++)
                {
                    if (oListaMenu[i].CodigoMenu == MenuEnvio.ID)
                    {
                        perfil.IdMenu = oListaMenu[i].IdMenu;
                        perfil.IdPerfil = oPerfil.IdPerfil;
                        break;
                    }
                }

                if (MenuEnvio.Checked == true)
                {
                    msj = ServiceSeguridadController.Instance.InsertarMenuPerfil(perfil);
                    Response.Write("<script language=javascript>alert('" + msj + "');</script>");
                }
                else
                {
                    msj = ServiceSeguridadController.Instance.DeleteMenuPerfil(perfil);
                    Response.Write("<script language=javascript>alert('" + msj + "');</script>");
                }
            }
            catch (Exception ex)
            {

            }
            
        }

        protected void MenuConsultas_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                oListaMenu = (WCFSeguridad.ListaMenu)Session["AllListaMenu"];
                oPerfil = (WCFSeguridad.Perfil)Session["PerfilSeleccionado"];

                string msj = string.Empty;

                var perfil = new WCFSeguridad.Perfil();
                oMenu = new WCFSeguridad.Menu();

                for (int i = 0; i <= oListaMenu.Count - 1; i++)
                {
                    if (oListaMenu[i].CodigoMenu == MenuConsultas.ID)
                    {
                        perfil.IdMenu = oListaMenu[i].IdMenu;
                        perfil.IdPerfil = oPerfil.IdPerfil;
                        break;
                    }
                }

                if (MenuConsultas.Checked == true)
                {
                    msj = ServiceSeguridadController.Instance.InsertarMenuPerfil(perfil);
                    Response.Write("<script language=javascript>alert('" + msj + "');</script>");
                }
                else
                {
                    msj = ServiceSeguridadController.Instance.DeleteMenuPerfil(perfil);
                    Response.Write("<script language=javascript>alert('" + msj + "');</script>");
                }
            }
            catch (Exception ex)
            {

            }
            
        }

        protected void MenuRegistro_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                oListaMenu = (WCFSeguridad.ListaMenu)Session["AllListaMenu"];
                oPerfil = (WCFSeguridad.Perfil)Session["PerfilSeleccionado"];

                string msj = string.Empty;

                var perfil = new WCFSeguridad.Perfil();
                oMenu = new WCFSeguridad.Menu();

                for (int i = 0; i <= oListaMenu.Count - 1; i++)
                {
                    if (oListaMenu[i].CodigoMenu == MenuRegistro.ID)
                    {
                        perfil.IdMenu = oListaMenu[i].IdMenu;
                        perfil.IdPerfil = oPerfil.IdPerfil;
                        break;
                    }
                }

                if (MenuRegistro.Checked == true)
                {
                    msj = ServiceSeguridadController.Instance.InsertarMenuPerfil(perfil);
                    Response.Write("<script language=javascript>alert('" + msj + "');</script>");
                }
                else
                {
                    msj = ServiceSeguridadController.Instance.DeleteMenuPerfil(perfil);
                    Response.Write("<script language=javascript>alert('" + msj + "');</script>");
                }
            }
            catch (Exception ex)
            {

            }
            
        }

        protected void MenuSeguridad_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                oListaMenu = (WCFSeguridad.ListaMenu)Session["AllListaMenu"];
                oPerfil = (WCFSeguridad.Perfil)Session["PerfilSeleccionado"];

                string msj = string.Empty;

                var perfil = new WCFSeguridad.Perfil();
                oMenu = new WCFSeguridad.Menu();

                for (int i = 0; i <= oListaMenu.Count - 1; i++)
                {
                    if (oListaMenu[i].CodigoMenu == MenuSeguridad.ID)
                    {
                        perfil.IdMenu = oListaMenu[i].IdMenu;
                        perfil.IdPerfil = oPerfil.IdPerfil;
                        break;
                    }
                }

                if (MenuSeguridad.Checked == true)
                {
                    msj = ServiceSeguridadController.Instance.InsertarMenuPerfil(perfil);
                    Response.Write("<script language=javascript>alert('" + msj + "');</script>");
                }
                else
                {
                    msj = ServiceSeguridadController.Instance.DeleteMenuPerfil(perfil);
                    Response.Write("<script language=javascript>alert('" + msj + "');</script>");
                }
            }
            catch (Exception ex) { }   
        }

        protected void MenuMantenimiento_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                oListaMenu = (WCFSeguridad.ListaMenu)Session["AllListaMenu"];
                oPerfil = (WCFSeguridad.Perfil)Session["PerfilSeleccionado"];

                string msj = string.Empty;

                var perfil = new WCFSeguridad.Perfil();
                oMenu = new WCFSeguridad.Menu();

                for (int i = 0; i <= oListaMenu.Count - 1; i++)
                {
                    if (oListaMenu[i].CodigoMenu == MenuMantenimiento.ID)
                    {
                        perfil.IdMenu = oListaMenu[i].IdMenu;
                        perfil.IdPerfil = oPerfil.IdPerfil;
                        break;
                    }
                }

                if (MenuMantenimiento.Checked == true)
                {
                    msj = ServiceSeguridadController.Instance.InsertarMenuPerfil(perfil);
                    Response.Write("<script language=javascript>alert('" + msj + "');</script>");
                }
                else
                {
                    msj = ServiceSeguridadController.Instance.DeleteMenuPerfil(perfil);
                    Response.Write("<script language=javascript>alert('" + msj + "');</script>");
                }
            }
            catch (Exception ex)
            {

            }
            
        }



        #region CREAR LISTA CHECKBOXTREEVIEW CON EL EL MENU

        private void CreateeTreeView(WCFSeguridad.ListaMenu source, int PadreMEnu, TreeNode parentNode)
        {
            try
            {
                var nuevalistamenu = source.Where(a => a.PadreMenu.Equals(PadreMEnu)).ToList();
                foreach (var i in nuevalistamenu)
                {
                    TreeNode newNode = new TreeNode(i.NombreMenu, i.IdMenu.ToString());
                    if (parentNode == null)
                    {
                        TreeView1.Nodes.Add(newNode);
                    }
                    else
                    {
                        parentNode.ChildNodes.Add(newNode);
                    }
                    CreateeTreeView(source, i.IdMenu, newNode);
                }
            }
            catch (Exception ex)
            {

            }
            
        }



        WCFSeguridad.ListaPerfil listaperilesuser = new WCFSeguridad.ListaPerfil();
        WCFSeguridad.ListaMenu listamenuUser = new WCFSeguridad.ListaMenu();

        void HabilitarCheckBox()
        {
            try
            {
                oPerfil = (WCFSeguridad.Perfil)Session["PerfilSeleccionado"];
                listamenuUser = ServiceSeguridadController.Instance.GetListarMenuPerfil(oPerfil);

                var listaMenuHijo = new WCFSeguridad.ListaMenu();
                var listaMenuPadre = new WCFSeguridad.ListaMenu();


                //DIVIDIR LISTAS MENU AND SUBMENU
                for (int n = 0; n <= listamenuUser.Count - 1; n++)
                {
                    if (listamenuUser[n].PadreMenu > Constantes.ValorCero)
                    {
                        listaMenuHijo.Add(listamenuUser[n]);
                    }
                    else
                    {
                        listaMenuPadre.Add(listamenuUser[n]);
                    }
                }


                //HABILITAR MENU PADRE
                for (int i = 0; i <= TreeView1.Nodes.Count - 1; i++)
                {
                    var tempp = TreeView1.Nodes[i].Value;
                    for (int mp = 0; mp <= listaMenuPadre.Count - 1; mp++)
                    {
                        if (listaMenuPadre[mp].IdMenu.ToString() == tempp)
                        {
                            TreeView1.Nodes[i].Checked = true;
                        }
                    }

                    //HABILITAR SUBMENU
                    int contador = Constantes.ValorCero;///contador

                    for (int j = 0; j <= TreeView1.Nodes[i].ChildNodes.Count - 1; j++)
                    {
                        var temp = TreeView1.Nodes[i].ChildNodes[j].Value;

                        for (int nnn = 0; nnn <= listaMenuHijo.Count - 1; nnn++)
                        {
                            if (listaMenuHijo[nnn].IdMenu.ToString() == temp)
                            {
                                TreeView1.Nodes[i].ChildNodes[j].Checked = true;
                                contador++;////contador
                                break;
                            }
                        }
                    }

                    if (contador < TreeView1.Nodes[i].ChildNodes.Count)////contador
                    {
                        TreeView1.Nodes[i].Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            
        }

        WCFSeguridad.ListaPerfil listaperfilChecked = new WCFSeguridad.ListaPerfil();
        WCFSeguridad.Perfil perfilChecked = new WCFSeguridad.Perfil();

        void ObtenerListadeValoresSeleccionados()
        {
            try
            {
                oPerfil = (WCFSeguridad.Perfil)Session["PerfilSeleccionado"];

                for (int i = 0; i <= TreeView1.Nodes.Count - 1; i++)
                {
                    perfilChecked = new WCFSeguridad.Perfil();

                    var perfiltemporalchecked = new WCFSeguridad.Perfil();//perfilchecked temporal

                    if (TreeView1.Nodes[i].Checked == true)
                    {
                        perfilChecked.IdPerfil = oPerfil.IdPerfil;
                        perfilChecked.IdMenu = Convert.ToInt32(TreeView1.Nodes[i].Value);
                        listaperfilChecked.Add(perfilChecked);
                    }

                    perfiltemporalchecked = new WCFSeguridad.Perfil();//menuperfil checked padre temporal
                    //perfiltemporalchecked = perfilChecked;//menuperfil checked padre temporal
                    perfiltemporalchecked.IdPerfil = oPerfil.IdPerfil;
                    perfiltemporalchecked.IdMenu = Convert.ToInt32(TreeView1.Nodes[i].Value);

                    int contador = Constantes.ValorCero; //comntador para verificar check
                    for (int j = 0; j <= TreeView1.Nodes[i].ChildNodes.Count - 1; j++)
                    {
                        if (TreeView1.Nodes[i].ChildNodes[j].Checked == true)
                        {
                            perfilChecked = new WCFSeguridad.Perfil();
                            perfilChecked.IdPerfil = oPerfil.IdPerfil;
                            perfilChecked.IdMenu = Convert.ToInt32(TreeView1.Nodes[i].ChildNodes[j].Value);
                            listaperfilChecked.Add(perfilChecked);
                            contador++;
                        }
                    }

                    if (contador > 0)
                    {
                        listaperfilChecked.Insert(0, perfiltemporalchecked);
                    }
                }
            }
            catch (Exception ex) { }
        }

        public string msje = string.Empty;

        protected void btnPrueba_Click(object sender, EventArgs e)
        {
            
        }

        WCFSeguridad.ListaPerfil listaperfilnochecked = new WCFSeguridad.ListaPerfil();
        WCFSeguridad.Perfil perfilnochecked = new WCFSeguridad.Perfil();

        void ObtenerValorNoChecked()
        {
            try
            {
                oPerfil = (WCFSeguridad.Perfil)Session["PerfilSeleccionado"];

                for (int i = 0; i <= TreeView1.Nodes.Count - 1; i++)
                {
                    perfilnochecked = new WCFSeguridad.Perfil();

                    var perfiltemporal = new WCFSeguridad.Perfil();//menuperfil padre temporal

                    if (TreeView1.Nodes[i].Checked == false)
                    {
                        perfilnochecked.IdPerfil = oPerfil.IdPerfil;
                        perfilnochecked.IdMenu = Convert.ToInt32(TreeView1.Nodes[i].Value);
                        listaperfilnochecked.Add(perfilnochecked);
                    }
                    perfiltemporal = new WCFSeguridad.Perfil();//menuperfil padre temporal
                    perfiltemporal = perfilnochecked;//menuperfil padre temporal

                    for (int j = 0; j <= TreeView1.Nodes[i].ChildNodes.Count - 1; j++)
                    {
                        if (TreeView1.Nodes[i].ChildNodes[j].Checked == false)
                        {
                            perfilnochecked = new WCFSeguridad.Perfil();
                            perfilnochecked.IdPerfil = oPerfil.IdPerfil;
                            perfilnochecked.IdMenu = Convert.ToInt32(TreeView1.Nodes[i].ChildNodes[j].Value);
                            listaperfilnochecked.Add(perfilnochecked);
                        }
                        else
                        {
                            listaperfilnochecked.Remove(perfiltemporal);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }



        void ValidarMenuPadre()
        {

        }

        #endregion

        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            try
            {
                int num = Convert.ToInt32(TreeView1.SelectedNode.Value);
                Response.Write("<script language=javascript>alert('Seleccionaste el menu nro: " + num + "');</script>");
            }
            catch (Exception ex)
            {

            }
            
        }


        void ObtenerValorSeleccionado()
        {
            try
            {
                int num = Convert.ToInt32(TreeView1.SelectedNode.Value);
                Response.Write("<script language=javascript>alert('Seleccionaste el menu nro: " + num + "');</script>");
            }
            catch (Exception ex)
            {

            }
            
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                ObtenerListadeValoresSeleccionados();
                for (int i = 0; i <= listaperfilChecked.Count - 1; i++)
                {
                    msje = ServiceSeguridadController.Instance.InsertarMenuPerfil(listaperfilChecked[i]);
                }

                ObtenerValorNoChecked();
                for (int i = 0; i <= listaperfilnochecked.Count - 1; i++)
                {
                    msje = ServiceSeguridadController.Instance.DeleteMenuPerfil(listaperfilnochecked[i]);
                }

                lblmsje.InnerText = "¡Cambios Guardados Correctamente!";

                //Response.Write("<script language=javascript>alert('Cambios Guardados Correctamente');</script>");
            }
            catch (Exception ex) { }
        }
    }
}