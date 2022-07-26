<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="asf.cms.view.auditReport.Edit"   MasterPageFile="~/view/master/Admin.Master" EnableViewState="false" %>

<asp:Content ContentPlaceHolderID="head" ID="h" runat="server">
    <link rel="stylesheet" type="text/css" href="../view/css/jqthemes/smoothness/jquery-ui-1.8.21.custom.css">

    <script src="../view/js/jquery/jquery-ui.js" type="text/javascript"></script>
    <script src="../view/js/jquery/jquery.form.js" type="text/javascript"></script>

    <script type="text/javascript" src="../view/js/tinymce/jquery.tinymce.js"></script>
    <script type="text/javascript" src="../view/js/tinymce/plugins/filemanager/js/mcfilemanager.js"></script>
    <script type="text/javascript" src="../view/js/editAuditReport.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="contenido" ID="cont"  runat="server">
<%
    asf.cms.model.PublicationVO p = (asf.cms.model.PublicationVO)GetRequestVar("publication");
    asf.cms.model.AuditReportVO a = (asf.cms.model.AuditReportVO)GetRequestVar("auditReport");
%>
		<h2>Informe de Auditor&iacute;a</h2>
  <table width="100%">
  <tr><td>
    <form id="frmNew" action="Edit" method="post">
    <input type="hidden" name="auditReportId" value="<%=a.Id %>" id="auditReportId" />
    <input type="hidden" name="type" id="type" size="5" value="<%=a.Type%>" /> 
    <input type="hidden" name="indexPermalink" id="indexPermalink" size="5" value="<%=a.IndexPermalink%>" /> 
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
        <td><input type="text"  name="directoryPath" id="directoryPath" value="<%=a.DirectoryPath%>" size="45"/> 
        <a href="javascript:void(0)" onclick="selectFolder('directoryPath')"><img src="../view/img/folder_explore.png" alt="Seleccionar" title="Seleccionar" /></a> </td>
    </tr>
    <tr>
        <td>Archivo principal:</td>
        <td><input type="text" name="mainFilePath" id="mainFilePath" value="<%=a.MainFilePath%>"  size="45"/>
        <a href="javascript:void(0)" onclick="selectFile('mainFilePath')"><img src="../view/img/folder_explore.png" alt="Seleccionar" title="Seleccionar" /></a> </td>
    </tr>
    <tr>
        <td>Resumen Ejecutivo:</td>
        <td><input type="text" name="executiveReportPath" id="executiveReportPath" value="<%=a.ExecutiveReportPath %>"   size="45"/>
        <a href="javascript:void(0)" onclick="selectFile('executiveReportPath')"><img src="../view/img/folder_explore.png" alt="Seleccionar" title="Seleccionar" /></a> </td>
    </tr>
    <tr>
        <td>Presentaci&oacute;n:</td>
        <td><input type="text" name="presentationPath" id="presentationPath" value="<%=a.PresentationPath %>"  size="30"/>
            <a href="javascript:void(0)" onclick="selectFile('presentationPath')"><img src="../view/img/folder_explore.png" alt="Seleccionar" title="Seleccionar" /></a> </td>
    </tr>
    <tr>
        <td>Publicar en:</td>
        <td><input type="text" name="published" id="published" value="<%=p.Published %>"  size="30"/></td>
    </tr>
    <tr>
        <td>Retirar en:</td>
        <td><input type="text" name="unpublished" id="unpublished" value="<%=p.Unpublished%>" size="30"/></td>
    </tr>
    </table>
    
	<input type="button" id="btnSave" name="btnSave" value="Aceptar" />
    </form>
    </td>
    
    <td valign="top">
    <h3>Cargar Auditorias del Informe</h3>
    <form action="../Auditoria/Load" method="post" id="loadAuditForm">
            <input type="hidden" name="year" id="loadAuditYear" size="5" value="<%=a.Year%>" /> 
    <table>
    <tr>
        <td>Directorio de auditorias:</td>
        <td><input type="text" id="auditDirectory" name="auditDirectory"  size="45" /><img src="../view/img/folder_explore.png" alt="Seleccionar" title="Seleccionar"  onclick="selectFolder('auditDirectory')"/></a></td>
    </tr>
    <tr>
        <td>Lista de auditorias:</td>
        <td><input type="text" id="auditFile" name="auditFile" size="45" /><img src="../view/img/folder_explore.png" alt="Seleccionar" title="Seleccionar" onclick="selectXLS('auditFile')" /></a></td>
    </tr>
    <tr>
        <td colspan="2"><input type="button" id="btnLoadAudit" name="btnLoadAudit" value="Cargar auditorias" />
        <input type="button" id="btnDeleteAudit" name="btnDeleteAudit" value="Borrar auditorias" />
        </td>
    </tr>
    <tr></tr>
    </table>
    </form>
    <br />
    <br />
    <br />
    <h3>Indice del Informe</h3>
    <table>
    <tr>
        <td colspan="2"><input type="button" id="btnGenerateIndex" name="btnGenerateIndex" value="Generar Indice"  /></td>
    </tr>
    <tr></tr>
    </table>
    </td>
    </tr>
    </table>
</asp:Content>
