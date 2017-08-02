using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Slin.Facturacion.BusinessEntities;

namespace Host.IIS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IServicioFacturacion" in both code and config file together.
    [ServiceContract]
    public interface IServicioFacturacion
    {
        #region CONSULTAS

        [OperationContract]
        ListaFacturaElectronica ListarDocumentoCabecera(FacturaElectronica oFacturaElectronica);

        [OperationContract]
        ListaDetalleFacturaElectronica ListarDocumentoDetalle(String tipoDocumento, String NumeroSerie, String NumeroDocumento, String RucEmpresa);

        [OperationContract]
        String GenerarXML(String tipoDocumento, String NumeroSerie, String NumeroDocumento, String RucEmpresa);

        [OperationContract]
        ListaFacturaElectronica GetListaMontoCab(String tipoDocumento, String NumeroSerie, String NumeroDocumento, String RucEmpresa);

        [OperationContract]
        FacturaElectronica GetObtenerDocumentoUnico(FacturaElectronica oFacturaElectronica);

        #endregion

        #region CRE, CPE

        [OperationContract]
        ListaFacturaElectronica ListarDocumentoCabeceraCRECPE(FacturaElectronica oFacturaElectronica);

        #endregion

        #region LISTAS

        [OperationContract]
        ListaEstado ListarEstadoDocumento();

        [OperationContract]
        ListaTipoDocumento ListarTipoDocumento();

        [OperationContract]
        ListaSerie ListarSerie(Serie oSerie);

        #endregion


        #region DOCUMENTO ANULADO

        [OperationContract]
        FacturaElectronica GetFechaDocumento(int tpodocumento, String serie, String nrodocumento, String rucempresa);

        [OperationContract]
        ListaFacturaElectronica GetValidarDocumentoExiste(FacturaElectronica oDocAnulado);

        [OperationContract]
        String InsertarDocumentoAnulado(FacturaElectronica oDocumentoAnulado);

        [OperationContract]
        ListaDocumento ValidarExisteDocAnulado(FacturaElectronica oDocumentoAnulado);

        [OperationContract]
        ListaFacturaElectronica GetListaDocumentoAnulado(FacturaElectronica oDocumentoAnulado);

        #endregion

        #region UTIL
        [OperationContract]
        ListaDocumento GetListaDocumentoCabExcel(FacturaElectronica oFactura);

        #endregion

        #region RESUMEN RC AND RA

        [OperationContract]
        ListaEstado GetListaTipoFecha();

        [OperationContract]
        ListaFacturaElectronica GetListaCabeceraRC(FacturaElectronica documentoRC);

        [OperationContract]
        ListaFacturaElectronica GetListaCabeceraRA(FacturaElectronica documentoRA);

        [OperationContract]
        ListaFacturaElectronica GetListaCabeceraRR(FacturaElectronica documentoRR);

        [OperationContract]
        ListaDetalleFacturaElectronica GetListaDetalleRC(FacturaElectronica documentoRC);

        [OperationContract]
        ListaDetalleFacturaElectronica GetListaDetalleRA(FacturaElectronica documentoRA);

        [OperationContract]
        ListaDetalleFacturaElectronica GetListaDetalleRR(FacturaElectronica documentoRR);

        #endregion

        #region ESTADO SISTEMA DOCUMENTO

        [OperationContract]
        ListaSistemaEstado GetListaEstadoSistema_Ok(string RucEntity);

        [OperationContract]
        ListaSistemaEstado GetListaEstadoSistema_Error(string RucEntity);

        #endregion


        #region CONSULTA PORTAL

        [OperationContract]
        ListaFacturaElectronica GetListaHistorialCliente(FacturaElectronica oFacturaElectronica);

        [OperationContract]
        FacturaElectronica GetDocumentoPortalWeb(FacturaElectronica oFacturaElectronica);

        #endregion


        #region DOCUMENT RECEIVED
        [OperationContract]
        ListaFacturaElectronica GetListaDocumentReceived(FacturaElectronica oFacturaElectronica);

        #endregion

        #region DOCUMENTO ENVIADO

        [OperationContract]
        String InsertarDocumentoEnviado(Documento odocumento);

        [OperationContract]
        ListaDocumento GetListaDocumentoEnviado(Documento odocumento);

        #endregion

        //#region CHANGE CAMPO XML

        //[OperationContract]
        //String GetCampoXmlResumenBajas(Int32 IdRac);


        //#endregion

        #region CATALOGO

        [OperationContract]
        String InsertTipoDocumentSend(ListaEstadoEnvio ListSend);

        [OperationContract]
        String InsertTipoDocumentPrint(ListaEstadoPrint ListPrint);

        [OperationContract]
        String DeleteTipoDocumentSend(ListaEstadoEnvio ListSend);

        [OperationContract]
        String DeleteTipoDocumentPrint(ListaEstadoPrint ListPrint);

        [OperationContract]
        ListaEstadoEnvio GetListEstadoEnvio(int IdEstadoEnvio, string RucEmpresa);

        [OperationContract]
        ListaEstadoPrint GetListEstadoPrint(int IdEstadoPrint, string RucEmpresa);

        #endregion

        #region exchangerate
        [OperationContract]
        ListExchangeRate Get_ExchangeRate_Today(DateTime fecha);
        #endregion


        #region list pendings document

        [OperationContract]
        ListaFacturaElectronica GetList_PendingsDocuments_RA(string ruccomp, string type);

        [OperationContract]
        ListaFacturaElectronica GetList_PendingsDocuments_RC(string ruccomp);

        [OperationContract]
        ListaFacturaElectronica GetList_PendingsDocuments_ErrorSend(string ruccomp);

        #endregion
    }
}
