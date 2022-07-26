<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="New.aspx.cs" Inherits="asf.cms.view.section.New"  MasterPageFile="~/view/master/Admin.Master" EnableViewState="false" %>
<%@ Register Src="../ContentAddModal.ascx" TagName="ContentAddModal" TagPrefix="asfcms" %>

<asp:Content ContentPlaceHolderID="head" ID="h" runat="server">
    <script type="text/javascript" src="../view/js/admin/v3/section/SectionEdit.js"></script>
    <%
        string UserType = ((asf.cms.model.UserVO)Session["User"]).Type;
        if (UserType == "ADMIN")
        {
            %>
            <script type="text/javascript" src="../view/js/admin/v3/ContentAdd.js"></script>
		    <script type="text/javascript">
                var addEnable = true;
            </script>
	        <%
        } else {
            %>
            <script type="text/javascript">
                var addEnable = false;
            </script>
            <%
        }
    %>
</asp:Content>

<asp:Content ContentPlaceHolderID="contenido" ID="cont"  runat="server">
<%
    asf.cms.bll.Section s = (asf.cms.bll.Section)GetRequestVar("section");
%>
    <div class="contenido_b">  
        
		<h2>Edición de sección</h2>

        <div id="resultMessages" class="ui segments">
            <div id="resultMessage" class="ui segment"><i class="icon"></i><span>Mensaje</span></div>
        </div>

        <button id="btnSave" class="ui basic primary button <%=string.IsNullOrEmpty(GetMessage("readonly")) ? "" : "disabled" %>"><i class="save icon"></i>Guardar</button>
        <button id="btnAddSection" class="ui basic secondary button <%=string.IsNullOrEmpty(GetMessage("readonly")) ? "" : "disabled" %>""><i class="icons"><i class="book icon"><i class="corner add icon rbc-icon"></i></i></i>Agregar sección</button>
        <button id="btnAddPublication" class="ui basic secondary button <%=string.IsNullOrEmpty(GetMessage("readonly")) && !(s.IsMain && s.ParentSectionId == null) ? "" : "disabled" %>""><i class="icons"><i class="file alternate icon"><i class="corner add icon rbc-icon"></i></i></i>Agregar publicación</button>

        <h4 class="ui dividing header">Información</h4>

        <form id="frmNew" class="ui form" action="<%=GetMessage("action") %>" method="post">

            <div class="ui error message"></div>

            <div class="fields">
                <div class="one wide field">
                    <label>Principal</label>
                    <div class="ui toggle checkbox">
                        <input type="checkbox" name="isMain" id="isMain" value="1"  <%=s.IsMain?"checked":"" %> <%=GetMessage("readonly")%> tabindex="0" class="hidden" />
                    </div>
                </div>
                <div class="nine wide field">
                    <label>Titulo Español</label>
                    <input type="text" name="spanishTitle" id="spanishTitle"  value="<%=s.SpanishTitle %>" <%=GetMessage("readonly")%> />
                </div>
                <div class="six wide field">
                    <label>Permalink</label>
                    <input type="text" name="permalink" id="permalink" readonly="" value="<%=s.Permalink %>" />
                </div>
            </div>

            <div class="fields">
                <div class="eight wide field">
                    <label>Sección superior</label>
                </div>
                <div class="two wide field">
                    <label>Posición</label>
                    <input type="text" name="position" id="position"  value="<%=s.Position %>" <%=GetMessage("readonly")%> />
                </div>
                <div class="six wide field">
                    <label>Clase</label>
                     <input type="text" name="CssClass" id="CssClass"  value="<%=s.CssClass %>" <%=GetMessage("readonly")%> />
                </div>
            </div>

            <div class="fields">
                <div class="ten wide field">
                    <label>URL Destino</label>
                    <input type="text" name="redirectTo" id="redirectTo"  value="<%=s.RedirectTo %>" <%=GetMessage("readonly")%> />
                </div>
                <div class="six wide field">
                    <label>Target del link</label>
                    <select class="ui selection dropdown <%=string.IsNullOrEmpty(GetMessage("readonly")) ? "" : "disabled" %>" name="redirectOptions" id="redirectOptions">
                        <option value="" <%=string.IsNullOrEmpty(s.RedirectOptions)? "selected":"" %>>Abrir en ventana actual</option>
                        <option value="_blank" <%=s.RedirectOptions=="_blank"? "selected":"" %>>Abrir en ventana nueva (_blank)</option>
                    </select>
                </div>
            </div>

            <input type="hidden" id="sectionId" name="sectionId" value="<%=s.SectionId %>" />
            <input type="hidden" id="sectionVisitas" name="visitas" value="<%=s.Visitas%>" />

            <input type="hidden" id="newPubSectionData" value="sectionId=<%=s.SectionId %>" />
            <input type="hidden" id="newSectionData" value="parentSectionId=<%=s.SectionId%>" />

        </form>

        <input type="hidden" id="parentSectionIdOld" name="parentSectionIdOld" value="<%=s.ParentSectionId %>" />

        <h4 class="ui dividing header">Contenidos</h4>

        <div id="sectionContent" class="ui loading segment">
            <div class="ui icon header">
                <i class="huge sticky note outline icon"></i>
                    No hay contenidos en esta seccion.
            </div>
        </div>
        
    </div>

    <asfcms:ContentAddModal ID="SectionAddModal" runat="Server" />

</asp:Content>
