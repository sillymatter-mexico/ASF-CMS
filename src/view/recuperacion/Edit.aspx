<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="asf.cms.view.recuperacion.Edit"    MasterPageFile="~/view/master/Admin.Master" EnableViewState="false" %>

<asp:Content ContentPlaceHolderID="head" ID="h" runat="server">
    <%
    asf.cms.model.RecuperacionVO rvo = (asf.cms.model.RecuperacionVO)GetRequestVar("Recuperacion");
    %>
    <script type="text/javascript" src="../view/js/tinymce/jquery.tinymce.js"></script>
    <script type="text/javascript" src="../view/js/tinymce/plugins/filemanager/js/mcfilemanager.js"></script>
    <script type="text/javascript" src="../view/js/jquery/jstree/_lib/jquery.js"></script>
    <script type="text/javascript" src="../view/js/jquery/jstree/_lib/jquery.hotkeys.js"></script>
    <script type="text/javascript" src="../view/js/jquery/jstree/jquery.jstree.js"></script>
    <style type="text/css">
    .visible{visibility:visible;}
    .invisible{opacity:0.4;
    filter:alpha(opacity=40);}
    </style>

    <script type="text/javascript">
        var files =<%=rvo.Files %>;
    </script>
    <script type="text/javascript" src="../view/js/editRecuperacion.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="contenido" ID="cont"  runat="server">
<%
    asf.cms.model.RecuperacionVO rvo = (asf.cms.model.RecuperacionVO)GetRequestVar("Recuperacion");
%>

<h2>Informe de recuperacion</h2>
<form id="frmRecuperaciones">
    <table cellpadding="6" width="80%">
    <tr>
    
        <td>T&iacute;tulo:</td>
        <td>
            <input type="hidden" name="id" id="id"  value="<%=rvo.Id %>"/>
            <input type="hidden" name="active" id="active" value='<%=rvo.Active.ToString() %>'/>
            <input type="hidden" name="files" id="files"/>
            <input type="hidden" name="creationDate" id="creationDate" value="<%=rvo.CreationDate.ToString() %>"/>
            <input type="text" name="title" id="title" size="40" value="<%=rvo.Title %>"/> 
            
        </td>
    </tr>   
    <tr>
        <td>Directorio del informe:</td>
        <td><input type="text" name="directoryPath" id="directoryPath" size="45" value="<%=rvo.DirectoryPath %>"/>
        <a href="javascript:void(0)" onclick="selectFolder('directoryPath')"><img src="../view/img/folder_explore.png" alt="Seleccionar" title="Seleccionar" /></a> 
               <input type="button" id="procesar" value="Procesar">
        </td>
    </tr>
    </table>
               <input type="button" id="guardar" value="Guardar Cambios">

       <div style="background-color:#FFFFD3;padding:6px;margin-top:15px;margin-bottom:15px;border:1px solid #FFCC20;position:relative">
       Para acomodar los elementos, arrastrelos a su posición deseada. Utilice la tecla <b>DEL</b> para cambiar visibilidad del elemento. Utilice tecla <b>F2</b> para cambiar el titulo
       </div>
        <div>
            <div style="float:left;width:20%">Titulo</div>
            <div style="text-align:right;float:left;width:70%"><a href="#" id="linkDocumento" target="_blank">
                <img style="margin:0;padding:0" alt="Ver archivo" title="Ver archivo" src="../view/assets/icon_external.png"/></a>
            </a></div>
            <div style="clear:both">&nbsp;</div>
            <div style="float:left;width:20%;height:500px;border:1px solid black;overflow-x:auto;overflow-y:auto" id='treesito' ></div>
            <div style="float:left;width:70%">
                <iframe style="border:1px solid black;margin-left:30px;width:100%;height:500px"  frameborder="0" src="" id="objectFile" >
                </iframe>
            </div>
        </div>
        <div style="clear:both">&nbsp;</div>
</form>        
</asp:Content>
