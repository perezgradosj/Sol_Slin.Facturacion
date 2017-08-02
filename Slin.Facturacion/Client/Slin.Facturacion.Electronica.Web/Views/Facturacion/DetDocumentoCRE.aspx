<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetDocumentoCRE.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Facturacion.DetDocumentoCRE" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link href="../../Scripts/mytable/tableStyle.css" rel="stylesheet" />

    <link href="../../Scripts/Table/css/styles.css" rel="stylesheet" />
    <script src="../../Scripts/Table/js/jquery-1.4.2.min.js"></script>
    <script src="../../Scripts/Table/js/jquery.dataTables.min.js"></script>
    <script src="../../Scripts/Table/js/jquery.metadata.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {

            $.metadata.setType("class");

            $("table.grid").each(function () {
                var grid = $(this);
                // Por cada GridView que se encuentre modificar el código HTML generado para agregar el THEAD.
                if (grid.find("tbody > tr > th").length > 0) {
                    grid.find("tbody").before("<thead><tr></tr></thead>");
                    grid.find("thead:first tr").append(grid.find("th"));
                    grid.find("tbody tr:first").remove();
                }
                // Si el GridView tiene la clase "sortable" aplicar el plugin DataTables si tiene más de 10 elementos.
                if (grid.hasClass("sortable") && grid.find("tbody:first > tr").length > 920) {
                    grid.dataTable({
                        sPaginationType: "full_numbers",
                        aoColumnDefs: [
                            { bSortable: false, aTargets: grid.metadata().disableSortCols }
                        ]
                    });
                }
            });
        });
    </script>


</head>
<body class="blue">
    <form id="form1" runat="server">


        <table class="CSSTableGenerator2">
            <tr style="height: 30px; font-family: 'Lucida Sans Unicode'">
                <td colspan="10">
                    <label runat="server" id="NombreEmpresaDetDoc" style="font-size: larger"></label>
                </td>
            </tr>
            <tr>
                <td></td>
            </tr>
        </table>

        <table class="CSSTableGenerator" style="text-align: left" border="1">
            <tr>
                <td colspan="10">
                    <label style="font-size: larger;">Documento Detalle</label>
                </td>
            </tr>


            <tr>
                <td colspan="2">
                    <label>Tipo Doc:</label>
                </td>
                <td>
                    <input class="texto" readonly="readonly" type="text" runat="server" id="txttipodocumentocod" size="12" />
                </td>
                <td colspan="2">
                    <label>Descripción:</label>
                </td>
                <td style="text-align: center">
                    <input class="texto" readonly="readonly" type="text" runat="server" id="txttipodocDescripcion" size="15" />
                </td>
                <td colspan="1" style="min-width: 105px"></td>
                <td colspan="2" style="max-width: 92px">
                    <label>Fecha Emisión:</label>
                </td>
                <td style="text-align: center">
                    <input class="cadena" readonly="readonly" runat="server" type="text" id="txtfechaemision" size="19" />
                </td>
                
            </tr>

            

            <tr>
                <td colspan="2">
                    <label>Serie:</label>
                </td>
                <td>
                    <input class="texto" readonly="readonly" runat="server" type="text" id="txtserie" size="12" />
                </td>
                <td colspan="2">
                    <label>Correlativo:</label>
                </td>
                <td style="text-align: center">
                    <input class="texto" readonly="readonly" runat="server" type="text" id="txtcorrelativo" size="15" />
                </td>
                <td colspan="4">&nbsp;</td>
            </tr>

            <tr>

                <td colspan="2">
                    <label>Emisor:</label>
                </td>
                <td>
                    <input class="texto" readonly="readonly" runat="server" type="text" id="txtrucempresa" size="12" />
                </td>
                <td colspan="2">
                    <label>Razón Social:</label>
                </td>
                <td colspan="5">
                    <input class="cadena" readonly="readonly" runat="server" type="text" id="txtrazonsocialempresa" style="width: 518px" />
                </td>
            </tr>

            <tr>
                <td colspan="2">
                    <label>Dirección:</label>
                </td>
                <td colspan="8">
                    <input class="cadena" readonly="readonly" runat="server" type="text" id="txtdireccionempresa" style="width: 732px" />
                </td>
            </tr>

            <tr>
                <td colspan="2">
                    <label>Ubigeo:</label>
                </td>
                <td>
                    <input class="texto" readonly="readonly" runat="server" type="text" id="txtubigeo" size="12" />
                </td>
                <td colspan="2">
                    <label>Ciudad:</label>
                </td>
                <td colspan="5">
                    <input class="cadena" readonly="readonly" runat="server" type="text" id="txtciudad" style="width: 518px" />
                </td>
            </tr>

            <tr>
                <td colspan="2">
                    <label>Cliente:</label>
                </td>
                <td>
                    <input class="texto" readonly="readonly" runat="server" type="text" id="txtclientenrodocumento" size="12" />
                </td>
                <td colspan="2" style="min-width: 90px">
                    <label>Razón Social:</label>
                </td>
                <td colspan="5">
                    <input class="cadena" readonly="readonly" runat="server" type="text" id="txtclienterazonsocial" style="width: 518px" />
                </td>
            </tr>

            <tr>
                <td colspan="2">
                    <label>Monto Neto:</label>
                </td>
                <td>
                    <input class="monto" readonly="readonly" runat="server" type="text" id="txtmontoneto" size="12" />
                </td>
                <td colspan="2">
                    <label>Tot Retenido:</label>
                </td>
                <td style="text-align: center">
                    <input class="monto" readonly="readonly" runat="server" type="text" id="txttotalretenido" size="15" />
                </td>
                <td colspan="2" style="max-width: 92px">
                    <label>Tasa Ret.%:</label>
                </td>
                <td style="text-align: center">
                    <input class="cadena" readonly="readonly" runat="server" type="text" id="txttasaretencion" size="12" />
                </td>
                <td>&nbsp;</td>
            </tr>

            <tr>
                <td colspan="2">
                    <label>Otros:</label>
                </td>
                <td>
                    <input class="monto" readonly="readonly" runat="server" type="text" id="txtotrostributos" size="12" />
                </td>
                <td colspan="2">
                    <label>Moneda:</label>
                </td>
                <td style="text-align: center">
                    <input class="cadena" readonly="readonly" runat="server" type="text" id="txtmoneda" size="15" />
                </td>
                <td colspan="2" style="min-width: 105px">
                    <label>Estado:</label>
                </td>
                <td style="text-align: left" colspan="2">
                    <input class="cadena" readonly="readonly" size="30" runat="server" type="text" id="txtEstadoDocumentoBD"  />
                </td>
            </tr>

            <tr>
                <td colspan="2">
                    <label>Nota :</label>
                </td>
                <td colspan="8">
                    <input class="cadena" readonly="readonly" runat="server" type="text" id="txtNotaRespuesta" size="102" />
                </td>
            </tr>

            <tr>
                <td colspan="2">
                    <label>Observaciones:</label>
                </td>
                <td colspan="8">
                    <input class="cadena" readonly="readonly" runat="server" type="text" id="txtobservacion" size="102" />
                </td>
            </tr>

            <tr>
                <td colspan="10">&nbsp;</td>
            </tr>
        </table>


        <table class="datagrid">
            <tr>
                <td colspan="2"></td>
                <td colspan="1">
                    <label style="font-size: larger; font-weight: bolder">Detalle</label>
                </td>
            </tr>

            <tr>
                <td colspan="2"></td>

                <td>
                    <asp:GridView CssClass="grid sortable {disableSortCols: []}" runat="server" ID="GridView1" AutoGenerateColumns="false" PageSize="20">
                        <Columns>
                            <asp:BoundField HeaderStyle-Height="17px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" DataField="TipoDocRelac" HeaderText="Tipo" HeaderStyle-ForeColor="White" />
                            <asp:BoundField HeaderStyle-Height="17px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" DataField="NroDocRelac" HeaderText="Serie-Correlativo" HeaderStyle-ForeColor="White" />
                            <asp:BoundField HeaderStyle-Height="17px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" DataField="FechaEmisionDocRelac" HeaderText="Fecha" HeaderStyle-ForeColor="White" />
                            <asp:BoundField HeaderStyle-Height="17px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" DataField="MonedaImpTotDocRelac" HeaderText="Moneda" HeaderStyle-ForeColor="White" />
                            <asp:BoundField HeaderStyle-Height="17px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" DataField="ImporteTotDocRelac" HeaderText="Importe Total" DataFormatString="{0:n}" HeaderStyle-ForeColor="White" />
                            <asp:BoundField HeaderStyle-Height="17px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" DataField="FechaPago" HeaderText="Fecha Pago" HeaderStyle-ForeColor="White" />
                            <asp:BoundField HeaderStyle-Height="17px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" DataField="NumeroPago" HeaderText="Numero Pago" HeaderStyle-ForeColor="White" />
                            <asp:BoundField HeaderStyle-Height="17px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" DataField="ImportePagoSinReten" HeaderText="Pag. Sin Ret." DataFormatString="{0:n}" HeaderStyle-ForeColor="White" />
                            <asp:BoundField HeaderStyle-Height="17px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" DataField="ImporteRetenido" HeaderText="Imp. Retenido" DataFormatString="{0:n}" HeaderStyle-ForeColor="White" />
                            <asp:BoundField HeaderStyle-Height="17px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" DataField="ImporteTotxPagoNeto" HeaderText="Imp. Neto Pagado" DataFormatString="{0:n}" HeaderStyle-ForeColor="White" />
                        </Columns>
                    </asp:GridView>
                </td>
                <td></td>
            </tr>
        </table>
    </form>
</body>
</html>
