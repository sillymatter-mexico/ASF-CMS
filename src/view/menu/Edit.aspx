<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="asf.cms.view.menu.Edit" MasterPageFile="~/view/master/Admin.Master" EnableViewState="false" %>

<%@ Import Namespace="asf.cms.bll" %>
<%@ Import Namespace="asf.cms.controller" %>
<%@ Import Namespace="System.Collections.Generic" %>

<asp:Content ContentPlaceHolderID="head" ID="h" runat="server">
  <script type="text/javascript" src="../view/js/admin/v3/menu/MenuEdit.js"></script>
  <script type="text/javascript">
    let menuItemClasses = <% Response.Write((string)GetRequestVar("classesJSON")); %>;
  </script>
</asp:Content>

<asp:Content ContentPlaceHolderID="contenido" ID="cont" runat="server">
  <div class="contenido_b">
    <h1>Menus</h1>
    <%
        List<asf.cms.bll.Menu> menus = GetRequestVar("menus") as List<asf.cms.bll.Menu>;
        foreach (asf.cms.bll.Menu menu in menus)
        {
          string menuKeyLow = menu.Key.ToLower();
    %>

    <div id="resultMessages" class="ui segments">
        <div id="resultMessage" class="ui segment"><i class="icon"></i><span>Mensaje</span></div>
    </div>

    <div id="<%=menuKeyLow %>Segment" class="ui segment">
        <h2>Menu: <%=menuKeyLow %></h2>
        <div class="ui divider"></div>

        <div class="ui form">
            <div class="fields">
                <div id="<%=menuKeyLow %>AddSection" class="ten wide field">
                    <label>Agregar sección</label>
                </div>
                <div class="six wide field">
                    <label>Clase CSS del menu:</label>
                    <input type="text" id="<%=menuKeyLow %>CssClass" value="<%=menu.CSSClass %>" placeholder="Clase CSS del menu"/>
                </div>
            </div>
        </div>

        <div class="ui horizontal divider">Secciones</div>
            <div id="<%=menuKeyLow %>ItemsContainer">

        <%foreach (asf.cms.bll.MenuItem item in menu.Items)
                {%>
            <div class="ui medium label" style="display:block; width:max-content; margin-bottom: 2px; margin-left: 0px;">
                <i class="large down caret icon"></i>
                <i class="large up caret icon"></i>
                <span><%=item.Section.Title %></span>
                <div class="ui mini inline input">
                    <input name="itemCssClass" id="<%=menuKeyLow %>ItemCssClass" type="text" placeholder="Clase CSS del elemento" value="<%=item.Section.CssClass %>">
                </div>
                <input name="sectionId" type="hidden" value="<%=item.Section.SectionId%>">
                <i class="large close icon"></i>
            </div>
            <%} %>

            </div>
        <div class="ui hortizontal divider"></div>

        <input type="hidden" id="<%=menuKeyLow %>" value="<%=menu.Key %>" />
        <button class="ui basic button" id="<%=menuKeyLow %>Save">Guardar menu</button>
    </div>
    <% } %>

    <div id="labelTemplate" class="ui medium label" style="display:block; width:max-content; margin-bottom: 2px; margin-left: 0px;">
        <i class="large down caret icon"></i>
        <i class="large up caret icon"></i>
        <span></span>
        <div class="ui mini inline input">
            <input name="itemCssClass" type="text" placeholder="Clase CSS del elemento">
        </div>
        <input name="sectionId" type="hidden" value="">
        <i class="large close icon"></i>
    </div>
  </div>
</asp:Content>
