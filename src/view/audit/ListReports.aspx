<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListReports.aspx.cs" Inherits="asf.cms.view.audit.ListReports" %>
<%@ Import Namespace="asf.cms.model" %>
<%@ Import Namespace="System.Collections.Generic" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>

<body>
    <form id="form1" runat="server">
    <div>
    <%
    List<AuditReportItem> list = (List<AuditReportItem>)GetRequestVar("list");
    foreach (AuditReportItem item in list)
    {%>
    <p>
        <%=item.GlobalTitle %>
        <ul>
        <li> 
            <%=item.ReportTitle %> 
            <% if (!String.IsNullOrEmpty(item.ReportLink))
               {%>
            <a href="<%=item.ReportLink %>" target="_blank"><img src="../view/assets/icon_external.png" /></a>
            <%} %>
        </li>    
        <%if(!String.IsNullOrEmpty(item.PresentationLink))
          { %>
            <li> 
                <%=item.PresentationTitle %> <a href="<%=item.PresentationLink %>" target="_blank"><img src="../view/assets/icon_ppt.png" /></a>
            </li>    
        <% }
        if(!String.IsNullOrEmpty(item.ExecutiveResumeLink))
          { %>
            <li>    
                <%=item.ExecutiveResumeTitle %> <a href="<%=item.ExecutiveResumeLink %>" target="_blank"><img src="../view/assets/icon_pdf.png" /></a>
            </li>    
       <% }%>
        </ul>
       </p> 
   <% }%>
    
    
    </div>
    </form>
</body>
</html>
