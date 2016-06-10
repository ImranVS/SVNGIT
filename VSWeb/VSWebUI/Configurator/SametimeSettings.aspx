<%@ Page Title="VitalSigns Plus - Sametime Settings" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="SametimeSettings.aspx.cs" Inherits="VSWebUI.Configurator.SametimeSettings" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>


<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx1" %>

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
//            $('#loginForm').attr('autocomplete', 'off');
        });
        </script>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            height: 31px;
        }
    </style>
     <script type="text/javascript">
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
                <div class="header" id="servernamelbldisp" runat="server">IBM Sametime Settings</div>
                <div id="successDiv" runat="server" class="alert alert-success" style="display: none">IBM Sametime Settings were successully updated.
                <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                </div>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <dx:ASPxRoundPanel ID="ASPxRoundPanel4" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                    CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                    HeaderText="User Accounts" Width="100%">
                   
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                            <table width="100%">
                                <tr>
                                    <td width="25%">
                                        <dx:ASPxLabel ID="ASPxLabel9" runat="server" CssClass="lblsmallFont" 
                                            Text="User Name 1:">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel11" runat="server" CssClass="lblsmallFont" 
                                            Text="User Name 2:">
                                        </dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxTextBox ID="UserTextBox" runat="server" Width="170px">
                                            <ValidationSettings>
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="User2TextBox" runat="server" Width="170px">
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="StpButton" runat="server" CssClass="sysButton"
                                            Text="Enter Sametime Password" OnClick="StpButton_Click" Wrap="False" 
                                            >
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="Stp2Button" runat="server" CssClass="sysButton"
                                            OnClick="Stp2Button_Click" Text="Enter Sametime Password" Wrap="False">
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="Label2" runat="server" CssClass="lblsmallFont" 
                                            Text="The Sametime Password is stored as an encrypted byte stream."></asp:Label>
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
                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="100%" 
                    HeaderText="Extended Statistics" Theme="Glass">
                    <PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
    <table class="tableWidth100Percent">
        <tr>
            <td>
                <dx:ASPxCheckBox ID="AdvancedSametimeCheckBox" runat="server" 
                    CheckState="Unchecked" CssClass="lblsmallFont" 
                    Text="Collect extended Sametime statistics">
                </dx:ASPxCheckBox>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel12" runat="server" CssClass="lblsmallFont" 
                                Height="16px" Text="Sametime Servlet Port:">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxTextBox ID="SametimeServletPortTextBox" runat="server" Width="170px" autocomplete="off"> 
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" CssClass="lblsmallFont" 
                    Text="The &quot;advanced&quot; Sametime statistics are captured by querying the statistics servlet installed on the Sametime server at http://hostname/servlets/statistics&lt;br&gt;&lt;br&gt; Normally, calling this servlet requires authentication. User Name 1 must be in the ACL of stconfig.nsf in the Sametime Monitor role.&lt;br&gt;&lt;br&gt; For more information, please google &quot;Sametime Statistics and Monitoring toolkit&quot;."></asp:Label>
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
                <dx:ASPxPopupControl ID="SameTimePopupControl" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                    CssPostfix="Glass" HeaderText="Password" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                    Modal="True" PopupHorizontalAlign="WindowCenter" 
                    PopupVerticalAlign="WindowCenter" Width="300px" Theme="MetropolisBlue">
                    <LoadingPanelImage Url="~/App_Themes/Glass/Web/Loading.gif">
                    </LoadingPanelImage>
                    <HeaderStyle>
                        <Paddings PaddingLeft="10px" PaddingRight="6px" PaddingTop="1px" />
                    </HeaderStyle>
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
                            <table class="style1">
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="passwordLabel" runat="server" Text="Enter Your Password">
                                        </dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxTextBox ID="passwordTextBox" runat="server" Password="True" 
                                            Width="170px" ClientInstanceName="resetPwdTxtBox">
                                            <ClientSideEvents KeyDown="function(s, e) {OnKeyDown(s, e);}" />
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style2">
                                        <dx:ASPxButton ID="passwordOK" runat="server" CssClass="sysButton"
                                            OnClick="passwordOK_Click"
                                            Text="OK" ClientInstanceName="goButton" 
                                            >
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="WhichUserLabel" runat="server" Text="ASPxLabel" 
                                            Visible="False">
                                        </dx:ASPxLabel>
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
                            <dx:ASPxButton ID="SametimeapplyButton" runat="server" CssClass="sysButton"
                                Text="Save" OnClick="SametimeapplyButton_Click">
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="FormCancelButton" runat="server" CssClass="sysButton"
                                Text="Cancel" OnClick="FormCancelButton_Click" 
                                CausesValidation="False">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
