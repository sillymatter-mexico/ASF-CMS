<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="asf.cms.view.auditReport.List" MasterPageFile="~/view/master/Admin.Master" EnableViewState="false" %>

<asp:Content ContentPlaceHolderID="head" ID="h" runat="server">

    <link rel="stylesheet" type="text/css" href="../view/css/jqthemes/smoothness/jquery-ui-1.8.21.custom.css">
    <%--<link rel="stylesheet" type="text/css" media="screen" href="../view/js/jquery/grid/css/ui.jqgrid.css">--%>

    <link rel="stylesheet" type="text/css" media="screen" href="../view/js/jquery/grid_4_15_5/css/ui.jqgrid.css" />


    <script src="../view/js/jquery/jquery-ui.js" type="text/javascript"></script>
    <script src="../view/js/jquery/jquery.form.js" type="text/javascript"></script>

    <%--<script src="../view/js/jquery/grid2/i18n/grid.locale-es.js" type="text/javascript"></script>--%>
    <%--<script src="../view/js/jquery/grid2/jquery.jqgrid.min.js" type="text/javascript"></script>--%>

    <script type="text/javascript" src="../view/js/jquery/grid_4_15_5/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../view/js/jquery/grid_4_15_5/jquery.jqgrid.min.js"></script>

    <script src="../view/js/jquery/jquery.alphanumeric.pack.js" type="text/javascript"></script>

    <script type="text/javascript" src="../view/js/auditReportList.js"></script>
    <script type="text/javascript" src="../view/js/addPub.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="contenido" ID="cont"  runat="server">
    <div class="contenido_b"> 
    
		<h2>Informes de Auditoria</h2>
		<center><p><a href="javascript:void(0)" id="btnNewReport" style="border:0;text-decoration:none">
		<img  style="border:0;text-decoration:none;" src="../view/img/add.png" alt"+" /></a> Agregar Informe de Auditoria</p></center>
        <table id="reportList"  class="scroll" cellpadding="0" cellspacing="0"></table>
	</div>
	<div id="newReportPopUp" style="font-size:12px !important"></div>

</asp:Content>
