<%@ Page Title="VitalSigns Plus - Mail Files" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="MailFiles.aspx.cs" Inherits="VSWebUI.MailFiles" %>

<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<%@ Register assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts.Web" tagprefix="dxchartsui" %>
<%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>
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
            document.getElementById('ContentPlaceHolder1_chartWidth').value = document.body.offsetWidth - 65;
            //cSelectServerRoundPanel.style.width = document.getElementById('ContentPlaceHolder1__chartWidth').value + "px";
            //1/15/2014 NS modified
            var cQuotaChart = document.getElementById('ContentPlaceHolder1_BiggestQuotaWebChart');
            if (cQuotaChart != null) {
                CBigmail.PerformCallback();
            }
            var cMailTempChart = document.getElementById('ContentPlaceHolder1_MailTemplateWebChart');
            if (cMailTempChart != null) {
                CMailTemp.PerformCallback();
            }
        }

        function ResizeChart(s, e) {
            document.getElementById('ContentPlaceHolder1_callbackState').value = 0;
            s.GetMainElement().style.width = document.getElementById('ContentPlaceHolder1__chartWidth').value + "px";
            //cSelectServerRoundPanel.style.width = document.getElementById('ContentPlaceHolder1__chartWidth').value + "px";
        }

        function ResetCallbackState() {
            window.document.form1.elements['ContentPlaceHolder1_callbackState'].value = 1;
           }
           function OnItemClick(s, e) {
           	if (e.item.parent == s.GetRootItem())
           		e.processOnServer = false;
           }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input id="chartWidth" type="hidden" runat="server" value="400" />
        <input id="callbackState" type="hidden" runat="server" value="0" />
        <table width="100%">
    <tr>
        <td>
            <div class="header" id="titleDiv" runat="server">Mail Files</div>
        </td>
        <td>
            &nbsp;
        </td>    
        <td align="right">
            <table>
                <tr>
                    <td>
					   <dx:ASPxMenu ID="ExportMenu" runat="server" ShowAsToolbar="True" Theme="Moderno" 
                            onitemclick="ASPxMenu1_ItemClick">
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
        </td>        
    </tr>
</table>
    <table class="style1">
    <tr>
        <td>
           <table>
			<tr>
			<td>
			<dx:ASPxComboBox ID="ServerComboBox" runat="server" ValueType="System.String" 
                        AutoPostBack="True" 
                        onselectedindexchanged="ServerComboBox_SelectedIndexChanged">
                    </dx:ASPxComboBox></td>		
		</tr>	
	</table>  
	</td>           
        </tr>
    <tr>
        <td>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <dxchartsui:WebChartControl ID="MailTemplateWebChart" runat="server" ClientInstanceName="CMailTemp"
                    Height="400px" Width="1200px" 
                    OnCustomCallback="MailTemplateWebChart_CustomCallback" PaletteName="Paper" 
						CrosshairEnabled="True">
                    <fillstyle>
                        <optionsserializable>
                            <cc1:SolidFillOptions />
                        </optionsserializable>
                    </fillstyle>
                    <seriestemplate>
                        <viewserializable>
                            <cc1:PieSeriesView RuntimeExploding="False">
                            </cc1:PieSeriesView>
                        </viewserializable>
                        <labelserializable>
                            <cc1:PieSeriesLabel LineVisible="True">
                                <fillstyle>
                                    <optionsserializable>
                                        <cc1:SolidFillOptions />
                                    </optionsserializable>
                                </fillstyle>
                                <pointoptionsserializable>
                                    <cc1:PiePointOptions>
                                    </cc1:PiePointOptions>
                                </pointoptionsserializable>
                            </cc1:PieSeriesLabel>
                        </labelserializable>
                        <legendpointoptionsserializable>
                            <cc1:PiePointOptions>
                            </cc1:PiePointOptions>
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
                    <dxchartsui:WebChartControl ID="BiggestQuotaWebChart" runat="server" 
                        AppearanceNameSerializable="Pastel Kit" ClientInstanceName="CBigmail" 
                        Height="400px" OnCustomCallback="BiggestQuotaWebChart_CustomCallback" 
                        Width="1200px" CrosshairEnabled="True">
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
                    <asp:AsyncPostBackTrigger ControlID="ServerComboBox" />
                </Triggers>
                </asp:UpdatePanel>
        </td>
    </tr>
        <tr>
            <td>
            <table class="style1">
        <tr>
            <td>
                <table>
				<tr>
			<dx:ASPxLabel ID="MessageLabel" runat="server" CssClass="lblsmallFont" 
								 Text="">
							 </dx:ASPxLabel>
			</tr>
                   <tr>
                        <td align="left">
                            <dx:ASPxButton runat="server" ID="btnCollapse" Text="Collapse All Rows" 
                                OnClick="btnCollapse_Click" CssClass="sysButton">
                                <Image Url="~/images/icons/forbidden.png">
                                </Image>
                            </dx:ASPxButton> 
                        </td>
                        <td>
                            &nbsp;
                        </td>
						<td>
            <dx:ASPxLabel ID="TypeLabel" runat="server" CssClass="lblsmallFont" 
                Text="Type:" >
            </dx:ASPxLabel>
        </td>
        <td>
            <dx:ASPxComboBox ID="TypeComboBox" runat="server"  onselectedindexchanged="TypeComboBox_SelectedIndexChanged"  AutoPostBack="True" >
			 <Items>
                                                                     <dx:ListEditItem Text="None" Value="0" />
                   
                                                                    </Items>
            </dx:ASPxComboBox>
        </td>
		 <td>
                            <dx:ASPxButton ID="ExportXlsButton" runat="server" Text="Export to XLS" 
                                Theme="Office2010Blue" onclick="ExportXlsButton_Click" Visible="False">
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="ExportXlsxButton" runat="server" Text="Export to XLSX" 
                                Theme="Office2010Blue" onclick="ExportXlsxButton_Click" Visible="False">
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="ExportPdfButton" runat="server" Text="Export to PDF" 
                                Theme="Office2010Blue" onclick="ExportPdfButton_Click" Visible="False">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxGridView ID="MailFileGridView" runat="server" 
                    AutoGenerateColumns="False" EnableTheming="True" KeyFieldName="ID" 
                    OnHtmlDataCellPrepared="MailFileGridView_HtmlDataCellPrepared" 
                    OnHtmlRowCreated="MailFileGridView_HtmlRowCreated" Theme="Office2003Blue" 
                    Width="100%" OnPageSizeChanged="MailFileGridView_PageSizeChanged"  OnDataBound="DataBound">
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" 
                            ShowInCustomizationForm="True" Visible="False" VisibleIndex="34">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Scan Date" FieldName="ScanDate" 
                            ShowInCustomizationForm="True" VisibleIndex="7" Width="150px">
                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
<Settings AllowAutoFilter="True" AutoFilterCondition="Contains"></Settings>

                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss" Wrap="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="File Name" FieldName="FileName" 
                            ShowInCustomizationForm="True" VisibleIndex="1">
                          <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                            <CellStyle CssClass="GridCss" Wrap="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Title" FieldName="Title" 
                            ShowInCustomizationForm="True" VisibleIndex="4">
                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                            <CellStyle CssClass="GridCss" Wrap="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="File Size" FieldName="FileSize" 
                            ShowInCustomizationForm="True" VisibleIndex="8">
                            <Settings AllowAutoFilter="False" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss2" Wrap="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Server" FieldName="Server" 
                            ShowInCustomizationForm="True" VisibleIndex="6">
                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />

                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                            <CellStyle CssClass="GridCss" Wrap="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Design Template Name" 
                            FieldName="DesignTemplateName" ShowInCustomizationForm="True" VisibleIndex="9">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                            <CellStyle CssClass="GridCss" Wrap="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="% of Quota" FieldName="PercentQuota" 
                            ShowInCustomizationForm="True" VisibleIndex="0">
                            <Settings AllowAutoFilter="False" />
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Quota" FieldName="Quota" 
                            ShowInCustomizationForm="True" VisibleIndex="3">
                            <Settings AllowAutoFilter="False" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss2">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="FTIndexed" FieldName="FTIndexed" 
                            ShowInCustomizationForm="True" VisibleIndex="10" Visible="False">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss" Wrap="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Enabled For Cluster Replication" 
                            FieldName="EnabledForClusterReplication" ShowInCustomizationForm="True" 
                            VisibleIndex="11" Visible="False">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                            <CellStyle CssClass="GridCss" Wrap="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="ReplicaID" FieldName="ReplicaID" 
                            ShowInCustomizationForm="True" VisibleIndex="12">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss" Wrap="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="ODS" FieldName="ODS" 
                            ShowInCustomizationForm="True" VisibleIndex="13">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss" Wrap="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" 
                            ShowInCustomizationForm="True" VisibleIndex="5">
                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />

                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader1" />
                            <CellStyle CssClass="GridCss1">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Document Count" FieldName="DocumentCount" 
                            ShowInCustomizationForm="True" VisibleIndex="14">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                            <CellStyle CssClass="GridCss2" Wrap="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Categories" FieldName="Categories" 
                            ShowInCustomizationForm="True" VisibleIndex="15">
                          <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />

                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss" Wrap="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Created" FieldName="Created" 
                            ShowInCustomizationForm="True" VisibleIndex="16" Width="150px">
                           <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />


                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss" Wrap="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Current Access Level" 
                            FieldName="CurrentAccessLevel" ShowInCustomizationForm="True" VisibleIndex="17">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                            <CellStyle CssClass="GridCss" Wrap="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="FTIndex Frequency" 
                            FieldName="FTIndexFrequency" ShowInCustomizationForm="True" 
                            VisibleIndex="18" Visible="False">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss" Wrap="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Is In Service" FieldName="IsInService" 
                            ShowInCustomizationForm="True" VisibleIndex="19">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss" Wrap="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Folder" FieldName="Folder" 
                            ShowInCustomizationForm="True" VisibleIndex="20">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss" Wrap="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Is Private Address Book" 
                            FieldName="IsPrivateAddressBook" ShowInCustomizationForm="True" 
                            VisibleIndex="21">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                            <CellStyle CssClass="GridCss" Wrap="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Is Public Addres Book" 
                            FieldName="IsPublicAddressBook" ShowInCustomizationForm="True" 
                            VisibleIndex="22">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                            <CellStyle CssClass="GridCss" Wrap="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Last Fixup" FieldName="LastFixup" 
                            ShowInCustomizationForm="True" VisibleIndex="23" Width="150px">
                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                            <CellStyle CssClass="GridCss" Wrap="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Last FT Indexed" FieldName="LastFTIndexed" 
                            ShowInCustomizationForm="True" VisibleIndex="24" Width="150px" 
                            Visible="False">
                         <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                            <CellStyle CssClass="GridCss" Wrap="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Percent Used" FieldName="PercentUsed" 
                            ShowInCustomizationForm="True" VisibleIndex="25">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                            <CellStyle CssClass="GridCss2" Wrap="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Details" FieldName="Details" 
                            ShowInCustomizationForm="True" VisibleIndex="26">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                            <CellStyle CssClass="GridCss" Wrap="False">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Last Modified" FieldName="LastModified" 
                            ShowInCustomizationForm="True" VisibleIndex="27" Width="150px">
                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                            <CellStyle CssClass="GridCss" Wrap="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Enabled For Replication" 
                            FieldName="EnabledForReplication" ShowInCustomizationForm="True" 
                            VisibleIndex="28">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                            <CellStyle CssClass="GridCss" Wrap="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="IsMail File" FieldName="IsMailFile" 
                            ShowInCustomizationForm="True" VisibleIndex="29" Visible="False">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                            <CellStyle CssClass="GridCss" Wrap="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Inbox DocCount" FieldName="InboxDocCount" 
                            ShowInCustomizationForm="True" Visible="False" VisibleIndex="2">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                            <CellStyle CssClass="GridCss" Wrap="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Q_PlaceBot Count" 
                            FieldName="Q_PlaceBotCount" ShowInCustomizationForm="True" VisibleIndex="30">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                            <CellStyle CssClass="GridCss2" Wrap="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Q_CustomForm Count" 
                            FieldName="Q_CustomFormCount" ShowInCustomizationForm="True" VisibleIndex="31">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                            <CellStyle CssClass="GridCss2" Wrap="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="PersonDocID" FieldName="PersonDocID" 
                            ShowInCustomizationForm="True" VisibleIndex="32">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                            <CellStyle CssClass="GridCss" Wrap="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="FileNamePath" FieldName="FileNamePath" 
                            ShowInCustomizationForm="True" VisibleIndex="33">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                            <CellStyle CssClass="GridCss" Wrap="True">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsBehavior AutoExpandAllGroups="True" ColumnResizeMode="NextColumn" />

<SettingsBehavior ColumnResizeMode="NextColumn" AutoExpandAllGroups="True"></SettingsBehavior>

                    <SettingsPager>
                        <PageSizeItemSettings Visible="True" 
                            Items="10, 20, 50, 100, 200, 500, 1000, 1500">
                        </PageSizeItemSettings>
                    </SettingsPager>
                      <TotalSummary>
											<dx:ASPxSummaryItem FieldName="Server" SummaryType="Count" ShowInColumn="Server" DisplayFormat="{0} Item(s)" />
										</TotalSummary>
                    <Settings ShowFilterRow="True" ShowHorizontalScrollBar="True" />

<Settings ShowFilterRow="True" ShowHorizontalScrollBar="True"></Settings>

                    <Styles>
                        <GroupRow Font-Bold="True">
                        </GroupRow>
                        <AlternatingRow CssClass="GridAltRow">
                        </AlternatingRow>
                        <Cell Wrap="True">
                        </Cell>
                    </Styles>
                    <Templates>
                        <GroupRowContent>
                            <%# Container.GroupText%>
                        </GroupRowContent>
                    </Templates>
                </dx:ASPxGridView>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
    </table>
            </td>
        </tr>
		 <tr>
            <td>
                <dx:ASPxGridViewExporter ID="ServerGridViewExporter" GridViewID="MailFileGridView" runat="server">
                </dx:ASPxGridViewExporter>
            </td>
        </tr>
    </table>
</asp:Content>
