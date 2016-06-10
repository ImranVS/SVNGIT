<%@ Page Title="VitalSigns Plus-SNMPDeviceProperties" Language="C#" MasterPageFile="~/Site1.Master"AutoEventWireup="true" CodeBehind="SNMPDeviceProperties.aspx.cs" Inherits="VSWebUI.Configurator.SNMPDeviceProperties" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>




<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.alert-success').delay(10000).fadeOut("slow", function () {
            });
            $('.alert-danger').delay(10000).fadeOut("slow", function () {
            });
        });
        </script>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%">
        <tr>
            <td>
                <div class="header" id="lblServer" runat="server">SNMP Device</div>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
                    Theme="Glass" Width="100%" EnableHierarchyRecreation="False">
                    <TabPages>
                        <dx:TabPage Text="Device Attributes">
                            <ContentCollection>
                                <dx:ContentControl runat="server">
                                    <table class="navbarTbl">
                                        <tr>
                                            <td>
                                                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" 
                                                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                                    GroupBoxCaptionOffsetY="-24px" HeaderText="Device Attributes" 
                                                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                                                    <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                                    <HeaderStyle Height="23px">
                                                    <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                                    </HeaderStyle>
                                                    <PanelCollection>
                                                        <dx:PanelContent runat="server">
                                                            <table class="style1">
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Name:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="NameTextBox" runat="server"  
                                                                            OnTextChanged="NameTextBox_TextChanged" Width="170px">
                                                                            <ValidationSettings ErrorDisplayMode="None">
                                                                                <RequiredField IsRequired="True" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel5" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Category:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="CategoryTextBox" runat="server" Width="170px">
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" 
                                                                                ErrorText="Category may not be empty." SetFocusOnError="True">
                                                                                <RequiredField ErrorText="" IsRequired="True" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td colspan="2">
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
                                                                            Text="IP or HostName:" Wrap="False">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="IPTextBox" runat="server" Width="170px">
                                                                          <%--  <ValidationSettings CausesValidation="True" ErrorDisplayMode="None">
                                                                                <RegularExpression ErrorText="This does not appear to be a valid IP Address device. Is it a valid host name?" 
                                                                                    ValidationExpression="^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$" />
                                                                                <RequiredField IsRequired="True" />
                                                                            </ValidationSettings>--%>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="oidLabel1" runat="server" CssClass="lblsmallFont" Text="OID:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="oidtextbox" runat="server" 
                                                                            OnTextChanged="oidtextbox_TextChanged" Width="170px">
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" 
                                                                                ErrorText="OID may not be empty." SetFocusOnError="True">
                                                                                <RequiredField ErrorText="" IsRequired="True" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
																	<td colspan="2">
                                                                        <dx:ASPxCheckBox ID="IncludeOnDashBoardCheckBox" runat="server" CheckState="Unchecked" 
                                                                            CssClass="lblsmallFont" Text="Include on Dashboard as 'Network Infrastructure'"  Wrap="False">
                                                                            <ValidationSettings ErrorDisplayMode="None">
                                                                            </ValidationSettings>
                                                                        </dx:ASPxCheckBox>
                                                                    </td>




                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Description:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="DescriptionTextBox" runat="server" 
                                                                            OnTextChanged="DescriptionTextBox_TextChanged" Width="170px">
                                                                            <ValidationSettings ErrorDisplayMode="None">
                                                                                <RequiredField IsRequired="True" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Location:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxComboBox ID="LocComboBox" runat="server">
                                                                            <ValidationSettings CausesValidation="True" Display="Dynamic" 
                                                                                ErrorDisplayMode="ImageWithTooltip" ErrorText="Location may not be empty.">
                                                                                <ErrorImage ToolTip="Select Location">
                                                                                </ErrorImage>
                                                                                <RequiredField ErrorText="Location may not be empty." IsRequired="True" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxComboBox>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td style="margin-left: 40px">
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
                                                    <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                                    <HeaderStyle Height="23px">
                                                    <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                                    </HeaderStyle>
                                                    <PanelCollection>
                                                        <dx:PanelContent runat="server">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel7" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Scan Interval:" Width="80px" Wrap="False">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td width="50px">
                                                                        <dx:ASPxTextBox ID="ScanIntervalTextBox" runat="server" Width="40px">
                                                                            <MaskSettings Mask="&lt;8..99999&gt;" />
                                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                    ValidationExpression="^\d+$" />
                                                                                <RequiredField IsRequired="True" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel8" runat="server" CssClass="lblsmallFont" 
                                                                            Height="16px" Text="minutes">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel9" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Off-Hours Scan Interval:" Wrap="False">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="OffScanTextBox" runat="server" Width="40px">
                                                                            <MaskSettings Mask="&lt;30..9999&gt;" />
                                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                    ValidationExpression="^\d+$" />
                                                                                <RequiredField IsRequired="True" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel11" runat="server" CssClass="lblsmallFont" 
                                                                            Text="minutes">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel10" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Retry Interval:" Wrap="False">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td width="50px">
                                                                        <dx:ASPxTextBox ID="RetryIntervalTextBox" runat="server" Width="40px">
                                                                            <MaskSettings Mask="&lt;2..1000&gt;" />
                                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                    ValidationExpression="^\d+$" />
                                                                                <RequiredField IsRequired="True" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel15" runat="server" CssClass="lblsmallFont" 
                                                                            Text="minutes">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel12" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Response Threshold:" Wrap="False">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="ResponseThrTextBox" runat="server" Width="40px">
                                                                            <MaskSettings Mask="&lt;250..99999&gt;" />
                                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                    ValidationExpression="^\d+$" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel13" runat="server" CssClass="lblsmallFont" 
                                                                            Text="milliseconds">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                </tr>
																<tr>
																<td colspan="6">

																<table>
                                                        <tr>
                                                           
                                                    <td class="caption">
                                                                <dx:ASPxLabel ID="lblSelectImage" runat="server" CssClass="lblsmallFont" Text="Select Image:">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                          
                                                       
													<td>
																						<dx:ASPxComboBox ID="CredentialsComboBox" runat="server" AutoPostBack="True" 
																							OnSelectedIndexChanged="CredentialsComboBox_SelectedIndexChanged" 
																							TextField="Name">
																						</dx:ASPxComboBox>
																					</td>
													<td style="width:100px" valign="top">
																					<img ID="Img1" runat="server" height="150" width="100"/>
																				</td>
                                                      </tr>
                                                       </table>
                                                  
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
                    <TabStyle Font-Bold="True">
                    </TabStyle>
                </dx:ASPxPageControl>
            </td>
        </tr>
        <tr>
            <td valign="top" colspan="3">
                <div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">Error attempting to update the status table.
                 </div>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td valign="top">
                <dx:ASPxButton ID="PingButton" runat="server" CausesValidation="False" 
                    CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                    CssPostfix="Office2010Blue" OnClick="PingButton_Click" 
                    SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="Ping" 
                                Theme="Office2010Blue">
                </dx:ASPxButton>
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                            <dx:ASPxButton ID="FormOKButton" runat="server" AutoPostBack="False" 
                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                CssPostfix="Office2010Blue" OnClick="FormOKButton_Click" 
                                SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="OK" 
                                Theme="Office2010Blue">
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="FormCancelButton" runat="server" CausesValidation="False" 
                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                CssPostfix="Office2010Blue" OnClick="FormCancelButton_Click" 
                                SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" 
                                Text="Cancel" Theme="Office2010Blue">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
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
                    </HeaderStyle>
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                            <dx:ASPxPanel ID="Panel2" runat="server" DefaultButton="btOK">
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
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
                                                        </dx:ASPxButton>
                                                        <dx:ASPxButton ID="ValidationUpdatedButton" runat="server" AutoPostBack="False" 
                                                            CausesValidation="False" 
                                                            CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                                            CssPostfix="Office2010Blue" OnClick="ValidationUpdatedButton_Click" 
                                                            SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="OK" 
                                                            Visible="False" Width="80px">
                                                            <ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }" />
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
    </asp:Content>
