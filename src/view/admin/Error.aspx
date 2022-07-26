<%@ Page Title="" Language="C#" MasterPageFile="~/view/master/Admin.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="asf.cms.view.admin.Error" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contenido" runat="server">
<div class="ui-state-error" style="padding:10px">
<%=GetMessage("Error") %>
</div>
</asp:Content>
