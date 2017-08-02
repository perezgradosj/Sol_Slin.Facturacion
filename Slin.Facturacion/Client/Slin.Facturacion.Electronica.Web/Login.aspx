<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <%--<script type="text/javascript">
        javascript:window.history.forward(1); //Esto es para cuando le pulse al botón de Atrás
        //javascript:window.history.back(1); //Esto para cuando le pulse al botón de Adelante
    </script>--%>


    <META HTTP-EQUIV="Cache-Control" CONTENT ="no-cache">
    <%--<meta http-equiv="set-cookie" content="no-cache" />--%>
    <meta charset="utf-8" >
    
    <script>
        <% 
            Response.AppendHeader("Pragma", "no-cache");
            Response.AppendHeader("Cache-Control", "no-cache");
            Response.CacheControl = "no-cache"; Response.Expires = -1;
            Response.ExpiresAbsolute = new DateTime(1900, 1, 1);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
        %>
    </script>

    <title>Acceso Sistema</title>

    <meta content="IE=edge,chrome=1" http-equiv="X-UA-Compatible">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="">

    <link rel="stylesheet" href="Scripts/Other/lib/font-awesome/css/font-awesome.css">

    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />

    <script src="Scripts/Other/lib/jquery-1.11.1.min.js" type="text/javascript"></script>

    <script src="Scripts/Util/custom.js"></script>

    <link rel="stylesheet" type="text/css" href="Scripts/Other/stylesheets/theme.css">
    <link rel="stylesheet" type="text/css" href="Scripts/Other/stylesheets/premium.css">


    <script type="text/javascript">
        {
            if (history.forward(1))
                location.replace(history.forward(1))
        }
    </script>

    <script type="text/javascript">
        window.history.forward(1);
        //window.history.forward();
    </script> 
    

    <script type="text/javascript">
        if (history.forward(1)) { location.replace(history.forward(1)) }
    </script>
</head>
<body class="theme-6" style="background-image: url(Img/home/fondo.jpg); background-repeat: repeat-y; background-size: 100%;">

    <%--<form id="form1" runat="server">
    </form>--%>

    <script type="text/javascript">
        $(function() {
            var match = document.cookie.match(new RegExp('color=([^;]+)'));
            if(match) var color = match[1];
            if(color) {
                $('body').removeClass(function (index, css) {
                    return (css.match (/\btheme-\S+/g) || []).join(' ')
                })
                $('body').addClass('theme-' + color);
            }

            $('[data-popover="true"]').popover({html: true});
        });
    </script>


    <style type="text/css">
        #line-chart {
            height:100px;
            width:600px;
            margin: 0px auto;
            margin-top: 1em;
        }
        .navbar-default .navbar-brand, .navbar-default .navbar-brand:hover { 
            color: #fff;
        }
    </style>
    <script type="text/javascript">
        $(function() {
            var uls = $('.sidebar-nav > ul > *').clone();
            uls.addClass('visible-xs');
            $('#main-menu').append(uls.clone());
        });
    </script>

    <div class="navbar navbar-default" role="navigation" >
        <div class="navbar-header" >
            <a class="" href="Login.aspx"><span class="navbar-brand" style="font-family:Cambria; font-size:medium;"><img runat="server" id="logoade" style="max-width:250px" />  Administración de Documentos Electrónicos</span></a>
        </div>
    </div>
    
    
    <div class="dialog" >
        <div class="panel panel-default" style="font-family:Cambria">
            <label class="panel-heading no-collapse" ><i class="fa fa-lock"></i> Iniciar Sesión</label>
            <div class="panel-body" style="font-family:Cambria">
                <form runat="server">

                    <div class="row">
                        <div class="col-md-3">
                            <label style="height:27px; padding-top: 7px">Empresa:</label>
                        </div>

                        <div class="col-md-3">                            
                            <select runat="server" id="cboempresa" class="form-control" style="max-width: 320px; min-width: 200px"></select>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-3">
                            <label style="height:27px; padding-top: 7px">Usuario:</label>
                        </div>
                        
                        <div class="col-md-7">
                            <input runat="server" id="txtusuario" class="form-control" maxlength="30" required="required" autocomplete="off" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-3">
                            <label style="height:27px; padding-top: 7px">Clave:</label>
                        </div>
                        
                        <div class="col-md-7">
                            <input runat="server" id="txtpassword" class="form-control" type="password" maxlength="30" required="required" autocomplete="off"/>
                            <label class="form-control" runat="server" id="lblmensaje" style="border:none;" visible="false"></label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-7">
                            <label></label>
                        </div>
                        <div class="col-md-2" style="align-content:flex-end">
                            <asp:Button runat="server" Height="30px" Font-Names="Cambria" CssClass="btn btn-primary" ID="btnIngreso" Font-Size="Small" OnClick="btnIngreso_Click" Text="Ingresar" />
                        </div>
                    </div>
                </form>
            </div>
        </div>
        <p class="pull-right" style=""><a href="#" target="blank" style="font-size: .75em; margin-top: .25em; color:white;">Design by Corp. Slin</a></p>
    </div>

    <script src="Scripts/Other/lib/bootstrap/js/bootstrap.js"></script>
    <script type="text/javascript">
        $("[rel=tooltip]").tooltip();
        $(function() {
            $('.demo-cancel-click').click(function(){return false;});
        });
    </script>
</body>
</html>
