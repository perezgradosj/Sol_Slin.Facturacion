<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="ConfiguracionAmbienteTrabj.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Configuracion.ConfiguracionAmbienteTrabj" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="content" style="background-repeat: no-repeat; background-image: url(../../Img/home/fondopantalla.jpg); background-size: cover">

        <div class="main-content">
            <div class="row">
                <div class="col-lg-3">
                    <%--<img runat="server" id="logoEmpresa" style="align-items:stretch; max-width:240px;" />--%>
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
                <a style="font-family: Cambria; height: 35px" href="ConfiguracionAmbienteTrabj">/ Configurarción Ambiente Trabajo</a>
            </div>
        </div>

        <div class="modal-header">
            <label style="font-family: Cambria; font-size: 16px"><i class="fa fa-windows padding-right-small"></i>Seleccionar Ambiente de Trabajo para el ADE</label>
        </div>

        <div class="modal-body" style="font-family: Cambria">

            <div class="row">
                <div class="col-md-2">
                    <b style="height: 27px; padding-top: 8px; font-size:medium">Ambiente Actual:</b>
                </div>
                <div class="col-md-5">
                    <b runat="server" id="lblambienteDesc" style="height: 27px; padding-top: 8px; font-size:medium"></b>
                </div>
            </div>

            <hr />
            <%--<div class="row">
                
            </div>--%>

            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27px; padding-top: 5px">Lista de Ambientes</label>
                </div>

                <div class="col-md-3">
                    <select runat="server" id="cboambiente" class="form-control"></select>
                </div>
            </div>


            <%--<hr />--%>
            <div class="modal-footer">
                <%--<div class="col-md-1">
                    <label></label>
                </div>--%>
                <div class="col-md-2">
                    <asp:Button runat="server" ID="btnGuardar" Visible="false" Text="Guardar Cambios" Font-Size="Small" CssClass="btn btn-primary" Height="30" OnClick="btnGuardar_Click" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
