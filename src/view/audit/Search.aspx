<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="asf.cms.view.audit.Search" EnableViewState="false" %>

<link rel="stylesheet" href="../view/js/jquery/jquery-autocomplete/jquery.autocomplete.css" type="text/css">
<style>
    body 
    {
    	font-family:Verdana;
        font-size:11px;
    }
    .grupoFuncional
    {
    	display:none;
    }
</style>
    <script src="../view/js/jquery/jquery.js" type="text/javascript"></script>
    <script src="../view/js/jquery/jquery-ui.js" type="text/javascript"></script>
    <script src="../view/js/jquery/jquery.form.js" type="text/javascript"></script>
    <script src="../view/js/jquery/jquery-autocomplete/jquery.autocomplete.js" type="text/javascript"></script>
    <script src="../view/js/auditSearch.js" type="text/javascript"></script>
    
    <div class="contenido_b"> 
    <form id="frmSearch">
    <table>
    <tr>
        <td colspan="2">A&ntilde;o &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Tipo</td>
        <td class="grupoFuncional">Grupo Funcional</td>
    </tr>
    <tr>
        <td colspan="2">
        <select id="anio" name="anio">
            <option value="">---</option>
        </select>&nbsp;&nbsp;
        <select id="tipo" name="tipo">
            <option value="">---</option>
        </select>
        </td>
        <td>
        <select id="grupoFuncionalId" name="grupoFuncionalId" class="grupoFuncional">
            <option value="">---</option>
        </select>
        </td>
    </tr>
    <tr>
        <td colspan="3">Sector</td>
    </tr>
    <tr>
        <td colspan="3">
        <select id="sector" name="sector">
            <option value="">---</option>
        </select>
        </td>
    </tr>
    <tr>
        <td>Ente</td>
        <td>T&iacute;tulo</td>
        <td></td>    
    </tr>
    <tr>
        <td>
            <input type="text" id="nombreEnte" name="nombreEnte" />
            <input type="hidden" id="ente" name="ente" />
        </td>
        <td colspan="2"><input type="text" id="titulo" name="titulo" size="60" /></td>        
    </tr>
    <tr>        
    <td>N&uacute;mero</td>
    </tr>
    <tr>
    <td><input type="text" id="numero" name="numero" /></td>
    <td><input type="button" id="btnSearchAudit" name="btnSearchAudit" value="Buscar" /></td>    
    <td></td>
    </tr>
    </table>
    </form>

 
        <table id="auditList"   cellpadding="1" cellspacing="0" border="1">
        <thead>
        <tr><th>A&ntilde;o</th><th>Tipo</th><th>Sector</th><th>Entidad</th><th>Numero</th><th>Titulo</th></tr></thead>
        <tbody>
        </tbody>
        </table>	
   
</div>
