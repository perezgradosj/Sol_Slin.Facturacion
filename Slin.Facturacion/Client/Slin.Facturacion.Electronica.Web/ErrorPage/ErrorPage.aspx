<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.ErrorPage.ErrorPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Page Error</title>

    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>

    <webopt:BundleReference runat="server" Path="~/Content/css" />
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />

    <link rel="stylesheet" href="../Scripts/Other/lib/font-awesome/css/font-awesome.css">

    <script type="text/javascript">
        $(function () {
            $(".knob").knob();
        });
    </script>

    <link rel="stylesheet" type="text/css" href="../Scripts/Other/stylesheets/theme.css">
    <link rel="stylesheet" type="text/css" href="../Scripts/Other/stylesheets/premium.css">

</head>

<body class="theme-6">

    <header>
        <!--JAVASCRIPT CODE-->
        <script type="text/javascript">
            $(function () {
                var match = document.cookie.match(new RegExp('color=([^;]+)'));
                if (match) var color = match[1];
                if (color) {
                    $('body').removeClass(function (index, css) {
                        return (css.match(/\btheme-\S+/g) || []).join(' ')
                    })
                    $('body').addClass('theme-' + color);
                }

                $('[data-popover="true"]').popover({ html: true });
            });
        </script>
        <style type="text/css">
            #line-chart {
                height: 300px;
                width: 800px;
                margin: 0px auto;
                margin-top: 1em;
            }

            .navbar-default .navbar-brand, .navbar-default .navbar-brand:hover {
                color: #fff;
            }
        </style>
        <script type="text/javascript">
            $(function () {
                var uls = $('.sidebar-nav > ul > *').clone();
                uls.addClass('visible-xs');
                $('#main-menu').append(uls.clone());
            });
        </script>
        <!-- END -->

        <div class="navbar navbar-default" role="navigation">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target=".navbar-collapse">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <a class="" href="../../Views/Home/Inicio"><span class="navbar-brand"><span class="fa fa-paper-plane"></span> Corp. Slin</span></a>
            </div>

            <div class="navbar-collapse collapse" style="height: 1px;">
                <ul id="main-menu" class="nav navbar-nav navbar-right">
                    <li class="dropdown hidden-xs">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown">
                            <span class="glyphicon glyphicon-user padding-right-small" style="position: relative; top: 3px;"></span>
                            <label id="txtuser" runat="server"></label>
                            <i class="fa fa-caret-down"></i>
                        </a>
                    </li>
                </ul>
            </div>
        </div>
    </header>

    <form id="form1" runat="server">
        <asp:ScriptManager runat="server">
            <Scripts>
                <asp:ScriptReference Name="MsAjaxBundle" />
                <asp:ScriptReference Name="jquery" />
                <asp:ScriptReference Name="bootstrap" />
                <asp:ScriptReference Name="respond" />
                <asp:ScriptReference Name="WebForms.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebForms.js" />
                <asp:ScriptReference Name="WebUIValidation.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebUIValidation.js" />
                <asp:ScriptReference Name="MenuStandards.js" Assembly="System.Web" Path="~/Scripts/WebForms/MenuStandards.js" />
                <asp:ScriptReference Name="GridView.js" Assembly="System.Web" Path="~/Scripts/WebForms/GridView.js" />
                <asp:ScriptReference Name="DetailsView.js" Assembly="System.Web" Path="~/Scripts/WebForms/DetailsView.js" />
                <asp:ScriptReference Name="TreeView.js" Assembly="System.Web" Path="~/Scripts/WebForms/TreeView.js" />
                <asp:ScriptReference Name="WebParts.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebParts.js" />
                <asp:ScriptReference Name="Focus.js" Assembly="System.Web" Path="~/Scripts/WebForms/Focus.js" />
                <asp:ScriptReference Name="WebFormsBundle" />
            </Scripts>
        </asp:ScriptManager>
        

        <div class="container">
            <%--<h2>EL SERVICIO NO ESTA DISPONIBLE</h2>--%>

            <h1 runat="server" id="lblmensaje"></h1>
            <br />
            <a class="btn btn-primary" href="../../Views/Home/Inicio"><i class="fa fa-home"></i> Regresar a Inicio</a>
            <a class="btn btn-primary" href="javascript:history.back(-1);"><i class="fa fa-home"></i> Ir a la Pág. Ant.</a>
        </div>

    </form>
</body>
</html>
