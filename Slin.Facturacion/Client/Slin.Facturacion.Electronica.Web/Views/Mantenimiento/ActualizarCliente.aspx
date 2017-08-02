<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="ActualizarCliente.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Mantenimiento.ActualizarCliente" %>

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
                <a style="font-family: Cambria; height: 35px" href="ActualizarCliente">/ Actualizar Cliente</a>
            </div>
        </div>

        <div class="modal-header">
            <label style="font-family: Cambria; font-size: 16px"><i class="glyphicon glyphicon-user padding-right-small"></i> Datos del Usuario</label>
        </div>

        <div class="modal-body" style="font-family: Cambria">

            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27PX; padding-top: 8px">Razón Social:</label>
                </div>
                <div class="col-md-6">
                    <input runat="server" id="txtnombreRazonSocial" class="form-control" required="required" placeholder="nombre o razon social" />
                </div>
            </div>

            <div class="row">

                <%--<div class="col-md-2">
                    <label style="height: 27PX; padding-top: 8px">Tipo Documento:</label>
                </div>
                <div class="col-md-2" style="width: 165px">
                    <select runat="server" id="cbotipodocumento" class="form-control" style="width: 150px" onchange="this.form.submit()" onserverchange="cbotipodocumento_ServerChange">
                    </select>
                </div>--%>

                <div class="col-md-2">
                    <label style="height: 27PX; padding-top: 8px">Nro Documento:</label>
                </div>
                <div class="col-md-2">
                    <input type="text" runat="server" class="form-control" style="width: 165px" maxlength="11" id="txtnroDocumentocliente" placeholder="12345678" required="required" pattern=".{11,}" title="Minimo 11 Numeros" readonly="readonly" />
                </div>

            </div>

            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27PX; padding-top: 8px">Teléfono:</label>
                </div>
                <div class="col-md-3">
                    <input runat="server" id="txttelefono" class="form-control" required="required" onkeypress="return Validasolonumeros(event)" placeholder="teléfono" />
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27PX; padding-top: 8px">Dirección:</label>
                </div>
                <div class="col-md-6">
                    <input runat="server" id="txtdireccion" class="form-control" required="required" placeholder="dirección" />
                </div>
            </div>



            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27PX; padding-top: 8px">Correo Electrónico</label>
                </div>
                <div class="col-md-6">
                    <input runat="server" type="email" id="txtemail" class="form-control" placeholder="ejemplo@ejemplo.com" required="required" />
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

        </div>


        <div class="modal-footer">
            <div class="row">
                <div class="col-md-5">
                    <a href="ListadoCliente" class="btn btn-primary" style="font-size: small; height: 30px; font-family: Cambria"><i class="fa fa-arrow-left"></i> Cancelar</a>
                    <asp:Button runat="server" ID="btnActualizar" CssClass="btn btn-primary" Text="Actualizar" Height="30px" Font-Names="Cambria" Font-Size="Small" OnClick="btnActualizar_Click" Enabled="false" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
