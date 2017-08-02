<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="ConsultaDocAnuladoSUNAT.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Facturacion.ConsultaDocAnuladoSUNAT" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

     <header>
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
                    if (grid.hasClass("sortable") && grid.find("tbody:first > tr").length > 0) {
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


        <link rel="stylesheet" href="../../Content/DatetimePicker/bootstrap-datepicker.css">
        <script src="../../Content/DatetimePicker/bootstrap-datepicker.min.js"></script>        

        <script type="text/javascript">
            $(document).ready(function () {
                var dp = $('#<%=txtfechadesde.ClientID%>');
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

        <script type="text/javascript">
            $(document).ready(function () {
                var dp = $('#<%=txtfechahasta.ClientID%>');
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

        <!-- validacion para numeros -->
        <script type="text/javascript">
            function validNumericos(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode
                if (((charCode == 8) || (charCode == 46)
                || (charCode >= 35 && charCode <= 40)
                    || (charCode >= 48 && charCode <= 57)
                    || (charCode >= 96 && charCode <= 105))) {
                    return true;
                }
                else {
                    return false;
                }
            }
        </script>

        <script src="../../Scripts/Validacion/Validaciones.js"></script>

    </header>

    <!-- INICIO CUERPO CONSULTA-->

    <div lang="es-pe" class="content" style="background-repeat:no-repeat; background-image:url(../../Img/home/fondopantalla.jpg); background-size:cover">

        <div class="main-content">
            <div class="row">
                <div class="col-lg-3" >
                    <img runat="server" id="logoEmpresa" style="align-items:stretch; max-width:240px;" />
                </div>

                <div class="col-lg-6">

                </div>

                <div class="col-lg-3" style="text-align:right;">
                    <label style="font-size:20px; font-family:Cambria;">Facturación Electrónica</label>
                    <label runat="server" id="lblempresa" style="font-size:15px; font-family:Cambria;"> </label>
                </div>
            </div>
        </div>

        <%--<br />--%>
        <hr />

        <div class="row" style="border-color:none">
            <div style="font-family: Cambria; font-size: medium;">
                <asp:HyperLink runat="server" BackColor="Transparent" NavigateUrl="~/Views/Home/Inicio" Width="60" BorderStyle="None" Font-Bold="true" Text="Home" CssClass="form-control"></asp:HyperLink>
                <a style="font-family: Cambria; height: 35px" href="ConsultaDocAnuladoSUNAT">/ Documento Anulado SUNAT</a>
            </div>
        </div>

        <div class="main-content" style="font-family: Cambria; font-size: medium">

            <ul class="nav nav-tabs">
                <li class="active"><a style="background-color:transparent; border-color:none; font-family: Cambria; font-size: 16px; font: bold" href="#lista" data-toggle="tab"><i class="fa fa-filter"></i> Criterios de Búsqueda</a></li>
            </ul>

        </div>

        <div class="tab-content" style="padding-top: 15px;">

            <div class="tab-pane active" id="consulta">

                <div class="col-md-12" style="font-family: Cambria;">
                    <div class="row">
                        <div class="col-md-1">
                            <label style="width: 120px; height: 27px; padding-top: 8px">Fecha Desde:</label>
                        </div>
                        <div class="col-md-2">
                            <input runat="server" type="text" id="txtfechadesde" class="form-control" style="width: 150px; font-size:12.5px" placeholder="seleccione fecha" value="fecha" />
                        </div>

                        <div class="col-md-1">
                            <label style="width: 85px; height: 27px; padding-top: 8px">Fecha Hasta:</label>
                        </div>
                        <div class="col-md-2">
                            <input runat="server" type="text" id="txtfechahasta" class="form-control" style="width: 150px;" placeholder="seleccione fecha" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-1">
                            <label style="width: 120px; height: 27px; padding-top: 8px">Tipo Doc.:</label>
                        </div>
                        <div class="col-md-2" style="text-align: left">
                            <select runat="server" id="cbotipodocumento" class="form-control" style="width: 150px;" onchange="this.form.submit()" onserverchange="cbotipodocumento_ServerChange"></select>
                        </div>

                        <div class="col-md-1">
                            <label style="width: 50px; padding-top: 8px">Serie:</label>
                        </div>
                        <div class="col-md-2">
                            <select runat="server" id="cboserie" class="form-control" style="width: 150px;"></select>
                        </div>

                        <div class="col-md-1">
                            <label style="width: 135px; padding-top: 8px">Nro Inicio:</label>
                        </div>
                        <div class="col-md-2">
                            <input runat="server" id="txtnuminicio" class="form-control" style="width: 150px;" placeholder="nro doc inicio" onkeypress="return Validasolonumeros(event)" maxlength="12" />
                        </div>

                        <div class="col-md-1">
                            <label style="width: 130px; padding-top: 8px">Nro Fin:</label>
                        </div>
                        <div class="col-md-2">
                            <input runat="server" id="txtnumfin" class="form-control" style="width: 150px;" placeholder="nro doc fin" onkeypress="return Validasolonumeros(event)" maxlength="12" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-1">
                            <label style="width: 100px; padding-top: 8px">Cliente:</label>
                        </div>
                        <div class="col-md-2">
                            <input runat="server" id="txtclienteruc" class="form-control" style="width: 150px;" placeholder="RUC" maxlength="11" onkeypress="return Validasolonumeros(event)" />
                        </div>
                        <div class="col-md-1">
                            <label style="width: 15px; padding-top: 8px;">R.Social:</label>
                        </div>
                        <div class="col-md-3" style="max-width: 300px;">
                            <input runat="server" style="width: auto" id="txtclientenombres" class="form-control" placeholder="Razon Social" />
                        </div>

                        <div class="col-md-2">
                            <label></label>
                        </div>

                        <div class="col-md-1">
                            <label style="width: 80px; padding-top: 8px" hidden="hidden">Estado:</label>
                        </div>
                        <div class="col-md-2">
                            <select runat="server" id="cboestado" class="form-control" style="width: 150px;" visible="false">
                            </select>
                        </div>
                    </div>

                    <br />

                    <div class="row">
                        <div class="col-md-1">
                            <label></label>
                        </div>
                        <div class="col-md-3">
                            <a runat="server" style="height: 30px; font-size: small; font-family: Cambria" class="btn btn-primary" id="btnBuscar" onserverclick="btnBuscar_Click" visible="false"><i class="fa fa-search"></i> Buscar</a>
                            <a href="ConsultaDocAnuladoSUNAT" style="height: 30px; font-size: small; font-family: Cambria" class="btn btn-primary">Limpiar</a>
                            <%--<input type="reset" class="btn btn-primary" style="font-size:small; height:30px; font-family:Cambria" value="Limpiar" />--%>
                        </div>
                        <div class="col-md-1">
                            <label></label>
                        </div>
                        <div class="col-md-3">
                            <asp:Button runat="server" ID="btnExportarExcel" Height="30px" Font-Size="Small" Font-Names="Cambria" OnClick="btnExportarExcel_Click" CssClass="btn btn-primary" Text="Exportar Excel" Enabled="false" />
                        </div>
                    </div>


                    <hr />
                    <!--LISTA DE RESULTADOS GridView1-->

                    <div class="row">

                        <div class="table-responsive">

                            <asp:GridView runat="server" ID="GridView1" CssClass="grid sortable {disableSortCols: []}" AutoGenerateColumns="false" OnRowDataBound="GridView1_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Xml" HeaderStyle-Font-Size="Smaller"  HeaderStyle-ForeColor="White" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="btnImgXML" ImageUrl="~/Img/xml.jpg" Height="15px" Width="12px" OnClick="btnImgXML_Click" ToolTip="Descargar Xml Doc." />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Pdf" HeaderStyle-Font-Size="Smaller"  HeaderStyle-ForeColor="White" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="btnImgPdf" ImageUrl="~/Img/pdf.jpg" Height="15px" Width="12px" OnClick="btnImgPdf_Click" ToolTip="Descargar Pdf Doc." />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Det" HeaderStyle-Font-Size="Smaller"  HeaderStyle-ForeColor="White" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="btnVerDetalle" ImageUrl="~/Img/detail.png" Height="15px" Width="12px" OnClick="btnVerDetalle_Click" ToolTip="Ver Detalle Documento" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:ImageField HeaderText="Es" HeaderStyle-Font-Size="Smaller"  HeaderStyle-ForeColor="White" DataImageUrlField="RutaImagen" ControlStyle-Width="12px" ControlStyle-Height="12px" ItemStyle-VerticalAlign="Top">
                                    </asp:ImageField>

                                    <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Nro" HeaderText="Nro" SortExpression="Nro" />
                                    <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="TipoDocumento.CodigoDocumento" HeaderText="Tpo doc." SortExpression="TipoDocumento.CodigoDocumento" HeaderStyle-Wrap="false"/>
                                    <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Serie.NumeroSerie" HeaderText="Serie" SortExpression="Serie.NumeroSerie" HeaderStyle-Wrap="false" />
                                    <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="NumeroDocumento" HeaderText="Correlativo" SortExpression="NumeroDocumento" HeaderStyle-Wrap="false" />
                                    <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="FechaEmision" DataFormatString="{0:d}" HeaderText="Fecha Emisión" SortExpression="FechaEmision" HeaderStyle-Wrap="false" />
                                    <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Cliente.NumeroDocumentoIdentidad" HeaderText="Nro doc Clie." SortExpression="Cliente.NumeroDocumentoIdentidad" HeaderStyle-Wrap="false" />
                                    <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Left" DataField="Cliente.Nombres" HeaderText="Cliente" SortExpression="Cliente.Nombres" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                                    <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Moneda.Descripcion" HeaderText="Moneda" SortExpression="Moneda.Descripcion" HeaderStyle-Wrap="false" ItemStyle-Wrap="false"/>
                                    <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Right" DataField="MontoTotal" HeaderText="Monto" SortExpression="MontoTotal" HeaderStyle-Wrap="false"/>
                                    <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Estado.Descripcion" HeaderText="Estado" SortExpression="Estado.Descripcion" ItemStyle-Wrap="false" HeaderStyle-Wrap="false"/>

                                    <asp:TemplateField Visible="false" HeaderText="Imp" HeaderStyle-Font-Size="Smaller"  HeaderStyle-ForeColor="White" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="btnImprimir" ImageUrl="~/Img/facturacion/print.png" Height="15px" Width="12px" OnClick="btnImprimir_Click" ToolTip="Imprimir Ticket" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <br />
                        </div>
                        <br />
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
