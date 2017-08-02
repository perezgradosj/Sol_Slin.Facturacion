<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="ConfigurarServiceTime.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Configuracion.ConfigurarServiceTime" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <header>


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
                <a style="font-family: Cambria; height: 35px" href="ConfigurarServiceTime">/ Configurar Hora Servicio</a>
            </div>
        </div>

        <div class="modal-header">
            <label style="font-family: Cambria; font-size: 16px"><i class="fa fa-times padding-right-small"></i> Datos del Servicio</label>
        </div>

        <div class="modal-body" style="font-family: Cambria">

            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27PX; padding-top: 8px" hidden="hidden">Id Service:</label>
                </div>
                <div class="col-md-2">
                    <input runat="server" id="txtidService" class="form-control" required="required" visible="false" hidden="hidden" />
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27PX; padding-top: 8px" hidden="hidden">Codigo Service:</label>
                </div>
                <div class="col-md-3">
                    <input runat="server" id="txtcodeService" class="form-control" required="required" visible="false" hidden="hidden" />
                </div>
            </div>

            <div class="row">
                <div class="col-md-4">
                    <label style="height: 27PX; padding-top: 8px; font-size:medium">Detenga el Servicio antes de Actualizar</label>
                </div>
            </div>

            <div class="row">

                <div class="col-md-2">
                    <label style="height: 27PX; padding-top: 8px">Nombre Servicio:</label>
                </div>
                <div class="col-md-3">
                    <input type="text" runat="server" class="form-control" id="txtnameservice" required="required" readonly="readonly" />
                </div>

            </div>

            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27PX; padding-top: 8px">Hora de Ejecución:</label>
                </div>
                <div class="col-md-3">
                    <input runat="server" id="txthoraejecucion" class="form-control" placeholder="hh:mm" />
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27PX; padding-top: 8px">Intervalo (min):</label>
                </div>
                <div class="col-md-2">
                    <input runat="server" id="txtintervalo" class="form-control" required="required" placeholder="ingrese numeros" />
                </div>
            </div>



            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27PX; padding-top: 8px">Max Intentos Envío</label>
                </div>
                <div class="col-md-2">
                    <input runat="server" type="text" id="txtmaxintentosEnvio" class="form-control" placeholder="ingrese numeros" required="required" />
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <label style="padding-top: 8px">Estado:</label>
                </div>
                <div class="col-md-2" style="width: 100px">
                    <select runat="server" id="cboestado" class="form-control" style="width: 165px;"></select>
                </div>
            </div>

            <div class="row">
                <div class="col-md-8">
                    <label runat="server" id="lblstatusService" style="height: 27PX; padding-top: 8px; font-size:medium"></label>
                </div>
                <div class="col-md-4">
                    <label runat="server" id="lblresponse" style="height: 27PX; padding-top: 8px"></label>
                </div>
            </div>

        </div>


        <div class="modal-footer">
            <div class="row">
                <div class="col-md-3">
                    <asp:Button runat="server" ID="btnActualizar" CssClass="btn btn-primary" Text="Actualizar" Height="30px" Font-Names="Cambria" Font-Size="Small" OnClick="btnActualizar_Click" Enabled="false" />
                    <%--<button class="btn btn-primary" style="font-size: small; height: 30px; font-family: Cambria" ><i class="fa fa-sign-out"></i> Regresar</button>--%>
                    <%--<asp:Button runat="server" ID="btnRegresar" CssClass="btn btn-primary" Text="Regresar" Height="30px" Font-Names="Cambria" Font-Size="Small" OnClick="btnRegresar_Click" Enabled="true" />--%>
                    <a href="ConfiguracionTimeService" style="height: 30px; font-size: small; font-family: Cambria" class="btn btn-primary" id="btnRegresar">Regresar</a>
                </div>
                <div class="col-md-1">
                </div>
                <div class="col-md-4">
                    <asp:Button runat="server" ID="btnIniciar" CssClass="btn btn-primary" Text="Iniciar" Height="30px" Font-Names="Cambria" Font-Size="Small" OnClick="btnIniciar_Click" Enabled="false" />
                    <asp:Button runat="server" ID="btnDetener" CssClass="btn btn-primary" Text="Detener" Height="30px" Font-Names="Cambria" Font-Size="Small" OnClick="btnDetener_Click" Enabled="false" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
