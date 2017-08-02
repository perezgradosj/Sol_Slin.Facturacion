<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="DetalleAmbiente.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Configuracion.DetalleAmbiente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <header>

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


        <script type="text/javascript">

            function HabilitarTextChangePWD() {

                if ($('#chkpwd').prop('checked')) {
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('txtclaveamb') != -1) {
                            document.forms[0].elements[i].disabled = false;
                        }
                    }

                } else {
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('txtclaveamb') != -1) {
                            document.forms[0].elements[i].disabled = true;
                        }
                    }
                }
            }
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
                    <label runat="server" id="lblempresa" style="font-size: medium; font-family: Cambria;"></label>
                </div>
            </div>
        </div>

        <%--<br />--%>
        <hr />

        <div class="row">
            <div style="font-family: Cambria; font-size: medium">
                <asp:HyperLink runat="server" BackColor="Transparent" NavigateUrl="~/Views/Home/Inicio" Width="60" BorderStyle="None" Font-Bold="true" Text="Home" CssClass="form-control"></asp:HyperLink>
                <a style="font-family: Cambria; height: 35px" href="DetalleAmbiente">/ Detalle Ambiente Seleccionado</a>
            </div>
        </div>

        <div class="modal-header">
            <%--<label style="font-family: Cambria; font-size: 16px"><i class="glyphicon glyphicon-user padding-right-small"></i>Configuración</label>--%>

            <b runat="server" id="lblambienteselec" style="height: 27px; font-size: larger; font-family: Cambria"></b>
            <label runat="server" id="lblidambSelected" visible="false" hidden="hidden"></label>
        </div>

        <div class="modal-body" style="font-family: Cambria">
            <!-- START -->
            <%--<div class="row">
                <div class="col-md-3" style="max-width: 250px">
                    <b runat="server" id="lblambienteselec" style="height: 27px; width: 250px; padding-top: 8px"></b>
                </div>
            </div>

            <br />--%>
            <b style="font-size: large">Tipos de Documento</b>
            <asp:Panel runat="server" BorderStyle="Solid" BorderColor="#c0c0c0" BorderWidth="1.8px">

                <div class="table-responsive">
                    <div class="row">
                        <div class="col-lg-8">
                            <asp:TreeView ID="TreeView1" runat="server" ShowCheckBoxes="All" ExpandDepth="0" Font-Size="Small" ForeColor="#333333" Font-Names="Cambria" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged">
                            </asp:TreeView>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <!-- END -->
            <br />
            <%--<hr />--%>
            <%--<div class="modal-footer">--%>
            <div class="row">
                
                <div class="col-md-4" >
                    <a href="ConfiguracionURL.aspx" class="btn btn-primary" style="font-size: small; height: 30px; font-family: Cambria"><i class="fa fa-arrow-left"></i> Cancelar</a>

                    <asp:Button runat="server" ID="btnGuardar" Text="Guardar Cambios" Font-Size="Small" CssClass="btn btn-primary" Height="30" OnClick="btnGuardar_Click" />
                </div>
            </div>


            <hr />
            <h5 style="font-family:Cambria">Datos de para envío a SUNAT según ambiente</h5>
            <div class="row" style="font-family:Cambria">
                <div class="col-md-2" style="max-width:60px">
                    <label style="height:27px; padding-top: 8px">Usuario:</label>
                </div>
                <div class="col-md-5">
                    <input runat="server" type="text" id="txtuseramb" class="form-control" required="required" placeholder="ingrese usuario" autocomplete="off"/>
                </div>
            </div>

            <div class="row" style="font-family:Cambria">
                <div class="col-md-2" style="max-width:60px">
                    <label style="height:27px; padding-top: 8px">Clave:</label>
                </div>
                <div class="col-md-5">
                    <input runat="server" disabled="disabled" type="password" id="txtclaveamb" class="form-control" required="required" placeholder="ingrese clave" autocomplete="off"/>
                </div>

                <div class="col-lg-1" style="max-width:10px">
                    <input type="checkbox" id="chkpwd" onclick="HabilitarTextChangePWD();"/>
                </div>
                <div class="col-lg-2">
                    <label style="height:27px;">Cambiar Contraseña</label>
                </div>
                <div class="col-lg-1">
                    <label runat="server" id="lbluseclaveamb" visible="false" hidden="hidden"></label>
                </div>
            </div>
            <br />
            <div class="row" style="font-family:Cambria">
                <div class="col-md-1" style="max-width:10px">
                    <label></label>
                </div>
                <div class="col-md-2" style="max-width: 110px">
                    <asp:Button runat="server" ID="btnSaveUserAmb" Text="Guardar Cambios" Font-Size="Small" CssClass="btn btn-primary" Height="30" OnClick="btnSaveUserAmb_Click"/>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
