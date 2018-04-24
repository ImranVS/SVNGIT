<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="WebSphereImportServers2.aspx.cs" Inherits="VSWebUI.Security.WebSphereImportServers2" %>
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
                            
                        </tr>
                        
                        <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel12" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Average Thread Poll:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="ThreadPollTextbox" runat="server" CssClass="txtsmall" Text="3"
                                                                            >
                                                                            <MaskSettings Mask="&lt;0..9999&gt;" />
<MaskSettings Mask="&lt;0..9999&gt;"></MaskSettings>

                                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                    ValidationExpression="^\d+$" />
<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." ValidationExpression="^\d+$"></RegularExpression>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>&nbsp;</td>
                                                                   <%-- <td valign="middle">
                                                                        <dx:ASPxLabel ID="ASPxLabel9" runat="server" CssClass="lblsmallFont" 
                                                                            Text="minutes">
                                                                        </dx:ASPxLabel>
                                                                    </td>--%>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel13" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Active Thread Count:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="ThreadCountTextBox" runat="server" CssClass="txtsmall" Text="3"
                                                                           >
                                                                            <MaskSettings Mask="&lt;0..99999&gt;" />
<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>

                                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                    ValidationExpression="^\d+$" />
<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." ValidationExpression="^\d+$"></RegularExpression>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <%--<td>
                                                                        <dx:ASPxLabel ID="ASPxLabel11" runat="server" CssClass="lblsmallFont" 
                                                                            Height="16px" Text="milliseconds">
                                                                        </dx:ASPxLabel>
                                                                    </td>--%>
                                                                </tr>
                                                                <tr>
                                                                
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel15" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Heap Current:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="HeapCurrentTextBox" runat="server" CssClass="txtsmall" Text="3"
                                                                           >
                                                                            <MaskSettings Mask="&lt;0..99999&gt;" />
<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>

                                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                    ValidationExpression="^\d+$" />
<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." ValidationExpression="^\d+$"></RegularExpression>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <%--<td valign="middle">
                                                                        <dx:ASPxLabel ID="ASPxLabel16" runat="server" CssClass="lblsmallFont" 
                                                                            Text="minutes">
                                                                        </dx:ASPxLabel>
                                                                    </td>--%>
                                                                     <td>&nbsp;</td><td>
                                                                        <dx:ASPxLabel ID="ASPxLabel17" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Maximum Heap:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="MaximunHeapTextBox" runat="server" Text="3"
                                                                            CssClass="txtsmall" 
                                                                            ToolTip="After what period of time is the device considered too slow?" 
                                                                           >
                                                                            <MaskSettings ErrorText="Enter number between 1 to 100" Mask="&lt;0..9999&gt;" 
                                                                                ShowHints="True" />
<MaskSettings Mask="&lt;0..9999&gt;" ErrorText="Enter number between 1 to 100" ShowHints="True"></MaskSettings>

                                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                    ValidationExpression="^\d+$" />
                                                                                <RequiredField ErrorText="Please enter the failure threshold.  How many times can the server be down before an alert is sent?" 
                                                                                    IsRequired="True" />
<RequiredField IsRequired="True" ErrorText="Please enter the failure threshold.  How many times can the server be down before an alert is sent?"></RequiredField>

<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." ValidationExpression="^\d+$"></RegularExpression>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel18" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Up Time:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="UpTimeTextBox" runat="server" CssClass="txtsmall" Text="3"
                                                                           >
                                                                            <MaskSettings Mask="&lt;0..9999&gt;" />
<MaskSettings Mask="&lt;0..9999&gt;"></MaskSettings>

                                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                    ValidationExpression="^\d+$" />
<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." ValidationExpression="^\d+$"></RegularExpression>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <%--<td class="style2">
                                                                        <dx:ASPxLabel ID="ASPxLabel19" runat="server" CssClass="lblsmallFont" 
                                                                            Text="minutes">
                                                                        </dx:ASPxLabel>
                                                                    </td>--%>
                                                                     <td>&nbsp;</td>
                                                                  
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel20" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Hung Thread Count:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="HungThradTextBox" runat="server" CssClass="txtsmall" Text="3"
                                                                            >
                                                                            <MaskSettings Mask="&lt;0..9999&gt;" />
<MaskSettings Mask="&lt;0..9999&gt;"></MaskSettings>

                                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                    ValidationExpression="^\d+$" />
<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." ValidationExpression="^\d+$"></RegularExpression>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <%--<td class="style2">
                                                                        <dx:ASPxLabel ID="ASPxLabel21" runat="server" CssClass="lblsmallFont" 
                                                                            Text="minutes">
                                                                        </dx:ASPxLabel>
                                                                    </td>--%>

                                                                    
                                                                </tr>
																<tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel22" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Dump Generator:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="DumpGeneratorTextBox" runat="server" CssClass="txtsmall" Text="3"
                                                                            >
                                                                            <MaskSettings Mask="&lt;0..9999&gt;" />
<MaskSettings Mask="&lt;0..9999&gt;"></MaskSettings>

                                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                    ValidationExpression="^\d+$" />
<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." ValidationExpression="^\d+$"></RegularExpression>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                   <%-- <td valign="middle">
                                                                        <dx:ASPxLabel ID="ASPxLabel23" runat="server" CssClass="lblsmallFont" 
                                                                            Text="minutes">
                                                                        </dx:ASPxLabel>
                                                                    </td>--%>
																	<td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        &nbsp;</td>
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
                                    Text="Done" Theme="Office2010Blue" Width="60px">
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