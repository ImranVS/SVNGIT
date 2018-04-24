<%@ Page Title="VitalSigns Plus -Top Mail Users" Language="C#" MasterPageFile="~/DashboardSite.Master"
	AutoEventWireup="true" CodeBehind="TopMailUsers.aspx.cs" Inherits="VSWebUI.TopMailUsers" %>

<%@ MasterType VirtualPath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraCharts" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script type="text/javascript">
		function Resized() {
			var callbackState = document.getElementById('ContentPlaceHolder1_callbackState').value;

			if (callbackState == 0)
				DoCallback();
		}

		function DoCallback() {
			document.getElementById('ContentPlaceHolder1_chartWidth').value = document.body.offsetWidth - 105;
			//cSelectServerRoundPanel.style.width = document.getElementById('ContentPlaceHolder1__chartWidth').value + "px";
			CBigmail.PerformCallback();

		}

		function ResizeChart(s, e) {
			document.getElementById('ContentPlaceHolder1_callbackState').value = 0;
			s.GetMainElement().style.width = document.getElementById('ContentPlaceHolder1__chartWidth').value + "px";
			//cSelectServerRoundPanel.style.width = document.getElementById('ContentPlaceHolder1__chartWidth').value + "px";
		}

		function ResetCallbackState() {
			window.document.form1.elements['ContentPlaceHolder1_callbackState'].value = 1;
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<input id="chartWidth" type="hidden" runat="server" value="400" />
	<input id="callbackState" type="hidden" runat="server" value="0" />
    <table>
					<tr>
						<td>
							<div class="header" id="titleDiv" runat="server">
								Today's Top 10 Mail Users</div>
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
					<tr>
						<td>
							<dx:ASPxComboBox ID="ServerComboBox" runat="server" ValueType="System.String" AutoPostBack="True"
								  onselectedindexchanged="ServerComboBox_SelectedIndexChanged">
							</dx:ASPxComboBox>
						</td>
						<td>
							<dx:ASPxRadioButtonList ID="CountorSizeRadioButtonList" runat="server" AutoPostBack="True"
								SelectedIndex="0" OnSelectedIndexChanged="CountorSizeRadioButtonList_SelectedIndexChanged"
								RepeatDirection="Horizontal">
								<Items>
									<dx:ListEditItem Text="Count" Value="Count" />
									<dx:ListEditItem Text="Size" Value="Size" />
								</Items>
							</dx:ASPxRadioButtonList>
						</td>
						<td>
							<dx:ASPxRadioButtonList ID="SentorReceivedRadioButtonList" runat="server" AutoPostBack="True"
								SelectedIndex="0" OnSelectedIndexChanged="SentorReceivedRadioButtonList_SelectedIndexChanged"
								RepeatDirection="Horizontal">
								<Items>
									<dx:ListEditItem Text="Sent" Value="Sent" />
									<dx:ListEditItem Text="Received" Value="Received" />
								</Items>
							</dx:ASPxRadioButtonList>
						</td>
					</tr>
				</table>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
					<ContentTemplate>
						<dxchartsui:WebChartControl ID="TopMailChartControl" runat="server" OnCustomCallback="TopMailChartControl_CustomCallback"
							Height="400px" Width="1030px" ClientInstanceName="CBigmail"
							CrosshairEnabled="True" onbounddatachanged="TopMailChartControl_BoundDataChanged" 
                            PaletteName="Oriel">
							<diagramserializable>
                            <cc1:XYDiagram>
                                <axisx visibleinpanesserializable="-1">
                                </axisx>
                                <axisy visibleinpanesserializable="-1">
                                    <label>
                                    <resolveoverlappingoptions allowrotate="False" />
                                    </label>
                                </axisy>
                            </cc1:XYDiagram>
                        </diagramserializable>
							<legend visibility="False"></legend>
							<seriesserializable>
                            <cc1:Series Name="Series 1" ArgumentScaleType="Qualitative">
                            </cc1:Series>
                            <cc1:Series Name="Series 2" ArgumentScaleType="Qualitative">
                            </cc1:Series>
                        </seriesserializable>
						</dxchartsui:WebChartControl>
					</ContentTemplate>
					<Triggers>
						<asp:AsyncPostBackTrigger ControlID="ServerComboBox" />
                        <asp:AsyncPostBackTrigger ControlID="CountorSizeRadioButtonList" />
                        <asp:AsyncPostBackTrigger ControlID="SentorReceivedRadioButtonList" />
					</Triggers>
				</asp:UpdatePanel>
	
</asp:Content>
