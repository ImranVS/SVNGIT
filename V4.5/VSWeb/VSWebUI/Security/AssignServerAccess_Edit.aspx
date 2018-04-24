<%@ Page Language="C#" AutoEventWireup="true" Title="VitalSigns Plus - Assign Server Access" MasterPageFile="~/Site1.Master" CodeBehind="AssignServerAccess_Edit.aspx.cs" Inherits="VSWebUI.Security.AssignServerAccess_Edit" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ MasterType virtualpath="~/Site1.Master" %>

<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
        <tr>
            <td>
                <div class="header" id="servernamelbldisp" runat="server">Server Access for </div>
            </td>
        </tr>
        <tr>
            <td>
                <div class="info">Server access applies to VitalSigns Configurator only and has no bearing on the Dashboard. The grid below describes server access for the selected user. The user has access to all servers/locations selected in the grid. 
                The user does NOT have access to any server/location that is de-selected.</div>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxButton ID="CollapseAllButton" runat="server" CssClass="sysButton" 
                    Text="Collapse All" onclick="CollapseAllButton_Click">
                    <Image Url="~/images/icons/forbidden.png">
                    </Image>
                </dx:ASPxButton>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxTreeList ID="ServerAccessTreeList" runat="server" 
                    Theme="Office2003Blue" AutoGenerateColumns="False" KeyFieldName="Id" 
                    ParentFieldName="LocId" 
                    onpagesizechanged="ServerAccessTreeList_PageSizeChanged" OnDataBound="DataBound">
                    <Columns>
                        <dx:TreeListTextColumn Caption="Server" FieldName="Name" VisibleIndex="0" 
                            Name="Name">
                            <HeaderStyle CssClass="GridCssHeader" />
							<CellStyle CssClass="GridCss">
							</CellStyle>
                        </dx:TreeListTextColumn>
                        <dx:TreeListTextColumn FieldName="ServerType" VisibleIndex="1" 
                            Name="ServerType">
                            <HeaderStyle CssClass="GridCssHeader" />
							<CellStyle CssClass="GridCss">
							</CellStyle>
                        </dx:TreeListTextColumn>
                        <dx:TreeListTextColumn FieldName="Description" VisibleIndex="2">
                            <HeaderStyle CssClass="GridCssHeader" />
							<CellStyle CssClass="GridCss">
							</CellStyle>
                        </dx:TreeListTextColumn>
                        <dx:TreeListTextColumn Caption="locid" FieldName="LocId" Visible="False" 
                            VisibleIndex="3" Name="locid">
                        </dx:TreeListTextColumn>
                        <dx:TreeListTextColumn Caption="actid" FieldName="ID" Name="actid" 
                            Visible="False" VisibleIndex="4">
                        </dx:TreeListTextColumn>
                        <dx:TreeListTextColumn Caption="srvtypeid" FieldName="srvtypeid" 
                            Name="srvtypeid" Visible="False" VisibleIndex="5">
                        </dx:TreeListTextColumn>
                    </Columns>
                      <Summary>
                         <dx:TreeListSummaryItem   FieldName="Name" SummaryType="Count" ShowInColumn="Name" DisplayFormat="{0} Item(s)" /></Summary>
                    <Settings GridLines="Both" />
															<SettingsBehavior AllowDragDrop="False" AutoExpandAllNodes="True" />
															<SettingsPager Mode="ShowPager" 
                        PageSize="50">
																<PageSizeItemSettings Visible="True">
																</PageSizeItemSettings>
															</SettingsPager>
															<SettingsSelection AllowSelectAll="True" Enabled="True" Recursive="True" />
															<Styles>
																<AlternatingNode Enabled="True">
																</AlternatingNode>
															</Styles>
                </dx:ASPxTreeList>
            </td>
        </tr>
	<%--	1/22/2016 Durga modified for VSPLUS--2516--%>
        <tr>
            <td>
                <div id="errorDiv" class="alert alert-danger" runat="server" style="display: none">
					Error.
				</div>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <dx:ASPxButton ID="AssignServerAccessButton" runat="server" 
                                CssClass="sysButton" Text="Assign" onclick="AssignServerAccessButton_Click">
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="ResetServerAccessButton" runat="server" CssClass="sysButton" 
                                Text="Reset" onclick="ResetServerAccessButton_Click">
                            </dx:ASPxButton>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <dx:ASPxButton ID="CancelButton" runat="server" CssClass="sysButton" 
                                Text="Cancel" onclick="CancelButton_Click">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>