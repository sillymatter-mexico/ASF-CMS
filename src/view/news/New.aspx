<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="New.aspx.cs" Inherits="asf.cms.view.news.New"  MasterPageFile="~/view/master/Admin.Master" EnableViewState="false" %>

<asp:Content ContentPlaceHolderID="head" ID="h" runat="server">
    <script type="text/javascript" src="../view/js/tinymce/jquery.tinymce.js"></script>
    <script type="text/javascript" src="../view/js/editNew.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="contenido" ID="cont"  runat="server">
<%
    asf.cms.model.NewsVO n = (asf.cms.model.NewsVO)GetRequestVar("new");
%>
    <h2>Noticia</h2>
    <form id="frmNew" action="<%=GetMessage("action") %>" method="post">
        <input type="hidden" id="Id" name="Id" value="<%=n.Id %>" />
        <input type="hidden" id="IsSection" name="IsSection" value="<%=n.IsSection %>" />
        
        <% if (n.IsSection)
           { %>
        <table cellpadding="6" width="80%">
            <tr>
                <td>
                   <h3>
                    <%= n.Title%>
                   </h3> 
                </td>
            </tr>
            <tr>
                <td>
                    Incluir en sección: <input type="checkbox" name="cbNewsSectionInclude" id="Checkbox1" value="1" <%=(n.IncludeInSection)?"checked":"" %>  size="30"/>
                </td>
            </tr>
        </table>
        <% }
           else
           { %>
        <table cellpadding="6" width="80%">
            <tr>
                <td>
                   <h3>
                    <%= n.Title%>
                   </h3> 
                </td>
            </tr>
            <tr>
                <td>Incluir en publicación: <input type="checkbox" name="cbNewsPublicationInclude" id="cbPublicationInclude" value="1" <%=(n.IncludeInPublication)?"checked":"" %>  size="30"/></td>
            </tr>
            <tr>
                <td>Días que durará vigente: <input type="text" name="txtNewsTTL" id="txtNewsTTL" value="<%=n.NewsTTL%>"  size="6"/></td>
            </tr>
            <tr>
                <td>Hacer noticia permanente: <input type="checkbox" name="cbNewsPin" id="cbNewsPin" value="1" <%=n.NewsPin==true?"checked":"" %>  size="30"/></td>
            </tr>
        </table>
        <div>
			<textarea id="newsContent" name="newsContent" rows="40" cols="70" style="width: 40%" class="tinymce"><%=n.Content%> </textarea>
        </div>
        <%} %>

	    <br />
        

		<input type="button" id="btnSave" name="btnSave" value="Aceptar" />
		<input type="reset" name="reset" value="Limpiar" />

    </form>
</asp:Content>
