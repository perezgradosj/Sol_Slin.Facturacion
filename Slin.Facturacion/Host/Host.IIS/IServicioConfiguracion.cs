using Slin.Facturacion.BusinessEntities;
using Slin.Facturacion.BusinessEntities.Configuracion;
using System;
using System.Collections.Generic;
//using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Host.IIS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IServicioConfiguracion" in both code and config file together.
    [ServiceContract]
    public interface IServicioConfiguracion
    {

        [OperationContract]
        ListaURLAmbiente GetListURLAmbienteSunat();

        [OperationContract]
        ListaDocumentoAmbiente GetListaDocAmbiente(int ID, string RucEntity);

        //[OperationContract]
        //AmbienteSunat GetAmbienteTrabjActual();

        [OperationContract]
        String InsertDocumentAmbiente(ListaDocumentoAmbiente objListDocAmb);

        [OperationContract]
        String UpdateDocAmbienteEstado(ListaDocumentoAmbiente objListDocAmb);

        [OperationContract]
        String UpdateURLAmbiente(ListaURLAmbiente objListUrlAmb);

        List<string> Delete_DocAmb(ListaDocumentoAmbiente objListDocAmb);

        #region CONFIG TIME SERVICE AND EXCHANGE RATE

        [OperationContract]
        TipoCambio GetexchangeRateToday(string Today, string RucEntity);

        [OperationContract]
        ListaMoneda GetListMoneda();

        [OperationContract]
        ListService GetListTimeService(string RucEntity);

        [OperationContract]
        String UpdateTimeService(Services oService);

        [OperationContract]
        String InsertExchangeRateToday(TipoCambio objRate);

        [OperationContract]
        String UpdateExchangeRateToday(TipoCambio objRate);

        //[OperationContract]
        //System.Data.DataTable GetListTypeDocumentRowSend(int id);

        #endregion END CONFIG TIME SERVICE AND EXCHANGE RATE


        #region NEW METHOD FOR CONFIG SEND AND PRINT

        [OperationContract]
        String InsertTypeDocument_ForSend(TipoDocumento otypedoc);

        [OperationContract]
        String InsertTypeDocument_ForPrint(TipoDocumento otypedoc);

        [OperationContract]
        ListaTipoDocumento ListTypeDocument_TypeSend(string RucEntity);

        [OperationContract]
        ListaTipoDocumento ListTypeDocument_TypePrint(string RucEntity);

        #endregion END NEW METHOD FOR CONFIG SEND AND PRINT

        #region SECONDARY USER SUNAT

        [OperationContract]
        ListSecondaryUser Get_SecondaryUserSunat(int IdAmb, string RucEntity);

        [OperationContract]
        string Insert_SecondaryUserSunat_Amb(SecondaryUser objSeconUser);

        #endregion

        #region certificate digital

        [OperationContract]
        ListCertificateDigital Get_CertificateDigitalInformation(string RucEntity);

        #endregion
    }
}
