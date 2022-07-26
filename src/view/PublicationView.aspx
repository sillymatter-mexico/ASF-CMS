<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PublicationView.aspx.cs" Inherits="asf.cms.view.PublicationView" MasterPageFile="~/view/master/Default.Master" EnableViewState="false" %>

<%@ Import Namespace="asf.cms.bll" %>
<%@ Import Namespace="asf.cms.controller" %>
<%@ Import Namespace="asf.cms.model" %>
<%@ Import Namespace="System.Collections.Generic" %>

<asp:Content ContentPlaceHolderID="HeaderContent" ID="Header" runat="server">
<%
    bool menu_mode_smart = GetRequestVar("menu_mode_smart") != null ? (bool)GetRequestVar("menu_mode_smart") : false,
        menu_show_sibling = GetRequestVar("menu_show_sibling") != null ? (bool)GetRequestVar("menu_show_sibling") : false,
        menu_show_superior = GetRequestVar("menu_show_superior") != null ? (bool)GetRequestVar("menu_show_superior") : false,
        is_top_menu = GetRequestVar("is_root_section") != null ? !(bool)GetRequestVar("is_root_section") : true;
    List<ContentElement> topMenu = new List<ContentElement>();
    List<ContentElement> topMenu2 = new List<ContentElement>();

    if(menu_mode_smart)
        topMenu = is_top_menu ?  ContentElementSiblingMenu(false) : ContentElementFromMenu((asf.cms.bll.Menu)GetRequestVar("menu_superior"), false);
    else if(menu_show_superior && menu_show_sibling)
    {
        topMenu = ContentElementFromMenu((asf.cms.bll.Menu)GetRequestVar("menu_superior"), false);
        topMenu2 = ContentElementSiblingMenu(false);
    }
    else if(menu_show_superior)
        topMenu = ContentElementFromMenu((asf.cms.bll.Menu)GetRequestVar("menu_superior"), false);
    else if(menu_show_sibling)
        topMenu = ContentElementSiblingMenu(false);

    foreach (ContentElement elem in topMenu)
    {
    %>
    <li><a href="<%=elem.Link %>"><%=elem.Title %></a></li>
    <%
    }
 
    foreach (ContentElement elem2 in topMenu2)
    {
    %>
    <li><a href="<%=elem2.Link %>"><%=elem2.Title %></a></li>
    <%
    }
%>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" ID="Contenido"  runat="server">
<%
    ContentElement publicationContent = (ContentElement)GetRequestVar("publicationContent");
    bool isSpecial = (bool)GetRequestVar("publicationIsSpecial");
%>
<div id="Con_<%=publicationContent.Link.Split('/')[2] %>" class="fondo2 <%=publicationContent.Class %>">
    <div class="cMax">
        <h2 style="color: #AC9C63;"><%=publicationContent.Title %></h2>
        <div id="<%=publicationContent.Link.Split('/')[2] %>">
            <%
    if (!String.IsNullOrEmpty(publicationContent.MainImage))
        Response.Write("<img  style=\"" + "max-width:85%; " + "\"" + " id=\"Img_" + publicationContent.Link.Split('/')[2] + "\" src=\"" + publicationContent.MainImage + "\"" + "/><br>");
    if (isSpecial)
        Response.Write("<span id=\"Date_" + publicationContent.Link.Split('/')[2] + "\">" + publicationContent.Published.ToShortDateString() + "</span>");
            %>
            <%=publicationContent.Content %>
        </div>
    </div>
</div>

</asp:Content>
