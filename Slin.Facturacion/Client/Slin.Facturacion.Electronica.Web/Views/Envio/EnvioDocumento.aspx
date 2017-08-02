<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="EnvioDocumento.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Envio.EnvioDocumento" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <header>
        <script src="../../Scripts/Validacion/Validaciones.js"></script>

        <script type="text/javascript">
            function getFolder() {
                return showModalDialog("folderDialog.HTA", "", "width:400px;height:400px;resizeable:yes;");
            }

            function SelCarpeta() {
                var objShell = new ActiveXObject("Shell.Application");
                var objFolder = objShell.BrowseForFolder(0, "SELECCIONE LA RUTA DONDE DESEA GUARDAR EL ARCHIVO", 0, 0);
                if (objFolder != null) {
                    alert('OK');
                    var objFolderItem = objFolder.Items().Item();
                    var objPath = objFolderItem.Path;
                    var foldername = objPath;
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('lblmensajerespuesta') != -1) {
                            document.forms[0].elements[i].innerText = foldername;
                        }
                    }
                    return false;
                }
                else {
                    alert('NO OK');
                }
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
                <a style="font-family: Cambria; height: 35px" href="EnvioDocumento">/ Envío de Documentos</a>
            </div>
        </div>

        <div class="modal-header">
            <label style="font-family: Cambria; font-size: 16px"><i class="fa fa-mail-forward"></i> Ingrese Datos</label>
        </div>

        <div class="modal-body" style="font-family: Cambria">

            <div class="row">
                <div class="col-lg-1">
                    <label style="height: 27px; padding-top: 8px">Serie:</label>
                </div>

                <div class="col-lg-2" style="max-width: 120px">
                    <input runat="server" id="txtnroserie" class="form-control" placeholder="serie" required="required" />
                </div>

                <div class="col-lg-2" style="max-width: 110px;">
                    <label style="padding-top: 8px">Correlativo:</label>
                </div>

                <div class="col-lg-2" style="max-width: 180px">
                    <input runat="server" id="txtnrodocumento" class="form-control" onkeypress="return Validasolonumeros(event)" maxlength="15" placeholder="correlativo" required="required" />
                </div>

                <div class="col-lg-2" style="max-width: 100px;">
                    <label style="padding-top: 8px">Tpo Doc:</label>
                </div>

                <div class="col-lg-3" style="max-width: 230px">
                    <select runat="server" id="cbotipodocumento" class="form-control" style="max-width: 230px"></select>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-1">
                    <label style="height: 27px; padding-top: 8px">Para:</label>
                </div>
                <div class="col-lg-5">
                    <input runat="server" id="txtemaildestino" class="form-control" placeholder="ejemplo@ejemplo.com" required="required" autocomplete="off" />
                </div>
                <div class="col-lg-3">
                    <label style="font-size: 14px; font-family: Cambria">(Use ";" para separar direcciones)</label>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-1">
                    <label style="height: 27px; padding-top: 8px">Asunto:</label>
                </div>
                <div class="col-lg-5">
                    <input runat="server" id="txtasunto" class="form-control" placeholder="asunto" />
                </div>
            </div>

            <div class="row">
                <div class="col-lg-1">
                    <label>Mensaje:</label>
                </div>
                <div class="col-lg-6">
                    <textarea id="txtmensaje" class="form-control" rows="3" runat="server"></textarea>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-lg-1">
                    <label>Adjuntar:</label>
                </div>
                <div class="col-lg-4" style="max-width: 350px">
                    <asp:FileUpload ID="FileUpload1" runat="server" Height="35px" onchange="javascript:try{submit();}catch(err){}" />
                </div>
                <div class="col-lg-5">
                    <%--<asp:Button runat="server" ID="btnAddFiles" Text="Add file" Height="35px" Font-Size="Small" ToolTip="Subir Archivos" OnClientClick="selectFile(); return false;" CssClass="btn btn-primary" Visible="false"/>--%>
                    <%--<button runat="server" id="btnUpFile" onserverclick="btnAddFiles_Click" class="btn btn-primary" hidden="hidden"><span hidden="hidden"><i class="fa fa-upload" hidden="hidden"></i></span></button>--%>
                </div>
            </div>
            <br />
            <div class="row">

                <div class="table-responsive">

                    <div class="col-lg-1">
                        <label></label>
                    </div>
                    <div class="col-lg-4">
                        <asp:ListBox ID="lstFiles" runat="server" Visible="false"></asp:ListBox>
                    </div>
                    <div class="col-lg-1">
                        <label></label>
                    </div>
                    <div class="col-lg-1">
                        <%--<label runat="server" id="Label1"></label>--%>
                        <%--<asp:Button runat="server" ID="btnDeleteSelected" OnClick="btnDeleteSelected_Click" Visible="false" CssClass="btn btn-primary"/>--%>
                        <button runat="server" id="btnDeleteSelected" onserverclick="btnDeleteSelected_Click" class="btn btn-primary" visible="false">X</button>
                    </div>
                </div>

            </div>

            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-lg-1">
                            <label></label>
                        </div>
                        <div class="col-lg-5">
                            <h4 runat="server" id="lblmensajerespuesta" visible="false" style="font-family: Cambria"></h4>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-lg-1">
                            <label></label>
                        </div>
                        <div class="col-lg-6">

                            <asp:Button runat="server" ID="btnEnviar" CssClass="btn btn-primary" Height="30px" Font-Size="Small" Text="Enviar" OnClick="btnEnviar_Click" Enabled="false" />

                        </div>

                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>




        </div>
    </div>

</asp:Content>
