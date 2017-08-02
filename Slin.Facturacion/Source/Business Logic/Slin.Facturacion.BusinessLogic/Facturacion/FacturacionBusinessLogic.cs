using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Slin.Facturacion.BusinessEntities;
using Slin.Facturacion.DataAccess;

using System.Xml;

namespace Slin.Facturacion.BusinessLogic
{
    public class FacturacionBusinessLogic
    {

        public FacturacionDataAccess objFacturacionDataAccess = new FacturacionDataAccess();


        

        public ListaFacturaElectronica ListarDocumentoCabecera(FacturaElectronica oFacturaElectronica)
        {
            return objFacturacionDataAccess.ListarDocumentoCabecera(oFacturaElectronica);
        }

        public ListaDetalleFacturaElectronica ListarDocumentoDetalle(String tipoDocumento, String NumeroSerie, String NumeroDocumento, String RucEmpresa)
        {
            return objFacturacionDataAccess.ListarDocumentoDetalle(tipoDocumento, NumeroSerie, NumeroDocumento, RucEmpresa);
        }

        public ListaEstado ListarEstadoDocumento()
        {
            return objFacturacionDataAccess.ListarEstadoDocumento();
        }

        public ListaTipoDocumento ListarTipoDocumento()
        {
            return objFacturacionDataAccess.ListarTipoDocumento();
        }

        public ListaSerie ListarSerie(Serie oSerie)
        {
            return objFacturacionDataAccess.ListarSerie(oSerie);
        }


        public String GenerarXML(String tipoDocumento, String NumeroSerie, String NumeroDocumento, String RucEmpresa)
        {
            return objFacturacionDataAccess.GenerarXML(tipoDocumento, NumeroSerie, NumeroDocumento, RucEmpresa);
        }
    }
}
