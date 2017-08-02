using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slin.Facturacion.DataAccess.Helper
{
    public class Procedimientos
    {
        #region FACTURACION

        
        public const string Usp_ListaDocumentoCabecera = "[Fact].[O.Usp_ListaDocumentoCabecera]";
        public const string Usp_ListaDocumentoDetalle = "[Fact].[O.Usp_ListaDocumentoDetalle]";
        public const string Usp_ListaTipoDocumento = "[Ctl].[Usp_ListaTipoDocumento]";
        public const string Usp_ListaEstadoDocumento = "[Fact].[O.Usp_ListaEstadoDocumento]";
        public const string Usp_ListaSerie = "[Fact].[O.Usp_ListaSerie]";



        public const string Usp_ListaDocumentoCabeceraCRECPE = "[Fact].[O.Usp_ListaDocumentoCabeceraCRECPE]";



        public const string Usp_GenerarXML = "[Fact].[O.Usp_GenerarXml]";

        public const string Usp_ListaMontosCab = "[Fact].[O.Usp_ListaMontosCab]";


        public const string Usp_ObtenerFechaDocumento = "[Fact].[O.Usp_ObtenerFechaDocumento]";

        public const string Usp_ValidarDocumentoExiste = "[Fact].[Usp_ValidarDocumentoExiste]";


        public const string Usp_InsertarDocumentoAnulado = "[Fact].[O.Usp_InsertaDocumentoAnulado]";
        public const string Usp_ListaDocumentoAnulado = "[Fact].[O.Usp_ListaDocumentoAnulado]";

        public const string Usp_ExisteDocAnulado = "[Fact].[Usp_ExisteDocAnualdo]";
        public const string Usp_ExisteDocWS = "[Fact].[Usp_ExisteDocWS]";

        public const string Usp_ObtenerDocumentoUnico = "[Fact].[O.Usp_ObtenerDocumentoUnico]";

        //public const string Usp_ListaResumentoDocumento = "[Fact].[Usp_ListaResumenDocumento]";


        public const string Usp_EstadoSistemaOK = "[Fact].[Usp_EstadoSistema_Ok]";
        public const string Usp_EstadoSistemaError = "[Fact].[Usp_EstadoSistema_Error]";

        public const string Usp_IfExistsDocument = "[Fact].[Usp_IfExistsDocument]";
        public const string Usp_InsertarDocumentoEnviado = "[Fact].[Usp_InsertarDocumentoEnviado]";
        public const string Usp_ListaDocumentoEnviado = "[Fact].[Usp_ListaDocumentoEnviado]";


        public const string Usp_VerificarStatusDocxSend = "[Fact].[Usp_VerificarStatusDocxSend]";

        public const string Usp_VerificarStatusDocxSend_Parameter = "[Fact].[Usp_VerificarStatusDocxSend_Parameter]";

        public const string Usp_VerificarStatusDocxPrint_Parameter = "[Fact].[Usp_VerificarStatusDocxPrint_Parameter]";
        #endregion

        #region Resumen RC

        public const string Usp_ListaCabeceraRC = "[Fact].[O.R.Usp_ListaCabeceraRC]";
        public const string Usp_ListaDetalleRC = "[Fact].[O.Usp_GetListaDetalleRC]";

        public const string Usp_ListaCabeceraRA = "[Fact].[O.R.Usp_ListaCabeceraRA]";
        public const string Usp_ListaDetalleRA = "[Fact].[O.Usp_GetListaDetalleRA]";

        public const string Usp_ListaCabeceraRR = "[Fact].[O.R.Usp_ListaCabeceraRR]";
        public const string Usp_ListaDetalleRR = "[Fact].[O.Usp_GetListaDetalleRR]";

        public const string Usp_GenerarXMLRC = "[Fact].[O.Usp_GenerarXmlRC]";
        #endregion

        #region UTIL




        public const string Usp_ListaDocumentoCabExcel = "[Fact].[O.Usp_ListaDocumentoCabExcel]";


        #endregion


        #region SEGURIDAD

        public const string Usp_IniciarSesion = "[Seg].[Usp_IniciarSesion]";

        public const string Usp_ActualizarContrasenia = "[Seg].[Usp_ActualizarContrasenia]";

        public const string Usp_ListaUsuario = "[Seg].[Usp_ListaUsuario]";
        public const string Usp_ValidarUsername = "[Seg].[Usp_ValidarUsername]";

        public const string Usp_ObtenerDatosxDNI = "[Seg].[Usp_ObtenerDatosxDNI]";

        
        public const string Usp_InsertarUsuario = "[Seg].[Usp_InsertarUsuarios]";
        public const string Usp_InsertarUsuario_Excel = "[Seg].[Usp_InsertarUsuarios_Excel]";
        public const string Usp_ActualizarUsuario = "[Seg].[Usp_ActualizarUsuarios]";

        public const string Usp_ObtenerUsuarioLogeado = "[Seg].[Usp_ObtenerUsuarioLogeado]";

        public const string Usp_BloquearUsuario = "[Seg].[Usp_BloquearUsuario]";

        public const string Usp_RegistrarUsuario = "[Seg].[Usp_RegistrarUsuario]";

        public const string Usp_ValidarDniUsuario = "[Seg].[Usp_ValidarDniUsuario]";



        //public const string Usp_ListaRol = "[Seg].[Usp_ListaRoles]";

        //public const string Usp_ListaPerfiles = "[Seg].[Usp_ListaPerfiles]";

        public const string Usp_ListarMenu = "[Seg].[Usp_ListarMenu]";//

        public const string Usp_ListadoPerfiles = "[Seg].[Usp_ListadoPerfiles]";
        //public const string Usp_ListarPerfilUsuario = "[Seg].[Usp_ListarPerfilUsuario]";
        public const string Usp_ListarMenuPerfilUsuario = "[Seg].[Usp_ListarMenuPerfilUsuario]";
        public const string Usp_ListarMenuPerfil = "[Seg].[Usp_ListarMenuPerfil]";

        
        public const string Usp_RegistrarUsuarioPerfil = "[Seg].[Usp_RegistrarPerfilUsuario]";
        public const string Usp_DeleteUsuarioPerfil = "[Seg].[Usp_DeletePerfilUsuario]";


        public const string Usp_ListadoRoles = "[Seg].[Usp_ListadoRoles]";
        public const string Usp_ListarRolUsuario = "[Seg].[Usp_ListarRolUsuario]";


        public const string Usp_RegistrarUsuarioRol = "[Seg].[Usp_RegistrarUsuarioRol]";
        public const string Usp_DeleteUsuarioRol = "[Seg].[Usp_DeleteUsuarioRol]";

        public const string Usp_ObtenerUltimoIdUsuario = "[Seg].[Usp_ObtenerUltimoIdUsuario]";

        public const string Usp_ObtenerIdUsuarioforDni = "[Seg].[GetIdUsuarioforDni]";


        public const string Usp_ListaPerfiles = "[Seg].[Usp_ListaPerfil]";
        public const string Usp_InsertarNuevoPerfil = "[Seg].[Usp_InsertarPerfil]";

        public const string Usp_InsertarMenuPerfil = "[Seg].[Usp_InsertarMenuPerfil]";
        public const string Usp_DeleteMenuPerfil = "[Seg].[Usp_DeleteMenuPerfil]";

        public const string Usp_DeleteProfile = "[Seg].[Usp_DeleteProfile]";

        
        #endregion

        #region CONSULTAS

        public const string Usp_GetStatusDocument = "[Fact].[Usp_GetStatusDocument]";

        #endregion



        #region CONFIGURACION

        public const string Usp_GetListUrlAmbiente = "[Conf].[Usp_GetListURLAmbiente]";

        public const string Usp_GetListDocAmbiente = "[Conf].[Usp_GetListDocAmbiente]";

        //public const string Usp_GetAmbienteTrabjActual = "[Conf].[Usp_GetAmbienteTrabjActual]";

        public const string Usp_InsertDocumentAmbient = "[Fact].[Usp_InsertDocumentAmbient]";
        public const string Usp_UpdateDocAmb = "[Conf].[Usp_UpdateDocAmb]";

        public const string Usp_UpdateURLAmb = "[Conf].[Usp_UpdateURL]";

        #endregion


        #region LISTAS

        public const string Usp_ListaEstado = "[Mtro].[Usp_ListaEstado]";
        public const string Usp_ListaEmpleado = "[Mtro].[Usp_ListaEmpleado]";

        public const string Usp_ListaEmpresa = "[Mtro].[Usp_ListaEmpresa]";

        public const string Usp_ListCompGrp = "[Mtro].[Usp_ListCompGrp]";

        public const string Usp_ListaTipoDocumentoIdentidad = "[Ctl].[Usp_ListaTipoDocumentoIdentidad]";


        public const string Usp_ListaPais = "[Mtro].[Usp_ListaPais]";
        public const string Usp_ListaDepartamento = "[Mtro].[Usp_ListaDepartamento]";
        public const string Usp_ListaProvincia = "[Mtro].[Usp_ListaProvincia]";
        public const string Usp_ListaDistrito = "[Mtro].[Usp_ListaDistrito]";
        
        #endregion

        #region MANTENIMIENTOS

        public const string Usp_InsertarEmpleado = "[Mtro].[Usp_InsertarEmpleado]";
        public const string Usp_ActualizarEmpleado = "[Mtro].[Usp_ActualizarEmpleado]";
        public const string Usp_ValidarDniRuc = "[Mtro].[Usp_ValidarDniRuc]";

        public const string Usp_InsertarEmpresa = "[Mtro].[Usp_InsertarEmpresa]";
        public const string Usp_ActualizarEmpresa = "[Mtro].[Usp_ActualizarEmpresa]";
        public const string Usp_ListadoEmpresa = "[Mtro].[Usp_ListadoEmpresa]";
        public const string Usp_ObtenerUbigeo = "[Mtro].[Usp_ObtenerUbigeo]";

        public const string Usp_ValidarRucEmpresa = "[Mtro].[Usp_ValidarRucEmpresa]";

        public const string Usp_InsertarCorreo = "[Seg].[Usp_InsertarCorreo]";
        public const string Usp_ActualizarCorreo = "[Seg].[Usp_ActualizarCorreo]";
        public const string Usp_DeleteCorreo = "[Seg].[Usp_DeleteCorreo]";

        public const string Usp_ListaCorreo = "[Seg].[Usp_ListaCorreo]";

        public const string Usp_ValidarExistsCorreoEmpresa = "[Seg].[Usp_ValidarExistsCorreoEmpresa]";


        public const string Usp_InsertarCliente = "[Mtro].[Usp_InsertarCliente]";
        public const string Usp_ActualizarCliente = "[Mtro].[Usp_ActualizarCliente]";
        public const string Usp_ListaCliente = "[Mtro].[Usp_ListaCliente]";
        public const string Usp_ValidarNroDocumentoCliente = "[Mtro].[Usp_ValidarNroDocumentoCliente]";

        public const string Usp_Insert_MailAlert = "[Seg].[Usp_Insert_MailAlert]";
        public const string Usp_Update_MailAlert = "[Seg].[Usp_Update_MailAlert]";


        public const string Usp_Delete_MailAlert = "[Seg].[Usp_Delete_MailAlert]";


        public const string Usp_Update_UserCompany = "[Seg].[Usp_Update_UserCompany]";
        public const string Usp_GetList_ProfileCompany = "[Seg].[Usp_GetList_ProfileCompany]";

        public const string Usp_GetList_NotificationsMail = "[Seg].[Usp_GetList_NotificationsMail]";

        public const string Usp_GetId = "[Seg].[Usp_GetId]";
        #endregion


        #region CONSULTA PORTAL

        public const string Usp_ObtenerHistorialCliente_EmpresaPort = "[Fact].[Usp_ListaHistorialCliente_EmpresaPort]";
        public const string Usp_ObtenerDocumentoPortalWeb = "[Fact].[O.Usp_ObtenerDocumentoPortalWeb]";



        #endregion


        #region CHANGE CAMPO XML 

        public const string Usp_ResumenBajasCampoXML = "[Fact].[Usp_ResumenBajasCampoXML]";


        #endregion


        #region SEGURIDAD KY

        public const string Usp_KySum41 = "[Seg].[Usp_GetKySum41]";

        public const string Usp_GetIdEntityEmp = "[Seg].[Usp_GerIdEntityEmp]";

        public const string Usp_GetCredentialEntitySend = "[Fact].[Usp_GetCredencialEntitySend]";

        public const string Usp_GetEntitySetup = "[Seg].[Usp_GetEntitySetup]";


        public const string Usp_GetAmbienteTrbjActual = "[Seg].[Usp_GetAmbienteTrbjActual]";

        public const string Usp_UpdateAmbTrabjActual = "[Seg].[Usp_UpdateAmbTrabjActual]";

        public const string Usp_GetListAmbiente = "[Seg].[Usp_GetListAmbiente]";

        public const string Usp_GetCredentialEntity_Received = "[Fact].[Usp_GetCredencialEntity_Received]";

        public const string Usp_Get_us_pwd_amb = "[Conf].[Usp_Get_us_pwd_amb]";
        public const string Usp_Insert_SecondaryUserSunat = "[Conf].[Usp_Insert_SecondaryUserSunat]";


        #endregion

        #region AUDITORIA
        public const string Usp_InsertRegistroLogueo = "[Seg].[Usp_InsertRegistroLogueo]";
        public const string Usp_UpdateRegistroLogueo = "[Seg].[Usp_UpdateRegistroLogueo]";

        public const string Usp_DeleteRegistroLogueo = "[Seg].[Usp_DeleteRegistroLogueo]";

        public const string Usp_GetListRegistroLogueo = "[Seg].[Usp_GetListRegistroLogueo]";

        public const string Usp_GetDataFromUserLogueo = "[Seg].[Usp_GetDataFromUserLogueo]";


        public const string Usp_InsertLogLogueo = "[Seg].[Usp_InsertLogLogueo]";
        public const string Usp_GetListLogLogueo = "[Seg].[Usp_GetListLogLogueo]";

        public const string Usp_InsertLogAde = "[Seg].[Usp_InsertLogAde]";
        public const string Usp_GetListLogAde = "[Seg].[Usp_GetListLogAde]";

        public const string Usp_GetListTipoLog = "[Seg].[Usp_GetListTipoLog]";

        #endregion

        #region DOCUMENTO FROM RECIBIDO


        public const string Usp_InsertaDocumentoCabecera_Rec = "[Fact].[I.Usp_InsertaCabecera]";
        public const string Usp_InsertaDocumentoDetalle_Rec = "[Fact].[I.Usp_InsertaDetalle]";
        public const string Usp_InsertDocumentoDetalle_Rec_rp = "[Fact].[I.Usp_InsertaDetalle_rp]";

        public const string Usp_GetListDocumentoCabeceraReceived = "[Fact].[I.Usp_GetListDocumentoCabeceraReceived]";

        #endregion

        #region GET DATA CLIENT 

        public const string Usp_GetEmailClient = "[Fact].[Usp_GetEmailCliente]";

        public const string Usp_UpdateStatusDocSend = "[Fact].[Usp_UpdateStatusDocSend]";

        public const string Usp_UpdateStatusDocPrint = "[Fact].[Usp_UpdateStatusDocPrint]";

        #endregion

        #region CONSULTA WS

        public const string Usp_GetDocumentoXML = "[Fact].[Usp_GetDocumentoXML]";

        #endregion

        #region UTIL DOCUMENTO SEND AND PRINT

        public const string Usp_GetstatusSend_TPO_CE = "[Conf].[Usp_GetStatusSend_TPO_CE]";
        public const string Usp_GetstatusPrint_TPO_CE = "[Conf].[Usp_GetStatusPrint_TPO_CE]";

        public const string Usp_InsertTipoDoc_Send = "[Conf].[Usp_InsertTipoDoc_Send]";
        public const string Usp_InsertTipoDoc_Print = "[Conf].[Usp_InsertTipoDoc_Print]";

        public const string Usp_DeteleTipoDoc_Send = "[Conf].[Usp_DeleteTipoDoc_Send]";
        public const string Usp_DeleteTipoDoc_Print = "[Conf].[Usp_DeleteTipoDoc_Print]";


        public const string Usp_InsertTypeDoc_ForSend = "[Conf].[Usp_InsertTypeDoc_ForSend]";
        public const string Usp_InsertTypeDoc_ForPrint = "[Conf].[Usp_InsertTypeDoc_ForPrint]";

        #endregion

        #region HELPER

        public const string Usp_InsertarProvinciaxDepartamento = "[Mtro].[Usp_InsertProvinciaxDepartmento]";
        public const string Usp_InsertarDistritoxProvincia = "[Mtro].[Usp_InsertDistritoxProvincia]";

        public const string Usp_UpdateDistritoForProvincia = "[Mtro].[Usp_GetIdProvinciaForDistrito]";

        public const string Usp_InsertarDepartamento = "[Mtro].[Usp_InsertDepartamento]";

        #endregion


        #region CONFIG TIME SERVICE AND EXCHANGE RATE

        public const string Usp_GetExchangeRateToday = "[Conf].[Usp_GetExchangeRateToday]";
        public const string Usp_GetListCoin = "[Conf].[Usp_GetListCoin]";
        public const string Usp_ListTimeService = "[Conf].[Usp_ListTimeService]";
        public const string Usp_UpdateTimeService = "[Conf].[Usp_UpdateTimeService]";
        public const string Usp_InsertExchangeRateToday = "[Fact].[Usp_InsertExchangeRateToday]";
        public const string Usp_UpdateExchangeRateToday = "[Fact].[Usp_UpdateExchaneRateToday]";

        public const string Usp_GetListTypeDocumentSendEdit = "[Conf].[Usp_GetListTypeDocumentSendEdit]";


        public const string Usp_ListTypeDocument_TypeSend = "[Conf].[Usp_ListTypeDocument_TypeSend]";
        public const string Usp_ListTypeDocument_TypePrint = "[Conf].[Usp_ListTypeDocument_TypePrint]";


        public const string Usp_GetTimeForExeProccessMD = "[Seg].[Usp_GetTimeForExeProccessMD]";

        public const string Usp_GetCertificateData = "[Conf].[Usp_GetCertificateData]";


        #endregion END CONFIG TIME SERVICE AND EXCHANGE RATE


        #region MANT BANK

        public const string Usp_Insert_CtaBank = "[Mtro].[Usp_Insert_CtaBank]";
        public const string Usp_Update_CtaBank = "[Mtro].[Usp_Update_CtaBank]";
        public const string Usp_getListCtaBank = "[Mtro].[Usp_GetListCtaBank]";

        #endregion


        #region SEDE

        public const string Usp_ListSede = "[Seg].[Usp_ListSede]";

        #endregion

        #region exchange rate


        public const string Usp_GetExchangeRate_TD = "[Fact].[Usp_GetExchangeRate_TD]";

        public const string Usp_Ins_ExchangeRate_Today = "[Fact].[Usp_Ins_ExchangeRate_Today]";

        #endregion

        #region valida status service for company

        public const string Usp_GetStatus_WService = "[Seg].[Usp_GetStatus_WService]";

        #endregion

        #region pending document

        public const string Usp_GetList_PendingDocuments_RA = "[Fact].[Usp_GetList_PendingDocuments_RA]";
        public const string Usp_GetList_PendingDocuments_RC = "[Fact].[Usp_GetList_PendingDocuments_RC]";
        public const string Usp_GetList_PendingDocuments_ErrorSend = "[Fact].[Usp_GetList_PendingDocuments_ErrorSend]";

        #endregion
    }
}

