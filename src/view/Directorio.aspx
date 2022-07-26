<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="asf.cms.view.Default" MasterPageFile="~/view/master/Default.Master" EnableViewState="false" %>
<%@ Import Namespace="asf.cms.bll" %>
<%@ Import Namespace="asf.cms.controller" %>
<%@ Import Namespace="asf.cms.model" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="asf.cms.util" %>

<asp:Content ContentPlaceHolderID="HeadScriptsContent" ID="Scripts" runat="server">
    <script src="/view/js/e19/twitter.txt"></script>
    <script type="text/javascript" src="/view/js/e19/jsontwitter.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" ID="Contenido" runat="server">
  <%
      string[] informesMenuClasses = new string[6] { "three", "four", "five", "six", "seven", "eight" };
      ContentElement informesCE = GetPublicationContent(54);
      List<ContentElement> informesMenuCE = ContentElementFromMenu((asf.cms.bll.Menu)GetRequestVar("menu_informes"));
      List<ContentElement> principalMenuCE = ContentElementFromMenu((asf.cms.bll.Menu)GetRequestVar("menu_principal"), false);
      List<ContentElement> secundarioMenuCE = ContentElementFromMenu((asf.cms.bll.Menu)GetRequestVar("menu_secundario"), false);
      List<ContentElement> adminMenuCE = ContentElementFromMenu((asf.cms.bll.Menu)GetRequestVar("menu_admin"), false);
      List<ContentElement> eventos = ContentElementFromPublications((List<PublicationVO>)GetRequestVar("eventos"));
      List<ContentElement> columnas = ContentElementFromPublications((List<PublicationVO>)GetRequestVar("columnas"));
  %>
  <!-- Carrusel -->
  <div id="banner1" class="fondo2">

    <div id="jssor_1" style="position: relative; margin: 0 auto; top: 0px; left: 0px; width: 1150px; height: 380px; overflow: hidden; visibility: hidden;">
      <!-- Loading Screen -->
      <div data-u="loading" class="jssorl-009-spin" style="position: absolute; top: 0px; left: 0px; width: 100%; height: 100%; text-align: center; background-color: rgba(0,0,0,0.7);">
        <img style="margin-top: -19px; position: relative; top: 50%; width: 38px; height: 38px;" src="img/spin.svg" />
      </div>
      <div data-u="slides" style="cursor: default; position: relative; top: 0px; left: 0px; width: 1150px; height: 380px; overflow: hidden;">
        <% foreach (Especiales esp in (List<Especiales>)GetRequestVar("bannerSuperior")) { Response.Write(esp.Content); } %>
      </div>
      <!-- Bullet Navigator -->
      <div data-u="navigator" class="jssorb051" style="position: absolute; bottom: 12px; right: 12px;" data-autocenter="1" data-scale="0.5" data-scale-bottom="0.75">
        <div data-u="prototype" class="i" style="width: 16px; height: 16px;">
          <svg viewbox="0 0 16000 16000" style="position: absolute; top: 0; left: 0; width: 100%; height: 100%;">
            <circle class="b" cx="8000" cy="8000" r="5800"></circle>
          </svg>
        </div>
      </div>
      <!-- Arrow Navigator -->
      <div data-u="arrowleft" class="jssora051" style="width: 55px; height: 55px; top: 0px; left: 25px; border-radius: 50% 50% 50% 50%; background: transparent url('/view/assets/imagenes/iconos/flecha-izquierda.png')  center center;"
        data-autocenter="2" data-scale="0.75" data-scale-left="0.75">
      </div>
      <div data-u="arrowright" class="jssora051" style="width: 55px; height: 55px; top: 0px; right: 25px; border-radius: 50% 50% 50% 50%; background: transparent url('/view/assets/imagenes/iconos/flecha-derecha.png') center center;"
        data-autocenter="2" data-scale="0.75" data-scale-right="0.75">
      </div>
    </div>
    <script type="text/javascript">jssor_1_slider_init();</script>

  </div>

  <!-- Eventos-->
  <div id="fichas" class="fondo5">
    <br>
    <h3 style="max-width: 1150px; padding-left: 15px; margin-left: auto; margin-right: auto; color: #AC9C63;">Eventos</h3>
    <div id="FichasCont" class="flex">
    <% foreach (ContentElement evento in eventos) { %>
      <div class="child evento" texto="<%=asf.cms.util.Encoder.RemoveHTML(evento.ContentNews) %>">
        <h3><%=evento.Title %></h3>
        <div class="evento_img" style="background-image: url('<%=evento.MainImage %>');"></div>
          <h5><%=evento.Published.ToShortDateString() %></h5>
        <%=evento.Content %>
      </div>
    <% } %>
    </div>
  </div>

  <!-- Informes -->
  <div id="dvInformes" class="fondo3">
    <div class="one">
      <h2><%=informesCE.Title %></h2>
      <div class="gold"></div>
    </div>
    <div class="grid">
      <div class="two">
        <%=informesCE.Content %>
      </div>
      <%
          int counter = 0;
          foreach (ContentElement elem in informesMenuCE)
          {
      %>


<!--
      <div class="<%=informesMenuClasses[counter] %>">
        <a class="<%=informesMenuClasses[counter] %>" href="<%=elem.Link %>">
          <div>
            <img class="<%=elem.Class %>" />
          </div>
          <%=elem.Title %>
        </a>
      </div>

-->




      <div class="<%=informesMenuClasses[counter] %>">
        <a class="hov" href="<%=elem.Link %>">
          <div>
       
<img style="width:120px;" src="/view/assets/imagenes/iconos/<%=elem.Class %>_g.png" >
<img style="width:120px;" src="/view/assets/imagenes/iconos/<%=elem.Class %>.png" >

          </div>
         <c> <%=elem.Title %> </c>
        </a>
      </div>



      <%
            counter = counter + 1 > informesMenuClasses.Length ? counter : counter + 1;
          }
      %>
    </div>
  </div>

  <!-- Menu principal (Acerca impacto)-->
  <div id="Menu1" class="fondo1">
    <div id="Menu1Cont" class="flex">
      <%
          foreach (ContentElement elem in principalMenuCE)
          {
      %>
      <div id="<%=elem.Title %>" class="child">
        <h2><%=elem.Title %></h2>
        <div class="gold">
        </div>
        <p>
          <%=elem.Content %>
        </p>
        <p style="text-align: right;">
          <a href="<%=elem.Link %>">Ver más..
            <div class="img_mas_consul" src="/view/assets/imagenes/iconos/flechaW.png"></div>
          </a>
        </p>
      </div>
      <%
          }
      %>
    </div>
  </div>

  <!-- Banners (especial 1) -->
  <div id="dvBanner" class="fondo5">
    <div id="dvBannercont">
      <div id="bannerlinks" class="slider">
        <% foreach (Especiales esp in (List<Especiales>)GetRequestVar("bannerLateral")) { Response.Write(esp.Content); } %> %>
      </div>
    </div>
  </div>

  <!-- Ligas Opinion -->
  <div id="fichasOpinion" class="fondo2">
    <br>
    <h3 style="max-width: 1150px; padding-left: 15px; margin-left: auto; margin-right: auto; color: #AC9C63;">Columna de Opinión ASF</h3>
    <div id="FichasOpinionCont">
      <div id="FichasOpiniondet" class="slider">
    <% foreach(ContentElement columna in columnas) { %>
      <div>
        <div class="op" >
          <h3><%=columna.Title %></h3>
          <div class="opinion_img" style="background-image: url('<%=columna.MainImage %>');"></div>
          <h5><%=columna.Published.ToShortDateString() %></h5>
          <%=columna.Content %>
        </div>
      </div>
    <% } %>
      </div>
    </div>
  </div>

  <!-- Twitter -->
  <div id="Dvdir" class="fondo2">
    <div id="FichasTwittsCont">
      <div id="FichasTwitts" class="flex">
      </div>
    </div>
  </div>

  <!-- Menu Secundario (Difusion)-->
  <div id="Menu1" class="fondo4">
    <div id="Menu1Cont" class="flex">
      <%
          foreach (ContentElement elem in secundarioMenuCE)
          {
      %>
      <div id="<%=elem.Title %>" class="child">
        <h2><%=elem.Title.ToUpper() %></h2>
        <div class="gold"></div>
        <div><%=elem.Content %></div>
        <p style="text-align: right;">
          <a href="<%=elem.Link %>">Ver más..
            <img class="img_mas_consul" src="/view/assets/imagenes/iconos/flechaW.png" />
          </a>
        </p>
      </div>
      <%
          }
      %>
    </div>
  </div>

  <!-- Menu Administracion -->
  <div id="Administracion" class="fondo2">
    <br />
    <h3 style="max-width: 1150px; padding-left: 15px; margin-left: auto; margin-right: auto; color: #AC9C63;">Administración</h3>
    <br />
    <div id="AdmonCont" class="flex">
      <%
          foreach (ContentElement elem in adminMenuCE)
          {
      %>
 <!--     <div class="child">
        <a class="hov" href="<%=elem.Link %>" target="_blank">
          <div class="<%=elem.Class %>"></div>
          
        </a>
      </div>

-->

      <div class="child">
         <a class="hov" href="<%=elem.Link %>">
          <div>
       
<img  src="/view/assets/imagenes/iconos/<%=elem.Class %>_g.png" >
<img  src="/view/assets/imagenes/iconos/<%=elem.Class %>.png" >

          </div>
        </a>
      </div>





      <%
          }
      %>
      <br />
    </div>
  </div>

</asp:Content>
