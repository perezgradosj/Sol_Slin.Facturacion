using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Slin.Facturacion.Proxies.ServicioFacturacion;
using Slin.Facturacion.ServiceController;
using Slin.Facturacion.Proxies.ServicioResumen;
namespace Slin.Facturacion.Electronica.Web.ServiceFacturacion
{
    public sealed class ServiceFacturacionController
    {

        private static readonly ServiceFacturacionController instance = new ServiceFacturacionController();
        static ServiceFacturacionController() { }
        private ServiceFacturacionController() { }
        public static ServiceFacturacionController Instance { get { return instance; } }

        #region CONSULTAS

        public  ListaFacturaElectronica ListarDocumentoCabecera(FacturaElectronica oFacturaElectronica)
        {
            return FacturacionServiceController.Instance.ListarDocumentoCabecera(oFacturaElectronica);
        }

        public  ListaDetalleFacturaElectronica ListarDocumentoDetalle(String tipoDocumento, String NumeroSerie, String NumeroDocumento, String RucEmpresa)
        {
            return FacturacionServiceController.Instance.ListarDocumentoDetalle(tipoDocumento, NumeroSerie, NumeroDocumento, RucEmpresa);
        }

        public  String GenerarXML(String tipoDocumento, String NumeroSerie, String NumeroDocumento, String RucEmpresa)
        {
            return FacturacionServiceController.Instance.GenerarXML(tipoDocumento, NumeroSerie, NumeroDocumento, RucEmpresa);
        }

        public  ListaFacturaElectronica GetListaMontoCab(String tipoDocumento, String NumeroSerie, String NumeroDocumento, String RucEmpresa)
        {
            return FacturacionServiceController.Instance.GetListaMontoCab(tipoDocumento, NumeroSerie, NumeroDocumento, RucEmpresa);
        }

        public  FacturaElectronica GetObtenerDocumentoUnico(FacturaElectronica oFacturaElectronica)
        {
            return FacturacionServiceController.Instance.GetObtenerDocumentoUnico(oFacturaElectronica);
        }

        #endregion

        #region CRE, CPE

        public  ListaFacturaElectronica ListarDocumentoCabeceraCRECPE(FacturaElectronica oFacturaElectronica)
        {
            return FacturacionServiceController.Instance.ListarDocumentoCabeceraCRECPE(oFacturaElectronica);
        }

        #endregion


        #region LISTAS


        public  ListaEstado ListarEstadoDocumento()
        {
            return FacturacionServiceController.Instance.ListarEstadoDocumento();
        }

        public ListaTipoDocumento ListarTipoDocumento()
        {
            return FacturacionServiceController.Instance.ListarTipoDocumento();
        }
        public  ListaSerie ListarSerie(Serie oSerie)
        {
            return FacturacionServiceController.Instance.ListarSerie(oSerie);
        }

        #endregion


        #region DOCUMENTO ANULADO

        public  FacturaElectronica GetFechaDocumento(int tpodocumento, String serie, String nrodocumento, String rucempresa)
        {
            return FacturacionServiceController.Instance.GetFechaDocumento(tpodocumento, serie, nrodocumento, rucempresa);
        }

        public  ListaFacturaElectronica GetValidarDocumentoExiste(FacturaElectronica oDocAnulado)
        {
            return FacturacionServiceController.Instance.GetValidarDocumentoExiste(oDocAnulado);
        }

        public  String InsertarDocumentoAnulado(FacturaElectronica oDocumentoAnulado)
        {
            return FacturacionServiceController.Instance.InsertarDocumentoAnulado(oDocumentoAnulado);
        }

        public  ListaDocumento ValidarExisteDocAnulado(FacturaElectronica oDocumentoAnulado)
        {
            return FacturacionServiceController.Instance.ValidarExisteDocAnulado(oDocumentoAnulado);
        }

        public  ListaFacturaElectronica GetListaDocumentoAnulado(FacturaElectronica oDocumentoAnulado)
        {
            return FacturacionServiceController.Instance.GetListaDocumentoAnulado(oDocumentoAnulado);
        }

        #endregion

        #region UTIL

        public  ListaDocumento GetListaDocumentoCabExcel(FacturaElectronica oFactura)
        {
            return FacturacionServiceController.Instance.GetListaDocumentoCabExcel(oFactura);
        }


        #endregion

        #region RESUMEN RC, RA AND RR

        public  ListaEstado GetListaTipoFecha()
        {
            return FacturacionServiceController.Instance.GetListaTipoFecha();
        }

        public  ListaFacturaElectronica GetListaCabeceraRC(FacturaElectronica documentoRC)
        {
            return FacturacionServiceController.Instance.GetListaCabeceraRC(documentoRC);
        }

        public  ListaFacturaElectronica GetListaCabeceraRA(FacturaElectronica documentoRA)
        {
            return FacturacionServiceController.Instance.GetListaCabeceraRA(documentoRA);
        }

        public  ListaFacturaElectronica GetListaCabeceraRR(FacturaElectronica documentoRR)
        {
            return FacturacionServiceController.Instance.GetListaCabeceraRR(documentoRR);
        }

        public  ListaDetalleFacturaElectronica GetListaDetalleRC(FacturaElectronica documentoRC)
        {
            return FacturacionServiceController.Instance.GetListaDetalleRC(documentoRC);
        }

        public  ListaDetalleFacturaElectronica GetListaDetalleRA(FacturaElectronica documentoRA)
        {
            return FacturacionServiceController.Instance.GetListaDetalleRA(documentoRA);
        }

        public  ListaDetalleFacturaElectronica GetListaDetalleRR(FacturaElectronica documentoRR)
        {
            return FacturacionServiceController.Instance.GetListaDetalleRR(documentoRR);
        }
        #endregion

        #region ESTADO SISTEMA DOCUMENTO

        public  ListaSistemaEstado GetListaEstadoSistema_Ok(string RucEntity)
        {
            return FacturacionServiceController.Instance.GetListaEstadoSistema_Ok(RucEntity);
        }

        public  ListaSistemaEstado GetListaEstadoSistema_Error(string RucEntity)
        {
            return FacturacionServiceController.Instance.GetListaEstadoSistema_Error(RucEntity);
        }
        #endregion



        #region Resumenes

        public  string ProcesaA(string fecha, string RucEmpresa, string TipoRes)
        {
            return new ResumenServiceController().ProcesaA(fecha, RucEmpresa, TipoRes);
        }

        public  string ProcesaB(string fecha, string RucEmpresa, string TipoRes)
        {
            return new ResumenServiceController().ProcesaB(fecha, RucEmpresa, TipoRes);
        }

        public  string ProcesaR(string fecha, string RucEmpresa, string TipoRes)
        {
            return new ResumenServiceController().ProcesaR(fecha, RucEmpresa, TipoRes);
        }

        #endregion


        #region DOCUMENT RECIVED

        public  ListaFacturaElectronica GetListaDocumentReceived(FacturaElectronica oFacturaElectronica)
        {
            return FacturacionServiceController.Instance.GetListaDocumentReceived(oFacturaElectronica);
        }

        #endregion

        #region DOCUMENTO ENVIADO

        public  String InsertarDocumentoEnviado(Documento odocumento)
        {
            return FacturacionServiceController.Instance.InsertarDocumentoEnviado(odocumento);
        }

        public  ListaDocumento GetListaDocumentoEnviado(Documento odocumento)
        {
            return FacturacionServiceController.Instance.GetListaDocumentoEnviado(odocumento);
        }

        #endregion


        #region CATALOGO

        public  String InsertTipoDocumentSend(ListaEstadoEnvio ListSend)
        {
            return FacturacionServiceController.Instance.InsertTipoDocumentSend(ListSend);
        }

        public  String InsertTipoDocumentPrint(ListaEstadoPrint ListPrint)
        {
            return FacturacionServiceController.Instance.InsertTipoDocumentPrint(ListPrint);
        }

        public  String DeleteTipoDocumentSend(ListaEstadoEnvio ListSend)
        {
            return FacturacionServiceController.Instance.DeleteTipoDocumentSend(ListSend);
        }

        public  String DeleteTipoDocumentPrint(ListaEstadoPrint ListPrint)
        {
            return FacturacionServiceController.Instance.DeleteTipoDocumentPrint(ListPrint);
        }

        public  ListaEstadoEnvio GetListEstadoEnvio(int IdEstadoEnvio, string RucEmpresa)
        {
            return FacturacionServiceController.Instance.GetListEstadoEnvio(IdEstadoEnvio, RucEmpresa);
        }

        public  ListaEstadoPrint GetListEstadoPrint(int IdEstadoPrint, string RucEmpresa)
        {
            return FacturacionServiceController.Instance.GetListEstadoPrint(IdEstadoPrint, RucEmpresa);
        }

        #endregion

        #region exchangerate

        public  ListExchangeRate Get_ExchangeRate_Today(DateTime fecha)
        {
            return FacturacionServiceController.Instance.Get_ExchangeRate_Today(fecha);
        }

        #endregion

        #region list pendings document

        public  ListaFacturaElectronica GetList_PendingsDocuments_RA(string ruccomp, string type)
        {
            return FacturacionServiceController.Instance.GetList_PendingsDocuments_RA(ruccomp, type);
        }

        public  ListaFacturaElectronica GetList_PendingsDocuments_RC(string ruccomp)
        {
            return FacturacionServiceController.Instance.GetList_PendingsDocuments_RC(ruccomp);
        }

        public  ListaFacturaElectronica GetList_PendingsDocuments_ErrorSend(string ruccomp)
        {
            return FacturacionServiceController.Instance.GetList_PendingsDocuments_ErrorSend(ruccomp);
        }

        #endregion

        
    }
}