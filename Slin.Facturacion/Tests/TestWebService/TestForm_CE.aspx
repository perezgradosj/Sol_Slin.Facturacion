<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" CodeBehind="TestForm_CE.aspx.cs" Inherits="TestWebService.TestForm_CE" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <div>
                <h3>PRUEBA DE CONSUMO DEL SERVICIO</h3>
                <hr />
                <input type="text" runat="server" id="txtxml" />
                <br />
                <br />
                <asp:Button runat="server" ID="btnConsume" OnClick="btnConsume_Click" Text="Consume" />
                <br />
                <label runat="server" id="lblreturn"></label>
            </div>

        </div>
    </form>
</body>
</html>
