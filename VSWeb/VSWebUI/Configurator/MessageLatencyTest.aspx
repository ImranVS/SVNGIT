<%@ Page Title="VitalSigns Plus-MailServicesGrid" Language="C#" MasterPageFile="~/Site1.Master"
	AutoEventWireup="true" CodeBehind="MessageLatencyTest.aspx.cs" Inherits=" VSWebUI.Configurator.MessageLatencyTest" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>





	
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
	<script src="../js/bootstrap.min.js" type="text/javascript"></script>
	<script type="text/javascript">
		$(document).ready(function () {
			$('.alert-success').delay(10000).fadeOut("slow", function () {
			});
		});
		function OnItemClick(s, e) {
			if (e.item.parent == s.GetRootItem())
				e.processOnServer = false;
		}
	</script>
	</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table width="100%">
		<tr>
			<td valign="top">
                <div class="header" id="Div2" runat="server">
								Message Latency Test
							</div>
			</td>
            <td align="right" valign="top">
                <table>
                    <tr>
                        <td>
                            <dx:ASPxMenu ID="ASPxMenu1" runat="server" EnableTheming="True" 
                    HorizontalAlign="Right" onitemclick="ASPxMenu1_ItemClick" ShowAsToolbar="True" 
                    Theme="Moderno">
                    <ClientSideEvents ItemClick="OnItemClick" />
                    <Items>
                        <dx:MenuItem Name="MainItem">
                            <Items>
                                <dx:MenuItem Name="ExchangeHeatMap" Text="Exchange Heat Map">
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
        <tr>
            <td colspan="2">
                <div class="info">
					A Message Latency Test routes text messages between pairs of servers and times the
					result. Select the servers to include in the test. The result will be displayed as a mail 
                    flow heat map as shown below.
				</div>
            </td>
        </tr>
	</table>
	<table width="100%">
		<tr>
			<td>
                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="100%" 
                    HeaderText="Test Parameters" Theme="Glass">
                    <PanelCollection>
<dx:PanelContent runat="server">
    <table>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel11" runat="server" CssClass="lblsmallFont" 
                    Text="Scan Interval:">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxTextBox ID="scantext" runat="server" CssClass="txtsmall" Width="100px">
                </dx:ASPxTextBox>
            </td>
            <td>
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" Text="minutes">
				</dx:ASPxLabel>
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                <dx:ASPxCheckBox ID="Enreportcheckbox" runat="server" CheckState="Unchecked" 
                    CssClass="lblsmallFont" Text="Enable report" Wrap="False">
                </dx:ASPxCheckBox>
            </td>
        </tr>
        </table>
        <table width="100%">
        <tr>
			<td valign="top">
				<table width="100%">
					<tr>
						<td valign="top">
							<div class="subheader" id="servernamelbldisp" runat="server">
								Servers to Include
							</div>
                            <div id="successDiv" runat="server" class="alert alert-success" style="display: none">
                Selected fields were successfully saved.
                <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
            </div>
            <div id="errorDiv" class="alert alert-danger" runat="server" style="display: none">
					 Please select at least one attribute.
					  <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    </div>
                        </td>
					</tr>
					<tr>
						<td>
							<dx:ASPxGridView runat="server" CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css"
								CssPostfix="Office2010Silver" KeyFieldName="ServerId" AutoGenerateColumns="False"
								Width="50%" ID="MessageLatencyTestgrd" Cursor="pointer" EnableTheming="True"
								Theme="Office2003Blue" OnPreRender="MessageLatencyTestgrd_PreRender">
								<Columns>
									<dx:GridViewDataTextColumn Caption="EnableLatencyTest" FieldName="EnableLatencyTest"
										VisibleIndex="5" Visible="false">
										<Settings AutoFilterCondition="Contains" />
										<Settings AutoFilterCondition="Contains"></Settings>
										<EditCellStyle CssClass="GridCss">
										</EditCellStyle>
										<EditFormCaptionStyle CssClass="GridCss" HorizontalAlign="Left">
										</EditFormCaptionStyle>
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewCommandColumn Caption="Enable" ShowSelectCheckbox="True" VisibleIndex="0">
										<ClearFilterButton Visible="True">
										</ClearFilterButton>
										<HeaderStyle CssClass="GridCssHeader1" />
									</dx:GridViewCommandColumn>
									<%--<dx:GridViewDataColumn Caption="Enable" VisibleIndex="0" Width="70px">
                <DataItemTemplate>
                   <dx:ASPxCheckBox ID="enablechbx" runat="server"  >
            
                 </dx:ASPxCheckBox>
               </DataItemTemplate>
			    <HeaderStyle CssClass="GridCssHeader" />
			   </dx:GridViewDataColumn>--%>
									<%--<dx:GridViewDataColumn Caption="Enable" VisibleIndex="0" CellStyle-HorizontalAlign="Center"
																	Width="50px">
																	<DataItemTemplate>
																		<dx:ASPxCheckBox ID="checklatency" runat="server" OnInit="checklatency_Init"
																			 KeyFieldName="ServerId" Value='<%# Eval("EnableLatencyTest") %>' />
																	</DataItemTemplate>
																	<HeaderStyle CssClass="GridCssHeader1" />
																	<CellStyle HorizontalAlign="Center" CssClass="GridCss1">
																	</CellStyle>
																</dx:GridViewDataColumn>--%>
									<dx:GridViewDataTextColumn Caption="Name" FieldName="ServerName" VisibleIndex="1">
										<Settings AutoFilterCondition="Contains" />
										<EditCellStyle CssClass="GridCss">
										</EditCellStyle>
										<EditFormCaptionStyle CssClass="GridCss" HorizontalAlign="Left">
										</EditFormCaptionStyle>
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Yellow Threshold (ms)" FieldName="LatencyYellowThreshold"
										VisibleIndex="3">
										<DataItemTemplate>
											<dx:ASPxTextBox ID="txtyellowthreshValue" runat="server" KeyFieldName="ServerId"
												Width="50px" Value='<%# Eval("LatencyYellowThreshold") %>'>
											</dx:ASPxTextBox>
										</DataItemTemplate>
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Red Threshold (ms)" 
										FieldName="LatencyRedThreshold" VisibleIndex="4" Width="15%">
										<DataItemTemplate>
											<dx:ASPxTextBox ID="txtredthreshValue" runat="server" KeyFieldName="ServerId" Width="50px"
												Value='<%# Eval("LatencyRedThreshold") %>'>
											</dx:ASPxTextBox>
										</DataItemTemplate>
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn FieldName="Location" Caption="Location" VisibleIndex="2">
										<Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
										<HeaderStyle CssClass="GridCssHeader"></HeaderStyle>
										<CellStyle CssClass="GridCss">
										</CellStyle>
									</dx:GridViewDataTextColumn>
								</Columns>
<SettingsBehavior AllowSort="false"/>
<Styles CssPostfix="Office2010Silver" CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css"></Styles>
							</dx:ASPxGridView>
						</td>
					</tr>
				</table>
			</td>
			<td valign="top">
				<table>
					<tr>
						<td valign="top">
							<div class="subheader" id="samplenamelbldisp" runat="server">
								Sample Report
							</div>
						</td>
					</tr>
					<tr>
						<td>
							<img id="Img1" runat="server" src="~/images/latency heat map.jpg" />
							
							
						</td>
					</tr>
				</table>
			</td>
		</tr>
    </table>
                        </dx:PanelContent>
</PanelCollection>
                </dx:ASPxRoundPanel>
			</td>
		</tr>
		
		<tr>
			<td>
				<table>
					<tr>
						<td>
							<dx:ASPxButton ID="FormSaveButton" runat="server" Text="Save" CssClass="sysButton" OnClick="FormSaveButton_Click">
							</dx:ASPxButton>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</asp:Content>
