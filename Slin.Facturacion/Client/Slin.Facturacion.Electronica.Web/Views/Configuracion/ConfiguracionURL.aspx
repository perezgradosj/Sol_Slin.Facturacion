<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="ConfiguracionURL.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Configuracion.ConfiguracionURL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <header>

        <script type="text/javascript">

            function HabilitarTextTest() {

                if ($('#chk1').prop('checked')) {
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('txtTestCE') != -1) {
                            document.forms[0].elements[i].disabled = false;
                        }
                    }

                } else {
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('txtTestCE') != -1) {
                            document.forms[0].elements[i].disabled = true;
                        }
                    }
                }


                if ($('#chk2').prop('checked')) {
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('txtTestGUIA') != -1) {
                            document.forms[0].elements[i].disabled = false;
                        }
                    }
                } else {
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('txtTestGUIA') != -1) {
                            document.forms[0].elements[i].disabled = true;
                        }
                    }
                }


                if ($('#chk3').prop('checked')) {
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('txtTestCPECRE') != -1) {
                            document.forms[0].elements[i].disabled = false;
                        }
                    }
                } else {
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('txtTestCPECRE') != -1) {
                            document.forms[0].elements[i].disabled = true;
                        }
                    }
                }

                if ($('#chk4').prop('checked')) {
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('txtQACEGUIACPECRE') != -1) {
                            document.forms[0].elements[i].disabled = false;
                        }
                    }
                } else {
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('txtQACEGUIACPECRE') != -1) {
                            document.forms[0].elements[i].disabled = true;
                        }
                    }
                }

                if ($('#chk5').prop('checked')) {
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('txtPRODCE') != -1) {
                            document.forms[0].elements[i].disabled = false;
                        }
                    }
                } else {
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('txtPRODCE') != -1) {
                            document.forms[0].elements[i].disabled = true;
                        }
                    }
                }


                if ($('#chk6').prop('checked')) {
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('txtPRODGUIA') != -1) {
                            document.forms[0].elements[i].disabled = false;
                        }
                    }
                } else {
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('txtPRODGUIA') != -1) {
                            document.forms[0].elements[i].disabled = true;
                        }
                    }
                }

                if ($('#chk7').prop('checked')) {
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('txtPRODCPECRE') != -1) {
                            document.forms[0].elements[i].disabled = false;
                        }
                    }
                } else {
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('txtPRODCPECRE') != -1) {
                            document.forms[0].elements[i].disabled = true;
                        }
                    }
                }

                if ($('#chk8').prop('checked')) {
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('txtCONSALLDOC') != -1) {
                            document.forms[0].elements[i].disabled = false;
                        }
                    }
                } else {
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('txtCONSALLDOC') != -1) {
                            document.forms[0].elements[i].disabled = true;
                        }
                    }
                }























                //if ($(document.forms[0].elements['chkTestCEe'].checked)) { tambien serive cuando el control es html
                <%--if ($('#<%=chkTestCE.ClientID%>').prop('checked')) {--%> //cuando el control es asp 


                //if ($('#chkTestCE').prop('checked')) {  // cuando el control es html
                //    for (var i = 0; i < document.forms[0].length; i++) {
                //        if (document.forms[0].elements[i].id.indexOf('txtTestCE') != -1) {
                //            document.forms[0].elements[i].disabled = false;
                //        } 
                //    }

                //} else {
                //    for (var i = 0; i < document.forms[0].length; i++) {
                //        if (document.forms[0].elements[i].id.indexOf('txtTestCE') != -1) {
                //            document.forms[0].elements[i].disabled = true;
                //        }
                //    }
                //}


                <%--$('#<%=txtTestCE.ClientID%>').disabled = false;--%>
                //$(document.forms[0].elements['texto'].disabled = false);
                //$('texto').disabled = false;

                <%--$('#<%=txtTestCE.ClientID%>').disabled = true;--%>

                //$(document.forms[0].elements['texto'].disabled = true);
                //$('texto').disabled = true;

                //if ($('#chkTestCE').prop('checked')) {
                //    $('#txtTestCE').disabled = false;
                //} else {
                //    $('#txtTestCE').disabled = true;
                //}
            }
        </script>


        <script type="text/javascript">

            

            <%--function HabilitarTextTest() {
                if ($('#<%=chkTestCE.ClientID%>').prop('checked')) {
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('txtTestCE') != -1) {
                            document.forms[0].elements[i].disabled = false;
                        }
                    }
                } else {
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('txtTestCE') != -1) {
                            document.forms[0].elements[i].disabled = true;
                        }
                    }
                }
            }--%>
        </script>








        <script type="text/javascript">
            function comprueba() {
                //return confirm("Confirme el postback");
                return true;
            }
        </script>
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
                <a style="font-family: Cambria; height: 35px" href="ConfiguracionURL.aspx">/ Configurarción de URL de Ambiente </a>
            </div>
        </div>

        <div class="modal-header">
            <label style="font-family: Cambria; font-size: 16px"><i class="glyphicon glyphicon-user padding-right-small"></i>Ambientes</label>
        </div>

        <div class="modal-body" style="font-family: Cambria">


            <!-- TEST -->
            <div class="row">
                <div class="col-md-2" style="max-width: 140px">
                    <label style="height: 27px; width: 100px; padding-top: 8px">TEST:</label>
                    <label runat="server" id="idTest" visible="false"></label>
                </div>
            </div>

            <div class="row">
                <div class="col-md-1" hidden="hidden">
                    <label runat="server" id="IdUrlTestCE"></label>
                    <!--ID URL-->
                </div>

                <div class="col-md-1">
                    <asp:Button runat="server" ID="btnEditTest" Text="Editar" Visible="false" Font-Size="Smaller" Height="25" OnClick="btnEditTest_Click" OnClientClick="return comprueba();" />
                </div>
                <div class="col-md-1" style="max-width: 65px">
                    <label style="height: 27px">CE:</label>
                </div>
                <div class="col-md-6">
                    <input runat="server" id="txtTestCE" class="form-control" placeholder="ingrese url" style="font-size: small" disabled="disabled" />
                </div>
                <div class="col-md-1">
                    <input type="checkbox" id="chk1" onclick="HabilitarTextTest();" />
                </div>

            </div>

            <div class="row">
                <div class="col-md-1">
                    <label runat="server" id="IdUrlTestGUIA" visible="false"></label>
                    <!--ID URL-->
                </div>
                <div class="col-md-1" style="max-width: 65px">
                    <label style="height: 27px">GUIA:</label>
                </div>
                <div class="col-md-6">
                    <input runat="server" id="txtTestGUIA" class="form-control" placeholder="ingrese url" disabled="disabled" style="font-size: small" />
                </div>
                <div class="col-md-1">
                    <input type="checkbox" id="chk2" onclick="HabilitarTextTest();" />
                </div>
            </div>

            <div class="row">
                <div class="col-md-1">
                    <label runat="server" id="IdUrlTestCPECRE" visible="false"></label>
                    <!--ID URL-->
                </div>
                <div class="col-md-1" style="max-width: 65px">
                    <label style="height: 27px">CPE/CRE:</label>
                </div>
                <div class="col-md-6">
                    <%--<textarea runat="server" id="txtTestCPECRE" rows="1" class="form-control" placeholder="ingrese url" readonly="readonly"></textarea>--%>

                    <input runat="server" id="txtTestCPECRE" class="form-control" placeholder="ingrese url" disabled="disabled" style="font-size: small" />
                </div>
                <div class="col-md-1">
                    <input type="checkbox" id="chk3" onclick="HabilitarTextTest();" />
                </div>
            </div>

            <!-- END TEST -->



            <!-- QA -->

            <div class="row">
                <div class="col-md-2" style="max-width: 140px">
                    <label style="height: 27px; width: 100px; padding-top: 8px">QA:</label>
                    <label runat="server" id="idhomolog" visible="false"></label>
                </div>
            </div>

            <div class="row">
                <div class="col-md-1" hidden="hidden">
                    <label runat="server" id="IdUrlQACEGUIACPECRE" visible="false"></label>
                    <!--ID URL-->
                </div>
                <div class="col-md-1">
                    <asp:Button runat="server" ID="btnEdtiQA" Visible="false" Text="Editar" Font-Size="Smaller" Height="25" OnClick="btnEdtiQA_Click" />
                </div>
                <div class="col-md-1" style="max-width: 65px">
                    <label style="height: 27px">CE:</label>
                </div>
                <div class="col-md-6">
                    <%--<textarea runat="server" id="txtQACEGUIACPECRE" rows="1" class="form-control" placeholder="ingrese url" readonly="readonly"></textarea>--%>

                    <input runat="server" id="txtQACEGUIACPECRE" class="form-control" placeholder="ingrese url" disabled="disabled" style="font-size: small" />
                </div>
                <div class="col-md-1">
                    <input type="checkbox" id="chk4" onclick="HabilitarTextTest();" />
                </div>
            </div>

            <%--<div class="row">
                <div class="col-md-1" style="max-width:5px">
                    <label></label>
                </div>
                <div class="col-md-1" style="max-width:65px">
                    <label style="height:27px">GUIA:</label>
                </div>
                <div class="col-md-6">
                    <textarea runat="server" id="Textarea4" rows="1" class="form-control" placeholder="ingrese url"></textarea>
                </div>
                <div class="col-md-1">
                    <asp:CheckBox runat="server" />
                </div>
            </div>

            <div class="row">
                <div class="col-md-1" style="max-width:5px">
                    <label></label>
                </div>
                <div class="col-md-1" style="max-width:65px">
                    <label style="height:27px">CPE/CRE:</label>
                </div>
                <div class="col-md-6">
                    <textarea runat="server" id="Textarea5" rows="1" class="form-control" placeholder="ingrese url"></textarea>
                </div>
                <div class="col-md-1">
                    <asp:CheckBox runat="server" />
                </div>
            </div>--%>

            <!-- END QA -->

            <!-- PRODUCTION -->
            <div class="row">
                <div class="col-md-2" style="max-width: 140px">
                    <label style="height: 27px; width: 100px; padding-top: 8px">PRODUCCIÓN:</label>
                    <label runat="server" id="idproducc" visible="false"></label>
                </div>
            </div>

            <div class="row">
                <div class="col-md-1" hidden="hidden">
                    <label runat="server" id="IdUrlProdCE" visible="false"></label>
                    <!--ID URL-->
                </div>
                <div class="col-md-1">
                    <asp:Button runat="server" ID="btnEditProduc" Visible="false" Text="Editar" Font-Size="Smaller" Height="25" OnClick="btnEditProduc_Click" />
                </div>
                <div class="col-md-1" style="max-width: 65px">
                    <label style="height: 27px">CE:</label>
                </div>
                <div class="col-md-6">
                    <%--<textarea runat="server" id="txtPRODCE" rows="1" class="form-control" placeholder="ingrese url" readonly="readonly"></textarea>--%>

                    <input runat="server" id="txtPRODCE" class="form-control" placeholder="ingrese url" disabled="disabled" style="font-size: small" />
                </div>
                <div class="col-md-1">
                    <input type="checkbox" id="chk5" onclick="HabilitarTextTest();" />
                </div>
            </div>

            <div class="row">
                <div class="col-md-1">
                    <label runat="server" id="IdUrlProdGUIA" visible="false"></label>
                    <!--ID URL-->
                </div>
                <div class="col-md-1" style="max-width: 65px">
                    <label style="height: 27px">GUIA:</label>
                </div>
                <div class="col-md-6">
                    <%--<textarea runat="server" id="txtPRODGUIA" rows="1" class="form-control" placeholder="ingrese url" readonly="readonly"></textarea>--%>

                    <input runat="server" id="txtPRODGUIA" class="form-control" placeholder="ingrese url" disabled="disabled" style="font-size: small" />
                </div>
                <div class="col-md-1">
                    <input type="checkbox" id="chk6" onclick="HabilitarTextTest();" />
                </div>
            </div>

            <div class="row">
                <div class="col-md-1">
                    <label runat="server" id="IdUrlProdCPECRE" visible="false"></label>
                    <!--ID URL-->
                </div>
                <div class="col-md-1" style="max-width: 65px">
                    <label style="height: 27px">CPE/CRE:</label>
                </div>
                <div class="col-md-6">
                    <%--<textarea runat="server" id="txtPRODCPECRE" rows="1" class="form-control" placeholder="ingrese url" readonly="readonly"></textarea>--%>

                    <input runat="server" id="txtPRODCPECRE" class="form-control" placeholder="ingrese url" disabled="disabled" style="font-size: small" />
                </div>
                <div class="col-md-1">
                    <input type="checkbox" id="chk7" onclick="HabilitarTextTest();" />
                </div>
            </div>

            <!-- PRODUCTION-->


            <!-- CONSULTA CON NRO TICKET O NRO DE ATENCION-->

            <div class="row">
                <div class="col-md-2" style="max-width: 160px">
                    <label style="height: 27px; width: 160px; padding-top: 8px">CONS. NRO ATEN.:</label>
                </div>
            </div>

            <div class="row">
                <div class="col-md-1">
                    <label runat="server" id="IdUrlConsCEGUIACPECRE" visible="false"></label>
                </div>
                <div class="col-md-1" style="max-width: 65px">
                    <label style="height: 27px">Todos:</label>
                </div>
                <div class="col-md-6">
                    <%--<textarea runat="server" id="txtCONSALLDOC" rows="1" class="form-control" placeholder="ingrese url" readonly="readonly"></textarea>--%>
                    <input runat="server" id="txtCONSALLDOC" class="form-control" placeholder="ingrese url" disabled="disabled" style="font-size: small" />
                </div>
                <div class="col-md-1">
                    <input type="checkbox" id="chk8" onclick="HabilitarTextTest();" />
                </div>
            </div>

            <!-- END CST-->


            <%--<hr />--%>
            <div class="modal-footer">
                <div class="col-md-1">
                    <label></label>
                </div>
                <div class="col-md-5">
                    <%--<a href="CrearPerfiles" class="btn btn-primary" style="height: 30px; font-family: Cambria; font-size: small"><i class="fa fa-plus"></i>Nuevo</a>
                    <button runat="server" id="btnBuscar" class="btn btn-primary" style="height: 30px; font-size: small" visible="false"><i class="fa fa-search"></i>Buscar</button>--%>
                    <asp:Button runat="server" ID="btnGuardar" Text="Guardar Cambios" Font-Size="Small" CssClass="btn btn-primary" Height="30" OnClick="btnGuardar_Click" OnClientClick="return comprueba();" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
