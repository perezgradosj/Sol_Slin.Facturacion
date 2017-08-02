<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="RegistroEmpresa.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Mantenimiento.RegistroEmpresa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <header>
        <script src="../../Scripts/Validacion/Validaciones.js"></script>
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
                <a style="font-family: Cambria; height: 35px" href="RegistroEmpresa">/ Registrar Empresa</a>
            </div>
        </div>


        <div class="modal-header">
            <label style="font-family:Cambria; font-size: 16px"><i class="fa fa-bank"></i> Datos de la Empresa</label>
        </div>

        <div class="modal-body" style="font-family:Cambria; font-size:medium">
            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27PX; padding-top: 8px">Razon Social:</label>
                </div>
                <div class="col-md-6" >
                    <input runat="server" id="txtrazonsocial" class="form-control" required="required" placeholder="razon social"/>
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <label style="height:27PX; padding-top: 8px">Razon Comercial:</label>
                </div>
                <div class="col-md-6">
                    <input runat="server" id="txtrazoncomercial" class="form-control" required="required" placeholder="razon comercial"/>
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27PX; padding-top: 8px">Tipo Documento:</label>
                </div>
                <div class="col-md-2" style="width: 165px">
                    <select runat="server" id="cbotipodocumento" class="form-control" style="width: 150px" onchange="this.form.submit()" onserverchange="cbotipodocumento_ServerChange">
                    </select>
                </div>

                <div class="col-md-2" style="max-width:100px">
                    <label style="height: 27PX; padding-top: 8px">Numero:</label>
                </div>
                <div class="col-md-2">
                    <input type="text" runat="server" class="form-control" style="width: 165px" maxlength="11" id="txtruc" placeholder="12345678" required="required" pattern=".{11,}" title="Minimo 11 Numeros" />
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27PX; padding-top: 8px">Dirección:</label>
                </div>
                <div class="col-md-6">                    
                    <input runat="server" id="txtdireccion" class="form-control" required="required" placeholder="dirección"/>
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27PX; padding-top: 8px">Domicilio Fiscal:</label>
                </div>
                <div class="col-md-6">
                    <input runat="server" id="txtdomiciliofiscal" class="form-control" required="required" placeholder="domicilio fiscal"/>
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27PX; padding-top: 8px">Urbanización:</label>
                </div>
                <div class="col-md-6">
                    <input runat="server" id="txturbanizacion" class="form-control" required="required" placeholder="urbanización"/>
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27PX; padding-top: 8px">País:</label>
                </div>
                <div class="col-md-3">
                    <select runat="server" id="cbopais" class="form-control" onchange="this.form.submit()" onserverchange="cbopais_ServerChange" style="max-width:430px;">
                    </select>
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27PX; padding-top: 8px">Departamento:</label>
                </div>
                <div class="col-md-2" style="width: 165px">
                    <select runat="server" id="cbodepartamento" class="form-control" style="width:160px" onchange="this.form.submit()" onserverchange="cbodepartamento_ServerChange" >
                    </select>
                </div>

                <div class="col-md-2" style="max-width:100px; ">
                    <label style="height: 27PX; padding-top: 8px">Provincia:</label>
                </div>
                <div class="col-md-2" style="width: 165px">
                    <select runat="server" id="cboprovincia" class="form-control" style="width: 160px" onchange="this.form.submit()" onserverchange="cboprovincia_ServerChange">
                    </select>
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27PX; padding-top: 8px">Distrito:</label>
                </div>
                <div class="col-md-2" style="width: 165px">
                    <select runat="server" id="cbodistrito" class="form-control" style="width:160px" onchange="this.form.submit()" onserverchange="cbodistrito_ServerChange">
                    </select>
                </div>

                <div class="col-md-2" style="max-width:100px">
                    <label>Ubigeo:</label>
                </div>

                <div class="col-md-2">
                    <input runat="server" id="txtubigeo" class="form-control" required="required" style="max-width: 160px" placeholder="seleccione distrito" />
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27PX; padding-top: 8px">Central Telefónica:</label>
                </div>
                <div class="col-md-3">
                    <input runat="server" id="txttelefono" class="form-control" required="required" onkeypress="return Validasolonumeros(event)" placeholder="teléfono" />
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27PX; padding-top: 8px">FAX:</label>
                </div>
                <div class="col-md-3">
                    <input runat="server" id="txtfax" class="form-control" onkeypress="return Validasolonumeros(event)" placeholder="fax" />
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27PX; padding-top: 8px">Página Web:</label>
                </div>
                <div class="col-md-6">
                    <input runat="server" id="txtpaginaweb" class="form-control" placeholder="www.ejemplo.com" />
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27PX; padding-top: 8px">Correo Electrónico</label>
                </div>
                <div class="col-md-6">
                    <input runat="server" type="email" id="txtemail" class="form-control" placeholder="ejemplo@ejemplo.com"/>
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <label style="height: 27PX; padding-top: 8px">Fecha Registro:</label>
                </div>
                <div class="col-md-2">
                    <input runat="server" id="txtfecharegistro" class="form-control" style="width: 150px;" readonly="readonly" />
                </div>

                <div class="col-md-1">
                    <label style="padding-top: 8px">Estado:</label>
                </div>
                <div class="col-md-2" style="width:100px">
                    <select runat="server" id="cboestado" class="form-control" style="width: 165px;">
                    </select>
                </div>
            </div>

        </div>

        <div class="modal-footer">
            <div class="row">
                <div class="col-md-5">
                    <asp:Button runat="server" ID="btnGuardar" Height="30px" Font-Names="Cambria" Font-Size="Small" CssClass="btn btn-primary" Text="Registrar" ToolTip="Guardar Registro" OnClick="btnGuardar_Click" Enabled="false" />
                    <input type="reset" class="btn btn-primary" style="font-size:small; height:30px; font-family:Cambria" value="Limpiar" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
