<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="ChangeCompany.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Seguridad.ChangeCompany" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="content" style="background-repeat: no-repeat; background-image: url(../../Img/home/fondopantalla.jpg); background-size: cover">

        <asp:UpdatePanel runat="server">
            <ContentTemplate>

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
                        <asp:HyperLink runat="server" BackColor="Transparent" NavigateUrl="~/Views/Seguridad/ChangeCompany" Width="60" BorderStyle="None" Font-Bold="true" Text="Home" CssClass="form-control"></asp:HyperLink>
                        <a style="font-family: Cambria; height: 35px" href="ChangeCompany">/ Cambiar de Empresa</a>
                    </div>
                </div>

                <div class="modal-header">
                    <label style="font-family: Cambria; font-size: 16px"><i class="glyphicon glyphicon-user padding-right-small"></i>Cambiar mi Usuario de Empresa</label>
                </div>



                <div class="modal-body" style="font-family: Cambria; font-size: medium">

                    <div class="row">
                        <div class="col-md-3" style="max-width: 180px">
                            <label style="height: 27px">Seleccionar Empresa:</label>
                        </div>
                        <div class="col-md-3">
                            <select runat="server" id="cboCompany" class="form-control" style="width: 200px;">
                                <%--<select runat="server" id="Select1" class="form-control" style="width: 200px;" onchange="this.form.submit()" onserverchange="cboCompany_ServerChange">--%>
                            </select>
                        </div>
                    </div>

                    <div class="row" hidden="hidden">
                        <div class="col-md-3" style="max-width: 180px">
                            <label style="height: 27px" hidden="hidden">Seleccionar Perfil:</label>
                        </div>
                        <div class="col-md-3">
                            <select runat="server" id="cboperfil" class="form-control" style="width: 200px;" visible="false">
                            </select>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-3" style="max-width: 180px">
                            <label style="height: 27px"></label>
                        </div>
                        <div class="col-md-5">
                            <label style="height: 27px">Nota: Si no está seguro de cambiar de empresa haga click en Cancelar</label>
                        </div>
                    </div>

                    <br />

                    <div class="row">
                        <div class="col-md-5">
                            <label runat="server" id="lblmsje" style="height: 27px; font-size:large"></label>
                        </div>
                    </div>

                </div>
                <div class="modal-footer" style="text-align: left">
                    <div class="row">
                        <div class="col-md-5">
                            <a href="../../Views/Home/Inicio" class="btn btn-primary" style="font-size: small; height: 30px; font-family: Cambria"><i class="fa fa-arrow-left"></i> Cancelar</a>
                            <asp:Button runat="server" Font-Names="Cambria" Font-Size="Small" Height="30px" ID="btnGuardar" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardar_Click" />
                        </div>
                    </div>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>

    </div>

</asp:Content>
