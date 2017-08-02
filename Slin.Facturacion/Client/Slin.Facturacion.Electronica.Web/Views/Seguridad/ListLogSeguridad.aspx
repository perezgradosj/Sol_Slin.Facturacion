<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="ListLogSeguridad.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Seguridad.ListLogSeguridad" %>
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
                    // Por cada GridView que se encuentre modificar el código HTML generado para agregar el THEAD.
                    if (grid.find("tbody > tr > th").length > 0) {
                        grid.find("tbody").before("<thead><tr></tr></thead>");
                        grid.find("thead:first tr").append(grid.find("th"));
                        grid.find("tbody tr:first").remove();
                    }
                    // Si el GridView tiene la clase "sortable" aplicar el plugin DataTables si tiene más de 10 elementos.
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

    <!-- INICIO CUERPO FILTRO-->

    <div lang="es-pe" class="content" style="background-repeat: no-repeat; background-image: url(../../Img/home/fondopantalla.jpg); background-size: cover">

        <div class="main-content">
            <div class="row">
                <div class="col-lg-3">
                    <img runat="server" id="logoEmpresa" style="align-items:stretch; max-width:240px;" />
                </div>

                <div class="col-lg-6">
                </div>

                <div class="col-lg-3" style="text-align: right;">
                    <label style="font-size: 20px; font-family: Cambria;">Facturación Electrónica</label>
                    <label runat="server" id="lblempresa" style="font-size: 15px; font-family: Cambria;"></label>
                </div>
            </div>
        </div>

        <%--<br />--%>
        <hr />

        <div class="row" style="border-color: none">
            <div style="font-family: Cambria; font-size: medium;">
                <asp:HyperLink runat="server" BackColor="Transparent" NavigateUrl="~/Views/Home/Inicio" Width="60" BorderStyle="None" Font-Bold="true" Text="Home" CssClass="form-control"></asp:HyperLink>
                <a style="font-family: Cambria; height: 35px" href="ListLogSeguridad">/ Log de Seguridad</a>
            </div>
        </div>

        <div class="main-content" style="font-family: Cambria; font-size: medium">
            <ul class="nav nav-tabs">
                <li class="active"><a style="background-color: transparent; border-color: none; font-family: Cambria; font-size: 16px; font: bold" href="#lista" data-toggle="tab"><i class="fa fa-filter"></i> Criterios de Búsqueda</a></li>
            </ul>
        </div>

        <div class="tab-content" style="padding-top: 15px;">
            <div class="tab-pane active" id="consulta">
                <div class="col-md-12" style="font-family: Cambria;">

                    <div class="row">
                        <div class="col-md-1">
                            <label style="width: 120px; height: 27px; padding-top: 7px">Fecha Desde:</label>
                        </div>
                        <div class="col-md-2">
                            <input runat="server" type="text" id="txtfechadesde" class="form-control" style="width: 150px;" placeholder="seleccione fecha" />
                        </div>

                        <div class="col-md-1">
                            <label style="width: 85px; height: 27px; padding-top: 7px">Fecha Hasta:</label>
                        </div>
                        <div class="col-md-2">
                            <input runat="server" type="text" id="txtfechahasta" class="form-control" style="width: 150px;" placeholder="seleccione fecha" maxlength="10" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-1">
                            <label style="width: 80px; padding-top: 7px">Tipo Log:</label>
                        </div>
                        <div class="col-md-2">
                            <select runat="server" id="cbotipolog" class="form-control" style="width: 150px;">
                            </select>
                        </div>

                        <div class="col-md-1">
                            <label style="width: 80px; padding-top: 7px">Perfil:</label>
                        </div>
                        <div class="col-md-2">
                            <select runat="server" id="cboperfil" class="form-control" style="width: 150px;">
                            </select>
                        </div>

                        <div class="col-md-1">
                            <label style="width: 80px; padding-top: 7px">Usuario:</label>
                        </div>
                        <div class="col-md-2">
                            <input runat="server" type="text" id="txtusername" class="form-control" style="min-width: 150px;" placeholder="usuario" maxlength="50" />
                        </div>

                    </div>

                    <br />
                    <div class="row">
                        <div class="col-md-1">
                            <label></label>
                        </div>
                        <div class="col-md-3">
                            <a runat="server" style="height: 30px; font-size: small; font-family: Cambria" class="btn btn-primary" id="btnBuscar" onserverclick="btnBuscar_ServerClick" visible="false"><i class="fa fa-search"></i> Buscar</a>
                            <asp:Button runat="server" ID="btnPDF" Height="30px" Font-Size="Small" Font-Names="Cambria" OnClick="btnPDF_Click" CssClass="btn btn-primary" Text="Exportar PDF" Enabled="false" />
                            <%--<a href="ConsultaDocumento" style="height: 30px; font-size: small; font-family: Cambria" class="btn btn-primary">Limpiar</a>--%>
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

                            <asp:GridView runat="server" ID="GVLogSeguridad" CssClass="grid sortable {disableSortCols: []}" AutoGenerateColumns="false" >
                                <Columns>

                                    <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="ID" HeaderText="Nro" SortExpression="ID" />
                                    <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Empleado.DNI" HeaderText="Nro Documento" SortExpression="Empleado.DNI" HeaderStyle-Wrap="false" />
                                    <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Perfil.IdPeril" HeaderText="Perfil" SortExpression="Serie.NumeroSerie" HeaderStyle-Wrap="false" Visible="false" />
                                    <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Left" DataField="Perfil.NombrePerfil" HeaderText="Perfil" SortExpression="Perfil.NombrePerfil" HeaderStyle-Wrap="false" />
                                    <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Left" DataField="Username" HeaderText="Usuario" SortExpression="Username" HeaderStyle-Wrap="false" />
                                    <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Left" DataField="NombresApellidos" HeaderText="Nombres" SortExpression="NombresApellidos" HeaderStyle-Wrap="false" />
                                    <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="FechaIngreso" DataFormatString="{0:g}" HeaderText="Fecha Ingreso" SortExpression="FechaIngreso" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                                    <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="FechaSalida" DataFormatString="{0:g}" HeaderText="Fecha Salida" SortExpression="FechaSalida" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                    <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Left" DataField="HostName" HeaderText="HostName" SortExpression="HostName" HeaderStyle-Wrap="false" Visible="false" />
                                    <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Left" DataField="TipoLog.Descripcion" HeaderText="ELogin" SortExpression="TipoLog.Descripcion" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />

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
