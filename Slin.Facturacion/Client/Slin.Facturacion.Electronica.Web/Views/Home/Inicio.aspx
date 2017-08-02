<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Mobile.Master" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Home.Inicio" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <header>
        <%--<script type="text/javascript">
            function sbClock() {
                var DateString = (new Date()).toString();
                self.status = DateString.substring(0, 3 + DateString.lastIndexOf(':'));
                setTimeout("sbClock()", 200);
            }
            window.onload = function () { sbClock(); }
        </script>--%>

        <link href="../../Scripts/Other/stylesheets/ade.css" rel="stylesheet" />


        <link href="../../Scripts/Table/css/styles.css" rel="stylesheet" />
        <script src="../../Scripts/Table/js/jquery.dataTables.min.js"></script>
        <script src="../../Scripts/Table/js/jquery.metadata.js"></script>

        <script type="text/javascript">
            $(document).ready(function () {
                $.metadata.setType("class");
                $("table.grid").each(function () {
                    var grid = $(this);
                    if (grid.find("tbody > tr > th").length > 0) {
                        grid.find("tbody").before("<thead><tr></tr></thead>");
                        grid.find("thead:first tr").append(grid.find("th"));
                        grid.find("tbody tr:first").remove();
                    }
                    if (grid.hasClass("sortable") && grid.find("tbody:first > tr").length > 10) {
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

        <%--<div class="modal-header">
            <label style="font-family:Cambria; font-size:large"><i class="fa fa-apple"></i> Estado del Sistema</label>
        </div>--%>
        <hr />

        <div class="modal-body" style="font-family: Cambria">

            <div class="row">

                <div class="col-lg-5">
                    <asp:CheckBox runat="server" ID="chkenable3D" Visible="false" OnCheckedChanged="chkenable3D_CheckedChanged" Text="3D" AutoPostBack="True" />
                    <label hidden="hidden">- Gráfico</label>
                    <div class="table-responsive">

                        <asp:Chart ID="grafico1" runat="server" Visible="false">
                            <Series>
                                <asp:Series Name="Series1" XValueMember="Codigo"
                                    YValueMembers="UltimaSemana" ChartType="Column">
                                </asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1" Area3DStyle-Enable3D="false" BackColor="Transparent">
                                </asp:ChartArea>
                            </ChartAreas>
                            <BorderSkin SkinStyle="Emboss" />
                        </asp:Chart>
                    </div>
                </div>

                <div class="col-lg-7">
                    <div class="table-responsive">
                        <label style="font-weight:bold">Documentos Enviados</label>
                        <asp:GridView ID="GVEstadoSistema_Ok" ClientIDMode="Static" runat="server" Visible="true"
                            AutoGenerateColumns="False" CssClass="tablenyx" HeaderStyle-Font-Size="Small"
                            GridLines="Horizontal" AllowPaging="True" PageSize="10" AllowSorting="true"
                            PagerStyle-BorderColor="Black" PagerStyle-BorderStyle="Double" PagerStyle-BorderWidth="1"
                            OnPageIndexChanging="GVEstadoSistema_Ok_PageIndexChanging" OnSorting="GVEstadoSistema_Ok_Sorting">
                            <Columns>
                                <asp:BoundField DataField="Criterio" HeaderText="" Visible="true"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="Hoy" HeaderText="Hoy"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Ayer" HeaderText="Ayer"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="SemanaActual" HeaderText="Semana Actual"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="UltimaSemana" HeaderText="Semana Pasada"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="TotalMes" HeaderText="Mes Actual"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" Visible="true" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="MesPasado" HeaderText="Mes Pasado"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" Visible="true" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                            <PagerSettings Mode="NextPreviousFirstLast"
                                FirstPageImageUrl="~/Img/pri.PNG"
                                PreviousPageImageUrl="~/Img/ant.PNG"
                                NextPageImageUrl="~/Img/sgt.PNG"
                                LastPageImageUrl="~/Img/ult.PNG" />
                        </asp:GridView>


                       <%-- <asp:GridView runat="server" ID="GVEstadoSistema_Ok" CssClass="grid sortable {disableSortCols: []}" AutoGenerateColumns="false">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="25px" DataField="Criterio" HeaderText="" Visible="true"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" />
                                <asp:BoundField HeaderStyle-Width="25px" DataField="Ayer" HeaderText="Ayer"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField HeaderStyle-Width="25px" DataField="UltimaSemana" HeaderText="Semana Pasada"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField HeaderStyle-Width="25px" DataField="TotalMes" HeaderText="Mes Actual"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" Visible="true" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField HeaderStyle-Width="25px" DataField="MesPasado" HeaderText="Mes Pasado"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" Visible="true" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </asp:GridView>--%>



                    </div>
                </div>
            </div>

            <br />

            <div class="row">

                <div class="col-lg-5">
                    <label hidden="hidden">Gráfico</label>
                    <div class="table-responsive">
                        <asp:Chart ID="grafico2" runat="server" Visible="false">
                            <Series>
                                <asp:Series Name="Series1" XValueMember="Codigo"
                                    YValueMembers="UltimaSemana" ChartType="Bar">
                                </asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1" Area3DStyle-Enable3D="true" BackColor="Transparent">
                                </asp:ChartArea>
                            </ChartAreas>
                            <BorderSkin SkinStyle="Emboss" />
                        </asp:Chart>
                    </div>
                </div>

                <div class="col-lg-7">
                    <div class="table-responsive">
                        <label style="font-weight:bold">Documentos Pendientes</label>
                        <asp:GridView ID="GVEstadoSistema_Error" ClientIDMode="Static" runat="server" Visible="true"
                            AutoGenerateColumns="False" CssClass="tablenyx_2" HeaderStyle-Font-Size="Small"
                            GridLines="Horizontal" AllowPaging="True" PageSize="10" AllowSorting="true"
                            PagerStyle-BorderColor="Black" PagerStyle-BorderStyle="Double" PagerStyle-BorderWidth="1" OnRowDataBound="GVEstadoSistema_Error_RowDataBound"
                            OnPageIndexChanging="GVEstadoSistema_Error_PageIndexChanging" OnSorting="GVEstadoSistema_Error_Sorting">
                            <Columns>
                                <asp:BoundField DataField="Criterio" HeaderText="" Visible="true"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="Hoy" HeaderText="Hoy"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Ayer" HeaderText="Ayer"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="SemanaActual" HeaderText="Semana Actual"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="UltimaSemana" HeaderText="Semana Pasada"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="TotalMes" HeaderText="Mes Actual"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" Visible="true" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="MesPasado" HeaderText="Mes Pasado"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" Visible="true" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                            <PagerSettings Mode="NextPreviousFirstLast"
                                FirstPageImageUrl="~/Img/pri.PNG"
                                PreviousPageImageUrl="~/Img/ant.PNG"
                                NextPageImageUrl="~/Img/sgt.PNG"
                                LastPageImageUrl="~/Img/ult.PNG" />
                        </asp:GridView>


                        <%--<asp:GridView runat="server" ID="GVEstadoSistema_Error" CssClass="grid sortable {disableSortCols: []}" AutoGenerateColumns="false">
                            <Columns>
                                <asp:BoundField ControlStyle-Width="25px" HeaderStyle-Width="25px" DataField="Criterio" HeaderText="Descripción" Visible="true"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" />
                                <asp:BoundField ControlStyle-Width="20px" HeaderStyle-Width="25px" DataField="Ayer" HeaderText="Ayer"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField ControlStyle-Width="20px" HeaderStyle-Width="25px" DataField="UltimaSemana" HeaderText="Semana Pasada"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField ControlStyle-Width="20px" HeaderStyle-Width="25px" DataField="TotalMes" HeaderText="Mes Actual"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" Visible="true" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField ControlStyle-Width="20px" DataField="MesPasado" HeaderText="Mes Pasado"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" Visible="true" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </asp:GridView>--%>
                    </div>
                </div>
            </div>

            <br />


            <div class="row">

                <div class="col-lg-5">
                    <label hidden="hidden">Gráfico</label>
                    <div class="table-responsive">
                        <asp:Chart ID="Chart1" runat="server" Visible="false">
                            <Series>
                                <asp:Series Name="Series1" XValueMember="Codigo"
                                    YValueMembers="UltimaSemana" ChartType="Bar">
                                </asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1" Area3DStyle-Enable3D="true" BackColor="Transparent">
                                </asp:ChartArea>
                            </ChartAreas>
                            <BorderSkin SkinStyle="Emboss" />
                        </asp:Chart>
                    </div>
                </div>

                <div class="col-lg-7">
                    <div class="table-responsive">
                        <label style="font-weight:bold">Total Documentos Electrónicos</label>
                        <asp:GridView ID="GVTotDocumentosADE" ClientIDMode="Static" runat="server" Visible="true"
                            AutoGenerateColumns="False" CssClass="tablenyx_3" HeaderStyle-Font-Size="Small"
                            GridLines="Horizontal" AllowPaging="True" PageSize="10" AllowSorting="true"
                            PagerStyle-BorderColor="Black" PagerStyle-BorderStyle="Double" PagerStyle-BorderWidth="1"
                            OnPageIndexChanging="GVTotDocumentosADE_PageIndexChanging" OnSorting="GVTotDocumentosADE_Sorting">
                            <Columns>
                                <asp:BoundField DataField="Criterio" HeaderText="" Visible="true"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" />
                                <asp:BoundField DataField="Hoy" HeaderText="Hoy"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Ayer" HeaderText="Ayer"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="SemanaActual" HeaderText="Semana Actual"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="UltimaSemana" HeaderText="Semana Pasada"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="TotalMes" HeaderText="Mes Actual"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" Visible="true" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="MesPasado" HeaderText="Mes Pasado"  HeaderStyle-ForeColor="White" HeaderStyle-Wrap="false" ItemStyle-Font-Size="Small" ItemStyle-Wrap="false" Visible="true" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                            <PagerSettings Mode="NextPreviousFirstLast"
                                FirstPageImageUrl="~/Img/pri.PNG"
                                PreviousPageImageUrl="~/Img/ant.PNG"
                                NextPageImageUrl="~/Img/sgt.PNG"
                                LastPageImageUrl="~/Img/ult.PNG" />
                        </asp:GridView>
                    </div>
                </div>

            </div>

        </div>


    </div>

</asp:Content>
