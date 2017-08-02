using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Slin.Facturacion.Electronica.Web.ServiceFacturacion;
using Slin.Facturacion.Proxies.ServicioResumen;
using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;
using System.Configuration;
using System.IO;
using System.Text;
using Slin.Facturacion.Common;
using Slin.Facturacion.Proxies.ServicioFacturacion;
using Slin.Facturacion.Electronica.Web.Views.Util;
using Slin.Facturacion.Common.Method;

namespace Slin.Facturacion.Electronica.Web.Views.Facturacion
{
    public partial class GenararRA : System.Web.UI.Page
    {
        //private string PathWriteFile = ConfigurationManager.AppSettings["PathWriteFile"].ToString();

        private string PathLogSLINADE = ConfigurationManager.AppSettings["PathLogSLINADE"];
        private string PathWriteFile = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                Cargar();
                Message();
            }
        }

        #region ENTITY

        ServiceWebFactController objController = new ServiceWebFactController();

        private WCFSeguridad.Usuario ousuarioLogeado;
        public WCFSeguridad.Usuario oUsuarioLogeado
        {
            get { return ousuarioLogeado; }
            set { ousuarioLogeado = value; }
        }

        private ListaFacturaElectronica olistfact;
        public ListaFacturaElectronica oList_FacturaElect
        {
            get { return olistfact; }
            set
            {
                olistfact = value;
            }
        }

        //RefactoryClass objRefact = new RefactoryClass();
        #endregion



        #region METHOD

        //private void CreateDirectory(string path)
        //{
        //    if (!System.IO.Directory.Exists(path))
        //    {
        //        System.IO.Directory.CreateDirectory(path);
        //    }
        //}

        void Cargar()
        {
            string Url = HttpContext.Current.Request.Url.AbsoluteUri;
            int result = objController.Process_UrlPage(Url);
            if (result == Constantes.ValorCero)
            { Response.Redirect("~/Views/Home/Inicio.aspx"); }

            try
            {
                txtfecharesumen.Value = DateTime.Now.ToShortDateString();
                oUsuarioLogeado = (WCFSeguridad.Usuario)Session["UsuarioLogueadoFact"];
                lblempresa.InnerText = oUsuarioLogeado.Empresa.RazonSocial.ToUpper();

                CapturarUserLogRoles();

                logoEmpresa.Src = "../../Img/home/" + oUsuarioLogeado.Empresa.RUC + ".png";

                ChargerList();
            }
            catch (Exception ex) { }
        }

        private void Message()
        {
            try
            {
                string respuesta = (string)Session["MessageGenerateRAProcess"];
                if (respuesta.Length > Constantes.ValorCero)
                {
                    Session.Remove("MessageGenerateRAProcess");
                    Response.Write("<script language=javascript>alert('" + respuesta + "');</script>");
                }
                
            }
            catch (Exception ex) { }
        }


        WCFSeguridad.ListaRol oListaRolesUserLog = new WCFSeguridad.ListaRol();
        void CapturarUserLogRoles()
        {
            try
            {
                oListaRolesUserLog = objController.GetList_RolUserLog();

                btnGenerar.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnGeneraSelected.Visible = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
                btnGeneraSelected.Enabled = Common.Helper.RolClass.Instance.ChargeUserRol(oListaRolesUserLog);
            }
            catch (Exception ex) { }
        }

        private void ChargerList()
        {
            ObtenerUsuarioLogueado();
            oList_FacturaElect = new ListaFacturaElectronica();

            oList_FacturaElect = ServiceFacturacionController.Instance.GetList_PendingsDocuments_RA(oUsuarioLogeado.Empresa.RUC, Constantes.Cod_RA);

            GVListDocumentPendingRA.DataSource = oList_FacturaElect;
            GVListDocumentPendingRA.DataBind();

            if (GVListDocumentPendingRA.Rows.Count > Constantes.ValorCero)
            {
                idSelectAll.Visible = true;
                lblseleccionar.Visible = true;
            }
            else
            {
                lblseleccionar.Visible = false;
                idSelectAll.Visible = false;
            }
        }

        void ObtenerUsuarioLogueado()
        {
            try
            {
                oUsuarioLogeado = objController.GetUserLogueado_Fact();
            }
            catch (Exception ex) { }
        }

        void GenerarResumen()
        {
            PathWriteFile = Singleton.Instance.Get_PathWriteOrder(PathLogSLINADE);
            Singleton.Instance.CreateDirectory(PathWriteFile + @"in\");
            try
            {
                ObtenerUsuarioLogueado();
                string msje = string.Empty;

                var fecha = Convert.ToDateTime(txtfecharesumen.Value);
                string fech = fecha.ToString("yyyy-MM-dd");



                string ContentFile = "CABECERA-PRINCIPAL|RA|" + fech + "|" + oUsuarioLogeado.Empresa.RUC;
                string NameFile = oUsuarioLogeado.Empresa.RUC + "-RA-" + fecha.ToString("yyyyMMdd");

                

                if (System.IO.File.Exists(PathWriteFile + @"in\" + NameFile + ".txt"))
                    System.IO.File.Delete(PathWriteFile + @"in" + NameFile + ".txt");

                using (StreamWriter sw = new StreamWriter(PathWriteFile + @"in\" + NameFile + ".txt", true, Encoding.UTF8))
                {
                    sw.WriteLine(ContentFile);
                }

                Response.Write("<script language=javascript>alert('" + Constantes.msjOrdenGenerate_RA + "');</script>");

                //if (System.IO.File.Exists(PathWriteFileRes + NameFile + ".txt"))
                //{
                //    Response.Write("<script language=javascript>alert('" + Constantes.msjOrdenGenerate_RA + "');</script>");
                //}
                //else
                //{
                //    Response.Write("<script language=javascript>alert('" + Constantes.msjOrdenGenerate_Res + PathWriteFileRes + "');</script>");
                //}
            }
            catch (Exception ex)
            {
                Response.Write("<script language=javascript>alert('" + ex.Message + "');</script>");
            }
            
        }

        #endregion





        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            GenerarResumen();
        }

        protected void btnGenerateOrder_Click(object sender, ImageClickEventArgs e)
        {
            PathWriteFile = Singleton.Instance.Get_PathWriteOrder(PathLogSLINADE);

            Singleton.Instance.CreateDirectory(PathWriteFile + @"in\");

            try
            {
                ObtenerUsuarioLogueado();

                ImageButton btnDetalles = sender as ImageButton;
                GridViewRow gvrow = (GridViewRow)btnDetalles.NamingContainer;
                
                var fecha = Convert.ToDateTime(GVListDocumentPendingRA.Rows[gvrow.RowIndex].Cells[1].Text);

                string fech = fecha.ToString("yyyy-MM-dd");

                string ContentFile = "CABECERA-PRINCIPAL|RA|" + fech + "|" + oUsuarioLogeado.Empresa.RUC;
                string NameFile = oUsuarioLogeado.Empresa.RUC + "-RA-" + fecha.ToString("yyyyMMdd");
                if (System.IO.File.Exists(PathWriteFile + @"in\" + NameFile + ".txt"))
                    System.IO.File.Delete(PathWriteFile + @"in\" + NameFile + ".txt");
                using (StreamWriter sw = new StreamWriter(PathWriteFile + @"in\" + @"\" + NameFile + ".txt", true, Encoding.UTF8))
                {
                    sw.WriteLine(ContentFile);
                }
                Response.Write("<script language=javascript>alert('Se a generado la Orden para generar el Resumen con fecha: " + fech + "');</script>");
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnGeneraSelected_Click(object sender, EventArgs e)
        {
            PathWriteFile = Singleton.Instance.Get_PathWriteOrder(PathLogSLINADE);
            Singleton.Instance.CreateDirectory(PathWriteFile + @"in\");

            var listSelect = new List<string>();
            for (int i = 0; i <= GVListDocumentPendingRA.Rows.Count - 1; i++)
            {
                CheckBox check = ((CheckBox)GVListDocumentPendingRA.Rows[i].Cells[0].FindControl("chkSelImp"));
                if (check.Checked)
                {
                    listSelect.Add("Select: " + (i + 1));
                }
            }

            try
            {
                


                if (GVListDocumentPendingRA.Rows.Count < Constantes.ValorUno)
                {
                    Response.Write("<script language=javascript>alert('No hay Registros');</script>");
                }
                if (listSelect.Count < Constantes.ValorUno)
                {
                    Response.Write("<script language=javascript>alert('Seleccione registros');</script>");
                }

                #region method generate resumen

                ObtenerUsuarioLogueado();

                var list_Write = new List<string>();



                for (int i = 0; i <= GVListDocumentPendingRA.Rows.Count - 1; i++)
                {
                    CheckBox check = ((CheckBox)GVListDocumentPendingRA.Rows[i].Cells[0].FindControl("chkSelImp"));

                    if (check.Checked)
                    {
                        var fecha = Convert.ToDateTime(GVListDocumentPendingRA.Rows[i].Cells[1].Text);

                        string fech = fecha.ToString("yyyy-MM-dd");

                        string ContentFile = "CABECERA-PRINCIPAL|RA|" + fech + "|" + oUsuarioLogeado.Empresa.RUC;
                        string NameFile = oUsuarioLogeado.Empresa.RUC + "-RA-" + fecha.ToString("yyyyMMdd");

                        if (System.IO.File.Exists(PathWriteFile + @"in\" + NameFile + ".txt"))
                            System.IO.File.Delete(PathWriteFile + @"in\" + NameFile + ".txt");

                        using (StreamWriter sw = new StreamWriter(PathWriteFile + @"in\" + NameFile + ".txt", true, Encoding.UTF8))
                        {
                            sw.WriteLine(ContentFile);
                        }
                        list_Write.Add(ContentFile + "*" + NameFile);
                    }
                }

                if (list_Write.Count > Constantes.ValorCero)
                {
                    //Response.Write("<script language=javascript>alert('Se Generaron " + list_Write.Count +" ordenes de resumen');</script>"); 
                    Session["MessageGenerateRAProcess"] = "Se Generaron " + list_Write.Count +" ordenes de resumen";
                    Response.Redirect("GenerarRA");
                }
                #endregion
            }
            catch (Exception ex) { }
            
        }
    }
}