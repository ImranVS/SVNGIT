<%@ Page Title="URL Properties-URLProperties" Language="C#" MasterPageFile="~/Site1.Master"
    AutoEventWireup="true" CodeBehind="URLProperties.aspx.cs" Inherits="VSWebUI.Configurator.URLProperties" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>






	
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript">
    	$(document).ready(function () {
    		var v = document.getElementById("ContentPlaceHolder1_ASPxPageControl1_ASPxRoundPanel4_PasswordTextBox_I")
    		v.type = 'password';
    	});
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
        .style1
        {
            width: 100%;
        }
        </style>
    <script type="text/javascript">
    
        function OnNameValidation(s, e) {
            var name = e.value;
            var n = name;
            if (name == null || name == "")
                return;
            while (n.indexOf('\'') >= 0) {
                name = n.replace('\'', '');
                n = name;
            }
            while (n.indexOf('\"') >= 0) {
                name = n.replace('\"', '~');
                n = name;
            }
            while (n.indexOf(',') >= 0) {
                name = n.replace(',', ' ');
                n = name;
            }
            e.value = n;
        }
    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table width="99%">
        <tr>
            <td>
                <div class="header" id="servernamelbldisp" runat="server">URL Properties</div>
            </td>
            <td valign="top" align="right">
                            <table>
                                <tr>
                                    <td>
                                        <dx:ASPxMenu ID="ASPxMenu1" runat="server" EnableTheming="True" 
                                            HorizontalAlign="Right"  onitemclick="ASPxMenu1_ItemClick" ShowAsToolbar="True" 
                                            Theme="Moderno">
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
        <tr>
            <td colspan="2">
                <dx:ASPxPageControl Font-Bold="True" ID="ASPxPageControl1" runat="server" ActiveTabIndex="0"
                                CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                                TabSpacing="0px" Width="100%" EnableHierarchyRecreation="False">
                                <TabPages>
                                    <dx:TabPage Text="URL Properties">
                                        <TabImage Url="~/images/information.png" />
                                        <TabImage Url="~/images/information.png">
                                        </TabImage>
                                        <ContentCollection>
                                            <dx:ContentControl ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">
                                                <table class="style1">
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                                CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                                                                Width="100%" HeaderText="URL Attributes">
                                                                <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                                                                <ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>
                                                                <HeaderStyle Height="23px">
                                                                </HeaderStyle>
                                                                <PanelCollection>
                                                                    <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                                                        <table width="100%">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Name:" CssClass="lblsmallFont">
                                                                                                </dx:ASPxLabel>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <dx:ASPxTextBox ID="NameTextBox" runat="server" ClientInstanceName="NameTxtBox" 
                                                                                                    Width="170px">
                                                                                                    <ClientSideEvents Validation="OnNameValidation"></ClientSideEvents>
                                                                                                    <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="None">
                                                                                                        <RequiredField IsRequired="True" />
                                                                                                        <RequiredField IsRequired="True"></RequiredField>
                                                                                                    </ValidationSettings>
                                                                                                    <ClientSideEvents Validation="OnNameValidation" />
                                                                                                </dx:ASPxTextBox>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                &nbsp;</td>
                                                                                            <td align="left">
                                                                                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" 
                                                                                                    Text="Address:">
                                                                                                </dx:ASPxLabel>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <dx:ASPxTextBox ID="IPAddressTextBox" runat="server" Width="170px">
                                                                                                
                                                                                                </dx:ASPxTextBox>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                &nbsp;</td>
                                                                                            <td align="right">
                                                                                                <dx:ASPxCheckBox ID="EnabledCheckBox" runat="server" CheckState="Unchecked" 
                                                                                        CssClass="lblsmallFont" Text="Enabled for scanning" Wrap="False">
                                                                                        <ValidationSettings ErrorDisplayMode="None">
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxCheckBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" 
                                                                                                    Text="Category:">
                                                                                                </dx:ASPxLabel>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <dx:ASPxTextBox ID="CategoryTextBox" runat="server" Width="170px">
                                                                                                    <ClientSideEvents Validation="OnNameValidation" />
<ClientSideEvents Validation="OnNameValidation"></ClientSideEvents>

                                                                                                    <ValidationSettings ErrorDisplayMode="None" SetFocusOnError="True">
                                                                                                        <RequiredField IsRequired="True" />
<RequiredField IsRequired="True"></RequiredField>
                                                                                                    </ValidationSettings>
                                                                                                </dx:ASPxTextBox>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                &nbsp;</td>
                                                                                            <td align="left">
                                                                                                <dx:ASPxLabel ID="ASPxLabel30" runat="server" CssClass="lblsmallFont" 
                                                                                                    Text="Location:">
                                                                                                </dx:ASPxLabel>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <dx:ASPxComboBox ID="LocationComboBox" runat="server">
                                                                                                    <ValidationSettings CausesValidation="True" Display="Dynamic" 
                                                                                                        ErrorDisplayMode="ImageWithTooltip">
<RequiredField IsRequired="True" ErrorText="Select Location"></RequiredField>

                                                                                                        <ErrorImage ToolTip="Select Location">
                                                                                                        </ErrorImage>
                                                                                                        <RequiredField ErrorText="Select Location" IsRequired="True" />
                                                                                                    </ValidationSettings>
                                                                                                </dx:ASPxComboBox>
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                &nbsp;</td>
                                                                                            <td>
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
                                                            <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                                CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Scan Settings"
                                                                SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                                                                <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                                                                <ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>
                                                                <HeaderStyle Height="23px">
                                                                </HeaderStyle>
                                                                <PanelCollection>
                                                                    <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="lblsmallFont" Text="Scan Interval:"
                                                                                        Wrap="False">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="ScanTextBox" runat="server" Style="margin-top: 0px" Width="40px">
                                                                                        <MaskSettings Mask="&lt;0..99999&gt;" />
                                                                                        <MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>
                                                                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" CausesValidation="True" SetFocusOnError="True">
                                                                                            <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
                                                                                                ValidationExpression="^\d+$"></RegularExpression>
                                                                                            <RequiredField IsRequired="True"></RequiredField>
                                                                                            <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
                                                                                                ValidationExpression="^\d+$" />
                                                                                            <RequiredField IsRequired="True" />
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel6" runat="server" CssClass="lblsmallFont" Text="minutes">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel8" runat="server" CssClass="lblsmallFont" 
                                                                                        Text="Response Threshold:" Wrap="False">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="RespThrTextBox" runat="server" CssClass="txtmed">
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
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel9" runat="server" CssClass="lblsmallFont" 
                                                                                        Text="milliseconds">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel5" runat="server" CssClass="lblsmallFont" Text="Retry Interval:"
                                                                                        Wrap="False">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="RetryTextBox" runat="server" Width="40px">
                                                                                        <MaskSettings Mask="&lt;1..99999&gt;" />
                                                                                        <MaskSettings Mask="&lt;1..99999&gt;"></MaskSettings>
                                                                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" CausesValidation="True" SetFocusOnError="True">
                                                                                            <RegularExpression ErrorText="Please enter a numeric value using the numbers only (1-9)."
                                                                                                ValidationExpression="^\d+$"></RegularExpression>
                                                                                            <RequiredField IsRequired="True"></RequiredField>
                                                                                            <%--<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
                                                                                                ValidationExpression="^\d+$" />
                                                                                            <RequiredField IsRequired="True" />--%>
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel10" runat="server" CssClass="lblsmallFont" Text="minutes">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel31" runat="server" CssClass="lblsmallFont" 
                                                                                        Text="Failures before Alert:" Wrap="False">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="SrvAtrFailBefAlertTextBox" runat="server" 
                                                                                        CssClass="txtsmall" Text="2">
                                                                                        <MaskSettings ErrorText="Enter number between 1 to 100" Mask="&lt;0..9999&gt;" 
                                                                                            ShowHints="True" />
<MaskSettings Mask="&lt;0..9999&gt;" ErrorText="Enter number between 1 to 100" ShowHints="True"></MaskSettings>

                                                                                        <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                            SetFocusOnError="True">
                                                                                            <RequiredField ErrorText="Please enter the failure threshold.  How many times can the server be down before an alert is sent?" 
                                                                                                IsRequired="True" />
                                                                                            <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                                ValidationExpression="^\d+$" />
<RequiredField IsRequired="True" ErrorText="Please enter the failure threshold.  How many times can the server be down before an alert is sent?"></RequiredField>

<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." ValidationExpression="^\d+$"></RegularExpression>
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel32" runat="server" CssClass="lblsmallFont" 
                                                                                        Text="consecutive failures" Wrap="False">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel7" runat="server" CssClass="lblsmallFont" 
                                                                                        Text="Off-Hours Scan Interval:" Wrap="False">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="OffScanTextBox" runat="server" Width="40px">
                                                                                        <MaskSettings Mask="&lt;0..99999&gt;" />
<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>

                                                                                        <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                            SetFocusOnError="True">
                                                                                            <RequiredField IsRequired="True" />
                                                                                            <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                                ValidationExpression="^\d+$" />
<RequiredField IsRequired="True"></RequiredField>

<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." ValidationExpression="^\d+$"></RegularExpression>
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel11" runat="server" CssClass="lblsmallFont" 
                                                                                        Text="minutes">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    &nbsp;</td>
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
                                                            <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                                CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                                                                Width="100%" HeaderText="Optional: Search Text">
                                                                <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                                                                <ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>
                                                                <HeaderStyle Height="23px">
                                                                </HeaderStyle>
                                                                <PanelCollection>
                                                                    <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                                                                        <div class="info" id="searchtextDiv">If you enter a value into the Search Text box, then VitalSigns will try to locate this text on the URL page and an Alert will be generated based on the option you select below.</div>
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Search Text  (optional):" 
                                                                                        CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="RequiredTextBox" runat="server" Width="170px">
                                                                                        <ValidationSettings ErrorDisplayMode="None">
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
																				<td>
																					<dx:ASPxRadioButtonList ID="AlertConditionRB" runat="server" AutoPostBack="True"
																						SelectedIndex="0" 
																						RepeatDirection="Horizontal" TextWrap="False">
																						<Items>
																							<dx:ListEditItem Text="Alert When Search Text is NOT Found" Value="0" />
																							<dx:ListEditItem Text="Alert When Search Text is Found" Value="1" />
																						</Items>
																					</dx:ASPxRadioButtonList>
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
                                                            <dx:ASPxRoundPanel ID="ASPxRoundPanel4" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                                CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                                                                Width="100%" HeaderText="Optional: Username/Password">
                                                                <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                                                                <ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>
                                                                <HeaderStyle Height="23px">
                                                                </HeaderStyle>
                                                                <PanelCollection>
                                                                    <dx:PanelContent ID="PanelContent5" runat="server" SupportsDisabledAttribute="True">
                                                                        <div class="info" id="userpwdDiv">If you enter values into the Username and Password 
                                                                            field, VitalSigns will use them when navigating to the page. This is only 
                                                                            applicable to pages requiring authentication when searching for specific text in 
                                                                            the page which is only available after authentication.</div>
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxLabel ID="ASPxLabel13" runat="server" CssClass="lblsmallFont" 
                                                                                        Text="Username:">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="UserNameTextBox" runat="server" AutoCompleteType="Disabled" 
                                                                                        Width="170px">
                                                                                        <ValidationSettings ErrorDisplayMode="None">
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="margin-left: 40px">
                                                                                    <dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="Password:" CssClass="lblsmallFont">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxTextBox ID="PasswordTextBox" runat="server" Width="170px" 
                                                                                         AutoCompleteType="Disabled">
                                                                                        <ValidationSettings ErrorDisplayMode="None">
                                                                                        </ValidationSettings>
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
                                        <TabImage Url="~/images/application_view_tile.png" />
                                        <TabImage Url="~/images/application_view_tile.png">
                                        </TabImage>
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
                                                <dx:PanelContent ID="PanelContent7" runat="server" SupportsDisabledAttribute="True">--%>
                                                <table class="style1">
                                                    <tr>
                                                        <td>
                                                            <div id="infoDivMaint" class="info">Maintenance Windows are times when you do not want the server monitored. You can define maintenance windows using the Hours &amp; Maintenance\Maintenance menu option. Use the Actions column to modify maintenance windows information.
                                                            </div>
                                                            <dx:ASPxButton ID="ToggleVeiwButton" runat="server" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
                                                                CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
                                                                Text="Switch to Calendar view" Visible="False" Width="178px">
                                                            </dx:ASPxButton>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxGridView ID="MaintWinListGridView" runat="server" AutoGenerateColumns="False"
                                                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" Width="100%" CssPostfix="Office2010Blue"
                                                                KeyFieldName="ID" Cursor="pointer" EnableTheming="True" OnHtmlRowCreated="MaintWinListGridView_HtmlRowCreated"
                                                                OnSelectionChanged="MaintWinListGridView_SelectionChanged" 
                                                                Theme="Office2003Blue" OnPageSizeChanged="MaintWinListGridView_PageSizeChanged">
                                                                <Columns>
                                                                    <dx:GridViewCommandColumn ShowInCustomizationForm="True" Visible="False" VisibleIndex="0">
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
                                                                    <dx:GridViewDataTextColumn Caption="Name" FieldName="Name" ShowInCustomizationForm="True"
                                                                        VisibleIndex="3">
                                                                       <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                        <Settings AutoFilterCondition="Contains"></Settings>
                                                                        <EditCellStyle CssClass="GridCss">
                                                                        </EditCellStyle>
                                                                        <EditFormCaptionStyle CssClass="GridCss" HorizontalAlign="Left">
                                                                        </EditFormCaptionStyle>
                                                                        <HeaderStyle CssClass="GridCssHeader">
                                                                            <Paddings Padding="5px" />
                                                                            <Paddings Padding="5px"></Paddings>
                                                                        </HeaderStyle>
                                                                        <CellStyle CssClass="GridCss">
                                                                        </CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataDateColumn FieldName="StartDate" ShowInCustomizationForm="True" VisibleIndex="4">
                                                                        <EditCellStyle CssClass="GridCss">
                                                                        </EditCellStyle>
                                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                                        </EditFormCaptionStyle>
                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                        <CellStyle CssClass="GridCss">
                                                                        </CellStyle>
                                                                    </dx:GridViewDataDateColumn>
                                                                    <dx:GridViewDataDateColumn FieldName="StartTime" ShowInCustomizationForm="True" VisibleIndex="5">
                                                                        <Settings AllowAutoFilter="False" />
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
                                                                    <dx:GridViewDataTextColumn FieldName="Duration" ShowInCustomizationForm="True" VisibleIndex="6">
                                                                        <EditCellStyle CssClass="GridCss">
                                                                        </EditCellStyle>
                                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                                        </EditFormCaptionStyle>
                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                        <CellStyle CssClass="GridCss">
                                                                        </CellStyle><Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataDateColumn FieldName="EndDate" ShowInCustomizationForm="True" VisibleIndex="7">
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
                                                                        </CellStyle><Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    </dx:GridViewDataTextColumn>
                                                                </Columns>
                                                                <SettingsBehavior AllowSelectByRowClick="True" ColumnResizeMode="NextColumn" ProcessSelectionChangedOnServer="True" />
                                                                <SettingsBehavior AllowSelectByRowClick="True" ColumnResizeMode="NextColumn" ProcessSelectionChangedOnServer="True">
                                                                </SettingsBehavior>
                                                                <SettingsPager PageSize="50">
                                                                    <PageSizeItemSettings Visible="True">
                                                                    </PageSizeItemSettings>
                                                                </SettingsPager>
                                                                <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                                                                <Settings ShowFilterRow="True" ShowGroupPanel="True"></Settings>
                                                                <Images SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css">
                                                                    <LoadingPanelOnStatusBar Url="~/App_Themes/Office2010Blue/GridView/Loading.gif">
                                                                    </LoadingPanelOnStatusBar>
                                                                    <LoadingPanel Url="~/App_Themes/Office2010Blue/GridView/Loading.gif">
                                                                    </LoadingPanel>
                                                                </Images>
                                                                <ImagesFilterControl>
                                                                    <LoadingPanel Url="~/App_Themes/Office2010Blue/GridView/Loading.gif">
                                                                    </LoadingPanel>
                                                                </ImagesFilterControl>
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
                                                <%--   </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxRoundPanel>--%>
                                            </dx:ContentControl>
                                        </ContentCollection>
                                    </dx:TabPage>
                                    <dx:TabPage Text="Alert">
                                        <TabImage Url="../images/icons/error.png">
                                        </TabImage>
                                        <ContentCollection>
                                            <dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">
                                                <table class="style1">
                                                    <tr>
                                                        <td>
                                                            <div id="infoDivAlerts" class="info">The list of available alerts is listed below. In order to add new alerts or configure existing alerts, please use the Alerts\Alert Definitions menu.
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxGridView ID="AlertGridView" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
                                                                CssPostfix="Office2010Blue" Cursor="pointer" KeyFieldName="ID" Theme="Office2003Blue"
                                                                Width="100%" EnableTheming="True" OnPageSizeChanged="AlertGridView_PageSizeChanged">
                                                                <Columns>
                                                                    <dx:GridViewDataTextColumn Caption="Name" FieldName="Name" ReadOnly="True" ShowInCustomizationForm="True"
                                                                        VisibleIndex="0">
                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                        <CellStyle CssClass="GridCss">
                                                                        </CellStyle><Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Start Time" FieldName="StartTime" ReadOnly="True"
                                                                        ShowInCustomizationForm="True" VisibleIndex="1">
                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                        <CellStyle CssClass="GridCss">
                                                                        </CellStyle>
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="To" FieldName="SendTo" ShowInCustomizationForm="True"
                                                                        VisibleIndex="3">
                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                        <CellStyle CssClass="GridCss">
                                                                        </CellStyle><Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Cc" FieldName="CopyTo" ShowInCustomizationForm="True"
                                                                        VisibleIndex="4">
                                                                        <PropertiesTextEdit DisplayFormatString="t">
                                                                        </PropertiesTextEdit>
                                                                        <EditCellStyle CssClass="GridCss">
                                                                        </EditCellStyle>
                                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                                        </EditFormCaptionStyle>
                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                        <CellStyle CssClass="GridCss">
                                                                        </CellStyle><Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Bcc" FieldName="BlindCopyTo" ShowInCustomizationForm="True"
                                                                        VisibleIndex="5">
                                                                        <PropertiesTextEdit DisplayFormatString="d">
                                                                        </PropertiesTextEdit>
                                                                        <EditCellStyle CssClass="GridCss">
                                                                        </EditCellStyle>
                                                                        <EditFormCaptionStyle CssClass="GridCss">
                                                                        </EditFormCaptionStyle>
                                                                        <HeaderStyle CssClass="GridCssHeader"></HeaderStyle>
                                                                        <CellStyle CssClass="GridCss">
                                                                        </CellStyle><Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Event Type" FieldName="EventName" ShowInCustomizationForm="True"
                                                                        VisibleIndex="7">
                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                        <CellStyle CssClass="GridCss">
                                                                        </CellStyle><Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Day(s)" FieldName="Day" ShowInCustomizationForm="True"
                                                                        VisibleIndex="9">
                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                        <CellStyle CssClass="GridCss">
                                                                        </CellStyle><Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Duration" FieldName="Duration" ShowInCustomizationForm="True"
                                                                        VisibleIndex="2">
                                                                        <HeaderStyle CssClass="GridCssHeader" />
                                                                        <CellStyle CssClass="GridCss">
                                                                        </CellStyle><Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                                                    </dx:GridViewDataTextColumn>
                                                                </Columns>
                                                                <SettingsBehavior ColumnResizeMode="NextColumn" />
                                                                <SettingsBehavior ColumnResizeMode="NextColumn"></SettingsBehavior>
                                                                <SettingsPager PageSize="50">
                                                                    <PageSizeItemSettings Visible="True">
                                                                    </PageSizeItemSettings>
                                                                </SettingsPager>
                                                                <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                                                                <Settings ShowFilterRow="True" ShowGroupPanel="True"></Settings>
                                                                <Images SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css">
                                                                    <LoadingPanelOnStatusBar Url="~/App_Themes/Office2010Blue/GridView/Loading.gif">
                                                                    </LoadingPanelOnStatusBar>
                                                                    <LoadingPanel Url="~/App_Themes/Office2010Blue/GridView/Loading.gif">
                                                                    </LoadingPanel>
                                                                </Images>
                                                                <ImagesFilterControl>
                                                                    <LoadingPanel Url="~/App_Themes/Office2010Blue/GridView/Loading.gif">
                                                                    </LoadingPanel>
                                                                </ImagesFilterControl>
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
                                    <Border BorderColor="#4986A2"></Border>
                                </ContentStyle>
                            </dx:ASPxPageControl>
            </td>
        </tr>
        <tr>
                        <td colspan="2">
                            <div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">Error attempting to update the status table.
                            </div>
                            <table>
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="FormOkButton" runat="server" Text="OK" CssClass="sysButton"
                                            OnClick="FormOkButton_Click" AutoPostBack="False">
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="FormCancelButton" runat="server" Text="Cancel" CssClass="sysButton"
                                            OnClick="FormCancelButton_Click" CausesValidation="False" AutoPostBack="False">
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
     </table>
    <dx:ASPxPopupControl ID="ErrorMessagePopupControl" runat="server" AllowDragging="True"
                                ClientInstanceName="pcErrorMessage" 
        CloseAction="CloseButton" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                CssPostfix="Glass" EnableAnimation="False" 
        EnableViewState="False" HeaderText="Validation Failure"
                                Height="150px" Modal="True" 
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                
        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="300px" 
        Theme="MetropolisBlue">
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
                                                                    <dx:ASPxButton ID="ValidationOkButton" runat="server" AutoPostBack="False" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
                                                                        CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
                                                                        Text="OK" Width="80px">
                                                                        <ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }" />
                                                                        <ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }"></ClientSideEvents>
                                                                    </dx:ASPxButton>
                                                                    <dx:ASPxButton ID="ValidationUpdatedButton" runat="server" AutoPostBack="False" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
                                                                        CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
                                                                        Text="OK" Visible="False" Width="80px" OnClick="ValidationUpdatedButton_Click">
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
                <asp:Label ID="Label3" runat="server" CssClass="lblsmallFont" 
                    Text="If you enter a value into this Search text box and the web page does NOT contain the text, then VitalSigns will trigger an alert." 
                    Visible="False"></asp:Label>
                <dx:ASPxTextBox ID="txtSearch" runat="server" Visible="False" Width="170px">
                    <ValidationSettings ErrorDisplayMode="None">
                    </ValidationSettings>
                </dx:ASPxTextBox>
                <dx:ASPxLabel ID="ASPxLabel16" runat="server" CssClass="lblsmallFont" 
                    Text="Search Text  (optional):" Visible="False">
                </dx:ASPxLabel>
</asp:Content>
