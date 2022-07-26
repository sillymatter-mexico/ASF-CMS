<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FriendlyList.aspx.cs" Inherits="asf.cms.view.recuperacion.FriendlyList" %>
<script language="javascript" type="text/javascript" src="../view/js/listRecuperacion.js"/></script>

<div id="lista">
    <h2>Recuperaciones</h2>
    <%
        System.Collections.Generic.List<asf.cms.model.RecuperacionVO> rlist = (System.Collections.Generic.List<asf.cms.model.RecuperacionVO>)GetRequestVar("Recuperaciones");
        foreach(asf.cms.model.RecuperacionVO rvo in rlist)
        {
    %>
        <a href="javascript:void(0)" onclick="showRecuperacion('<%=rvo.Id %>')"><%=rvo.Title %></a><br />
    <%} %>
</div>
<div id="item"></div>