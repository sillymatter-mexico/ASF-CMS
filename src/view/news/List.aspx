<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="asf.cms.view.news.List" MasterPageFile="~/view/master/Admin.Master" EnableViewState="false" %>

<asp:Content ContentPlaceHolderID="head" ID="h" runat="server">
    <script type="text/javascript" src="../view/js/admin/v3/news/NewsList.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="contenido" ID="cont"  runat="server">
    <div class="contenido_b"> 
    
		<h2>Administración de Noticias</h2>

        <table id="newsList" class="ui celled unstackable table" style="width: 100%"></table>

        <input type="hidden" id="hUname" value="<%=((asf.cms.model.UserVO)Session["User"]).Username %>" />
	</div>
</asp:Content>
