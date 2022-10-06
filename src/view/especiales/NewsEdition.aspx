<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewsEdition.aspx.cs" Inherits="asf.cms.view.publication.NewsEdition" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
		<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
		<title>Edición de noticia asociada a publicación</title>
        <script src="../view/js/jquery/jquery.js" type="text/javascript"></script>
        <script src="../view/js/jquery/jquery.form.js" type="text/javascript"></script>
        <script src="../view/js/jquery/jquery-ui.js" type="text/javascript"></script>
        

        <%--<script src="../view/js/jquery/grid/js/grid.locale-sp.js" type="text/javascript"></script>--%>
        <%--<script src="../view/js/jquery/grid/js/jquery.jqGrid.min.js" type="text/javascript"></script>--%>

    <script type="text/javascript" src="../view/js/jquery/grid_4_15_5/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../view/js/jquery/grid_4_15_5/jquery.jqgrid.min.js"></script>


        <script type="text/javascript" src="../view/js/tinymce/jquery.tinymce.js"></script>
        <script type="text/javascript" src="../view/js/editPubNew.js"></script>
	</head>
<body>
    <form id="frmNew" action="../PublicationAdm/<%=GetMessage("action") %>" method="post">
    <div>

    <%
        asf.cms.model.PublicationVO p = (asf.cms.model.PublicationVO)GetRequestVar("publication");
    %>
  
        <input type="hidden" id="publicationId" name="publicationId" value="<%=p.Id %>" />
        <input type="hidden" id="sectionId" name="sectionId" value="<%=p.SectionId %>" />
        <input type="hidden" id="permalink" name="permalink" value="<%=p.Permalink %>" />
        <table cellpadding="6" width="80%">
            
            
            <tr>
                <td>Incluir en sección: <input type="checkbox" name="cbNewsSectionInclude" id="cbNewsSectionInclude" value="1" <%=bool.Parse(this.GetMessage("sectionNewsInclude"))?"checked":"" %>  size="30"/></td>
            </tr>
            <tr>
                <td>Incluir en publicación: <input type="checkbox" name="cbNewsPublicationInclude" id="cbPublicationInclude" value="1" <%=p.NewsInclude==true?"checked":"" %>  size="30"/></td>
            </tr>
            <tr>
                <td>Días que durará vigente: <input type="text" name="txtNewsTTL" id="txtNewsTTL" value="<%=p.NewsTTL%>"  size="6"/></td>
            </tr>
            <tr>
                <td>Hacer noticia permanente: <input type="checkbox" name="cbNewsPin" id="cbNewsPin" value="1" <%=p.NewsPin==true?"checked":"" %>  size="30"/></td>
            </tr>
        </table>
        <div>
			    <textarea id="newsContent" name="newsContent" rows="30" cols="70" style="width: 40%" class="tinymce"><%=p.NewsContent%> </textarea>
        </div>
	    <br />

        <input type="button" id="btnSave" name="btnSave" value="Aceptar" />
        <input type="button" id="btnCancel" name="btnCancel" value="Cancelar" />

    </div>
    </form>
</body>
</html>
