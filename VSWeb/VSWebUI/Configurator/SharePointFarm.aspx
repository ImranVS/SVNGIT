<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="SharePointFarm.aspx.cs" Inherits="VSWebUI.Configurator.SharePointFarm" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>






    
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
	
	

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript">
    	$(document).ready(function () {
    		$('.alert-success').delay(10000).fadeOut("slow", function () {
    		});
    	});
    	function OnItemClick(s, e) {
    		if (e.item.parent == s.GetRootItem())
    			e.processOnServer = false;
    	}
        </script>
    <style type="text/css">

.dxeBase
{
	font: 12px Tahoma;
}
.tab {border-collapse:collapse;}
.tab .first {border-left:1px solid #CCC;border-top:1px solid #CCC;border-right:1px solid #CCC;border-bottom:1px solid #CCC;}
.tab .second {border-left:1px solid #CCC;border-top:1px solid #CCC;border-right:1px solid #CCC;border-bottom:1px solid #CCC;}​
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table valign="top" align="right">
<tr><td >
                            
                        </td></tr>
</table>
<table width="100%">

 <tr>
        <td>
            <div class="header" id="lblServer" runat="server">Microsoft SharePoint Farms</div>
        </td>
    </tr>

        <tr>
            <td colspan="2">
            <asp:UpdatePanel ID="PanelControl2" runat="server">
                <ContentTemplate>
                  <dx:ASPxPageControl Font-Bold="True" ID="ASPxPageControlWindow" runat="server" ActiveTabIndex="0"
                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass"
                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" TabSpacing="0px" 
                    Width="100%" EnableHierarchyRecreation="False">
                    <TabPages>
                        <dx:TabPage Text="Server Attributes">
                            <TabImage Url="~/images/information.png" />
                            <TabImage Url="~/images/information.png">
                            </TabImage>
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">
	
                                    <table class="style1" width="100%">
                          
                                        <tr>
                                            <td colspan="2">
                                                <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" 
                                                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                                    GroupBoxCaptionOffsetY="-24px" HeaderText="Server Attributes" 
                                                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                                                    <ContentPaddings Padding="14px" PaddingBottom="10px" PaddingLeft="4px" 
                                                        PaddingTop="10px" />
                                                    <HeaderStyle Height="23px">
                                                    </HeaderStyle>
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                                            <table class="style1">
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel12" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Name:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="NameTextBox" runat="server" CssClass="txtsmall" 
                                                                            Width="170px" Enabled="false">
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dx:ASPxRoundPanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" 
                                                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                                    GroupBoxCaptionOffsetY="-24px" HeaderText="Tests Settings" 
                                                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                                                    <ContentPaddings Padding="14px" PaddingBottom="10px" PaddingLeft="4px" 
                                                        PaddingTop="10px" />
                                                    <HeaderStyle Height="23px">
                                                    </HeaderStyle>
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Test Application Full URL:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="TestAppTextBox" runat="server" CssClass="txtsmall" 
                                                                            Width="200px">
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                </tr>
																<tr>
																	<td>
																		<dx:ASPxCheckBox ID="LogonTest" runat="server" CheckState="Checked" Text="Log On Test">
																		</dx:ASPxCheckBox>
																	</td>
																	<td>
																		<dx:ASPxCheckBox ID="SiteCollectionCreationTest" runat="server" CheckState="Checked" Text="Site Collection Creation Test">
																		</dx:ASPxCheckBox>
																	</td>
																	<td>
																		<dx:ASPxCheckBox ID="FileUploadTest" runat="server" CheckState="Checked" Text="File Upload Test">
																		</dx:ASPxCheckBox>
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
                    </TabPages>
                    <LoadingPanelImage Url="~/App_Themes/Glass/Web/Loading.gif">
                    </LoadingPanelImage>
                    <Paddings PaddingLeft="0px" />
                    <Paddings PaddingLeft="0px"></Paddings>
                    <ContentStyle>
                        <Border BorderColor="#4986A2" />
                        <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px"></Border>
                    </ContentStyle>
              </dx:ASPxPageControl>
                </ContentTemplate>
            </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table>
                    <tr>
                        <td>
                        <div id="errorDiv" class="alert alert-danger" runat="server" style="display: none">Error.
                                        </div>
                            <dx:ASPxButton ID="FormOkButton" runat="server" Text="OK" CssClass="sysButton" onclick="FormOkButton_Click" >
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="FormCancelButton" runat="server" Text="Cancel" CssClass="sysButton" onclick="FormCancelButton_Click" >
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
</table>
</asp:Content>
