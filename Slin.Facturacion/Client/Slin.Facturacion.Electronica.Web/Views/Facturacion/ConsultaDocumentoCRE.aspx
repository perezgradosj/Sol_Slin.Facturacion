<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="ConsultaDocumentoCRE.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Facturacion.ConsultaDocumentoCRE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <header>
        <link href="../../Scripts/Table/css/styles.css" rel="stylesheet" />
        <script src="../../Scripts/Table/js/jquery.dataTables.min.js"></script>
        <script src="../../Scripts/Table/js/jquery.metadata.js"></script>

        <script type="text/javascript">
            $(document).ready(function () {

                $.metadata.setType("class");

                $("table.grid").each(function () {
                    var grid = $(this);
                    // Por cada GridView que se encuentre modificar el código HTML generado para agregar el THEAD.
                    if (grid.find("tbody > tr > th").length > 0) {
                        grid.find("tbody").before("<thead><tr></tr></thead>");
                        grid.find("thead:first tr").append(grid.find("th"));
                        grid.find("tbody tr:first").remove();
                    }
                    // Si el GridView tiene la clase "sortable" aplicar el plugin DataTables si tiene más de 10 elementos.
                    if (grid.hasClass("sortable") && grid.find("tbody:first > tr").length > 0) {
                        grid.dataTable({
                            sPaginationType: "full_numbers",
                            aoColumnDefs: [
                                { bSortable: false, aTargets: grid.metadata().disableSortCols }
                            ]
                        });
                    }
                });
            });
        </script>


        <link rel="stylesheet" href="../../Content/DatetimePicker/bootstrap-datepicker.css">
        <script src="../../Content/DatetimePicker/bootstrap-datepicker.min.js"></script>

        <script type="text/javascript">
            $(document).ready(function () {
                var dp = $('#<%=txtfechadesde.ClientID%>');
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

        <script type="text/javascript">
            $(document).ready(function () {
                var dp = $('#<%=txtfechahasta.ClientID%>');
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

        <!-- validacion para numeros -->
        <script type="text/javascript">
            function validNumericos(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode
                if (((charCode == 8) || (charCode == 46)
                || (charCode >= 35 && charCode <= 40)
                    || (charCode >= 48 && charCode <= 57)
                    || (charCode >= 96 && charCode <= 105))) {
                    return true;
                }
                else {
                    return false;
                }
            }
        </script>

        <script src="../../Scripts/Validacion/Validaciones.js"></script>


        <script type="text/javascript">

            function SelectAll() {
                //var frm = document.forms['aspnetForm'];
                if ($('#<%=idSelectAll.ClientID%>').prop('checked')) {
                    //alert("OK");
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('chkSelImp') != -1) {
                            document.forms[0].elements[i].checked = true
                        }
                    }
                } else {
                    //alert("NOT OK");
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('chkSelImp') != -1) {
                            document.forms[0].elements[i].checked = false
                        }
                    }
                }
            }
        </script>

        <script type="text/javascript">
            function Validar() {
                if ($('#<%=idSelectAll.ClientID%>').prop('checked')) {
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('idSelectAll') != -1) {
                            document.forms[0].elements[i].checked = false
                        }
                    }
                }
            }
        </script>

        <script type="text/javascript">

            function SelectAll_Send() {
                //var frm = document.forms['aspnetForm'];
                if ($('#<%=idSelectAlltoSend.ClientID%>').prop('checked')) {
                    //alert("OK");
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('chkSelSend') != -1) {
                            document.forms[0].elements[i].checked = true
                        }
                    }
                } else {
                    //alert("NOT OK");
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('chkSelSend') != -1) {
                            document.forms[0].elements[i].checked = false
                        }
                    }
                }
            }
        </script>


         <script type="text/javascript">
            function Validar_Send() {
                if ($('#<%=idSelectAlltoSend.ClientID%>').prop('checked')) {
                    for (var i = 0; i < document.forms[0].length; i++) {
                        if (document.forms[0].elements[i].id.indexOf('idSelectAlltoSend') != -1) {
                            document.forms[0].elements[i].checked = false
                        }
                    }
                }
            }
        </script>

    </header>

    <!-- INICIO CUERPO CONSULTA-->

    <div lang="es-pe" class="content" style="background-repeat: no-repeat; background-image: url(../../Img/home/fondopantalla.jpg); background-size: cover">

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

        <div class="row" style="border-color: none">
            <div style="font-family: Cambria; font-size: medium;">
                <asp:HyperLink runat="server" BackColor="Transparent" NavigateUrl="~/Views/Home/Inicio" Width="60" BorderStyle="None" Font-Bold="true" Text="Home" CssClass="form-control"></asp:HyperLink>
                <a style="font-family: Cambria; height: 35px" href="ConsultaDocumentoCRE">/ Consulta de Documentos CRE</a>
            </div>
        </div>

        <div class="main-content" style="font-family: Cambria; font-size: medium">
            <ul class="nav nav-tabs">
                <li class="active"><a style="background-color: transparent; border-color: none; font-family: Cambria; font-size: 16px; font: bold" href="#lista" data-toggle="tab"><i class="fa fa-filter"></i>Criterios de Búsqueda</a></li>
            </ul>
        </div>

        <div class="tab-content" style="padding-top: 15px;">
            <div class="tab-pane active" id="consulta">
                <div class="col-md-12" style="font-family: Cambria;">

                    <div class="row">
                        <div class="col-md-1">
                            <label style="width: 120px; height: 27px; padding-top: 8px">Fecha Desde:</label>
                        </div>
                        <div class="col-md-2">
                            <%--<input runat="server" type="date" id="Date1" class="form-control" style="width: 150px; font-size: 12.5px" placeholder="seleccione fecha" value="fecha" />--%>
                            <input runat="server" type="text" id="txtfechadesde" class="form-control" style="width: 150px;" placeholder="seleccione fecha" />
                        </div>

                        <div class="col-md-1">
                            <label style="width: 85px; height: 27px; padding-top: 8px">Fecha Hasta:</label>
                        </div>
                        <div class="col-md-2">
                            <input runat="server" type="text" id="txtfechahasta" class="form-control" style="width: 150px;" placeholder="seleccione fecha" maxlength="10" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-1">
                            <label style="width: 120px; height: 27px; padding-top: 8px">Tipo Doc.:</label>
                        </div>
                        <div class="col-md-2" style="text-align: left">
                            <select runat="server" id="cbotipodocumento" class="form-control" style="width: 150px;" onchange="this.form.submit()" onserverchange="cbotipodocumento_ServerChange" disabled="disabled"></select>
                        </div>

                        <div class="col-md-1">
                            <label style="width: 50px; padding-top: 8px">Serie:</label>
                        </div>
                        <div class="col-md-2">
                            <select runat="server" id="cboserie" class="form-control" style="width: 150px;"></select>
                        </div>

                        <div class="col-md-1">
                            <label style="width: 135px; padding-top: 8px">Nro Inicio:</label>
                        </div>
                        <div class="col-md-2">
                            <input runat="server" id="txtnuminicio" class="form-control" style="width: 150px;" placeholder="nro doc inicio" onkeypress="return Validasolonumeros(event)" maxlength="12" />
                        </div>

                        <div class="col-md-1">
                            <label style="width: 130px; padding-top: 8px">Nro Fin:</label>
                        </div>
                        <div class="col-md-2">
                            <input runat="server" id="txtnumfin" class="form-control" style="width: 150px;" placeholder="nro doc fin" onkeypress="return Validasolonumeros(event)" maxlength="12" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-1">
                            <label style="width: 100px; padding-top: 8px">Cliente:</label>
                        </div>
                        <div class="col-md-2">
                            <input runat="server" id="txtclienteruc" class="form-control" style="width: 150px;" placeholder="RUC" maxlength="11" onkeypress="return Validasolonumeros(event)" />
                        </div>
                        <div class="col-md-1">
                            <label style="width: 15px; padding-top: 8px;">R.Social:</label>
                        </div>
                        <div class="col-md-3" style="max-width: 300px;">
                            <input runat="server" style="width: auto" id="txtclientenombres" class="form-control" placeholder="Razon Social" />
                        </div>

                        <div class="col-md-2">
                            <label></label>
                        </div>

                        <div class="col-md-1">
                            <label style="width: 80px; padding-top: 8px">Estado:</label>
                        </div>
                        <div class="col-md-2">
                            <select runat="server" id="cboestado" class="form-control" style="width: 150px;">
                            </select>
                        </div>
                    </div>

                    <br />
                    <div class="row">
                        <div class="col-md-1">
                            <label></label>
                        </div>
                        <div class="col-md-3">
                            <a runat="server" style="height: 30px; font-size: small; font-family: Cambria" class="btn btn-primary" id="btnBuscar" onserverclick="btnBuscar_Click" visible="false"><i class="fa fa-search"></i> Buscar</a>
                        </div>
                        <div class="col-md-1">
                            <label></label>
                        </div>
                        <div class="col-md-3">
                            <asp:Button runat="server" ID="btnExportarExcel" Height="30px" Font-Size="Small" Font-Names="Cambria" OnClick="btnExportarExcel_Click" CssClass="btn btn-primary" Text="Exportar Excel" Enabled="false" />
                        </div>

                        <div class="col-md-1">
                            <label style="width: 80px; padding-top: 8px">Impresoras: </label>
                        </div>
                        <div class="col-md-3">
                            <select runat="server" id="cboimpresoras" class="form-control" visible="true"></select>
                        </div>
                    </div>

                    <hr />

                    <!--LISTA DE RESULTADOS GridView1-->
                    <div class="row">



                        <div class="table-responsive">

                            <asp:GridView runat="server" ID="GridView1" CssClass="grid sortable {disableSortCols: []}" AutoGenerateColumns="false" OnRowDataBound="GridView1_RowDataBound">
                                <Columns>

                                    <asp:TemplateField HeaderText="Send" HeaderStyle-Font-Size="Smaller"  HeaderStyle-ForeColor="White" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkSelSend" Height="15px" Width="12px" ToolTip="Select to Send" OnClick="return Validar_Send();" AutoPostBack="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--<asp:TemplateField HeaderText="Sel" HeaderStyle-Font-Size="Smaller"  HeaderStyle-ForeColor="White" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkSelImp" Height="15px" Width="12px" ToolTip="Sel to Print." OnClick="return Validar();" AutoPostBack="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                    <asp:TemplateField HeaderText="Xml" HeaderStyle-Font-Size="Smaller"  HeaderStyle-ForeColor="White" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="btnImgXML" ImageUrl="~/Img/xml.jpg" Height="15px" Width="12px" OnClick="btnImgXML_Click" ToolTip="Descargar Xml Doc." />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="CDR" HeaderStyle-Font-Size="Smaller"  HeaderStyle-ForeColor="White" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="btnImgCDR" ImageUrl="~/Img/xml.jpg" Height="15px" Width="12px" OnClick="btnImgCDR_Click" ToolTip="Descargar CDR Doc." />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Pdf" HeaderStyle-Font-Size="Smaller"  HeaderStyle-ForeColor="White" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="btnImgPdf" ImageUrl="~/Img/pdf.jpg" Height="15px" Width="12px" OnClick="btnImgPdf_Click" ToolTip="Descargar Pdf Doc." />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Det" HeaderStyle-Font-Size="Smaller"  HeaderStyle-ForeColor="White" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="btnVerDetalle" ImageUrl="~/Img/detail.png" Height="15px" Width="12px" OnClick="btnVerDetalle_Click" ToolTip="Ver Detalle Documento" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:ImageField HeaderText="Es" HeaderStyle-Font-Size="Smaller"  HeaderStyle-ForeColor="White" DataImageUrlField="RutaImagen" ControlStyle-Width="12px" ControlStyle-Height="12px" ItemStyle-VerticalAlign="Top">
                                    </asp:ImageField>

                                    <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Nro" HeaderText="Nro" SortExpression="Nro" />
                                    <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="TipoDocumento.CodigoDocumento" HeaderText="Tpo doc." SortExpression="TipoDocumento.CodigoDocumento" HeaderStyle-Wrap="false" />
                                    <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Serie.NumeroSerie" HeaderText="Serie" SortExpression="Serie.NumeroSerie" HeaderStyle-Wrap="false" />
                                    <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="NumeroDocumento" HeaderText="Correlativo" SortExpression="NumeroDocumento" HeaderStyle-Wrap="false" />
                                    <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="FechaEmision" DataFormatString="{0:d}" HeaderText="Fecha Emisión" SortExpression="FechaEmision" HeaderStyle-Wrap="false" />
                                    <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Cliente.NumeroDocumentoIdentidad" HeaderText="Nro doc Clie." SortExpression="Cliente.NumeroDocumentoIdentidad" HeaderStyle-Wrap="false" />
                                    <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Left" DataField="Cliente.Nombres" HeaderText="Cliente" SortExpression="Cliente.Nombres" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />
                                    <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Moneda.Descripcion" HeaderText="Moneda" SortExpression="Moneda.Descripcion" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                    <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Right" DataField="MontoTotal" HeaderText="Monto" DataFormatString="{0:n}" SortExpression="MontoTotal" HeaderStyle-Wrap="false" />
                                    <asp:BoundField  HeaderStyle-ForeColor="White" ItemStyle-Font-Size="Smaller" ItemStyle-HorizontalAlign="Center" DataField="Estado.Descripcion" HeaderText="Estado" SortExpression="Estado.Descripcion" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" />

                                    
                                </Columns>
                            </asp:GridView>
                            <br />
                        </div>
                        <br />


                        <div class="row">
                            <div class="col-md-3">
                                <label style="min-width: 7px"></label>
                                <input type="checkbox" runat="server" id="idSelectAlltoSend" onchange="SelectAll_Send()" visible="false" />
                                <label runat="server" id="lblseleccionar_send" visible="false">Sel. todos Send</label>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-3" style="max-width:150px">
                                <label style="min-width: 7px"></label>
                                <input type="checkbox" runat="server" id="idSelectAll" onchange="SelectAll()" visible="false" />
                                <label runat="server" id="lblseleccionar" visible="false">Sel. todos Print</label>
                            </div>
                            <div class="col-md-1" style="max-width: 50px">
                                <label runat="server" style="font-size: small" id="lblcopies" visible="false">Copias:</label>
                            </div>
                            <div class="col-md-2">
                                <input runat="server" id="txtCopies" class="form-control" type="number" style="width: 70px; height: 25px" min="1" visible="false" />
                            </div>
                            <div class="col-md-1">
                                <label style="min-width:7px"></label>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-3">
                                <label style="min-width: 7px"></label>
                                <asp:Button runat="server" ID="btnSend" Height="30px" Font-Size="Small" Font-Names="Cambria" OnClick="btnSend_Click" CssClass="btn btn-primary" Text="Enviar" Enabled="false" />
                                <asp:Button runat="server" ID="btnPrint" Height="30px" Font-Size="Small" Font-Names="Cambria" OnClick="btnPrint_Click" CssClass="btn btn-primary" Text="Imprimir" Enabled="false" />
                            </div>
                        </div>

                        <hr />

                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
