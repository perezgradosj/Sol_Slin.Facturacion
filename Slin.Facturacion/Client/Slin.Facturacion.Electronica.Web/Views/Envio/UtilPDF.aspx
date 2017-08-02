<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UtilPDF.aspx.cs" Inherits="Slin.Facturacion.Electronica.Web.Views.Envio.UtilPDF" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>

        <rsweb:reportviewer runat="server" id="ReportViewer1">
            <%--<LocalReport ReportEmbeddedResource="Report.Reporte.MilmoFactura.rdlc" >
        </LocalReport>--%>
        </rsweb:reportviewer>

        <asp:ScriptManager ID="ScriptManager" runat="server">
        </asp:ScriptManager>
    </form>
</body>
</html>
