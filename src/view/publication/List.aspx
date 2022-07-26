<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="asf.cms.view.publication.List" MasterPageFile="~/view/master/Admin.Master" EnableViewState="false" %>
<%@ Register Src="../ContentAddModal.ascx" TagName="ContentAddModal" TagPrefix="asfcms" %>

<asp:Content ContentPlaceHolderID="head" ID="h" runat="server">
    <script type="text/javascript" src="../view/js/admin/v3/publication/PublicationList.js"></script>
    <%
        if (((asf.cms.model.UserVO)Session["User"]).Type == "ADMIN")
        {
            %>
            <script type="text/javascript" src="../view/js/admin/v3/ContentAdd.js"></script>
            <script type="text/javascript">
                var addEnable = true;
            </script>
            <%
        } else {
            %>
            <script type="text/javascript">
                var addEnable = false;
            </script>
            <%
        }
    %>
</asp:Content>

<asp:Content ContentPlaceHolderID="contenido" ID="cont"  runat="server">
    <div class="contenido_b"> 
        <h2>Administración de Publicaciones</h2>
		
        <div id="resultMessages" class="ui segments">
            <div id="resultMessage" class="ui segment"><i class="icon"></i><span>Mensaje</span></div>
        </div>

        <table id="publicationList" class="ui celled unstackable table" style="width: 100%">

        </table>	

        <input type="hidden" id="hUname" value="<%=((asf.cms.model.UserVO)Session["User"]).Username %>" />
        
        <%
            if (GetMessage("userType") == "ADMIN")
            {
                %>
                    <asfcms:ContentAddModal ID="SectionAddModal" runat="Server" />
	            <%
            }
        %>

	</div>
</asp:Content>
