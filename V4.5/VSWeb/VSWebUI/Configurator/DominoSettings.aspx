<%@ Page Title="VitalSigns Plus - Domino Settings" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="DominoSettings.aspx.cs" Inherits="VSWebUI.Configurator.webform" %>
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
        });
       //10/30/2013 NS added - fix for when an Enter key is pressed within the text box on the page - redirect the
       //submit function to the actual Go button on the page instead of performing a whole page submit
       function OnKeyDown(s, e) {
           //alert(window.event.keyCode);
           //var keyCode = (window.event) ? e.which : e.keyCode;
           //alert(keyCode);
           var keyCode = window.event.keyCode;
           if (keyCode == 13)
               goButton.DoClick();
       }
   </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="80%">
        <tr>
            <td>
                <div class="header" id="servernamelbldisp" runat="server">IBM Domino Settings</div>
            </td>
        </tr>
        <tr>
            <td>
                <div class="info">VitalSigns is using the ID listed in the Notes ID field below. <b>Important:</b> if you switch IDs using the Notes client, you must provide the new password for VitalSigns.
                            </div>
                <div id="successDiv" runat="server" class="alert alert-success" style="display: none">IBM Domino Settings were successully updated.
                <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxRoundPanel runat="server" GroupBoxCaptionOffsetY="-24px" 
                    HeaderText="Notes ID" 
                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" CssPostfix="Glass" 
                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" ID="ASPxRoundPanel2" 
                    Width="100%">
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
<PanelCollection>
<dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
    <table>
        <tr>
            <td align="right">
                <dx:ASPxLabel runat="server" Text="Notes Program Directory Path:" Wrap="False" 
                    CssClass="lblsmallFont" ID="ASPxLabel1"></dx:ASPxLabel>

            </td>
            <td>
                <dx:ASPxTextBox runat="server" Width="400px" ID="NotesProgramTextBox">
<ValidationSettings ErrorDisplayMode="ImageWithTooltip">
<RequiredField IsRequired="True"></RequiredField>
</ValidationSettings>
</dx:ASPxTextBox>

            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <dx:ASPxLabel runat="server" Text="For example: C:\Program Files\Lotus\Notes" 
                    Wrap="False" CssClass="lblsmallFont" ID="ASPxLabel4"></dx:ASPxLabel>

            </td>
        </tr>
        <tr>
            <td align="right">
                <dx:ASPxLabel runat="server" Text="Notes ID file:" Wrap="False" Height="16px" 
                    CssClass="lblsmallFont" ID="ASPxLabel2"></dx:ASPxLabel>

            </td>
            <td>
                <dx:ASPxTextBox runat="server" Width="400px" ID="NotesIDfileTextBox">
<ValidationSettings ErrorDisplayMode="ImageWithTooltip">
<RequiredField IsRequired="True"></RequiredField>
</ValidationSettings>
</dx:ASPxTextBox>

            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <dx:ASPxLabel runat="server" 
                    Text="For example: C:\Program Files\Lotus\Notes\Data\myname.ID" Wrap="False" 
                    CssClass="lblsmallFont" ID="ASPxLabel5"></dx:ASPxLabel>

            </td>
        </tr>
        <tr>
            <td align="right">
                <dx:ASPxLabel runat="server" Text="Notes.INI:" Wrap="False" 
                    CssClass="lblsmallFont" ID="ASPxLabel3"></dx:ASPxLabel>

            </td>
            <td>
                <dx:ASPxTextBox runat="server" Width="400px" ID="NotesINITextBox">
<ValidationSettings>
<RequiredField IsRequired="True"></RequiredField>
</ValidationSettings>
</dx:ASPxTextBox>

            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <dx:ASPxLabel runat="server" 
                    Text="For example: C:\Program Files\Lotus\Notes\notes.ini" Wrap="False" 
                    CssClass="lblsmallFont" ID="ASPxLabel6"></dx:ASPxLabel>

            </td>
        </tr>
        <tr>
            <td valign="top" align="right">
                <dx:ASPxLabel ID="ASPxLabel9" runat="server" CssClass="lblsmallFont" 
                    Text="Notes Password:" Wrap="False">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxButton ID="RegisterButton" runat="server" CssClass="sysButton"
                    Text="Register Notes Password" Wrap="False" OnClick="RegisterButton_Click">
                </dx:ASPxButton>
                 <div id="successDivPwd" class="alert alert-success" runat="server" style="width: 130px; display: none">Password is correct.
                 <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                </div>
                <div id="errorDivPwd" class="alert alert-danger" runat="server" style="width: 130px; display: none">Password is incorrect.
                <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                </div>   
            </td>
        </tr>
    </table>
    </dx:PanelContent>
</PanelCollection>
</dx:ASPxRoundPanel>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <dx:ASPxRoundPanel runat="server" GroupBoxCaptionOffsetY="-24px" 
                    HeaderText="Other Settings" 
                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" CssPostfix="Glass" 
                    ID="ASPxRoundPanel3" Width="100%">
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
<PanelCollection>
<dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
    <table>
        <tr>
            <td colspan="2">
                <dx:ASPxCheckBox runat="server" CheckState="Unchecked" 
                    Text="EXJournaling is enabled" AutoPostBack="True" CssClass="lblsmallFont" 
                    ID="EXJournalingEnabledCheckBox" 
                    OnCheckedChanged="EXJournalingEnabledCheckBox_CheckedChanged"></dx:ASPxCheckBox>

            </td>
            <td>
            <dx:ASPxCheckBox runat="server" CheckState="Unchecked" 
                    Text="Enable Console Commands" AutoPostBack="True" CssClass="lblsmallFont" 
                    ID="EnableConsoleCommandCheckBox" 
                    OnCheckedChanged="EnableConsoleCommandCheckBox_CheckedChanged" 
                    Width="196px"></dx:ASPxCheckBox></td>
        </tr>
        
        <tr>
            <td>
                <dx:ASPxLabel runat="server" Text="Threshold:" CssClass="lblsmallFont" 
                    ID="ThresholdLabel"></dx:ASPxLabel>

            </td>
            <td>
            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updatepan1">
                <ContentTemplate>
                <dx:ASPxTextBox ID="ThresholdTextBox" runat="server" Enabled="False" 
                    Width="170px" CssClass="txtsmall">
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                    </ValidationSettings>
                </dx:ASPxTextBox>
                
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="EXJournalingEnabledCheckBox"></asp:AsyncPostBackTrigger>
</Triggers>
</asp:UpdatePanel>

            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel runat="server" Text="Consecutive Telnet:" CssClass="lblsmallFont" 
                    ID="ASPxLabel7"></dx:ASPxLabel>

            </td>
            <td>
                <dx:ASPxTextBox ID="ConsecutiveTelnetTextBox" runat="server" Enabled="True" 
                    Width="170px" CssClass="txtsmall">
                    <MaskSettings Mask="&lt;0..999999999&gt;" />
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
                    <RequiredField ErrorText="Enter Number of Consecutive Telnets" IsRequired="True" />
                    <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                          ValidationExpression="^\d+$" />
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
        <tr>
            <td>
                <dx:ASPxRoundPanel ID="ASPxRoundPanel5" runat="server" 
                    HeaderText="Domino HTTP Username/Password" Theme="Glass" Width="100%" 
                    Visible="False">
                    <HeaderStyle HorizontalAlign="Center" />
                    <PanelCollection>
<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
    <table >
        <tr>
            <td colspan="2">
                <div id="infoDivDominoPwd" class="info">VitalSigns uses this username and password to access
                pages which require authentication, such as when checking
                the Notes Traveler Servlet.
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel10" runat="server" CssClass="lblsmallFont" 
                    Text="User Name:" Wrap="False">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxTextBox ID="DominoUsernameTextBox" runat="server" Width="170px">
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel11" runat="server" CssClass="lblsmallFont" 
                    Text="Password:">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxButton ID="RegisterDominoPwdButton" runat="server" 
                    OnClick="RegisterDominoPwdButton_Click" Text="Register Domino Password" 
                    Theme="Office2010Blue" Wrap="False">
                </dx:ASPxButton>
                <div id="successDivDomPwd" class="alert alert-success" runat="server" style="width: 130px; display: none">Password is correct.
                <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                </div>
                <div id="errorDivDomPwd" class="alert alert-danger" runat="server" style="width: 130px; display: none">Password is INCORRECT.
                <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                </div>
            </td>
        </tr>
    </table>
                        </dx:PanelContent>
</PanelCollection>
                </dx:ASPxRoundPanel>
                <dx:ASPxPopupControl runat="server" PopupHorizontalAlign="WindowCenter" 
                    PopupVerticalAlign="WindowCenter" Modal="True" 
                    HeaderText="Please Enter a Password:" Theme="MetropolisBlue" 
                    ID="ResetPwdPopupControl">
                    <ContentCollection>
<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                            <table class="tableWidth100Percent">
                                <tr>
                                    <td colspan="2">
                                        <dx:ASPxTextBox runat="server" Width="170px" Password="True" 
                                            ClientInstanceName="resetPwdTxtBox" ID="ResetPwdTextBox">
<ClientSideEvents KeyDown="function(s, e) {OnKeyDown(s, e);}"></ClientSideEvents>
</dx:ASPxTextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxButton runat="server" ClientInstanceName="goButton" Text="OK" CssClass="sysButton"
                                            ID="ResetPwdOKBtn" 
                                            OnClick="ResetPwdOKBtn_Click"></dx:ASPxButton>

                                    </td>
                                    <td>
                                        <dx:ASPxButton runat="server" Text="Cancel" CssClass="sysButton"
                                            ID="ResetPwdCancelBtn" OnClick="ResetPwdCancelBtn_Click"></dx:ASPxButton>

                                    </td>
                                </tr>
                            </table>
    <dx:ASPxLabel ID="SetWhichPwd" runat="server" Text="" Visible="false">
    </dx:ASPxLabel>
                        </dx:PopupControlContentControl>
</ContentCollection>
</dx:ASPxPopupControl>

<dx:ASPxPopupControl runat="server" PopupHorizontalAlign="WindowCenter" 
                        PopupVerticalAlign="WindowCenter" Modal="True" HeaderText="Information" 
                        Theme="Glass" ID="MsgPopupControl"><ContentCollection>
<dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">
                            <table>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel runat="server" Text="ASPxLabel" Wrap="False" Theme="Default" 
                                            CssClass="lblsmallFont" ID="MsgLabel"></dx:ASPxLabel>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxButton runat="server" Text="OK" Theme="Office2010Blue" Width="60px" 
                                            ID="ASPxButton1" OnClick="ASPxButton1_Click"></dx:ASPxButton>

                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
</ContentCollection>
</dx:ASPxPopupControl>
            </td>
        </tr>
        <tr>
            <td>
                <div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">The following fields were not updated:
                </div>
                <table>
                    <tr>
                        <td>
                <dx:ASPxButton ID="SaveButton" runat="server" OnClick="SaveButton_Click" 
                    Text="Save" CssClass="sysButton">
                </dx:ASPxButton>
                        </td>
                        <td>
                <dx:ASPxButton ID="CancelButton" runat="server" OnClick="CancelButton_Click" 
                    Text="Cancel" CausesValidation="False" 
                    CssClass="sysButton">
                </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
    <dx:ASPxCheckBox runat="server" Wrap="False" CheckState="Unchecked" 
                    Text="Prompt for password or sensitive operations" Visible="False" 
                    CssClass="lblsmallFont" ID="PromptforpasswordCheckBox"></dx:ASPxCheckBox>

    <dx:ASPxCheckBox runat="server" Wrap="False" CheckState="Unchecked" 
                    Text="Alert on Server. Expansion Factor" Visible="False" 
                    CssClass="lblsmallFont" ID="AlertonCheckBox"></dx:ASPxCheckBox>

                <dx:ASPxLabel runat="server" Text="BlackBerry Agent:" Wrap="False" 
                    CssClass="lblsmallFont" ID="BlackberryAgentLabel" Visible="False"></dx:ASPxLabel>

                <dx:ASPxTextBox runat="server" Width="170px" Visible="False" 
                    ID="BlackBerryAgentTextBox"></dx:ASPxTextBox>

            <dx:ASPxRoundPanel runat="server" GroupBoxCaptionOffsetY="-24px" 
                    HeaderText="OutBound Email" Width="288px" 
                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" CssPostfix="Glass" 
                    ID="ASPxRoundPanel4" Visible="False">
<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
<PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
    <table>
        <tr>
            <td colspan="2">
                <dx:ASPxCheckBox runat="server" Wrap="False" CheckState="Unchecked" 
                    Text="Alert on &#39;stuck&#39; pending message" CssClass="lblsmallFont" 
                    ID="AlertonstuckpendingmessageCheckBox"></dx:ASPxCheckBox>

            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label runat="server" 
                    Text="Send &lt;b&gt;Pending Mail Alert&lt;/b&gt; if a specific message has been in the outbound mail queue longer than:" 
                    CssClass="lblsmallFont" ID="Label2"></asp:Label>

            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxTextBox runat="server" Width="40px" ID="SendpendingmailAlertTextBox">
<ValidationSettings ErrorDisplayMode="ImageWithTooltip" CausesValidation="True" 
                        SetFocusOnError="True">
<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
        ValidationExpression="^\d+$"></RegularExpression>
</ValidationSettings>
</dx:ASPxTextBox>

            </td>
            <td>
                <dx:ASPxLabel runat="server" Text="minutes" Wrap="False" 
                    CssClass="lblsmallFont" ID="ASPxLabel8"></dx:ASPxLabel>

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
