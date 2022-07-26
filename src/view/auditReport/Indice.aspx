<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Indice.aspx.cs" Inherits="asf.cms.view.auditReport.Indice" %>

<%@ Import Namespace="asf.cms.model" %>
<%@ Import Namespace="System.Collections.Generic" %>

    <div><ul>
<% 
    List<AuditVO> index = (List<AuditVO>)GetRequestVar("Indice");
    string actual = "";
    foreach (AuditVO a in index)
    { 
        if(!a.IndexPosition.StartsWith(actual+".")&&a.IndexPosition.IndexOf(".")!=-1)
        {    actual=a.IndexPosition.Substring(0,a.IndexPosition.IndexOf("."));
%>
            </ul>
            <div><h2>TOMO <%=actual %></h2></div>
            <ul style='list-style: none;'>
<%
        }
%>
<li><%=a.IndexPosition %> <a href='<%=a.FilePath %>'> <%=a.Title %> </a></li>
<% }%>
    </ul>
    </div>
    
