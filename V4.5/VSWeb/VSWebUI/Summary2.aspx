<%@ Page Title="" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="Summary2.aspx.cs" Inherits="VSWebUI.WebForm11" %>
<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>




<%@ Register Src="~/Controls/StatusBox.ascx" TagName="StatusBox" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<br />
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
                     <Settings ShowColumnHeaders="false" />
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
                                </dx:ASPxGridView>
                            </td></tr>
                            </table>                             
                                
                            </ItemTemplate>
                            <Paddings Padding="0px"></Paddings>
                            <ContentStyle BackColor="Transparent">
                            </ContentStyle>
                            <ItemStyle Width="300px"  Height="200px" BackColor="#F8F8C0">
                                <Paddings Padding="0px" />
                                <Paddings Padding="0px"></Paddings>
                            <Border BorderWidth="1px" />
                            </ItemStyle>
                            <Border BorderWidth="0px" BorderColor="#F8F8C0"></Border>
                        </dx:ASPxDataView>
</asp:Content>
