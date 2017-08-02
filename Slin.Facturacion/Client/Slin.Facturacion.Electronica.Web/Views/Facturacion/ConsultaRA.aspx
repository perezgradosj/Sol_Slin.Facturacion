<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="ConsultaRA.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Facturacion.ConsultaRA" %>

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

        <hr />

        <div class="row">
            <div style="font-family: Cambria; font-size: medium">
                <asp:HyperLink runat="server" BackColor="Transparent" NavigateUrl="~/Views/Home/Inicio" Width="60" BorderStyle="None" Font-Bold="true" Text="Home" CssClass="form-control"></asp:HyperLink>
                <a style="font-family: Cambria; height: 35px" href="ConsultaRA">/ Resumen de Anulados RA</a>
            </div>
        </div>

        <div class="modal-header">
            <label style="font-family: Cambria; font-size: 16px"><i class="fa fa-filter"></i> Criterios de Búsqueda</label>
        </div>

        <div class="modal-body" style="font-family: Cambria">

            <div class="row">
                <div class="col-lg-2" style="width: 110px">
                    <label style="width: 100px; height: 27px; padding-top: 7px">Fecha Desde:</label>
                </div>
                <div class="col-lg-2">
                    <input runat="server" id="txtfechadesde" class="form-control" placeholder="fecha desde" />
                </div>


                <div class="col-lg-2" style="width: 110px">
                    <label style="width: 100px; height: 27px; padding-top: 7px">Fecha Hasta:</label>
                </div>
                <div class="col-lg-2">
                    <input runat="server" id="txtfechahasta" class="form-control" placeholder="fecha hasta" />
                </div>


                <div class="col-lg-2" style="max-width: 110px;">
                    <label style="padding-top: 7px; height: 27px">Tipo Fecha:</label>
                </div>
                <div class="col-lg-3" style="max-width: 220px">
                    <select runat="server" id="cbotipofecha" class="form-control"></select>
                </div>
            </div>


            <div class="row">
                <div class="col-lg-2" style="width: 110px">
                    <label style="width: 80px; height: 27px; padding-top: 7px">Estado:</label>
                </div>
                <div class="col-lg-2">
                    <select runat="server" id="cboestado" class="form-control"></select>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-2" style="width: 110px">
                    <label></label>
                </div>
                <div class="col-lg-5">
                    <a runat="server" id="btnNuevo" style="height: 30px; font-size: small; font-family: Cambria" href="GenerarRA.aspx" class="btn btn-primary" aria-hidden="true" visible="false"><i class="fa fa-plus"> Nuevo</i></a>
                    <a runat="server" id="btnBuscar" style="height: 30px; font-size: small; font-family: Cambria" class="btn btn-primary" onserverclick="btnBuscar_ServerClick" visible="false"><i class="fa fa-search"></i> Buscar</a>
                </div>
            </div>

            <hr />
            <div class="row">
                <div class="table-responsive">

                    <asp:GridView runat="server" ID="GVListadoRA" CssClass="grid sortable {disableSortCols: [0]}" AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField HeaderText="Xml" HeaderStyle-Font-Size="Smaller"  HeaderStyle-ForeColor="White" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" ID="btnImgXML" ImageUrl="~/Img/xml.jpg" Height="15px" Width="12px" OnClick="btnImgXML_Click" ToolTip="Descargar Xml Doc." />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Det" HeaderStyle-Font-Size="Smaller"  HeaderStyle-ForeColor="White" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" ID="btnVerDetalle" ImageUrl="~/Img/detail.png" Height="15px" Width="12px" OnClick="btnVerDetalle_Click" ToolTip="Ver Detalle Documento" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="NombreArchivoXML" HeaderText="Nombre Archivo-Sec" SortExpression="NombreArchivoXML" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="FechaInicio" HeaderText="Fecha Emisión" DataFormatString="{0:d}" SortExpression="FechaInicio" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="FechaFin" HeaderText="Fecha Fin" DataFormatString="{0:d}" Visible="false" SortExpression="FechaFin" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="FechaEnvio" HeaderText="Fecha Envío" DataFormatString="{0:u}" SortExpression="FechaEnvio" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Estado.Descripcion" HeaderText="Estado" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                            <asp:BoundField  HeaderStyle-ForeColor="White"  ItemStyle-Font-Size="Smaller"  ItemStyle-HorizontalAlign="Left" DataField="MensajeEnvio" HeaderText="Mensaje Envio" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="NumeroAtencion" HeaderText="Num Ticket" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
