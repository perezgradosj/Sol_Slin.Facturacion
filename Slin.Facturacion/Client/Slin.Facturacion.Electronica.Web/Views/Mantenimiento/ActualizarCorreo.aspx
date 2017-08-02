<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="ActualizarCorreo.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Mantenimiento.ActualizarCorreo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <header>
        <script type="text/javascript">

            function HabilitarTextChangePWD() {
                
                if ($('#chkpwd').prop('checked')) {
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('txtpassword') != -1) {
                            document.forms[0].elements[i].disabled = false;
                        }
                    }

                } else {
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('txtpassword') != -1) {
                            document.forms[0].elements[i].disabled = true;
                        }
                    }
                }
            }
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
                <a style="font-family: Cambria; height: 35px" href="ActualizarCorreo">/ Actualizar de Correo</a>
            </div>
        </div>


        <div class="modal-header">
            <label style="font-family:Cambria; font-size: 16px"><i class="fa fa-bank"></i> Información de la Empresa</label>
        </div>

        <div class="modal-body" style="font-family:Cambria">


            <div class="row">

                <div class="col-lg-2" style="max-width:140px">
                    <label style="height:27px; padding-top: 8px">Empresa:</label>
                </div>
                <div class="col-lg-4">
                    <select runat="server" id="cboempresa" class="form-control" aria-readonly="false" disabled="disabled"></select>
                </div>

            </div>

            

            <div class="row">

                <div class="col-lg-2" style="max-width:140px">
                    <label style="height:27px; padding-top: 8px">Email:</label>
                </div>
                <div class="col-lg-4">
                    <input runat="server" id="txtemail" class="form-control" required="required" autocomplete="off" />
                </div>

            </div>


            <div class="row">

                <div class="col-lg-2" style="max-width:140px">
                    <label id="lblpwd" style="height:27px; padding-top: 8px">Contraseña:</label>
                </div>
                <div class="col-lg-4">
                    <input type="password" runat="server" id="txtpassword" class="form-control" disabled="disabled" />
                </div>

                <div class="col-lg-1" style="max-width:10px">
                    <input type="checkbox" id="chkpwd" onclick="HabilitarTextChangePWD();"/>
                </div>
                <div class="col-lg-2">
                    <label style="height:27px;">Cambiar Contraseña</label>
                </div>

            </div>

            <div class="row">
                <div class="col-lg-2" style="max-width:140px">
                    <label style="height:27px; padding-top: 8px">Dominio:</label>
                </div>
                <div class="col-lg-4">
                    <input runat="server" id="txtDomain" class="form-control" autocomplete="off"/>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-2" style="max-width:140px">
                    <label style="height:27px; padding-top: 7px">IP:</label>
                </div>
                <div class="col-lg-2">
                    <input runat="server" id="txtIP" class="form-control"/>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-2" style="max-width:140px">
                    <label style="height:27px; max-width:140px; padding-top: 7px;">Puerto:</label>
                </div>
                <div class="col-lg-2">
                    <input runat="server" id="txtPort" class="form-control" required="required" autocomplete="off"/>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-2" style="max-width:140px">
                    <label style="height:27px; padding-top: 8px" hidden="hidden">Puerto Alternativo:</label>
                </div>
                <div class="col-lg-3">
                    <input runat="server" id="txtPortAlternative" class="form-control" hidden="hidden" visible="false" />
                </div>
            </div>

            <div class="row">
                <div class="col-lg-2" style="max-width:140px">
                    <label style="height:27px; padding-top: 7px">Tipo Mail:</label>
                </div>
                <div class="col-lg-2">
                    <select runat="server" id="cboTypeMail" class="form-control"></select>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-2" style="max-width:140px">
                    <label style="height:27px; padding-top: 7px">Estado:</label>
                </div>
                <div class="col-lg-2">
                    <select runat="server" id="cboestado" class="form-control"></select>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-2" style="max-width:140px">
                    <label style="height:27px; padding-top: 7px">Usa SSL?</label>
                </div>
                <div class="col-lg-2">
                    <select runat="server" id="cboSSL" class="form-control"></select>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-2" style="max-width:140px">
                </div>

                <div class="col-lg-5">
                    <a href="ListadoCorreo" class="btn btn-primary" style="height:30px; font-size:small; font-family:Cambria"><i class="fa fa-arrow-left"></i> Cancelar</a>
                    <asp:Button runat="server" ID="btnActualizar" Height="30px" Font-Size="Small" CssClass="btn btn-primary" Text="Actualizar" Enabled="false" OnClick="btnActualizar_Click" />
                </div>
            </div>

        </div>
    </div>
</asp:Content>
