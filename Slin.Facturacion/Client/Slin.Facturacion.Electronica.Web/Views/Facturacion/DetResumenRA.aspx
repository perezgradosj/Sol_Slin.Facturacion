<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetResumenRA.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Facturacion.DetResumenRA" %>

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
<body>
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
                <td colspan="10" style="height: 18px">
                    <label style="font-size: larger;" runat="server" id="cabecera"></label>
                </td>
            </tr>



            <tr>
                <td colspan="2" style="min-width: 80px">
                    <label>Nro Ticket:</label>
                </td>
                <td style="width: 110px">
                    <input class="texto" readonly="readonly" type="text" runat="server" id="txtnroticket" size="19" />
                </td>

                <td colspan="4" style="width: 10000px"></td>

                <td colspan="2" style="max-width: 70px">
                    <label>Fecha Envió:</label>
                </td>
                <td style="max-width: 105px">
                    <input class="texto" readonly="readonly" runat="server" type="text" id="txtfechaenvio" style="width: 180px" />
                </td>
            </tr>

            <tr>
                <td colspan="2">
                    <label>Secuencia:</label>
                </td>
                <td style="width: 110px">
                    <input class="texto" readonly="readonly" runat="server" type="text" id="txtsecuencia" size="19" />
                </td>

                <td colspan="7">&nbsp;</td>
            </tr>

            <tr>

                <td colspan="2">
                    <label>Estado:</label>
                </td>
                <td style="width: 110px">
                    <input class="texto" readonly="readonly" runat="server" type="text" id="txtestado" size="19" />
                </td>
                <td colspan="2" style="min-width: 85px">
                    <label>Descripción:</label>
                </td>
                <td colspan="5">
                    <input class="cadena" readonly="readonly" runat="server" type="text" id="txtestadodescripcion" style="width: 500px" />
                </td>
            </tr>

            <tr>
                <td colspan="2">
                    <label>Mensaje:</label>
                </td>
                <td colspan="8">
                    <textarea rows="4" class="cadena" readonly="readonly" runat="server" type="text" id="txtmensaje" style="width: 755px"></textarea>
                </td>
            </tr>

            <tr>
                <td colspan="10">&nbsp;
                </td>
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
                    <asp:GridView CssClass="grid sortable {disableSortCols: []}" runat="server" ID="GVListadoDetalleRA" AutoGenerateColumns="false" PageSize="20">
                        <Columns>
                            <asp:BoundField HeaderStyle-Height="17px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" DataField="TipoDocumento.CodigoDocumento" HeaderText="Tipo Doc." SortExpression="TipoDocumento.CodigoDocumento"  HeaderStyle-ForeColor="White" ItemStyle-Wrap="false" HeaderStyle-Wrap="false"  />
                            <asp:BoundField HeaderStyle-Height="17px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" DataField="Serie.NumeroSerie" HeaderText="Serie" SortExpression="Serie.NumeroSerie"  HeaderStyle-ForeColor="White" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                            <asp:BoundField HeaderStyle-Height="17px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" DataField="FacturaElectronica.NumeroDocumento" HeaderText="Correlativo" SortExpression="FacturaElectronica.NumeroDocumento"  HeaderStyle-ForeColor="White" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                            <asp:BoundField HeaderStyle-Height="17px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" DataField="FacturaElectronica.FechaCarga" HeaderText="Fecha Anulado" DataFormatString="{0:d}" SortExpression="FacturaElectronica.FechaCarga"  HeaderStyle-ForeColor="White" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                            <asp:BoundField HeaderStyle-Height="17px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" DataField="FacturaElectronica.MotivoAnulado" HeaderText="Motivo Baja"  HeaderStyle-ForeColor="White" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                            <asp:BoundField HeaderStyle-Height="17px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Size="Smaller" DataField="FacturaElectronica.MensajeAnulado" HeaderText="Mensaje"  HeaderStyle-ForeColor="White" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                        </Columns>
                    </asp:GridView>
                </td>
                <td></td>
            </tr>
        </table>
    </form>
</body>
</html>
