using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Slin.Facturacion.BusinessEntities.InterfaceXMLNOTE
{
    [XmlRoot(ElementName = "NoteClass")]
    public class NoteClass
    {
        [XmlElement(ElementName = "CE")]
        public CE CE { get; set; }
        [XmlElement(ElementName = "CABECERAPRINCIPAL")]
        public CABECERAPRINCIPAL CABECERAPRINCIPAL { get; set; }
        [XmlElement(ElementName = "CABECERANOTA")]
        public CABECERANOTA CABECERANOTA { get; set; }
        [XmlElement(ElementName = "CABECERAEMISOR")]
        public CABECERAEMISOR CABECERAEMISOR { get; set; }
        [XmlElement(ElementName = "CABECERARECEPTOR")]
        public CABECERARECEPTOR CABECERARECEPTOR { get; set; }
        [XmlElement(ElementName = "TOTAL")]
        public TOTAL TOTAL { get; set; }
        [XmlElement(ElementName = "LINEAS")]
        public LINEAS LINEAS { get; set; }

        [XmlElement(ElementName = "REFERENCIAS")]
        public REFERENCIAS REFERENCIAS { get; set; }

        [XmlElement(ElementName = "DOCAFECTADO")]
        public DOCAFECTADO DOCAFECTADO { get; set; }

        [XmlElement(ElementName = "ANTICIPOS")]
        public ANTICIPOS ANTICIPOS { get; set; }
        [XmlElement(ElementName = "DETRACCION")]
        public DETRACCION DETRACCION { get; set; }
        [XmlElement(ElementName = "FACTGUIA")]
        public FACTGUIA FACTGUIA { get; set; }
        [XmlElement(ElementName = "LEYENDAS")]
        public LEYENDAS LEYENDAS { get; set; }
        [XmlElement(ElementName = "EXTRAS")]
        public EXTRAS EXTRAS { get; set; }
        [XmlElement(ElementName = "MAIL")]
        public MAIL MAIL { get; set; }
        [XmlElement(ElementName = "ADICIONAL")]
        public ADICIONAL ADICIONAL { get; set; }
    }



    [XmlRoot(ElementName = "CE")]
    public class CE
    {
        [XmlElement(ElementName = "ID")]
        public string ID { get; set; }
    }

    [XmlRoot(ElementName = "CABECERAPRINCIPAL")]
    public class CABECERAPRINCIPAL
    {
        [XmlElement(ElementName = "TipoCE")]
        public string TipoCE { get; set; }
        [XmlElement(ElementName = "ID_CE")]
        public string ID_CE { get; set; }
        [XmlElement(ElementName = "Toperacion")]
        public string Toperacion { get; set; }
        [XmlElement(ElementName = "FEmision")]
        public string FEmision { get; set; }
        [XmlElement(ElementName = "TMoneda")]
        public string TMoneda { get; set; }
        [XmlElement(ElementName = "NroPlaca")]
        public string NroPlaca { get; set; }
        [XmlElement(ElementName = "FecVenc")]
        public string FecVenc { get; set; }
    }


    [XmlRoot(ElementName = "CABECERANOTA")]
    public class CABECERANOTA
    {
        [XmlElement(ElementName = "TpoNota")]
        public string TpoNota { get; set; }
        [XmlElement(ElementName = "MotivoNota")]
        public string MotivoNota { get; set; }
    }

    [XmlRoot(ElementName = "CABECERAEMISOR")]
    public class CABECERAEMISOR
    {
        [XmlElement(ElementName = "EmiTpoDoc")]
        public string EmiTpoDoc { get; set; }
        [XmlElement(ElementName = "EmiNumDoc")]
        public string EmiNumDoc { get; set; }
        [XmlElement(ElementName = "EmiNombre")]
        public string EmiNombre { get; set; }
        [XmlElement(ElementName = "EmiNComer")]
        public string EmiNComer { get; set; }
        [XmlElement(ElementName = "EmiUbigeo")]
        public string EmiUbigeo { get; set; }
        [XmlElement(ElementName = "EmiDirFiscal")]
        public string EmiDirFiscal { get; set; }
        [XmlElement(ElementName = "EmiDirUrbani")]
        public string EmiDirUrbani { get; set; }
        [XmlElement(ElementName = "EmiDirProvin")]
        public string EmiDirProvin { get; set; }
        [XmlElement(ElementName = "EmiDirDepart")]
        public string EmiDirDepart { get; set; }
        [XmlElement(ElementName = "EmiDirDistrito")]
        public string EmiDirDistrito { get; set; }
        [XmlElement(ElementName = "EmiCodPais")]
        public string EmiCodPais { get; set; }
    }

    [XmlRoot(ElementName = "CABECERARECEPTOR")]
    public class CABECERARECEPTOR
    {
        [XmlElement(ElementName = "RecTpoDoc")]
        public string RecTpoDoc { get; set; }
        [XmlElement(ElementName = "RecNumDocu")]
        public string RecNumDocu { get; set; }
        [XmlElement(ElementName = "RecNombre")]
        public string RecNombre { get; set; }
        [XmlElement(ElementName = "RecNComer")]
        public string RecNComer { get; set; }
        [XmlElement(ElementName = "RecUbigeo")]
        public string RecUbigeo { get; set; }
        [XmlElement(ElementName = "RecDirFiscal")]
        public string RecDirFiscal { get; set; }
        [XmlElement(ElementName = "RecDirUrbani")]
        public string RecDirUrbani { get; set; }
        [XmlElement(ElementName = "RecDirProvin")]
        public string RecDirProvin { get; set; }
        [XmlElement(ElementName = "RecDirDepart")]
        public string RecDirDepart { get; set; }
        [XmlElement(ElementName = "RecDirDistrito")]
        public string RecDirDistrito { get; set; }
        [XmlElement(ElementName = "RecCodPais")]
        public string RecCodPais { get; set; }
        [XmlElement(ElementName = "LugUbigeo")]
        public string LugUbigeo { get; set; }
        [XmlElement(ElementName = "LugDirFiscal")]
        public string LugDirFiscal { get; set; }
        [XmlElement(ElementName = "LugDirUrbani")]
        public string LugDirUrbani { get; set; }
        [XmlElement(ElementName = "LugDirProvin")]
        public string LugDirProvin { get; set; }
        [XmlElement(ElementName = "LugDirDepart")]
        public string LugDirDepart { get; set; }
        [XmlElement(ElementName = "LugDirDistrito")]
        public string LugDirDistrito { get; set; }
        [XmlElement(ElementName = "LugCodPais")]
        public string LugCodPais { get; set; }
        [XmlElement(ElementName = "LocalAnexo")]
        public string LocalAnexo { get; set; }
    }

    [XmlRoot(ElementName = "TOTAL")]
    public class TOTAL
    {
        [XmlElement(ElementName = "TotVtaGrab")]
        public string TotVtaGrab { get; set; }
        [XmlElement(ElementName = "TotVtaInaf")]
        public string TotVtaInaf { get; set; }
        [XmlElement(ElementName = "TotVtaExon")]
        public string TotVtaExon { get; set; }
        [XmlElement(ElementName = "TotVtaGrat")]
        public string TotVtaGrat { get; set; }
        [XmlElement(ElementName = "TotTotDscto")]
        public string TotTotDscto { get; set; }
        [XmlElement(ElementName = "TotSumIGV")]
        public string TotSumIGV { get; set; }
        [XmlElement(ElementName = "TotSumISC")]
        public string TotSumISC { get; set; }
        [XmlElement(ElementName = "TotSumOTrib")]
        public string TotSumOTrib { get; set; }
        [XmlElement(ElementName = "TotSumOCargo")]
        public string TotSumOCargo { get; set; }
        [XmlElement(ElementName = "TotDctoGlobal")]
        public string TotDctoGlobal { get; set; }
        [XmlElement(ElementName = "TotAnticipo")]
        public string TotAnticipo { get; set; }
        [XmlElement(ElementName = "TotImporTotal")]
        public string TotImporTotal { get; set; }
        [XmlElement(ElementName = "MontoLiteral")]
        public string MontoLiteral { get; set; }
    }

    [XmlRoot(ElementName = "LINEA")]
    public class LINEA
    {
        [XmlElement(ElementName = "LnNroOrden")]
        public string LnNroOrden { get; set; }
        [XmlElement(ElementName = "LnUndMed")]
        public string LnUndMed { get; set; }
        [XmlElement(ElementName = "LnCantidad")]
        public string LnCantidad { get; set; }
        [XmlElement(ElementName = "LnCodProd")]
        public string LnCodProd { get; set; }
        [XmlElement(ElementName = "LnDescrip")]
        public string LnDescrip { get; set; }
        [XmlElement(ElementName = "LnValUnit")]
        public string LnValUnit { get; set; }
        [XmlElement(ElementName = "LnMntPrcVta")]
        public string LnMntPrcVta { get; set; }
        [XmlElement(ElementName = "LnValVta")]
        public string LnValVta { get; set; }
        [XmlElement(ElementName = "LnMntIGV")]
        public string LnMntIGV { get; set; }
        [XmlElement(ElementName = "LnCodAfectIGV")]
        public string LnCodAfectIGV { get; set; }
        [XmlElement(ElementName = "LnMntISC")]
        public string LnMntISC { get; set; }
        [XmlElement(ElementName = "LnCodSisISC")]
        public string LnCodSisISC { get; set; }
        [XmlElement(ElementName = "LnDescMnto")]
        public string LnDescMnto { get; set; }

        [XmlElement(ElementName = "LnPorcIGV")]
        public string LnPorcIGV { get; set; }
    }

    [XmlRoot(ElementName = "LINEAS")]
    public class LINEAS
    {
        [XmlElement(ElementName = "LINEA")]
        public List<LINEA> LINEA { get; set; }
    }

    [XmlRoot(ElementName = "DOCUMENTO")]
    public class DOCUMENTO
    {
        [XmlElement(ElementName = "DocNroOrden")]
        public string DocNroOrden { get; set; }
        [XmlElement(ElementName = "DocID")]
        public string DocID { get; set; }
        [XmlElement(ElementName = "DocTpoDoc")]
        public string DocTpoDoc { get; set; }
    }

    [XmlRoot(ElementName = "DOCAFECTADO")]
    public class DOCAFECTADO
    {
        [XmlElement(ElementName = "DOCUMENTO")]
        public List<DOCUMENTO> DOCUMENTO { get; set; }
    }

    [XmlRoot(ElementName = "REFERENCIA")]
    public class REFERENCIA
    {
        [XmlElement(ElementName = "RefNroOrden")]
        public string RefNroOrden { get; set; }
        [XmlElement(ElementName = "RefID")]
        public string RefID { get; set; }
        [XmlElement(ElementName = "RefTpoDoc")]
        public string RefTpoDoc { get; set; }
    }

    [XmlRoot(ElementName = "REFERENCIAS")]
    public class REFERENCIAS
    {
        [XmlElement(ElementName = "REFERENCIA")]
        public List<REFERENCIA> REFERENCIA { get; set; }
    }

    [XmlRoot(ElementName = "ANTICIPO")]
    public class ANTICIPO
    {
        [XmlElement(ElementName = "AntNroOrden")]
        public string AntNroOrden { get; set; }
        [XmlElement(ElementName = "AntMonto")]
        public string AntMonto { get; set; }
        [XmlElement(ElementName = "AntTpoDocAnt")]
        public string AntTpoDocAnt { get; set; }
        [XmlElement(ElementName = "AntIdDocAnt")]
        public string AntIdDocAnt { get; set; }
        [XmlElement(ElementName = "AntNumDocEmi")]
        public string AntNumDocEmi { get; set; }
        [XmlElement(ElementName = "AntTpoDocEmi")]
        public string AntTpoDocEmi { get; set; }
    }

    [XmlRoot(ElementName = "ANTICIPOS")]
    public class ANTICIPOS
    {
        [XmlElement(ElementName = "ANTICIPO")]
        public List<ANTICIPO> ANTICIPO { get; set; }
    }

    [XmlRoot(ElementName = "DETRACCION")]
    public class DETRACCION
    {
        [XmlElement(ElementName = "DetValBBSS")]
        public string DetValBBSS { get; set; }
        [XmlElement(ElementName = "DetCtaBN")]
        public string DetCtaBN { get; set; }
        [XmlElement(ElementName = "DetPorcent")]
        public string DetPorcent { get; set; }
        [XmlElement(ElementName = "DetMonto")]
        public string DetMonto { get; set; }
    }

    [XmlRoot(ElementName = "FACTGUIA")]
    public class FACTGUIA
    {
        [XmlElement(ElementName = "DatoNroDoc")]
        public string DatoNroDoc { get; set; }
        [XmlElement(ElementName = "DatoCodTpoDoc")]
        public string DatoCodTpoDoc { get; set; }
        [XmlElement(ElementName = "DatoTpoDoc")]
        public string DatoTpoDoc { get; set; }
        [XmlElement(ElementName = "DatoNumDocRem")]
        public string DatoNumDocRem { get; set; }
        [XmlElement(ElementName = "DestNumDoc")]
        public string DestNumDoc { get; set; }
        [XmlElement(ElementName = "DestTpoDoc")]
        public string DestTpoDoc { get; set; }
        [XmlElement(ElementName = "DestRazSoc")]
        public string DestRazSoc { get; set; }
        [XmlElement(ElementName = "TrasMotivo")]
        public string TrasMotivo { get; set; }
        [XmlElement(ElementName = "TrasPeso")]
        public string TrasPeso { get; set; }
        [XmlElement(ElementName = "TrasUndMed")]
        public string TrasUndMed { get; set; }
        [XmlElement(ElementName = "TrasModalidad")]
        public string TrasModalidad { get; set; }
        [XmlElement(ElementName = "TrasFecInicio")]
        public string TrasFecInicio { get; set; }
        [XmlElement(ElementName = "TranIDDoc")]
        public string TranIDDoc { get; set; }
        [XmlElement(ElementName = "TranTpoDoc")]
        public string TranTpoDoc { get; set; }
        [XmlElement(ElementName = "TranRazSoc")]
        public string TranRazSoc { get; set; }
        [XmlElement(ElementName = "CondIDDoc")]
        public string CondIDDoc { get; set; }
        [XmlElement(ElementName = "CondTpoDoc")]
        public string CondTpoDoc { get; set; }
        [XmlElement(ElementName = "VehiConstancia")]
        public string VehiConstancia { get; set; }
        [XmlElement(ElementName = "VehiPlaca")]
        public string VehiPlaca { get; set; }
        [XmlElement(ElementName = "DirLlegUbigeo")]
        public string DirLlegUbigeo { get; set; }
        [XmlElement(ElementName = "DirLlegDireccion")]
        public string DirLlegDireccion { get; set; }
        [XmlElement(ElementName = "DirParUbigeo")]
        public string DirParUbigeo { get; set; }
        [XmlElement(ElementName = "DirParDireccion")]
        public string DirParDireccion { get; set; }
    }

    [XmlRoot(ElementName = "LEYENDA")]
    public class LEYENDA
    {
        [XmlElement(ElementName = "LeyNroOrden")]
        public string LeyNroOrden { get; set; }
        [XmlElement(ElementName = "LeyCodigo")]
        public string LeyCodigo { get; set; }
        [XmlElement(ElementName = "LeyDescrip")]
        public string LeyDescrip { get; set; }
    }

    [XmlRoot(ElementName = "LEYENDAS")]
    public class LEYENDAS
    {
        [XmlElement(ElementName = "LEYENDA")]
        public List<LEYENDA> LEYENDA { get; set; }
    }

    [XmlRoot(ElementName = "EXTRA")]
    public class EXTRA
    {
        [XmlElement(ElementName = "ExID")]
        public string ExID { get; set; }
        [XmlElement(ElementName = "ExDato")]
        public string ExDato { get; set; }
        [XmlElement(ElementName = "ExTpoDato")]
        public string ExTpoDato { get; set; }
    }

    [XmlRoot(ElementName = "EXTRAS")]
    public class EXTRAS
    {
        [XmlElement(ElementName = "EXTRA")]
        public List<EXTRA> EXTRA { get; set; }
    }

    [XmlRoot(ElementName = "MAIL")]
    public class MAIL
    {
        [XmlElement(ElementName = "Para")]
        public string Para { get; set; }
        [XmlElement(ElementName = "CC")]
        public string CC { get; set; }
        [XmlElement(ElementName = "CCO")]
        public string CCO { get; set; }
    }

    [XmlRoot(ElementName = "ADICIONAL")]
    public class ADICIONAL
    {
        [XmlElement(ElementName = "Sede")]
        public string Sede { get; set; }
        [XmlElement(ElementName = "Usuario")]
        public string Usuario { get; set; }
        [XmlElement(ElementName = "Impresora")]
        public string Impresora { get; set; }
        [XmlElement(ElementName = "Campo1")]
        public string Campo1 { get; set; }
        [XmlElement(ElementName = "Campo2")]
        public string Campo2 { get; set; }
        [XmlElement(ElementName = "Campo3")]
        public string Campo3 { get; set; }
        [XmlElement(ElementName = "Campo4")]
        public string Campo4 { get; set; }
        [XmlElement(ElementName = "Campo5")]
        public string Campo5 { get; set; }
        [XmlElement(ElementName = "Campo6")]
        public string Campo6 { get; set; }
        [XmlElement(ElementName = "Campo7")]
        public string Campo7 { get; set; }
        [XmlElement(ElementName = "Campo8")]
        public string Campo8 { get; set; }
        [XmlElement(ElementName = "Campo9")]
        public string Campo9 { get; set; }
        [XmlElement(ElementName = "Campo10")]
        public string Campo10 { get; set; }
    }
}
