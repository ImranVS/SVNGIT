<%@ Page Title="VitalSigns Plus - IBM Domino Cluster" Language="C#" MasterPageFile="~/Site1.Master"AutoEventWireup="true"
    CodeBehind="NetworkLatencyedit.aspx.cs" Inherits="VSWeb.NetworkLatencyedit" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>








<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.alert-success').delay(10000).fadeOut("slow", function () {
            });
        });
        </script>
    <style type="text/css">
        .style16
        {
            width: 100%;
            height: 190px;
        }
        .dxeBase
{
	font: 12px Tahoma;
}

.dxeButtonEdit
{
	background-color: white;
	border: 1px solid #9F9F9F;
	width: 170px;
}
.dxeTrackBar, 
.dxeIRadioButton, 
.dxeButtonEdit, 
.dxeTextBox, 
.dxeRadioButtonList, 
.dxeCheckBoxList, 
.dxeMemo, 
.dxeListBox, 
.dxeCalendar, 
.dxeColorTable
{
	-webkit-tap-highlight-color: rgba(0,0,0,0);
}

.dxeButtonEdit .dxeEditArea
{
	background-color: white;
}

.dxeEditArea
{
	font: 12px Tahoma;
	border: 1px solid #A0A0A0;
}
.dxeEditAreaSys
{
	width: 100%;
	background-position: 0 0; /*iOS Safari*/
}

.dxeEditAreaSys, .dxeEditAreaNotStrechSys
{
	border: 0px!important;
	padding: 0px;
}
.dxeButtonEditButton,
.dxeSpinIncButton,
.dxeSpinDecButton,
.dxeSpinLargeIncButton,
.dxeSpinLargeDecButton
{
	padding: 0px 2px 0px 3px;
	<%--background: #e6e6e6 url('<%=WebResource("DevExpress.Web.ASPxEditors.Images.edtDropDownBack.gif")%>') repeat-x top;--%>
}
.dxeButtonEditButton,
.dxeCalendarButton,
.dxeSpinIncButton,
.dxeSpinDecButton,
.dxeSpinLargeIncButton,
.dxeSpinLargeDecButton
{
	vertical-align: middle;
	border: 1px solid #7f7f7f;
	cursor: pointer;
} 
.dxeTextBox,
.dxeMemo
{
	background-color: white;
	border: 1px solid #9f9f9f;
}
.dxeTextBoxSys, .dxeMemoSys
{
    border-collapse:separate!important;
}

.dxeTextBox .dxeEditArea
{
	background-color: white;
}
        .style17
        {
            width: 100%;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="90%">
    <tr>
        <td valign="top">    
            <div class="header" id="servernamelbldisp" runat="server">Network Latency</div>
        </td>
    </tr>
    <tr>
        <td>
          <table>
                                <tr>
                                    <td valign="top">
                                    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px"
                                HeaderText="Network Latency" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                                Width="100%">
                                <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                                <ContentPaddings Padding="2px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>
                                <HeaderStyle Height="23px">
                                    <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                    <Paddings Padding="0px" PaddingLeft="2px" PaddingRight="2px" PaddingBottom="7px">
                                    </Paddings>
                                </HeaderStyle>
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                                        <table>
                                            <tr>
                                                <td>
                                               
                                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Name:" CssClass="lblsmallFont">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="ClsAttNameTextBox" runat="server" Width="170px">
                                                        <ValidationSettings>
                                                            <RequiredField IsRequired="True" />
<RequiredField IsRequired="True"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    &nbsp;</td>
                                                <td>
                                                    <dx:ASPxCheckBox ID="scanEnableCheckBox" runat="server" CheckState="Unchecked" 
                                                        Text="Enabled for scanning">
                                                    </dx:ASPxCheckBox>
                                                </td>
                                            </tr>
                                           
                                        </table>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" 
                                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                            GroupBoxCaptionOffsetY="-24px" HeaderText="Scan Settings" 
                                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                                            <ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" 
                                                PaddingTop="10px" />
                                            <HeaderStyle Height="23px">
                                            <Paddings Padding="0px" PaddingBottom="7px" PaddingLeft="2px" 
                                                PaddingRight="2px" PaddingTop="0px" />
                                            </HeaderStyle>
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent1" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td width="100px">
                                                                <dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" 
                                                                    Text="Scan Interval:">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td width="70px">
                                                                <dx:ASPxTextBox ID="ScanIntervalTextBox" runat="server" 
                                                                    Style="margin-left: 0px" Width="65px">
                                                                    <MaskSettings Mask="&lt;0..99999&gt;" />
                                                                    <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                        SetFocusOnError="True">
                                                                        <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                            ValidationExpression="^\d+$" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxLabel ID="ASPxLabel5" runat="server" CssClass="lblsmallFont" 
                                                                    Height="16px" Text="minutes">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="lblsmallFont" 
                                                                    Text="Test Duration:" Wrap="False">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxTextBox ID="testduration" runat="server" Style="margin-left: 0px" 
                                                                    Width="65px">
                                                                    <MaskSettings Mask="&lt;0..99999&gt;" />
                                                                    <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                        SetFocusOnError="True">
                                                                        <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                            ValidationExpression="^\d+$" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxLabel ID="ASPxLabel6" runat="server" CssClass="lblsmallFont" 
                                                                    Text="minutes">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxRoundPanel>
                                    </td>
                                </tr>
                               
                            </table>
            
        <div id="errorDiv" class="alert alert-danger" runat="server" style="display: none">Error.
        </div>
        <table>
            <tr>
                <td>
                    <dx:ASPxButton ID="formOKButton" runat="server" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
                        CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
                        Text="OK" onclick="formOKButton_Click">
                    </dx:ASPxButton>
                </td>
                <td>
                    <dx:ASPxButton ID="FormCancelButton" runat="server" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
                        CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
                        Text="Cancel" onclick="FormCancelButton_Click" 
                        CausesValidation="False">
                    </dx:ASPxButton>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <dx:ASPxPopupControl ID="ErrorMessagePopupControl" runat="server" 
                        AllowDragging="True" ClientInstanceName="pcErrorMessage" 
                        CloseAction="CloseButton" CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                        CssPostfix="Glass" EnableAnimation="False" EnableViewState="False" 
                        HeaderText="Validation Failure" Height="150px" Modal="True" 
                        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
                        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="300px">
                        <LoadingPanelImage Url="~/App_Themes/Glass/Web/Loading.gif">
                        </LoadingPanelImage>
                        <HeaderStyle>
                        <Paddings PaddingLeft="10px" PaddingRight="6px" PaddingTop="1px" />
<Paddings PaddingLeft="10px" PaddingTop="1px" PaddingRight="6px"></Paddings>
                        </HeaderStyle>
                        <ContentCollection>
                            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                                <dx:ASPxPanel ID="Panel1" runat="server" DefaultButton="btOK">
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent6" runat="server">
                                            <div style="min-height: 70px;">
                                                <dx:ASPxLabel ID="ErrorMessageLabel" runat="server">
                                                </dx:ASPxLabel>
                                            </div>
                                            <div>
                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td align="center">
                                                            <dx:ASPxButton ID="ValidationOkButton" runat="server" AutoPostBack="False" 
                                                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                                                CssPostfix="Office2010Blue" 
                                                                SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="OK" 
                                                                Width="80px">
                                                                <ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }" />
<ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }"></ClientSideEvents>
                                                            </dx:ASPxButton>
                                                            <dx:ASPxButton ID="ValidationUpdatedButton" runat="server" AutoPostBack="False" 
                                                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                                                CssPostfix="Office2010Blue"  
                                                                SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="OK" 
                                                                Visible="False" Width="80px" OnClick="ValidationUpdatedButton_Click">
                                                                <ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }" />
<ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }"></ClientSideEvents>
                                                            </dx:ASPxButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxPanel>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>
                </td>
            </tr>
        </table>
        </td>
    </tr>
</table>
                                        
</asp:Content>
