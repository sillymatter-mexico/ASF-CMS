<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PopUp.aspx.cs" Inherits="asf.cms.view.especiales.PopUp" %>
<style type="text/css">
#frmPopUp table td {font-size:12px;}

</style>

    <%
        asf.cms.model.EspecialesVO e = (asf.cms.model.EspecialesVO)GetRequestVar("publication");
    %>
  	
        <form id="frmNewPublicationPopUp" action="Insert" method="post">
        <input type="hidden" name="sectionId" id="sectionIdNewPublication" value="<%=e.SectionId %>" />
        <table cellpadding="6" width="80%">
        <tr>
            <td>Titulo:</td>
            <td>
                <input type="text" name="title" id="titleNewPublication" size="45"  />
            </td>
        </tr>
        <!-- <tr>
            <td>Tipo:</td>
            <td>
                <!-- <input type="text" name="type" id="typeNewPublication" size="45"  /> ->
            </td>
        </tr> -->
        <tr>
            <td>Permalink:</td>
            <td><input type="text" name="permalink" id="permalinkNewPublication" readonly="readonly" class="readonly" size="45"/></td>
        </tr>
        </table>
        </form>
