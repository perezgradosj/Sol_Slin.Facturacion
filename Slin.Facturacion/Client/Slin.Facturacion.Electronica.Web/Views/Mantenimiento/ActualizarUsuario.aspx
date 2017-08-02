<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="ActualizarUsuario.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Mantenimiento.ActualizarUsuario" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <header>
        <link rel="stylesheet" href="../../Content/DatetimePicker/bootstrap-datepicker.css">
        <script src="../../Content/DatetimePicker/bootstrap-datepicker.min.js"></script>

        <script type="text/javascript">
            $(document).ready(function () {
                var dp = $('#<%=txtfechaexpiracion.ClientID%>');
                dp.datepicker({
                    changeMonth: true,
                    changeYear: true,
                    format: "dd/mm/yyyy",
                    language: "tr"
                }).on('changeDate', function (ev) {
                    $(this).blur();
                    $(this).datepicker('hide');
                });
            });
        </script>

        <script src="../../Scripts/Validacion/Validaciones.js"></script>


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
                <a style="font-family: Cambria; height: 35px" href="ActualizarUsuario">/ Actualizar Usuario</a>
            </div>
        </div>

        <div class="modal-header">
            <label style="font-family:Cambria; font-size: 16px"><i class="glyphicon glyphicon-user padding-right-small"></i> Datos del Usuario</label>
        </div>

        <div class="modal-body" style="font-family:Cambria">

            <div class="row">
                <div class="col-md-2" style="max-width:130px">
                    <label style="height:27px; padding-top: 8px"> Nombres:</label>
                </div>
                <div class="col-md-5">
                    <input runat="server" id="txtnombres" class="form-control" required="required" onkeypress="return soloLetras(event)" autocomplete="off" />
                </div>
            </div>

            <div class="row">
                <div class="col-md-2" style="max-width:130px">
                    <label style="height:27px; padding-top: 8px">Ape Pateno:</label>
                </div>
                <div class="col-md-5">
                    <input runat="server" id="txtapepaterno" class="form-control" onkeypress="return soloLetras(event)" autocomplete="off"/>
                </div>
            </div>

            <div class="row">
                <div class="col-md-2" style="max-width:130px">
                    <label style="height:27px; padding-top: 8px">Ape Materno:</label>
                </div>
                <div class="col-md-5">
                    <input runat="server" id="txtapematerno" class="form-control" onkeypress="return soloLetras(event)" autocomplete="off"/>
                </div>
            </div>



            <div class="row">
                <div class="col-md-2" style="max-width:130px">
                    <label style="height:27px; padding-top: 8px">Codigó/DNI:</label>
                </div>
                <div class="col-md-3">
                    <input runat="server" id="txtdni" class="form-control" required="required" readonly="readonly" onkeypress="return Validasolonumeros(event)" maxlength="15" autocomplete="off"/>
                </div>
            </div>

            <div class="row">
                <div class="col-md-2" style="max-width:130px">
                    <label style="height:27px; padding-top: 8px">Usuario:</label>
                </div>
                <div class="col-md-3">
                    <input runat="server" id="txtusername" class="form-control" required="required" maxlength="20" readonly="readonly" autocomplete="off"/>
                </div>
            </div>

            <div class="row">
                <div class="col-md-2" style="max-width:130px">
                    <label style="height:27px; padding-top: 8px">Clave:</label>
                </div>
                <div class="col-md-3">
                    <input type="password" runat="server" id="txtpassword" class="form-control" disabled="disabled"/>
                </div>

                <div class="col-lg-1" style="max-width:10px">
                    <input type="checkbox" id="chkpwd" onclick="HabilitarTextChangePWD();"/>
                </div>
                <div class="col-lg-2">
                    <label style="height:27px;">Cambiar Contraseña</label>
                </div>
            </div>

            <div class="row">
                <div class="col-md-2" style="max-width:130px">
                    <label style="height:27px; padding-top: 8px">Telef. Cel.:</label>
                </div>
                <div class="col-md-3">
                    <input runat="server" id="txttelefono" class="form-control" onkeypress="return Validasolonumeros(event)" autocomplete="off"/>
                </div>
            </div>


            <div class="row">
                <div class="col-md-2" style="max-width:130px">
                    <label style="height:27px; padding-top: 8px">Dirección:</label>
                </div>
                <div class="col-md-5">
                    <input runat="server" id="txtdireccion" class="form-control" autocomplete="off" />
                </div>
            </div>

            

            <div class="row">
                <div class="col-md-2" style="max-width:130px">
                    <label style="height:27px; padding-top: 8px">Email:</label>
                </div>
                <div class="col-md-5">
                    <input runat="server" id="txtemail" class="form-control" autocomplete="off" />
                </div>
            </div>

            <div class="row">
                <div class="col-md-2" style="max-width:130px">
                    <label style="height:27px; padding-top: 8px">Perfil:</label>
                </div>
                <div class="col-md-3">
                    <%--<select runat="server" id="Select1" class="form-control" onchange="this.form.submit()" onserverchange="cboperfil_ServerChange"></select>--%>
                    <select runat="server" id="cboperfil" class="form-control" ></select>
                </div>
            </div>

            <div class="row">
                <div class="col-md-2" style="max-width:130px">
                    <label style="height:27px; padding-top: 8px">F. Expiración:</label>
                </div>
                <div class="col-md-2">
                    <input runat="server" id="txtfechaexpiracion" class="form-control" placeholder="seleccione fecha" />
                </div>
            </div>

            <div class="row">
                <div class="col-md-2" style="max-width:130px">
                    <label style="height:27px; padding-top: 8px">Estado:</label>
                </div>
                 <div class="col-md-2">
                    <select runat="server" id="cboestado" class="form-control"></select>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-2" style="max-width:130px">
                    <label style="height:27px; padding-top: 8px">Empresa:</label>
                </div>
                <div class="col-lg-3">
                    <select runat="server" id="cboempresa" class="form-control" aria-readonly="true" disabled="disabled"></select>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-2" style="max-width:130px">
                    <label style="height:27px; padding-top: 8px">Sede:</label>
                </div>
                <div class="col-lg-3">
                    <select runat="server" id="cbosede" class="form-control" aria-readonly="true"></select>
                </div>
            </div>

            <br />

            <%--<b style="font-size:large">Asignar Roles</b>--%>
            <asp:Panel runat="server" BorderStyle="Solid" BorderColor="#c0c0c0" BorderWidth="1.8px" GroupingText="# Asignar Roles" >

                <%--<div class="table-responsive">--%>
                    <div class="row" >
                        <div class="col-lg-8">
                            <asp:TreeView ID="TreeView1" runat="server" ShowCheckBoxes="All" ExpandDepth="0" Font-Size="14px" ForeColor="#333333" Font-Names="Cambria">
                            </asp:TreeView>
                        </div>
                    </div>
                <%--</div>--%>
            </asp:Panel>

        </div>
        

        <div class="modal-footer">
            <div class="row">
                <div class="col-md-4">
                    <a href="ListaUsuarios" class="btn btn-primary" style="font-size:small; height:30px; font-family:Cambria" ><i class="fa fa-arrow-left"></i> Cancelar</a>
                    <asp:Button runat="server" ID="btnActualizar" CssClass="btn btn-primary" Text="Actualizar" Height="30px" Font-Names="Cambria" Font-Size="Small" OnClick="btnActualizar_Click" Enabled="false" />
                    
                </div>
            </div>
        </div>
    </div>

</asp:Content>
