<%@ Page Title="" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="O365UserLicenseswithServices.aspx.cs" Inherits="VSWebUI.Dashboard.O365UserLicenseswithServices" %>
<%@ MasterType VirtualPath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts" TagPrefix="cc2" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table>
		<tr>
			<td>
				<div class="header" id="servernamelbldisp" runat="server">
					Office365 Users with Licences and Services</div>
			</td>
		</tr>
	</table>
	<table width="100%">
<tr>
<td>
	<dx:ASPxGridView ID="UserLicensesgrid" runat="server" 
		AutoGenerateColumns="True" KeyFieldName = "ServerId"  
					EnableTheming="True" Theme="Office2003Blue" 
		onhtmldatacellprepared="UserLicensesgrid_HtmlDataCellPrepared" onpagesizechanged="UserLicensesgrid_PageSizeChanged" 
					>
					
					<SettingsBehavior AllowFocusedRow="True" 
						AllowSelectSingleRowOnly="True" AllowSort="False" />
					<%--	<Settings ShowFilterRow="True" />--%>
					<SettingsPager AlwaysShowPager="True" SEOFriendly="Enabled" >
						<PageSizeItemSettings Visible="True">
						</PageSizeItemSettings>
					</SettingsPager>
				<%--	<Settings HorizontalScrollBarMode="Auto" />--%>
					<Styles>
						<Header CssClass="GridCssHeader">
						</Header>
						<AlternatingRow CssClass="GridAltRow">
						</AlternatingRow>
						<Cell CssClass="GridCss">
						</Cell>
					</Styles>
				</dx:ASPxGridView>
</td>
</tr>
</table>
</asp:Content>
