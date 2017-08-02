<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="NotificationsEmail.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Mantenimiento.NotificationsEmail" %>

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

                    if (grid.hasClass("sortable") && grid.find("tbody:second > tr").length > 0) {
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
                <a style="font-family: Cambria; height: 35px" href="NotificationsEmail">/ Correos de Notificaciones</a>
            </div>
        </div>

        <div class="modal-header">
            <label style="font-family: Cambria; font-size: 16px"><i class="glyphicon glyphicon-user padding-right-small"></i>Cuentas de correo para las notificaciones</label>
        </div>

        <div class="modal-body" style="font-family: Cambria">

            <div class="row">
                <div class="col-md-1">
                    <label style="height: 27px; padding-top: 6px">Mail:</label>
                </div>
                <div class="col-md-4">
                    <input runat="server" id="txtemailnotify" type="email" class="form-control" placeholder="ejemplo@ejemplo.com" autocomplete="off" autofocus="autofocus" />
                </div>
            </div>

            <div class="row">
                <div class="col-md-1">
                    <label style="height: 27px; padding-top: 6px">Contacto:</label>
                </div>
                <div class="col-md-4">
                    <input runat="server" id="txtcontactname" class="form-control" placeholder="ingrese texto" />
                    <label runat="server" id="lbltemp_mail" visible="false"></label>
                    <label runat="server" id="lbltymail" visible="false"></label>
                </div>
            </div>

            <div class="row">
                <div class="col-md-1">
                    <label style="height:27px; padding-top: 7px">Tipo:</label>
                </div>
                <div class="col-lg-2">
                    <select runat="server" id="cboTypeMail" class="form-control"></select>
                </div>
            </div>

            <div class="row">
                <div class="col-md-1">
                </div>
                <div class="col-md-1">
                    <asp:Button runat="server" ID="btnGuardar" Height="30px" Font-Size="Small" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardar_Click" Enabled="false" />
                </div>
            </div>



            <br />

            <b style="font-size: small">Lista de Cuentas Registradas</b>
            <br />

            <div class="row">
                <div class="table-responsive" lang="es-pe">
                    <div class="col-lg-8" lang="es-pe">
                        <asp:GridView runat="server" Visible="true" ID="GVListNotificationsMail" CssClass="grid sortable {disableSortCols: []}" AutoGenerateColumns="false"
                            AutoGenerateEditButton="false" 
                            HeaderStyle-ForeColor="#009933" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center">

                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="20px" ControlStyle-Width="20px" ItemStyle-Width="20px"  HeaderText="Editar"  HeaderStyle-ForeColor="White" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" ID="btnImgEditar" OnClick="btnImgEditar_Click" ImageUrl="~/Img/editar.png" Height="15px" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField HeaderStyle-Width="300px" ControlStyle-Width="300px" ItemStyle-Width="300px"  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Email" HeaderText="Email" SortExpression="Email" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" ReadOnly="true" ApplyFormatInEditMode="true" />
                                <asp:BoundField HeaderStyle-Width="250px" ControlStyle-Width="250px" ItemStyle-Width="250px"  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="ContactName" HeaderText="Nombre Contacto" SortExpression="ContactName" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" ReadOnly="true" ApplyFormatInEditMode="true" />
                                <asp:BoundField HeaderStyle-Width="250px" ControlStyle-Width="250px" ItemStyle-Width="250px"  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="TypeMail" HeaderText="Tipo" SortExpression="TypeMail" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" ReadOnly="true" ApplyFormatInEditMode="true" />

                                <asp:TemplateField HeaderStyle-Width="20px" ControlStyle-Width="20px" ItemStyle-Width="20px"  HeaderText="Eliminar"  HeaderStyle-ForeColor="White" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" ID="btnDelete" OnClick="btnDelete_Click" ImageUrl="~/Img/delete.png" Height="15px" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <br />
            <div class="row">
                <div class="col-lg-3">
                    <a class="btn btn-primary" href="ListadoCorreo" style="height: 30px; font-family: Cambria; font-size: small"><i class="fa fa-arrow-left"></i> Regresar</a>

                </div>
            </div>
        </div>
    </div>

</asp:Content>
