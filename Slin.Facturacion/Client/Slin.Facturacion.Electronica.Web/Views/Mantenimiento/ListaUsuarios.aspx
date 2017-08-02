<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="ListaUsuarios.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Mantenimiento.ListaUsuarios" %>

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
            <div style="font-family: Cambria">
                <asp:HyperLink runat="server" BackColor="Transparent" NavigateUrl="~/Views/Home/Inicio" Width="60" BorderStyle="None" Font-Bold="true" Text="Home" CssClass="form-control"></asp:HyperLink>
                <a style="font-family: Cambria" href="ListaUsuarios">/ Lista de Usuarios</a>
            </div>
        </div>

        <div class="main-content" style="font-family: Cambria;">

            <ul class="nav nav-tabs">
                <li class="active"><a style="background-color: transparent; border-color: none; font-family: Cambria; font-size: 16px; font: bold" href="#home" data-toggle="tab"><i class="fa fa-filter"></i>Criterios de Búsqueda</a></li>
            </ul>

            <div class="col-md-12">
                <br />

                <div class="row">
                    <div class="col-md-1">
                        <label style="height: 27px; width: 100px; padding-top: 8px">DNI:</label>
                    </div>
                    <div class="col-md-2">
                        <input runat="server" id="txtbuscarcodigo" class="form-control" maxlength="30" style="width: 150px" placeholder="dni" />
                    </div>

                    <div class="col-md-1">
                        <label style="height: 27px; width: 100px; padding-top: 8px">Usuario:</label>
                    </div>
                    <div class="col-md-3">
                        <input runat="server" id="txtbuscarusername" class="form-control" maxlength="20" placeholder="usuario" />
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-1">
                        <label style="height: 27px; width: 100px; padding-top: 8px">Estado:</label>
                    </div>
                    <div class="col-md-2">
                        <select runat="server" id="cbobuscarestado" class="form-control" style="width: 150px">
                        </select>
                    </div>

                    <%--PARA VER SI ESTO VA O YA NO--%>
                    <div class="col-md-1">
                        <label style="height: 27px; width: 100px; padding-top: 8px" hidden="hidden">Empresa:</label>
                    </div>
                    <div class="col-md-3">
                        <select runat="server" id="cbobuscarempresa" class="form-control" visible="false" aria-readonly="true"></select>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-1">
                        <label></label>
                    </div>

                    <div class="col-md-3" style="max-width: 200px">
                        <a runat="server" id="btnNuevo" hidden="hidden" aria-hidden="false" href="RegistroUsuario" class="btn btn-primary" style="height: 30px; font-size: small; font-family: Cambria"><i class="fa fa-plus"></i> Nuevo</a>
                        <button runat="server" id="btnBuscar" class="btn btn-primary" style="height: 30px; font-size: small; font-family: Cambria" onserverclick="btnBuscar_Click" visible="false"><i class="fa fa-search"></i> Buscar</button>
                    </div>
                    <div class="col-md-1">
                        <label></label>
                    </div>
                    <div class="col-md-4">
                        <asp:Button runat="server" ID="btnPDF" Height="30px" Font-Size="Small" Font-Names="Cambria" OnClick="btnPDF_Click" CssClass="btn btn-primary" Text="Exp. PDF" Enabled="false" />
                        <asp:Button runat="server" ID="btnExportarExcel" Height="30px" Font-Size="Small" Font-Names="Cambria" OnClick="btnExportarExcel_Click" CssClass="btn btn-primary" Text="Exp. Excel" Enabled="false" />
                    </div>
                </div>

                <hr />

                <div class="row">

                    <div class="table-responsive">
                        <asp:GridView runat="server" ID="GVListaUsuarios" CssClass="grid sortable {disableSortCols: []}" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField HeaderText="Editar"  HeaderStyle-ForeColor="White" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" ID="btnImgEditar" OnClick="btnImgEditar_Click" ImageUrl="~/Img/editar.png" Height="15px" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="Nro" HeaderText="Nro" SortExpression="Nro"  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="Empleado.DNI" HeaderText="Dni" SortExpression="Empleado.DNI"  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="Empleado.NombresApellidos" HeaderText="Nombre Apellidos" SortExpression="Empleado.NombresApellidos"  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="Username" HeaderText="Usuario" SortExpression="Username"  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                <%--<asp:BoundField DataField="Empresa.RazonSocial" HeaderText="Empresa" SortExpression="Empresa.RazonSocial"  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />--%>
                                <asp:BoundField DataField="Perfil.NombrePerfil" HeaderText="Perfil" SortExpression="Perfil.NombrePerfil"  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="Estado.Descripcion" HeaderText="Estado" SortExpression="Estado.Descripcion"  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
