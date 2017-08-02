using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Slin.Facturacion.Electronica.Web.ErrorPage
{
    public partial class ContactoSlinAde : System.Web.UI.Page
    {
        public string Msje = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            string respuesta = (string)Session["SessionListCompanyNull"];
            if (respuesta != null)
            {
                lblmsje.InnerText = respuesta;
            }
        }
    }
}