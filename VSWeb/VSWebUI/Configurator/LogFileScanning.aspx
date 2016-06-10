<%@ Page Title="VitalSigns Plus - Log File Scanning" Language="C#" MasterPageFile="~/Site1.Master"
	AutoEventWireup="true" CodeBehind="LogFileScanning.aspx.cs" Inherits="VSWebUI.Configurator.LogFileScanning" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
<script type="text/javascript">
    //5/21/2015 NS added for VSPLUS-1771
    var visibleIndex;
    function OnCustomButtonClick(s, e) {
        visibleIndex = e.visibleIndex;

        if (e.buttonID == "deleteButton")
            LogFileGridView.GetRowValues(e.visibleIndex, 'Keyword', OnGetRowValues);

        function OnGetRowValues(values) {
            var id = values[0];
            var name = values[1];
            var OK = (confirm('Are you sure you want to delete the keyword - ' + values + '?'))
            if (OK == true) {
                LogFileGridView.DeleteRow(visibleIndex);
            }
        }
    }
        </script>
	</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table style="margin-top: 0px" width="100%">
        <tr>
            <td>
                <div class="header" id="lblServer" runat="server">IBM Domino Log File Scanning</div>
            </td>
        </tr>
		<tr>
			<td valign="top">
				<%-- <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
        CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Log File Scanning"
        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%" Height="100%">
        <HeaderStyle Height="23px">
            <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
        </HeaderStyle>
        <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
        <PanelCollection>
            <dx:PanelContent runat="server" SupportsDisabledAttribute="True">--%>
				<table id="logtable" width="100%">
					<tr>
						<td>
							<div class="info">
								The Domino Server log file also known as LOG.NSF is found on every Server. 
                                Whenever anything &#39;significant&#39; happens on the server, an entry is written to 
                                the log file. The problem is, of course, that there are so many log files and so 
                                little time. This tab allows you to define specific log entries you are interested in knowing
								about immediately, whenever the word(s) appear in the log file.
							</div>
						</td>
					</tr>
                    <tr>
                        <td>
                            <dx:ASPxButton ID="NewButton" runat="server" Text="New" CssClass="sysButton" AutoPostBack="False">
                                <ClientSideEvents Click="function() { LogFileGridView.AddNewRow(); }" />
                                <Image Url="~/images/icons/add.png">
                                </Image>
                            </dx:ASPxButton>
                        </td>
                    </tr>
					<tr>
						<td>
							<dx:ASPxGridView ID="LogFileGridView" runat="server" AutoGenerateColumns="False"
								OnRowDeleting="LogFileGridView_RowDeleting"
								OnRowInserting="LogFileGridView_RowInserting" OnRowUpdating="LogFileGridView_RowUpdating"
								KeyFieldName="ID" Width="100%" ClientInstanceName="LogFileGridView" OnHtmlDataCellPrepared="LogFileGridView_HtmlDataCellPrepared"
								EnableTheming="True" Theme="Office2003Blue" OnPageSizeChanged="LogFileGridView_PageSizeChanged">
                                <ClientSideEvents CustomButtonClick="OnCustomButtonClick" />
								<Columns>
									<dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" VisibleIndex="0" width="70px">
										<EditButton Visible="True">
											<Image Url="../images/edit.png">
											</Image>
										</EditButton>
										<NewButton Visible="True">
											<Image Url="../images/icons/add.png">
											</Image>
										</NewButton>
										<DeleteButton Visible="False">
											<Image Url="../images/delete.png">
											</Image>
										</DeleteButton>
										<CancelButton Visible="True">
											<Image Url="~/images/cancel.gif">
											</Image>
										</CancelButton>
										<UpdateButton Visible="True">
											<Image Url="~/images/update.gif">
											</Image>
										</UpdateButton>
										<CellStyle>
											<Paddings Padding="3px" />
										</CellStyle>
										<ClearFilterButton Visible="True">
											<Image Url="~/images/clear.png">
											</Image>
										</ClearFilterButton>
										<HeaderStyle CssClass="GridCssHeader1" />
									</dx:GridViewCommandColumn>
									<dx:GridViewDataTextColumn Caption="Keywords" VisibleIndex="2" 
                                        FieldName="Keyword">
										<Settings AutoFilterCondition="Contains" />
										<EditCellStyle CssClass="GridCss">
										</EditCellStyle>
										<EditFormCaptionStyle CssClass="GridCss">
										</EditFormCaptionStyle>
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss">
										</CellStyle>
                                         <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Exclude scanning for" VisibleIndex="3" 
                                        FieldName="NotRequiredKeyword">
										<Settings AutoFilterCondition="Contains" />
										<EditCellStyle CssClass="GridCss">
										</EditCellStyle>
										<EditFormCaptionStyle CssClass="GridCss">
										</EditFormCaptionStyle>
										<HeaderStyle CssClass="GridCssHeader" />
										<CellStyle CssClass="GridCss">
										</CellStyle>
                                         <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataCheckColumn Caption="Limit to one alert per day" Width="120px"
                                        VisibleIndex="4" FieldName="RepeatOnce">
										<Settings AllowAutoFilter="False" />
										<EditCellStyle CssClass="GridCss">
										</EditCellStyle>
										<EditFormCaptionStyle CssClass="GridCss">
										</EditFormCaptionStyle>
										<HeaderStyle CssClass="GridCssHeader" Wrap="True">
											<Paddings Padding="5px" />
										</HeaderStyle>
										<CellStyle CssClass="GridCss1">
										</CellStyle>
									</dx:GridViewDataCheckColumn>
									<dx:GridViewDataCheckColumn Caption="Scan log.nsf" VisibleIndex="5" Width="80px"
                                        FieldName="Log">
										<Settings AllowAutoFilter="False" />
										<EditCellStyle CssClass="GridCss">
										</EditCellStyle>
										<EditFormCaptionStyle CssClass="GridCss">
										</EditFormCaptionStyle>
										<HeaderStyle CssClass="GridCssHeader" Wrap="True">
											<Paddings Padding="5px" />
										</HeaderStyle>
										<CellStyle CssClass="GridCss1">
										</CellStyle>
									</dx:GridViewDataCheckColumn>
									<dx:GridViewDataCheckColumn Caption="Scan AgentLog.nsf " VisibleIndex="6" Width="100px"
                                        FieldName="AgentLog">
										<Settings AllowAutoFilter="False" />
										<EditCellStyle CssClass="GridCss">
										</EditCellStyle>
										<EditFormCaptionStyle CssClass="GridCss" >
										</EditFormCaptionStyle>
										<HeaderStyle CssClass="GridCssHeader" Wrap="True">
											<Paddings Padding="5px" />
										</HeaderStyle>
										<CellStyle CssClass="GridCss1">
										</CellStyle>
									</dx:GridViewDataCheckColumn>
								    <dx:GridViewCommandColumn ButtonType="Image" Caption="Delete" VisibleIndex="1" 
                                        Width="60px">
                                        <CustomButtons>
                                            <dx:GridViewCommandColumnCustomButton ID="deleteButton" Text="Delete">
                                                <Image Url="~/images/delete.png">
                                                </Image>
                                            </dx:GridViewCommandColumnCustomButton>
                                        </CustomButtons>
                                        <HeaderStyle CssClass="GridCssHeader1" />
                                        <CellStyle CssClass="GridCss1">
                                        </CellStyle>
                                    </dx:GridViewCommandColumn>
								</Columns>
								<Settings ShowFilterRow="True"  />
								<SettingsText ConfirmDelete="Are you sure you want to delete this record?" />
								<Styles>
									<LoadingPanel ImageSpacing="5px">
									</LoadingPanel>
									<GroupRow Font-Bold="True">
									</GroupRow>
									<AlternatingRow CssClass="GridAltRow" Enabled="True">
									</AlternatingRow>
									<Header ImageSpacing="5px" SortingImageSpacing="5px">
									</Header>
									<EditForm CssClass="GridCssEditForm">
									</EditForm>
								</Styles>
								<StylesEditors ButtonEditCellSpacing="0">
									<ProgressBar Height="21px">
									</ProgressBar>
								</StylesEditors>
								<Templates>
									<EditForm>
										<table>
											<tr>
												<td>
													&nbsp;
												</td>
												<td colspan="2">
													<asp:HiddenField ID="LogFileIDHiddenField" runat="server" Value='<%# Eval("ID") %>' />
													<asp:Label ID="Label2" runat="server" CssClass="lblsmallFont" Text="VitalSigns will search the selected logs for the word or phrase in the Keywords field. If you would like to be alerted &lt;b&gt;every time&lt;/b&gt; the word/phrase is found, do not check the limit box."></asp:Label>
												</td>
											</tr>
                                            <tr>
                                                <td colspan="3">
                                                    &nbsp;
                                                </td>
                                            </tr>
											<tr>
												<td>
													&nbsp;
												</td>
												<td>
													<dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Keywords:" CssClass="lblsmallFont">
													</dx:ASPxLabel>
												</td>
												<td>
													<dx:ASPxTextBox ID="LogFileTextBox" runat="server" Width="350px" Value='<%#Eval("Keyword")%>'>
													</dx:ASPxTextBox>
												</td>
											</tr>
											<tr>
												<td>
												</td>
												<td>
													<dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Exclude scanning for:" CssClass="lblsmallFont">
													</dx:ASPxLabel>
												</td>
												<td>
													<dx:ASPxTextBox ID="NotLogFileTextBox" runat="server" Width="350px" Value='<%#Eval("NotRequiredKeyword") %>'>
													</dx:ASPxTextBox>
												</td>
											</tr>
											<tr>
												<td colspan="2">
													&nbsp;
												</td>
												<td>
													<table>
														<tr>
															<td>
																<dx:ASPxCheckBox ID="logCheckBox" runat="server" Text=" Scan log.nsf " CheckState="Unchecked"
																	Value='<%#Eval("Log") %>'>
																</dx:ASPxCheckBox>
															</td>
															<td>
																<dx:ASPxCheckBox ID="LogFileCheckBox" runat="server" Text="Limit to one Alert per day"
																	CheckState="Unchecked" Value='<%#Eval("RepeatOnce") %>'>
																</dx:ASPxCheckBox>
															</td>
														</tr>
														<tr>
															<td>
																<dx:ASPxCheckBox ID="AgentlogCheckBox" runat="server" Text=" Scan AgentLog.nsf  "
																	CheckState="Unchecked" Value='<%#Eval("AgentLog") %>'>
																</dx:ASPxCheckBox>
															</td>
															<td>
															</td>
														</tr>
													</table>
												</td>
											</tr>
											<tr>
												<td colspan="3" align="right">
													<dx:ASPxGridViewTemplateReplacement ID="UpdateButton" runat="server" ReplacementType="EditFormUpdateButton" />
													<dx:ASPxGridViewTemplateReplacement ID="CancelButton" runat="server" ReplacementType="EditFormCancelButton" />
												</td>
											</tr>
										</table>
									</EditForm>
								</Templates>
								<SettingsBehavior ColumnResizeMode="Control"  ConfirmDelete="True" ></SettingsBehavior>
								<SettingsPager PageSize="50" SEOFriendly="Enabled">
									<PageSizeItemSettings Visible="true" />
								</SettingsPager>
							</dx:ASPxGridView>
						</td>
					</tr>
					<tr>
						<td>
							&nbsp;
						</td>
					</tr>
					<tr>
						<td>
							&nbsp;
						</td>
					</tr>
				</table>
				<%-- </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>--%>
			</td>
		</tr>
	</table>
</asp:Content>
