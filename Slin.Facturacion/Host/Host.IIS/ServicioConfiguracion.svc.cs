using Slin.Facturacion.BusinessEntities;
using Slin.Facturacion.BusinessEntities.Configuracion;
using Slin.Facturacion.ServiceImplementation;
using System;
using System.Collections.Generic;
//using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Host.IIS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ServicioConfiguracion" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ServicioConfiguracion.svc or ServicioConfiguracion.svc.cs at the Solution Explorer and start debugging.
    public class ServicioConfiguracion : IServicioConfiguracion
    {

        ServicioConfiguracionSOA objMethod = new ServicioConfiguracionSOA();


        #region AMBIENTE

        public ListaURLAmbiente GetListURLAmbienteSunat()
        {
            return objMethod.GetListURLAmbienteSunat();
        }

        public ListaDocumentoAmbiente GetListaDocAmbiente(int ID, string RucEntity)
        {
            return objMethod.GetListaDocAmbiente(ID, RucEntity);
        }

        public String InsertDocumentAmbiente(ListaDocumentoAmbiente objListDocAmb)
        {
            return objMethod.InsertDocumentAmbiente(objListDocAmb);
        }

        public String UpdateDocAmbienteEstado(ListaDocumentoAmbiente objListDocAmb)
        {
            return objMethod.UpdateDocAmbienteEstado(objListDocAmb);
        }

        public String UpdateURLAmbiente(ListaURLAmbiente objListUrlAmb)
        {
            return objMethod.UpdateURLAmbiente(objListUrlAmb);
        }

        public List<string> Delete_DocAmb(ListaDocumentoAmbiente objListDocAmb)
        {
            return objMethod.Delete_DocAmb(objListDocAmb);
        }

        #endregion

        #region CONFIG TIME SERVICE AND EXCHANGE RATE

        public TipoCambio GetexchangeRateToday(string Today, string RucEntity)
        {
            return objMethod.GetexchangeRateToday(Today, RucEntity);
        }

        public ListaMoneda GetListMoneda()
        {
            return objMethod.GetListMoneda();
        }

        public ListService GetListTimeService(string RucEntity)
        {
            return objMethod.GetListTimeService(RucEntity);
        }

        public String UpdateTimeService(Services oService)
        {
            return objMethod.UpdateTimeService(oService);
        }

        public String InsertExchangeRateToday(TipoCambio objRate)
        {
            return objMethod.InsertExchangeRateToday(objRate);
        }

        public String UpdateExchangeRateToday(TipoCambio objRate)
        {
            return objMethod.UpdateExchangeRateToday(objRate);
        }



        public System.Data.DataTable GetListTypeDocumentRowSend(int id)
        {
            return objMethod.GetListTypeDocumentRowSend(id);
        }

        #endregion END CONFIG TIME SERVICE AND EXCHANGE RATE

        #region NEW METHOD FOR CONFIG SEND AND PRINT

        public String InsertTypeDocument_ForSend(TipoDocumento otypedoc)
        {
            return objMethod.InsertTypeDocument_ForSend(otypedoc);
        }

        public String InsertTypeDocument_ForPrint(TipoDocumento otypedoc)
        {
            return objMethod.InsertTypeDocument_ForPrint(otypedoc);
        }

        public ListaTipoDocumento ListTypeDocument_TypeSend(string RucEntity)
        {
            return objMethod.ListTypeDocument_TypeSend(RucEntity);
        }

        public ListaTipoDocumento ListTypeDocument_TypePrint(string RucEntity)
        {
            return objMethod.ListTypeDocument_TypePrint(RucEntity);
        }

        #endregion END NEW METHOD FOR CONFIG SEND AND PRINT

        #region SERVICE

        public ListService GetTimeServicceForExeProccessMD(string nameService)
        {
            return objMethod.GetTimeServicceForExeProccessMD(nameService);
        }

        #endregion

        #region SECONDARY USER SUNAT

        public ListSecondaryUser Get_SecondaryUserSunat(int IdAmb, string RucEntity)
        {
            return objMethod.Get_SecondaryUserSunat(IdAmb, RucEntity);
        }

        public string Insert_SecondaryUserSunat_Amb(SecondaryUser objSeconUser)
        {
            return objMethod.Insert_SecondaryUserSunat_Amb(objSeconUser);
        }

        #endregion

        #region CERTIFICADO DIGITAL

        public ListCertificateDigital Get_CertificateDigitalInformation(string RucEntity)
        {
            return objMethod.Get_CertificateDigitalInformation(RucEntity);
        }

        #endregion
    }
}
