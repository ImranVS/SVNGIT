<%@ Page Title="VitalSigns Plus - Send Log Files" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="SendLogFiles.aspx.cs" Inherits="VSWebUI.Configurator.SendLogFiles" Async="true" %>


<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>






<%@ Register assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.alert-success').delay(10000).fadeOut("slow", function () {
            });
        });
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	
	<%--<script type="text/javascript">
		var prm = Sys.WebForms.PageRequestManager.getInstance();
		prm.add_initializeRequest(InitializeRequest);
		prm.add_endRequest(EndRequest);
		var postBackElement;
		function InitializeRequest(sender, args) {
			if (prm.get_isInAsyncPostBack())
				args.set_cancel(true);
			postBackElement = args.get_postBackElement().name;
			//console.log(args.get_postBackElement().id);
			//console.log((args.get_postBackElement().id).indexOf('ZipButton'));
			//alert(postBackElement.id);

			//if ((postBackElement.id).indexOf('ZipButton') >= 0) {
			if(postBackElement == 'ctl00$ContentPlaceHolder1$ASPxRoundPanel1$ZipButton'){
				//alert('is combo - display');
				$get('ContentPlaceHolder1_ASPxRoundPanel1_WaitingLabel').style.display = 'block';
			}
		}
		function EndRequest(sender, args) {
			if (postBackElement == 'ctl00$ContentPlaceHolder1$ASPxRoundPanel1$ZipButton') {
				$get('ContentPlaceHolder1_ASPxRoundPanel1_WaitingLabel').style.display = 'none';
			}
		}
    </script>--%>


	<table>
	
		<tr>
			<td>
				<div class="header" id="servernamelbldisp" runat="server">Send Log Files</div>
				<%--<dx:ASPxLabel ID="successLabel" runat="server" CssFilePath="~/css/vswebforms.css"
                                            Text="" Visible="false">
                                        </dx:ASPxLabel>--%>
			</td>
		</tr>
        <tr>
            <td>
				<div id="successDiv" runat="server" class="alert alert-success" style="display: none">Test</div>
            </td>
        </tr>
		<tr>
			<td>
				<dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                    CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Log File Info"
                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="630px">
                    <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                    <HeaderStyle Height="23px">
                    </HeaderStyle>
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <table class="style1">
								<tr>
									<td>
										
									</td>
								</tr>
								<tr>
									<td>
										<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
											
											
												<dx:ASPxTreeList ID="LogFilesTree" runat="server" 
													AutoGenerateColumns="False" CssClass="lblsmallFont" 
													CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
													CssPostfix="Office2010Blue" KeyFieldName="ID" 
													ParentFieldName="ParentID" 
													Theme="Office2003Blue">
													<Columns>
														<dx:TreeListTextColumn Caption="File" FieldName="Path" 
															HeaderStyle-CssClass="lblsmallFont" ShowInCustomizationForm="True" 
															VisibleIndex="0">
															<HeaderStyle CssClass="lblsmallFont" />
														</dx:TreeListTextColumn>
												
														<dx:TreeListTextColumn FieldName="OrderNum" ShowInCustomizationForm="True" 
															Visible="False" VisibleIndex="1">
														</dx:TreeListTextColumn>
												
														<dx:TreeListTextColumn FieldName="isFolder" ShowInCustomizationForm="True" 
															Visible="False" VisibleIndex="2">
														</dx:TreeListTextColumn>
													</Columns>
													<Settings GridLines="Both" />
													<SettingsBehavior AllowDragDrop="False" AutoExpandAllNodes="False" />
													<SettingsSelection AllowSelectAll="True" Enabled="True" Recursive="True" />

													<styles cssfilepath="~/App_Themes/Office2010Blue/{0}/styles.css" 
														csspostfix="Office2010Blue">
														<Header CssClass="GridCssHeader">
														</Header>
														<Cell CssClass="GridCss">
														</Cell>
														<loadingpanel imagespacing="5px">
														</loadingpanel>
														<AlternatingNode CssClass="GridAltRow" Enabled="True">
														</AlternatingNode>
													</styles>
													<stylespager>
														<pagenumber forecolor="#3E4846">
														</pagenumber>
														<summary forecolor="#1E395B">
														</summary>
													</stylespager>
													<styleseditors buttoneditcellspacing="0">
													</styleseditors>
												</dx:ASPxTreeList>

											</ContentTemplate>
										</asp:UpdatePanel>

									</td>
								</tr>

								<tr>
									<td>
										<dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" 
												Text="Email address to send Log Files:">
										</dx:ASPxLabel>
									</td>
								</tr>

								<tr>
									<td>
										<dx:ASPxTextBox ID="EmailAddressTextBox" runat="server" Width="170px">
                                            <ValidationSettings>
														<RegularExpression ErrorText="Enter a valid email address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
										    </ValidationSettings>
										</dx:ASPxTextBox>
									</td>
								</tr>

                                <tr>
                                    <td>
										<dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" CssFilePath="~/css/vswebforms.css"
													Text="Use the following button to zip and send your log files to the specified email address" Visible="false">
										</dx:ASPxLabel>
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
                            <div id="errorDiv" class="alert alert-danger" runat="server" style="display: none">One or more log files did not send</div>
                            <div id="errorDiv2" class="alert alert-danger" runat="server" style="display: none">One or more log files did not send</div>
                        </td>
                    </tr>
                    <tr>
									<td>

										<dx:ASPxButton ID="ZipButton" runat="server" Text="Zip and Send" OnClick="ZipButton_Click"
											 ClientIDMode="AutoID" ClientInstanceName="startAjaxRequest" AutoPostBack="true" CssClass="sysButton" />

									</td>
								</tr>
								<tr>
									<td>
										<asp:Label ID="WaitingLabel" runat="server" Text="Zipping and sending the files, please wait." CssClass="lblsmallFont" style="display: none"></asp:Label>
									</td>
								</tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
