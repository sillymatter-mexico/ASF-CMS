<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContentAddModal.ascx.cs" Inherits="asf.cms.view.ContentAddModal" %>

<div id="modalNewContent" class="ui small modal">
    <div class="header">Título de modal</div>
    <div class="content">
        <div class="ui middle aligned grid">
            <div class="row">
                <div class="two wide column"><b>Título</b></div>
                <div class="fourteen wide column">
                    <div class="ui input fluid">
                        <input id="titleNewContent" name="title" type="text" placeholder="Título">
                    </div>
                    <div id="titleNewContentMsg" class="ui pointing red basic label" style="display:none">Mensaje de error</div>
                </div>
            </div>
            <div class="row">
                <div class="two wide column"><b>Permalink</b></div>
                <div class="fourteen wide column">
                    <div class="ui input fluid">
                        <input id="permalinkNewContent" name="permalink" type="text" placeholder="Permalink" readonly="">
                    </div>
                </div>
            </div>
	    </div>
    </div>
    <div class="actions">
        <div class="ui deny primary button">Cancelar</div>
	    <div class="ui approve secondary button">Aceptar</div>
    </div>
</div>