<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="AnularDocumento.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Facturacion.AnularDocumento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <header>
        <script src="../../Scripts/Validacion/Validaciones.js"></script>

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

        <%--<br />--%>

        <hr />

        <div class="row">
            <div style="font-family: Cambria; font-size: medium">
                <asp:HyperLink runat="server" BackColor="Transparent" NavigateUrl="~/Views/Home/Inicio" Width="60" BorderStyle="None" Font-Bold="true" Text="Home" CssClass="form-control"></asp:HyperLink>
                <a style="font-family: Cambria; height: 35px" href="AnularDocumento">/ Anular Documento</a>
            </div>
        </div>

        <div class="modal-header">
            <label style="font-family: Cambria; font-size: 16px"><i class="fa fa-book"></i> Datos del Documento</label>
        </div>

        <asp:UpdatePanel runat="server">
            <ContentTemplate>

                <div class="modal-body" style="font-family: Cambria;">
                    <div class="row">
                        <div class="col-lg-2">
                            <label style="height: 27px; padding-top: 8px">Tipo Documento:</label>
                        </div>
                        <div class="col-lg-3">
                            <select runat="server" id="cbotipodocumento" class="form-control"></select>
                        </div>

                        <div class="col-lg-1" style="max-width: 5px">
                        </div>

                    </div>

                    <div class="row">

                        <div class="col-lg-2">
                            <label style="height: 27px; padding-top: 8px">Serie - Correlativo:</label>
                        </div>
                        <div class="col-lg-2" style="max-width: 120px">
                            <input runat="server" id="txtserie" class="form-control" required="required" />
                        </div>

                        <div class="col-lg-1" style="width: 1px">
                            <label></label>
                        </div>
                        <div class="col-lg-3" style="max-width: 206px">
                            <input runat="server" id="txtnrodocumento" class="form-control" required="required" onkeypress="return Validasolonumeros(event)" maxlength="12" />
                        </div>



                        <div class="col-lg-1" style="max-width: 5px">
                            <label></label>
                        </div>

                        <div class="col-lg-1" style="padding-top: 0px;">
                            <a runat="server" style="font-family: Cambria; max-height: 26px; max-width: 70px; font-size: smaller" id="btnConsultar" class="btn btn-primary" onserverclick="btnConsultar_ServerClick"><i class="fa fa-search"></i> Buscar</a>
                        </div>

                    </div>

                    <div class="row">
                        <div class="col-lg-2">
                            <label style="height: 27px; padding-top: 8px">Motivo de Anulación:</label>
                        </div>
                        <div class="col-lg-4">
                            <textarea runat="server" id="txtmotivoanul" class="form-control" rows="2" required="required"></textarea>
                        </div>
                    </div>

                    <br />
                    <div class="row">



                        <asp:Panel runat="server" ID="DinamicPanel" Visible="false">
                            <div class="col-lg-2">
                                <label>Fecha Documento:</label>
                            </div>
                            <div class="col-lg-2">
                                <input runat="server" id="txtfecha" class="form-control" readonly="readonly" />
                            </div>

                            <div class="col-lg-2">
                                <label runat="server" id="lblmonto_total"></label>
                            </div>
                            <div class="col-lg-2">
                                <input runat="server" id="txtmontototal" class="form-control" readonly="readonly" />
                            </div>

                            <div class="col-lg-1">
                                <input runat="server" id="txtCodigoDocumento" readonly="readonly" visible="false" />
                            </div>
                        </asp:Panel>

                        <asp:Panel runat="server" ID="DinamicPanel2" Visible="false">
                            <div class="col-lg-6">
                                <h4 runat="server" id="lblmensaje" style="font-family: Cambria;"></h4>
                            </div>
                        </asp:Panel>


                    </div>

                </div>



                <div class="modal-footer">
                    <div class="row">
                        <div class="col-lg-5">
                            <a class="btn btn-primary" href="DocumentosAnulados" style="height: 30px; font-size: small; font-family: Cambria"><i class="fa fa-arrow-left"></i> Cancelar</a>
                            <asp:Button runat="server" ID="btnRegistrar" Height="30px" Font-Names="Cambria" Text="Registrar" Font-Size="Small" CssClass="btn btn-primary" OnClick="btnRegistrar_Click" Enabled="false" />

                        </div>
                    </div>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>

    </div>

</asp:Content>
