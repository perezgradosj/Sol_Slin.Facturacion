<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="ListadoEmpresa.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Mantenimiento.ListadoEmpresa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

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

        <%--<br />--%>
        <hr />

        <div class="row">
            <div class="col-lg-4" style="font-family: Cambria; font-size: medium">
                <asp:HyperLink runat="server" BackColor="Transparent" NavigateUrl="~/Views/Home/Inicio" Width="60" BorderStyle="None" Font-Bold="true" Text="Home" CssClass="form-control"></asp:HyperLink>
                <a style="font-family: Cambria; height: 35px" href="ListadoEmpresa">/ Lista de Empresas</a>
            </div>
        </div>

        <div class="modal-body" style="font-family: Cambria">

            <ul class="nav nav-tabs">
                <li class="active"><a data-toggle="tab" style="background-color: transparent; border-color: none; font-family: Cambria; font-size: 16px"><i class="fa fa-filter"></i> Criterios de Búsqueda</a></li>
            </ul>
            <br />

            <div class="row">
                <div class="col-md-1">
                    <label style="height: 27px; width: 120px; padding-top: 8px">Razón S.</label>
                </div>
                <div class="col-md-3">
                    <input runat="server" id="txtbuscarrsocial" class="form-control" placeholder="razon social" readonly="readonly" />
                </div>

                <div class="col-md-1">
                    <label style="height: 27px; width: 100px; padding-top: 8px">RUC:</label>
                </div>
                <div class="col-md-2">
                    <input runat="server" id="txtbuscarruc" class="form-control" placeholder="ruc" style="width: 150px" readonly="readonly" />
                </div>
            </div>

            <div class="row">
                <div class="col-md-1">
                    <label style="height: 27px; width: 100px; padding-top: 8px" hidden="hidden">Estado:</label>
                </div>
                <div class="col-md-2">
                    <select runat="server" id="cbobuscarestado" class="form-control" style="width: 150px" hidden="hidden" visible="false">
                    </select>
                </div>
            </div>

            <div class="row">
                <div class="col-md-1">
                    <label></label>
                </div>
                <div class="col-md-5">
                    <%--<a href="RegistroEmpresa" runat="server" id="btnNuevo" class="btn btn-primary" style="height: 30px; font-size: small; font-family: Cambria" hidden="hidden" aria-hidden="false"><i class="fa fa-plus"></i> Nuevo</a>--%>
                    <a runat="server" id="btnbuscar" class="btn btn-primary" style="height: 30px; font-size: small; font-family: Cambria" onserverclick="btnbuscar_ServerClick" visible="false"><i class="fa fa-search"> Buscar</i></a>

                </div>
            </div>

            <hr />
            <div class="row">
                <div class="table-responsive">
                    <asp:GridView runat="server" ID="GVListadoEmpresa" CssClass="grid sortable {disableSortCols: []}" AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField HeaderText="Editar"  HeaderStyle-ForeColor="White" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" ID="btnImgEditar" OnClick="btnImgEditar_Click" ImageUrl="~/Img/editar.png" Height="15px" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="Nro" HeaderText="Nro" SortExpression="Nro"  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                            <asp:BoundField DataField="RUC" HeaderText="Ruc" SortExpression="Ruc"  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                            <asp:BoundField DataField="RazonSocial" HeaderText="Razon Social" SortExpression="RazonSocial"  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                            <asp:BoundField DataField="Direccion" HeaderText="Dirección" SortExpression="Direccion"  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                            <asp:BoundField DataField="Telefono" HeaderText="Telefono"  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                            <asp:BoundField DataField="Email" HeaderText="Email"  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                            <%--<asp:BoundField DataField="PaginaWeb" HeaderText="Web"                                          HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" ItemStyle-Wrap="false"/>--%>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>


        </div>
    </div>
</asp:Content>
