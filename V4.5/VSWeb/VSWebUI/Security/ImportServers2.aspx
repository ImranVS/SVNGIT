<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="ImportServers2.aspx.cs" Inherits="VSWebUI.Security.ImportServers2" %>
<%@ MasterType virtualpath="~/Site1.Master" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>




<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
    <style type="text/css">
        .style1
        {
            height: 37px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table class="tableWidth100Percent">
        <tr>
            <td>
            <div class="header" id="servernamelbldisp" runat="server">Import Servers</div>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" 
                    HeaderText="Step 2 - Set Monitoring Defaults" Theme="Glass" Width="700px">
                    <PanelCollection>
                    <dx:PanelContent>
                    <table align="left">
                        <tr>
                            <td>
                               <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Scan Interval:" 
                                                                                CssClass="lblsmallFont">
                                                                            </dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxTextBox ID="SrvAtrScanIntvlTextBox" runat="server" CssClass="txtsmall" 
                                    Text="8">
<MaskSettings Mask="&lt;0..9999&gt;"></MaskSettings>
                                                                                <MaskSettings Mask="&lt;0..9999&gt;" />
                                                                                <ValidationSettings ErrorDisplayMode="None">
                                                                                </ValidationSettings>
                                                                            </dx:ASPxTextBox>
                            </td>
                            <td>
                                <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="minutes" CssClass="lblsmallFont">
                                                                            </dx:ASPxLabel>
                            </td>
                            <td>
                                 <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Response Threshold:" 
                                                                                CssClass="lblsmallFont" Wrap="False">
                                                                            </dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxTextBox ID="SrvAtrResponseThTextBox" runat="server" CssClass="txtmed" 
                                    Text="50000">
                                                                                <MaskSettings Mask="&lt;0..99999&gt;" />
<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>
                                                                                <ValidationSettings ErrorDisplayMode="None">
                                                                                </ValidationSettings>
                                                                            </dx:ASPxTextBox>
                            </td>
                            <td>
                               <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="milliseconds" CssClass="lblsmallFont">
                                                                            </dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Off-Hours Scan Interval:" 
                                                                                CssClass="lblsmallFont">
                                                                            </dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxTextBox ID="SrvAtrOffScanIntvlTextBox" runat="server" 
                                    CssClass="txtsmall" Text="15">
<MaskSettings Mask="&lt;0..9999&gt;"></MaskSettings>
                                                                                <MaskSettings Mask="&lt;0..9999&gt;" />
                                                                                <ValidationSettings ErrorDisplayMode="None">
                                                                                </ValidationSettings>
                                                                            </dx:ASPxTextBox>
                            </td>
                            <td>
                                <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="minutes" CssClass="lblsmallFont">
                                                                            </dx:ASPxLabel>
                            </td>
                             <td>
                                <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Failures before Alert:" 
                                                                                CssClass="lblsmallFont">
                                                                            </dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxTextBox ID="SrvAtrFailBefAlertTextBox" runat="server" CssClass="txtsmall"
                                                                                
                                    Text="2">
                                                                                <MaskSettings ErrorText="Enter number between 1 to 100" Mask="&lt;0..9999&gt;" 
                                                                                    ShowHints="True" />
<MaskSettings Mask="&lt;0..9999&gt;" ErrorText="Enter number between 1 to 100" ShowHints="True"></MaskSettings>

                                                                                <ValidationSettings ErrorDisplayMode="None">
                                                                                    <RequiredField IsRequired="True" ErrorText="Please enter the failure threshold.  How many times can the server be down before an alert is sent?" />
<RequiredField IsRequired="True" ErrorText="Please enter the failure threshold.  How many times can the server be down before an alert is sent?"></RequiredField>
                                                                                </ValidationSettings>
                                                                            </dx:ASPxTextBox>
                            </td>
                            <td>
                                <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="consecutive failures" CssClass="lblsmallFont">
                                                                            </dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Retry Interval:" 
                                                                                CssClass="lblsmallFont">
                                                                            </dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxTextBox ID="SrvAtrRetryIntvlTextBox" runat="server" CssClass="txtsmall" 
                                    Text="2">
<MaskSettings Mask="&lt;0..9999&gt;"></MaskSettings>
                                                                                <MaskSettings Mask="&lt;0..9999&gt;" />
                                                                                <ValidationSettings ErrorDisplayMode="None">
                                                                                </ValidationSettings>
                                                                            </dx:ASPxTextBox>
                            </td>
                            <td>
                                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="minutes" CssClass="lblsmallFont">
                                                                            </dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxLabel ID="ASPxLabel36" runat="server" CssClass="lblsmallFont" 
                                    Text="Cluster Replication Delay:">
                                </dx:ASPxLabel>
                            </td>
                            <td>
                                <dx:ASPxTextBox ID="AdvClusterRepTextBox" runat="server" CssClass="txtsmall" 
                                    Text="120">
                                    <MaskSettings Mask="&lt;0..999999&gt;" />
                                    <ValidationSettings ErrorDisplayMode="None">
                                    </ValidationSettings>
                                </dx:ASPxTextBox>
                            </td>
                            <td>
                                <dx:ASPxLabel ID="ASPxLabel40" runat="server" CssClass="lblsmallFont" 
                                    Text="seconds">
                                </dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxLabel runat="server" Text="Pending Mail Threshold:" ID="ASPxLabel23" 
                                                                                CssClass="lblsmallFont">
                                                                            </dx:ASPxLabel>
                            </td>
                            <td colspan="2">
                                <dx:ASPxTextBox runat="server" CssClass="txtmed" 
                                    ID="SrvAtrPendingMailThTextBox" Text="200">
<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>
                                                                                <MaskSettings Mask="&lt;0..99999&gt;" />
                                                                                <ValidationSettings ErrorDisplayMode="None">
                                                                                </ValidationSettings>
                                                                            </dx:ASPxTextBox>
                            </td>
                            <td colspan="3">
                                <dx:ASPxCheckBox ID="SrvAtrDBHealthCheckBox" runat="server" 
                                                                                CheckState="Unchecked" CssClass="lblsmallFont" 
                                                                                
                                    Text="Scan this server for Database Health (Daily)">
                                                                            </dx:ASPxCheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                               <dx:ASPxLabel ID="ASPxLabel25" runat="server" CssClass="lblsmallFont" 
                                                                                Text="Dead Mail Threshold:">
                                                                            </dx:ASPxLabel>
                            </td>
                            <td colspan="2">
                                <dx:ASPxTextBox ID="SrvAtrDeadMailThTextBox" runat="server" CssClass="txtmed" 
                                    Text="500">
<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>
                                                                                <MaskSettings Mask="&lt;0..99999&gt;" />
                                                                                <ValidationSettings ErrorDisplayMode="None">
                                                                                </ValidationSettings>
                                                                            </dx:ASPxTextBox>
                            </td>
                            <td colspan="3">
                                <dx:ASPxCheckBox ID="AdvMonitorBESNtwrkQCheckBox" runat="server" CheckState="Unchecked"
                                                                                Text="Monitor BES Network Queue" CssClass="lblsmallFont">
                                                                            </dx:ASPxCheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxLabel ID="ASPxLabel26" runat="server" CssClass="lblsmallFont" 
                                                                                Text="Held Mail Threshold:">
                                                                            </dx:ASPxLabel>
                            </td>
                            <td colspan="2">
                                 <dx:ASPxTextBox ID="SrvAtrHeldMailThTextBox" runat="server" CssClass="txtmed" 
                                     Text="200">
<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>
                                                                                <MaskSettings Mask="&lt;0..99999&gt;" />
                                                                                <ValidationSettings ErrorDisplayMode="None">
                                                                                </ValidationSettings>
                                                                            </dx:ASPxTextBox>
                                 
                            </td>
                            <td colspan="3">
                               <dx:ASPxCheckBox ID="AdvNtwrkConCheckBox" runat="server" CheckState="Checked" 
                                                                                CssClass="lblsmallFont" 
                                    Text="Test for network connectivity" Checked="True">
                                                                            </dx:ASPxCheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                
                                <dx:ASPxLabel ID="ASPxLabel27" runat="server" CssClass="lblsmallFont" 
                                    Text="Disk Space Threshold:" Visible="False">
                                </dx:ASPxLabel>
                                
                            </td>
                            <td colspan="4">
                                <dx:ASPxTrackBar ID="AdvDiskSpaceThTrackBar" runat="server" CssFilePath="~/App_Themes/Office2010Black/{0}/styles.css"
                                                                                CssPostfix="Office2010Black" 
                                    Position="10" PositionStart="10" SpriteCssFilePath="~/App_Themes/Office2010Black/{0}/sprite.css"
                                                                                Step="1" Width="95%" 
                                    EnableViewState="false" SmallTickFrequency="5"
                                                                                ScalePosition="LeftOrTop" AutoPostBack="True" 
                                                                                
                                    OnValueChanged="AdvDiskSpaceThTrackBar_ValueChanged" 
                                    Theme="Office2010Blue" Visible="False">
                                                                            </dx:ASPxTrackBar>
                            </td>
                            <td>
                           
                                <dx:ASPxLabel ID="DiskLabel" runat="server" Height="16px" 
                                    CssClass="lblsmallFont" Visible="False">
                                </dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxLabel ID="ASPxLabel28" runat="server" CssClass="lblsmallFont" 
                                    Text="Memory Threshold:">
                                </dx:ASPxLabel>
                            </td>
                            <td colspan="4">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <dx:ASPxTrackBar ID="AdvMemoryThTrackBar" runat="server" CssFilePath="~/App_Themes/Office2010Black/{0}/styles.css"
                                                                                CssPostfix="Office2010Black" 
                                    EnableViewState="False" Position="95"
                                                                                PositionStart="95" SpriteCssFilePath="~/App_Themes/Office2010Black/{0}/sprite.css"
                                                                                Width="95%" Step="1" 
                                    ScalePosition="LeftOrTop" AutoPostBack="True" 
                                                                                
                                    OnPositionChanged="AdvMemoryThTrackBar_PositionChanged" 
                                    Theme="Office2010Blue">
                                                                                <ValueToolTipStyle>
                                                                                </ValueToolTipStyle>
                                                                            </dx:ASPxTrackBar>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <dx:ASPxLabel ID="MemLabel" runat="server" CssClass="lblsmallFont">
                                </dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxLabel ID="ASPxLabel29" runat="server" CssClass="lblsmallFont" 
                                    Text="CPU Threshold:">
                                </dx:ASPxLabel>
                            </td>
                            <td colspan="4">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <dx:ASPxTrackBar ID="AdvCPUThTrackBar" runat="server" CssFilePath="~/App_Themes/Office2010Black/{0}/styles.css"
                                                                                CssPostfix="Office2010Black" 
                                    EnableViewState="False" Position="90"
                                                                                PositionStart="90" SpriteCssFilePath="~/App_Themes/Office2010Black/{0}/sprite.css"
                                                                                Width="95%" Step="1" 
                                    ScalePosition="LeftOrTop" AutoPostBack="True" 
                                                                                
                                    OnValueChanged="AdvCPUThTrackBar_ValueChanged" Theme="Office2010Blue">
                                                                            </dx:ASPxTrackBar>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                </td>
                            <td>
                                <dx:ASPxLabel ID="CpuLabel" runat="server" CssClass="lblsmallFont">
                                </dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                               &nbsp; 
                                </td>
                            <td>
                               &nbsp; 
                                </td>
                            <td>
                               &nbsp; 
                                </td>
                        </tr>
                    </table>
                    <br />
                    <table align="left">
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" 
                    
                    Text="The following server(s) will be assigned monitoring defaults selected above:" 
                    CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td align="left">
                <dx:ASPxLabel ID="SrvLabel" runat="server" ForeColor="Black">
                </dx:ASPxLabel>
            </td>
        </tr>
                        <tr>
                            <td align="left">
                                <dx:ASPxCheckBoxList ID="SrvCheckBoxList" runat="server" RepeatColumns="5" 
                                    Visible="False">
                                </dx:ASPxCheckBoxList>
                            </td>
                        </tr>
        <tr>
            <td align="left">
            </td>
        </tr>
                        <tr>
                            <td align="left">
                                <dx:ASPxButton ID="AssignButton" runat="server" OnClick="AssignButton_Click" 
                                    Text="Next" CssClass="sysButton">
                                </dx:ASPxButton>
                            </td>
                        </tr>
    </table>
                    </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
            </td>
        </tr>
    </table>

</asp:Content>