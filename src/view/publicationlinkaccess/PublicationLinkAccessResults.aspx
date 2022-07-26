<%@ Page Title="" Language="C#" MasterPageFile="~/view/master/Admin.Master" AutoEventWireup="true" CodeBehind="PublicationLinkAccessResults.aspx.cs" Inherits="asf.cms.view.publicationlinkaccess.PublicationLinkAccessResults" %>

<%@ Import Namespace="asf.cms.model" %>
<%@ Import Namespace="System.Collections.Generic" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contenido" runat="server">

    <%
        IList<PublicationLinkAccessResultVO> linkAccessResults = (IList<PublicationLinkAccessResultVO>)GetRequestVar("publicationLinkAccessResults");
    %>

    <div class="ui container">
		<h1>Registro de acceso a ligas</h1>

        <%
            if (linkAccessResults != null && linkAccessResults.Count > 0)
            {
            %>

        <table class="ui sortable compact celled table">
            <thead>
                <tr>
                    <th>Liga</th>
                    <th>Total de accesos</th>
                    <th>Accessos del año</th>
                    <th>Accessos del mes</th>
                    <th>Accessos del dia</th>
                </tr>
            </thead>
            <tbody>
                <% foreach (PublicationLinkAccessResultVO item in linkAccessResults)
                    { 
                    %>
                <tr>
                    <td><%=item.AccessUrl %></td>
                    <td class="collapsing center aligned"><%=item.TotalHits %></td>
                    <td class="collapsing center aligned"><%=item.YearHits %></td>
                    <td class="collapsing center aligned"><%=item.MonthHits %></td>
                    <td class="collapsing center aligned"><%=item.DayHits %></td>
                </tr>
                    <%
                    }
                %>
            </tbody>
        </table>
            <% 
            }
            else
            { 
                %>
            <div class="ui placeholder segment">
                <div class="ui icon header">
                    <i class="ban icon"></i>
                    No hay registros que cumplan los criterios.
                </div>
            </div>
                <%
                }
            %>
    </div>

</asp:Content>
