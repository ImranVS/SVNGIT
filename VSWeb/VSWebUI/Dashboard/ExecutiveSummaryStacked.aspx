<%@ Page Title="VitalSigns Plus - Executive Summary" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="ExecutiveSummaryStacked.aspx.cs" Inherits="VSWebUI.Dashboard.ExecutiveSummaryStacked" %>
<%@ MasterType virtualpath="~/DashboardSite.Master" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="http://maxcdn.bootstrapcdn.com/bootstrap/3.1.1/css/bootstrap.min.css" rel="stylesheet">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="header" id="servernamelbldisp" runat="server">Server Status Summary</div>
    <dx:ASPxPanel ID="ASPxPanel1" runat="server" Width="100%">
    </dx:ASPxPanel>
    <div class="subheader" id="dominoDiv" runat="server" style="display: none">Domino Servers</div>
    <div class="progress" id="dominoprogressDiv" runat="server" style="display: none">
      <div class="progress-bar progress-bar-success" id="dominoDiv1" style="display:none; width: 50%" runat="server">
        <span>OK</span>
      </div>
      <div class="progress-bar progress-bar-warning" id="dominoDiv2" style="display:none; width: 30%" runat="server">
        <span>Issue</span>
      </div>
      <div class="progress-bar progress-bar-danger" id="dominoDiv3" style="display:none; width: 10%" runat="server">
        <span>Down</span>
      </div>
      <div class="progress-bar progress-bar-info" id="dominoDiv4" style="display:none; width: 10%" runat="server">
        <span>Maintenance</span>
      </div>
    </div>
    <div class="subheader" id="sametimeDiv" runat="server" style="display: none">Sametime Servers</div>
    <div class="progress" id="sametimeprogressDiv" runat="server" style="display: none">
      <div class="progress-bar progress-bar-success" id="sametimeDiv1" style="display:none; width: 60%" runat="server">
        <span>OK</span>
      </div>
      <div class="progress-bar progress-bar-warning" id="sametimeDiv2" style="display:none; width: 20%" runat="server">
        <span>Issue</span>
      </div>
      <div class="progress-bar progress-bar-danger" id="sametimeDiv3" style="display:none; width: 10%" runat="server">
        <span>Down</span>
      </div>
      <div class="progress-bar progress-bar-info" id="sametimeDiv4" style="display:none; width: 10%" runat="server">
        <span>Maintenance</span>
      </div>
    </div>
</asp:Content>
