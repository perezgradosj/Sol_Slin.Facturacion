using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
//using System.ServiceModel;
using System.Text;

using Slin.Facturacion.BusinessEntities;
using Slin.Facturacion.ServiceImplementation;

using WCF = global::System.ServiceModel;

namespace Host.IIS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ServicioFacturacion" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ServicioFacturacion.svc or ServicioFacturacion.svc.cs at the Solution Explorer and start debugging.

    [WCF::ServiceBehavior(Name="ServicioFacturacion",
        Namespace="http://www.slin.com.pe",
        InstanceContextMode=WCF::InstanceContextMode.PerSession,
        ConcurrencyMode = WCF::ConcurrencyMode.Single)]
    public class ServicioFacturacion : IServicioFacturacion
    {
        ServicioFacturacionSOA objMethod = new ServicioFacturacionSOA();

        //Singleton_SI objMethod = new Singleton_SI();

        #region CONSULTAS

        public ListaFacturaElectronica ListarDocumentoCabecera(FacturaElectronica oFacturaElectronica)
        {
            return objMethod.ListarDocumentoCabecera(oFacturaElectronica);
        }

        public ListaDetalleFacturaElectronica ListarDocumentoDetalle(String tipoDocumento, String NumeroSerie, String NumeroDocumento, String RucEmpresa)
        {
            return objMethod.ListarDocumentoDetalle(tipoDocumento, NumeroSerie, NumeroDocumento, RucEmpresa);
        }

        public String GenerarXML(String tipoDocumento, String NumeroSerie, String NumeroDocumento, String RucEmpresa)
        {
            return objMethod.GenerarXML(tipoDocumento, NumeroSerie, NumeroDocumento, RucEmpresa);
        }

        public ListaFacturaElectronica GetListaMontoCab(String tipoDocumento, String NumeroSerie, String NumeroDocumento, String RucEmpresa)
        {
            return objMethod.GetListaMontoCab(tipoDocumento, NumeroSerie, NumeroDocumento, RucEmpresa);
        }
        #endregion


        #region CRE, CPE

        public ListaFacturaElectronica ListarDocumentoCabeceraCRECPE(FacturaElectronica oFacturaElectronica)
        {
            return objMethod.ListarDocumentoCabeceraCRECPE(oFacturaElectronica);
        }

        #endregion


        #region LISTAS

        public ListaEstado ListarEstadoDocumento()
        {
            return objMethod.ListarEstadoDocumento();
        }

        public ListaTipoDocumento ListarTipoDocumento()
        {
            return objMethod.ListarTipoDocumento();
        }

        public ListaSerie ListarSerie(Serie oSerie)
        {
            return objMethod.ListarSerie(oSerie);
        }


        #endregion

        #region DOCUMENTO ANULADO

        public FacturaElectronica GetFechaDocumento(int tpodocumento, String serie, String nrodocumento, String rucempresa)
        {
            return objMethod.GetFechaDocumento(tpodocumento, serie, nrodocumento, rucempresa);
        }

        public ListaFacturaElectronica GetValidarDocumentoExiste(FacturaElectronica oDocAnulado)
        {
            return objMethod.GetValidarDocumentoExiste(oDocAnulado);
        }

        public String InsertarDocumentoAnulado(FacturaElectronica oDocumentoAnulado)
        {
            return objMethod.InsertarDocumentoAnulado(oDocumentoAnulado);
        }

        public ListaDocumento ValidarExisteDocAnulado(FacturaElectronica oDocumentoAnulado)
        {
            return objMethod.ValidarExisteDocAnulado(oDocumentoAnulado);
        }

        public ListaFacturaElectronica GetListaDocumentoAnulado(FacturaElectronica oDocumentoAnulado)
        {
            return objMethod.GetListaDocumentoAnulado(oDocumentoAnulado);
        }


        public FacturaElectronica GetObtenerDocumentoUnico(FacturaElectronica oFacturaElectronica)
        {
            return objMethod.GetObtenerDocumentoUnico(oFacturaElectronica);
        }

        #endregion

        #region UTIL

        public ListaDocumento GetListaDocumentoCabExcel(FacturaElectronica oFactura)
        {
            return objMethod.GetListaDocumentoCabExcel(oFactura);
        }

        #endregion


        #region RESUMEN RC, RA AND RR

        public ListaEstado GetListaTipoFecha()
        {
            return objMethod.GetListaTipoFecha();
        }

        public ListaFacturaElectronica GetListaCabeceraRC(FacturaElectronica documentoRC)
        {
            return objMethod.GetListaCabeceraRC(documentoRC);
        }


        public ListaFacturaElectronica GetListaCabeceraRA(FacturaElectronica documentoRA)
        {
            return objMethod.GetListaCabeceraRA(documentoRA);
        }

        public ListaFacturaElectronica GetListaCabeceraRR(FacturaElectronica documentoRR)
        {
            return objMethod.GetListaCabeceraRR(documentoRR);
        }

        public ListaDetalleFacturaElectronica GetListaDetalleRC(FacturaElectronica documentoRC)
        {
            return objMethod.GetListaDetalleRC(documentoRC);
        }

        public ListaDetalleFacturaElectronica GetListaDetalleRA(FacturaElectronica documentoRA)
        {
            return objMethod.GetListaDetalleRA(documentoRA);
        }

        public ListaDetalleFacturaElectronica GetListaDetalleRR(FacturaElectronica documentoRR)
        {
            return objMethod.GetListaDetalleRR(documentoRR);
        }
        #endregion


        #region ESTADO SISTEMA DOCUMENTO

        public ListaSistemaEstado GetListaEstadoSistema_Ok(string RucEntity)
        {
            return objMethod.GetListaEstadoSistema_Ok(RucEntity);

        }

        public ListaSistemaEstado GetListaEstadoSistema_Error(string RucEntity)
        {
            return objMethod.GetListaEstadoSistema_Error(RucEntity);
        }
        #endregion


        #region CONSULTA PORTAL

        public ListaFacturaElectronica GetListaHistorialCliente(FacturaElectronica oFacturaElectronica)
        {
            return objMethod.GetListaHistorialCliente(oFacturaElectronica);
        }

        public FacturaElectronica GetDocumentoPortalWeb(FacturaElectronica oFacturaElectronica)
        {
            return objMethod.GetDocumentoPortalWeb(oFacturaElectronica);
        }

        #endregion


        #region DOCUMENTO ENVIADO

        public String InsertarDocumentoEnviado(Documento odocumento)
        {
            return objMethod.InsertarDocumentoEnviado(odocumento);
        }

        public ListaDocumento GetListaDocumentoEnviado(Documento odocumento)
        {
            return objMethod.GetListaDocumentoEnviado(odocumento);
        }

        #endregion


        #region DOCUMENT RECIVED

        public ListaFacturaElectronica GetListaDocumentReceived(FacturaElectronica oFacturaElectronica)
        {
            return objMethod.GetListaDocumentReceived(oFacturaElectronica);
        }

        #endregion

        #region CATALOGO

        public String InsertTipoDocumentSend(ListaEstadoEnvio ListSend)
        {
            return objMethod.InsertTipoDocumentSend(ListSend);
        }

        public String InsertTipoDocumentPrint(ListaEstadoPrint ListPrint)
        {
            return objMethod.InsertTipoDocumentPrint(ListPrint);
        }

        public String DeleteTipoDocumentSend(ListaEstadoEnvio ListSend)
        {
            return objMethod.DeleteTipoDocumentSend(ListSend);
        }

        public String DeleteTipoDocumentPrint(ListaEstadoPrint ListPrint)
        {
            return objMethod.DeleteTipoDocumentPrint(ListPrint);
        }

        public ListaEstadoEnvio GetListEstadoEnvio(int IdEstadoEnvio, string RucEmpresa)
        {
            return objMethod.GetListEstadoEnvio(IdEstadoEnvio, RucEmpresa);
        }

        public ListaEstadoPrint GetListEstadoPrint(int IdEstadoPrint, string RucEmpresa)
        {
            return objMethod.GetListEstadoPrint(IdEstadoPrint, RucEmpresa);
        }

        #endregion

        #region exchangerate

        public ListExchangeRate Get_ExchangeRate_Today(DateTime fecha)
        {
            return objMethod.Get_ExchangeRate_Today(fecha);
        }

        #endregion

        #region list pendings document

        public ListaFacturaElectronica GetList_PendingsDocuments_RA(string ruccomp, string type)
        {
            return objMethod.GetList_PendingsDocuments_RA(ruccomp, type);
        }

        public ListaFacturaElectronica GetList_PendingsDocuments_RC(string ruccomp)
        {
            return objMethod.GetList_PendingsDocuments_RC(ruccomp);
        }

        public ListaFacturaElectronica GetList_PendingsDocuments_ErrorSend(string ruccomp)
        {
            return objMethod.GetList_PendingsDocuments_ErrorSend(ruccomp);
        }

        #endregion
    }
}
