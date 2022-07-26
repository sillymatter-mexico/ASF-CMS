<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="asf.cms.view.especiales.List" MasterPageFile="~/view/master/Admin.Master" EnableViewState="false" %>

<asp:Content ContentPlaceHolderID="head" ID="h" runat="server">
    <script type="text/javascript" src="../view/js/especialesList.js"></script>
    <script type="text/javascript" src="../view/js/addPubEsp.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="contenido" ID="cont"  runat="server">
    <div class="contenido_b"> 
    
		<h2>Administración de Publicaciones Especiales</h2>
		<%if (GetMessage("userType") == "ADMIN")
      { %>
		<center><p><a href="javascript:void(0)" id="btnNewPublication" style="border:0;text-decoration:none"><img  style="border:0;text-decoration:none;" src="../view/img/add.png" alt"+" /></a> Agregar Publicacion Especial</p></center>
	<%} %>
        <table id="publicationList"  class="scroll" cellpadding="0" cellspacing="0"></table>	
        <input type="hidden" id="hUname" value="<%=((asf.cms.model.UserVO)Session["User"]).Username %>" />
        <input type="hidden" id="newPubSectionData" value="" />
        
	</div>
	<div id="newPublicationPopUp" style="font-size:12px !important"></div>

</asp:Content>
