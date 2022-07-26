<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="asf.cms.view.Search" MasterPageFile="~/view/master/Default.Master" EnableViewState="false" %>
<%@ Import Namespace="asf.cms.model" %>
<%@ Import Namespace="System.Collections.Generic" %>
<asp:Content ContentPlaceHolderID="head" ID="h" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="contenido" ID="cont"  runat="server">
<% 
    List<SearchResultVO> results = (List<SearchResultVO>)GetRequestVar("results");
    string q = GetMessage("q");
%>
    <div class="contenido_b"> 
    Resultados que coinciden con: <i><b><%=q %></b></i> (Encontrados: <%=results.Count %>)
    </div>
<%foreach (SearchResultVO svo in results)
  { %>
    <div class="searchResult">
        <div class="resultTitle"><a href="<%=svo.Link %>"><%= svo.Title%></a></div>
        <div class="resultLink"><%=svo.Link %></div>
        <div class="resultContent"><%=svo.Content %></div>
    </div>
<%} %>
</asp:Content>
