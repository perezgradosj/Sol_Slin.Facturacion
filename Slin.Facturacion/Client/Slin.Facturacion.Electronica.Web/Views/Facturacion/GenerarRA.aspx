<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="GenerarRA.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Facturacion.GenararRA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <header>

        <link rel="stylesheet" href="../../Content/DatetimePicker/bootstrap-datepicker.css">
        <script src="../../Content/DatetimePicker/bootstrap-datepicker.min.js"></script>

        <script type="text/javascript">
            $(document).ready(function () {
                var dp = $('#<%=txtfecharesumen.ClientID%>');
                dp.datepicker({
                    changeMonth: true,
                    changeYear: true,
                    format: "dd/mm/yyyy",
                    language: "tr"
                }).on('changeDate', function (ev) {
                    $(this).blur();
                    $(this).datepicker('hide');
                });
            });
        </script>

        <link href="../../Scripts/Table/css/styles.css" rel="stylesheet" />
        <script src="../../Scripts/Table/js/jquery.dataTables.min.js"></script>
        <script src="../../Scripts/Table/js/jquery.metadata.js"></script>

        <script type="text/javascript">
            $(document).ready(function () {

                $.metadata.setType("class");

                $("table.grid").each(function () {
                    var grid = $(this);

                    if (grid.find("tbody > tr > th").length > 0) {
                        grid.find("tbody").before("<thead><tr></tr></thead>");
                        grid.find("thead:first tr").append(grid.find("th"));
                        grid.find("tbody tr:first").remove();
                    }

                    if (grid.hasClass("sortable") && grid.find("tbody:first > tr").length > 10) {
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

        <script type="text/javascript">

            function SelectAll() {
                //var frm = document.forms['aspnetForm'];
                if ($('#<%=idSelectAll.ClientID%>').prop('checked')) {
                    //alert("OK");
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('chkSelImp') != -1) {
                            document.forms[0].elements[i].checked = true
                        }
                    }
                } else {
                    //alert("NOT OK");
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('chkSelImp') != -1) {
                            document.forms[0].elements[i].checked = false
                        }
                    }
                }
            }
        </script>



        <script type="text/javascript">
            function Validar() {
                if ($('#<%=idSelectAll.ClientID%>').prop('checked')) {
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('idSelectAll') != -1) {
                            document.forms[0].elements[i].checked = false
                        }
                    }
                }
            }
        </script>

    </header>


    <div class="content" style="background-repeat: no-repeat; background-image: url(../../Img/home/fondopantalla.jpg); background-size: cover">

        <div class="main-content">
            <div class="row">
                <div class="col-lg-3">
                    <img runat="server" id="logoEmpresa" style="align-items: stretch; max-width: 240px;" />
                </div>

                <div class="col-lg-6">
                </div>

                <div class="col-lg-3" style="text-align: right;">
                    <label style="font-size: 20px; font-family: Cambria;">Facturación Electrónica</label>
                    <label runat="server" id="lblempresa" style="font-size: 15px; font-family: Cambria;"></label>
                </div>
            </div>
        </div>

        <hr />

        <div class="row">
            <div style="font-family: Cambria; font-size: medium">
                <asp:HyperLink runat="server" BackColor="Transparent" NavigateUrl="~/Views/Home/Inicio" Width="60" BorderStyle="None" Font-Bold="true" Text="Home" CssClass="form-control"></asp:HyperLink>
                <a style="font-family: Cambria; height: 35px" href="GenerarRA">/ Generar Resumen de Doc. Anulados</a>
            </div>
        </div>

        <div class="modal-header">
            <label style="font-family: Cambria; font-size: 17px"><i class="fa fa-file-text"></i> Datos</label>
        </div>

        <div class="modal-body" style="font-family: Cambria">

            <div class="row">
                <div class="col-lg-2" style="max-width: 140px;">
                    <label style="height: 27px;">Seleccione Fecha:</label>
                </div>

                <div class="col-lg-2">
                    <input runat="server" id="txtfecharesumen" class="form-control" placeholder="seleccione fecha" required="required" />
                </div>
                <div class="col-md-1" style="max-width: 10px;">
                    <label></label>
                </div>
                <div class="col-lg-2">
                    <asp:Button runat="server" ID="btnGenerar" Height="29px" Font-Size="Small" Text="Generar Resumen" CssClass="btn btn-primary" Font-Names="Cambria" OnClick="btnGenerar_Click" />
                </div>
            </div>

            <br />
            <hr />
            <b style="font-size: small">Lista de Documentos anulados pendientes de envio</b>
            <div class="row">
                <div class="table-responsive">
                    <asp:GridView runat="server" ID="GVListDocumentPendingRA" CssClass="grid sortable {disableSortCols: []}" AutoGenerateColumns="false">

                        <Columns>
                            <%--<asp:TemplateField HeaderText="Editar"  HeaderStyle-ForeColor="White" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" ID="btnGenerateOrder" OnClick="btnGenerateOrder_Click" ImageUrl="~/Img/select.png" Height="15px" />
                                </ItemTemplate>
                            </asp:TemplateField>--%>

                            <asp:TemplateField HeaderText="Sel" HeaderStyle-Font-Size="Smaller"  HeaderStyle-ForeColor="White" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkSelImp" Height="15px" Width="12px" ToolTip="Select" OnClick="return Validar();" AutoPostBack="false" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="FechaEmision2" SortExpression="FechaEmision2" HeaderText="Fecha Documento" DataFormatString="{0:d}" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Estado.Descripcion" HeaderText="Estado Documento" SortExpression="Estado.Descripcion" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Cantidad" HeaderText="Cant. Docs." SortExpression="Cantidad" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <label style="min-width:7px"></label>
                    <input type="checkbox" runat="server" id="idSelectAll" onchange="SelectAll()" visible="false" />
                    <label runat="server" id="lblseleccionar" visible="false">Seleccionar todos</label>
                </div>
            </div>

            <div class="row">
                <div class="col-md-5">
                    <a href="ConsultaRA" class="btn btn-primary" style="height: 29px; font-family: Cambria; font-size: small"><i class="fa fa-arrow-left"></i> Cancelar</a>
                    <asp:Button runat="server" ID="btnGeneraSelected" Height="29px" Font-Size="Small" Font-Names="Cambria" OnClick="btnGeneraSelected_Click" CssClass="btn btn-primary" Text="Generar Seleccionados" Enabled="false" />
                </div>
            </div>
        </div>

        <%--<div class="modal-footer" style="text-align: left">
            
        </div>--%>
    </div>
</asp:Content>
