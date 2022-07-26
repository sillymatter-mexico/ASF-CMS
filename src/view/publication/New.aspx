﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="New.aspx.cs" Inherits="asf.cms.view.publication.New"  MasterPageFile="~/view/master/Admin.Master" EnableViewState="false" ValidateRequest="false" %>

<asp:Content ContentPlaceHolderID="head" ID="h" runat="server">
    <%
        asf.cms.model.PublicationVO p = (asf.cms.model.PublicationVO)GetRequestVar("publication");
        string isSpecial = (!string.IsNullOrEmpty(p.AutogeneratedType)).ToString().ToLower();
    %>

    <script type="text/javascript" src="../view/lib/DataTables/Select-1.3.1/dataTables.select.min.js"></script>
    <script type="text/javascript" src="../view/lib/DataTables/Select-1.3.1/select.dataTables.js"></script>
    <script type="text/javascript" src="../view/lib/DataTables/Select-1.3.1/select.semanticui.min.js"></script>

    <script type="text/javascript" src="../view/js/tinymce/jquery.tinymce.js"></script>
    <script type="text/javascript" src="../view/js/admin/v3/publication/PublicationEdit.js"></script>
    <script type="text/javascript" src="../view/js/admin/v3/publication/PublicationMetaEdit.js"></script>
    <script type="text/javascript" src="../view/js/admin/v3/publication/PublicationFileEdit.js"></script>
    <script type="text/javascript" src="../view/js/admin/v3/publication/PublicationHistoricEdit.js"></script>
    <script type="text/javascript" src="../view/js/admin/v3/publication/PublicationNewsEdit.js"></script>

    <script type="text/javascript">
        var metaTags = <%=p.Meta %>;
        var filesModified = [];
        var isSpecial = <%= isSpecial %>;
        var isMain = <%= p.IsMain.ToString().ToLower() %>;
    </script>

    <link rel="stylesheet" href="../view/lib/DataTables/Select-1.3.1/select.dataTables.min.css" />
    <link rel="stylesheet" href="../view/lib/DataTables/Select-1.3.1/select.semanticui.min.css" />
</asp:Content>

<asp:Content ContentPlaceHolderID="contenido" ID="cont"  runat="server">
<%
    asf.cms.model.PublicationVO p = (asf.cms.model.PublicationVO)GetRequestVar("publication");
%>
<div class="contenido_b">

	<h2>Publicaci&oacute;n</h2>

    <div id="resultMessages" class="ui segments">
        <div id="resultMessage" class="ui segment"><i class="icon"></i><span>Mensaje</span></div>
    </div>

    <button id="btnSave" name="btnSave" class="ui basic primary button"><i class="save icon"></i>Guardar</button>

    <div class="ui basic buttons">
        <button id="btnNewsEdit" class="ui button"><i class="newspaper icon"></i>Noticia</button>
        <button id="btnFilesEdit" class="ui button"><i class="archive icon"></i>Archivos</button>
        <button id="btnMetaEdit" class="ui button"><i class="tags icon"></i>Meta</button>
        <button id="btnHistoricEdit" class="ui button"><i class="history icon"></i>Historicos</button>
    </div>

    <h4 class="ui dividing header">Información</h4>

    <form id="frmNew" class="ui form" action="<%=GetMessage("action") %>" method="post">

        <input type="hidden" id="Id" name="Id" value="<%=p.Id %>" />
        <input type="hidden" id="created" name="created" value="<%=p.Created%>" />
        <input type="hidden" id="visitas" name="visitas" value="<%=p.Visitas%>" />
        <input type="hidden" id="languageId" name="languageId" value="<%=p.LanguageId%>" />
        
        <div class="ui error message"></div>

        <div class="fields">
            <div class="one wide field">
                <label>Principal</label>
                <div class="ui toggle checkbox">
                    <input type="checkbox" name="isMain" id="isMain" <%=p.IsMain?"checked":"" %> <%=GetMessage("readonly")%> <%=string.IsNullOrEmpty(GetMessage("readonly")) && p.IsMain ? "readonly" : "" %> tabindex="0" class="hidden" />
                </div>
            </div>
            <div class="nine wide field">
                <label>Título</label>
                <input type="text" name="title" id="title" value="<%=p.Title %>" />
            </div>
            <div class="six wide field">
                <label>Permalink</label>
                <input type="text" name="permalink" id="permalink" readonly="readonly" class="readonly" value="<%=p.Permalink %>" />
            </div>
        </div>

        <div class="fields">
            <div class="eight wide field">
                <label>Publicar en</label>
                <div id="published" class="ui calendar">
                    <div class="ui input left icon">
                        <i class="calendar icon"></i>
                        <input type="text" name="published"  value="<%=p.Published %>" />
                    </div>
                </div>
            </div>
            <div class="eight wide field">
                <label>Retirar en</label>
                <div id="unpublished" class="ui calendar">
                    <div class="ui input left icon">
                        <i class="calendar icon"></i>
                        <input type="text" name="unpublished"  value="<%=p.Unpublished%>" />
                    </div>
                </div>
            </div>
        </div>
               
        <div class="fields">
            <div class="eight wide field">
                <label>Sección superior</label>
            </div>
            <div class="two wide field">
                <label>Posición</label>
                <input type="text" name="Position" id="Position" value="<%=p.Position %>" />
            </div>
            <div class="six wide field">
                <label>Clase</label>
                <input type="text" name="CssClass" id="CssClass" value="<%=p.CssClass %>" />
            </div>
        </div>

        <h4 class="ui dividing header">Contenidos</h4>

        <div class="sixteen wide field">
		    <textarea id="elm1" name="elm1" rows="40" cols="70" style="width: 40%" class="tinymce"><%=p.Content %> </textarea>
        </div>

    </form>

    <input type="hidden" id="parentSectionIdOld" name="parentSectionIdOld" value="<%=p.SectionId %>" />

    <div id="newsModalPanel" class="ui large modal">
        <i class="close icon"></i>
        <div class="header">Noticia</div>
        <div class="content">
            <form id="publicationNewsForm" class="ui form">

                <div class="ui error message"></div>

                <div class="fields">
                    <div class="five wide field"></div>
                    <div class="two wide field">
                        <label>Permanente</label>
                        <div class="ui toggle checkbox">
                            <input type="checkbox" name="pubNewPermanent" id="pubNewPermanent" class="hidden" <%=p.NewsPin ? "checked":"" %> />
                        </div>
                    </div>
                    <div class="two wide field">
                        <label>Sección</label>
                        <div class="ui toggle checkbox">
                            <input type="checkbox" name="pubNewSectionInclude" id="pubNewSectionInclude" class="hidden" <%=GetRequestVar("sectionNewsInclude") as String == "true" ? "checked":"" %> />
                        </div>
                    </div>
                    <div class="two wide field">
                        <label>Incluir</label>
                        <div class="ui toggle checkbox">
                            <input type="checkbox" name="pubNewInclude" id="pubNewInclude" class="hidden" <%=p.NewsInclude ? "checked":"" %> />
                        </div>
                    </div>
                    <div class="four wide field">
                        <label>Duración</label>
                        <input type="text" name="pubNewTTL" id="pubNewTTL" value="<%=p.NewsTTL %>" />
                    </div>
                    <div class="five wide field"></div>
                </div>

                <h4 class="ui dividing header">Contenido de noticia</h4>

                <div class="sixteen wide field">
		            <textarea id="pubNewContent" name="pubNewContent" rows="40" cols="70" class="tinymce"><%=p.NewsContent %></textarea>
                </div>
            </form>
        </div>
        <div class="actions">
            <button class="ui primary deny button"><i class="cancel icon"></i>Cancelar</button>
            <button class="ui secondary approve button"><i class="save icon"></i>Guardar</button>
        </div>
    </div>

    <div id="filesModalPanel" class="ui large modal">
        <i class="close icon"></i>
        <div class="header">Archivos</div>
        <div class="content">
            <table id="filesList" class="ui celled unstackable table" style="width: 100%"></table>
        </div>
        <div class="actions">
            <button class="ui primary deny button"><i class="cancel icon"></i>Cerrar</button>
        </div>
    </div>

    <div id="metaModalPanel" class="ui large modal">
        <i class="close icon"></i>
        <div class="header">Meta-Tags</div>
        <div class="content">
            <form class="ui form" id="metaForm">
                <div class="ui error message"></div>
                <div class="fields">
                    <div class="six wide field">
                        <label>Tipo</label>
                        <select id="metaType" name="metaType">
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
                    </div>
                    <div class="ten wide field">
                        <label>Valor</label>
                        <input type="text" id="metaValue" name="metaValue" maxlength="300" />
                    </div>
                </div>
                <button class="ui basic button" id="btnAgregaMeta"><i class="icons"><i class="tag icon"></i><i class="add corner icon"></i></i>Agregar</button>
            </form>

            <table id="metaList" class="ui celled unstackable table" style="width:100%"></table>

        </div>
        <div class="actions">
            <button class="ui primary deny button"><i class="cancel icon"></i>Cancelar</button>
            <button class="ui secondary approve button"><i class="save icon"></i>Guardar</button>
        </div>
    </div>

    <div id="historicModalPanel" class="ui large modal">
        <div class="header">Históricos</div>
        <div class="content">
            <table id="historicList" class="ui celled unstackable selectable table"></table>
            
            <h4 class="ui dividing header">Vista previa</h4>

            <div class="ui top attached tabular menu">
                <a class="item active" data-tab="historicContent">Contenido</a>
                <a class="item" data-tab="historicNews">Noticia</a>
            </div>
            <div id="historicContent" class="ui bottom attached tab segment active center aligned" data-tab="historicContent">
                <i class="big ban icon"></i><br />No hay contenido
            </div>
            <div id="historicNews" class="ui bottom attached tab segment center aligned" data-tab="historicNews">
                <i class="big ban icon"></i><br />No hay contenido
            </div>
        </div>
        <div class="actions">
			<div class="ui deny primary button">Cerrar</div>
		</div>
    </div>

    <div id="historicConfirmModalPanel" class="ui modal">
		<div class="header">Recuperar histórico</div>
		<div class="content">
			<div class="ui grid">
                <div class="row">
                    <div class="four wide center aligned column"><i class="huge yellow exclamation triangle icon"></i></div>
				    <div class="twelve wide column">
                        <p>
                            Seleccione los campos deseados a recuperar de la versión histórica. Al aceptar la recuperación se sobre escribirán los campos seleccionados en la publicación.
                        </p>
                        <input id="historicId" name="id" type="hidden" value="<%=p.Id %>" />
                        <input id="historicPublicationId" name="historicId" type="hidden" />
                        <div class="ui checkbox"><input id="historicFieldContent" name="fieldContent" type="checkbox" /><label>Contenido</label></div><br />
                        <div class="ui checkbox"><input id="historicFieldNews" name="fieldNews" type="checkbox" /><label>Noticia</label></div><br />
                        <div class="ui checkbox"><input id="historicFieldTitle" name="fieldTitle" type="checkbox" /><label>Título</label></div><br />
				    </div>
                </div>
			</div>
		</div>
		<div class="actions">
			<div class="ui deny primary button">Cancelar</div>
			<div class="ui approve disabled secondary button">Aceptar</div>
		</div>
	</div>

</div>

</asp:Content>
