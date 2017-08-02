using WCFConfiguracion = Slin.Facturacion.Proxies.ServicioConfiguracion;
using WCFFacturacion = Slin.Facturacion.Proxies.ServicioFacturacion;
using WCFMantenimiento = Slin.Facturacion.Proxies.ServicioMantenimiento;
using WCFSeguridad = Slin.Facturacion.Proxies.ServicioSeguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slin.Facturacion.ServiceController
{
    public sealed class Singleton_SC
    {
        //private static readonly Singleton_SC instance = new Singleton_SC();
        //static Singleton_SC() { }
        //private Singleton_SC() { }
        //public static Singleton_SC Instance { get { return instance; } }


        //#region configuracion

        //#region AMBIENTE

        //public WCFConfiguracion.ListaURLAmbiente GetListURLAmbienteSunat()
        //{
        //    return ConfiguracionServiceController.Instance.GetListURLAmbienteSunat();
        //}

        //public WCFConfiguracion.ListaDocumentoAmbiente GetListaDocAmbiente(int ID, string RucEntity)
        //{
        //    return ConfiguracionServiceController.Instance.GetListaDocAmbiente(ID, RucEntity);
        //}

        //public String InsertDocumentAmbiente(WCFConfiguracion.ListaDocumentoAmbiente objListDocAmb)
        //{
        //    return ConfiguracionServiceController.Instance.InsertDocumentAmbiente(objListDocAmb);
        //}

        //public String UpdateDocAmbienteEstado(WCFConfiguracion.ListaDocumentoAmbiente objListDocAmb)
        //{
        //    return ConfiguracionServiceController.Instance.UpdateDocAmbienteEstado(objListDocAmb);
        //}

        //public String UpdateURLAmbiente(WCFConfiguracion.ListaURLAmbiente objListUrlAmb)
        //{
        //    return ConfiguracionServiceController.Instance.UpdateURLAmbiente(objListUrlAmb);
        //}

        //#endregion

        //#region CONFIG TIME SERVICE AND EXCHANGE RATE

        //public WCFConfiguracion.TipoCambio GetexchangeRateToday(string Today, string RucEntity)
        //{
        //    return ConfiguracionServiceController.Instance.GetexchangeRateToday(Today, RucEntity);
        //}

        //public WCFConfiguracion.ListaMoneda GetListMoneda()
        //{
        //    return ConfiguracionServiceController.Instance.GetListMoneda();
        //}

        //public WCFConfiguracion.ListService GetListTimeService(string RucEntity)
        //{
        //    return ConfiguracionServiceController.Instance.GetListTimeService(RucEntity);
        //}

        //public String UpdateTimeService(WCFConfiguracion.Services oService)
        //{
        //    return ConfiguracionServiceController.Instance.UpdateTimeService(oService);
        //}

        //public String InsertExchangeRateToday(WCFConfiguracion.TipoCambio objRate)
        //{
        //    return ConfiguracionServiceController.Instance.InsertExchangeRateToday(objRate);
        //}

        //public String UpdateExchangeRateToday(WCFConfiguracion.TipoCambio objRate)
        //{
        //    return ConfiguracionServiceController.Instance.UpdateExchangeRateToday(objRate);
        //}

        //#endregion END CONFIG TIME SERVICE AND EXCHANGE RATE


        //#region NEW METHOD FOR CONFIG SEND AND PRINT

        //public String InsertTypeDocument_ForSend(WCFConfiguracion.TipoDocumento otypedoc)
        //{
        //    return ConfiguracionServiceController.Instance.InsertTypeDocument_ForSend(otypedoc);
        //}


        //public String InsertTypeDocument_ForPrint(WCFConfiguracion.TipoDocumento otypedoc)
        //{
        //    return ConfiguracionServiceController.Instance.InsertTypeDocument_ForPrint(otypedoc);
        //}

        //public WCFConfiguracion.ListaTipoDocumento ListTypeDocument_TypeSend(string RucEntity)
        //{
        //    return ConfiguracionServiceController.Instance.ListTypeDocument_TypeSend(RucEntity);
        //}

        //public WCFConfiguracion.ListaTipoDocumento ListTypeDocument_TypePrint(string RucEntity)
        //{
        //    return ConfiguracionServiceController.Instance.ListTypeDocument_TypePrint(RucEntity);
        //}

        //#endregion END NEW METHOD FOR CONFIG SEND AND PRINT

        //#region SECONDARY USER SUNAT

        //public WCFConfiguracion.ListSecondaryUser Get_SecondaryUserSunat(int IdAmb, string RucEntity)
        //{
        //    return ConfiguracionServiceController.Instance.Get_SecondaryUserSunat(IdAmb, RucEntity);
        //}

        //public string Insert_SecondaryUserSunat_Amb(WCFConfiguracion.SecondaryUser objSeconUser)
        //{
        //    return ConfiguracionServiceController.Instance.Insert_SecondaryUserSunat_Amb(objSeconUser);
        //}

        //#endregion

        //#region certificate digital

        //public WCFConfiguracion.ListCertificateDigital Get_CertificateDigitalInformation(string RucEntity)
        //{
        //    return ConfiguracionServiceController.Instance.Get_CertificateDigitalInformation(RucEntity);
        //}

        //#endregion

        //#endregion

        //#region facturacion

        //#region CONSULTAS

        //public WCFFacturacion.ListaFacturaElectronica ListarDocumentoCabecera(WCFFacturacion.FacturaElectronica oFacturaElectronica)
        //{
        //    return FacturacionServiceController.Instance.ListarDocumentoCabecera(oFacturaElectronica);
        //}

        //public WCFFacturacion.ListaDetalleFacturaElectronica ListarDocumentoDetalle(String tipoDocumento, String NumeroSerie, String NumeroDocumento, String RucEmpresa)
        //{
        //    return FacturacionServiceController.Instance.ListarDocumentoDetalle(tipoDocumento, NumeroSerie, NumeroDocumento, RucEmpresa);
        //}

        //public String GenerarXML(String tipoDocumento, String NumeroSerie, String NumeroDocumento, String RucEmpresa)
        //{
        //    return FacturacionServiceController.Instance.GenerarXML(tipoDocumento, NumeroSerie, NumeroDocumento, RucEmpresa);
        //}

        //public WCFFacturacion.ListaFacturaElectronica GetListaMontoCab(String tipoDocumento, String NumeroSerie, String NumeroDocumento, String RucEmpresa)
        //{
        //    return FacturacionServiceController.Instance.GetListaMontoCab(tipoDocumento, NumeroSerie, NumeroDocumento, RucEmpresa);
        //}

        //#endregion

        //#region CRE, CPE

        //public WCFFacturacion.ListaFacturaElectronica ListarDocumentoCabeceraCRECPE(WCFFacturacion.FacturaElectronica oFacturaElectronica)
        //{
        //    return FacturacionServiceController.Instance.ListarDocumentoCabeceraCRECPE(oFacturaElectronica);
        //}

        //#endregion

        //#region LISTAS

        //public WCFFacturacion.ListaEstado ListarEstadoDocumento()
        //{
        //    return FacturacionServiceController.Instance.ListarEstadoDocumento();
        //}

        //public WCFFacturacion.ListaTipoDocumento ListarTipoDocumento()
        //{
        //    return FacturacionServiceController.Instance.ListarTipoDocumento();
        //}

        //public WCFFacturacion.ListaSerie ListarSerie(WCFFacturacion.Serie oSerie)
        //{
        //    return FacturacionServiceController.Instance.ListarSerie(oSerie);
        //}

        //#endregion

        //#region DOCUMENTO ANULADO

        //public WCFFacturacion.FacturaElectronica GetFechaDocumento(int tpodocumento, String serie, String nrodocumento, String rucempresa)
        //{
        //    return FacturacionServiceController.Instance.GetFechaDocumento(tpodocumento, serie, nrodocumento, rucempresa);
        //}

        //public WCFFacturacion.ListaFacturaElectronica GetValidarDocumentoExiste(WCFFacturacion.FacturaElectronica oDocAnulado)
        //{
        //    return FacturacionServiceController.Instance.GetValidarDocumentoExiste(oDocAnulado);
        //}

        //public String InsertarDocumentoAnulado(WCFFacturacion.FacturaElectronica oDocumentoAnulado)
        //{
        //    return FacturacionServiceController.Instance.InsertarDocumentoAnulado(oDocumentoAnulado);
        //}

        //public WCFFacturacion.ListaDocumento ValidarExisteDocAnulado(WCFFacturacion.FacturaElectronica oDocumentoAnulado)
        //{
        //    return FacturacionServiceController.Instance.ValidarExisteDocAnulado(oDocumentoAnulado);
        //}

        //public WCFFacturacion.ListaFacturaElectronica GetListaDocumentoAnulado(WCFFacturacion.FacturaElectronica oDocumentoAnulado)
        //{
        //    return FacturacionServiceController.Instance.GetListaDocumentoAnulado(oDocumentoAnulado);
        //}


        //public WCFFacturacion.FacturaElectronica GetObtenerDocumentoUnico(WCFFacturacion.FacturaElectronica oFacturaElectronica)
        //{
        //    return FacturacionServiceController.Instance.GetObtenerDocumentoUnico(oFacturaElectronica);
        //}

        //#endregion

        //#region UTIL
        //public WCFFacturacion.ListaDocumento GetListaDocumentoCabExcel(WCFFacturacion.FacturaElectronica oFactura)
        //{
        //    return FacturacionServiceController.Instance.GetListaDocumentoCabExcel(oFactura);
        //}

        //#endregion

        //#region RESUMEN RC, RA AND RR

        //public WCFFacturacion.ListaEstado GetListaTipoFecha()
        //{
        //    return FacturacionServiceController.Instance.GetListaTipoFecha();
        //}

        //public WCFFacturacion.ListaFacturaElectronica GetListaCabeceraRC(WCFFacturacion.FacturaElectronica documentoRC)
        //{
        //    return FacturacionServiceController.Instance.GetListaCabeceraRC(documentoRC);
        //}

        //public WCFFacturacion.ListaFacturaElectronica GetListaCabeceraRA(WCFFacturacion.FacturaElectronica documentoRA)
        //{
        //    return FacturacionServiceController.Instance.GetListaCabeceraRA(documentoRA);
        //}

        //public WCFFacturacion.ListaFacturaElectronica GetListaCabeceraRR(WCFFacturacion.FacturaElectronica documentoRR)
        //{
        //    return FacturacionServiceController.Instance.GetListaCabeceraRR(documentoRR);
        //}

        //public WCFFacturacion.ListaDetalleFacturaElectronica GetListaDetalleRC(WCFFacturacion.FacturaElectronica documentoRC)
        //{
        //    return FacturacionServiceController.Instance.GetListaDetalleRC(documentoRC);
        //}

        //public WCFFacturacion.ListaDetalleFacturaElectronica GetListaDetalleRA(WCFFacturacion.FacturaElectronica documentoRA)
        //{
        //    return FacturacionServiceController.Instance.GetListaDetalleRA(documentoRA);
        //}

        //public WCFFacturacion.ListaDetalleFacturaElectronica GetListaDetalleRR(WCFFacturacion.FacturaElectronica documentoRR)
        //{
        //    return FacturacionServiceController.Instance.GetListaDetalleRR(documentoRR);
        //}

        //#endregion

        //#region ESTADO SISTEMA DOCUMENTO

        //public WCFFacturacion.ListaSistemaEstado GetListaEstadoSistema_Ok(string RucEntity)
        //{
        //    return FacturacionServiceController.Instance.GetListaEstadoSistema_Ok(RucEntity);
        //}

        //public WCFFacturacion.ListaSistemaEstado GetListaEstadoSistema_Error(string RucEntity)
        //{
        //    return FacturacionServiceController.Instance.GetListaEstadoSistema_Error(RucEntity);
        //}
        //#endregion

        //#region DOCUMENT RECIVED

        //public WCFFacturacion.ListaFacturaElectronica GetListaDocumentReceived(WCFFacturacion.FacturaElectronica oFacturaElectronica)
        //{
        //    return FacturacionServiceController.Instance.GetListaDocumentReceived(oFacturaElectronica);
        //}

        //#endregion

        //#region DOCUMENTO ENVIADO

        //public String InsertarDocumentoEnviado(WCFFacturacion.Documento odocumento)
        //{
        //    return FacturacionServiceController.Instance.InsertarDocumentoEnviado(odocumento);
        //}

        //public WCFFacturacion.ListaDocumento GetListaDocumentoEnviado(WCFFacturacion.Documento odocumento)
        //{
        //    return FacturacionServiceController.Instance.GetListaDocumentoEnviado(odocumento);
        //}

        //#endregion

        //#region CATALOGO

        //public String InsertTipoDocumentSend(WCFFacturacion.ListaEstadoEnvio ListSend)
        //{
        //    return FacturacionServiceController.Instance.InsertTipoDocumentSend(ListSend);
        //}

        //public String InsertTipoDocumentPrint(WCFFacturacion.ListaEstadoPrint ListPrint)
        //{
        //    return FacturacionServiceController.Instance.InsertTipoDocumentPrint(ListPrint);
        //}

        //public String DeleteTipoDocumentSend(WCFFacturacion.ListaEstadoEnvio ListSend)
        //{
        //    return FacturacionServiceController.Instance.DeleteTipoDocumentSend(ListSend);
        //}

        //public String DeleteTipoDocumentPrint(WCFFacturacion.ListaEstadoPrint ListPrint)
        //{
        //    return FacturacionServiceController.Instance.DeleteTipoDocumentPrint(ListPrint);
        //}

        //public WCFFacturacion.ListaEstadoEnvio GetListEstadoEnvio(int IdEstadoEnvio, string RucEmpresa)
        //{
        //    return FacturacionServiceController.Instance.GetListEstadoEnvio(IdEstadoEnvio, RucEmpresa);
        //}

        //public WCFFacturacion.ListaEstadoPrint GetListEstadoPrint(int IdEstadoPrint, string RucEmpresa)
        //{
        //    return FacturacionServiceController.Instance.GetListEstadoPrint(IdEstadoPrint, RucEmpresa);
        //}

        //#endregion

        //#region exchangerate

        //public WCFFacturacion.ListExchangeRate Get_ExchangeRate_Today(DateTime fecha)
        //{
        //    return FacturacionServiceController.Instance.Get_ExchangeRate_Today(fecha);
        //}

        //#endregion

        //#region list pendings document

        //public WCFFacturacion.ListaFacturaElectronica GetList_PendingsDocuments_RA(string ruccomp, string type)
        //{
        //    return FacturacionServiceController.Instance.GetList_PendingsDocuments_RA(ruccomp, type);
        //}

        //public WCFFacturacion.ListaFacturaElectronica GetList_PendingsDocuments_RC(string ruccomp)
        //{
        //    return FacturacionServiceController.Instance.GetList_PendingsDocuments_RC(ruccomp);
        //}

        //public WCFFacturacion.ListaFacturaElectronica GetList_PendingsDocuments_ErrorSend(string ruccomp)
        //{
        //    return FacturacionServiceController.Instance.GetList_PendingsDocuments_ErrorSend(ruccomp);
        //}

        //#endregion

        //#endregion

        //#region mantenimiento

        //#region LISTAS

        //public WCFMantenimiento.ListaSexo GetListaSexo()
        //{
        //    return MantenimientoServiceController.Instance.GetListaSexo();
        //}

        //public WCFMantenimiento.ListaEstado GetListaEstado()
        //{
        //    return MantenimientoServiceController.Instance.GetListaEstado();
        //}

        //public WCFMantenimiento.ListaEmpresa GetListaEmpresa()
        //{
        //    return MantenimientoServiceController.Instance.GetListaEmpresa();
        //}

        //public WCFMantenimiento.ListaTipoDocumentoIdentidad GetListaTipoDocumentoIdentidad()
        //{
        //    return MantenimientoServiceController.Instance.GetListaTipoDocumentoIdentidad();
        //}

        //public WCFMantenimiento.ListaPais GetListaPais()
        //{
        //    return MantenimientoServiceController.Instance.GetListaPais();
        //}

        //public WCFMantenimiento.ListaDepartamento GetListaDepartamento(Int32 IdPais)
        //{
        //    return MantenimientoServiceController.Instance.GetListaDepartamento(IdPais);
        //}

        //public WCFMantenimiento.ListaProvincia GetListaProvincia(Int32 IdDepartamento)
        //{
        //    return MantenimientoServiceController.Instance.GetListaProvincia(IdDepartamento);
        //}

        //public WCFMantenimiento.ListaDistrito GetListaDistrito(Int32 IdProvincia)
        //{
        //    return MantenimientoServiceController.Instance.GetListaDistrito(IdProvincia);
        //}


        //#endregion

        //#region LISTA DATA

        //public WCFMantenimiento.ListaEmpleados GetListaEmpleado(WCFMantenimiento.Empleado oEmpleado)
        //{
        //    return MantenimientoServiceController.Instance.GetListaEmpleado(oEmpleado);
        //}

        //public WCFMantenimiento.ListaEmpresa GetListadoEmpresa(WCFMantenimiento.Empresa oEmpresa)
        //{
        //    return MantenimientoServiceController.Instance.GetListadoEmpresa(oEmpresa);
        //}

        //#endregion

        //#region REGISTRO

        //public String RegistrarEmpleado(WCFMantenimiento.Empleado oEmpleado)
        //{
        //    return MantenimientoServiceController.Instance.RegistrarEmpleado(oEmpleado);
        //}

        //public String ActualizarEmpleado(WCFMantenimiento.Empleado oEmpleado)
        //{
        //    return MantenimientoServiceController.Instance.ActualizarEmpleado(oEmpleado);
        //}

        //public Int32 ValidarDniRuc(String Dni_Ruc)
        //{
        //    return MantenimientoServiceController.Instance.ValidarDniRuc(Dni_Ruc);
        //}

        //public WCFMantenimiento.ListaEmpresa ValidarEmpresaRuc(String Ruc_Empresa)
        //{
        //    return MantenimientoServiceController.Instance.ValidarEmpresaRuc(Ruc_Empresa);
        //}

        //public String RegistrarEmpresa(WCFMantenimiento.Empresa oEmpresa)
        //{
        //    return MantenimientoServiceController.Instance.RegistrarEmpresa(oEmpresa);
        //}

        //public String ActualizarEmpresa(WCFMantenimiento.Empresa oEmpresa)
        //{
        //    return MantenimientoServiceController.Instance.ActualizarEmpresa(oEmpresa);
        //}

        //public WCFMantenimiento.Ubigeo GetUbigeo(String CodigoUbigeo)
        //{
        //    return MantenimientoServiceController.Instance.GetUbigeo(CodigoUbigeo);
        //}

        //#endregion

        //#region MANTENIMIENTO DE CLIENTE

        //public WCFMantenimiento.ListaCliente ValidarNroClienteExiste(String NroDocumento)
        //{
        //    return MantenimientoServiceController.Instance.ValidarNroClienteExiste(NroDocumento);
        //}

        //public String InsertarCliente(WCFMantenimiento.Cliente oCliente)
        //{
        //    return MantenimientoServiceController.Instance.InsertarCliente(oCliente);
        //}

        //public String ActualizarCliente(WCFMantenimiento.Cliente oCliente)
        //{
        //    return MantenimientoServiceController.Instance.ActualizarCliente(oCliente);
        //}

        //public WCFMantenimiento.ListaCliente GetListaCliente(WCFMantenimiento.Cliente oCliente)
        //{
        //    return MantenimientoServiceController.Instance.GetListaCliente(oCliente);
        //}

        //#endregion

        //#region MANT BANK

        //public string Insert_CtaBank(WCFMantenimiento.Bank objbank)
        //{
        //    return MantenimientoServiceController.Instance.Insert_CtaBank(objbank);
        //}

        //public string Update_CtaBank(WCFMantenimiento.Bank objbank)
        //{
        //    return MantenimientoServiceController.Instance.Update_CtaBank(objbank);
        //}

        //public WCFMantenimiento.ListBank GetListBank(string RucEntity)
        //{
        //    return MantenimientoServiceController.Instance.GetListBank(RucEntity);
        //}

        //#endregion END MANT BANK

        //#endregion

        //#region seguridad

        //#region SEGURIDAD

        //public Int32 ValidarAcceso(WCFSeguridad.Usuario oUsuario)
        //{
        //    return SeguridadServiceController.Instance.ValidarAcceso(oUsuario);
        //}

        //public String ActualizarContrasenia(WCFSeguridad.Usuario oUsuario)
        //{
        //    return SeguridadServiceController.Instance.ActualizarContrasenia(oUsuario);
        //}

        //public WCFSeguridad.Usuario GetUsuarioLogeado(WCFSeguridad.Usuario oUsuario)
        //{
        //    return SeguridadServiceController.Instance.GetUsuarioLogeado(oUsuario);
        //}



        //#endregion

        //#region USUARIO DATAACCESS

        //public String RegistrarUsuario(WCFSeguridad.Usuario oUsuario)
        //{
        //    return SeguridadServiceController.Instance.RegistrarUsuario(oUsuario);
        //}

        //public String InsertarUsuario(WCFSeguridad.Usuario oUsuario)
        //{
        //    return SeguridadServiceController.Instance.InsertarUsuario(oUsuario);
        //}

        //public String ActualizarUsuario(WCFSeguridad.Usuario oUsuario)
        //{
        //    return SeguridadServiceController.Instance.ActualizarUsuario(oUsuario);
        //}

        //public WCFSeguridad.ListaUsuario GetListaUsuario(WCFSeguridad.Usuario oUsuario)
        //{
        //    return SeguridadServiceController.Instance.GetListaUsuario(oUsuario);
        //}

        //public WCFSeguridad.ListaUsuario ValidarUsername(String Username)
        //{
        //    return SeguridadServiceController.Instance.ValidarUsername(Username);
        //}

        //public WCFSeguridad.ListaUsuario ValidarDniUsuario(String Dni_Ruc)
        //{
        //    return SeguridadServiceController.Instance.ValidarDniUsuario(Dni_Ruc);
        //}

        //public WCFSeguridad.ListaUsuario InsertarUsuario_ForExcel(WCFSeguridad.ListaUsuario olistausuario)
        //{
        //    return SeguridadServiceController.Instance.InsertarUsuario_ForExcel(olistausuario);
        //}


        //public WCFSeguridad.ListaMenu GetListaMenu()
        //{
        //    return SeguridadServiceController.Instance.GetListaMenu();
        //}

        //public WCFSeguridad.ListaMenu GetListarMenuPerfil(WCFSeguridad.Perfil oPerfil)
        //{
        //    return SeguridadServiceController.Instance.GetListarMenuPerfil(oPerfil);
        //}

        //public WCFSeguridad.ListaPerfil GetListaPerfiles(string RucEntity)
        //{
        //    return SeguridadServiceController.Instance.GetListaPerfiles(RucEntity);
        //}


        //public WCFSeguridad.ListaMenu GetListarMenu(WCFSeguridad.Usuario oUsuario)
        //{
        //    return SeguridadServiceController.Instance.GetListarMenu(oUsuario);
        //}



        //public String RegistrarUsuarioPerfil(WCFSeguridad.Usuario oUsuario)
        //{
        //    return SeguridadServiceController.Instance.RegistrarUsuarioPerfil(oUsuario);
        //}


        //public String DeleteUsuarioPerfil(WCFSeguridad.Usuario oUsuario)
        //{
        //    return SeguridadServiceController.Instance.DeleteUsuarioPerfil(oUsuario);
        //}



        //public WCFSeguridad.ListaRol GetListadoRol()
        //{
        //    return SeguridadServiceController.Instance.GetListadoRol();
        //}

        //public WCFSeguridad.ListaRol GetListaRolesUsuario(WCFSeguridad.Usuario oUsuario)
        //{
        //    return SeguridadServiceController.Instance.GetListaRolesUsuario(oUsuario);
        //}

        //public String RegistrarUsuarioRol(WCFSeguridad.Usuario oUsuario)
        //{
        //    return SeguridadServiceController.Instance.RegistrarUsuarioRol(oUsuario);
        //}

        //public String DeleteUsuarioRol(WCFSeguridad.Usuario oUsuario)
        //{
        //    return SeguridadServiceController.Instance.DeleteUsuarioRol(oUsuario);
        //}

        //public Int32 ObtenerNuevoIdUsuario()
        //{
        //    return SeguridadServiceController.Instance.ObtenerNuevoIdUsuario();
        //}


        //public String RegistrarNuevoPerfil(WCFSeguridad.Perfil oPerfil)
        //{
        //    return SeguridadServiceController.Instance.RegistrarNuevoPerfil(oPerfil);
        //}

        //public String Delete_ProfileComp(WCFSeguridad.Perfil profile)
        //{
        //    return SeguridadServiceController.Instance.Delete_ProfileComp(profile);
        //}


        //public WCFSeguridad.ListaPerfil GetListaPerfil(WCFSeguridad.Perfil oPerfil)
        //{
        //    return SeguridadServiceController.Instance.GetListaPerfil(oPerfil);
        //}


        //public String InsertarMenuPerfil(WCFSeguridad.Perfil oPerfil)
        //{
        //    return SeguridadServiceController.Instance.InsertarMenuPerfil(oPerfil);
        //}


        //public String DeleteMenuPerfil(WCFSeguridad.Perfil oPerfil)
        //{
        //    return SeguridadServiceController.Instance.DeleteMenuPerfil(oPerfil);
        //}

        //public WCFSeguridad.ListaEmpleados ValidarExisteEmpleado(String DNI)
        //{
        //    return SeguridadServiceController.Instance.ValidarExisteEmpleado(DNI);
        //}

        //public String BloquearUsuario(WCFSeguridad.Usuario oUsuario)
        //{
        //    return SeguridadServiceController.Instance.BloquearUsuario(oUsuario);
        //}


        ////Correo
        //public String InsertarCorreo(WCFSeguridad.Correo oCorreo)
        //{
        //    return SeguridadServiceController.Instance.InsertarCorreo(oCorreo);
        //}

        //public String ActualizarCorreo(WCFSeguridad.Correo oCorreo)
        //{
        //    return SeguridadServiceController.Instance.ActualizarCorreo(oCorreo);
        //}


        //public String DeleteCorreo(WCFSeguridad.Correo oCorreo)
        //{
        //    return SeguridadServiceController.Instance.DeleteCorreo(oCorreo);
        //}

        //public WCFSeguridad.ListaCorreo GetListaCorreo(WCFSeguridad.Empresa oEmpresa)
        //{
        //    return SeguridadServiceController.Instance.GetListaCorreo(oEmpresa);
        //}

        //public WCFSeguridad.ListaCorreo ValidarExistsCorreoEmpresa(WCFSeguridad.Correo oCorreo)
        //{
        //    return SeguridadServiceController.Instance.ValidarExistsCorreoEmpresa(oCorreo);
        //}

        //#endregion

        //#region Ky

        //public WCFSeguridad.ListaSum41 GetKySum41ToDeep(string value1, string value2, string value3, string value4, string value5)
        //{
        //    return SeguridadServiceController.Instance.GetKySum41ToDeep(value1, value2, value3, value4, value5);
        //}


        //public WCFSeguridad.ListaEmpresa GetobjEntity(string value)
        //{
        //    return SeguridadServiceController.Instance.GetobjEntity(value);
        //}


        //public WCFSeguridad.Empresa GetobjEntitySingle(string value)
        //{
        //    return SeguridadServiceController.Instance.GetobjEntitySingle(value);
        //}

        //public WCFSeguridad.ListaEmpresa GetEntityEmpresa(string entityId)
        //{
        //    return SeguridadServiceController.Instance.GetEntityEmpresa(entityId);
        //}


        //public WCFSeguridad.AmbienteTrabjActual GetAmbienteTrabjActual(string RucEntity)
        //{
        //    return SeguridadServiceController.Instance.GetAmbienteTrabjActual(RucEntity);
        //}

        //public String UpdateAmbTrabjActual(WCFSeguridad.AmbienteTrabjActual objAmb)
        //{
        //    return SeguridadServiceController.Instance.UpdateAmbTrabjActual(objAmb);
        //}

        //public WCFSeguridad.ListaAmbienteSunat GetListAmbTrabj()
        //{
        //    return SeguridadServiceController.Instance.GetListAmbTrabj();
        //}

        //#endregion

        //#region Get Info Entity

        //public WCFSeguridad.Empresa GetCredentialEntity(string RucEmpresa)
        //{
        //    return SeguridadServiceController.Instance.GetCredentialEntity(RucEmpresa);
        //}

        //public WCFSeguridad.ListaEmail GetListTypeMailEntity()
        //{
        //    return SeguridadServiceController.Instance.GetListTypeMailEntity();
        //}

        //public WCFSeguridad.ListSSL GetListUseProt_SSL()
        //{
        //    return SeguridadServiceController.Instance.GetListUseProt_SSL();
        //}

        //#endregion

        //#region AUDITORIA

        //public WCFSeguridad.Usuario GetDataFromUserLogueo(string Username)
        //{
        //    return SeguridadServiceController.Instance.GetDataFromUserLogueo(Username);
        //}

        //public Int32 InsertRegistroLogueo(WCFSeguridad.LogueoClass ObjLogeo)
        //{
        //    return SeguridadServiceController.Instance.InsertRegistroLogueo(ObjLogeo);
        //}

        //public String UpdateRegistroLogueo(WCFSeguridad.LogueoClass ObjLogeo)
        //{
        //    return SeguridadServiceController.Instance.UpdateRegistroLogueo(ObjLogeo);
        //}

        //public String DeleteRegistroLogueox3M(string FechaDesde, string RucEntity)
        //{
        //    return SeguridadServiceController.Instance.DeleteRegistroLogueox3M(FechaDesde, RucEntity);
        //}

        //public WCFSeguridad.ListaLogueoClass GetListLogueoClass(WCFSeguridad.LogueoClass objLogueo)
        //{
        //    return SeguridadServiceController.Instance.GetListLogueoClass(objLogueo);
        //}

        //public Int32 InsertLogLogueo(WCFSeguridad.LogueoClass ObjLogeo)
        //{
        //    return SeguridadServiceController.Instance.InsertLogLogueo(ObjLogeo);
        //}

        //public WCFSeguridad.ListaLogueoClass GetListLogLogueo(WCFSeguridad.LogueoClass objlog)
        //{
        //    return SeguridadServiceController.Instance.GetListLogLogueo(objlog);
        //}

        //#endregion

        //#region LOG ADE

        //public Int32 InsertLogADE(WCFSeguridad.LogAde objlog)
        //{
        //    return SeguridadServiceController.Instance.InsertLogADE(objlog);
        //}

        //public WCFSeguridad.ListaLogAde GetListLogADE(WCFSeguridad.LogAde objlog)
        //{
        //    return SeguridadServiceController.Instance.GetListLogADE(objlog);
        //}

        //#endregion

        //#region UTIL LOG

        //public WCFSeguridad.ListaTipoLog GetListTipoLog()
        //{
        //    return SeguridadServiceController.Instance.GetListTipoLog();
        //}

        //#endregion

        //#region SEDE

        //public WCFSeguridad.ListaSede GetListSede(string RucEntity)
        //{
        //    return SeguridadServiceController.Instance.GetListSede(RucEntity);
        //}

        //#endregion

        //#region user change company
        //public String Update_UserCompany(WCFSeguridad.Usuario ouser)
        //{
        //    return SeguridadServiceController.Instance.Update_UserCompany(ouser);
        //}

        //public WCFSeguridad.ListaPerfil GetList_ProfileCompany(int IdComp)
        //{
        //    return SeguridadServiceController.Instance.GetList_ProfileCompany(IdComp);
        //}

        //public WCFSeguridad.ListAuthenticate Get_ListAuthenticate()
        //{
        //    return SeguridadServiceController.Instance.Get_ListAuthenticate();
        //}

        //#endregion

        //#region mail to alert

        //public WCFSeguridad.ListaCorreo GetList_NotificationsMail(string ruccomp)
        //{
        //    return SeguridadServiceController.Instance.GetList_NotificationsMail(ruccomp);
        //}

        //public String Insert_MailToAlert(WCFSeguridad.Correo oCorreo)
        //{
        //    return SeguridadServiceController.Instance.Insert_MailToAlert(oCorreo);
        //}

        //public String Update_MailToAlert(WCFSeguridad.Correo oCorreo)
        //{
        //    return SeguridadServiceController.Instance.Update_MailToAlert(oCorreo);
        //}

        //public String Delete_MailToAlert(WCFSeguridad.Correo oCorreo)
        //{
        //    return SeguridadServiceController.Instance.Delete_MailToAlert(oCorreo);
        //}

        //#endregion


        //#endregion

    }
}
