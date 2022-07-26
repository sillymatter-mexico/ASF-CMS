<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="asf.cms.view.group.List" MasterPageFile="~/view/master/Admin.Master" EnableViewState="false" %>

<%@ Import Namespace="asf.cms.model" %>
<%@ Import Namespace="System.Collections.Generic" %>

<asp:Content ContentPlaceHolderID="head" ID="h" runat="server">
    <script type="text/javascript" src="../view/js/admin/v3/group/GroupList.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="contenido" ID="cont"  runat="server">
    <div class="contenido_b"> 
        <h2>Administración de Grupos</h2>

        <div id="resultMessages" class="ui segments">
            <div id="resultMessage" class="ui segment"><i class="icon"></i><span>Mensaje</span></div>
        </div>

        <table id="groupList" class="ui unstackable table" style="width: 100%"></table>
	</div>

    <div id="groupAddModalPanel" class="ui modal">
        <div class="header">Agregar grupo</div>
        <div class="content">
            <form class="ui form">
                <div class="field">
                    <label>Nombre de grupo</label>
                    <input type="text" name="groupName" id="groupName" />
                </div>
	        </form>
        </div>
        <div class="actions">
            <div class="ui primary deny icon button"><i class="cancel icon"></i> Cerrar</div>
            <div class="ui secondary approve icon button"><i class="save icon"></i> Guardar</div>
        </div>
    </div>

    <div id="groupUsersModalPanel" class="ui modal">
        <%
            List<GroupHasUserVO> users = (List<GroupHasUserVO>)GetRequestVar("users");
        %>
        <div class="header">Usarios de grupo</div>
        <div class="content">
            <input type="hidden" name="group_id" />
            <select class="ui fluid search dropdown" multiple="" name="users">
            <%
                foreach (GroupHasUserVO user in users)
                {
            %>
                <option value="<%=user.GroupHasUserId.Username %>"><%=user.GroupHasUserId.Username %></option>
            <%
                }
            %>
            </select>
        </div>
        <div class="actions">
            <div class="ui primary deny icon button"><i class="cancel icon"></i> Cerrar</div>
            <div class="ui secondaty approve icon button"><i class="save icon"></i> Guardar</div>
        </div>
    </div>

    <div id="groupSectionsModalPanel" class="ui modal">
        <div class="header">Secciones de grupo</div>
        <div class="content">
            <input type="hidden" name="group_id" />
        </div>
        <div class="actions">
            <div class="ui primary deny icon button"><i class="cancel icon"></i> Cerrar</div>
            <div class="ui secondary approve icon button"><i class="save icon"></i> Guardar</div>
        </div>
    </div>
</asp:Content>
