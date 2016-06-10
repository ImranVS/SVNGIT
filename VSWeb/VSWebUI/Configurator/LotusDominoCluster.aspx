<%@ Page Title="VitalSigns Plus - IBM Domino Cluster" Language="C#" MasterPageFile="~/Site1.Master"AutoEventWireup="true"
    CodeBehind="LotusDominoCluster.aspx.cs" Inherits="VSWeb.LotusDominoCluster" %>
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
            <div class="header" id="servernamelbldisp" runat="server">Notes Database Replica</div>
        </td>
    </tr>
    <tr>
        <td>
            <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0"
            Width="100%" CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass"
            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" TabSpacing="0px" 
            Font-Bold="True" EnableHierarchyRecreation="False">
            <TabPages>
                <dx:TabPage Text="Scan Settings">
               
<TabImage Url="~/images/icons/information.png"></TabImage>
                    <ContentCollection>
                        <dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">
                            <table>
                                <tr>
                                    <td valign="top">
                                    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px"
                                HeaderText="Replica Attributes" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                                Width="100%">
                                <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                                <ContentPaddings Padding="2px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>
                                <HeaderStyle Height="23px">
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
                                            <tr>
                                                <td>
                                                   
                                                    <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Category:" 
                                                        CssClass="lblsmallFont">
                                                    </dx:ASPxLabel>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <dx:ASPxComboBox ID="ClusterCategoryComboBox" runat="server" > 
                                                        
                                                       
                                                        
                                                        <Items>
                                                            <dx:ListEditItem Text="Administration Server" Value="Administration Server" />
                                                            <dx:ListEditItem Text="Application" Value="Application" />
                                                            <dx:ListEditItem Text="Backup" Value="Backup" />
                                                            <dx:ListEditItem Text="Development" Value="Development" />
                                                            <dx:ListEditItem Text="Directory" Value="Directory" />
                                                            <dx:ListEditItem Text="Disaster Recovery" Value="Disaster Recovery" />
                                                            <dx:ListEditItem Text="Gateway" Value="Gateway" />
                                                            <dx:ListEditItem Text="Hub" Value="Hub" />
                                                            <dx:ListEditItem Text="iNotes" Value="iNotes" />
                                                            <dx:ListEditItem Text="Internet Cluster Manager" 
                                                                Value="Internet Cluster Manager" />
                                                            <dx:ListEditItem Text="Mail" Value="Mail" />
                                                            <dx:ListEditItem Text="Multifunction" Value="Multifunction" />
                                                            <dx:ListEditItem Text="Production" Value="Production" />
                                                            <dx:ListEditItem Text="Pass-thru" Value="Pass-thru" />
                                                            <dx:ListEditItem Text="QuickPlace" Value="QuickPlace" />
                                                            <dx:ListEditItem Text="Replication" Value="Replication" />
                                                            <dx:ListEditItem Text="Web" Value="Web" />
                                                            <dx:ListEditItem Text="Test" Value="Test" />
                                                            <dx:ListEditItem Text="Other" Value="Other" />
                                                        </Items>
                                                        
                                                       
                                                        
                                                        <LoadingPanelStyle ImageSpacing="5px">
                                                        </LoadingPanelStyle>
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                        
                                                    </dx:ASPxComboBox>
                                                </td>
                                                <td>
                                                    &nbsp;</td>
                                                <td>
                                                    &nbsp;</td>
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
                                            </HeaderStyle>
                                            <PanelCollection>
                                                <dx:PanelContent runat="server">
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
                                                                    Text="Off-Hours Scan Interval:" Wrap="False">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxTextBox ID="OHScanTextBox" runat="server" Style="margin-left: 0px" 
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
                                <tr>
                                    <td>
                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" 
                                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                            GroupBoxCaptionOffsetY="-24px" HeaderText="Alert Settings" 
                                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                                            <ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" 
                                                PaddingTop="10px" />
                                            <HeaderStyle Height="23px">
                                            </HeaderStyle>
                                            <PanelCollection>
                                                <dx:PanelContent runat="server">
                                                    <table>
                                                        <tr>
                                                                        <td>
                                                                            <div class="info">The <b>Difference Threshold</b> is the percentage difference in document count between two replicas on different servers. 
                                                                            In other words, if a database on server A has 100 documents, but the same database on server B has only 80 documents, an alert will go out if the threshold is set to 20%.</div>
                                                                        </td>
                                                                    </tr>
                                                                    
                                                        <tr>
                                                            <td>
                                                                <table>
                                                                    <tr>
                                                                        <td align="left">
                                                                            <dx:ASPxLabel ID="ASPxLabel7" runat="server" align="right" 
                                                                                CssClass="lblsmallFont" Text="Difference Threshold:" Wrap="False">
                                                                            </dx:ASPxLabel>
                                                                        </td>
                                                                        <td>
																		<%--1/28/2016 Durga Modified for VSPLUS 2499--%>
                                                                            <dx:ASPxTextBox ID="AlertTextBox" runat="server"
                                                                                Text="5" CssClass="txtsmall">
                                                                                <MaskSettings Mask="&lt;0..999&gt;" />
                                                                                <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                    SetFocusOnError="True">
                                                                                    <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                        ValidationExpression="^\d+$" />
                                                                                    <RequiredField IsRequired="True" />
                                                                                </ValidationSettings>
                                                                            </dx:ASPxTextBox>
                                                                        </td>
                                                                        <td>
                                                                            <dx:ASPxLabel ID="ASPxLabel14" runat="server" CssClass="lblsmallFont" Text="%">
                                                                            </dx:ASPxLabel>
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;</td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxRoundPanel>
                                    </td>
                                </tr>
                            </table>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Text="Servers">
                <TabImage Url="~/images/icons/dominoserver.gif"></TabImage>
                    <ContentCollection>
                        <dx:ContentControl ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">
                            
                            <table class="style16">
                                <tr>
                                    <td colspan="3">
                                        <div class="info">The <b>File Mask</b> is a wild-card text string that if found in the file name, will qualify the database for cluster analysis. 
                                            For example, putting <b>mail</b> as the file mask will select both mail\aforbes.nsf and mail2\Zbrown.nsf. 
                                            It is not case sensitive. <br /><br /> 
                                            Enter <b>.nsf</b> as the file mask to select <b>all</b> databases on the server.<br /> <br />
                                            If you want to exclude a folder or folders from scanning, enter folder name(s) 
                                            into the <b>Exclude Folder(s)</b> field using a comma as a value separator, e.g., mail, 
                                            mail1, mail2.
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel4" runat="server" 
                                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                            GroupBoxCaptionOffsetY="-24px" HeaderText="Server A" 
                                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                                            <ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" 
                                                PaddingTop="10px" />
                                            <HeaderStyle Height="23px">
                                            </HeaderStyle>
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxLabel ID="ASPxLabel8" runat="server" CssClass="lblsmallFont" 
                                                                    Text="Domino Server:" Wrap="False">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxComboBox ID="ServerAComboBox" runat="server" >
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                                        <RequiredField IsRequired="True" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxLabel ID="ASPxLabel9" runat="server" CssClass="lblsmallFont" 
                                                                    Text="File Mask:" Wrap="False">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxTextBox ID="ServerATextBox" runat="server" Width="170px">
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                                        <RequiredField IsRequired="True" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                        </tr>	
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxLabel ID="ASPxLabel15" runat="server" CssClass="lblsmallFont" 
                                                                    Text="Exclude Folder(s):" Wrap="False">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxTextBox ID="ExcludeATextBox" runat="server" Width="170px">
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxRoundPanel>
                                    </td>
                                    <td>
                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel5" runat="server" 
                                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                            GroupBoxCaptionOffsetY="-24px" HeaderText="Server B" 
                                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                                            <ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" 
                                                PaddingTop="10px" />
                                            <HeaderStyle Height="23px">
                                            </HeaderStyle>
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent41" runat="server" SupportsDisabledAttribute="True">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxLabel ID="ASPxLabel10" runat="server" CssClass="lblsmallFont" 
                                                                    Text="Domino Server:" Wrap="False">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxComboBox ID="ServerBComboBox3" runat="server"  >
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                                        <RequiredField IsRequired="True" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxLabel ID="ASPxLabel11" runat="server" CssClass="lblsmallFont" 
                                                                    Text="File Mask:" Wrap="False">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxTextBox ID="ServerBTextBox" runat="server" Width="170px">
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                                        <RequiredField IsRequired="True" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                        </tr>
													
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxLabel ID="ASPxLabel16" runat="server" CssClass="lblsmallFont" 
                                                                    Text="Exclude Folder(s):" Wrap="False">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxTextBox ID="ExcludeBTextBox" runat="server" Width="170px">
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                        </tr>
													
                                                    </table>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxRoundPanel>
                                    </td>
                                    <td>
                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel6" runat="server" 
                                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                            GroupBoxCaptionOffsetY="-24px" HeaderText="Server C" 
                                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                                            <ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" 
                                                PaddingTop="10px" />
                                            <HeaderStyle Height="23px">
                                            </HeaderStyle>
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent5" runat="server" SupportsDisabledAttribute="True">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxLabel ID="ASPxLabel12" runat="server" CssClass="lblsmallFont" 
                                                                    Text="Domino Server:" Wrap="False">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxComboBox ID="ServerCComboBox" runat="server"  AutoPostBack="true" OnSelectedIndexChanged = "ServerCComboBox_SelectedIndexChanged">
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                                    </ValidationSettings>
																	 <Items>
                                                                     <dx:ListEditItem Text="None" Value="0" />
                        
                                                                    </Items>
                                                                </dx:ASPxComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxLabel ID="ASPxLabel13" runat="server" CssClass="lblsmallFont" 
                                                                    Text="File Mask:" Wrap="False">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxTextBox ID="ServerCTextBox" runat="server" Width="170px" >
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                        </tr>
													<%--	<tr>
														<td>
														 <dx:ASPxButton ID="ResetButtonC" runat="server" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
                        CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" 
                        Text="Reset" OnClick="ResetButtonC_Click" CausesValidation="False" >
                    </dx:ASPxButton>
														</td>
														</tr>--%>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxLabel ID="ASPxLabel17" runat="server" CssClass="lblsmallFont" 
                                                                    Text="Exclude Folder(s):" Wrap="False">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxTextBox ID="ExcludeCTextBox" runat="server" Width="170px">
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxRoundPanel>
                                    </td>
                                </tr>
                            </table>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Text="Maintenance Windows">
                <TabImage Url="~/images/application_view_tile.png"/>
<TabImage Url="~/images/application_view_tile.png"></TabImage>
                    <ContentCollection>
                        <dx:ContentControl ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">
                       <%-- <dx:ASPxRoundPanel ID="MaintRoundPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                            CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Maintenance Windows"
                                            Height="50px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
                                            Width="100%">
                                            <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                                            <ContentPaddings Padding="2px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>
                                            <HeaderStyle Height="23px">
                                                <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                                <Paddings Padding="0px" PaddingLeft="2px" PaddingRight="2px" PaddingBottom="7px">
                                                </Paddings>
                                            </HeaderStyle>
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">--%>
                                                    <table width="100%">
                                                        <tr  height="0px">
                                                            <td>
                                                                
                                                                <dx:ASPxButton ID="ToggleVeiwButton" runat="server" 
                                                                    CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                                                    CssPostfix="Office2010Blue" 
                                                                    SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" 
                                                                    Text="Switch to Calendar View" Visible="False" Width="178px">
                                                                </dx:ASPxButton>
                                                               
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div id="maintDiv" class="info">Maintenance Windows are times when you do not want the server monitored. You can define maintenance windows using the Hours & Maintenance\Maintenance menu option. Use the Actions column to modify maintenance windows information.</div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxGridView ID="MaintWinListGridView" runat="server" AutoGenerateColumns="False"
                            CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" Width="100%" CssPostfix="Office2010Blue"
                            KeyFieldName="ID" OnHtmlRowCreated="MaintWinListGridView_HtmlRowCreated" OnPageSizeChanged="MaintWinListGridView_PageSizeChanged"
                                                                    OnSelectionChanged="MaintWinListGridView_SelectionChanged" 
                                                                    EnableTheming="True" Theme="Office2003Blue" Cursor="pointer" >
                            <Columns>
                                <dx:GridViewCommandColumn ShowInCustomizationForm="True" Visible="False" 
                                    VisibleIndex="0">
                                    <ClearFilterButton Visible="True">
                                    </ClearFilterButton>
                                </dx:GridViewCommandColumn>
                                <dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True" ShowInCustomizationForm="True"
                                    Visible="False" VisibleIndex="1">
                                    <EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
                                    </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="ServerID" ReadOnly="True" ShowInCustomizationForm="True"
                                    Visible="False" VisibleIndex="2">
                                    <EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
                                    </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Name" FieldName="Name" 
                                    ShowInCustomizationForm="True" VisibleIndex="3">
                                    <Settings AutoFilterCondition="Contains" />
<Settings AutoFilterCondition="Contains"></Settings>

                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss" HorizontalAlign="Left">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" >
                                    <Paddings Padding="5px" />
<Paddings Padding="5px"></Paddings>
                                    </HeaderStyle>
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>

                               
                                <dx:GridViewDataDateColumn FieldName="StartDate" ShowInCustomizationForm="True" 
                                    VisibleIndex="4">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataDateColumn FieldName="StartTime" ShowInCustomizationForm="True" 
                                    VisibleIndex="5">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                    <PropertiesDateEdit DisplayFormatString="t">
                                    </PropertiesDateEdit>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataTextColumn FieldName="Duration" ShowInCustomizationForm="True" 
                                    VisibleIndex="6">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataDateColumn FieldName="EndDate" ShowInCustomizationForm="True" 
                                    VisibleIndex="7">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataTextColumn FieldName="MaintType" ReadOnly="True" ShowInCustomizationForm="True"
                                    VisibleIndex="8">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                            </Columns>
                                                                    <settingspager PageSize="50">
                                                                        <pagesizeitemsettings visible="True">
                                                                        </pagesizeitemsettings>
                                                                    </settingspager>
                                                                    <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                                                                     <SettingsBehavior AllowSelectByRowClick="True" ColumnResizeMode="NextColumn" ProcessSelectionChangedOnServer="True" />


<Settings ShowFilterRow="True" ShowGroupPanel="True"></Settings>

                            <Styles CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" CssPostfix="Office2010Blue">
                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                </Header>
                                <AlternatingRow CssClass="GridAltRow">
                                </AlternatingRow>
                                <LoadingPanel ImageSpacing="5px">
                                </LoadingPanel>
                            </Styles>
                            <StylesPager>
                                <PageNumber ForeColor="#3E4846">
                                </PageNumber>
                                <Summary ForeColor="#1E395B">
                                </Summary>
                            </StylesPager>
                            <StylesEditors ButtonEditCellSpacing="0">
                                <ProgressBar Height="21px">
                                </ProgressBar>
                            </StylesEditors>
                        </dx:ASPxGridView>
                                                                </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                               <%-- </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxRoundPanel>--%>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Text="Alert">
                <TabImage Url="../images/icons/error.png"></TabImage>
                    <ContentCollection>
                        <dx:ContentControl ID="ContentControl4" runat="server" SupportsDisabledAttribute="True">
                            <table class="style17">
                                <tr>
                                    <td>
                                        <table class="style2">
                                            <tr>
                                                <td>
                                                    <div id="alertDiv" class="info">The list of available alerts is listed below. In order to add new alerts or configure existing alerts, please use the Alerts\Alert Definitions menu.</div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dx:ASPxGridView ID="AlertGridView" runat="server" AutoGenerateColumns="False" 
                                                        CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                                        CssPostfix="Office2010Blue" Cursor="pointer" KeyFieldName="ID" 
                                                        Theme="Office2003Blue" Width="100%" EnableTheming="True" OnPageSizeChanged="AlertGridView_PageSizeChanged">
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn Caption="Name" FieldName="Name" ReadOnly="True" 
                                                                ShowInCustomizationForm="True" VisibleIndex="0">
                                                                <Settings AutoFilterCondition="Contains" />
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Start Time" FieldName="StartTime" 
                                                                ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="1">
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Duration" FieldName="Duration" 
                                                                ShowInCustomizationForm="True" VisibleIndex="2" Width="70px">
                                                                <Settings AutoFilterCondition="Contains" />
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss" HorizontalAlign="Left">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader">
                                                                <Paddings Padding="5px" />
                                                                </HeaderStyle>
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="To" FieldName="SendTo" 
                                                                ShowInCustomizationForm="True" VisibleIndex="3">
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="cc" FieldName="CopyTo" 
                                                                ShowInCustomizationForm="True" VisibleIndex="4">
                                                                <PropertiesTextEdit DisplayFormatString="t">
                                                                </PropertiesTextEdit>
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Bcc" FieldName="BlindCopyTo" 
                                                                ShowInCustomizationForm="True" VisibleIndex="5">
                                                                <PropertiesTextEdit DisplayFormatString="d">
                                                                </PropertiesTextEdit>
                                                                <EditCellStyle CssClass="GridCss">
                                                                </EditCellStyle>
                                                                <EditFormCaptionStyle CssClass="GridCss">
                                                                </EditFormCaptionStyle>
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Event Type" FieldName="EventName" 
                                                                ShowInCustomizationForm="True" VisibleIndex="6" Width="200px">
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Day(s)" FieldName="Day" 
                                                                ShowInCustomizationForm="True" VisibleIndex="8">
                                                                <HeaderStyle CssClass="GridCssHeader" />
                                                                <CellStyle CssClass="GridCss">
                                                                </CellStyle>
                                                            </dx:GridViewDataTextColumn>
                                                        </Columns>
                                                        <SettingsBehavior ColumnResizeMode="NextColumn" />
                                                        <SettingsPager PageSize="50">
                                                            <PageSizeItemSettings Visible="True">
                                                            </PageSizeItemSettings>
                                                        </SettingsPager>
                                                        <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                                                        <Styles CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                                            CssPostfix="Office2010Blue">
                                                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                            </Header>
                                                            <AlternatingRow CssClass="GridAltRow">
                                                            </AlternatingRow>
                                                            <LoadingPanel ImageSpacing="5px">
                                                            </LoadingPanel>
                                                        </Styles>
                                                        <StylesPager>
                                                            <PageNumber ForeColor="#3E4846">
                                                            </PageNumber>
                                                            <Summary ForeColor="#1E395B">
                                                            </Summary>
                                                        </StylesPager>
                                                        <StylesEditors ButtonEditCellSpacing="0">
                                                            <ProgressBar Height="21px">
                                                            </ProgressBar>
                                                        </StylesEditors>
                                                    </dx:ASPxGridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
            </TabPages>
            <Paddings PaddingLeft="0px" />

<Paddings PaddingLeft="0px"></Paddings>

            <ContentStyle>
                <Border BorderColor="#4986A2" />
                <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px"></Border>
            </ContentStyle>
        </dx:ASPxPageControl>
        <br />
        <div id="errorDiv1" class="alert alert-danger"  visible="false" runat="server" style="display: none">Error.
        </div>
        <table>
            <tr>
                <td>
                    <dx:ASPxButton ID="formOKButton" runat="server" CssClass="sysButton"
                        Text="OK" onclick="formOKButton_Click" >
                    </dx:ASPxButton>
                </td>
                <td>
                    <dx:ASPxButton ID="FormCancelButton" runat="server" CssClass="sysButton"
                        Text="Cancel" onclick="FormCancelButton_Click" 
                        CausesValidation="False">
                    </dx:ASPxButton>
                </td>
            </tr>
			</table>
			<table>
            <tr>
                <td>
				 <div id="errorDiv" class="alert alert-danger" runat="server" style="display: none">Error.
                 </div>
                   
                </td>
				</tr>
				<tr>
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
