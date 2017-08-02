<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Home.Profile" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

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
                <a style="font-family: Cambria; height: 35px" href="Profile">/ Mis Datos</a>
            </div>
        </div>


        <div class="modal-header">
            <label style="font-family:Cambria; font-size:17px"><i class="glyphicon glyphicon-user padding-right-small"></i> Datos Personales</label>
        </div>


        <div class="modal-body" style="font-family:Cambria; font-size:medium">

            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27px; padding-top: 8px">Nombres:</label>
                </div>
                <div class="col-md-4">
                    <input runat="server" id="txtnombres" class="form-control" readonly="readonly" />
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27px; padding-top: 8px">Apellidos:</label>
                </div>
                <div class="col-md-4">
                    <input runat="server" id="txtapellidos" class="form-control" readonly="readonly"/>
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27px; padding-top: 8px">Usuario:</label>
                </div>
                <div class="col-md-3">
                    <input runat="server" id="txtusuario" class="form-control" readonly="readonly"/>
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27px; padding-top: 8px">DNI:</label>
                </div>
                <div class="col-md-3">
                    <input runat="server" class="form-control" id="txtdni" readonly="readonly" />
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27px; padding-top: 8px">Telefono:</label>
                </div>
                <div class="col-md-3">
                    <input runat="server" id="txttelefono" class="form-control" readonly="readonly" />
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27px; padding-top: 8px">Email:</label>
                </div>
                <div class="col-md-4">
                    <input runat="server" type="email" id="txtemail" class="form-control" readonly="readonly" />
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27px; padding-top: 8px">Dirección:</label>
                </div>
                <div class="col-md-5">
                    <input runat="server" id="txtdireccion" class="form-control" readonly="readonly" />
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27px; padding-top: 8px">Empresa:</label>
                </div>
                <div class="col-md-5">
                    <input runat="server" id="txtempresa" class="form-control" readonly="readonly"/>
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27px; padding-top: 8px" hidden="hidden">RUC:</label>
                </div>
                <div class="col-md-3">
                    <input runat="server" id="txtruc" visible="false" class="form-control"/>
                </div>
            </div>

        </div>

        <div class="modal-footer" style="text-align:left">
            <div class="row">
                <div class="col-md-3">
                    <a href="Inicio" style="height: 30px; font-size: small; font-family:Cambria" class="btn btn-primary"><i class="fa fa-home"> Inicio</i></a>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
