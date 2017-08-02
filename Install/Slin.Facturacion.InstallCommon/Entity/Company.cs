using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Slin.Facturacion.InstallCommon
{
    public class Company
    {
        public int IdCompany { get; set; }
        public string CodCompany { get; set; }
        public int IdUbi { get; set; }
        public string Ubi { get; set; }
        public string Ruc { get; set; }
        public string RazonSocial { get; set; }
        public string RazonComercial { get; set; }
        public string Telefono { get; set; }
        public string Fax { get; set; }
        public string Direccion { get; set; }
        public string DomicilioFiscal { get; set; }
        public string Urbanizacion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string PaginaWeb { get; set; }
        public string Email { get; set; }
        public int IdEstado { get; set; }
        public int Id_TDI { get; set; }
        public string TpoLogin { get; set; }
        public string Name_ServerAD { get; set; }
        public string Us_Cert { get; set; }
        public string Pwd_Cert { get; set; }      
        public string Protocolo { get; set; }
        public string UserAdmin { get; set; }
        public string Url_CompanyLogo { get; set; }
        public string Url_CompanyConsult { get; set; }
        //public string ExpirationDate_Cert { get; set; }
    }

    public class ListCompany : List<Company>
    {

    }

    public class User
    {
        public int IdUsuario { get; set; }
        public int IdEstado { get; set; }
        public int IdCompany { get; set; }
        public int IdPerfil { get; set; }
        public string Nombres { get; set; }
        public string ApePaterno { get; set; }
        public string ApeMaterno { get; set; }
        public string DNI { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public string Sede { get; set; }
    }

    public class ListUser : List<User>
    {

    }


    public class Menu
    {
        public int IdMenu { get; set; }
        public int IdPerfil { get; set; }
        public string NombreMenu { get; set; }
    }

    public class ListMenu : List<Menu>
    {

    }

    public class Ambiente
    {
        public int IdAmb { get; set; }
        public string NameAmbiente { get; set; }
        public string Cod { get; set; }
    }

    public class ListAmbiente : List<Ambiente>
    {

    }

    public class TypeDocument
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public int IdAmbient { get; set; }
        public int IdEstado { get; set; }
        public string RucCompany { get; set; }
        public string Descripcion { get; set; }

    }

    public class ListTypeDocument : List<TypeDocument>
    {

    }


    public class CredentialCertificate
    {
        public string RucCompany { get; set; }
        public string NombreUsuario { get; set; }
        public string Password { get; set; }
        public int IdAmb { get; set; }
        public string ExpirationDate { get; set; }
        public string NameCertificate { get; set; }
        public int Id { get; set; }
    }

    public class ListCredentialCertificate : List<CredentialCertificate>
    {

    }

    public class Correo
    {

        public int IdEmail { get; set; }
        public string Mensaje { get; set; }
        public string Asunto { get; set; }
        public string EmailDestino { get; set; }
        public string EmailSalida { get; set; }
        public string Password { get; set; }
        public int IdCompany { get; set; }
        public string RucCompany { get; set; }
        public string Email { get; set; }
        public string Domain { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        public int Port_Alternative { get; set; }
        public int IdEstado { get; set; }
        public string TypeMail { get; set; }
        public int IdSSL { get; set; }
        public string CodeSSL { get; set; }
        public string DescriptionSSL { get; set; }
    }

    public class ListCorreo : List<Correo>
    {

    }

    public class Rol
    {
        public int IdRol { get; set; }
        public string NombreRol { get; set; }
        public string CodigoRol { get; set; }
    }

    public class ListRol : List<Rol>
    {

    }

    public class Perfil
    {
        public int IdPerfil { get; set; }
        public string NombrePerfil { get; set; }
        public string Codigo { get; set; }
        public string RucCompany { get; set; }
    }

    public class ListPerfil : List<Perfil>
    {

    }

    public class UserRol
    {
        public int IdRol { get; set; }
        public string Dni_Ruc { get; set; }
    }

    public class ListUserRol : List<UserRol>
    {

    }

    public class MenuPerfil
    {
        public int IdMenu { get; set; }
        public int IdPerfil { get; set; }
    }

    public class ListMenuPerfil : List<MenuPerfil>
    {

    }

    public class ConfigMain
    {
        public string TAB { get; set; }
        public string NOM { get; set; }
        public string POS { get; set; }
        public string VAL { get; set; }
        public string MND { get; set; }
        public string DOC { get; set; }
        public string MSG { get; set; }
        public string ECV { get; set; }
        public string ECN { get; set; }
    }

    public class ListConfigMain : List<ConfigMain>
    {

    }

    public class Web_App
    {
        public string Name { get; set; }
        public string Port { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public string Protocolo { get; set; }
    }

    public class ListWeb_App : List<Web_App>
    {

    }


    public class ConectionClass
    {
        public string Host { get; set; }
        public string DBName { get; set; }
        public string UserDB { get; set; }
        public string PwdDB { get; set; }
    }



    [XmlRoot(ElementName = "Service")]
    public class Service
    {
        [XmlElement(ElementName = "Type")]
        public string Type { get; set; }
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "Description")]
        public string Description { get; set; }
        [XmlElement(ElementName = "Description_Res")]
        public string Description_Res { get; set; }
        [XmlElement(ElementName = "Path_Exe")]
        public string Path_Exe { get; set; }
        [XmlElement(ElementName = "Hability")]
        public string Hability { get; set; }
        [XmlElement(ElementName = "ProcessExe")]
        public string ProcessExe { get; set; }

        [XmlElement(ElementName = "CodeService")]
        public string CodeService { get; set; }
        [XmlElement(ElementName = "SubType")]
        public string SubType { get; set; }

        public string ValueTime { get; set; }
        public string IntervalValue { get; set; }
        public int MaxNumAttempts { get; set; }
        public string RucEntity { get; set; }
        public int IdEstado { get; set; }
        public string NameService { get; set; }
    }

    [XmlRoot(ElementName = "Services")]
    public class Services
    {
        [XmlElement(ElementName = "Service")]
        public List<Service> Service { get; set; }
    }

    public class ListSevices : List<Service>
    {

    }
}
