using Slin.Facturacion.BusinessEntities;
using Slin.Facturacion.BusinessEntities.Configuracion;
using Slin.Facturacion.DataAccess;
using System;
using System.Collections.Generic;
//using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slin.Facturacion.ServiceImplementation
{
    public class ServicioConfiguracionSOA
    {
        //private static readonly ServicioConfiguracionSOA instance = new ServicioConfiguracionSOA();
        //static ServicioConfiguracionSOA() { }
        //private ServicioConfiguracionSOA() { }
        //public static ServicioConfiguracionSOA Instance { get { return instance; } }

        ConfiguracionDataAccess objConfiguracionDataAccess = new ConfiguracionDataAccess();


        #region ambiente

        public ListaURLAmbiente GetListURLAmbienteSunat()
        {
            return objConfiguracionDataAccess.GetListURLAmbienteSunat();
        }

        public ListaDocumentoAmbiente GetListaDocAmbiente(int ID, string RucEntity)
        {
            return objConfiguracionDataAccess.GetListaDocAmbiente(ID, RucEntity);
        }

        public String InsertDocumentAmbiente(ListaDocumentoAmbiente objListDocAmb)
        {
            return objConfiguracionDataAccess.InsertDocumentAmbiente(objListDocAmb);
        }

        public String UpdateDocAmbienteEstado(ListaDocumentoAmbiente objListDocAmb)
        {
            return objConfiguracionDataAccess.UpdateDocAmbienteEstado(objListDocAmb);
        }

        public String UpdateURLAmbiente(ListaURLAmbiente objListUrlAmb)
        {
            return objConfiguracionDataAccess.UpdateURLAmbiente(objListUrlAmb);
        }

        public List<string> Delete_DocAmb(ListaDocumentoAmbiente objListDocAmb)
        {
            return objConfiguracionDataAccess.Delete_DocAmb(objListDocAmb);
        }

        #endregion

        #region CONFIG TIME SERVICE AND EXCHANGE RATE

        public TipoCambio GetexchangeRateToday(string Today, string RucEntity)
        {
            return objConfiguracionDataAccess.GetexchangeRateToday(Today, RucEntity);
        }

        public ListaMoneda GetListMoneda()
        {
            return objConfiguracionDataAccess.GetListMoneda();
        }

        public ListService GetListTimeService(string RucEntity)
        {
            return objConfiguracionDataAccess.GetListTimeService(RucEntity);
        }

        public String UpdateTimeService(Services oService)
        {
            return objConfiguracionDataAccess.UpdateTimeService(oService);
        }

        public String InsertExchangeRateToday(TipoCambio objRate)
        {
            return objConfiguracionDataAccess.InsertExchangeRateToday(objRate);
        }

        public String UpdateExchangeRateToday(TipoCambio objRate)
        {
            return objConfiguracionDataAccess.UpdateExchangeRateToday(objRate);
        }



        public System.Data.DataTable GetListTypeDocumentRowSend(int id)
        {
            return objConfiguracionDataAccess.GetListTypeDocumentRowSend(id);
        }

        #endregion END CONFIG TIME SERVICE AND EXCHANGE RATE



        #region NEW METHOD FOR CONFIG SEND AND PRINT

        public String InsertTypeDocument_ForSend(TipoDocumento otypedoc)
        {
            return objConfiguracionDataAccess.InsertTypeDocument_ForSend(otypedoc);
        }


        public String InsertTypeDocument_ForPrint(TipoDocumento otypedoc)
        {
            return objConfiguracionDataAccess.InsertTypeDocument_ForPrint(otypedoc);
        }

        public ListaTipoDocumento ListTypeDocument_TypeSend(string RucEntity)
        {
            return objConfiguracionDataAccess.ListTypeDocument_TypeSend(RucEntity);
        }

        public ListaTipoDocumento ListTypeDocument_TypePrint(string RucEntity)
        {
            return objConfiguracionDataAccess.ListTypeDocument_TypePrint(RucEntity);
        }

        #endregion END NEW METHOD FOR CONFIG SEND AND PRINT



        #region SERVICE

        public ListService GetTimeServicceForExeProccessMD(string nameService)
        {
            return objConfiguracionDataAccess.GetTimeServicceForExeProccessMD(nameService);
        }

        #endregion

        #region SECONDARY USER SUNAT

        public ListSecondaryUser Get_SecondaryUserSunat(int IdAmb, string RucEntity)
        {
            return objConfiguracionDataAccess.Get_SecondaryUserSunat(IdAmb, RucEntity);
        }

        public string Insert_SecondaryUserSunat_Amb(SecondaryUser objSeconUser)
        {
            return objConfiguracionDataAccess.Insert_SecondaryUserSunat_Amb(objSeconUser);
        }

        #endregion

        #region certificate digital

        public ListCertificateDigital Get_CertificateDigitalInformation(string RucEntity)
        {
            return objConfiguracionDataAccess.Get_CertificateDigitalInformation(RucEntity);
        }

        #endregion
    }
}
