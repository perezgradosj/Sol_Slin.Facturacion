using Slin.Facturacion.Proxies.ServicioFacturacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Windows.Controls;

namespace Slin.Facturacion.Electronica.Web.Helper.Pdf
{
    public class GeneraPdf
    {
        GridView dv = new GridView();

        private DataTable dt;

        private void AgregarDocumento(FacturaElectronica oFact, DetalleFacturaElectronica oDet)
        {
            dt = new DataTable();

            dt.Columns.Add("Cantidad");
            dt.Columns.Add("1");
            dt.Columns.Add("2");
            dt.Columns.Add("3");
            dt.Columns.Add("4");
            dt.Columns.Add("5");
            dt.Columns.Add("6");
            
            
        }
    }
}