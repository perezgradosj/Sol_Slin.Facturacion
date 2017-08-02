using Slin.Facturacion.Proxies.ServicioConfiguracion;
using Slin.Facturacion.ServiceController;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

//using System.Runtime.Serialization;

namespace Slin.Facturacion.Electronica.Web.ServiceFacturacion
{
    public sealed class ServiceConfiguracionController
    {
        private static readonly ServiceConfiguracionController instance = new ServiceConfiguracionController();
        static ServiceConfiguracionController() { }
        private ServiceConfiguracionController() { }
        public static ServiceConfiguracionController Instance { get { return instance; } }
        

        public ListaURLAmbiente GetListURLAmbienteSunat()
        {
            return ConfiguracionServiceController.Instance.GetListURLAmbienteSunat();
        }

        public ListaDocumentoAmbiente GetListaDocAmbiente(int ID, string RucEntity)
        {
            return ConfiguracionServiceController.Instance.GetListaDocAmbiente(ID, RucEntity);
        }

        public String InsertDocumentAmbiente(ListaDocumentoAmbiente objListDocAmb)
        {
            return ConfiguracionServiceController.Instance.InsertDocumentAmbiente(objListDocAmb);
        }

        public String UpdateDocAmbienteEstado(ListaDocumentoAmbiente objListDocAmb)
        {
            return ConfiguracionServiceController.Instance.UpdateDocAmbienteEstado(objListDocAmb);
        }

        public String UpdateURLAmbiente(ListaURLAmbiente objListUrlAmb)
        {
            return ConfiguracionServiceController.Instance.UpdateURLAmbiente(objListUrlAmb);
        }


        #region CONFIG TIME SERVICE AND EXCHANGE RATE

        public TipoCambio GetexchangeRateToday(string Today, string RucEntity)
        {
            return ConfiguracionServiceController.Instance.GetexchangeRateToday(Today, RucEntity);
        }

        public ListaMoneda GetListMoneda()
        {
            return ConfiguracionServiceController.Instance.GetListMoneda();
        }

        public  ListService GetListTimeService(string RucEntity)
        {
            return ConfiguracionServiceController.Instance.GetListTimeService(RucEntity);
        }

        public String UpdateTimeService(Services oService)
        {
            return ConfiguracionServiceController.Instance.UpdateTimeService(oService);
        }

        public String InsertExchangeRateToday(TipoCambio objRate)
        {
            return ConfiguracionServiceController.Instance.InsertExchangeRateToday(objRate);
        }

        public String UpdateExchangeRateToday(TipoCambio objRate)
        {
            return ConfiguracionServiceController.Instance.UpdateExchangeRateToday(objRate);
        }

        #endregion END CONFIG TIME SERVICE AND EXCHANGE RATE


        #region NEW METHOD FOR CONFIG SEND AND PRINT

        public String InsertTypeDocument_ForSend(TipoDocumento otypedoc)
        {
            return ConfiguracionServiceController.Instance.InsertTypeDocument_ForSend(otypedoc);
        }

        public String InsertTypeDocument_ForPrint(TipoDocumento otypedoc)
        {
            return ConfiguracionServiceController.Instance.InsertTypeDocument_ForPrint(otypedoc);
        }

        public ListaTipoDocumento ListTypeDocument_TypeSend(string RucEntity)
        {
            return ConfiguracionServiceController.Instance.ListTypeDocument_TypeSend(RucEntity);
        }

        public ListaTipoDocumento ListTypeDocument_TypePrint(string RucEntity)
        {
            return ConfiguracionServiceController.Instance.ListTypeDocument_TypePrint(RucEntity);
        }

        #endregion END NEW METHOD FOR CONFIG SEND AND PRINT

        #region SECONDARY USER SUNAT

        public ListSecondaryUser Get_SecondaryUserSunat(int IdAmb, string RucEntity)
        {
            return ConfiguracionServiceController.Instance.Get_SecondaryUserSunat(IdAmb, RucEntity);
        }

        public string Insert_SecondaryUserSunat_Amb(SecondaryUser objSeconUser)
        {
            return ConfiguracionServiceController.Instance.Insert_SecondaryUserSunat_Amb(objSeconUser);
        }

        #endregion


        #region certificate digital

        public ListCertificateDigital Get_CertificateDigitalInformation(string RucEntity)
        {
            return ConfiguracionServiceController.Instance.Get_CertificateDigitalInformation(RucEntity);
        }

        #endregion
    }
}