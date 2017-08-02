<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="ConfiguracionImpresion.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Configuracion.ConfiguracionImpresion" %>

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

        <script type="text/javascript">
        $(function () {
            $("#<%=TreeView1.ClientID %> input[type=checkbox]").bind("click", function () {
                var table = $(this).closest("table");
                if (table.next().length > 0 && table.next()[0].tagName == "DIV") {
                    var childDiv = table.next();
                    var isChecked = $(this).is(":checked");
                    $("input[type=checkbox]", childDiv).each(function () {
                        if (isChecked) {
                            $(this).attr("checked", "checked");
                        } else {
                            $(this).removeAttr("checked");
                        }
                    });
                }
                else {
                    var parentDIV = $(this).closest("DIV");
                    if ($("input[type=checkbox]", parentDIV).length == $("input[type=checkbox]:checked", parentDIV).length) {
                        $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");
                    }
                    else {
                        $("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
                    }
                }
            });
        })
    </script>

    </header>

    <div class="content" style="background-repeat: no-repeat; background-image: url(../../Img/home/fondopantalla.jpg); background-size: cover">

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

        <div class="row">
            <div style="font-family: Cambria; font-size: medium">
                <asp:HyperLink runat="server" BackColor="Transparent" NavigateUrl="~/Views/Home/Inicio" Width="60" BorderStyle="None" Font-Bold="true" Text="Home" CssClass="form-control"></asp:HyperLink>
                <a style="font-family: Cambria; height: 35px" href="ConfiguracionImpresion">/ Configurarción de Impresión</a>
            </div>
        </div>

        <div class="modal-header">
            <label style="font-family: Cambria; font-size: 16px"><i class="glyphicon glyphicon-user padding-right-small"></i>Configurar Proceso de Impresión</label>
        </div>

        <div class="modal-body" style="font-family: Cambria">

            <div class="row" hidden="hidden">
                <div class="col-md-2">
                    <label style="height: 27px; padding-top: 6px">Estado de Envío:</label>
                </div>
                <div class="col-md-3">
                    <select runat="server" id="cboestado" class="form-control" style="width: 180px;" onchange="this.form.submit()" onserverchange="cboestado_ServerChange"></select>
                </div>
            </div>

            <%--<br />--%>

            <b style="font-size:large">Tipos de Documento</b>
            <asp:Panel runat="server" BorderStyle="Solid" BorderColor="#c0c0c0" BorderWidth="1.8px" Visible="false">

                <div class="table-responsive">
                    <div class="row" >
                        <div class="col-lg-8">
                            <asp:TreeView ID="TreeView1" runat="server" ShowCheckBoxes="All" ExpandDepth="0" Font-Size="Small" ForeColor="#333333" Font-Names="Cambria" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged">
                            </asp:TreeView>
                        </div>
                    </div>
                </div>
            </asp:Panel>



            <div class="row">
                <div class="table-responsive" lang="es-pe">
                    <div class="col-lg-8" lang="es-pe">
                        <asp:GridView runat="server" Visible="true" ID="GVListaTypeDocument" CssClass="grid sortable {disableSortCols: []}" AutoGenerateColumns="false" OnRowDataBound="GVListaTypeDocument_RowDataBound"
                            OnRowUpdating="GVListaTypeDocument_RowUpdating"
                            OnRowEditing="GVListaTypeDocument_RowEditing"
                            OnRowCancelingEdit="GVListaTypeDocument_RowCancelingEdit"
                            AutoGenerateEditButton="false" 
                            EditRowStyle-BackColor="#339966" HeaderStyle-ForeColor="#009933" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center"
                            >

                            <Columns>
                                <asp:CommandField HeaderStyle-Width="60px" ControlStyle-Width="60px" ItemStyle-Width="60px" ShowEditButton="true" HeaderText="Editar"  HeaderStyle-ForeColor="White" ItemStyle-ForeColor="#003366" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" ItemStyle-Wrap="false"/>

                                <asp:BoundField HeaderStyle-Width="20px" ControlStyle-Width="20px" ItemStyle-Width="20px"  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="IdTipoDocumento" HeaderText="ID" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" ReadOnly="true" ApplyFormatInEditMode="true" />
                                <asp:BoundField HeaderStyle-Width="50px" ControlStyle-Width="50px" ItemStyle-Width="50px"  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="CodigoDocumento" HeaderText="Código" SortExpression="CodigoDocumento" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" ReadOnly="true" ApplyFormatInEditMode="true"/>
                                <asp:BoundField HeaderStyle-Width="180px" ControlStyle-Width="180px" ItemStyle-Width="180px"  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Left" DataField="Descripcion" HeaderText="Descripcion" SortExpression="Descripcion" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" ReadOnly="true" ApplyFormatInEditMode="true"/>

                                <asp:TemplateField HeaderStyle-Width="135px" ControlStyle-Width="135px" ItemStyle-Width="135px"  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" HeaderText="Estado" HeaderStyle-Wrap="true" ItemStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:DropDownList DataTextField="Descripcion" DataValueField="IdEstado" ID="cboEstadoRow" runat="server" Enabled="false"></asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <label style="font-family:Cambria" >Nota: Las Notas de Crédito y Notas de Débito tomarán la configuración del Documento de origen.</label>

            <br />
            <div class="row">
                <div class="col-lg-3">
                    <a class="btn btn-primary" href="../Home/Inicio.aspx" style="height: 30px; font-family:Cambria; font-size:small"><i class="fa fa-home"></i> Inicio</a>
                    <asp:Button runat="server" ID="btnGuardar" Height="30px" Font-Size="Small" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardar_Click" Enabled="false" Visible="false" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
