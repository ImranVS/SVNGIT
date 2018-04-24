﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="VSWebUI.Login" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>




<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<link rel="stylesheet" type="text/css" href="css/style.css" />
<style>
	::-ms-reveal {
    display: none;
}
</style>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>::. VitalSigns Plus - Login .::</title>
    <link href="css/vital_signs_styles.css" rel="stylesheet" type="text/css" />
      <link rel="shortcut icon" href="images/favicon.ico" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="main">
        <div class="container">
            <div class="header">
                <div class="logo_left">
                    
                        <img src="images/vitalsigns_logo.png" alt="vitalsignslogo" width="174" height="51"
                            border="0" /></div>
                <div class="logo_right">
                    <div class="welcome_div">
                        Welcome to
                        <br />
                        RPR Wyatt</div>
                    <div class="rpr_logo">
                        <a href="#">
                            <img src="images/rprwyatt_logo.jpg" alt="rprwyattlogo" width="84" height="41" border="0" /></a></div>
                </div>
            </div>
            <div class="clear">
            </div>
            <div class="login_div">
                <div class="login_div_top" id="LoginHeader" runat="server">
                    User Login</div>
					 <div class="login_div_bot">
				<p class="body_txt" id="ExpaireText" runat="server" visible="false">
                      Your session expired due to inactivity.</p>
                    <p class="body_txt">
                       Enter your login name and password below 
					   or use the Dashboard Only / Executive Summary
					   buttons to access with limited functionality.</p>
                    <p class="clear">
                    </p>
                    <p><b>Username:</b></p>
                    <p>
                    <dx:ASPxTextBox runat="server" NullText="Enter your Login Name" ID="LoginTextBox" class="txt_box_form" Width="100%" Height="28">
                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" CausesValidation="True" Display="Dynamic"
                            SetFocusOnError="True">
                            <RequiredField IsRequired="True" ErrorText="Enter your Login Name"></RequiredField>
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                   </p>
                    <p class="clear">
                    </p><br />
                     <p><b>Password:</b></p>
                    <p>
                    <dx:ASPxTextBox runat="server"  class="txt_box_form"  Width="100%" Height="28" 
                        ID="PasswordTextBox">
                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" CausesValidation="True">
                            <RequiredField ErrorText="Enter your Password"></RequiredField>
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                    </p>
                    <p class="clear gap">
                    </p>
                    <p class="body_txt links" style="text-align:center">
                        <dx:ASPxCheckBox runat="server" CheckState="Unchecked" Text="Remember me" ID="RememberCheckBox" >
                        </dx:ASPxCheckBox>
                    </p>
                    <p class="clear gap">
                    </p>
                    <div id="conerror" runat="server" class="alert alert-danger" style="display: none">Error
			            </div>
                    <p class="body_txt links">
                        <dx:ASPxLabel ID="ErrorLabel" runat="server" ForeColor="Red" Visible="true">
                        </dx:ASPxLabel>
                    </p>
                    <p class="clear">
                    </p>
                    <div class="captcha_div2">
                <dx:ASPxCaptcha ID="ValidateCaptcha" runat="server" CssClass="lblsmallfont" 
                    CssFilePath="~/App_Themes/PlasticBlue/{0}/styles.css" CssPostfix="PlasticBlue" 
                    Font-Overline="False" 
                    SpriteCssFilePath="~/App_Themes/PlasticBlue/{0}/sprite.css" Visible="False">
                    <LoadingPanelImage Url="~/App_Themes/PlasticBlue/Editors/Loading.gif">
                    </LoadingPanelImage>
                    <TextBoxStyle CssClass="lblsmallfont" Width="80px" />
                    <ValidationSettings ErrorText="Invalid Entry!">
                        <RequiredField ErrorText="Enter code" IsRequired="True" />
                    </ValidationSettings>
                    <ChallengeImage BackgroundColor="WhiteSmoke" BorderColor="#CCCCCC" 
                        ForegroundColor="#485C9F" Height="40" Width="130">
                    </ChallengeImage>
                </dx:ASPxCaptcha>
                    </div>
                    <p class="clear gap">
                    </p>
                    <p>
                        <asp:Button ID="Button1" runat="server" Text="Login" class="submit_btn" OnClick="LoginButton_Click"  OnClientClick="javascript:return ASPxClientEdit.ValidateGroup(null)" />
                        
                    </p>
                    <p class="clear gap">
                    </p>
                    <p class="body_txt links">
                        <asp:LinkButton ID="ForgotPwdLink" runat = "server" Text="Forgot Password?"
                    OnClick="ForgotPwdLink_Click"></asp:LinkButton></p>
                    <p class="clear gap">
                    </p>
                    <p class="br_line">
                    </p>
                    <p>
                        <asp:Button ID="DashOnlyButton" runat="server" Text="Dashboard Only" class="dash_btn" OnClick="DashOnlyButton_Click"/><asp:Button ID="SummaryButton" runat="server" Text="Executive Summary" class="dash_btn" OnClick="SummaryButton_Click"/> 
                    </p>
                </div>
            </div>
            <div class="dash_main_div">
                <a href="http://rprvitalsigns.com/mobility/mobile-monitoring/" target=_blank>
                    <div class="iphone_div">
                        <p>
                            <img src="images/iphone_icon.png" alt="iphoneicon" width="28" height="36" border="0" /></p>
                        <p class="clear gap">
                        </p>
                        <p class="body_txt2">
                            Mobile Dashboard<br />
                            for</p>
                        <p class="body_txt3">
                            iPhone</p>
                    </div>
                </a><a href="http://rprvitalsigns.com/mobility/mobile-monitoring/" target=_blank>
                    <div class="android_div">
                        <p>
                            <img src="images/android_icon.png" alt="androidicon" width="32" height="36" border="0" /></p>
                        <p class="clear gap">
                        </p>
                       <p class="body_txt2">Mobile Dashboard<br />
for</p>
  <p class="body_txt3">Android</p>
                    </div>
                </a>
            </div>
            <div class="clear">
                <dx:ASPxNavBar ID="MainMenu" runat="server" AutoCollapse="True" CssFilePath="~/App_Themes/PlasticBlue/{0}/styles.css"
                    CssPostfix="PlasticBlue" SpriteCssFilePath="~/App_Themes/PlasticBlue/{0}/sprite.css"
                    AutoPostBack="True" Font-Bold="True" AllowSelectItem="True" Font-Size="Large"
                    Visible="False">
                    <GroupHeaderImage Url="~/images/information.png">
                    </GroupHeaderImage>
                    <ItemImage Url="~/images/cog.png">
                    </ItemImage>
                    <LoadingPanelImage Url="~/App_Themes/PlasticBlue/Web/nbLoading.gif">
                    </LoadingPanelImage>
                    <GroupHeaderStyle Font-Names="microsoft sans serif, 9.5pt" Font-Size="9.5pt">
                    </GroupHeaderStyle>
                    <ItemStyle Font-Names="Microsoft Sans Serif" Font-Size="9.5pt" ForeColor="#15428B">
                        <SelectedStyle Font-Bold="False" Font-Names="Arial">
                        </SelectedStyle>
                        <HoverStyle BackColor="#3F5396" ForeColor="White">
                        </HoverStyle>
                    </ItemStyle>
                </dx:ASPxNavBar>
            </div>
        </div>
        <div class="clear">
        </div>
        <div class="wrapper_footer">
            <div class="footer">
                © 2015, RPR Wyatt , Inc. All rights reserved.</div>
        </div>
    </div>
    </form>
    <script type="text/javascript">        var x = document.getElementById("PasswordTextBox_I"); x.setAttribute("type", "password");</script>
</body>
</html>
