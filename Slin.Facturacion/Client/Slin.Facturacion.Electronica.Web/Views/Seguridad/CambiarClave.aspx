<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="CambiarClave.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Seguridad.CambiarClave" %>

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
                <asp:HyperLink runat="server" BackColor="Transparent" NavigateUrl="~/Views/Home/Inicio" Width="60" BorderStyle="None" Font-Bold="true" Text="Home" CssClass="form-control" ></asp:HyperLink>
                <a style="font-family: Cambria; height: 35px" href="CambiarClave">/ Cambiar Contraseña</a>
            </div>
        </div>


        <div class="modal-header">
            <label style="font-family: Cambria; font-size: 16px"><i class="glyphicon glyphicon-user padding-right-small"></i> Cambiar Contraseña</label>
        </div>


        <div class="modal-body" style="font-family: Cambria; font-size: medium">

            <div class="row">
                <div class="col-md-3" style="max-width:170px">
                    <label style="height: 27px">Contraseña Antigua:</label>
                </div>
                <div class="col-md-4">
                    <input type="password" runat="server" class="form-control" id="txtantiguaclave" required="required" />
                </div>
            </div>

            <div class="row">
                <div class="col-md-3" style="max-width:170px">
                    <label style="height: 27px">Nueva Contraseña:</label>
                </div>
                <div class="col-md-4">
                    <input type="password" runat="server" id="txtnuevaclave" class="form-control" required="required" />
                </div>
            </div>

            <div class="row">
                <div class="col-md-3" style="max-width:170px">
                    <label style="height: 27px">Repetir Contraseña:</label>
                </div>
                <div class="col-md-4">
                    <input type="password" runat="server" id="txtconfirmarclave" class="form-control" required="required"/>
                </div>
            </div>

        </div>

        <div class="modal-footer" style="text-align: left">
            <div class="row">
                <div class="col-md-5" >
                    <a href="../../Views/Home/Inicio" class="btn btn-primary" style="font-size:small; height: 30px; font-family:Cambria"><i class="fa fa-arrow-left"></i> Cancelar</a>
                    <asp:Button runat="server" Font-Names="Cambria" Font-Size="Small" Height="30px" ID="btnCambiarClave" Text="Guardar" CssClass="btn btn-primary" OnClick="btnCambiarClave_Click" />
                    
                </div>
            </div>
        </div>

    </div>

</asp:Content>
