<%@ Page Language="C#"  SmartNavigation="false" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="VSWebUI.ErrorPage" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>




<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
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
            <div class="login_div" align="center">
                <div class="login_div_top" id="LoginHeader" runat="server">
                   
                  Oops something went wrong</div>

			
	<div class="login_div_bot" align="center">
	<b><p class="body_txt">&nbsp; Sorry, an internal error occurred with the page you are 
            trying to access</p></b>
				<p class="clear gap">
                    </p>
					<p class="body_txt" id="ExpaireText" runat="server" >
                        In most cases this page is displayed when an error has occurred and has been 
                        logged. You may click one of the buttons below to proceed.
</p>
					
<p class="body_txt">
                      *Note that error messages are intentionally hidden for security reasons. 
                      However, your site administrator has access to the error logs which can help 
                      identify and resolve the underlying issue.</p>
                   <p class="clear gap">
                    </p>
					
					 <p class="br_line">
                    </p>
					<p class="clear gap">
                    </p>
					
					<p>
					 
					<asp:Button ID="btnConfigurator" runat="server" Text="Go to Configurator" Wrap="False" class="dash_btn" OnClick="btnConfigurator_Click" CausesValidation="False"/>
                    <asp:Button  Wrap="False" ID="btnDashboard" runat="server" Text="Go to Dashboard" class="dash_btn" OnClick="btnDashboard_Click"/> 
                    
       </p>
				 <p>
                        <asp:Button ID="Loginbt" runat="server" Text="Login" class="submit_btn" OnClick="LoginButton_Click"  OnClientClick="javascript:return ASPxClientEdit.ValidateGroup(null)"  Visible="false"/>
                        
                    </p>	 
            </div>

         
            <div class="clear">
                
            </div>
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
    <script type="text/javascript">    	var x = document.getElementById("PasswordTextBox_I"); x.setAttribute("type", "password");</script>
</body>
</html>
<%-- <dx:ASPxButton ID="btnConfigurator" runat="server" Text="Go to Configurator" 
                     Wrap="False"  OnClick="btnConfigurator_Click" Theme="Office2010Blue"
                    CausesValidation="False">
                </dx:ASPxButton><dx:ASPxButton ID="btnDashboard" runat="server" Text="Go to Dashboard" 
                    Wrap="False"  OnClick="btnDashboard_Click" Theme="Office2010Blue"
                    CausesValidation="False">
                </dx:ASPxButton>--%>

				