<%@ Page Title="VitalSigns Plus - Biggest Mail Files" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="BiggestExchangeMailFiles.aspx.cs" Inherits="VSWebUI.BiggestExchangeMailFiles" %>
<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>









<%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>
<%@ Register assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts.Web" tagprefix="dxchartsui" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        
        
      
    </style>

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
        CItemSizeServer.PerformCallback();
        CMBSizeDatabase.PerformCallback();
        CItemSizeDatabase.PerformCallback();  

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
		  <div class="header" id="titleDiv" runat="server">Biggest Mail Files for Exchange</div>
		<table>
		<tr>
		<td>
            <dx:ASPxLabel ID="TypeLabel" runat="server" CssClass="lblsmallFont" 
                Text="Type:" >
            </dx:ASPxLabel>
        </td>
        <td>
            <dx:ASPxComboBox ID="TypeComboBox" runat="server"  onselectedindexchanged="TypeComboBox_SelectedIndexChanged"  AutoPostBack="True" >
			<Items>
                    <dx:ListEditItem Text="Select Type" Value="0" />
                </Items>
            </dx:ASPxComboBox>
        </td>
		</tr>
		</table>
		 <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
        EnableTheming="True" Theme="Glass" Width="100%" EnableHierarchyRecreation="False">
        <TabPages>
            <dx:TabPage Text="Mailbox Size(MB) By Server">
                <TabImage Url="~/images/icons/email.png">
                </TabImage>
                <ContentCollection>
                    <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
					 <table class="style1">
        <tr>
            <td>   
			<table> 
			<tr>
			<td>
			
			<dx:ASPxComboBox ID="ServerComboBox" runat="server" ValueType="System.String" 
                        AutoPostBack="True" 
                        onselectedindexchanged="ServerComboBox_SelectedIndexChanged">
                    </dx:ASPxComboBox>
			</td>
			
			</tr>
			</table>              
                </td>
              
        </tr>
        <tr>
            <td width="100%">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <dxchartsui:WebChartControl ID="BigMailChartControl" runat="server" 
                    oncustomcallback="BigMailChartControl_CustomCallback" Height="600px" 
                    Width="1030px" ClientInstanceName="CBigmail" 
                        AppearanceNameSerializable="Chameleon" CrosshairEnabled="True">
                    </dxchartsui:WebChartControl>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ServerComboBox" />
                </Triggers>
                </asp:UpdatePanel>
                
                    </td>
        </tr>
    </table>
					</dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>

			<dx:TabPage Text="Message Count By Server">
                <TabImage Url="~/images/icons/email.png">
                </TabImage>
                <ContentCollection>
                    <dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">
					 <table class="style1">
        <tr>
            <td>              
                    <dx:ASPxComboBox ID="ItemSizeServercombobox" runat="server" ValueType="System.String" 
                        AutoPostBack="True" 
                        onselectedindexchanged="ItemSizeServercombobox_SelectedIndexChanged">
                    </dx:ASPxComboBox>
                </td>
              
        </tr>
        <tr>
            <td width="100%">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <dxchartsui:WebChartControl ID="chartItemSizeServer" runat="server" 
                    oncustomcallback="chartItemSizeServer_CustomCallback" Height="400px" 
                    Width="1030px" ClientInstanceName="CItemSizeServer" 
                        AppearanceNameSerializable="Chameleon">
                        <fillstyle>
                            <optionsserializable>
                                <cc1:SolidFillOptions />
                            </optionsserializable>
                        </fillstyle>
                        <seriestemplate>
                            <viewserializable>
                                <cc1:SideBySideBarSeriesView>
                                </cc1:SideBySideBarSeriesView>
                            </viewserializable>
                            <labelserializable>
                                <cc1:SideBySideBarSeriesLabel LineVisible="True">
                                    <fillstyle>
                                        <optionsserializable>
                                            <cc1:SolidFillOptions />
                                        </optionsserializable>
                                    </fillstyle>
                                    <pointoptionsserializable>
                                        <cc1:PointOptions>
                                        </cc1:PointOptions>
                                    </pointoptionsserializable>
                                </cc1:SideBySideBarSeriesLabel>
                            </labelserializable>
                            <legendpointoptionsserializable>
                                <cc1:PointOptions>
                                </cc1:PointOptions>
                            </legendpointoptionsserializable>
                        </seriestemplate>
                        <crosshairoptions>
                            <commonlabelpositionserializable>
                                <cc1:CrosshairMousePosition />
                            </commonlabelpositionserializable>
                        </crosshairoptions>
                        <tooltipoptions>
                            <tooltippositionserializable>
                                <cc1:ToolTipMousePosition />
                            </tooltippositionserializable>
                        </tooltipoptions>
                    </dxchartsui:WebChartControl>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ItemSizeServercombobox" />
                </Triggers>
                </asp:UpdatePanel>
                
                    </td>
        </tr>
    </table>
					</dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>

			<dx:TabPage Text="Mailbox Size (MB) By Database">
                <TabImage Url="~/images/icons/database.png">
                </TabImage>
                <ContentCollection>
                    <dx:ContentControl ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">
					 <table class="style1">
        <tr>
            <td>              
                    <dx:ASPxComboBox ID="MBSizeDatabasecombobox" runat="server" ValueType="System.String" 
                        AutoPostBack="True" 
                        onselectedindexchanged="MBSizeDatabasecombobox_SelectedIndexChanged">
                    </dx:ASPxComboBox>
                </td>
              
        </tr>
        <tr>
            <td width="100%">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <dxchartsui:WebChartControl ID="ChartMBSizeDatabase" runat="server" 
                    oncustomcallback="ChartMBSizeDatabase_CustomCallback" Height="400px" 
                    Width="1030px" ClientInstanceName="CMBSizeDatabase" 
                        AppearanceNameSerializable="Chameleon">
                        <fillstyle>
                            <optionsserializable>
                                <cc1:SolidFillOptions />
                            </optionsserializable>
                        </fillstyle>
                        <seriestemplate>
                            <viewserializable>
                                <cc1:SideBySideBarSeriesView>
                                </cc1:SideBySideBarSeriesView>
                            </viewserializable>
                            <labelserializable>
                                <cc1:SideBySideBarSeriesLabel LineVisible="True">
                                    <fillstyle>
                                        <optionsserializable>
                                            <cc1:SolidFillOptions />
                                        </optionsserializable>
                                    </fillstyle>
                                    <pointoptionsserializable>
                                        <cc1:PointOptions>
                                        </cc1:PointOptions>
                                    </pointoptionsserializable>
                                </cc1:SideBySideBarSeriesLabel>
                            </labelserializable>
                            <legendpointoptionsserializable>
                                <cc1:PointOptions>
                                </cc1:PointOptions>
                            </legendpointoptionsserializable>
                        </seriestemplate>
                        <crosshairoptions>
                            <commonlabelpositionserializable>
                                <cc1:CrosshairMousePosition />
                            </commonlabelpositionserializable>
                        </crosshairoptions>
                        <tooltipoptions>
                            <tooltippositionserializable>
                                <cc1:ToolTipMousePosition />
                            </tooltippositionserializable>
                        </tooltipoptions>
                    </dxchartsui:WebChartControl>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="MBSizeDatabasecombobox" />
                </Triggers>
                </asp:UpdatePanel>
                
                    </td>
        </tr>
    </table>
					</dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>

			<dx:TabPage Text="Message Count By Database">
                <TabImage Url="~/images/icons/database.png">
                </TabImage>
                <ContentCollection>
                    <dx:ContentControl ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">
					 <table class="style1">
        <tr>
            <td>              
                    <dx:ASPxComboBox ID="ItemSizeDatabaseComboBox" runat="server" ValueType="System.String" 
                        AutoPostBack="True" 
                        onselectedindexchanged="ItemSizeDatabaseComboBox_SelectedIndexChanged">
                    </dx:ASPxComboBox>
                </td>
              
        </tr>
        <tr>
            <td width="100%">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <dxchartsui:WebChartControl ID="chartItemSizeDatabase" runat="server" 
                    oncustomcallback="chartItemSizeDatabase_CustomCallback" Height="400px" 
                    Width="1030px" ClientInstanceName="CItemSizeDatabase" 
                        AppearanceNameSerializable="Chameleon">
                        <fillstyle>
                            <optionsserializable>
                                <cc1:SolidFillOptions />
                            </optionsserializable>
                        </fillstyle>
                        <seriestemplate>
                            <viewserializable>
                                <cc1:SideBySideBarSeriesView>
                                </cc1:SideBySideBarSeriesView>
                            </viewserializable>
                            <labelserializable>
                                <cc1:SideBySideBarSeriesLabel LineVisible="True">
                                    <fillstyle>
                                        <optionsserializable>
                                            <cc1:SolidFillOptions />
                                        </optionsserializable>
                                    </fillstyle>
                                    <pointoptionsserializable>
                                        <cc1:PointOptions>
                                        </cc1:PointOptions>
                                    </pointoptionsserializable>
                                </cc1:SideBySideBarSeriesLabel>
                            </labelserializable>
                            <legendpointoptionsserializable>
                                <cc1:PointOptions>
                                </cc1:PointOptions>
                            </legendpointoptionsserializable>
                        </seriestemplate>
                        <crosshairoptions>
                            <commonlabelpositionserializable>
                                <cc1:CrosshairMousePosition />
                            </commonlabelpositionserializable>
                        </crosshairoptions>
                        <tooltipoptions>
                            <tooltippositionserializable>
                                <cc1:ToolTipMousePosition />
                            </tooltippositionserializable>
                        </tooltipoptions>
                    </dxchartsui:WebChartControl>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ItemSizeDatabaseComboBox" />
                </Triggers>
                </asp:UpdatePanel>
                
                    </td>
        </tr>
    </table>
					</dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
		</TabPages>
	</dx:ASPxPageControl>
</asp:Content>
