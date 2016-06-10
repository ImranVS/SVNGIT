<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
	CodeBehind="Feedback.aspx.cs" Inherits="VSWebUI.Configurator.Feedback" Async="true" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>











<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script type="text/javascript">
    // <![CDATA[
		function Uploader_OnUploadStart() {
			ASPxButton1.SetEnabled(false);
		}
		function Uploader_OnFileUploadComplete(args) {
			var imgSrc = aspxPreviewImgSrc;
			if (args.isValid) {
				var date = new Date();
				imgSrc = "log_files/" + args.callbackData + "?dx=" + date.getTime();
			}
			getPreviewImageElement().src = imgSrc;
		}
		function Uploader_OnFilesUploadComplete(args) {
			UpdateUploadButton();
		}
		function UpdateUploadButton() {
			ASPxButton1.SetEnabled(uploader.GetText(0) != "");
		}
		function getPreviewImageElement() {
			return document.getElementById("ASPxLabel6");
		}
     
    // ]]> 
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dx:ASPxPageControl Font-Bold="True" ID="ASPxPageControl1" runat="server" ActiveTabIndex="1"
                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
		CssPostfix="Glass" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                    TabSpacing="0px" Width="100%" 
		 AutoPostBack="true" EnableHierarchyRecreation="False">
                    <TabPages>
                        <dx:TabPage Text="Feedback">
                            <TabImage Url="~/images/icons/information.png" />
<TabImage Url="~/images/icons/information.png"></TabImage>
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">
  
	<dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
		CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="FeedBack" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
		Width="70%" Height="70%">
		<ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

		<HeaderStyle Height="23px">
			<Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
<Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
		</HeaderStyle>
		<PanelCollection>
			<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
				<table>
					<tr>
						<td>
							<dx:ASPxLabel ID="SubjectLabel" runat="server" CssClass="lblsmallFont" Text="Subject:">
							</dx:ASPxLabel>
						</td>
						<td>
							<dx:ASPxTextBox ID="SubjectTextBox" runat="server" CssClass="txtsmall" Width="170px">
								<ValidationSettings ErrorDisplayMode="ImageWithTooltip">
									<RequiredField IsRequired="True" />
									<RequiredField IsRequired="True"></RequiredField>
								</ValidationSettings>
							</dx:ASPxTextBox>
						</td>
					</tr>
					<tr>
						<td>
							<dx:ASPxLabel ID="TypeLabel" runat="server" CssClass="lblsmallFont" Text="Type:">
							</dx:ASPxLabel>
						</td>
						<td>
							<dx:ASPxTextBox ID="TypeTextBox" runat="server" CssClass="txtsmall" Width="170px">
								<ValidationSettings ErrorDisplayMode="ImageWithTooltip">
									<RequiredField IsRequired="True" />
									<RequiredField IsRequired="True"></RequiredField>
								</ValidationSettings>
							</dx:ASPxTextBox>
						</td>
					</tr>
					<tr>
						<td>
							<dx:ASPxLabel ID="MessageLabel" runat="server" CssClass="lblsmallFont" Text="Message:">
							</dx:ASPxLabel>
						</td>
						<td>
							<%--<dx:ASPxTextBox ID="MessageTextBox" runat="server" CssClass="txtsmall" Width="170px"
								Height="100px">
								<ValidationSettings ErrorDisplayMode="ImageWithTooltip">
									<RequiredField IsRequired="True" />
									<RequiredField IsRequired="True"></RequiredField>
								</ValidationSettings>
							</dx:ASPxTextBox>--%>
							<dx:ASPxMemo ID="MessageMemo" ClientInstanceName="memo" runat="server" Height="80px" Width="63%" 
                        AutoResizeWithContainer="True">
                    </dx:ASPxMemo>
						</td>
					</tr>
					<tr>
						<td>
							<dx:ASPxLabel ID="StatusLabel" runat="server" CssClass="lblsmallFont" Text="Status:"
								Visible="false">
							</dx:ASPxLabel>
						</td>
						<td>
							<dx:ASPxTextBox ID="StatusTextBox" runat="server" CssClass="txtsmall" Width="170px"
								Visible="false">
							</dx:ASPxTextBox>
						</td>
					</tr>
					<tr>
						<td>
							<dx:ASPxLabel ID="AttachmentsLabel" runat="server" CssClass="lblsmallFont" Text="Attachments:">
							</dx:ASPxLabel>
						</td>
						<td>
							<dx:ASPxUploadControl ID="fileupld" runat="server" ClientInstanceName="uploader"
								ShowProgressPanel="false" Size="30" OnFileUploadComplete="fileupld_FileUploadComplete"
								CancelButtonHorizontalPosition="Right" ShowUploadButton="false">
								<ClientSideEvents FileUploadComplete="function(s, e) { Uploader_OnFileUploadComplete(e); }"
									FilesUploadComplete="function(s, e) { Uploader_OnFilesUploadComplete(e); }" FileUploadStart="function(s, e) { Uploader_OnUploadStart(); }"
									TextChanged="function(s, e) { UpdateUploadButton(); }"></ClientSideEvents>
								<ValidationSettings MaxFileSize="4194304">
								</ValidationSettings>
								<CancelButton ImagePosition="Right">
								</CancelButton>
							</dx:ASPxUploadControl>
						</td>
						<td>
							<dx:ASPxLabel ID="ASPxLabel7" runat="server" CssClass="lblsmallFont">
							</dx:ASPxLabel>
						</td>
						<td>
							<dx:ASPxLabel ID="SendingMail" runat="server" CssClass="lblsmallFont">
							</dx:ASPxLabel>
						</td>
					</tr>
					<tr>
						<td>
							<dx:ASPxButton ID="SendButton" runat="server" CausesValidation="False" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
								CssPostfix="Office2010Blue" Font-Bold="False" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
								Text="Send" Width="75px" OnClick="SendButton_Click">
							</dx:ASPxButton>
						</td>
						
					</tr>
				</table>
			</dx:PanelContent>
		</PanelCollection>
	</dx:ASPxRoundPanel>  </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
						 <dx:TabPage Text="Pending Feedback">
                            <TabImage Url="~/images/icons/information.png" />
                            <TabImage Url="~/images/icons/information.png">
                            </TabImage>
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl6" runat="server" SupportsDisabledAttribute="True">
	<dx:ASPxRoundPanel ID="ViewFeedBackRoundPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
		CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="View Pending FeedBack" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
		Width="70%" Height="70%">
		<ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

		<HeaderStyle Height="23px">
			<Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
<Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
		</HeaderStyle>
		<PanelCollection>
			<dx:PanelContent ID="ViewFeedBackPanelContent" runat="server" SupportsDisabledAttribute="True">
   <dx:ASPxGridView runat="server" 
                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                CssPostfix="Office2010Blue" KeyFieldName="ID" 
                AutoGenerateColumns="False" ID="ViewFeedbackGridViews" 
                  OnRowDeleting ="ViewFeedbackGridViews_RowDeleting"  EnableModelValidation="False"   
                Width="100%"   Theme="Office2003Blue" >
            
                <Columns>
                         <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" FixedStyle="Left" 
                        VisibleIndex="0" Width="60px">
                            <EditButton Visible="False">
                                <Image Url="../images/edit.png">
                                </Image>
                            </EditButton>
                            <NewButton Visible="False">
                                <Image Url="../images/icons/add.png">
                                </Image>
                            </NewButton>
                            <DeleteButton Visible="True" >
                                <Image Url="../images/update.gif">
                                </Image>
                            </DeleteButton>                             
                            <CancelButton Visible="False">
                                <Image Url="~/images/cancel.gif">
                                </Image>
                            </CancelButton>
                            <UpdateButton Visible="False">
                                <Image Url="~/images/update.gif">
                                </Image>
                            </UpdateButton>                                     
                            <CellStyle CssClass="GridCss1">
                                <Paddings Padding="3px" />
<Paddings Padding="3px"></Paddings>
                            </CellStyle>
                            <ClearFilterButton Visible="True">
                                <Image Url="~/images/clear.png">
                                      </Image>
                            </ClearFilterButton>
                            <HeaderStyle CssClass="GridCssHeader" >                                       
                                
                            </HeaderStyle>                                       
                        </dx:GridViewCommandColumn>
                         <dx:gridviewdatatextcolumn FieldName="Subject">   
                              
                           
                            <Settings AllowAutoFilter="False" />
<Settings AllowAutoFilter="False"></Settings>

                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:gridviewdatatextcolumn>
                         <dx:gridviewdatatextcolumn FieldName="Type">      
                             
                           
                            <Settings AllowAutoFilter="False" />
<Settings AllowAutoFilter="False"></Settings>

                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:gridviewdatatextcolumn>  
                         <dx:gridviewdatatextcolumn FieldName="Message"   >   
                               
                           
                            <Settings AllowAutoFilter="False" />
<Settings AllowAutoFilter="False"></Settings>

                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:gridviewdatatextcolumn>
                         <dx:gridviewdatatextcolumn FieldName="Status"> 
                            <Settings AllowAutoFilter="False" />
                      <Settings AllowAutoFilter="False"></Settings>

                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:gridviewdatatextcolumn>
              
					     <dx:gridviewdatatextcolumn FieldName="ID"  Visible="false">   
                                
                            
                            <Settings AllowAutoFilter="False" />
<Settings AllowAutoFilter="False"></Settings>

                            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                        </dx:gridviewdatatextcolumn>
                    </Columns>
                    <Settings ShowFilterRow="True" />
                    <SettingsText ConfirmDelete="'Are sure you want to update this record as Completed?'" />

<Settings ShowFilterRow="True"></Settings>

<%--<SettingsText ConfirmDelete="&#39;Are sure you want to update this record as Completed?&#39;"></SettingsText>--%>

                    <Images SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css">
                    <LoadingPanelOnStatusBar Url="~/App_Themes/Office2010Blue/GridView/Loading.gif">
                    </LoadingPanelOnStatusBar>
                    <LoadingPanel Url="~/App_Themes/Office2010Blue/GridView/Loading.gif">
                    </LoadingPanel>
                    </Images>
                    <ImagesFilterControl>
                    <LoadingPanel Url="~/App_Themes/Office2010Blue/GridView/Loading.gif">
                    </LoadingPanel>
                    </ImagesFilterControl>
                    <Styles>
                    <Header SortingImageSpacing="5px" ImageSpacing="5px">
                    </Header>
                    <LoadingPanel ImageSpacing="5px">
                    </LoadingPanel>
                     <AlternatingRow CssClass="GridAltRow" Enabled="True">
                        </AlternatingRow>
                    </Styles>
                    <StylesPager>
                    <PageNumber ForeColor="#3E4846">
                    </PageNumber>
                    <Summary ForeColor="#1E395B">
                    </Summary>
                    </StylesPager>
                    <StylesEditors ButtonEditCellSpacing="0">
                    <ProgressBar Height="21px">
                    </ProgressBar>
                    </StylesEditors>
                    <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" />

<SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True">
</SettingsBehavior>

                    <SettingsPager PageSize="10" SEOFriendly="Enabled" >
                    <PageSizeItemSettings Visible="true" />
<PageSizeItemSettings Visible="True"></PageSizeItemSettings>
                    </SettingsPager>
                </dx:ASPxGridView>
				</dx:PanelContent>
		</PanelCollection>
	</dx:ASPxRoundPanel>
                               </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                    </TabPages>
                    <ContentStyle>
                        <Border BorderColor="#4986A2" />
                        <Border BorderColor="#4986A2"></Border>
                    </ContentStyle>
                </dx:ASPxPageControl>
	
</asp:Content>
