<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="asf.cms.view.user.List"  MasterPageFile="~/view/master/Admin.Master" EnableViewState="false" %>

<asp:Content ContentPlaceHolderID="head" ID="h" runat="server">
    <script type="text/javascript" src="../view/js/admin/v3/user/UserList.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="contenido" ID="cont"  runat="server">
    <div class="contenido_b"> 
    
		<h2>Administración de Usuarios</h2>

        <div id="resultMessages" class="ui segments">
            <div id="resultMessage" class="ui segment"><i class="icon"></i><span>Mensaje</span></div>
        </div>

        <table id="userList"  class="ui celled unstackable table" style="width: 100%;"></table> 
        
        <div id="userModalPanel" class="ui large modal">
            <div class="header">Usuario</div>
            <div class="content">
                <form id="frmNewUsr" class="ui form">

                    <div class="ui error message"></div>

                    <div class="fields">
                        <div class="two wide field">
                            <label>Activo</label>
                            <div class="ui toggle checkbox">
                                <input type="checkbox" name="active" id="active" />
                            </div>
                        </div>
                        <div class="eight wide field">
                            <label>Nombre de usuario</label>
                            <input type="text" name="username" id="username" value="" />
                        </div>
                        <div class="six wide field">
                            <label>Perfil</label>
<%
    if (((asf.cms.model.UserVO)Session["User"]).Type == "ADMIN")
    {
%>
                            <div class="ui selection dropdown">
                                <input type="hidden" name="type" id="type" />
                                <i class="dropdown icon"></i>
                                <div class="default text">Seleccione el perfil</div>
                                <div class="menu">
                                    <div class="item" data-value="RECUPERACIONES">Recuperaciones</div>
                                    <div class="item" data-value="COLABORATOR">Colaborador</div>
                                    <div class="item" data-value="USER">Usuario</div>
                                    <div class="item" data-value="ADMIN">Administrador</div>
                                </div>
                            </div>
<%
    }
    else
    {
%>
                            <input type="text" name="type" id="type" readonly />
<%
    }
%>
                        </div>
                    </div>
                    <div class="fields">
                        <div class="eight wide field">
                            <label>Contraseña</label>
                            <input type="password" name="password" id="password"  value=""/>
                        </div>
                        <div class="eight wide field">
                            <label>Confirmar contraseña</label>
                            <input type="password" name="passwordConfirm" id="passwordConfirm" value="" />
                        </div>
                    </div>
                    <input type="hidden" name="requirePassword" id="requirePassword" value="" />
                    <input type="hidden" name="oldUsername" id="oldUsername" value="" />
                </form>
            </div>
            <div class="actions">
                <div class="ui primary deny icon button"><i class="cancel icon"></i>Cancelar</div>
                <div class="ui secondary approve icon button"><i class="save icon"></i>Guardar</div>
            </div>
        </div>

	</div>
</asp:Content>
