<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Executive_Summary.aspx.cs" Inherits="VSWebUI.Executive_Summary" %>
<%@ Register Src="~/Controls/StatusBox.ascx" TagName="StatusBox" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="shortcut icon" href="images/favicon.ico" />   
    <link rel="stylesheet" type="text/css" href="css/style.css" />
    <link rel="stylesheet" type="text/css" href="css/control.css" />
    <link rel="stylesheet" type="text/css" href="css/vswebforms.css" />
    <link rel="stylesheet" type="text/css" href="css/menu_style.css" />
   
</head>
<body id="Body1" runat="server">
    <form id="form1" runat="server">
    <div class="wrapper">
    <div style="width: 100%; padding-left: 5px">
            <table width="100%">
                <tr>                    
                    <td>
                        <div style="padding-left:10px; padding-top:20px">
                            <asp:Image ID="Vslogo" runat="server" ImageUrl="~/images/menulogo.png" />
                            
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    <div id="maindiv">
    <table><tr> <td style="padding-top:20px">
        <uc1:StatusBox ID="StatusBox1" runat="server" Button1CssClass="button1" 
                                        Button2CssClass="button2" 
                                        Button3CssClass="button3"
                                        Button4CssClass="button4" 
                                        ButtonCssClass="button" Height="100%" Label11CssClass="label11" Label11Text="20"
                                        Label12CssClass="label12" Label12Text="Not Responding" Label21CssClass="label11"
                                        Label21Text="10" Label22CssClass="label12" Label22Text="No Issues" Label31CssClass="label41"
                                        Label31Text="3" Label32CssClass="label42" Label32Text="Issues" Label41CssClass="label11"
                                        Label41Text="4" Label42CssClass="label12" Label42Text="In Maintenance" Title="All Servers"
                                        TitleCssClass="title"  TitleTableCssClass="titletbl"
                                        Width="300px" />
                                        </td></tr></table>
       
       </div>
    </div>
    <%--<div id="holder">--%>
        <div id="footer">
            <div id="leftnav">
                <p class="two">
                    Copyright 2012, RPR Wyatt, Inc. All rights reserved.</p>
            </div>
            <div id="rightnav">
                &nbsp;</div>
        </div>
    <%--</div>--%>
    </form>
</body>
</html>
