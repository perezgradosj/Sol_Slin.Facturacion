<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="ConfiguracionTimeService.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Configuracion.ConfiguracionTimeService" %>

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

        <script src="../../Scripts/Validacion/Validaciones.js"></script>

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
        <%--<br />--%>

        <div class="row">
            <div style="font-family: Cambria; font-size: medium">
                <asp:HyperLink runat="server" BackColor="Transparent" NavigateUrl="~/Views/Home/Inicio" Width="60" BorderStyle="None" Font-Bold="true" Text="Home" CssClass="form-control"></asp:HyperLink>
                <a style="font-family: Cambria; height: 35px" href="ConfiguracionTimeService">/ Configuración Hora de Servicio</a>
            </div>
        </div>

        <div class="modal-header">
            <label style="font-family: Cambria; font-size: 16px"><i class="fa fa-edit"></i> Seleccione un Servicio a Configurar</label>
        </div>

        <div class="modal-body" style="font-family: Cambria">

            <div class="row">

                <div class="table-responsive">
                    <asp:GridView runat="server" ID="GVListService" CssClass="grid sortable {disableSortCols: [0]}" AutoGenerateColumns="false">
                        <Columns>

                            <asp:TemplateField HeaderText="Editar"  HeaderStyle-ForeColor="White" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" ID="btnImgEditar" OnClick="btnImgEditar_Click" ImageUrl="~/Img/editar.png" Height="15px" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="IdService" HeaderText="Cod. Serv." SortExpression="NombreArchivoXML" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Left" DataField="NombreService" HeaderText="Nombre Servicio" DataFormatString="{0:d}" SortExpression="FechaInicio2" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="ValueTime" HeaderText="Hora Ejecución hh/mm" DataFormatString="{0:d}" SortExpression="FechaEnvio2" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="IntervaleValue" HeaderText="Intervalo" SortExpression="Estado.Descripcion" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="MaxNumAttempts" HeaderText="Max Intentos Envío" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" />
                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Estado.Descripcion" HeaderText="Estado" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" />
                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="ServiceStatus" HeaderText="Estado Windows" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Left" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>


            <br />
            <div class="row">
                <div class="col-lg-3">
                    <%--<a class="btn btn-primary" href="../Configuracion/Configurar" style="height: 30px; font-family:Cambria; font-size:small"><i class="fa fa-sign-out"></i> Inicio</a>--%>
                    <%--<asp:Button runat="server" ID="btnGuardar" Height="30px" Font-Size="Small" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardar_Click" Enabled="false" />--%>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
