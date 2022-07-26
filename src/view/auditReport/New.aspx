<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="New.aspx.cs" Inherits="asf.cms.view.auditReport.New" %>

    <form id="frmNew" action="Insert" method="post">
    <table cellpadding="6" width="80%">
    <tr>
        <td>A&ntilde;o:</td>
        <td>
            <input type="text" name="year" id="year" size="5"/> 
        </td>
    </tr>   
    <tr>
        <td>Titulo:</td>
        <td><input type="text" name="title" id="title" size="45"/></td>
    </tr>
    <tr>
        <td>Permalink:</td>
        <td><input type="text" name="permalink" id="permalink" readonly="readonly" class="readonly"  size="45"/></td>
    </tr>
    </table>
</form>
