<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="ImportarUsuarios.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Mantenimiento.ImportarUsuarios" %>

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
                <a style="font-family: Cambria; height: 35px" href="ImportarUsuarios">/ Importar Usuarios desde Excel</a>
            </div>
        </div>

        <div class="modal-header">
            <label style="font-family: Cambria; font-size: 16px"><i class="glyphicon glyphicon-user padding-right-small"></i>Información del Cliente</label>
        </div>

        <div class="modal-body" style="font-family: Cambria">
            <div class="row">
                <div class="col-lg-4">
                    <asp:FileUpload ID="FileUpload1" runat="server" />
                </div>
                <%--<div class="col-lg-2" style="max-width:120px">
                    <label style="height: 27px; padding-top: 7px">Nombre Hoja:</label>
                </div>
                <div class="col-lg-2">
                    <input runat="server" id="txtnombrehoja" class="form-control" placeholder="Hoja1" visible="false" />
                </div>--%>
            </div>
            <br />
            <div class="row">
                <div class="col-lg-3">
                    <asp:Button runat="server" Height="27px" Font-Names="Cambria" Font-Size="Small" CssClass="btn btn-primary" ID="btnValidar" Text="Validar Registros" OnClick="btnValidar_Click" ToolTip="Validar Datos" />
                </div>
            </div>
            <hr />

            <asp:Panel runat="server" ID="panel1" Visible="true">
                <div class="row">
                    <div class="col-lg-10">
                        <label runat="server" id="lblmensaje2" style="font-family: Cambria; font-size: medium"></label>
                    </div>
                </div>
                <div class="row">
                    <div class="table-responsive">
                        <asp:GridView CssClass="grid sortable {disableSortCols: []}" runat="server" ID="GVListaUsuariosExcel" AutoGenerateColumns="false">
                            <Columns>
                                <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Estado.IdEstado" HeaderText="IdEstado" Visible="true" SortExpression="Estado.IdEstado" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Perfil.IdPerfil" HeaderText="Perfil" SortExpression="Perfil.IdPerfil" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Empleado.Nombres" HeaderText="Nombres" SortExpression="Empleado.Nombres" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Empleado.ApePaterno" HeaderText="Ape Paterno" SortExpression="Empleado.ApePaterno" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Empleado.ApeMaterno" HeaderText="Ape Materno" SortExpression="Empleado.ApeMaterno" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Empleado.DNI" HeaderText="Dni" SortExpression="Empleado.DNI" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Username" HeaderText="Username" SortExpression="Username" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </asp:Panel>
            
            <asp:Panel runat="server" ID="panel2" Visible="false">
                <h3 runat="server" id="lblmensajeok">Todos los Registros son correctos ya puede realizar la Carga</h3>
            </asp:Panel>
        </div>

        <div class="modal-footer">
            <div class="row">
                <div class="col-md-5">
                    <asp:Button runat="server" ID="btnGuardar" Height="30px" Font-Names="Cambria" Font-Size="Small" CssClass="btn btn-primary" Text="Registrar" ToolTip="Guardar Registro" OnClick="btnGuardar_Click" Enabled="false" />
                    <input type="reset" class="btn btn-primary" style="font-size: small; height: 30px; font-family: Cambria" value="Limpiar" />
                </div>
            </div>
        </div>

    </div>
</asp:Content>
