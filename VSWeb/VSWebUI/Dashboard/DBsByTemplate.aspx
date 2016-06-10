<%@ Page Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="DBsByTemplate.aspx.cs" Inherits="VSWebUI.Dashboard.DBsByTemplate" %>
<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%">
    <tr>
        <td>
            <div class="header" id="servernamelbldisp" runat="server">Databases by Inherited Template</div>
        </td>
        <td>
            &nbsp;
        </td>    
        <td align="right">
            <table>
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
        </td>        
    </tr>
</table>
<table class="tableWidth100Percent">
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="btnCollapse" runat="server" Text="Collapse All Rows" 
                                             CssClass="sysButton" OnClick="btnCollapse_Click">
                                            <Image Url="~/images/icons/forbidden.png">
                                            </Image>
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btnExpand" runat="server" Text="Expand All Rows" 
                                            Theme="Office2010Blue" OnClick="btnExpand_Click" Visible="false">
                                            <Image Url="~/images/icons/add.png">
                                            </Image>
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxGridView ID="ByTemplateGridView" runat="server" 
                                AutoGenerateColumns="False" EnableTheming="True" Theme="Office2003Blue" 
                                Width="100%" OnPageSizeChanged="ByTemplateGridView_PageSizeChanged">
                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="File Name" FieldName="FileName" 
                                        ShowInCustomizationForm="True" VisibleIndex="0">
                                       <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Title" FieldName="Title" 
                                        ShowInCustomizationForm="True" VisibleIndex="1">
                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Template" FieldName="DesignTemplateName" 
                                        ShowInCustomizationForm="True" VisibleIndex="2">
                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" 
                                        ShowInCustomizationForm="True" VisibleIndex="3">
                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Server" FieldName="Server" 
                                        ShowInCustomizationForm="True" VisibleIndex="4">
                                       <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Quota" FieldName="Quota" 
                                        ShowInCustomizationForm="True" VisibleIndex="5">
                                        <HeaderStyle CssClass="GridCssHeader2" />
                                        <CellStyle CssClass="GridCss2">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Scan Date" FieldName="ScanDate" 
                                        ShowInCustomizationForm="True" VisibleIndex="6">
                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="File Size" FieldName="FileSize" 
                                        ShowInCustomizationForm="True" VisibleIndex="8">
                                        <HeaderStyle CssClass="GridCssHeader2" />
                                        <CellStyle CssClass="GridCss2">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                                <SettingsBehavior AutoExpandAllGroups="True" />
                                <SettingsPager PageSize="50">
                                    <PageSizeItemSettings Visible="True">
                                    </PageSizeItemSettings>
                                </SettingsPager>
                                <Settings ShowFilterRow="True" />
                                <Styles>
                                    <AlternatingRow CssClass="GridAltRow" Enabled="True">
                                    </AlternatingRow>
                                    <GroupRow Font-Bold="True">
                                    </GroupRow>
                                </Styles>
                                <Templates>
                                    <GroupRowContent>
                                        <%# Container.GroupText%>
                                    </GroupRowContent>
                                </Templates>
                            </dx:ASPxGridView>
                        </td>
                    </tr>
                </table>
</asp:Content>
