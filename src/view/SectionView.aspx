<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SectionView.aspx.cs" Inherits="asf.cms.view.SectionView" MasterPageFile="~/view/master/Default.Master" EnableViewState="false" %>

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

  %>


<!--

  <header>
    <input type="checkbox" id="nav-toggle" class="nav-toggle">
    <nav id="nav">
      <ul>
        <span class="pnoli">
          <a class="noli" href="/Default/Index" title="Inicio">
            <img alt="" src="/view/assets/imagenes/iconos/home_d.png"></a>
          <a class="noli" href="/Section/50_Transparencia" title="Transparencia">
            <img alt="" src="/view/assets/imagenes/iconos/inai_D.png"></a>
          <a class="noli" href="/Section/58_Informes_de_auditoria" title="Informes">
            <img alt="" src="/view/assets/imagenes/iconos/informes_d.png"></a>
          <a class="noli msg" href="#" title="Contacto">
            <img alt="" src="/view/assets/imagenes/iconos/correo_d.png"></a>
          <a class="noli" href="/Section/210_Directorio" title="Directorio">
            <img alt="" src="/view/assets/imagenes/iconos/telefono_D.png"></a>
          <a class="noli" href="/Section/211_Busqueda" title="B&uacute;squeda">
            <img alt="" src="/view/assets/imagenes/iconos/lupa_d.png"></a>
        </span>
      </ul>
    </nav>
    <label for="nav-toggle" class="nav-toggle-label">
      <span></span>
    </label>
  </header>


-->

<!--

  <header>
    <input type="checkbox" id="nav-toggle" class="nav-toggle">
    <nav id="nav">
<ul>
             <span class="pnoli">
          <a class="noli" href="/Default/Index" title="Inicio">
            <img alt="" src="/view/assets/imagenes/iconos/home_d.png"></a>
          <a class="noli" href="/Section/50_Transparencia" title="Transparencia">
            <img alt="" src="/view/assets/imagenes/iconos/inai_D.png"></a>
          <a class="noli" href="/Section/58_Informes_de_auditoria" title="Informes">
            <img alt="" src="/view/assets/imagenes/iconos/informes_d.png"></a>
          <a class="noli msg" href="#" title="Contacto">
            <img alt="" src="/view/assets/imagenes/iconos/correo_d.png"></a>
          <a class="noli" href="/Section/210_Directorio" title="Directorio">
            <img alt="" src="/view/assets/imagenes/iconos/telefono_D.png"></a>
          <a class="noli" href="/Section/211_Busqueda" title="B&uacute;squeda">
            <img alt="" src="/view/assets/imagenes/iconos/lupa_d.png"></a>
        </span>-->

        <%
            foreach (ContentElement elem in topMenu)
            {
            %>
            <li><a href="<%=elem.Link %>" target="<%=elem.Target %>"><%=elem.Title %></a></li>
            <%
            }
        %>
    <!--  </ul>
      <ul>-->
        <%
            foreach (ContentElement elem2 in topMenu2)
            {
            %>
            <li><a href="<%=elem2.Link %>" target="<%=elem2.Target %>"><%=elem2.Title %></a></li>
            <%
            }
        %>
 
   <!--  </ul>
    </nav>
    <label for="nav-toggle" class="nav-toggle-label">
      <span></span>
    </label>
  </header>
-->
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" ID="Contenido" runat="server">

    <%
        List<ContentElement> sectionContent = (List<ContentElement>)GetRequestVar("sectionContent"); //GetCurrentSectionContent(true);
        List<ContentElement>.Enumerator contentEnumerator = sectionContent.GetEnumerator();
        bool isSpecial = (bool)GetRequestVar("sectionIsSpecial");
        if (contentEnumerator.MoveNext())
        {
    %>
    <div id="Con_<%=contentEnumerator.Current.Link.Split('/')[2] %>" class="fondo2 <%=contentEnumerator.Current.Class %>">
        <div class="cMax">
            <h2 style="color: #AC9C63;"><%=contentEnumerator.Current.Title %></h2>
            <div id="<%=contentEnumerator.Current.Link.Split('/')[2] %>">
                <%
                    if (!String.IsNullOrEmpty(contentEnumerator.Current.MainImage))
                        Response.Write("<img  style=\"" + "max-width:85%; " + "\"" + " id=\"Img_" + contentEnumerator.Current.Link.Split('/')[2] + "\" src=\"" + contentEnumerator.Current.MainImage + "\"" + "/><br>");
                    if (isSpecial)
                        Response.Write("<span id=\"Date_" + contentEnumerator.Current.Link.Split('/')[2] + "\">" + contentEnumerator.Current.Published.ToShortDateString() + "</span>");
                %>
                <%=contentEnumerator.Current.Content %>
            </div>
        </div>
        <%
            if (contentEnumerator.MoveNext())
            {
        %>
        <div id="cn_<%=contentEnumerator.Current.Link.Split('/')[2] %>" class="fondo3 <%=contentEnumerator.Current.Class %>">
            <div class="cMax">
                <h2><strong><%=contentEnumerator.Current.Title %></strong></h2>
                <div id="<%=contentEnumerator.Current.Link.Split('/')[2] %>">
                <%
                    if (!String.IsNullOrEmpty(contentEnumerator.Current.MainImage))
                        Response.Write("<img  style=\"" + "max-width:85%; " + "\"" + " id=\"Img_" + contentEnumerator.Current.Link.Split('/')[2] + "\" src=\"" + contentEnumerator.Current.MainImage + "\"" + "/><br>");
                    if (isSpecial)
                        Response.Write("<span id=\"Date_" + contentEnumerator.Current.Link.Split('/')[2] + "\">" + contentEnumerator.Current.Published.ToShortDateString() + "</span>");
                %>
                  <%=contentEnumerator.Current.Content %>
                </div>
            </div>
        </div>
      
    </div>
    <%
    while (contentEnumerator.MoveNext())
    {%>
    <div id="Cont_<%=contentEnumerator.Current.Link.Split('/')[2] %>" class="fondo6 <%=contentEnumerator.Current.Class %>">
        <div id="<%=contentEnumerator.Current.Link.Split('/')[2] %>" class="cMax">

            <h2><%=contentEnumerator.Current.Title %></h2>

            <div>
                <%
                    if (!String.IsNullOrEmpty(contentEnumerator.Current.MainImage))
                        Response.Write("<img  style=\"" + "max-width:85%; " + "\"" + " id=\"Img_" + contentEnumerator.Current.Link.Split('/')[2] + "\" src=\"" + contentEnumerator.Current.MainImage + "\"" + "/><br>");
                    if (isSpecial)
                        Response.Write("<span id=\"Date_" + contentEnumerator.Current.Link.Split('/')[2] + "\">" + contentEnumerator.Current.Published.ToShortDateString() + "</span>");
                %>
                <%=contentEnumerator.Current.Content %>
            </div>
        </div>
    </div>
    <%
            }
        }
    }
    %>
</asp:Content>
