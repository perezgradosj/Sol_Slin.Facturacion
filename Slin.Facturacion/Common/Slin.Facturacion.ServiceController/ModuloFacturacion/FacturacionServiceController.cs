using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Slin.Facturacion.Proxies.ServicioFacturacion;

namespace Slin.Facturacion.ServiceController
{
    public sealed class FacturacionServiceController
    {

        private static readonly FacturacionServiceController instance = new FacturacionServiceController();
        static FacturacionServiceController() { }
        private FacturacionServiceController() { }
        public static FacturacionServiceController Instance { get { return instance; } }



        #region CONSULTAS

        public ListaFacturaElectronica ListarDocumentoCabecera(FacturaElectronica oFacturaElectronica)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.ListarDocumentoCabecera(oFacturaElectronica);
            }
        }

        public ListaDetalleFacturaElectronica ListarDocumentoDetalle(String tipoDocumento, String NumeroSerie, String NumeroDocumento, String RucEmpresa)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.ListarDocumentoDetalle(tipoDocumento, NumeroSerie, NumeroDocumento, RucEmpresa);
            }
        }

        public String GenerarXML(String tipoDocumento, String NumeroSerie, String NumeroDocumento, String RucEmpresa)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.GenerarXML(tipoDocumento, NumeroSerie, NumeroDocumento, RucEmpresa);
            }
        }

        public ListaFacturaElectronica GetListaMontoCab(String tipoDocumento, String NumeroSerie, String NumeroDocumento, String RucEmpresa)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.GetListaMontoCab(tipoDocumento, NumeroSerie, NumeroDocumento, RucEmpresa);
            }
        }

        #endregion

        #region CRE, CPE

        public ListaFacturaElectronica ListarDocumentoCabeceraCRECPE(FacturaElectronica oFacturaElectronica)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.ListarDocumentoCabeceraCRECPE(oFacturaElectronica);
            }
        }

        #endregion

        #region LISTAS

        public ListaEstado ListarEstadoDocumento()
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.ListarEstadoDocumento();
            }
        }

        public ListaTipoDocumento ListarTipoDocumento()
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.ListarTipoDocumento();
            }
        }

        public ListaSerie ListarSerie(Serie oSerie)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.ListarSerie(oSerie);
            }
        }

        #endregion

        #region DOCUMENTO ANULADO

        public FacturaElectronica GetFechaDocumento(int tpodocumento, String serie, String nrodocumento, String rucempresa)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.GetFechaDocumento(tpodocumento, serie, nrodocumento, rucempresa);
            }
        }

        public ListaFacturaElectronica GetValidarDocumentoExiste(FacturaElectronica oDocAnulado)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.GetValidarDocumentoExiste(oDocAnulado);
            }
        }

        public String InsertarDocumentoAnulado(FacturaElectronica oDocumentoAnulado)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.InsertarDocumentoAnulado(oDocumentoAnulado);
            }
        }

        public ListaDocumento ValidarExisteDocAnulado(FacturaElectronica oDocumentoAnulado)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.ValidarExisteDocAnulado(oDocumentoAnulado);
            }
        }

        public ListaFacturaElectronica GetListaDocumentoAnulado(FacturaElectronica oDocumentoAnulado)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.GetListaDocumentoAnulado(oDocumentoAnulado);
            }
        }


        public FacturaElectronica GetObtenerDocumentoUnico(FacturaElectronica oFacturaElectronica)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.GetObtenerDocumentoUnico(oFacturaElectronica);
            }
        }

        #endregion

        #region UTIL
        public ListaDocumento GetListaDocumentoCabExcel(FacturaElectronica oFactura)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.GetListaDocumentoCabExcel(oFactura);
            }
        }

        #endregion

        #region RESUMEN RC, RA AND RR

        public ListaEstado GetListaTipoFecha()
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.GetListaTipoFecha();
            }
        }

        public ListaFacturaElectronica GetListaCabeceraRC(FacturaElectronica documentoRC)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.GetListaCabeceraRC(documentoRC);
            }
        }

        public ListaFacturaElectronica GetListaCabeceraRA(FacturaElectronica documentoRA)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.GetListaCabeceraRA(documentoRA);
            }
        }

        public ListaFacturaElectronica GetListaCabeceraRR(FacturaElectronica documentoRR)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.GetListaCabeceraRR(documentoRR);
            }
        }

        public ListaDetalleFacturaElectronica GetListaDetalleRC(FacturaElectronica documentoRC)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.GetListaDetalleRC(documentoRC);
            }
        }

        public ListaDetalleFacturaElectronica GetListaDetalleRA(FacturaElectronica documentoRA)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.GetListaDetalleRA(documentoRA);
            }
        }

        public ListaDetalleFacturaElectronica GetListaDetalleRR(FacturaElectronica documentoRR)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.GetListaDetalleRR(documentoRR);
            }
        }

        #endregion

        #region ESTADO SISTEMA DOCUMENTO

        public ListaSistemaEstado GetListaEstadoSistema_Ok(string RucEntity)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.GetListaEstadoSistema_Ok(RucEntity);
            }
        }

        public ListaSistemaEstado GetListaEstadoSistema_Error(string RucEntity)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.GetListaEstadoSistema_Error(RucEntity);
            }
        }
        #endregion

        #region DOCUMENT RECIVED

        public ListaFacturaElectronica GetListaDocumentReceived(FacturaElectronica oFacturaElectronica)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.GetListaDocumentReceived(oFacturaElectronica);
            }
        }

        #endregion

        #region DOCUMENTO ENVIADO

        public String InsertarDocumentoEnviado(Documento odocumento)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.InsertarDocumentoEnviado(odocumento);
            }
        }

        public ListaDocumento GetListaDocumentoEnviado(Documento odocumento)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.GetListaDocumentoEnviado(odocumento);
            }
        }

        #endregion

        #region CATALOGO

        public String InsertTipoDocumentSend(ListaEstadoEnvio ListSend)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.InsertTipoDocumentSend(ListSend);
            }
        }

        public String InsertTipoDocumentPrint(ListaEstadoPrint ListPrint)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.InsertTipoDocumentPrint(ListPrint);
            }
        }

        public String DeleteTipoDocumentSend(ListaEstadoEnvio ListSend)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.DeleteTipoDocumentSend(ListSend);
            }
        }

        public String DeleteTipoDocumentPrint(ListaEstadoPrint ListPrint)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.DeleteTipoDocumentPrint(ListPrint);
            }
        }

        public ListaEstadoEnvio GetListEstadoEnvio(int IdEstadoEnvio, string RucEmpresa)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.GetListEstadoEnvio(IdEstadoEnvio, RucEmpresa);
            }
        }

        public ListaEstadoPrint GetListEstadoPrint(int IdEstadoPrint, string RucEmpresa)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.GetListEstadoPrint(IdEstadoPrint, RucEmpresa);
            }
        }

        #endregion

        #region exchangerate

        public ListExchangeRate Get_ExchangeRate_Today(DateTime fecha)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.Get_ExchangeRate_Today(fecha);
            }
        }

        #endregion

        #region list pendings document

        public ListaFacturaElectronica GetList_PendingsDocuments_RA(string ruccomp, string type)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.GetList_PendingsDocuments_RA(ruccomp, type);
            }
        }

        public ListaFacturaElectronica GetList_PendingsDocuments_RC(string ruccomp)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.GetList_PendingsDocuments_RC(ruccomp);
            }
        }

        public ListaFacturaElectronica GetList_PendingsDocuments_ErrorSend(string ruccomp)
        {
            using (ServicioFacturacionClient Client = new ServicioFacturacionClient())
            {
                return Client.GetList_PendingsDocuments_ErrorSend(ruccomp);
            }
        }

        #endregion
    }
}
