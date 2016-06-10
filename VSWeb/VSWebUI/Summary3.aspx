<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Summary3.aspx.cs" Inherits="VSWebUI.Summary3" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>




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
   
    <style type="text/css">
        .style43
        {
            font-weight: 700;
            color: #000000;
        }
    </style>
   
</head>
<body id="Body1" runat="server">
    <form id="form1" runat="server">
    <div class="wrapper">
    <div style="width: 100%; padding-left: 7px">
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
    <table><tr> <td style="padding-top:0px">
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
       <table width="100%"><tr><td valign="top" align="left" style="padding-left:20px;">
        <table border="1" style="border-collapse: collapse;">
            <tr>
                <td style="padding-right:5px;">
                <asp:Label ID="Typelbl" runat="server" CssClass="style43" Text='Domino'></asp:Label><br />
                                  <dx:ASPxGridView ID="ASPxGridView1" runat="server"
                              OnHtmlDataCellPrepared="ASPxGridView1_HtmlDataCellPrepared" 
                                    AutoGenerateColumns="False"  Width="295px" >
                     
                              <Columns>
                              <dx:GridViewDataImageColumn Caption=" " FieldName="imgsource" Width="10px"></dx:GridViewDataImageColumn>
                              <dx:GridViewDataTextColumn Caption="Server Name" FieldName="Name"></dx:GridViewDataTextColumn>
                              <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" Width="8px"  Visible="false">                             
                              </dx:GridViewDataTextColumn>
                               <dx:GridViewDataTextColumn Caption=" " FieldName="" Width="10px"></dx:GridViewDataTextColumn>
                              </Columns>
                                <SettingsPager Mode="ShowAllRecords">
                                    <PageSizeItemSettings Caption="">
                                    </PageSizeItemSettings>
                                </SettingsPager>
                                <Settings ShowColumnHeaders="False" />
                                </dx:ASPxGridView>
                </td>
            </tr>
        </table>
       </td><td valign="top" align="left">
       <dx:ASPxDataView ID="ASPxDataView1" runat="server" AllowPaging="False" 
                            Layout="Flow" ondatabound="ASPxDataView1_DataBound" 
            BackColor="#F8F8C0" >
                            <Paddings PaddingLeft="25px" />
                            <ContentStyle BackColor="#f8f0c0" />
       
                            <ItemTemplate>
                            <table><tr><td>
                                <asp:Label ID="Typelbl" runat="server" CssClass="style43" Text='<%#Eval("Type")%>'></asp:Label>
                            </td></tr>
                            <tr><td>
                            <dx:ASPxGridView ID="ASPxGridView1" runat="server"
                              OnHtmlDataCellPrepared="ASPxGridView1_HtmlDataCellPrepared" 
                                    AutoGenerateColumns="False"  Width="295px" >
                     
                              <Columns>
                              <dx:GridViewDataImageColumn Caption=" " FieldName="imgsource" Width="10px"></dx:GridViewDataImageColumn>
                              <dx:GridViewDataTextColumn Caption="Server Name" FieldName="Name"></dx:GridViewDataTextColumn>
                              <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" Width="8px"  Visible="false">                             
                              </dx:GridViewDataTextColumn>
                               <dx:GridViewDataTextColumn Caption=" " FieldName="" Width="10px"></dx:GridViewDataTextColumn>
                              </Columns>
                                <SettingsPager PageSize="5">
                                    <PageSizeItemSettings Caption="">
                                    </PageSizeItemSettings>
                                </SettingsPager>
                                <Settings ShowColumnHeaders="False" />
                                </dx:ASPxGridView>
                            </td></tr>
                            </table>                             
                                
                            </ItemTemplate>
                            <Paddings Padding="0px"></Paddings>
                            <ContentStyle BackColor="Transparent">
                            </ContentStyle>
                            <ItemStyle Width="305px"  Height="175px" BackColor="#F8F8C0">
                                <Paddings Padding="0px" />
                                <Paddings Padding="0px"></Paddings>
                            <Border BorderWidth="1px" />
                            </ItemStyle>
                            <Border BorderWidth="0px" BorderColor="#F8F8C0"></Border>
                        </dx:ASPxDataView>
                        </td></tr></table>
    </div>
    <%--  <Settings ShowVerticalScrollBar="true"/>--%>
        <div id="footer">
            <div id="leftnav">
                <p class="two">
                    Copyright 2012, RPR Wyatt, Inc. All rights reserved.</p>
            </div>
            <div id="rightnav">
                &nbsp;</div>
        </div>
    <%--<div id="holder">--%>
    </form>
</body>
</html>
