<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="WebSphereProperties.aspx.cs" Inherits="VSWebUI.Configurator.WebSphereProperties" %>


<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>






    
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
	
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
  <%--  4/5/2016 Sowmya added for VSPLUS-2923--%>
    <script type="text/javascript">
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
<table width="100%">
        <tr>
            <td>
            <table style ="width:100%">
            <tr>
            <td>
                <div class="header" id="lblServer" runat="server">WebSphere Server</div>
    <asp:Label ID="lblServerId" runat="server" Font-Bold="False" Visible="False"></asp:Label>
            </td>
               <%--  4/5/2016 Sowmya added for VSPLUS-2923--%>
            <td align="right">
                           <table>
                    <tr>
                        <td align="right">
                           <dx:ASPxMenu ID="ASPxMenu1" runat="server" EnableTheming="True" HorizontalAlign="Right"
														 ShowAsToolbar="True" Theme="Moderno" onitemclick="ASPxMenu1_ItemClick">
														<ClientSideEvents ItemClick="OnItemClick" />
														<Items>
															<dx:MenuItem Name="MainItem">
																<Items>
																	<dx:MenuItem Name="ServerDetailsPage" Text="View Details in Dashboard">
																	</dx:MenuItem>
																</Items>
																<Image Url="~/images/icons/Gear.png">
																</Image>
															</dx:MenuItem>
														</Items>
													</dx:ASPxMenu>
                        </td>
                    </tr>
                </table>
                        </td>
                        </tr>
                        </table>
                        </td>  
        </tr>

        <tr>
            <td>
            <asp:UpdatePanel ID="PanelControl2" runat="server">
                <ContentTemplate>
                  <dx:ASPxPageControl Font-Bold="True" ID="ASPxPageControlWindow" runat="server" ActiveTabIndex="1"
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
                                            <td>
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
																<%--	<tr>
																		<td>
																			<dx:ASPxLabel ID="ASPxLabel9" runat="server" CssClass="lblsmallFont" 
																				Text="
																				 Name:">
																			</dx:ASPxLabel>
																		</td>
																		<td>
																			<dx:ASPxComboBox ID="ASPxComboBox2" runat="server" AutoPostBack="True" 
																				OnSelectedIndexChanged="ASPxComboBox2_SelectedIndexChanged" 
																				TextField="CellName" ValueField="CellID" Width="220px">
																				<ValidationSettings ErrorDisplayMode="Text" SetFocusOnError="True">
																					<RequiredField ErrorText="Select Cell Name" IsRequired="True" />
																				</ValidationSettings>
																			</dx:ASPxComboBox>
																		</td>
																	</tr>--%>
															
																</tr
															<tr>
																	<td>
                                                                        <dx:ASPxLabel ID="ASPxLabel9" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Cell Name:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
																	<td>
																	<dx:ASPxComboBox ID="CellnameComboBox" runat="server"  
																	  OnSelectedIndexChanged="CellTypeComboBox_SelectedIndexChanged" 
																	   ValueType="System.String" AutoPostBack="True" >
																	    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True" 
                                                                                                        ErrorText="CellName may not be empty.">
                                                                                                        <RequiredField IsRequired="True" ErrorText="" />
                                                                                                    </ValidationSettings>
																	</dx:ASPxComboBox>
																	</td>
																	<td>
                                                                        &nbsp;</td>
																		 <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel11" runat="server" CssClass="lblsmallFont" 
                                                                            Text=" Node Name:">
																			
                                                                        </dx:ASPxLabel>
																		</td>
																		<td>
																		<dx:ASPxComboBox ID="NodenameComboBox" runat="server" AutoPostBack="true"
																		  ValueType="System.String" >
																		   <ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True" 
                                                                                                        ErrorText="NodeName may not be empty.">
                                                                                                        <RequiredField IsRequired="True" ErrorText="" />
                                                                                                    </ValidationSettings>
																	</dx:ASPxComboBox>
                                                                    </td>
																	</tr>
																<tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel12" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Name:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
																	<td>
                                                                      <dx:ASPxTextBox ID="NameTextBox" runat="server" CssClass="lblsmallFont"
                                                                            Width="170px">
																			 <ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True" 
                                                                                                        ErrorText="Name may not be empty.">
                                                                                                        <RequiredField IsRequired="True" ErrorText="" />
                                                                                                    </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel36" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Description:">

                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="DescTextBox" runat="server" CssClass="txtsmall" 
                                                                            Width="170px">
																			 <ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True" 
                                                                                                        ErrorText="Description may not be empty.">
                                                                                                        <RequiredField IsRequired="True" ErrorText="" />
                                                                                                    </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        <dx:ASPxCheckBox ID="EnabledCheckBox" runat="server" CheckState="Unchecked" 
                                                                            CssClass="lblsmallFont" Text="Enabled for scanning" Wrap="False">
                                                                        </dx:ASPxCheckBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel14" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Location:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxComboBox ID="LocationComboBox" runat="server" CssClass="txtsmall" 
                                                                            Width="170px">
																			 <ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True" 
                                                                                                        ErrorText="Location may not be empty.">
                                                                                                        <RequiredField IsRequired="True" ErrorText="" />
                                                                                                    </ValidationSettings>
                                                                        </dx:ASPxComboBox>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel13" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Category:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="CategoryTextBox" runat="server" CssClass="txtsmall" 
                                                                            Width="170px">
																			<ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True" 
                                                                                                        ErrorText="Category may not be empty.">
                                                                                                        <RequiredField IsRequired="True" ErrorText="" />
                                                                                                    </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                  
                                                                      
                                                                </tr>
																<tr>
																<td>
																<dx:ASPxLabel ID="ASPxLabel6" runat="server" CssClass="lblsmallFont" 
                                                                        Text="Credentials:"  Visible="false"></dx:ASPxLabel>
																</td>
																<td>
																<dx:ASPxComboBox ID="CredentialsComboBox" runat="server" AutoPostBack="false" Visible="false" >
                                                               
                                                              <%-- <ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True" 
                                                                                                        ErrorText="Credentials may not be empty.">
                                                                                                        <RequiredField IsRequired="True" ErrorText="" />
                                                                                                    </ValidationSettings>--%>
                                                                </dx:ASPxComboBox>

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
                                                
                                                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" 
                                                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                                    GroupBoxCaptionOffsetY="-24px" HeaderText="Scan Settings" 
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
                                                                            Text="Scan Interval:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="ScanIntvlTextBox" runat="server" CssClass="txtsmall" 
                                                                            Width="40px">
                                                                            <MaskSettings Mask="&lt;0..9999&gt;" />
                                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                    ValidationExpression="^\d+$" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td valign="middle">
                                                                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" 
                                                                            Text="minutes">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel7" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Response Threshold:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="ResponseTextBox" runat="server" CssClass="txtsmall" 
                                                                            Width="40px">
                                                                            <MaskSettings Mask="&lt;0..99999&gt;" />
                                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                    ValidationExpression="^\d+$" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel34" runat="server" CssClass="lblsmallFont" 
                                                                            Height="16px" Text="milliseconds">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel5" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Retry Interval:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="RetryTextBox" runat="server" CssClass="txtsmall" 
                                                                            Width="40px">
																			<MaskSettings Mask="&lt;1..99999&gt;" />
																				<ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
																				<RequiredField IsRequired="True" />
																				<RegularExpression ErrorText="Please enter a numeric value using the numbers only (1-9)."
																					ValidationExpression="^\d+$" />
																				<RequiredField IsRequired="True"></RequiredField>
																				</ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td valign="middle">
                                                                        <dx:ASPxLabel ID="ASPxLabel30" runat="server" CssClass="lblsmallFont" 
                                                                            Text="minutes">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel35" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Failures before Alert:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="ConsFailuresBefAlertTextBox" runat="server" 
                                                                            CssClass="txtsmall"
                                                                            Width="40px">
                                                                            <MaskSettings ErrorText="Enter number between 1 to 100" Mask="&lt;0..9999&gt;" 
                                                                                ShowHints="True" />
                                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                    ValidationExpression="^\d+$" />
                                                                                <RequiredField ErrorText="Please enter the failure threshold.  How many times can the server be down before an alert is sent?" 
                                                                                    IsRequired="True" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Off-Hours Scan Interval:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="OffscanTextBox" runat="server" CssClass="txtsmall" 
                                                                            Width="40px">
                                                                            <MaskSettings Mask="&lt;0..9999&gt;" />
                                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                    ValidationExpression="^\d+$" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td class="style2">
                                                                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="lblsmallFont" 
                                                                            Text="minutes">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;</td>
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
                                                
                                                <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" Visible="false" 
                                                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                                    GroupBoxCaptionOffsetY="-24px" HeaderText="WebSphere Settings" 
                                                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                                                    <ContentPaddings Padding="14px" PaddingBottom="10px" PaddingLeft="4px" 
                                                        PaddingTop="10px" />
                                                    <HeaderStyle Height="23px">
                                                    </HeaderStyle>
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel8" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Average Thread Poll:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="ThreadPollTextbox" runat="server" CssClass="txtsmall" 
                                                                            Width="40px">
                                                                            <MaskSettings Mask="&lt;0..9999&gt;" />
                                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                    ValidationExpression="^\d+$" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                   <%-- <td valign="middle">
                                                                        <dx:ASPxLabel ID="ASPxLabel9" runat="server" CssClass="lblsmallFont" 
                                                                            Text="minutes">
                                                                        </dx:ASPxLabel>
                                                                    </td>--%>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel10" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Active Thread Count:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="ThreadCountTextBox" runat="server" CssClass="txtsmall" 
                                                                            Width="40px">
                                                                            <MaskSettings Mask="&lt;0..99999&gt;" />
                                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                    ValidationExpression="^\d+$" />
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
                                                                        <dx:ASPxTextBox ID="HeapCurrentTextBox" runat="server" CssClass="txtsmall" 
                                                                            Width="40px">
                                                                            <MaskSettings Mask="&lt;0..99999&gt;" />
                                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                    ValidationExpression="^\d+$" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <%--<td valign="middle">
                                                                        <dx:ASPxLabel ID="ASPxLabel16" runat="server" CssClass="lblsmallFont" 
                                                                            Text="minutes">
                                                                        </dx:ASPxLabel>
                                                                    </td>--%>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel17" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Maximum Heap:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="MaximunHeapTextBox" runat="server" 
                                                                            CssClass="txtsmall" 
                                                                            ToolTip="After what period of time is the device considered too slow?" 
                                                                            Width="40px">
                                                                            <MaskSettings ErrorText="Enter number between 1 to 100" Mask="&lt;0..9999&gt;" 
                                                                                ShowHints="True" />
                                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                    ValidationExpression="^\d+$" />
                                                                                <RequiredField ErrorText="Please enter the failure threshold.  How many times can the server be down before an alert is sent?" 
                                                                                    IsRequired="True" />
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
                                                                        <dx:ASPxTextBox ID="UpTimeTextBox" runat="server" CssClass="txtsmall" 
                                                                            Width="40px">
                                                                            <MaskSettings Mask="&lt;0..9999&gt;" />
                                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                    ValidationExpression="^\d+$" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <%--<td class="style2">
                                                                        <dx:ASPxLabel ID="ASPxLabel19" runat="server" CssClass="lblsmallFont" 
                                                                            Text="minutes">
                                                                        </dx:ASPxLabel>
                                                                    </td>--%>

                                                                  
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel20" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Hung Thread Count:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="HungThradTextBox" runat="server" CssClass="txtsmall" 
                                                                            Width="40px">
                                                                            <MaskSettings Mask="&lt;0..9999&gt;" />
                                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                    ValidationExpression="^\d+$" />
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
                                                                        <dx:ASPxTextBox ID="DumpGeneratorTextBox" runat="server" CssClass="txtsmall" 
                                                                            Width="40px">
                                                                            <MaskSettings Mask="&lt;0..9999&gt;" />
                                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                    ValidationExpression="^\d+$" />
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
                       <dx:TabPage Text="Disk Settings" Visible="false">
                                                    <TabImage Url="~/images/drive.png">
                                                    </TabImage>
                                                    <ContentCollection>
                                                        <dx:ContentControl ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">
                                                        <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxRoundPanel ID="DiskSettingsRoundPanel2" runat="server" Width="100%" 
                                                                    HeaderText="Disk Settings" Theme="Glass">
                                                                <PanelCollection>
                                                                                <dx:PanelContent ID="PanelContent11" runat="server" SupportsDisabledAttribute="True">
                                                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" OnUnload="UpdatePanel_Unload">
                                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="SelCriteriaRadioButtonList" />
                                            
                                                                </Triggers>
                                                        <ContentTemplate>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td valign="top">
                                                                                                 <dx:ASPxRadioButtonList ID="SelCriteriaRadioButtonList" 
                                                                                                     ClientInstanceName="diskSettingsRadioBtn" runat="server" 
                                                                                                SelectedIndex="0" 
                                                                                OnSelectedIndexChanged="SelCriteriaRadioButtonList_SelectedIndexChanged" 
                                                                                AutoPostBack="True" TextWrap="False">
                                                                                                <Items>
                                                                                                    <dx:ListEditItem Selected="True" Text="All Disks - By Percentage" Value="0" />
                                                                                                    <dx:ListEditItem Text="All Disks - By GB" Value="3" />
                                                                                                    <dx:ListEditItem Text="Selected Disks" Value="1" />
                                                                                                    <dx:ListEditItem Text="No Disk Checking" Value="2" />
                                                                                                </Items>
                                                                                            </dx:ASPxRadioButtonList>

                                                                        </td>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td valign="top">
                                                                           
                                                          
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                    <div id="infoDiskDiv" class="info" runat="server" style="display: none">The column 'Free Space Threshold' should be set to a value in GB or Percent
                                                                                    (1 to 100) of the remaining free space when an alert should be generated. The column 'Threshold Type' should be set accordingly in either 
                                                                                    percent or GB.
                                                                                     </div>
                                                                                     <dx:ASPxLabel ID="DiskGridInfo" runat="server" Text="Please select disks from the grid below:" Visible="False" CssClass="lblsmallFont">
                                                                            </dx:ASPxLabel>
                                                                                    <dx:ASPxLabel ID="DiskLabel" runat="server" Visible="True" CssClass="lblsmallFont">
                                                                                            </dx:ASPxLabel>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                    <dx:ASPxGridView ID="DiskGridView" ClientInstanceName="diskSettingsGrid" 
                                                                                            runat="server" AutoGenerateColumns="False" Cursor="pointer" 
                                                                                    KeyFieldName="DiskName" Theme="Office2003Blue" OnPageSizeChanged="DiskGridView_PageSizeChanged"
                                                                                            OnPreRender="DiskGridView_PreRender" 
                                                                                            onrowupdating="DiskGridView_RowUpdating" Visible="False" Width="100%"
                                                              >
                                                                <Columns>
                                                                   
                                                                    <dx:GridViewDataTextColumn Caption="isSelected" FieldName="isSelected"
                                                                        VisibleIndex="12" visible=false>
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                        <Settings AutoFilterCondition="Contains"></Settings>
                                                                        <EditCellStyle CssClass="GridCss">
                                                                        </EditCellStyle>
                                                                        <EditFormCaptionStyle CssClass="GridCss" HorizontalAlign="Left">
                                                                        </EditFormCaptionStyle>
                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                        <CellStyle CssClass="GridCss">
                                                                        </CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Disk Name" FieldName="DiskName" ReadOnly=true
                                                                        VisibleIndex="4" Width="100px">
                                                                        <Settings AutoFilterCondition="Contains" />
                                                                        <Settings AutoFilterCondition="Contains"></Settings>
                                                                        <EditCellStyle CssClass="GridCss">
                                                                        </EditCellStyle>
                                                                        <EditFormCaptionStyle CssClass="GridCss" HorizontalAlign="Left">
                                                                        </EditFormCaptionStyle>
                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                        <CellStyle CssClass="GridCss">
                                                                        </CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Free Space Threshold" 
                                                                        FieldName="Threshold" VisibleIndex="9">
                                                                        <PropertiesTextEdit DisplayFormatString="g" Width="50px"><ValidationSettings><RegularExpression ValidationExpression="(^(100(?:\.0{1,2})?))|(?!^0*$)(?!^0*\.0*$)^\d{1,2}(\.\d{1,2})?$" /></ValidationSettings></PropertiesTextEdit>
                                                                        <DataItemTemplate>
                                                                            <dx:ASPxTextBox ID="txtFreeSpaceThresholdValue" runat="server" Width="150px" Value='<%# Eval("Threshold") %>'>
                                                                            </dx:ASPxTextBox>
                                                                        </DataItemTemplate>
                                                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                        <CellStyle CssClass="GridCss2">
                                                                        </CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataComboBoxColumn Caption="Threshold Type (% or GB)" 
                                                                        FieldName="ThresholdType" VisibleIndex="11">
                                                                        <PropertiesComboBox Width="50px"></PropertiesComboBox>
                                                                        <DataItemTemplate>
                                                                            <dx:ASPxComboBox ID="txtFreeSpaceThresholdType" runat="server" TextField="ThresholdType" 
                                                                                Value='<%# Eval("ThresholdType") %>' ValueField="ThresholdType" ValueType="System.String">
                                                                                <Items>
                                                                                    <dx:ListEditItem Text="Percent" Value="Percent" />
                                                                                    <dx:ListEditItem Text="GB" Value="GB" />
                                                                                </Items>
                                                                            </dx:ASPxComboBox>
                                                                        </DataItemTemplate>
                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                        <CellStyle CssClass="GridCss">
                                                                        </CellStyle>
                                                                    </dx:GridViewDataComboBoxColumn>
                                                                    <dx:GridViewCommandColumn Caption="Select"
                                                                        Visible="true" VisibleIndex="3" ShowSelectCheckbox="True">
                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                        <CellStyle CssClass="GridCss1">
                                                                        </CellStyle>
                                                                    </dx:GridViewCommandColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Disk Size" FieldName="DiskSize" 
                                                                        VisibleIndex="5">
                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                        <CellStyle CssClass="GridCss2">
                                                                        </CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Disk Free Space" FieldName="DiskFree" 
                                                                        VisibleIndex="6">
                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                        <CellStyle CssClass="GridCss2">
                                                                        </CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Percent Free" FieldName="PercentFree" 
                                                                        VisibleIndex="7">
                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                        <CellStyle CssClass="GridCss2">
                                                                        </CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                </Columns>
                                                                <SettingsPager PageSize="50" SEOFriendly="Enabled" Mode="ShowAllRecords">
                                                                </SettingsPager>
                                                                                        <SettingsEditing Mode="Inline">
                                                                                        </SettingsEditing>
                                                               
                                                                <Images SpriteCssFilePath="~/App_Themes/Office2010Silver/{0}/sprite.css">
                                                                    <LoadingPanelOnStatusBar Url="~/App_Themes/Office2010Silver/GridView/Loading.gif">
                                                                    </LoadingPanelOnStatusBar>
                                                                    <LoadingPanel Url="~/App_Themes/Office2010Silver/GridView/Loading.gif">
                                                                    </LoadingPanel>
                                                                </Images>
                                                                <ImagesFilterControl>
                                                                    <LoadingPanel Url="~/App_Themes/Office2010Silver/GridView/Loading.gif">
                                                                    </LoadingPanel>
                                                                </ImagesFilterControl>
                                                                <Styles>
                                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                    </Header>
                                                                    <GroupRow Font-Bold="True">
                                                                    </GroupRow>
                                                                    <AlternatingRow CssClass="GridAltRow" Enabled="True">
                                                                    </AlternatingRow>
                                                                    <LoadingPanel ImageSpacing="5px">
                                                                    </LoadingPanel>
                                                                </Styles>
                                                                <StylesEditors ButtonEditCellSpacing="0">
                                                                    <ProgressBar Height="21px">
                                                                    </ProgressBar>
                                                                </StylesEditors>
                                                            </dx:ASPxGridView>
                                                                                        <dx:ASPxTrackBar ID="AdvDiskSpaceThTrackBar" runat="server" AutoPostBack="True" 
                                                                                                CssFilePath="~/App_Themes/Office2010Black/{0}/styles.css" 
                                                                                                CssPostfix="Office2010Black" EnableViewState="false" 
                                                                                                OnValueChanged="AdvDiskSpaceThTrackBar_ValueChanged" Position="10" 
                                                                                                PositionStart="10" ScalePosition="LeftOrTop" SmallTickFrequency="5" 
                                                                                                
                                                                                SpriteCssFilePath="~/App_Themes/Office2010Black/{0}/sprite.css" Step="1" 
                                                                                                Width="100%" Theme="Office2010Blue">
                                                                                            </dx:ASPxTrackBar>
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <dx:ASPxLabel ID="GBTitle" runat="server" Text="Current threshold:" CssClass="lblsmallFont" 
                                                                                                        Visible="false">
                                                                                                        </dx:ASPxLabel>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <dx:ASPxTextBox ID="GBTextBox" runat="server" Width="170px" Visible="False">
                                                                                                        </dx:ASPxTextBox>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <dx:ASPxLabel ID="GBLabel" runat="server" Text="GB free space" CssClass="lblsmallFont" 
                                                                                                            Visible="False">
                                                                                                        </dx:ASPxLabel>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="3">
                                                                                                        &nbsp;
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                            <asp:Label ID="Label4" runat="server" ForeColor="#0033CC" CssClass="lblsmallFont" 
                                                                                                
                                                                                
                                                                                            Text="&nbsp;&lt;b&gt;Disk Space &lt;/b&gt;alerts will trigger if any of the drives on the Windows server fall below the threshold."></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table> 
                                                                                
                                                                        </td>   
                                                                    </tr>
                                                                </table>
                                                                </ContentTemplate>
                                                                
                                                            </asp:UpdatePanel>
                                                                          </dx:PanelContent>
                                                                        </PanelCollection>                 

                                                                </dx:ASPxRoundPanel>
                                                            </td>
                                                        </tr>
                                                        </table>
                                                        
                                                        </dx:ContentControl>
                                                    </ContentCollection>
                                                </dx:TabPage>
                                                <dx:TabPage Text="Advanced" Visible="false">
							<TabImage Url="~/images/package_green.png">
							</TabImage>
							<ContentCollection>
								<dx:ContentControl ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">
									<table>
										<tr>
											<td valign="top">
												<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<dx:ASPxRoundPanel ID="MemRoundPanel9" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
															CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Memory Usage Alert"
															Height="50px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="400px">
															<ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
															<HeaderStyle Height="23px">
															</HeaderStyle>
															<PanelCollection>
																<dx:PanelContent ID="PanelContent7" runat="server" SupportsDisabledAttribute="True">
																	<table cellspacing="3px" width="100%">
																		<tr>
																			<td>
																				<dx:ASPxLabel ID="MemLabel" runat="server" Visible="False">
																				</dx:ASPxLabel>
																				<dx:ASPxTrackBar ID="AdvMemoryThTrackBar" runat="server" AutoPostBack="True" CssFilePath="~/App_Themes/Office2010Black/{0}/styles.css"
																					CssPostfix="Office2010Black" EnableViewState="False" OnPositionChanged="AdvMemoryThTrackBar_PositionChanged"
																					Position="95" PositionStart="95" ScalePosition="LeftOrTop" SpriteCssFilePath="~/App_Themes/Office2010Black/{0}/sprite.css"
																					Step="1" Width="95%" Theme="Office2010Blue">
																					<ValueToolTipStyle BackColor="White">
																					</ValueToolTipStyle>
																				</dx:ASPxTrackBar>
																			</td>
																		</tr>
																		<tr>
																			<td align="left">
																				<asp:Label ID="Label5" runat="server" Style="color: #0033CC" Text="&lt;b&gt;Memory Utilization&lt;/b&gt; alerts will trigger if the percentage of memory in use on the server exceeeds this threshold.  &lt;br/&gt;&lt;br/&gt; If you don't want to get memory alerts, set the threshold to zero."></asp:Label>
																			</td>
																		</tr>
																	</table>
																</dx:PanelContent>
															</PanelCollection>
														</dx:ASPxRoundPanel>
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
											<td valign="top">
												<asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<dx:ASPxRoundPanel ID="CPURoundPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
															CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="CPU Utilization Alert"
															Height="59px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="396px">
															<ContentPaddings Padding="2px" PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
															<HeaderStyle Height="23px">
															</HeaderStyle>
															<PanelCollection>
																<dx:PanelContent ID="PanelContent8" runat="server" SupportsDisabledAttribute="True">
																	<table cellspacing="3px" width="100%">
																		<tr>
																			<td>
																				<dx:ASPxLabel ID="CpuLabel" runat="server" Visible="False">
																				</dx:ASPxLabel>
																				<dx:ASPxTrackBar ID="AdvCPUThTrackBar" runat="server" AutoPostBack="True" CssFilePath="~/App_Themes/Office2010Black/{0}/styles.css"
																					CssPostfix="Office2010Black" EnableViewState="False" OnValueChanged="AdvCPUThTrackBar_ValueChanged"
																					Position="90" PositionStart="90" ScalePosition="LeftOrTop" SpriteCssFilePath="~/App_Themes/Office2010Black/{0}/sprite.css"
																					Step="1" Width="95%" Theme="Office2010Blue">
																				</dx:ASPxTrackBar>
																			</td>
																		</tr>
																		<tr>
																			<td align="left">
																				<asp:Label ID="Label6" runat="server" Style="color: #0033CC" Text="&lt;b&gt;CPU Utilization&lt;/b&gt; alerts will trigger if the CPU utilization rate exceeds this threshold. &lt;br/&gt;&lt;br/&gt;  If you don't want to get CPU alerts, set the threshold to zero. "></asp:Label>
																			</td>
																		</tr>
																	</table>
																</dx:PanelContent>
															</PanelCollection>
														</dx:ASPxRoundPanel>
													</ContentTemplate>
												</asp:UpdatePanel>
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
            <td>
            <div id="errorDiv" class="alert alert-danger" runat="server" style="display: none">Error.
            <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                                        </div>
                <table>
                    <tr>
                        <td>
                        
                            <dx:ASPxButton ID="FormOkButton" runat="server" Text="OK" CssClass="sysButton" onclick="FormOkButton_Click" >
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="FormCancelButton" runat="server" Text="Cancel" CssClass="sysButton" onclick="FormCancelButton_Click"  CausesValidation ="false">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
</table>
</asp:Content>
