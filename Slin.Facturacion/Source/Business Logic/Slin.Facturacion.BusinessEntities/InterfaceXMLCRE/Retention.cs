using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Slin.Facturacion.BusinessEntities.InterfaceXMLCRE
{
    [XmlRoot("Retention")]
    public class Retention
    {
        [XmlElement("CE")]
        public CE CE { get; set; }

        [XmlElement("CABECERAPRINCIPAL")]
        public CabeceraPrincipal CabPrincipal { get; set; }

        [XmlElement("CABECERAEMISOR")]
        public Emisor Emisor { get; set; }

        [XmlElement("CABECERARECEPTOR")]
        public Receptor Receptor { get; set; }

        [XmlElement("DATOCE")]
        public DatosCE DatosCE { get; set; }

        [XmlElement("ITEMS")]
        public Items Items { get; set; }

        //INVOICE //
        [XmlElement("TOTAL")]
        public Total Total { get; set; }

        [XmlElement("LINEAS")]
        public Lineas Lineas { get; set; }

        [XmlElement("REFERENCIAS")]
        public Referencias Referencias { get; set; }



        //[XmlElement("Item")]
        //public Detalle Item { get; set; }

        //[XmlElement("Detalle")]
        //public Detalle Detalle { get; set; }

        //[XmlElement("Referencia")]
        //public Referencia Referencia { get; set; }


        [XmlElement("PERCEPCION")]
        public Percepcion Percepcion { get; set; }

        [XmlElement("CABECERANOTA")]
        public CabeceraNota CabNota { get; set; }

        [XmlElement("EXTRAS")]
        public Extras Extras { get; set; }


        [XmlElement("AFECTADOS")]
        public Afectados Afectados { get; set; }

        [XmlElement("ANTICIPOS")]
        public Anticipos Anticipos { get; set; }


        [XmlElement("MAIL")]
        public Mail Mail { get; set; }

        [XmlElement("ADICIONAL")]
        public Adicional Adicional { get; set; }
    }


    [XmlRoot("CE")]
    public class CE
    {
        [XmlElement("ID")]
        public string ID { get; set; }
    }


    [XmlRoot("CABECERAPRINCIPAL")]
    public class CabeceraPrincipal
    {
        [XmlElement("TipoCE")]
        public string TipoCE { get; set; }

        [XmlElement("ID_CE")]
        public string ID_CE { get; set; }

        [XmlElement("FEmision")]
        public string FEmision { get; set; }

        [XmlElement("TMoneda")]
        public string TMoneda { get; set; }


    }


    [XmlRoot("CABECERAEMISOR")]
    public class Emisor
    {
        //**** Datos //
        [XmlElement("EmiTpoDoc")]
        public string EmiTpoDoc { get; set; }

        [XmlElement("EmiNumDocu")]
        public string EmiNumDocu { get; set; }

        [XmlElement("EmiNombre")]
        public string EmiNombre { get; set; }

        [XmlElement("EmiNComer")]
        public string EmiNComer { get; set; }

        [XmlElement("EmiUbigeo")]
        public string EmiUbigeo { get; set; }

        [XmlElement("EmiDirFiscal")]
        public string EmiDirFiscal { get; set; }

        [XmlElement("EmiDirUrbani")]
        public string EmiDirUrbani { get; set; }

        [XmlElement("EmiDirProvin")]
        public string EmiDirProvin { get; set; }

        [XmlElement("EmiDirDepart")]
        public string EmiDirDepart { get; set; }

        [XmlElement("EmiDirDistrito")]
        public string EmiDirDistrito { get; set; }

        [XmlElement("EmiCodPais")]
        public string EmiCodPais { get; set; }


    }

    [XmlRoot("CABECERARECEPTOR")]
    public class Receptor
    {
        //** DATOS **//

        [XmlElement("RecTpoDoc")]
        public string RecTpoDoc { get; set; }

        [XmlElement("RecNumDocu")]
        public string RecNumDocu { get; set; }

        [XmlElement("RecNombre")]
        public string RecNombre { get; set; }

        [XmlElement("RecNComer")]
        public string RecNComer { get; set; }

        [XmlElement("RecUbigeo")]
        public string RecUbigeo { get; set; }

        [XmlElement("RecDirFiscal")]
        public string RecDirFiscal { get; set; }

        [XmlElement("RecDirUrbani")]
        public string RecDirUrbani { get; set; }

        [XmlElement("RecDirProvin")]
        public string RecDirProvin { get; set; }

        [XmlElement("RecDirDepart")]
        public string RecDirDepart { get; set; }

        [XmlElement("RecDirDistrito")]
        public string RecDirDistrito { get; set; }

        [XmlElement("RecCodPais")]
        public string RecCodPais { get; set; }
    }

    [XmlRoot("Linea")]
    public class Linea
    {
        [XmlElement("LnNroOrden")]
        public string LnNroOrden { get; set; }

        [XmlElement("LnUndMed")]
        public string LnUndMed { get; set; }

        [XmlElement("LnCantidad")]
        public string LnCantidad { get; set; }

        [XmlElement("LnCodProd")]
        public string LnCodProd { get; set; }

        [XmlElement("LnDescrip")]
        public string LnDescrip { get; set; }

        [XmlElement("LnValUnit")]
        public string LnValUnit { get; set; }

        [XmlElement("LnMntPrcVta")]
        public string LnMntPrcVta { get; set; }

        [XmlElement("LnValVta")]
        public string LnValVta { get; set; }

        [XmlElement("LnMntIGV")]
        public string LnMntIGV { get; set; }

        [XmlElement("LnCodAfecIGV")]
        public string LnCodAfecIGV { get; set; }

        [XmlElement("LnMntISC")]
        public string LnMntISC { get; set; }

        [XmlElement("LnCodSisISC")]
        public string LnCodSisISC { get; set; }

        [XmlElement("LnDescMnto")]
        public string LnDescMnto { get; set; }


        //[XmlElement("ItMontIGV")]
        //public string ItMontIGV { get; set; }

        //[XmlElement("ItCodAfecIGV")]
        //public string ItCodAfecIGV { get; set; }

        //[XmlElement("ItMontISC")]
        //public string ItMontISC { get; set; }

        //[XmlElement("ItCodAfecISC")]
        //public string ItCodAfecISC { get; set; }

        //[XmlElement("ItDescMnto")]
        //public string ItDescMnto { get; set; }

    }

    public class Lineas
    {
        [XmlElement("LINEA")]
        public List<Linea> ListLinea = new List<Linea>();
    }

    [XmlRoot("TOTAL")]
    public class Total
    {
        //[XmlElement("TotVtaGrab")]
        //public string TotVtaGrab { get; set; }

        //[XmlElement("TotVtaInaf")]
        //public string TotVtaInaf { get; set; }

        //[XmlElement("TotVtaExon")]
        //public string TotVtaExon { get; set; }

        //[XmlElement("TotVtaGrat")]
        //public string TotVtaGrat { get; set; }

        //[XmlElement("TotTotDscto")]
        //public string TotTotDscto { get; set; }

        //[XmlElement("TotSumIGV")]
        //public string TotSumIGV { get; set; }

        //[XmlElement("TotSumISC")]
        //public string TotSumISC { get; set; }

        //[XmlElement("TotSumOTrib")]
        //public string TotSumOTrib { get; set; }

        //[XmlElement("TotDctoGlobal")]
        //public string TotDctoGlobal { get; set; }

        //[XmlElement("TotSumOCargo")]
        //public string TotSumOCargo { get; set; }

        //[XmlElement("TotAnticipo")]
        //public string TotAnticipo { get; set; }

        //[XmlElement("TotImporTotal")]
        //public string TotImporTotal { get; set; }

        //[XmlElement("MontoLiteral")]
        //public string MontoLiteral { get; set; }


        [XmlElement("TotVtaGrab")]
        public string TotVtaGrab { get; set; }

        [XmlElement("TotVtaInaf")]
        public string TotVtaInaf { get; set; }

        [XmlElement("TotVtaExon")]
        public string TotVtaExon { get; set; }

        [XmlElement("TotVtaGrat")]
        public string TotVtaGrat { get; set; }

        [XmlElement("TotVtaDscto")]
        public string TotVtaDscto { get; set; }

        [XmlElement("TotSumIGV")]
        public string TotSumIGV { get; set; }

        [XmlElement("TotSumISC")]
        public string TotSumISC { get; set; }

        [XmlElement("TotSumOTrib")]
        public string TotSumOTrib { get; set; }

        [XmlElement("TotDsctoGlobal")]
        public string TotDsctoGlobal { get; set; }

        [XmlElement("TotSumOCargo")]
        public string TotSumOCargo { get; set; }

        [XmlElement("TotAnticipo")]
        public string TotAnticipo { get; set; }

        [XmlElement("TotImporTotal")]
        public string TotImporTotal { get; set; }

        [XmlElement("MontoLiteral")]
        public string MontoLiteral { get; set; }
    }



    [XmlRoot("AFECTADO")]
    public class Afectado
    {
        [XmlElement("DocuAfeID")]
        public string DocuAfeID { get; set; }

        [XmlElement("TipoCE")]
        public string TipoCE { get; set; }

        [XmlElement("ID_CE")]
        public string ID_CE { get; set; }
    }

    public class Afectados
    {
        [XmlElement("AFECTADOS")]
        public List<Afectado> ListAfectado = new List<Afectado>();
    }


    [XmlRoot("DATOCE")]
    public class DatosCE
    {
        [XmlElement("RegimenCE")]
        public string RegimenCE { get; set; }

        [XmlElement("TasaCE")]
        public string TasaCE { get; set; }

        [XmlElement("ObsCE")]
        public string ObsCE { get; set; }

        [XmlElement("ImpTotCE")]
        public string ImpTotCE { get; set; }

        [XmlElement("MonImpTotCE")]
        public string MonImpTotCE { get; set; }

        [XmlElement("ImpTot")]
        public string ImpTot { get; set; }

        [XmlElement("MonImpTot")]
        public string MonImpTot { get; set; }
    }



    public class Items
    {
        [XmlElement("ITEM")]
        public List<Item> ListItems = new List<Item>();
    }

    public class Item
    {
        [XmlElement("ItNroOrden")]
        public string ItNroOrden { get; set; }

        [XmlElement("TpoDocRelac")]
        public string TpoDocRelac { get; set; }

        [XmlElement("NumDocRelac")]
        public string NumDocRelac { get; set; }

        [XmlElement("FEmisDocRelac")]
        public string FEmisDocRelac { get; set; }

        [XmlElement("ITotDocRelac")]
        public string ITotDocRelac { get; set; }

        [XmlElement("MDocRelac")]
        public string MDocRelac { get; set; }

        [XmlElement("FecMovi")]
        public string FecMovi { get; set; }

        [XmlElement("NumMovi")]
        public string NumMovi { get; set; }

        [XmlElement("ImpSOperMov")]
        public string ImpSOperMov { get; set; }

        [XmlElement("MonMovi")]
        public string MonMovi { get; set; }

        [XmlElement("ImpOper")]
        public string ImpOper { get; set; }

        [XmlElement("MonImpOper")]
        public string MonImpOper { get; set; }

        [XmlElement("FecOper")]
        public string FecOper { get; set; }

        [XmlElement("ImpTotOper")]
        public string ImpTotOper { get; set; }

        [XmlElement("MonOper")]
        public string MonOper { get; set; }

        [XmlElement("MonRefeTC")]
        public string MonRefeTC { get; set; }

        [XmlElement("MonDestTC")]
        public string MonDestTC { get; set; }

        [XmlElement("FactorTC")]
        public string FactorTC { get; set; }

        [XmlElement("FechaTC")]
        public string FechaTC { get; set; }

        //[XmlElement("LnPorcIGV")]
        //public string LnPorcIGV { get; set; }
    }

    [XmlRoot("DatosAdicionales")]
    public class DatosAdicionales
    {
        [XmlElement("AditionalInformation")]
        public string AditionalInformation { get; set; }
    }


    [XmlRoot("PERCEPCION")]
    public class Percepcion
    {
        [XmlElement("PerBaseImp")]
        public string PerBaseImp { get; set; }

        [XmlElement("PerMntoPer")]
        public string PerMntoPer { get; set; }

        [XmlElement("PerMntoTot")]
        public string PerMntoTot { get; set; }
    }


    [XmlRoot("CABECERANOTA")]
    public class CabeceraNota
    {
        [XmlElement("TpoNota")]
        public string TpoNota { get; set; }

        [XmlElement("MotivoNota")]
        public string MotivoNota { get; set; }
    }

    [XmlRoot("EXTRA")]
    public class Extra
    {
        [XmlElement("ExID")]
        public string ExID { get; set; }

        [XmlElement("ExLinea")]
        public string ExLinea { get; set; }

        [XmlElement("ExDato")]
        public string ExDato { get; set; }

        [XmlElement("ExTpoDato")]
        public string ExTpoDato { get; set; }
    }

    public class Extras
    {
        [XmlElement("EXTRA")]
        public List<Extra> ListExtra = new List<Extra>();

    }



    [XmlRoot("Referencia")]
    public class Referencia
    {
        [XmlElement("RefNroOrden")]
        public string RefNroOrden { get; set; }

        [XmlElement("RefID")]
        public string RefID { get; set; }

        [XmlElement("RefTpoDoc")]
        public string RefTpoDoc { get; set; }

        //[XmlElement("RefDRelac")]
        //public string RefDRelac { get; set; }

        //[XmlElement("RefTpoDRelac")]
        //public string RefTpoDRelac { get; set; }
    }

    public class Referencias
    {
        [XmlElement("REFERENCIA")]
        public List<Referencia> ListReferencia = new List<Referencia>();
    }


    [XmlRoot("ANTICIPO")]
    public class Anticipo
    {
        [XmlElement("AntNroOrden")]
        public string AntNroOrden { get; set; }

        [XmlElement("AntMonto")]
        public string AntMonto { get; set; }

        [XmlElement("AntTpoDocAnt")]
        public string AntTpoDocAnt { get; set; }

        [XmlElement("AntIdDocAnt")]
        public string AntIdDocAnt { get; set; }

        [XmlElement("AntNumDocEmi")]
        public string AntNumDocEmi { get; set; }

        [XmlElement("AntTpoDocEmi")]
        public string AntTpoDocEmi { get; set; }
    }

    public class Anticipos
    {
        [XmlElement("ANTICIPO")]
        public List<Anticipo> ListAnticipo = new List<Anticipo>();
    }


    [XmlRoot("MAIL")]
    public class Mail
    {
        [XmlElement("ID_DC")]
        public int Id_Dc { get; set; }

        [XmlElement("NUM_CPE")]
        public string Num_Cpe { get; set; }

        [XmlElement("Para")]
        public string Para { get; set; }

        [XmlElement("CC")]
        public string CC { get; set; }

        //[XmlElement("CCO")]
        //public string CCO { get; set; }

        [XmlElement("CCO")]
        public string CCO { get; set; }

    }

    [XmlRoot("ADICIONAL")]
    public class Adicional
    {
        [XmlElement("Sede")]
        public string Sede { get; set; }

        [XmlElement("Usuario")]
        public string Usuario { get; set; }

        [XmlElement("Impresora")]
        public string Impresora { get; set; }

        [XmlElement("Campo1")]
        public string Campo1 { get; set; }

        [XmlElement("Campo2")]
        public string Campo2 { get; set; }

        [XmlElement("Campo3")]
        public string Campo3 { get; set; }

        [XmlElement("Campo4")]
        public string Campo4 { get; set; }

        [XmlElement("Campo5")]
        public string Campo5 { get; set; }

        [XmlElement("Campo6")]
        public string Campo6 { get; set; }

        [XmlElement("Campo7")]
        public string Campo7 { get; set; }

        [XmlElement("Campo8")]
        public string Campo8 { get; set; }

        [XmlElement("Campo9")]
        public string Campo9 { get; set; }

        [XmlElement("Campo10")]
        public string Campo10 { get; set; }
    }
}
