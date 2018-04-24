<%@ Page Title="VitalSigns Plus - Configurator" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="VSWebUI.Default" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
.auto-style1 {
	text-align: center;
	vertical-align: top;
}
#feedback
{
 display:none;
}
.auto-style2 {
	font-family: Tahoma, Geneva, Verdana, sans-serif;
	color:Gray;
	font-size: small;
	font-weight: normal;
}
.headline {
	font-family: Tahoma, Geneva, Verdana, sans-serif;
	font-size: large;
	font-weight: bold;
	color: Gray;
}
.WelcomeLine{
	font-family: Tahoma, Geneva, Verdana, sans-serif;
	font-size:xx-large;
	font-weight:bold;
	color:#5060A8;
}
.hrline
{
    color: #C1C1C1;
    background-color: #C1C1C1;
    height: 1px;
    border: 0;
}

a.home1 {
text-decoration: none;
color:black; 
}
a.home1:hover{
	color: red;
	
	text-decoration: none;
}
a.home1:visited {text-decoration:none;}
</style>
<script type="text/javascript">

//    var d = new Date();
//    
//    var d1 = new Date(d.getTime() + 10 * 60000);
  // alert(d1);
    


    function RGBA(red, green, blue, alpha) {
        this.red = red;
        this.green = green;
        this.blue = blue;
        this.alpha = alpha;
        this.getCSS = function () {
            return "rgba(" + this.red + "," + this.green + "," + this.blue + "," + this.alpha + ")";
        }
    }

    // store a copy of the color
    var bgColor = new RGBA(255, 0, 0, 0.5);

    function setBgOpacity(elem, opac) {
        var ua = window.navigator.userAgent;
        var msie = ua.indexOf("MSIE ");

        if (msie > 0) {
            //Do nothing since IE does not support opacity
        }
        else{
            bgColor.alpha = opac;
            elem.style.backgroundColor = bgColor.getCSS();
        }
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      
    <%--<table style="width: 100%;" >
        <tr>
            <td>
                &nbsp;</td>
            <td align=left valign=top>
                <dx:ASPxLabel ID="ASPxLabel47" runat="server" Font-Size="Large" 
                    ForeColor="#182A50" Text="VitalSigns Configurator" Width="250px" 
                    Font-Bold="True">
                </dx:ASPxLabel>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <br />
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" ForeColor="Black" 
                    Text="Welcome to VS Configurator. Check the below FAQs.">
                </dx:ASPxLabel>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:HyperLink ID="HyperLink1" runat="server" CssClass="styleone1" 
                    NavigateUrl="~/Dashboard/" Visible="False">Go to Dashboard</asp:HyperLink>
            </td>
            <td>
                &nbsp;</td>
        </tr>
    </table>--%>
    <span class="WelcomeLine">Welcome to VitalSigns</span><br /><br />
    <table style="width: 60%">
    
	<tr class="link">
	
		<td>
        <div onmouseover="setBgOpacity(this, '0.1');" onmouseout="setBgOpacity(this, '0');">
        <table width="100%">
            <tr>
                <td class="auto-style1" style="width: 120px">
                    <img alt="logo" src="../images/icons/dashicon2.gif" />
                </td>
                <td>
                    <a class="home1" href="../Dashboard/OverallHealth1.aspx" target="_self">
                    <span class="headline">Go to the Dashboard</span><br class="auto-style2" />
		            <span class="auto-style2">Check the status of your servers and more...</span></a>
                </td>
            </tr>
        </table>
        </div>
        <div>
            <hr class="hrline" />
        </div>
	</td>
	</tr>
		<tr >
		<td>
        
        <div onmouseover="setBgOpacity(this, '0.1');" onmouseout="setBgOpacity(this, '0');">
        <table width="100%">
            <tr>
                <td class="auto-style1" style="width: 120px">
                    <img alt="help" height="30" src="../images/imagesIcons/license-icon.png" width="31" />
                </td>
                <td>
                    <a class="home1" href="LicenseInformation.aspx" target="_self">
                    <span class="headline">License Information</span><br />		            
                    <span class="auto-style2"><asp:Label ID="LicenseTypeLabel" runat="server"></asp:Label>&nbsp;version.
                    <asp:Label ID="ExpiresonLabel" runat="server" Text="Expires on "></asp:Label><asp:Label ID="ExpirationLabel" runat="server"></asp:Label></span></a>
                </td>
            </tr>
        </table>
        </div>
        <div>
            <hr class="hrline" />
        </div>
        </td>
	</tr>
	<tr class="link">
		<td>
        <div onmouseover="setBgOpacity(this, '0.1');" onmouseout="setBgOpacity(this, '0');">
        <table width="100%">
            <tr>
                <td class="auto-style1" style="width: 120px">
                    <img alt="help" height="30" src="../images/imagesIcons/help.gif" width="31" />
                </td>
                <td>
                    <a class="home1" href="GetAssemblyInfo.aspx">
		            <span class="headline">VitalSigns Assembly Version Information</span><br class="auto-style2" />
		            <span class="auto-style2">Find the build version of the UI & Service files</span></a>
                </td>
            </tr>
        </table>
        </div>
        <div>
            <hr class="hrline" />
        </div>
        </td>
	</tr>
	<tr class="link">
		<td>
        <div onmouseover="setBgOpacity(this, '0.1');" onmouseout="setBgOpacity(this, '0');">
        <table width="100%">
            <tr>
                <td class="auto-style1" style="width: 120px">
                    <img alt="group" height="29" src="../images/imagesIcons/messagebox_warning.png" width="33" />
                </td>
                <td>
                    <a class="home1" href="Alert_Settings.aspx" target="_self">
                    <span class="headline">Current status of the VitalSigns alerts</span>
                    <br class="auto-style2" />
		            <span class="auto-style2">The alerts are </span><span id="AlertsOnLabel" class="auto-style2" runat="server"></span>
                    </a>
                </td>
            </tr>
        </table>
        </div>
        <div>
            <hr class="hrline" />
        </div>
        </td>
	</tr>
	<tr class="link">
		<td>
        <div onmouseover="setBgOpacity(this, '0.1');" onmouseout="setBgOpacity(this, '0');">
        <table width="100%">
            <tr>
                <td class="auto-style1" style="width: 120px">
                    <img alt="News" height="40" width="40" src="../images/imagesIcons/home.png" />
                </td>
                <td>
                    <a class="home1" href="http://rprvitalsigns.com/" target=_blank>
                    <span class="headline">Visit us</span><br class="auto-style2" />
		            <span class="auto-style2">Follow the progress of VitalSigns at http://rprvitalsigns.com</a>
                </td>
            </tr>
		
        </table>
        </div>
        </td>
	</tr><tr><td>
		<div>
            <hr class="hrline" />
        </div></td>
		</tr>
	<tr class="link"  >
		<td >
        <div onmouseover="setBgOpacity(this, '0.1');" onmouseout="setBgOpacity(this, '0');" id="feedback">
        <table width="0%" >
            <tr  >
                <td class="auto-style1" style="width: 120px" >
                    <img alt="News" height="30" src="../images/imagesIcons/news.gif" width="45" />
                </td>
                <td>
                    <a class="home1" href="Feedback.aspx" visible="false">
                    <span class="headline">Feedback
		           </span><br class="auto-style2" />
		            <span class="auto-style2">Feedback Details</span></a>
                </td>
            </tr>
        </table>
        </div>
        </td>
	</tr>
	</table>
</asp:Content>
