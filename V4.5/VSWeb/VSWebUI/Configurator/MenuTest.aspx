<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="MenuTest.aspx.cs"
    Inherits="VSWebUI.MenuTest" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>


<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="height: 480px;">
        <dx:ASPxMenu BorderBetweenItemAndSubMenu="HideRootOnly" EnableViewState="False" EnableClientSideAPI="true"
            ID="ASPxMenu1" AutoSeparators="None" runat="server" ShowSubMenuShadow="true"
            ItemImagePosition="Right" GutterWidth="0px" ShowPopOutImages="True" 
            GutterColor="#FFFF99">
            <Items>
                <dx:MenuItem Text="SubMenu Template" Name="submenu">
                   <SubMenuStyle Paddings-Padding="8px" Font-Size="10px" Font-Names="Verdana" Wrap="False"
                        ForeColor="#000000" >
<Paddings Padding="8px"></Paddings>
                    </SubMenuStyle>
                    <Items>
                     <dx:MenuItem Text="ASP.NET" Name="aspdotnet">
                            <Template>
                             <table width="900px">
                            <tr>
                                <td>
                                    <a href=""><dx:ASPxLabel ID="ASPxLabel1" EnableViewState="False" runat="server" Text="News"
                                        Font-Size="12px" ForeColor="#003399"  /></a>
                                </td>
                                <td>
                                    <a href=""><dx:ASPxLabel ID="ASPxLabel2" EnableViewState="False" runat="server" Text="News"
                                        Font-Size="12px" ForeColor="#003399"  /></a>
                                </td>
                                <td>
                                    <a href=""><dx:ASPxLabel ID="ASPxLabel3" EnableViewState="False" runat="server" Text="News"
                                        Font-Size="12px" ForeColor="#003399"  /></a>
                                </td>
                                <td>
                                    <a href=""><dx:ASPxLabel ID="ASPxLabel4" EnableViewState="False" runat="server" Text="News"
                                        Font-Size="12px" ForeColor="#003399"  /></a>
                                </td>
                                <td>
                                    <a href=""><dx:ASPxLabel ID="ASPxLabel5" EnableViewState="False" runat="server" Text="News"
                                        Font-Size="12px" ForeColor="#003399"  /></a>
                                </td>
                                <td>
                                    <a href=""><dx:ASPxLabel ID="ASPxLabel6" EnableViewState="False" runat="server" Text="News"
                                        Font-Size="12px" ForeColor="#003399"  /></a>
                                </td>
                                <td>
                                    <a href=""><dx:ASPxLabel ID="ASPxLabel7" EnableViewState="False" runat="server" Text="News"
                                        Font-Size="12px" ForeColor="#003399"  /></a>
                                </td>
                                
                            </tr>
                        </table>
                             </Template>
                         </dx:MenuItem>
                          </Items>
                </dx:MenuItem>
                   <dx:MenuItem Text="SubMenu Template2" Name="submenu">
                   <SubMenuStyle Paddings-Padding="8px" Font-Size="10px" Font-Names="Verdana" Wrap="False"
                        ForeColor="#000000" >
<Paddings Padding="8px"></Paddings>
                       </SubMenuStyle>
                    <Items>
                     <dx:MenuItem Text="ASP.NET" Name="aspdotnet">
                            <Template>
                             <table width="900px" >
                            <tr>
                               
                                <td>
                                    <a href=""><dx:ASPxLabel ID="ASPxLabel3" EnableViewState="False" runat="server" Text="2ndNews"
                                        Font-Size="12px" ForeColor="#003399"  /></a>
                                </td>
                                <td>
                                    <a href=""><dx:ASPxLabel ID="ASPxLabel4" EnableViewState="False" runat="server" Text="2ndNews"
                                        Font-Size="12px" ForeColor="#003399"  /></a>
                                </td>
                                <td>
                                    <a href=""><dx:ASPxLabel ID="ASPxLabel5" EnableViewState="False" runat="server" Text="2ndNews"
                                        Font-Size="12px" ForeColor="#003399"  /></a>
                                </td>
                                <td>
                                    <a href=""><dx:ASPxLabel ID="ASPxLabel6" EnableViewState="False" runat="server" Text="2ndNews"
                                        Font-Size="12px" ForeColor="#003399"  /></a>
                                </td>
                                <td>
                                    <a href=""><dx:ASPxLabel ID="ASPxLabel7" EnableViewState="False" runat="server" Text="2ndNews"
                                        Font-Size="12px" ForeColor="#003399"  /></a>
                                </td>
                                
                            </tr>
                        </table>
                             </Template>
                         </dx:MenuItem>
                          </Items>
                </dx:MenuItem>
            </Items>
        </dx:ASPxMenu>
    </div>
</asp:Content>
