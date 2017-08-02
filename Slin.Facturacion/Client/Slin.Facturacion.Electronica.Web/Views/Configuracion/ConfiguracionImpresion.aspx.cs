using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Slin.Facturacion.Proxies.ServicioFacturacion;
using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;
using WCFMantenimiento = Slin.Facturacion.Proxies.ServicioMantenimiento;
using WCFConfiguracion = Slin.Facturacion.Proxies.ServicioConfiguracion;
using Slin.Facturacion.Common;
using Slin.Facturacion.Electronica.Web.ServiceFacturacion;
using Slin.Facturacion.Electronica.Web.Views.Util;

namespace Slin.Facturacion.Electronica.Web.Views.Configuracion
{
    public partial class ConfiguracionImpresion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                Cargar();
                Mensaje();
            }
        }

        #region ENTITY
        ServiceWebFactController objController = new ServiceWebFactController();

        private ListaEstado olistaestado;
        public ListaEstado oListaEstado
        {
            get { return olistaestado; }
            set { olistaestado = value; }
        }

        private WCFSeguridad.Usuario ousuarioLogeado;
        public WCFSeguridad.Usuario oUsuarioLogeado
        {
            get { return ousuarioLogeado; }
            set { ousuarioLogeado = value; }
        }

        private ListaTipoDocumento olistatipoDocumento;
        public ListaTipoDocumento oListaTipoDocumento
        {
            get { return olistatipoDocumento; }
            set { olistatipoDocumento = value; }
        }

        private ListaEstadoPrint olistaestadoPrint;
        public ListaEstadoPrint oListaEstadoPrint
        {
            get { return olistaestadoPrint; }
            set { olistaestadoPrint = value; }
        }

        private ListaEstadoPrint olistinsertPrint;
        public ListaEstadoPrint oListInsertPrint
        {
            get { return olistinsertPrint; }
            set { olistinsertPrint = value; }
        }

        private ListaEstadoPrint olistdeletePrint;
        public ListaEstadoPrint oListDeletePrint
        {
            get { return olistdeletePrint; }
            set { olistdeletePrint = value; }
        }

        private EstadoPrint objprint;
        public EstadoPrint ObjPrint
        {
            get { return objprint; }
            set { objprint = value; }
        }

        #endregion

        #region METHOD

        void Mensaje()
        {
            try
            {
                string respuesta = (string)Session["MensajeUpdChangeTipoDocPrint"];
                //if (respuesta == Constantes.msjActualizado)
                if (respuesta.Length > 0)
                {
                    Session.Remove("MensajeUpdChangeTipoDocPrint");
                    Response.Write("<script language=javascript>alert('" + respuesta + "');</script>");
                }
                
            }
            catch (Exception ex)
            {

            }
        }

        void ObtenerUsuarioLogeado()
        {
            try
            {
                oUsuarioLogeado = objController.GetUserLogueado_Fact();
            }
            catch (Exception ex) { }
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
            catch (Exception ex) { }
        }

        void CargarLista()
        {
            try
            {
                oListaEstado = ServiceFacturacionController.Instance.ListarEstadoDocumento();
                oListaEstado.RemoveAt(0);

                cboestado.DataSource = oListaEstado;
                cboestado.DataValueField = "IdEstado";
                cboestado.DataTextField = "Descripcion";
                cboestado.DataBind();
            }
            catch (Exception ex)
            {

            }
        }

        void Cargar()
        {
            string Url = HttpContext.Current.Request.Url.AbsoluteUri;
            int result = objController.Process_UrlPage(Url);
            if (result == Constantes.ValorCero)
            { Response.Redirect("~/Views/Home/Inicio.aspx"); }

            try
            {
                //Session.Remove("ListaAllTipoDocumento");
                ObtenerUsuarioLogeado();
                lblempresa.InnerText = oUsuarioLogeado.Empresa.RazonSocial.ToUpper();
                CargarLista();
                //CapturarUserLogRoles();
                logoEmpresa.Src = "../../Img/home/" + oUsuarioLogeado.Empresa.RUC + ".png";

                oListaTipoDocumento = new ListaTipoDocumento();
                oListaTipoDocumento = ServiceFacturacionController.Instance.ListarTipoDocumento();

                oListaTipoDocumento.RemoveAt(0);
                CreateeTreeView(oListaTipoDocumento, 0, null);

                ListarTipoDocumentoPrint();
                //Session["ListaAllTipoDocumento"] = oListaTipoDocumento;


                oListaEstado = new ListaEstado();
                oListaEstado = ServiceFacturacionController.Instance.ListarEstadoDocumento();
                oListaEstado.RemoveAt(0);
                oListaEstado.Insert(0, new Estado() { IdEstado = Constantes.ValorCero, Descripcion = Constantes.ValorSeleccione });

                Session["ListStatusDocumentPrint"] = oListaEstado;

                CargaListTypeDocumentPrint();
                Session["oListTypeDocumentPrint"] = oListTypeDocument;
            }
            catch (Exception ex)
            {

            }
        }

        private void CreateeTreeView(ListaTipoDocumento source, int PadreTipoDoc, TreeNode parentNode)
        {
            try
            {
                var nuevalistamenu = source.Where(a => a.Padre.Equals(PadreTipoDoc)).ToList();
                foreach (var i in nuevalistamenu)
                {
                    //TreeNode newNode = new TreeNode(i.Descripcion, i.IdTipoDocumento.ToString());
                    TreeNode newNode = new TreeNode(i.Descripcion, i.CodigoDocumento);
                    if (parentNode == null)
                    {
                        TreeView1.Nodes.Add(newNode);
                    }
                    else
                    {
                        parentNode.ChildNodes.Add(newNode);
                    }
                    CreateeTreeView(source, i.IdTipoDocumento, newNode);
                }
            }
            catch (Exception ex)
            {

            }
        }
        void ListarTipoDocumentoPrint()
        {
            ObtenerUsuarioLogeado();
            oListaEstadoPrint = new ListaEstadoPrint();

            oListaEstadoPrint = ServiceFacturacionController.Instance.GetListEstadoPrint(int.Parse(cboestado.Value), oUsuarioLogeado.Empresa.RUC);

            HabilitarCheckContent(oListaEstadoPrint);
        }
        void HabilitarCheckContent(ListaEstadoPrint objlista)
        {
            try
            {
                for (int j = 0; j <= TreeView1.Nodes.Count - 1; j++)
                {
                    TreeView1.Nodes[j].Checked = false;
                }

                //var listaAlltpoDoc = (ListaTipoDocumento)Session["ListaAllTipoDocumento"];

                for (int i = 0; i <= TreeView1.Nodes.Count - 1; i++)
                {
                    for (int mp = 0; mp <= objlista.Count - 1; mp++)
                    {
                        //if (TreeView1.Nodes[i].Value == objlista[mp].TipoDocumento.IdTipoDocumento.ToString())
                        if (TreeView1.Nodes[i].Value == objlista[mp].TipoDocumento.CodigoDocumento)
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

        protected void cboestado_ServerChange(object sender, EventArgs e)
        {
            try
            {
                ListarTipoDocumentoPrint();
                string script = @"<script type='text/javascript'>";
                script += @"</script>";
                Response.Write(script);
            }
            catch (Exception ex)
            {

            }
        }
        private void LlenarLista()
        {
            try
            {
                ObtenerUsuarioLogeado();
                oListInsertPrint = new ListaEstadoPrint();
                oListDeletePrint = new ListaEstadoPrint();

                for (int i = 0; i <= TreeView1.Nodes.Count - 1; i++)
                {
                    ObjPrint = new EstadoPrint();
                    ObjPrint.Empresa = new Empresa();
                    ObjPrint.TipoDocumento = new TipoDocumento();

                    ObjPrint.TipoDocumento.CodigoDocumento = TreeView1.Nodes[i].Value;
                    ObjPrint.IdEstadoPrint = int.Parse(cboestado.Value);
                    ObjPrint.Empresa.RUC = oUsuarioLogeado.Empresa.RUC;

                    if (TreeView1.Nodes[i].Checked == true)
                    {
                        oListInsertPrint.Add(ObjPrint);
                    }
                    else
                    {
                        oListDeletePrint.Add(ObjPrint);
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script language=javascript>alert('Error: " + ex.Message + "');</script>");
            }
        }
        private void GuardarCambiosPrintDocument()
        {
            try
            {
                LlenarLista();
                
                if (oListInsertPrint.Count > 0)
                {
                    var resultInsert = ServiceFacturacionController.Instance.InsertTipoDocumentPrint(oListInsertPrint);
                    if (oListInsertPrint.Count == TreeView1.Nodes.Count)
                    {
                        Session["MensajeUpdChangeTipoDocPrint"] = Constantes.msjActualizado;
                    }
                }

                if (oListDeletePrint.Count > 0)
                {
                    var resulDelete = ServiceFacturacionController.Instance.DeleteTipoDocumentPrint(oListDeletePrint);
                    Session["MensajeUpdChangeTipoDocPrint"] = Constantes.msjActualizado;
                }
                Response.Redirect("ConfiguracionImpresion");
            }
            catch (Exception ex)
            {
                //Session["MensajeUpdChangeTipoDocPrint"] = ex.Message;
                Response.Write("<script language=javascript>alert('Error: " + ex.Message + "');</script>");
            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            GuardarCambiosPrintDocument();
        }
        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
        }




        #region OTHER METHOD CONFIG
        private WCFConfiguracion.ListaTipoDocumento olisttypedocument;
        public WCFConfiguracion.ListaTipoDocumento oListTypeDocument
        {
            get { return olisttypedocument; }
            set { olisttypedocument = value; }
        }

        private void CargaListTypeDocumentPrint()
        {
            ObtenerUsuarioLogeado();

            oListTypeDocument = new WCFConfiguracion.ListaTipoDocumento();
            oListTypeDocument = ServiceConfiguracionController.Instance.ListTypeDocument_TypePrint(oUsuarioLogeado.Empresa.RUC);

            GVListaTypeDocument.DataSource = oListTypeDocument;
            GVListaTypeDocument.DataBind();
        }

        int inicia = Constantes.ValorCero;

        private void GeneraDropDownList(DropDownList cboCombo)
        {
            oListaEstado = new ListaEstado();
            oListaEstado = (ListaEstado)Session["ListStatusDocumentPrint"];

            cboCombo.DataSource = oListaEstado;
            cboCombo.DataTextField = "Descripcion";
            cboCombo.DataValueField = "IdEstado";
            cboCombo.DataBind();

            cboCombo.SelectedValue = oListTypeDocument[inicia].Estado.IdEstado + string.Empty;
            inicia++;
        }

        private void BinData(int edit)
        {
            oListTypeDocument = new WCFConfiguracion.ListaTipoDocumento();
            oListTypeDocument = (WCFConfiguracion.ListaTipoDocumento)Session["oListTypeDocumentPrint"];

            GVListaTypeDocument.DataSource = oListTypeDocument;
            GVListaTypeDocument.DataBind();

            if (edit != Constantes.ValorMenosUno)
            {
                GridViewRow row = GVListaTypeDocument.Rows[edit];
                ((DropDownList)row.FindControl("cboEstadoRow")).Enabled = true;
            }
        }

        #endregion

        protected void GVListaTypeDocument_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList combo = (DropDownList)e.Row.FindControl("cboEstadoRow");
                    combo.ClearSelection();

                    if (!object.ReferenceEquals(combo, DBNull.Value))
                    {
                        this.GeneraDropDownList(combo);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void GVListaTypeDocument_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            ObtenerUsuarioLogeado();

            WCFConfiguracion.TipoDocumento oType = new WCFConfiguracion.TipoDocumento();
            oType.Estado = new WCFConfiguracion.Estado();
            oType.Empresa = new WCFConfiguracion.Empresa();

            GridViewRow row = GVListaTypeDocument.Rows[e.RowIndex];

            oType.CodigoDocumento = GVListaTypeDocument.Rows[e.RowIndex].Cells[2].Text;
            oType.Estado.IdEstado = int.Parse(((DropDownList)row.FindControl("cboEstadoRow")).SelectedItem.Value);
            oType.Empresa.RUC = oUsuarioLogeado.Empresa.RUC;

            var result = ServiceConfiguracionController.Instance.InsertTypeDocument_ForPrint(oType);
            Session["MensajeUpdChangeTipoDocPrint"] = result;

            Response.Redirect("ConfiguracionImpresion");
            GVListaTypeDocument.EditIndex = -1;
            

        }

        protected void GVListaTypeDocument_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GVListaTypeDocument.EditIndex = e.NewEditIndex;
            int num = e.NewEditIndex;
            BinData(num);
        }

        protected void GVListaTypeDocument_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GVListaTypeDocument.EditIndex = -1;
            BinData(-1);
        }

    }
}