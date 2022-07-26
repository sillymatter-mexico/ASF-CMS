<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModificationLog.aspx.cs" Inherits="asf.cms.view.log.ModificationLog" MasterPageFile="~/view/master/Admin.Master" EnableViewState="false" %>

<asp:Content ContentPlaceHolderID="head" ID="h" runat="server">
    <script type="text/javascript" src="../view/js/admin/v3/log/ModificationLog.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="contenido" ID="cont"  runat="server">
	<div class="ui container">
		<h1>Registro de modificaciones</h1>
            
        <form id="modificationSearchForm" class="ui form">
            <div class="fields">
                <div class="field six wide">
                    <label>Tipo de modificación</label>
                    <select name="searchType" class="ui simple dropdown">
                        <option value="">--TODOS--</option>
                        <option value="MODIFY">Actualizaciones</option>
                        <option value="DELETE">Eliminaciones</option>
                        <option value="RECOVER">Recuperaciones</option>
                        <option value="CREATE">Creaciones</option>
                    </select>
                </div>
                <div class="field six wide">
                    <label>Tipo de objetivo</label>
                    <select name="searchTarget" class="ui simple dropdown">
                        <option value="">--TODOS--</option>
                        <option value="PUBLICATION">Publicación</option>
                        <option value="SECTION">Sección</option>
                    </select>
                </div>
                <div class="field six wide">
                    <label>Periodo</label>
                    <select name="period" class="ui simple dropdown">
                        <option value="year">Último año</option>
                        <option value="month">Último mes</option>
                        <option value="week">Últimos siete dias</option>
                        <option value="day">Último día</option>
                    </select>
                </div>
            </div>
            <button id="filterBtn" class="ui button">
                <i class="filter icon"></i>
                Filtrar
            </button>
        </form>
        <h4 class="ui horizontal divider">
            Listado de registros
        </h4>
        <div id="modificationNoResults" class="ui placeholder segment">
            <div class="ui icon header">
                <i class="ban icon"></i>
                No hay registros que cumplan los criterios.
            </div>
        </div>
		<div id="modificationList" class="ui relaxed divided list">
			<div id="modificationItem" class="item">
				<i class="large middle aligned icons" style="float:left;">
                    <i class="icon"></i>
                    <i class="corner icon"></i>
				</i>
				<div class="content">
					<div class="header">
						2019-10-11 (12:00:20 UTC)
					</div>
                    <div class="description">
                        <span class="message"></span><i class="plus circle icon"></i>
                    </div>
                    <div class="extra">
                    </div>
				</div>
			</div>
		</div>
	</div>
</asp:Content>
