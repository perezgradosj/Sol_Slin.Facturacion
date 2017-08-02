<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="DocumentosEnviados.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Envio.DocumentosEnviados" %>

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

    </header>

    <div class="content" style="background-repeat:no-repeat; background-image:url(../../Img/home/fondopantalla.jpg); background-size:cover">


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

        <div class="row">
            <div style="font-family: Cambria; font-size: medium">
                <asp:HyperLink runat="server" BackColor="Transparent" NavigateUrl="~/Views/Home/Inicio" Width="60" BorderStyle="None" Font-Bold="true" Text="Home" CssClass="form-control"></asp:HyperLink>
                <a style="font-family: Cambria; height: 35px" href="DocumentosEnviados">/ Documentos Enviados</a>
            </div>
        </div>

        <div class="modal-header">
            <label style="font-family: Cambria; font-size: 16px"><i class="fa fa-filter"></i>Criterios de Búsqueda</label>
        </div>


        <div class="modal-body" style="font-family: Cambria;">
            <div class="row">
                <div class="col-lg-2" style="max-width: 110px">
                    <label style="height: 27px; padding-top: 8px">Fecha Desde:</label>
                </div>
                <div class="col-lg-2">
                    <input runat="server" id="txtfechadesde" class="form-control" placeholder="seleccione fecha" />
                </div>

                <div class="col-lg-2" style="max-width: 130px">
                    <label style="padding-top: 8px">Fecha Hasta:</label>
                </div>
                <div class="col-lg-2">
                    <input runat="server" id="txtfechahasta" class="form-control" placeholder="seleccione fecha" />
                </div>
            </div>


            <div class="row">
                <div class="col-lg-2" style="max-width: 110px">
                    <label style="height: 27px; padding-top: 8px">Serie:</label>
                </div>
                <div class="col-lg-2">
                    <input runat="server" id="txtserie" class="form-control" placeholder="serie" />
                </div>

                <div class="col-lg-2" style="max-width: 130px">
                    <label style="padding-top: 8px">Tpo Documento:</label>
                </div>
                <div class="col-lg-3">
                    <select runat="server" id="cbotipodocumento" class="form-control"></select>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-2" style="max-width: 110px">
                    <label></label>
                </div>
                <div class="col-lg-4">
                    <a class="btn btn-primary" runat="server" id="btnNuevo" href="EnvioDocumento" style="font-size:small;height: 30px; font-family:Cambria"><i class="fa fa-plus" hidden="hidden" aria-hidden="false"></i> Nuevo</a>
                    <a runat="server" id="btnBuscar" style="font-size: small; height: 30px; font-family: Cambria" class="btn btn-primary" onserverclick="btnBuscar_ServerClick" visible="false"><i class="fa fa-search"></i> Buscar</a>
                </div>
            </div>

            <hr />

            <div class="row">
                <div class="table-responsive">
                    <asp:GridView runat="server" ID="GVDocumentoEnviado" CssClass="grid sortable {disableSortCols: []}" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Nro" HeaderText="Nro" SortExpression="Nro" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                            <%--<asp:BoundField ItemStyle-Font-Size="Smaller" DataField="TipoDocumento.CodigoDocumento" HeaderText="Tipo Doc." SortExpression="TipoDocumento.CodigoDocumento"  HeaderStyle-ForeColor="White" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" HeaderStyle-Width="25px" />--%>
                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="TipoDocumento.Descripcion" HeaderText="Tipo Doc." SortExpression="TipoDocumento.Descripcion" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Serie" HeaderText="Serie" SortExpression="Serie" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="NumeroDocumento" HeaderText="Correlativo" SortExpression="NumeroDocumento" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="FechaEnvio" HeaderText="Fecha Envio" DataFormatString="{0:u}" SortExpression="FechaEnvio" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Asunto" HeaderText="Asunto" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" Visible="false"/>
                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Left" DataField="Destino" HeaderText="Email Destino" ItemStyle-Wrap="true" HeaderStyle-Wrap="false" />
                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Remitente" HeaderText="Remitente" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" Visible="false"/>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
