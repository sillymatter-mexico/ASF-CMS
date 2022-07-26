<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FriendlyShow.aspx.cs" Inherits="asf.cms.view.recuperacion.FriendlyShow" %>
<link rel="stylesheet" type="text/css" href="../view/js/jquery/jstree/themes/default/style.css" />
<script type="text/javascript" src="../view/js/jquery/jstree/_lib/jquery.js"></script>
<script type="text/javascript" src="../view/js/jquery/jstree/_lib/jquery.hotkeys.js"></script>
<script type="text/javascript" src="../view/js/jquery/jstree/jquery.jstree.js"></script>
<script language="javascript" src="../view/js/showRecuperacion.js"/></script>
<%
    asf.cms.model.RecuperacionVO rvo = (asf.cms.model.RecuperacionVO)GetRequestVar("Recuperacion");
%>

<script type="text/javascript">
    $(document).ready(function () {
        //var files_<%=rvo.Id%>=<%=rvo.Files %>;
        makeTree(<%=rvo.Files %>, <%=rvo.Id %>);
    });
</script>
<div style="margin:0;padding:0">
    <div style="float:left;width:35%;margin:0;padding:0"><h3><%=rvo.Title %></h3></div>
    <div style="text-align:right;float:left;width:60%;margin:0;padding:0">
<%if (String.IsNullOrEmpty(GetMessage("src")))
  { %>            
        <a href="javascript:void(0)" onclick="showList()" style="margin:0;padding:0">
            <img  alt="Regresar a la lista" style="margin:0;padding:0;" title="Regresar a la lista" src="../view/assets/previous.png"/></a> 
<%} %>
            <a href="#" id="linkDocumento_<%=rvo.Id %>" target="_blank" style="margin:0;padding:0">
                <img style="margin:0;padding:0" alt="Ver archivo" title="Ver archivo" src="../view/assets/icon_external.png"/></a>
            </div>
            <div style="clear:both;height:5px;margin:0;padding:0">&nbsp;</div>
            <div style="float:left;margin:0;padding:0;width:30%;height:400px;border:1px solid black;overflow:auto" id='treesito_<%=rvo.Id %>'></div>
            <div style="float:left;width:68%">
                <iframe style="border:1px solid black;margin:0;padding:0;margin-left:10px;width:100%;height:400px"  frameborder="0" src="" id="objectFile_<%=rvo.Id %>" >
                </iframe>
            </div>
        </div>
<div style="clear:both">&nbsp;</div>