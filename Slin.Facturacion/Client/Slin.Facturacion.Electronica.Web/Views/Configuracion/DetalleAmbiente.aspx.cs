using Slin.Facturacion.Common;
using Slin.Facturacion.Electronica.Web.ServiceFacturacion;
using Slin.Facturacion.Proxies.ServicioConfiguracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;
using WCFFacturacion = Slin.Facturacion.Proxies.ServicioFacturacion;
using Slin.Facturacion.Electronica.Web.Views.Util;

namespace Slin.Facturacion.Electronica.Web.Views.Configuracion
{
    public partial class DetalleAmbiente : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                Cargar();
                //Mensaje();
            }
        }


        #region ENTITY

        ServiceWebFactController objController = new ServiceWebFactController();

        private ListaDocumentoAmbiente olistaDocumentoAmbiente;
        public ListaDocumentoAmbiente oListaDocumentoAmbiente
        {
            get { return olistaDocumentoAmbiente; }
            set { olistaDocumentoAmbiente = value; }
        }

        private ListaDocumentoAmbiente objlistaUpdate;
        public ListaDocumentoAmbiente ObjListaUpdate
        {
            get { return objlistaUpdate; }
            set { objlistaUpdate = value; }
        }

        private DocumentoAmbiente objdocamb;
        public DocumentoAmbiente ObjDocAmb
        {
            get { return objdocamb; }
            set { objdocamb = value; }
        }


        private WCFSeguridad.Usuario ousuarioLogeado;
        public WCFSeguridad.Usuario oUsuarioLogeado
        {
            get { return ousuarioLogeado; }
            set { ousuarioLogeado = value; }
        }


        private WCFFacturacion.ListaTipoDocumento olistatipodocument;
        public WCFFacturacion.ListaTipoDocumento oListaTipoDocument
        {
            get { return olistatipodocument; }
            set { olistatipodocument = value; }
        }

        private DocumentoAmbiente docambient;
        public DocumentoAmbiente DocAmbient
        {
            get { return docambient; }
            set { docambient = value; }
        }

        private ListaDocumentoAmbiente listinsertDocAmb;
        public ListaDocumentoAmbiente ListInsertDocAmb
        {
            get { return listinsertDocAmb; }
            set { listinsertDocAmb = value; }
        }

        private ListaDocumentoAmbiente listupdateDocAmb;
        public ListaDocumentoAmbiente ListUpdateDocAmb
        {
            get { return listupdateDocAmb; }
            set { listupdateDocAmb = value; }
        }

        #endregion


        #region METHOD

        private void RecibirObjetoSeleccionado()
        {
            oListaDocumentoAmbiente = (ListaDocumentoAmbiente)Session["ListaDocumentoAmbiente"];
        }

        private void ObtenerUsuarioLogeado()
        {
            try
            {
                oUsuarioLogeado = objController.GetUserLogueado_Fact();
            }
            catch (Exception ex) { }
        }

        private void Cargar()
        {
            string Url = HttpContext.Current.Request.Url.AbsoluteUri;
            int result = objController.Process_UrlPage(Url);
            if (result == Constantes.ValorCero)
            { Response.Redirect("~/Views/Home/Inicio.aspx"); }

            try
            {
                ObtenerUsuarioLogeado();
                RecibirObjetoSeleccionado();
                lblempresa.InnerText = oUsuarioLogeado.Empresa.RazonSocial.ToUpper();
                logoEmpresa.Src = "../../Img/home/" + oUsuarioLogeado.Empresa.RUC + ".png";

                CargarListaTreeView();


                var nombreAmbiente = (string)Session["NombreAmbienteSeleccionado"];
                if (nombreAmbiente != null)
                {
                    lblambienteselec.InnerText = nombreAmbiente;
                }

                int idamselected = (int)Session["IdAmbiente"];

                lblidambSelected.InnerText = idamselected + string.Empty;


                var SecondaryUser = ServiceConfiguracionController.Instance.Get_SecondaryUserSunat(idamselected, oUsuarioLogeado.Empresa.RUC);
                if (SecondaryUser.Count == 1)
                {
                    txtuseramb.Value = new Helper.Encrypt().DecryptKey(SecondaryUser[0].UserName);
                    txtclaveamb.Value = SecondaryUser[0].Password;
                    lbluseclaveamb.InnerText = SecondaryUser[0].Password;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void CargarListaTreeView()
        {
            oListaTipoDocument = new WCFFacturacion.ListaTipoDocumento();

            oListaTipoDocument = ServiceFacturacionController.Instance.ListarTipoDocumento();
            oListaTipoDocument.RemoveAt(0);

            CreateetreeView(oListaTipoDocument, 0, null);
            HabilitarCheckContent(oListaDocumentoAmbiente);
        }


        private void CreateetreeView(WCFFacturacion.ListaTipoDocumento source, int PadreTipoDoc, TreeNode parentNode)
        {
            try
            {
                var nuevalista = source.Where(a => a.Padre.Equals(PadreTipoDoc)).ToList();

                foreach (var i in nuevalista)
                {
                    TreeNode newNode = new TreeNode(i.Descripcion, i.CodigoDocumento); ;

                    if (parentNode == null)
                    {
                        TreeView1.Nodes.Add(newNode);
                    }
                    else
                    {
                        parentNode.ChildNodes.Add(newNode);//AGREGA SI HAY SUB NODO
                    }
                    CreateetreeView(source, i.IdTipoDocumento, newNode);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void HabilitarCheckContent(ListaDocumentoAmbiente objlista)
        {
            try
            {
                for (int j = 0; j <= TreeView1.Nodes.Count - 1; j++)
                {
                    TreeView1.Nodes[j].Checked = false;
                }

                for (int i = 0; i <= TreeView1.Nodes.Count - 1; i++)
                {
                    for (int mp = 0; mp <= objlista.Count - 1; mp++)
                    {
                        if (TreeView1.Nodes[i].Value == objlista[mp].TIPO_CE)
                        {
                            TreeView1.Nodes[i].Checked = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            GuardarCambiosTipoDocAmb();
        }


        private void LlenarLista()
        {
            ListInsertDocAmb = new ListaDocumentoAmbiente();
            ListUpdateDocAmb = new ListaDocumentoAmbiente();

            try
            {
                ObtenerUsuarioLogeado();
                int idamb = (int)Session["IdAmbiente"];

                for (int i = 0; i <= TreeView1.Nodes.Count - 1; i++)
                {
                    DocAmbient = new DocumentoAmbiente();
                    DocAmbient.Empresa = new Empresa();
                    DocAmbient.Estado = new Estado();
                    DocAmbient.AmbienteSunat = new AmbienteSunat();

                    DocAmbient.TIPO_CE = TreeView1.Nodes[i].Value;
                    DocAmbient.Empresa.RUC = oUsuarioLogeado.Empresa.RUC;
                    DocAmbient.AmbienteSunat.IdAmbiente = idamb;

                    if (TreeView1.Nodes[i].Checked == true)
                    {
                        DocAmbient.Estado.IdEstado = Constantes.ValorUno;
                        ListInsertDocAmb.Add(DocAmbient);
                    }
                    else
                    {
                        DocAmbient.Estado.IdEstado = Constantes.ValorDos;
                        ListUpdateDocAmb.Add(DocAmbient);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void GuardarCambiosTipoDocAmb()
        {
            LlenarLista();

            Session.Remove("ListaDocumentoAmbiente");
            Session.Remove("IdAmbiente");


            if (ListInsertDocAmb.Count > 0)
            {
                var resultinsert = ServiceConfiguracionController.Instance.InsertDocumentAmbiente(ListInsertDocAmb);
                if (ListInsertDocAmb.Count == TreeView1.Nodes.Count)
                {
                    Session["MensajeUpdDocAmd"] = resultinsert;
                }
            }

            if (ListUpdateDocAmb.Count > 0)
            {
                var resultupdate = ServiceConfiguracionController.Instance.UpdateDocAmbienteEstado(ListUpdateDocAmb);
                Session["MensajeUpdDocAmd"] = resultupdate;
            }
            
            Response.Redirect("ConfiguracionURL");
        }

        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {

        }

        protected void btnSaveUserAmb_Click(object sender, EventArgs e)
        {
            Execute_SaveSecondaryUserSNT();
        }

        private void Execute_SaveSecondaryUserSNT()
        {
            LlenarObjectSecondayUserSunat();

            var result = ServiceConfiguracionController.Instance.Insert_SecondaryUserSunat_Amb(objSecondaryUser);

            if (result == Constantes.msjActualizado)
            {
                Response.Write("<script language=javascript>alert('" + result + "');</script>");
            }
        }


        SecondaryUser objSecondaryUser = new SecondaryUser();
        private void LlenarObjectSecondayUserSunat()
        {
            ObtenerUsuarioLogeado();

            objSecondaryUser = new SecondaryUser();

            objSecondaryUser.RucEntity = oUsuarioLogeado.Empresa.RUC;
            objSecondaryUser.UserName = new Helper.Encrypt().EncryptKey(txtuseramb.Value.Trim());
            objSecondaryUser.Password = txtclaveamb.Value.Length == Constantes.ValorCero ? lbluseclaveamb.InnerText : new Helper.Encrypt().EncryptKey(txtclaveamb.Value.Trim());
            objSecondaryUser.IdAmb = int.Parse(lblidambSelected.InnerText);
        }
    }
}