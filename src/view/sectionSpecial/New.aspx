<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="New.aspx.cs" Inherits="asf.cms.view.sectionSpecial.New" ValidateRequest="false" MasterPageFile="~/view/master/Admin.Master" EnableViewState="false" %>

<asp:Content ContentPlaceHolderID="head" ID="h" runat="server">
    <script type="text/javascript" src="../view/js/tinymce/jquery.tinymce.js"></script>
    <script type="text/javascript" src="../view/js/editFiles.js"></script>
    <script type="text/javascript" src="../view/js/editMeta.js"></script>
    <script type="text/javascript" src="../view/js/sectionSpecialNew.js"></script>
    
</asp:Content>

<asp:Content ContentPlaceHolderID="contenido" ID="cont"  runat="server">
    <%
    asf.cms.model.PublicationVO p = (asf.cms.model.PublicationVO)GetRequestVar("publication");
    %>
    <script type="text/javascript" language="javascript">
    var metaTags=<%=p.Meta %>;
    </script>
    <h2>Publicaci&oacute;n</h2>
		<br />
        <strong>
            <% if (p.Id > 0)
               { %>
                <a href="../PublicationAdm/EditNew?id=<%=p.Id %>" target="popup" id="btnNewsEdition" onClick="window.open(this.href, this.target, 'width=800,height=800'); return false;">Noticia asociada</a>
                &nbsp;<a href="javascript:void(0)"  id="btnFiles">Archivos</a>
                &nbsp;<a href="javascript:void(0)"  id="btnMeta">Meta-Tags</a>
            <%} %>
        </strong>
        <br />
        <form id="frmNew" action="<%=GetMessage("action") %>" method="post">
        <table cellpadding="6" width="80%">
        <tr>
            <td>Titulo:</td>
            <td>
                <input type="hidden" id="Id" name="Id" value="<%=p.Id %>" />
                <input type="hidden" id="sectionId" name="sectionId" value="<%=p.SectionId %>" />
                <input type="hidden" id="created" name="created" value="<%=p.Created%>" />
                <input type="hidden" id="visitas" name="visitas" value="<%=p.Visitas%>" />
                <input type="text" name="title" id="title" size="45" value="<%=p.Title %>" />
            </td>
           </tr>
        <tr>
            <td>Idioma:</td>
            <td><select name="languageId" id="languageId">
                    <option value="1" <%=p.LanguageId==1?"selected":"" %>>Español</option>
                    <option value="2" <%=p.LanguageId==2?"selected":"" %>>Ingles</option>
                </select>
            <input type="checkbox" name="isMain" id="isMain" value="1" <%=p.IsMain?"checked":"" %> /> Publicacion Principal
            </td>
        </tr>
        <tr>
            <td>Publicar en:</td>
            <td><input type="text" name="published" id="published" value="<%=p.Published %>"  size="30"/></td>
        </tr>
        <tr>
            <td>Retirar en:</td>
            <td><input type="text" name="unpublished" id="unpublished" value="<%=p.Unpublished%>" size="30"/></td>
        </tr>
        <tr>
            <td>Permalink:</td>
            <td><input type="text" name="permalink" id="permalink" readonly="readonly" class="readonly" value="<%=p.Permalink %>" size="45"/></td>
        </tr>
        <tr>
            <td>Posición:</td>
            <td><input type="text" name="Position" id="Position" value="<%=p.Position %>" size="4"/></td>
        </tr>
        </table>
        <br />
        <div>
			    <textarea id="elm1" name="elm1" rows="40" cols="70" style="width: 40%" class="tinymce"><%=p.Content %> </textarea>
        </div>
        <br />
        
		<input type="button" id="btnSave" name="btnSave" value="Aceptar" />
		<input type="reset" name="reset" value="Limpiar" />
        </form>
        <div id="filesPopUp" style="font-size:11px;">
            <br />
            Edite el titulo de los archivos
            <br />
            <br />
            <br />
            <table id="filesList" class="scroll" cellpadding="0" cellspacing="0"></table>
        </div>
        <div id="metaPopUp" style="font-size:11px;">
            <br />
            Tipo: 
            <select id="typeMeta">
                <option value="abstract">Abstract</option>
                <option value="author">Author</option>
                <option value="cache-control">Cache-control</option>
                <option value="copyright">Copyright</option>
                <option value="date">Date</option>
                <option value="description">Description</option>
                <option value="distribution">Distribution</option>
                <option value="expires">Expires</option>
                <option value="keywords">Keywords</option>
                <option value="language">Language</option>
                <option value="pragma">Pragma</option>
                <option value="refresh">Refresh</option>
                <option value="revisit">Revisit</option>
                <option value="robots">Robots</option>
                <option value="title">Title</option>
                <option value="window-target">Window-target</option>
            </select> 
            Valor: <input type="text" id="valueMeta" maxlength="300" size="40" />
            <input type="button" id="btnAgregaMeta" value="Agregar" />
            <br />
            <br />
            <br />
            <table id="metaList" class="scroll" cellpadding="0" cellspacing="0"></table>
        </div>

</asp:Content>