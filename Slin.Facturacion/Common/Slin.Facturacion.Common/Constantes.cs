using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slin.Facturacion.Common
{
    public class Constantes
    {
        #region LISTAS

        public const string ValorTodos = "- Todos -";
        public const string ValorSeleccione = "- Seleccione -";

        #endregion


        #region PERFIL

        public const string ValorAdmin = "";
        public const string ValorContabilidad = "";

        #endregion

        #region ValoresNumericos

        public const int ValorMenosUno = -1;
        public const int ValorCero = 0;
        public const int ValorUno = 1;
        public const int ValorDos = 2;
        public const int ValorTres = 3;
        public const int ValorCuatro = 4;
        public const int ValorCinco = 5;
        public const int ValorSeis = 6;
        public const int ValorSiete = 7;
        public const int ValorOcho = 8;
        public const int ValorNueve = 9;
        public const int ValorDiez = 10;

        public const int ValorOnce = 11;
        public const int ValorDoce = 12;
        public const int ValorTrece = 13;
        public const int ValorCatorce = 14;
        public const int ValorQuince = 15;
        public const int ValorVeinte = 20;


        public const int Valor50 = 50;

        public const double ValorCeroD = 0.0;
        #endregion


        #region RT

        public const string ValorRoot = "88888888";

        #endregion

        #region SUMARY

        public const string Cod_RR = "RR";
        public const string Cod_RC = "RC";
        public const string Cod_RA = "RA";

        #endregion

        #region Mensajes

        public const string MensajeNumDoc = "<script language=javascript>alert('El Numero de Documento de Inicio Debe Ser Menor al Numero Final');</script>";
        public const string MensajeFecha = "<script language=javascript>alert('La Fecha Final debe Ser Mayor');</script>";


        public const string msjCompletarCampos = "<script language=javascript>alert('Complete los Campos');</script>";

        public const string msjErrorPassword = "<script language=javascript>alert('Las Contaseñas son Distintas');</script>";

        public const string msjUsernameExist = "<script language=javascript>alert('El Usurname ya Existe');</script>";

        public const string msjActualizado = "Actualizado Correctamente";

        public const string msjClaveCambiada = "Contraseña Actualizada Correctamente";

        public const string msjRegistrado = "Registrado Correctamente";
        public const string msjErrorAlInsertar = "Error al Insertar Registro";

        public const string msjEnviadoCorrectamente = "¡Enviado Correctamente!";
        public const string msjErrorEnvio = "Error al Enviar";
        public const string msjNoExisteDocIncor = "El Documento no Existe o los Datos son Incorrectos";

        public const string msjValidaMotivoAnul = "Ingrese un motivo de anulacación valido (mínimo 5 caracteres)";

        public const string msjVacio = "";

        public const string msjLogeado = "";


        public const string msjRegistroExistente = "El Registro ya Existe";
        public const string msjAnuladoCorrectamente = "Anulado Correctamente";

        public const string msjErroralregistr = "Error al Registrar";

        public const string msjBarcodeCreate_Ok = "Barcode Create successful";

        public const string msjUsuarioBloqueado = "Usuario Bloqueado";
        public const string StatusLocked = "Bloqueado";

        public const string msjOrdenGenerate_RA = "Se generó la orden para el resumen, consulte el resumen en la seccion de Consultas Resumen de Bajas.";
        public const string msjOrdenGenerate_RC = "Se generó la orden para el resumen, consulte el resumen en la seccion de consultas Resumen de Boletas.";
        public const string msjOrdenGenerate_RR = "Se generó la orden para el resumen, consulte el resumen en la seccion de consultas Resumen de Reversiones.";
        public const string msjOrdenGenerate_Res = "No se pudo crear la orden para el resumen, consulte el permiso para escribir en la ruta: ";

        public const string msjPdfFailCreated = "No se pudo crear el PDF!.";
        #endregion


        #region TYPE DOCUMENT IDENTIFICATION

        public const string Code_0 = "NO DOM. S/RUC";
        public const string Code_1 = "DNI";
        public const string Code_4 = "C.E.";
        public const string Code_6 = "RUC";
        public const string Code_7 = "PASAPORTE";
        public const string Code_A = "CED. DIPLOMAT";

        #endregion


        #region TIPO DOCUMENTO

        public const string Factura = "01";
        public const string Boleta = "03";
        public const string NotaCredito = "07";
        public const string NotaDebito = "08";
        public const string GuiaRemision = "09";
        public const string TicketMaqRegistradora = "12";
        public const string DOCAFP = "18";
        public const string GuiaRemisionTransportista = "31";
        public const string Retencion = "20";
        public const string Percepcion = "40";



        public const string FacturaDesc = "Factura";
        public const string BoletaDesc = "Boleta de Venta";
        public const string NotaCreditoDesc = "Nota de Credito";
        public const string NotaDebitoDesc = "Nota de Debito";
        public const string GuiaRemisionDesc = "Guia de Remisión";
        public const string TicketMaqRegistradoraDesc = "Ticket";
        public const string DOCAFPDesc = "DOCAPF";
        public const string GuiaRemisionTransportistaDesc = "Guia de Remisión Trans";
        public const string RetencionDesc = "Retención";
        public const string PercepcionDesc = "Percepción";

        public const string Subject_Fact = "FACTURA ELECTRÓNICA - ";
        public const string Subject_Bol = "BOLETA DE VENTA ELECTRÓNICA - ";
        public const string Subject_Nc = "NOTA DE CRÉDITO ELECTRÓNICA - ";
        public const string Subject_Nd = "NOTA DE DÉBITO ELECTRÓNICA - ";
        public const string Subject_Ret = "COMPROBANTE DE RETENCIÓN ELECTRÓNICA - ";
        public const string Subject_Per = "COMPROBANTE DE PERCEPCIÓN ELECTRÓNICA - ";
        public const string Subject_Guia = "GUIA DE REMISIÓN ELECTRÓNICA - ";


        #endregion



        #region Code Type NC and ND

        public const string Cod_NC_01 = "01";
        public const string Cod_NC_02 = "02";
        public const string Cod_NC_03 = "03";
        public const string Cod_NC_04 = "04";
        public const string Cod_NC_05 = "05";
        public const string Cod_NC_06 = "06";
        public const string Cod_NC_07 = "07";
        public const string Cod_NC_08 = "08";
        public const string Cod_NC_09 = "09";

        public const string Desc_NC_01 = "Anulación de la Operacion";
        public const string Desc_NC_02 = "Anulación por Error en el RUC";
        public const string Desc_NC_03 = "Correccion por error en la descripción";
        public const string Desc_NC_04 = "Descuento global";
        public const string Desc_NC_05 = "Descuento por Item";
        public const string Desc_NC_06 = "Devolucion Total";
        public const string Desc_NC_07 = "Devolucion por Item";
        public const string Desc_NC_08 = "Bonificacion";
        public const string Desc_NC_09 = "Disminucion en el valor";

        public const string Cod_ND_01 = "01";
        public const string Cod_ND_02 = "02";
        public const string Cod_ND_03 = "03";

        public const string Desc_ND_01 = "Intereses por mora";
        public const string Desc_ND_02 = "Aumento en el valor";
        public const string Desc_ND_03 = "Penalidades";

        #endregion



        #region UNIDADES

        public const string Val_NIU = "NIU";
        public const string Val_KGM = "KGM";
        public const string Val_MTR = "MTR";
        public const string Val_BX = "BX";
        public const string Val_PK = "PK";
        public const string Val_LTR = "LTR";
        public const string Val_GLL = "GLL";
        public const string Val_CMT = "CMT";
        public const string Val_P4 = "P4";
        public const string Val_KT = "KT";
        public const string Val_CNP = "CNP";
        public const string Val_MTQ = "MTQ";
        public const string Val_RO = "RO";
        public const string Val_BG = "BG";
        public const string Val_MTK = "MTK";
        public const string Val_HUR = "HUR";

        public const string Val_UNI = "UNI";
        public const string Val_KG = "KG";
        public const string Val_METRO = "METRO";
        public const string Val_CAJA = "CAJA";
        public const string Val_PACK = "PACK";
        public const string Val_LT = "LT";
        public const string Val_GLN = "GLN";
        public const string Val_CM = "CM";
        public const string Val_4PACK = "4 PACK";
        public const string Val_KIT = "KIT";
        public const string Val_PQ100 = "PQ100";
        public const string Val_M3 = "M3";
        public const string Val_ROLL = "ROLL";
        public const string Val_BOLSA = "BOLSA";
        public const string Val_M2 = "M2";
        public const string Val_HORA = "HORA";

        #endregion


        #region EXTENSION

        public const string Ext_XML = ".xml";
        public const string Ext_PDF = ".pdf";

        #endregion

        #region FACTURACION

        public const int ValorIGV = 18;

        public const string ValorCeroMonto = "0.00";

        public const string DOLAR = "$";
        public const string SOLES = "S/";
        public const string USD = "USD";
        public const string PEN = "PEN";

        #endregion

        #region UTIL

        public const string ValorExportPDF = "PDF";
        public const string ValorExportExcel = "EXCEL";

        #endregion


        #region TIPO AMBIENTE DE TRABAJO

        public const int Produccion = 1;
        public const int Test = 2;
        public const int Homologacion = 3;

        public const string AmbienteTEST = "Ambiente de Test";
        public const string AmbienteQA = "Ambiente de Homologación";
        public const string AmbientePROD = "Ambiente de Producción";

        #endregion

        #region ACTIVEDIRECTORY

        public const string MsjLoginWithActiveDirectoryOK = "El servidor ha devuelto una referencia.\r\n";
        public const string MsjLoginWithActiveDirectoryOK_ENG = "A referral was returned from the server.\r\n";
        public const string MsjLoginWithActiveDirectoryOK_ENG_sl = "A referral was returned from the server.";
        public const string LoginWithLDAP = "LDAP";

        #endregion


        #region EXTRA

        public const string RolGuardar = "G";
        public const string RolModificar = "M";
        public const string RolBuscar = "B";
        public const string RolExportar = "EX";
        public const string RolEnviar = "EN";
        public const string RolNuevo = "N";

        public const string Guion = "-";

        public const string ValorSI = "SI";
        public const string ValorNO = "NO";

        public const string SaltoLinea = "\n";

        #endregion

        #region INSTALL

        public const string MsjeProgressComplete = "ProcessOk";
        public const string Admin = "Administrador";

        public const string root = "root";
        public const string rootdni = "7777777";
        public const string rootemail = "root@root.com";
        public const string DateNull = "1753-01-01 00:00:00.000";
        public const string rootfone = "0000000000";

        public const string ValorWS = "WS";
        public const string ValorAPP = "APP";

        public const string App_slinade = "slinade";
        public const string App_serviceade = "serviceade";
        public const string App_serviceinterface = "serviceinterface";
        public const string App_serviceconsult = "serviceconsult";
        public const string App_portalade = "portalade";
        public const string App_serviceportal = "serviceportal";

        public const string ValueTime_Initial = "21:00";
        public const string IntervalValue_Initial = "5";
        public const int MaxNumAttempts_Initial = 50;


        #endregion


        #region mensaje to services

        public static string MsjStart = "[" + DateTime.Now + "] ----------------------------------------INICIO-------------------------------------";
        public static string MsjStartProcessDoc = "[" + DateTime.Now + "] Doc. en proceso e info  : ";
        public static string Msj_DocProcess = "[" + DateTime.Now + "] Doc. en proceso         : ";


        public static string MsjFileDelete = "[" + DateTime.Now + "] Eliminando Archivo      : ";
        public static string MsjEndProcessDoc = "[" + DateTime.Now + "] Fin Proceso para el Doc : ";
        public static string MsjEnd = "[" + DateTime.Now + "] -----------------------------------------FIN---------------------------------------";


        public static string MsjRucInvalid = "[" + DateTime.Now + "] Ruc de empresa invalido : ";
        public static string MsjRucInactivo = "[" + DateTime.Now + "] El servicio para el Ruc: RUCREPLACE no está activo o no cuenta con este servicio.";

        public static string MsjDocNoObtenido_BD = "[" + DateTime.Now + "] No se obtuvo Doc. en BD : ";
        public static string MsjDocObtenido_BD = "[" + DateTime.Now + "] Se obtuvo Doc. de la BD : ";

        public static string MsjXmlNoExiste_inPath = "[" + DateTime.Now + "] Xml no existe del Doc.  : ";

        public static string MsjFormatPrint_Termic = "[" + DateTime.Now + "] Formato a Imprimir      : Termico";

        public static string MsjFormatPrint_Pdf = "[" + DateTime.Now + "] Formato a Imprimir      : PDF";

        public static string MsjErrorReadXml = "[" + DateTime.Now + "] Error al leer el XML.";

        public static string PrinterName = "[" + DateTime.Now + "] Nombre de la Impresora  : ";

        public static string MsjPrinted_Ok = "[" + DateTime.Now + "] Doc. Impresó con exito.";

        public static string MsjNot_Printed = "[" + DateTime.Now + "] El Doc. no se a impreso.";

        public static string Msj_PdfNotCreated = "[" + DateTime.Now + "] No se pudo crear el PDF.";

        public static string Msj_Error = "[" + DateTime.Now + "] Error                   : ";

        public static string Msj_Pdf_Procesing = "[" + DateTime.Now + "] Procesando PDF          ";

        public static string Msj_PdfFail = "[" + DateTime.Now + "] No se pudo crear el Pdf.";


        public static string Msj_DocUpdate = "[" + DateTime.Now + "] Doc. actualizado        : ";
        public static string Msj_DocEnvRegistr = "[" + DateTime.Now + "] Doc. enviado registrado : ";
        public static string Doc_inProcess = "[" + DateTime.Now + "] Documento en Proceso    : ";

        public static string Doc_inConsult = "[" + DateTime.Now + "] Documento en consulta   : ";

        public static string Msje_FileConsult_pdf = "[" + DateTime.Now + "] Archivo Consultado      : PDF";
        public static string Msje_FileConsult_xml = "[" + DateTime.Now + "] Archivo Consultado      : XML";

        public static string Msje_StatusConsult = "[" + DateTime.Now + "] Consulta de estado.      ";


        public static string MsjFileDelete_Send = "[" + DateTime.Now + "] Eliminando Archivo      : ";
        public static string MsjEndProcess_Send = "[" + DateTime.Now + "] Fin Proceso para el Doc : ";
        public static string MsjRucInvalid_Send = "[" + DateTime.Now + "] Ruc de empresa invalido : ";
        public static string MsjRucInactivo_Send = "[" + DateTime.Now + "] El servicio para el Ruc : RUCREPLACE no está activo o no cuenta con este servicio.";

        public static string Msj_FileProccesing = "[" + DateTime.Now + "] Procesando Archivo      : ";
        public static string Msj_FilePdfExists = "[" + DateTime.Now + "] El PDF existe.          : ";

        public static string Msj_CreatingPdfFile = "[" + DateTime.Now + "] Creando PDF del Doc.    : ";
        public static string Msj_PdfCreatedSuccess = "[" + DateTime.Now + "] PDF creado con Exito    : ";
        public static string Msj_ErrorCreatedPdf = "[" + DateTime.Now + "] Error al crear PDF      : ";

        public static string Msj_CreatingXmlDoc = "[" + DateTime.Now + "] Creando XML del Doc.    : ";
        public static string Msj_AditionalsFiles = "[" + DateTime.Now + "] Archivos adicionales    : ";
        public static string Msj_FilesToSend = "[" + DateTime.Now + "] Archivos a enviar       : ";

        public static string Msj_DocSendSuccess = "[" + DateTime.Now + "] Doc. enviado con exito  : ";
        public static string Msj_DocNotSend = "[" + DateTime.Now + "] Doc. no enviado         : ";

        public static string Msj_NoEmailWasReceived = "[" + DateTime.Now + "] No se obvuvieron correos de destino.";

        public static string Msj_OutgoingMailNull = "[" + DateTime.Now + "] No se obtuvieron correos salientes. (No existe/Inactivo)";

        public static string Value_DateToLog = "[" + DateTime.Now + "] ";

        public static string Msj_FileNotExists = "[" + DateTime.Now + "] El recurso no existe    : ";
        public static string Msj_DocAnul_Registr = "[" + DateTime.Now + "] Doc. Anulado registrado : ";


        public static string Msj_EmailDestino = "[" + DateTime.Now + "] Email destino           : ";
        public static string Msj_TpoDoc = "[" + DateTime.Now + "] Tipo Documento          : ";
                                                                  
        public static string Msj_Response_PdfBase64 = "[" + DateTime.Now + "] Se reenviara el Pdf en Formato Base64";
        public static string Msj_Response_xmlBase64 = "[" + DateTime.Now + "] Se reenviara el xml en Formato Base64";

        public static string Msj_XmlNoExists = "[" + DateTime.Now + "] No se obtuvo el xml del doc.";

        #endregion

        //public const string HtmlLineSend_Slin = "<html><head><title></title></head><body lang=\"es-pe\"><div class=\"modal-content\" style=\"border:solid\"><div class=\"modal-header\"><div class=\"col-lg-9\" style=\"text-align:left; padding-left:5px\"><img src=\"http://200.60.6.20:8190/WebFac/Img/home/slin.png\" style=\"font-size:x-large\" /></div></div><div class=\"modal-body\"><div class=\"row\" style=\"padding-left:5px\"><div class=\"col-lg-3\"><label style=\"font-family:Cambria;\">Señor(es),</label></div><div class=\"col-lg-9\"><b>{ClienteRazonSocial}</b></div></div><br /><div class=\"row\" style=\"padding-left:5px\"><div class=\"col-lg-8\"><b style=\"font-family:Cambria;\">Le Informamos que ha recibido un Documento Electrónico de {RazonSocialEmisor}</b></div></div><br /><br /><div class=\"row\" style=\"padding-left:5px\"><div class=\"col-lg-8\"><table border=\"0\"><tr><td><label style=\"font-family:Cambria;\">Número de Documento</label></td><td><b>: {SerieCorrelativo}</b></td></tr><tr><td><label style=\"font-family:Cambria;\">Tipo de Documento</label></td><td><b>: {TipoDocumento}</b></td></tr><tr><td><label style=\"font-family:Cambria;\">Fecha Emisión</label></td><td><b>: {FechaEmision}</b></td></tr><tr><td><label style=\"font-family:Cambria;\">Moneda</label></td><td><b>: {Moneda}</b></td></tr><tr><td><label style=\"font-family:Cambria;\">Valor</label></td><td><b lang=\"es-pe\">: {MontoTotal}</b></td></tr></table></div><div class=\"col-lg-4\"><label></label></div></div><br /><b style=\"font-family:Cambria; font-size:small; padding-left:5px;\">Para Consultar su Documento en la Web visite: <a style=\"font-family:Cambria\" href=\"www.slin.com.pe\">http://www.slin.com.pe</a></b><br /><br /><b style=\"padding-left:5px\">Atentamente,</b><br><br></div><div class=\"modal-footer\"><div class=\"row\" style=\"padding-left:5px\"><div class=\"col-lg-6\"><label style=\"font-family:Cambria; font-size:smaller;\">Este es un sistema automático, por favor no responda este mensaje al correo.</label></div></div></div></div></body></html>";
        public const string HtmlLineSend_Company = "<html><head><title></title></head><body lang=\"es-pe\"><div class=\"modal-content\" style=\"border:solid\"><div class=\"modal-header\"><div class=\"col-lg-9\" style=\"text-align:left; padding-left:5px\"><img src=\"{URL_COMPANY_LOGO}\" style=\"font-size:x-large\" /></div></div><div class=\"modal-body\"><div class=\"row\" style=\"padding-left:5px\"><div class=\"col-lg-3\"><label style=\"font-family:Cambria;\">Señor(es),</label></div><div class=\"col-lg-9\"><b>{ClienteRazonSocial}</b></div></div><br /><div class=\"row\" style=\"padding-left:5px\"><div class=\"col-lg-8\"><b style=\"font-family:Cambria;\">Le Informamos que ha recibido un Documento Electrónico de {RazonSocialEmisor}</b></div></div><br /><br /><div class=\"row\" style=\"padding-left:5px\"><div class=\"col-lg-8\"><table border=\"0\"><tr><td><label style=\"font-family:Cambria;\">Tipo de Documento</label></td><td><b>: {TipoDocumento}</b></td></tr><tr><td><label style=\"font-family:Cambria;\">Número de Documento</label></td><td><b>: {SerieCorrelativo}</b></td></tr><tr><td><label style=\"font-family:Cambria;\">Fecha Emisión</label></td><td><b>: {FechaEmision}</b></td></tr></table></div><div class=\"col-lg-4\"><label></label></div></div><br /><b style=\"font-family:Cambria; font-size:small; padding-left:5px;\">Para Consultar su Documento en la Web visite: <a style=\"font-family:Cambria\" href=\"URL_COMPANY_CONSULT\">{URL_COMPANY_CONSULT}</a></b><br /><br /><b style=\"padding-left:5px\">Atentamente,</b><br><br></div><div class=\"modal-footer\"><div class=\"row\" style=\"padding-left:5px\"><div class=\"col-lg-6\"><label style=\"font-family:Cambria; font-size:smaller;\">Este es un sistema automático, por favor no responda este mensaje al correo.</label></div></div></div></div></body></html>";
        //public const string HtmlLineSend_Company = "<html><head><title></title></head><body lang=\"es-pe\"><div class=\"modal-content\" style=\"border:solid\"><div class=\"modal-header\"><div class=\"col-lg-9\" style=\"text-align:left; padding-left:5px\"><img src=\"{URL_COMPANY_LOGO}\" style=\"font-size:x-large\" /></div></div><div class=\"modal-body\"><div class=\"row\" style=\"padding-left:5px\"><div class=\"col-lg-3\"><label style=\"font-family:Cambria;\">Señor(es),</label></div><div class=\"col-lg-9\"><b>{ClienteRazonSocial}</b></div></div><br /><div class=\"row\" style=\"padding-left:5px\"><div class=\"col-lg-8\"><b style=\"font-family:Cambria;\">Le Informamos que ha recibido un Documento Electrónico de {RazonSocialEmisor}</b></div></div><br /><br /><div class=\"row\" style=\"padding-left:5px\"><div class=\"col-lg-8\"><table border=\"0\"><tr><td><label style=\"font-family:Cambria;\">Tipo de Documento</label></td><td><b>: {TipoDocumento}</b></td></tr><tr><td><label style=\"font-family:Cambria;\">Número de Documento</label></td><td><b>: {SerieCorrelativo}</b></td></tr><tr><td><label style=\"font-family:Cambria;\">Fecha Emisión</label></td><td><b>: {FechaEmision}</b></td></tr></table></div><div class=\"col-lg-4\"><label></label></div></div><br /><b style=\"font-family:Cambria; font-size:small; padding-left:5px;\">Para cualquier solicitud, duda o comentario escribir:</b><br /><b style=\"font-family:Cambria; font-size:smaller; padding-left:5px; font-weight:normal;\">- Vanessa Tapia &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:vanessa.tapia@pe.pwc.com</b><br /><b style=\"font-family:Cambria; font-size:smaller; padding-left:5px; font-weight:normal;\"> - Milagros Barraza &nbsp;&nbsp;:milagros.barraza@pe.pwc.com <br /><br /><b style=\"font-family:Cambria; font-size:small; padding-left:5px; font-weight:normal;\">- Para consultar su documento Electrónico visite&nbsp;&nbsp;&nbsp;: </b><a style=\"font-family:Cambria\" href=\"URL_COMPANY_CONSULT\">{URL_COMPANY_CONSULT}</a></b><br /><br /><b style=\"padding-left:5px\">Atentamente,</b><br><br></div><div class=\"modal-footer\"><div class=\"row\" style=\"padding-left:5px\"><div class=\"col-lg-6\"><label style=\"font-family:Cambria; font-size:smaller;\">Este es un sistema automático, por favor no responda este mensaje al correo.</label></div></div></div></div></body></html>";


        //public const string ScriptUpdate_ProfileUser = "USE DATABASEREPLACE SET IDENTITY_INSERT [Seg].[Perfil] ON  INSERT [Seg].[Perfil] ([IdPerfil], [NombrePerfil], [Codigo], [RucEntity]) VALUES (5, N'Seguridad de la Información', N'S', N'RUCREPLACE') SET IDENTITY_INSERT [Seg].[Perfil] OFF UPDATE [Seg].[Usuarios] SET [IdPerfil] = 5 WHERE Username = 'USERREPLACE' SET IDENTITY_INSERT [Seg].[MenuPerfil] ON  INSERT [Seg].[MenuPerfil] ([IdMenuPerfil], [IdMenu], [IdPerfil]) VALUES (40, 1, 5) INSERT [Seg].[MenuPerfil] ([IdMenuPerfil], [IdMenu], [IdPerfil]) VALUES (41, 5, 5) INSERT [Seg].[MenuPerfil] ([IdMenuPerfil], [IdMenu], [IdPerfil]) VALUES (42, 13, 5) INSERT [Seg].[MenuPerfil] ([IdMenuPerfil], [IdMenu], [IdPerfil]) VALUES (43, 14, 5) INSERT [Seg].[MenuPerfil] ([IdMenuPerfil], [IdMenu], [IdPerfil]) VALUES (44, 16, 5) SET IDENTITY_INSERT [Seg].[MenuPerfil] OFF";
        //public const string ScriptUpdate_ProfileUser_Second = "USE DATABASEREPLACE DECLARE @NEWID INT SET @NEWID = (SELECT MAX(IdPerfil) + 1 FROM [Seg].[Perfil]) SET IDENTITY_INSERT [Seg].[Perfil] ON  INSERT [Seg].[Perfil] ([IdPerfil], [NombrePerfil], [Codigo], [RucEntity]) VALUES (@NEWID, N'Seguridad de la Información', N'S', N'RUCREPLACE') SET IDENTITY_INSERT [Seg].[Perfil] OFF UPDATE [Seg].[Usuarios] SET [IdPerfil] = @NEWID WHERE Username = 'USERREPLACE' DECLARE @NEWIDMENUPERFIL INT SET @NEWIDMENUPERFIL = (SELECT MAX([IdMenuPerfil]) FROM [Seg].[MenuPerfil]) SET IDENTITY_INSERT [Seg].[MenuPerfil] ON  INSERT [Seg].[MenuPerfil] ([IdMenuPerfil], [IdMenu], [IdPerfil]) VALUES (@NEWIDMENUPERFIL + 1, 1, @NEWID) INSERT [Seg].[MenuPerfil] ([IdMenuPerfil], [IdMenu], [IdPerfil]) VALUES (@NEWIDMENUPERFIL + 2, 5, @NEWID) INSERT [Seg].[MenuPerfil] ([IdMenuPerfil], [IdMenu], [IdPerfil]) VALUES (@NEWIDMENUPERFIL + 3, 13, @NEWID) INSERT [Seg].[MenuPerfil] ([IdMenuPerfil], [IdMenu], [IdPerfil]) VALUES (@NEWIDMENUPERFIL + 4, 14, @NEWID) INSERT [Seg].[MenuPerfil] ([IdMenuPerfil], [IdMenu], [IdPerfil]) VALUES (@NEWIDMENUPERFIL + 5, 16, @NEWID) SET IDENTITY_INSERT [Seg].[MenuPerfil] OFF";

        public const string ScriptUpdate_ProfileUser = "USE DATABASEREPLACE DECLARE @NEWID INT SET @NEWID = (SELECT MAX(IdPerfil) + 1 FROM [Seg].[Perfil])  SET IDENTITY_INSERT [Seg].[Perfil] ON  INSERT [Seg].[Perfil] ([IdPerfil], [NombrePerfil], [Codigo], [RucEntity]) VALUES (@NEWID, N'Seguridad de la Información', N'SI', N'RUCREPLACE')   SET IDENTITY_INSERT [Seg].[Perfil] OFF UPDATE [Seg].[Usuarios] SET [IdPerfil] = @NEWID WHERE Username = 'USERREPLACE'   DECLARE @NEWIDMENUPERFIL INT SET @NEWIDMENUPERFIL = (SELECT ISNULL(MAX([IdMenuPerfil]),0) + 1 FROM [Seg].[MenuPerfil])  SET IDENTITY_INSERT [Seg].[MenuPerfil] ON  INSERT [Seg].[MenuPerfil] ([IdMenuPerfil], [IdMenu], [IdPerfil]) VALUES (@NEWIDMENUPERFIL, 1, @NEWID)   INSERT [Seg].[MenuPerfil] ([IdMenuPerfil], [IdMenu], [IdPerfil]) VALUES (@NEWIDMENUPERFIL + 1, 5, @NEWID)   INSERT [Seg].[MenuPerfil] ([IdMenuPerfil], [IdMenu], [IdPerfil]) VALUES (@NEWIDMENUPERFIL + 2, 6, @NEWID)   INSERT [Seg].[MenuPerfil] ([IdMenuPerfil], [IdMenu], [IdPerfil]) VALUES (@NEWIDMENUPERFIL + 3, 13, @NEWID)   INSERT [Seg].[MenuPerfil] ([IdMenuPerfil], [IdMenu], [IdPerfil]) VALUES (@NEWIDMENUPERFIL + 4, 14, @NEWID)   INSERT [Seg].[MenuPerfil] ([IdMenuPerfil], [IdMenu], [IdPerfil]) VALUES (@NEWIDMENUPERFIL + 5, 16, @NEWID)   INSERT [Seg].[MenuPerfil] ([IdMenuPerfil], [IdMenu], [IdPerfil]) VALUES (@NEWIDMENUPERFIL + 6, 18, @NEWID)   INSERT [Seg].[MenuPerfil] ([IdMenuPerfil], [IdMenu], [IdPerfil]) VALUES (@NEWIDMENUPERFIL + 7, 44, @NEWID) INSERT [Seg].[MenuPerfil] ([IdMenuPerfil], [IdMenu], [IdPerfil]) VALUES (@NEWIDMENUPERFIL + 8, 45, @NEWID)   INSERT [Seg].[MenuPerfil] ([IdMenuPerfil], [IdMenu], [IdPerfil]) VALUES (@NEWIDMENUPERFIL + 9, 47, @NEWID)  SET IDENTITY_INSERT [Seg].[MenuPerfil] OFF";
        public const string ScriptUpdate_ProfileUser_Second = "USE DATABASEREPLACE  DECLARE @NEWID INT SET @NEWID = (SELECT MAX(IdPerfil) + 1 FROM [Seg].[Perfil])  SET IDENTITY_INSERT [Seg].[Perfil] ON  INSERT [Seg].[Perfil] ([IdPerfil], [NombrePerfil], [Codigo], [RucEntity]) VALUES (@NEWID, N'Seguridad de la Información', N'SI', N'RUCREPLACE')   SET IDENTITY_INSERT [Seg].[Perfil] OFF UPDATE [Seg].[Usuarios] SET [IdPerfil] = @NEWID WHERE Username = 'USERREPLACE'   DECLARE @NEWIDMENUPERFIL INT SET @NEWIDMENUPERFIL = (SELECT ISNULL(MAX([IdMenuPerfil]),0) + 1 FROM [Seg].[MenuPerfil])   SET IDENTITY_INSERT [Seg].[MenuPerfil] ON   INSERT [Seg].[MenuPerfil] ([IdMenuPerfil], [IdMenu], [IdPerfil]) VALUES (@NEWIDMENUPERFIL, 1, @NEWID)   INSERT [Seg].[MenuPerfil] ([IdMenuPerfil], [IdMenu], [IdPerfil]) VALUES (@NEWIDMENUPERFIL + 1, 5, @NEWID)   INSERT [Seg].[MenuPerfil] ([IdMenuPerfil], [IdMenu], [IdPerfil]) VALUES (@NEWIDMENUPERFIL + 2, 6, @NEWID)   INSERT [Seg].[MenuPerfil] ([IdMenuPerfil], [IdMenu], [IdPerfil]) VALUES (@NEWIDMENUPERFIL + 3, 13, @NEWID)   INSERT [Seg].[MenuPerfil] ([IdMenuPerfil], [IdMenu], [IdPerfil]) VALUES (@NEWIDMENUPERFIL + 4, 14, @NEWID)   INSERT [Seg].[MenuPerfil] ([IdMenuPerfil], [IdMenu], [IdPerfil]) VALUES (@NEWIDMENUPERFIL + 5, 16, @NEWID)   INSERT [Seg].[MenuPerfil] ([IdMenuPerfil], [IdMenu], [IdPerfil]) VALUES (@NEWIDMENUPERFIL + 6, 18, @NEWID)   INSERT [Seg].[MenuPerfil] ([IdMenuPerfil], [IdMenu], [IdPerfil]) VALUES (@NEWIDMENUPERFIL + 7, 44, @NEWID)   INSERT [Seg].[MenuPerfil] ([IdMenuPerfil], [IdMenu], [IdPerfil]) VALUES (@NEWIDMENUPERFIL + 8, 45, @NEWID)   INSERT [Seg].[MenuPerfil] ([IdMenuPerfil], [IdMenu], [IdPerfil]) VALUES (@NEWIDMENUPERFIL + 9, 47, @NEWID)   SET IDENTITY_INSERT [Seg].[MenuPerfil] OFF   DECLARE @IDAMB INT SET @IDAMB = (SELECT IDAMBIENTE FROM [Conf].[Tpo_Amb_Trabj] WHERE COD = 'DEV')  INSERT INTO [Conf].[AmbienteTrabj] (IDAMBIENTE, RUCENTITY) VALUES (@IDAMB, 'RUCREPLACE')";
    }
}
