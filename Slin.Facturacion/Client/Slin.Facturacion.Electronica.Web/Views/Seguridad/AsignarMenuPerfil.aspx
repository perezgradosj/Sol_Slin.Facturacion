<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="AsignarMenuPerfil.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Seguridad.AsignarMenuPerfil" %>


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
            <div style="font-family: Cambria; font-size: medium">
                <asp:HyperLink runat="server" BackColor="Transparent" NavigateUrl="~/Views/Home/Inicio" Width="60" BorderStyle="None" Font-Bold="true" Text="Home" CssClass="form-control"></asp:HyperLink>
                <a style="font-family: Cambria; height: 35px" href="AsignarMenuPerfil">/ Asignar Menu Perfil</a>
            </div>
        </div>

        <div class="modal-header">
            <label style="font-family: Cambria; font-size: 16px"><i class="glyphicon glyphicon-user padding-right-small"></i> Seleccionar Perfil</label>
        </div>

        <div class="modal-body" style="font-family: Cambria">

            <asp:UpdatePanel runat="server">
                <ContentTemplate>

                

            <div class="row">
                <div class="col-md-2" style="max-width: 140px">
                    <label style="height: 27px;">Nombre Perfil:</label>
                </div>
                <div class="col-md-3">
                    <input runat="server" id="txtnombreperfil" class="form-control" autofocus="autofocus" autocomplete="off" placeholder="ingrese texto" />
                </div>
            </div>
            <%--<div class="row">
                <div class="col-md-2" style="max-width: 140px">
                    <label style="height: 27px;">Cod. Perfil:</label>
                </div>
                <div class="col-md-3">
                    <input runat="server" id="txtcodigo" class="form-control" visible="true" autocomplete="off" placeholder="ingrese texto" />
                </div>
            </div>--%>
            <div class="row">
                <div class="col-md-2" style="max-width: 140px">
                    <label></label>
                </div>
                <div class="col-md-2">
                    <asp:Button runat="server" ID="btnGuardar" Height="29px" Font-Names="Cambria" CssClass="btn btn-primary" Text="Guardar" Font-Size="Small" Enabled="false" OnClick="btnGuardar_Click" />
                </div>
            </div>
            <hr />

            <div class="row">
                <div class="table-responsive">
                    <asp:GridView runat="server" ID="GVListaPerfiles" CssClass="grid sortable {disableSortCols: []}" AutoGenerateColumns="false"
                        OnRowDataBound="GVListaPerfiles_RowDataBound">
                        <Columns>

                            <asp:TemplateField HeaderText="Sel"  HeaderStyle-ForeColor="White" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" ID="btnImgSelect" ImageUrl="~/Img/select.png" OnClick="btnImgSelect_Click" Width="15px" Height="15px" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="IdPerfil" HeaderText="ID"  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                            <asp:BoundField DataField="NombrePerfil" HeaderText="Nombre Perfil" SortExpression="NombrePerfil"  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                            <%--<asp:BoundField DataField="Codigo" HeaderText="Código" SortExpression="Codigo"  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" Visible="false" />--%>


                            <asp:TemplateField HeaderStyle-Width="20px" ControlStyle-Width="20px" ItemStyle-Width="20px" HeaderText="Eliminar"  HeaderStyle-ForeColor="White" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" ID="btnDelete" OnClick="btnDelete_Click" ImageUrl="~/Img/delete.png" Height="15px" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%--<asp:TemplateField HeaderText="Editar"  HeaderStyle-ForeColor="White" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" ID="btnImgEdit" ImageUrl="~/Img/editar.png" OnClick="btnImgEdit_Click" Width="15px" Height="15px" />
                                </ItemTemplate>
                            </asp:TemplateField>--%>

                        </Columns>
                    </asp:GridView>
                </div>
            </div>

                    </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
