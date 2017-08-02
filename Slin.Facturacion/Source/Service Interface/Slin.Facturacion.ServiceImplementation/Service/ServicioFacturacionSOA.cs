using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Slin.Facturacion.BusinessEntities;
using Slin.Facturacion.DataAccess;

namespace Slin.Facturacion.ServiceImplementation
{
    public class ServicioFacturacionSOA
    {
        //private static readonly ServicioFacturacionSOA instance = new ServicioFacturacionSOA();
        //static ServicioFacturacionSOA() { }
        //private ServicioFacturacionSOA() { }
        //public static ServicioFacturacionSOA Instance { get { return instance; } }


        FacturacionDataAccess objFacturacionDataAccess = new FacturacionDataAccess();

        #region CONSULTAS
        public ListaFacturaElectronica ListarDocumentoCabecera(FacturaElectronica oFacturaElectronica)
        {
            return objFacturacionDataAccess.ListarDocumentoCabecera(oFacturaElectronica);
        }

        public ListaDetalleFacturaElectronica ListarDocumentoDetalle(String tipoDocumento, String NumeroSerie, String NumeroDocumento, String RucEmpresa)
        {
            return objFacturacionDataAccess.ListarDocumentoDetalle(tipoDocumento, NumeroSerie, NumeroDocumento, RucEmpresa);
        }

        public String GenerarXML(String tipoDocumento, String NumeroSerie, String NumeroDocumento, String RucEmpresa)
        {
            return objFacturacionDataAccess.GenerarXML(tipoDocumento, NumeroSerie, NumeroDocumento, RucEmpresa);
        }


        public ListaFacturaElectronica GetListaMontoCab(String tipoDocumento, String NumeroSerie, String NumeroDocumento, String RucEmpresa)
        {
            return objFacturacionDataAccess.GetListaMontoCab(tipoDocumento, NumeroSerie, NumeroDocumento, RucEmpresa);
        }

        #endregion

        #region CRE, CPE

        public ListaFacturaElectronica ListarDocumentoCabeceraCRECPE(FacturaElectronica oFacturaElectronica)
        {
            return objFacturacionDataAccess.ListarDocumentoCabeceraCRECPE(oFacturaElectronica);
        }

        #endregion

        #region LISTAS

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


        #endregion

        #region DOCUMENTO ANULADO

        public FacturaElectronica GetFechaDocumento(int tpodocumento, String serie, String nrodocumento, String rucempresa)
        {
            return objFacturacionDataAccess.GetFechaDocumento(tpodocumento, serie, nrodocumento, rucempresa);
        }

        public ListaFacturaElectronica GetValidarDocumentoExiste(FacturaElectronica oDocAnulado)
        {
            return objFacturacionDataAccess.GetValidarDocumentoExiste(oDocAnulado);
        }

        public Documento ValidarExisteDoc_WS(string NUM_CE, string TIPO_CE)
        {
            return objFacturacionDataAccess.ValidarExisteDoc_WS(NUM_CE, TIPO_CE);
        }

        public String InsertarDocumentoAnulado(FacturaElectronica oDocumentoAnulado)
        {
            return objFacturacionDataAccess.InsertarDocumentoAnulado(oDocumentoAnulado);
        }

        public ListaDocumento ValidarExisteDocAnulado(FacturaElectronica oDocumentoAnulado)
        {
            return objFacturacionDataAccess.ValidarExisteDocAnulado(oDocumentoAnulado);
        }

        public ListaFacturaElectronica GetListaDocumentoAnulado(FacturaElectronica oDocumentoAnulado)
        {
            return objFacturacionDataAccess.GetListaDocumentoAnulado(oDocumentoAnulado);
        }




        #endregion

        #region UTIL

        public ListaDocumento GetListaDocumentoCabExcel(FacturaElectronica oFactura)
        {
            return objFacturacionDataAccess.GetListaDocumentoCabExcel(oFactura);
        }


        public FacturaElectronica GetObtenerDocumentoUnico(FacturaElectronica oFacturaElectronica)
        {
            return objFacturacionDataAccess.GetObtenerDocumentoUnico(oFacturaElectronica);
        }

        #endregion

        #region RESUMEN RC AND RC

        public ListaEstado GetListaTipoFecha()
        {
            return objFacturacionDataAccess.GetListaTipoFecha();
        }

        public ListaFacturaElectronica GetListaCabeceraRC(FacturaElectronica documentoRC)
        {
            return objFacturacionDataAccess.GetListaCabeceraRC(documentoRC);
        }


        public ListaFacturaElectronica GetListaCabeceraRA(FacturaElectronica documentoRA)
        {
            return objFacturacionDataAccess.GetListaCabeceraRA(documentoRA);
        }

        public ListaFacturaElectronica GetListaCabeceraRR(FacturaElectronica documentoRR)
        {
            return objFacturacionDataAccess.GetListaCabeceraRR(documentoRR);
        }

        public ListaDetalleFacturaElectronica GetListaDetalleRC(FacturaElectronica documentoRC)
        {
            return objFacturacionDataAccess.GetListaDetalleRC(documentoRC);
        }

        public ListaDetalleFacturaElectronica GetListaDetalleRA(FacturaElectronica documentoRA)
        {
            return objFacturacionDataAccess.GetListaDetalleRA(documentoRA);
        }

        public ListaDetalleFacturaElectronica GetListaDetalleRR(FacturaElectronica documentoRR)
        {
            return objFacturacionDataAccess.GetListaDetalleRR(documentoRR);
        }
        #endregion

        #region ESTADO SISTEMA DOCUMENTO

        public ListaSistemaEstado GetListaEstadoSistema_Ok(string RucEntity)
        {
            return objFacturacionDataAccess.GetListaEstadoSistema_Ok(RucEntity);
            
        }

        public ListaSistemaEstado GetListaEstadoSistema_Error(string RucEntity)
        {
            return objFacturacionDataAccess.GetListaEstadoSistema_Error(RucEntity);
        }
        #endregion

        #region CONSULTA PORTAL

        public ListaFacturaElectronica GetListaHistorialCliente(FacturaElectronica oFacturaElectronica)
        {
            return objFacturacionDataAccess.GetListaHistorialCliente(oFacturaElectronica);
        }

        public FacturaElectronica GetDocumentoPortalWeb(FacturaElectronica oFacturaElectronica)
        {
            return objFacturacionDataAccess.GetDocumentoPortalWeb(oFacturaElectronica);
        }

        #endregion

        #region DOCUMENTO ENVIADO

        public String InsertarDocumentoEnviado(Documento odocumento)
        {
            return objFacturacionDataAccess.InsertarDocumentoEnviado(odocumento);
        }

        public ListaDocumento GetListaDocumentoEnviado(Documento odocumento)
        {
            return objFacturacionDataAccess.GetListaDocumentoEnviado(odocumento);
        }

        public String InsertarListDocEnviado(List<Documento> listDocSend)
        {
            return objFacturacionDataAccess.InsertarListDocEnviado(listDocSend);
        }

        #endregion

        #region DOCUMENT XML RECEIVED

        public ListaDocumento GetListaIfExistsDocumentoCabecera(string NUM_CPE)
        {
            return objFacturacionDataAccess.GetListaIfExistsDocumentoCabecera(NUM_CPE);
        }

        public int InsertarDocumentoCabecera_Rec(FacturaElectronica oFactura, byte[] XmlVarBinary)
        {
            return objFacturacionDataAccess.InsertarDocumentoCabecera_Rec(oFactura, XmlVarBinary);
        }


        //public string InsertarDocumentoDetalle_Rec(DetalleFacturaElectronica odetalle)
        //{
        //    return objFacturacionDataAccess.InsertarDocumentoDetalle_Rec(odetalle);
        //}

        public string InsertarDocumentoDetalle_Rec(ListaDetalleFacturaElectronica olistadetalle, int ID_DC)
        {
            return objFacturacionDataAccess.InsertarDocumentoDetalle_Rec(olistadetalle, ID_DC);
        }

        public string InsertarDocumentoDetalle_CRE_CPE(ListaDocCRECPE olistadetalle, int ID_DC)
        {
            return objFacturacionDataAccess.InsertarDocumentoDetalle_CRE_CPE(olistadetalle, ID_DC);
        }

        public ListaFacturaElectronica GetListaDocumentReceived(FacturaElectronica oFacturaElectronica)
        {
            return objFacturacionDataAccess.GetListaDocumentReceived(oFacturaElectronica);
        }


        #endregion END DOCUMENT XML RECEIVED

        #region SEND MAIL

        public ListaFacturaElectronica GetListDocumentoSendMail()
        {
            return objFacturacionDataAccess.GetListDocumentoSendMail();
        }

        public ListaFacturaElectronica GetListDocumentoSendMail_Parameter(string NUM_CE)
        {
            return objFacturacionDataAccess.GetListDocumentoSendMail_Parameter(NUM_CE);
        }

        public Cliente GetEmailClient(string NroDocumento)
        {
            return objFacturacionDataAccess.GetEmailClient(NroDocumento);
        }

        public string UpdateDocCabStatuSend(ListaFacturaElectronica olistaUpdate)
        {
            return objFacturacionDataAccess.UpdateDocCabStatuSend(olistaUpdate);
        }















        public ListaFacturaElectronica GetListDocumentoPrint_Parameter(string NUM_CE)
        {
            return objFacturacionDataAccess.GetListDocumentoPrint_Parameter(NUM_CE);
        }

        public string UpdateDocCabStatusPrint(ListaFacturaElectronica olistaUpdate)
        {
            return objFacturacionDataAccess.UpdateDocCabStatusPrint(olistaUpdate);
        }

        #endregion

        #region CATALOGO

        public String InsertTipoDocumentSend(ListaEstadoEnvio ListSend)
        {
            return objFacturacionDataAccess.InsertTipoDocumentSend(ListSend);
        }

        public String InsertTipoDocumentPrint(ListaEstadoPrint ListPrint)
        {
            return objFacturacionDataAccess.InsertTipoDocumentPrint(ListPrint);
        }

        public String DeleteTipoDocumentSend(ListaEstadoEnvio ListSend)
        {
            return objFacturacionDataAccess.DeleteTipoDocumentSend(ListSend);
        }

        public String DeleteTipoDocumentPrint(ListaEstadoPrint ListPrint)
        {
            return objFacturacionDataAccess.DeleteTipoDocumentPrint(ListPrint);
        }

        public ListaEstadoEnvio GetListEstadoEnvio(int IdEstadoEnvio, string RucEmpresa)
        {
            return objFacturacionDataAccess.GetListEstadoEnvio(IdEstadoEnvio, RucEmpresa);
        }

        public ListaEstadoPrint GetListEstadoPrint(int IdEstadoPrint, string RucEmpresa)
        {
            return objFacturacionDataAccess.GetListEstadoPrint(IdEstadoPrint, RucEmpresa);
        }

        #endregion

        #region exchangerate

        public ListExchangeRate Get_ExchangeRate_Today(DateTime fecha)
        {
            return objFacturacionDataAccess.Get_ExchangeRate_Today(fecha);
        }

        #endregion

        #region Exchange Rate

        public string Insert_ExchangeRate_Value(ExchangeRate objtype)
        {
            return objFacturacionDataAccess.Insert_ExchangeRate_Value(objtype);
        }

        #endregion

        #region list pendings document

        public ListaFacturaElectronica GetList_PendingsDocuments_RA(string ruccomp, string type)
        {
            return objFacturacionDataAccess.GetList_PendingsDocuments_RA(ruccomp, type);
        }


        public ListaFacturaElectronica GetList_PendingsDocuments_RC(string ruccomp)
        {
            return objFacturacionDataAccess.GetList_PendingsDocuments_RC(ruccomp);
        }

        public ListaFacturaElectronica GetList_PendingsDocuments_ErrorSend(string ruccomp)
        {
            return objFacturacionDataAccess.GetList_PendingsDocuments_ErrorSend(ruccomp);
        }

        #endregion
    }
}
