<%@ Page Title="VitalSigns Plus - AdHocRpt" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="AdHocRpt.aspx.cs" Inherits="VSWebUI.DashboardReports.AdHocRpt" %>
<%@ MasterType virtualpath="~/Reports.Master" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.XtraReports.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Controls/DateRange.ascx" TagName="DateRange" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <style type="text/css">
    .tdpadded
    {
        padding-left:20px;
    }
   </style>
   <script type="text/javascript">
       function OnItemClick(s, e) {
           if (e.item.parent == s.GetRootItem())
               e.processOnServer = false;
       }
       function SelectionChangedSiteControl(s, e) {
           if (s.GetValue() == '0') {
               menu1.SetVisible(true);
           }
           else if (s.GetValue() == '1') {
               menu1.SetVisible(false);
           }
       }
   </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%">
        <tr>
            <td class="tdpadded" valign="top">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table> 
                    <tr>
                        <td>
                            <dx:ASPxButton ID="SubmitBtn" runat="server" Text="Submit" 
                                CssClass="sysButton">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                </table>        
                    </ContentTemplate>
                </asp:UpdatePanel>
                <dx:ASPxButton ID="ReptBtn" runat="server" Text="Reports" 
                                Theme="Office2010Blue" onclick="ReptBtn_Click" Visible="False">
                            </dx:ASPxButton>
                <table>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <dx:ASPxRadioButtonList ID="OptimizedRadioButtonList" runat="server" 
                                SelectedIndex="0" AutoPostBack="True">
                                <ClientSideEvents SelectedIndexChanged="SelectionChangedSiteControl" />
                                <Items>
                                    <dx:ListEditItem Selected="True" Text="Optimized for Screen" Value="0" />
                                    <dx:ListEditItem Text="Optimized for Printing" Value="1" />
                                </Items>
                            </dx:ASPxRadioButtonList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            
                            </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Statistic Type:" 
                                CssClass="lblsmallFont">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxComboBox ID="StatTypeComboBox" runat="server" AutoPostBack="True" 
                                onselectedindexchanged="StatTypeComboBox_SelectedIndexChanged" 
                                SelectedIndex="0" Theme="Default">
                                <Items>
                                    <dx:ListEditItem Text="Connections" Value="Connections" />
                                    <dx:ListEditItem Selected="True" Text="Domino" Value="Domino" />
                                    <dx:ListEditItem Text="Exchange" Value="Exchange" />
                                </Items>
                            </dx:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" 
                                Text="Statistic:" CssClass="lblsmallFont">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <dx:ASPxComboBox ID="StatComboBox" runat="server" ValueType="System.String" 
                                        Theme="Default">
                                    </dx:ASPxComboBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="StatTypeComboBox" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Statistic Summary Type:" 
                                CssClass="lblsmallFont">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxComboBox ID="SummaryTypeComboBox" runat="server" SelectedIndex="1" 
                                Theme="Default">
                                <Items>
                                    <dx:ListEditItem Text="Total" Value="0" />
                                    <dx:ListEditItem Selected="True" Text="Average" Value="1" />
                                </Items>
                            </dx:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                            </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Date Range:" 
                                CssClass="lblsmallFont">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <uc1:DateRange ID="dtPick" runat="server" Width="100px" Height="100%"></uc1:DateRange>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                
                <table width="100%">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="header" id="titledisp" runat="server" style="display: block">Report for &lt;statistic&gt; </div>
                                     </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="OptimizedRadioButtonList" />
                                            <asp:AsyncPostBackTrigger ControlID="SubmitBtn" />
                                        </Triggers>
                                    </asp:UpdatePanel>   
                                </td>
                                <td align="right">
                                    <dx:ASPxMenu ID="ASPxMenu1" ClientInstanceName="menu1" runat="server" HorizontalAlign="Right" 
                                        ShowAsToolbar="True" Theme="Moderno" onitemclick="ASPxMenu1_ItemClick">
                                        <ClientSideEvents ItemClick="OnItemClick" />
                                        <Items>
                        <dx:MenuItem Name="MainItem">
                            <Items>
                                <dx:MenuItem Name="ExportXLSItem" Text="Export to XLS">
                                </dx:MenuItem>
                                <dx:MenuItem Name="ExportXLSXItem" Text="Export to XLSX">
                                </dx:MenuItem>
                                <dx:MenuItem Name="ExportPDFItem" Text="Export to PDF">
                                </dx:MenuItem>
                                
                            </Items>
                            <Image Url="~/images/icons/Gear.png">
                            </Image>
                        </dx:MenuItem>
                    </Items>
                                    </dx:ASPxMenu>
                                </td>
                            </tr>
                        </table>
                        
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td>
                                    <dx:ASPxPivotGrid ID="ASPxPivotGrid1" runat="server" Theme="Office2003Blue"  
                            Width="800px">
                            <OptionsView HorizontalScrollBarMode="Auto" ></OptionsView>
                            <OptionsPager AlwaysShowPager="True">
                            </OptionsPager>
						
                </dx:ASPxPivotGrid>
                                </td>
                            </tr>
                        </table>
                <table>
                    <tr>
                        <td>
                            <dx:ASPxDocumentViewer ID="ASPxDocumentViewer1" runat="server" Width="910px" Theme="Moderno"   SettingsSplitter-SidePaneVisible="false">
                            </dx:ASPxDocumentViewer>
        <dx:ASPxPivotGridExporter ID="ServerPivotGridExporter" ASPxPivotGridID="ASPxPivotGrid1" runat="server">
                                    </dx:ASPxPivotGridExporter>
                        </td>
                    </tr>
                </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="OptimizedRadioButtonList" />
                        <asp:AsyncPostBackTrigger ControlID="SubmitBtn" />
                        <asp:PostBackTrigger ControlID="ASPxMenu1" />
                    </Triggers>
                </asp:UpdatePanel>
                        
            </td>
        </tr>
    </table>
</asp:Content>
