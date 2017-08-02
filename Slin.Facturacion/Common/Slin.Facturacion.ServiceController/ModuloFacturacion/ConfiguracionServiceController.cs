using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Slin.Facturacion.Proxies.ServicioConfiguracion;

namespace Slin.Facturacion.ServiceController
{
    public sealed class ConfiguracionServiceController
    {
        private static readonly ConfiguracionServiceController instance = new ConfiguracionServiceController();
        static ConfiguracionServiceController() { }
        private ConfiguracionServiceController() { }
        public static ConfiguracionServiceController Instance { get { return instance; } }


        #region AMBIENTE

        public ListaURLAmbiente GetListURLAmbienteSunat()
        {
            using (ServicioConfiguracionClient Client = new ServicioConfiguracionClient())
            {
                return Client.GetListURLAmbienteSunat();
            }
        }

        public ListaDocumentoAmbiente GetListaDocAmbiente(int ID, string RucEntity)
        {
            using (ServicioConfiguracionClient Client = new ServicioConfiguracionClient())
            {
                return Client.GetListaDocAmbiente(ID, RucEntity);
            }
        }

        public String InsertDocumentAmbiente(ListaDocumentoAmbiente objListDocAmb)
        {
            using (ServicioConfiguracionClient Client = new ServicioConfiguracionClient())
            {
                return Client.InsertDocumentAmbiente(objListDocAmb);
            }
        }

        public String UpdateDocAmbienteEstado(ListaDocumentoAmbiente objListDocAmb)
        {
            using (ServicioConfiguracionClient Client = new ServicioConfiguracionClient())
            {
                return Client.UpdateDocAmbienteEstado(objListDocAmb);
            }
        }

        public String UpdateURLAmbiente(ListaURLAmbiente objListUrlAmb)
        {
            using (ServicioConfiguracionClient Client = new ServicioConfiguracionClient())
            {
                return Client.UpdateURLAmbiente(objListUrlAmb);
            }
        }

        #endregion

        #region CONFIG TIME SERVICE AND EXCHANGE RATE

        public TipoCambio GetexchangeRateToday(string Today, string RucEntity)
        {
            using (ServicioConfiguracionClient Client = new ServicioConfiguracionClient())
            {
                return Client.GetexchangeRateToday(Today, RucEntity);
            }
        }

        public ListaMoneda GetListMoneda()
        {
            using (ServicioConfiguracionClient Client = new ServicioConfiguracionClient())
            {
                return Client.GetListMoneda();
            }
        }

        public ListService GetListTimeService(string RucEntity)
        {
            using (ServicioConfiguracionClient Client = new ServicioConfiguracionClient())
            {
                return Client.GetListTimeService(RucEntity);
            }
        }

        public String UpdateTimeService(Services oService)
        {
            using (ServicioConfiguracionClient Client = new ServicioConfiguracionClient())
            {
                return Client.UpdateTimeService(oService);
            }
        }

        public String InsertExchangeRateToday(TipoCambio objRate)
        {
            using (ServicioConfiguracionClient Client = new ServicioConfiguracionClient())
            {
                return Client.InsertExchangeRateToday(objRate);
            }
        }

        public String UpdateExchangeRateToday(TipoCambio objRate)
        {
            using (ServicioConfiguracionClient Client = new ServicioConfiguracionClient())
            {
                return Client.UpdateExchangeRateToday(objRate);
            }
        }

        //public System.Data.DataTable GetListTypeDocumentRowSend(int id)
        //{
        //    using (ServicioConfiguracionClient Client = new ServicioConfiguracionClient())
        //    {
        //        return Client.GetListTypeDocumentRowSend(id);
        //    }
        //}

        #endregion END CONFIG TIME SERVICE AND EXCHANGE RATE

        #region NEW METHOD FOR CONFIG SEND AND PRINT

        public String InsertTypeDocument_ForSend(TipoDocumento otypedoc)
        {
            using (ServicioConfiguracionClient Client = new ServicioConfiguracionClient())
            {
                return Client.InsertTypeDocument_ForSend(otypedoc);
            }
        }


        public String InsertTypeDocument_ForPrint(TipoDocumento otypedoc)
        {
            using (ServicioConfiguracionClient Client = new ServicioConfiguracionClient())
            {
                return Client.InsertTypeDocument_ForPrint(otypedoc);
            }
        }

        public ListaTipoDocumento ListTypeDocument_TypeSend(string RucEntity)
        {
            using (ServicioConfiguracionClient Client = new ServicioConfiguracionClient())
            {
                return Client.ListTypeDocument_TypeSend(RucEntity);
            }
        }

        public ListaTipoDocumento ListTypeDocument_TypePrint(string RucEntity)
        {
            using (ServicioConfiguracionClient Client = new ServicioConfiguracionClient())
            {
                return Client.ListTypeDocument_TypePrint(RucEntity);
            }
        }

        #endregion END NEW METHOD FOR CONFIG SEND AND PRINT

        #region SECONDARY USER SUNAT

        public ListSecondaryUser Get_SecondaryUserSunat(int IdAmb, string RucEntity)
        {
            using (ServicioConfiguracionClient Client = new ServicioConfiguracionClient())
            {
                return Client.Get_SecondaryUserSunat(IdAmb, RucEntity);
            }
        }

        public string Insert_SecondaryUserSunat_Amb(SecondaryUser objSeconUser)
        {
            using (ServicioConfiguracionClient Client = new ServicioConfiguracionClient())
            {
                return Client.Insert_SecondaryUserSunat_Amb(objSeconUser);
            }
        }

        #endregion

        #region certificate digital

        public ListCertificateDigital Get_CertificateDigitalInformation(string RucEntity)
        {
            using (ServicioConfiguracionClient Client = new ServicioConfiguracionClient())
            {
                return Client.Get_CertificateDigitalInformation(RucEntity);
            }
        }

        #endregion
    }
}
