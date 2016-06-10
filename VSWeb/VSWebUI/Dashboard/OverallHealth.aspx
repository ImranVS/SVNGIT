<%@ Page Title="" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true"
    CodeBehind="OverallHealth.aspx.cs" Inherits="VSWebUI.Dashboard.OverallHealth" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="../Controls/StatusBox.ascx" TagName="StatusBox" TagPrefix="uc1" %>


<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
        CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
        HeaderText="Overall Health" Width="700px">
        <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
        <ContentPaddings PaddingTop="10px" PaddingBottom="10px" Padding="2px"></ContentPaddings>
        <HeaderStyle Height="23px">
            <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
            <Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
        </HeaderStyle>
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                <table style="width: 98%;">
                    <tr>
                        <td colspan="3">
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <img id="imgButton" alt="" src="../images/viewselector.png" style="width: 103px;
                                            height: 28px;" />
                                        <dx:ASPxPopupControl ClientInstanceName="ASPxPopupClientControl" Width="150px" Height="165px"
                                            MaxWidth="800px" MaxHeight="800px" MinHeight="150px" MinWidth="150px" ID="pcMain"
                                            PopupElementID="imgButton" HeaderText="View Selector" runat="server" PopupHorizontalAlign="LeftSides"
                                            PopupVerticalAlign="Below" EnableHierarchyRecreation="True">
                                            <ContentCollection>
                                                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                                                    <asp:Panel ID="Panel1" runat="server">
                                                        <table border="0" cellpadding="4" cellspacing="0">
                                                            <tr>
                                                                <td valign="top" style="color: #666666; width: 110px;">
                                                                    <span><b>View by Location<br />
                                                                        &nbsp;
                                                                        <br />
                                                                        Filter by Type...<br />
                                                                        <br />
                                                                        My Servers<br />
                                                                        <br />
                                                                        All Servers<br />
                                                                    </b></span>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </dx:PopupControlContentControl>
                                            </ContentCollection>
                                        </dx:ASPxPopupControl>
                                    </td>
                                    <td>
                                    </td>
                                    <td align="right">
                                        <img id="imgbutton2" alt="" src="../images/filterselector.png" style="width: 103px;
                                            height: 28px;" />
                                        <dx:ASPxPopupControl ClientInstanceName="ASPxPopupClientControl" Width="150px" Height="165px"
                                            MaxWidth="800px" MaxHeight="800px" MinHeight="150px" MinWidth="150px" ID="ASPxPopupControl1"
                                            PopupElementID="imgbutton2" HeaderText="Filter by Type" runat="server" PopupHorizontalAlign="LeftSides"
                                            PopupVerticalAlign="Below" EnableHierarchyRecreation="True">
                                            <ContentCollection>
                                                <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                                                    <asp:Panel ID="Panel2" runat="server">
                                                        <table border="0" cellpadding="4" cellspacing="0">
                                                            <tr>
                                                                <td valign="top" style="color: #666666; width: 110px;" colspan="3">
                                                                    <span><b>
                                                                        <dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server" CheckState="Unchecked" Text="Domino">
                                                                        </dx:ASPxCheckBox>
                                                                        <br />
                                                                        <dx:ASPxCheckBox ID="ASPxCheckBox2" runat="server" CheckState="Unchecked" Text="BES">
                                                                        </dx:ASPxCheckBox>
                                                                        <br />
                                                                        <dx:ASPxCheckBox ID="ASPxCheckBox3" runat="server" CheckState="Unchecked" Text="Sharepoint">
                                                                        </dx:ASPxCheckBox>
                                                                        <br />
                                                                        <dx:ASPxCheckBox ID="ASPxCheckBox4" runat="server" CheckState="Unchecked" Text="Traveler">
                                                                        </dx:ASPxCheckBox>
                                                                    </b></span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Select All" Width="70px">
                                                                    </dx:ASPxButton>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxButton ID="ASPxButton2" runat="server" Text="Cancel">
                                                                    </dx:ASPxButton>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxButton ID="ASPxButton3" runat="server" Text="Save">
                                                                    </dx:ASPxButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </dx:PopupControlContentControl>
                                            </ContentCollection>
                                        </dx:ASPxPopupControl>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; vertical-align: top">
                            <uc1:StatusBox ID="StatusBox1" runat="server" Button1CssClass="button1" Button1Link="NewForm1.aspx?from=Form1"
                                Button2CssClass="button2" Button2Link="~/Dashboard/NewForm1.aspx?from=Form2"
                                Button3CssClass="button3" Button3Link="~/Dashboard/NewForm1.aspx?from=Form3"
                                Button4CssClass="button4" Button4Link="~/Dashboard/NewForm1.aspx?from=Form4"
                                ButtonCssClass="button" Height="75px" 
                                Label11CssClass="label11" Label11Text="0" Label12CssClass="label12" Label12Text="Not Responding" 
                                Label21CssClass="label11" Label21Text="0" Label22CssClass="label12" Label22Text="No Issues" 
                                Label31CssClass="label41" Label31Text="0" Label32CssClass="label42" Label32Text="Issues" 
                                Label41CssClass="label11" Label41Text="0" Label42CssClass="label12" Label42Text="In Maintenance" 
                                Title="All Servers" TitleCssClass="title" TitleLink="~/Dashboard/StatusList.aspx" TitleTableCssClass="titletbl"
                                Width="300px" />
                        </td>
                        <td>
                        </td>
                        <td style="text-align: right; vertical-align: top" align="right">
                               <uc1:StatusBox ID="StatusBox2" runat="server" Button1CssClass="button1" Button1Link="NewForm1.aspx?from=Form1"
                                Button2CssClass="button2" Button2Link="~/Dashboard/NewForm1.aspx?from=Form2"
                                Button3CssClass="button3" Button3Link="~/Dashboard/NewForm1.aspx?from=Form3"
                                Button4CssClass="button4" Button4Link="~/Dashboard/NewForm1.aspx?from=Form4"
                                ButtonCssClass="button" Height="75px" 
                                Label11CssClass="label11" Label11Text="0" Label12CssClass="label12" Label12Text="Not Responding" 
                                Label21CssClass="label11" Label21Text="0" Label22CssClass="label12" Label22Text="No Issues" 
                                Label31CssClass="label41" Label31Text="0" Label32CssClass="label42" Label32Text="Issues" 
                                Label41CssClass="label11" Label41Text="0" Label42CssClass="label12" Label42Text="In Maintenance" 
                                Title="All Servers" TitleCssClass="title" TitleLink="~/Dashboard/StatusList.aspx" TitleTableCssClass="titletbl"
                                Width="300px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; vertical-align: top">
                            <uc1:StatusBox ID="StatusBox3" runat="server" Height="75px" Title="All Servers" TitleCssClass="title"
                                Width="300px" TitleTableCssClass="titletbl" Button1CssClass="button1" Button2CssClass="button2"
                                Button3CssClass="button3" Button4CssClass="button4" ButtonCssClass="button" 
                                Label11CssClass="label11" Label11Text="0" Label12CssClass="label12" Label12Text="Not Responding" 
                                Label21CssClass="label11" Label21Text="0" Label22CssClass="label12" Label22Text="No Issues" 
                                Label31CssClass="label41" Label31Text="0" Label32CssClass="label42" Label32Text="Issues" 
                                Label41CssClass="label11" Label41Text="0" Label42CssClass="label12" Label42Text="In Maintenance" 
                                Button4Link="~/Dashboard/NewForm1.aspx?from=Form4" TitleLink="~/Dashboard/StatusList.aspx"
                                 />
                        </td>
                        <td>
                        </td>
                        <td style="text-align: right; vertical-align: top">
                            <uc1:StatusBox ID="StatusBox4" runat="server" Height="75px" TitleCssClass="title"
                                Width="300px" TitleTableCssClass="titletbl" Button1CssClass="button1" Button2CssClass="button2"
                                Button3CssClass="button3" Button4CssClass="button4" ButtonCssClass="button" 
                                Label11CssClass="label11" Label11Text="0" Label12CssClass="label12" Label12Text="Not Responding" 
                                Label21CssClass="label11" Label21Text="0" Label22CssClass="label12" Label22Text="No Issues" 
                                Label31CssClass="label41" Label31Text="0" Label32CssClass="label42" Label32Text="Issues" 
                                Label41CssClass="label11" Label41Text="0" Label42CssClass="label12" Label42Text="In Maintenance" 
                                Title="All Servers" Button4Link="~/Dashboard/NewForm1.aspx?from=Form4" TitleLink="~/Dashboard/StatusList.aspx"
                                 />
                        </td>
                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
</asp:Content>
