<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SecurityScreen.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Banner.SecurityScreen" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Banner</title>

    <meta content="IE=edge,chrome=1" http-equiv="X-UA-Compatible">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="">

    <link rel="stylesheet" href="../../Scripts/Other/lib/font-awesome/css/font-awesome.css">

    <link href="../../Content/bootstrap.css" rel="stylesheet" />
    <link href="../../Content/bootstrap.min.css" rel="stylesheet" />

    <script src="../../Scripts/Other/lib/jquery-1.11.1.min.js" type="text/javascript"></script>

    <script src="../../Scripts/Util/custom.js"></script>

    <link rel="stylesheet" type="text/css" href="../../Scripts/Other/stylesheets/theme.css">
    <link rel="stylesheet" type="text/css" href="../../Scripts/Other/stylesheets/premium.css">


    <%--<script type="text/javascript">
        function pedirConfirmacion() {
            if (confirm("Do you want to save data?")) {
                __doPostBack('', 'SaveData'); //"SaveData" es un argumento que se chequeará en el Load de la página
            }
        }
    </script>--%>

    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Desea Continuar?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>


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
<body style="background-color: #494646">
    <form id="form1" runat="server">

        <div class="modal-header">
            <h3 runat="server" id="lblRazonSocialEmpresa" style="text-align: center; color: white"></h3>
        </div>
        <div class="modal-body">


            <br />
            <br />
            <br />


            <div class="row">
                <div class="col-lg-4">
                </div>

                <div class="col-lg-4" style="text-align: center">
                    <img runat="server" id="lblLogoEmpresa" style="max-width: 250px" />
                </div>

                <div class="col-lg-4">
                </div>
            </div>

            <br />
            <br />
            <br />

            <div class="row" style="text-align: center">

                <br />

                <br />

                <div class="row" style="color: white">

                    <div class="col-lg-2">
                    </div>

                    <div class="col-lg-8">
                        <asp:Panel runat="server" BorderColor="Orange" BorderWidth="1px">
                            <br />
                            <b style="text-align: left; font-family: Cambria; color: black">Esta Aplicación tiene Clasificación DC2 (Distribución Restringida)</b>
                            <br />
                            <br />
                            <label style="font-family: Cambria">La Infomación almacenada o procesada en este sistema es propiedad de PricewaterhouseCoopers S. Civil de R.L.</label>
                            <br />
                            <label style="font-family: Cambria">y su uso está estrictamente restringido a los usuarios autorizados por PwC. Usted debe saber que este sistema</label>
                            <br />
                            <label style="font-family: Cambria">es continuamente monitoreado en favor de la aplicación de las leyes locales y otros propósitos de la empresa.</label>
                            <br />
                            <asp:Button ID="btnConfirm" BackColor="Orange" Font-Names="Cambria" ForeColor="White" CssClass="btn btn-sm" Font-Bold="true" runat="server" OnClick="btnConfirm_Click" Text="Aceptar" /><!-- OnClientClick="Confirm()" -->
                            <asp:Button ID="btnCancelar" BackColor="Orange" Font-Names="Cambria" ForeColor="White" CssClass="btn btn-sm" Font-Bold="true" runat="server" OnClick="btnCancelar_Click" Text="Cancelar" />
                            <br />
                            <br />
                        </asp:Panel>
                    </div>



                    <div class="col-lg-2">
                    </div>
                </div>

                <div class="row">
                    <div class="col-lg-4">
                    </div>
                    <div class="col-lg-4">
                    </div>
                    <div class="col-lg-4">
                    </div>
                </div>

                <br />

            </div>

            <div class="row">
            </div>

        </div>
    </form>
</body>
</html>
