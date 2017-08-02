<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="ListadoCorreo.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Mantenimiento.ListadoCorreo" %>

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
                <a style="font-family: Cambria; height: 35px" href="ListadoCorreo">/ Listado de Correo</a>
            </div>
        </div>

        <ul class="nav nav-tabs">
            <li class="active"><a style="background-color:transparent; border-color:none; font-family: Cambria; font-size: 16px; font: bold" href="#home" data-toggle="tab"><i class="fa fa-filter"></i> Criterios de Búsqueda</a></li>
        </ul>



        <div class="modal-body" style="font-family: Cambria">


            <div class="row">
                <div class="col-md-1">
                    <label style="height: 27px; padding-top: 8px">Email:</label>
                </div>

                <div class="col-md-4">
                    <input runat="server" id="txtemail" class="form-control" placeholder="ejemplo@ejemplo.com" maxlength="150" />
                </div>
            </div>


            <%--<div class="row">
                <div class="col-md-2" style="max-width: 130px;">
                    <label style="height: 27px; padding-top: 8px">Razon Social:</label>
                </div>

                <div class="col-md-4">
                    <input runat="server" id="txtrazonsocial" class="form-control" placeholder="razon social" />
                </div>
            </div>--%>

            <div class="row">
                <div class="col-md-1">
                    <label style="height: 27px; padding-top: 8px">Estado:</label>
                </div>

                <div class="col-md-2">
                    <select runat="server" id="cboestado" class="form-control"></select>
                </div>
            </div>


            <div class="row">
                <div class="col-md-1">
                    <label style="max-width: 130px; height: 27px; padding-top: 8px"></label>
                </div>
                <div class="col-md-5">
                    <a href="RegistroCorreo" runat="server" id="btnNuevo" class="btn btn-primary" style="height: 30px; font-size: small; font-family: Cambria" hidden="hidden" aria-hidden="false"><i class="fa fa-plus"></i> Nuevo</a>
                    <a runat="server" style="height: 30px; font-size: small; font-family: Cambria" id="btnBuscar" class="btn btn-primary" onserverclick="btnBuscar_ServerClick" aria-hidden="false" hidden="hidden"><i class="fa fa-search"> Buscar</i></a>
                    
                </div>
            </div>

            <hr />
            <div class="row">
                <div class="table-responsive">
                    <asp:GridView runat="server" ID="GVListadoCorreo" CssClass="grid sortable {disableSortCols: []}" AutoGenerateColumns="false">

                        <Columns>
                            <asp:TemplateField HeaderText="Editar"  HeaderStyle-ForeColor="White" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" ID="btnImgEditar" OnClick="btnImgEditar_Click" ImageUrl="~/Img/editar.png" Height="15px" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="IdEmail" HeaderText="ID" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Left" DataField="Empresa.RazonSocial" HeaderText="Empresa" SortExpression="Empresa.RazonSocial" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Left" DataField="Email" HeaderText="Correo " SortExpression="Email" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Password" HeaderText="Password" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" Visible="false" />

                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Domain" HeaderText="Dominio" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="IP" HeaderText="IP" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Port" HeaderText="Puerto" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Estado.Descripcion" HeaderText="Estado" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                            <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="TypeMail" HeaderText="Tipo" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" Visible="true" />

                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            
            <div class="modal-footer">
                <div class="col-md-2">
                    <a href="NotificationsEmail" runat="server" id="btnVerNotificationsMai" class="btn btn-primary" style="height: 30px; font-size: small; font-family: Cambria"><i class="fa fa-mobile"></i> Ver Correo de Notificaciones</a>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
