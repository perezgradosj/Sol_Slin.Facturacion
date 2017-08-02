<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="AsignarPerfil.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Seguridad.AsignarPerfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <header>
        <%--<script src="../../Scripts/jquery-1.10.2.min.js"></script>--%>

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

        <div class="row">
            <div style="font-family: Cambria; font-size: medium">
                <asp:HyperLink runat="server" BackColor="Transparent" NavigateUrl="~/Views/Home/Inicio" Width="60" BorderStyle="None" Font-Bold="true" Text="Home" CssClass="form-control"></asp:HyperLink>
                <a style="font-family: Cambria; height: 35px" href="AsignarPerfil">/ Asignar Perfil</a>
            </div>
        </div>

        <div class="modal-header">
            <label style="font-family: Cambria; font-size: 16px"><i class="glyphicon glyphicon-user padding-right-small"></i>Asignar Perfil Usuario</label>
        </div>



        <asp:UpdatePanel runat="server">
            <ContentTemplate>



                <div class="modal-body" style="font-family: Cambria">

                    <div class="row">
                        <div class="col-md-2" style="max-width: 140px">
                            <label style="height: 27px; width: 100px; padding-top: 8px">Nombre Perfil:</label>
                        </div>
                        <div class="col-md-3">
                            <input runat="server" id="txtnombreperfil" class="form-control" readonly="readonly" />
                        </div>
                    </div>

                    <asp:Panel runat="server" BorderStyle="Solid" BorderColor="#c0c0c0" Visible="false">
                        <div class="container">

                            <div class="row">
                                <div class="col-lg-5">
                                    <label>Seleccionar Para Asignar Roles</label>
                                </div>
                            </div>

                            <div class="row">

                                <div class="col-lg-3">
                                    <asp:CheckBox runat="server" ID="MenuConfiguracion" Text="Configuración" Checked="false" OnCheckedChanged="MenuConfiguracion_CheckedChanged" AutoPostBack="True" />
                                </div>

                            </div>

                            <div class="row">
                                <div class="col-lg-2">
                                    <asp:CheckBox runat="server" ID="MenuEnvio" Text="Envió" Checked="false" OnCheckedChanged="MenuEnvio_CheckedChanged" AutoPostBack="True" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-2">
                                    <asp:CheckBox runat="server" ID="MenuConsultas" Text="Consulta" Checked="false" OnCheckedChanged="MenuConsultas_CheckedChanged" AutoPostBack="True" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-2">
                                    <asp:CheckBox runat="server" ID="MenuRegistro" Text="Registro" Checked="false" OnCheckedChanged="MenuRegistro_CheckedChanged" CausesValidation="True" AutoPostBack="True" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-2">
                                    <asp:CheckBox runat="server" ID="MenuSeguridad" Text="Seguridad" Checked="false" OnCheckedChanged="MenuSeguridad_CheckedChanged" AutoPostBack="True" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-2">
                                    <asp:CheckBox runat="server" ID="MenuMantenimiento" Text="Mantenimiento" Checked="false" OnCheckedChanged="MenuMantenimiento_CheckedChanged" AutoPostBack="True" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                    <br />

                    <asp:Panel runat="server" BorderStyle="Solid" BorderColor="#c0c0c0" BorderWidth="1.8px" GroupingText="# Menú del Sistema">

                        <div class="table-responsive">
                            <div class="row">
                                <div class="col-lg-8">
                                    <asp:TreeView ID="TreeView1" runat="server" ShowCheckBoxes="All" ExpandDepth="0" Font-Size="Small" ForeColor="#333333" Font-Names="Cambria" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged">
                                    </asp:TreeView>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                    <br />

                    <div class="row">
                        <%--<div class="col-md-1">
                        </div>--%>

                        <div class="col-md-5">
                            <label runat="server" style="font-size:large" id="lblmsje"></label>
                        </div>
                    </div>



                    <br />

                    <div class="row">
                        <div class="col-lg-4">
                            <a class="btn btn-primary" href="../Seguridad/AsignarMenuPerfil" style="height: 30px; font-family: Cambria; font-size: small"><i class="fa fa-arrow-left"></i> Cancelar</a>
                            <asp:Button runat="server" ID="btnGuardar" Height="30px" Font-Size="Small" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardar_Click" Enabled="false" />
                        </div>
                    </div>

                </div>
            </ContentTemplate>
        </asp:UpdatePanel>


    </div>

</asp:Content>
