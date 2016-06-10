<%@ Page Title="VitalSigns Plus - CPU/Memory Health"  Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="CPUMemoryHealth.aspx.cs" Inherits="VSWebUI.Dashboard.CPUMemoryHealth" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%">
		<tr>
			<td>
				<div class="header" id="servernamelbldisp" runat="server">
					CPU / Memory Health
                </div>
			</td>
		</tr>
	</table>
	<table width="100%">
        <tr>
            <td>
                <dx:ASPxGridView ID="CPUMemGrid" runat="server" AutoGenerateColumns="False" 
                    EnableTheming="True" Theme="Office2003Blue" 
                    onhtmldatacellprepared="CPUMemGrid_HtmlDataCellPrepared" 
                    onpagesizechanged="CPUMemGrid_PageSizeChanged" Width="100%">
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="Server Name" FieldName="servername" 
                            VisibleIndex="1" Width="160px">
                            <Settings AllowAutoFilter="True" AllowHeaderFilter="True" 
                                AutoFilterCondition="Contains" HeaderFilterMode="CheckedList" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Memory Utilization" FieldName="memdisp" 
                            VisibleIndex="2">
                            <Settings AllowAutoFilter="False" AllowHeaderFilter="False" />
                            <HeaderStyle CssClass="GridCssHeader2" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="CPU Utilization" FieldName="cpudisp" 
                            VisibleIndex="6">
                            <Settings AllowAutoFilter="False" AllowHeaderFilter="False" />
                            <HeaderStyle CssClass="GridCssHeader2" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" Visible="False" 
                            VisibleIndex="10">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Server Type" FieldName="servertype" 
                            VisibleIndex="0">
                            <Settings AllowAutoFilter="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Memory" FieldName="Memory" Visible="False" 
                            VisibleIndex="11">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Memory Threshold" 
                            FieldName="Memory_Threshold" Visible="False" VisibleIndex="12">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="CPU" FieldName="CPU" Visible="False" 
                            VisibleIndex="13">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="CPU Threshold" FieldName="CPU_Threshold" 
                            Visible="False" VisibleIndex="14">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Current Memory" FieldName="memory" 
                            Visible="False" VisibleIndex="3">
                            <Settings AllowAutoFilter="False" />
                            <HeaderStyle CssClass="GridCssHeader2" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Average Memory" FieldName="avgmemory" 
                            VisibleIndex="4">
                            <Settings AllowAutoFilter="False" />
                            <HeaderStyle CssClass="GridCssHeader2" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Peak Memory" FieldName="maxmemory" 
                            VisibleIndex="5">
                            <Settings AllowAutoFilter="False" />
                            <HeaderStyle CssClass="GridCssHeader2" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Current CPU" FieldName="cpu" 
                            Visible="False" VisibleIndex="7">
                            <Settings AllowAutoFilter="False" />
                            <HeaderStyle CssClass="GridCssHeader2" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Average CPU" FieldName="avgcpu" 
                            VisibleIndex="8">
                            <Settings AllowAutoFilter="False" />
                            <HeaderStyle CssClass="GridCssHeader2" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Peak CPU" FieldName="maxcpu" 
                            VisibleIndex="9">
                            <Settings AllowAutoFilter="False" />
                            <HeaderStyle CssClass="GridCssHeader2" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsBehavior AutoExpandAllGroups="True" />
                    <SettingsPager AlwaysShowPager="True" PageSize="20">
                        <PageSizeItemSettings Visible="True">
                        </PageSizeItemSettings>
                    </SettingsPager>
                    <Settings GroupFormat="{1}" ShowFilterRow="True" />
                    <Styles>
                        <AlternatingRow CssClass="GridAltRow">
                        </AlternatingRow>
                        <Header CssClass="GridCssHeader">
                        </Header>
                        <Cell CssClass="GridCss">
                        </Cell>
                    </Styles>
                </dx:ASPxGridView>
            </td>
        </tr>
    </table>
</asp:Content>
