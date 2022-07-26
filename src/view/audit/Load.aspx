<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Load.aspx.cs" Inherits="asf.cms.view.audit.Load"   MasterPageFile="~/view/master/Admin.Master" EnableViewState="false" %>

<asp:Content ContentPlaceHolderID="head" ID="h" runat="server">
    <script type="text/javascript" src="../view/js/tinymce/jquery.tinymce.js"></script>
    <script type="text/javascript" src="../view/js/editAuditReport.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="contenido" ID="cont"  runat="server">
<%
    //    asf.cms.model.PublicationVO p = (asf.cms.model.PublicationVO)GetRequestVar("publication");
    asf.cms.model.PublicationVO p = new asf.cms.model.PublicationVO();
    asf.cms.model.AuditReportVO a = new asf.cms.model.AuditReportVO();
%>
  
		<h2>Informe de Auditor&iacute;a</h2>
        <form id="frmNew" action="<%=GetMessage("action") %>" method="post">
        <table cellpadding="6" width="80%">
        <tr>
            <td>A&ntilde;o:</td>
            <td>
                <input type="text" name="year" id="year" size="5" value="<%=a.Year%>" />
            </td>
          </tr>
        <tr>
            <td>Titulo:</td>
            <td><input type="text" name="title" id="title" value="<%=p.Title%>" size="45"/></td>
        </tr>
        <tr>
            <td>Permalink:</td>
            <td><input type="text" name="permalink" id="permalink" readonly="readonly" class="readonly" value="<%=p.Permalink %>" size="45"/></td>
        </tr>
          <tr>
            <td>Ruta del Informe:</td>
            <td><input type="text"  name="directoryPath" id="directoryPath"/> </td>
        </tr>
        <tr>
            <td>Archivo principal:</td>
            <td><input type="text" name="mainFilePath" id="mainFilePath" value="<%=a.MainFilePath%>"  size="30"/></td>
        </tr>
        <tr>
            <td>Resumen Ejecutivo:</td>
            <td><input type="text" name="executiveReportPath" id="executiveReportPath" value="<%=a.ExecutiveReportPath %>"  size="30"/></td>
        </tr>
        <tr>
            <td>Presentaci&oacute;n:</td>
            <td><input type="text" name="presentationPath" id="presentationPath" value="<%=a.PresentationPath %>"  size="30"/></td>
        </tr>
        <tr>
            <td>Pubslicar en:</td>
            <td><input type="text" name="published" id="Text3" value="<%=p.Published %>"  size="30"/></td>
        </tr>
        <tr>
            <td>Retirar en:</td>
            <td><input type="text" name="unpublished" id="unpublished" value="<%=p.Unpublished%>" size="30"/></td>
        </tr>
        </table>
        
		<input type="button" id="btnSave" name="btnSave" value="Aceptar" />
		<input type="reset" name="reset" value="Limpiar" />
        </form>
</asp:Content>
