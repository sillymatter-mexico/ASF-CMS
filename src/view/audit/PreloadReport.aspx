<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PreloadReport.aspx.cs" Inherits="asf.cms.view.audit.PreloadReport" MasterPageFile="~/view/master/Admin.Master" EnableViewState="false" %>

<asp:Content ContentPlaceHolderID="head" ID="h" runat="server">
    <script type="text/javascript" src="../view/js/preloadReport.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="contenido" ID="cont"  runat="server">
<h2>Vista previa de la carga</h2>
<div style="text-align:right"><button id="btnBack">Corregir</button>&nbsp;&nbsp;&nbsp;<button id="btnGo">Aplicar</button></div>
    <div id="preloadTabs"  >
    <input type="hidden" id="year" name="year" value="<%=(int)GetRequestVar("year") %>" />
    <ul  class="ttabs">
        <li><a href="#tabs-1">Entidades</a></li>
        <li><a href="#tabs-2">Sectores</a></li>
        <li><a href="#tabs-3">Grupos Funcionales</a></li>
        <li><a href="#tabs-4">Completas</a></li>
        <li><a href="#tabs-5">Auditorias Sin Archivo</a></li>
        <li><a href="#tabs-6">Archivos Sin Auditoria</a></li>
    </ul>
        <div id="tabs-1">
            <div>Las siguientes entidades no se encontraron en el sistema y seran insertadas.</div>
            <table id="entityList"  class="scroll" cellpadding="0" cellspacing="0"></table>
        </div>
        <div id="tabs-2">
            <div>Los siguientes sectores no se encontraron en el sistema y seran insertados.</div>
            <table id="sectorList"  class="scroll" cellpadding="0" cellspacing="0"></table>
        </div>
        <div id="tabs-3">
            <div>Los siguientes grupos funcionales no se encontraron en el sistema y seran insertados</div>
            <table id="groupList"  class="scroll" cellpadding="0" cellspacing="0"></table>
        </div>
        <div id="tabs-4">
            <div>Se encontro el archivo correspondiente a las siguientes auditorias </div>
            <table id="completeAuditList"  class="scroll" cellpadding="0" cellspacing="0"></table>

        </div>
        <div id="tabs-5">
            <div>No se encontro archivo para las siguientes auditorias </div>
            <table id="orphanAuditList"  class="scroll" cellpadding="0" cellspacing="0"></table>
        </div>
        <div id="tabs-6">
            <div>Los siguientes archivos no fueron asignados a ninguna auditoria</div>
            <table id="orphanFileList"  class="scroll" cellpadding="0" cellspacing="0"></table>
        </div>
    </div>
</asp:Content>

