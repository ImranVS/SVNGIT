<%@ Page Title="VitalSigns Plus - EditServertask" Language="C#" MasterPageFile="~/Site1.Master"AutoEventWireup="true" CodeBehind="EditServerTask.aspx.cs" Inherits="VSWebUI.Configurator.EditServerTask" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.alert-success').delay(10000).fadeOut("slow", function () {
            });
        });
        </script>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table>
    <tr>
        <td>
            <div class="header" id="servernamelbldisp" runat="server">Server Task Definition</div>
        </td>
    </tr>
    <tr>
        <td>
            <table width="100%">
                <tr>
                    <td colspan="2">
                        <div class="info">VitalSigns can monitor any server task, if the correct settings are provided for task. To obtain the correct settings, issue a &quot;show stat server.task&quot; command on Domino Server console window. You'll get back something like <br /><br />
                        Server.Task = Rooms and Resources Manager:Idle:[05/20/2011 05:40:13 MST] <br />
                        Server.Task = Admin Process:Idle:[05/20/2011 05:39:40 MST] <br />
                        Server.Task = HTTP Server:Listen for connect requests on TCP Port:80:[05/20/2011 05:40:13 MST] <br /><br />
                        The text immediately following the '=' sign is the <b>Console String</b>, and the text following that is the <b>Idle String</b>.
                        </div>
                    </td>
                </tr>
                    <tr>
                        <td valign="top">

                       
                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" 
                    HeaderText="Task Definition" Width="100%" 
                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                    GroupBoxCaptionOffsetY="-24px" 
                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Height="180px">
                    <HeaderStyle Height="23px" >
                    </HeaderStyle>
                    <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
<PanelCollection>
<dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
    <table width="100%">
        <tr>
            <td width="150px">
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Task Name:" CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxTextBox ID="EditTaskNameTextBox" runat="server" Width="170px" 
                    CssClass="txtsmall">
                    <ValidationSettings EnableCustomValidation="True">
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Idle String:" CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxTextBox ID="EditIdleStringTextBox" runat="server" Width="170px" 
                    CssClass="txtsmall">
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Console String:" CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxTextBox ID="EditConsoleStringTextBox" runat="server" Width="170px" 
                    CssClass="txtsmall">
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Load Command:" 
                    CssClass="lblsmallFont" Wrap="False">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxTextBox ID="EditLoadCmdTextBox" runat="server" Width="170px" 
                    CssClass="txtsmall">
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="Retry Count:" CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxTextBox ID="EditRetryCountTextBox" runat="server" Width="170px" 
                    CssClass="txtsmall">
                    <MaskSettings ErrorText="Please enter a Retry Interval that is a positive number, in minutes." 
                        Mask="&lt;0..100&gt;" ShowHints="True" />
                    <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                        SetFocusOnError="True">
                        <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                            ValidationExpression="^\d+$" />
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </td>
        </tr>
    </table>
    </dx:PanelContent>
</PanelCollection>
                </dx:ASPxRoundPanel>
            </td>
            <td valign="top">
                <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" Width="100%" 
                    HeaderText="Task Hang Detection" 
                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                    GroupBoxCaptionOffsetY="-24px" 
                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Height="180px">
                    <HeaderStyle Height="23px" >
                    </HeaderStyle>
                    <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
<PanelCollection>
<dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
    <table>
        <tr>
            <td>
                <dx:ASPxCheckBox ID="EditEnableCheckBox" runat="server" CheckState="Unchecked" 
                    Text="Enable Hang Detection" CssClass="lblsmallFont" AutoPostBack="True" 
                    OnCheckedChanged="EditEnableCheckBox_CheckedChanged">
                </dx:ASPxCheckBox>
            </td>
        </tr>
        <tr><td>
        <table><tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="Maximum run time:" CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
          
             
              <td>
                <dx:ASPxTextBox ID="EditMaxRunTimeTextBox" runat="server" Width="40px" 
                    CssClass="txtsmall">
                    <MaskSettings ErrorText="Please enter a Retry Interval that is a positive number, in minutes." 
                        Mask="&lt;0..100&gt;" />
                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" CausesValidation="True" 
                        SetFocusOnError="True">
                        <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                            ValidationExpression="^\d+$" />
                    </ValidationSettings>
                </dx:ASPxTextBox>
                </td>
                 <td>
                <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="minutes" CssClass="lblsmallFont">
                </dx:ASPxLabel>
                 </td>
               
           </tr></table></td>
           
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel9" runat="server" CssClass="lblsmallFont" 
                    Text="Hang Detection is optional and may not be appropriate for all tasks.

The [time/date] stamp shown above is tracked. If the time is further in the past than the Maximum Run Time specified, the task will be considered hung.">
                </dx:ASPxLabel>
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
                <div id="errorDiv" class="alert alert-danger" runat="server" style="display: none">Error.
                </div>
                <table>
<tr>
 <td>
                <dx:ASPxButton ID="formOKButton" runat="server" CssClass="sysButton"
                    Text="OK" 
                    onclick="formOKButton_Click">
                </dx:ASPxButton>
            </td>
            <td>
                <dx:ASPxButton ID="CancelButton" runat="server" CssClass="sysButton"
                    Text="Cancel" onclick="CancelButton_Click1" 
                    CausesValidation="False">
                </dx:ASPxButton>
            </td>
</tr>
</table>
               </td>
<td colspan="2">

</td>
           
        </tr>
                </table>
    <dx:ASPxPopupControl ID="ErrorMessagePopupControl" runat="server" CloseAction="CloseButton"
                        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                        HeaderText="Validation Failure" AllowDragging="True" EnableAnimation="False"
                        Height="150px" Width="300px" ClientInstanceName="pcErrorMessage" 
                        EnableViewState="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                        CssPostfix="Glass" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css">
                        <LoadingPanelImage Url="~/App_Themes/Glass/Web/Loading.gif">
                        </LoadingPanelImage>
                        <HeaderStyle>
                        <Paddings PaddingLeft="10px" PaddingRight="6px" PaddingTop="1px" />
                        </HeaderStyle>
                        <ContentCollection>
                            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                                <dx:ASPxPanel ID="Panel1" runat="server" DefaultButton="btOK">
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent5" runat="server">
                                            <div style="min-height: 70px;">
                                                <dx:ASPxLabel ID="ErrorMessageLabel" runat="server" Text="Username:">
                                                </dx:ASPxLabel>
                                            </div>
                                            <div>
                                                <table width="100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="center">
                                                            <dx:ASPxButton ID="ValidationOkButton" runat="server" Text="Ok" Width="80px" 
                                                                AutoPostBack="False" 
                                                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                                                CssPostfix="Office2010Blue" 
                                                                SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css">
                                                                <ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }" />
                                                            </dx:ASPxButton>
                                                            <dx:ASPxButton ID="ValidationUpdatedButton" runat="server" AutoPostBack="False" 
                                                                OnClick="ValidationUpdatedButton_Click" Text="Ok" Visible="False" 
                                                                Width="80px" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                                                CssPostfix="Office2010Blue" 
                                                                SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css">
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
        </td>
    </tr>
</table>
    </asp:Content>
