<%@ Page Title="Applications" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
	CodeBehind="Applications.aspx.cs" Inherits="VSWebUI.Configurator.Applications" %>
<%@ MasterType virtualpath="~/Site1.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>

	



<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx1" %>
    
    
    

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet'
		type='text/css' />
	<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
	<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
	<script src="../js/bootstrap.min.js" type="text/javascript"></script>
	<script type="text/javascript">
		$(document).ready(function () {
			$('.alert-success').delay(10000).fadeOut("slow", function () {
			});
		});
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
		//10/30/2013 NS added - fix for when an Enter key is pressed within the text box on the page - redirect the
		//submit function to the actual Go button on the page instead of performing a whole page submit
		function OnKeyDown(s, e) {
			//alert(window.event.keyCode);
			//var keyCode = (window.event) ? e.which : e.keyCode;
			//alert(keyCode);
			var keyCode = window.event.keyCode;
			if (keyCode == 13)
				goButton.DoClick();
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table>
		<tr>
			<td>
				<div class="header" id="servernamelbldisp" runat="server">
					WebSphere Applications</div>
			</td>
		</tr>
		<tr>
			<td>
				<table>
					
					<tr>
						<td>
							<div id="successDiv" runat="server" class="alert alert-success" style="display: none">
								Success.
							</div>
							<div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">
								Error.
							</div>
							
						</td>
					</tr>
				
					
				</table>
			</td>	
               
		</tr>
        <tr>
        <td>
         <dx:ASPxCallback ID="cb" runat="server" ClientInstanceName="cb" OnCallback="cb_callback" />
        <dx:ASPxGridView ID="ServicesGrid" runat="server" 
                                                    AutoGenerateColumns="False" ClientInstanceName="WindowsGridView" 
                                                    EnableTheming="True" KeyFieldName="ID"  Theme="Office2003Blue" OnPageSizeChanged="ServicesGrid_PageSizeChanged"
                                                    Width="100%" OnHtmlDataCellPrepared="ServicesGrid_HtmlDataCellPrepared" >
                                                    <Columns>
														
                                                        <dx:GridViewDataColumn Caption="Select" VisibleIndex="0" 
                                                            CellStyle-HorizontalAlign="Center" Width="50px">
															<DataItemTemplate>                                                           
															<dx:ASPxCheckBox ID="checkToMonitor" runat="server" OnInit="checkToMonitor_Init" Value='<%# Eval("isSelected") %>' />
															</DataItemTemplate>
															<HeaderStyle CssClass="GridCssHeader1" />
															<CellStyle HorizontalAlign="Center" CssClass="GridCss1">
															</CellStyle>
                                                        </dx:GridViewDataColumn>
														
														<dx:GridViewDataTextColumn Caption="Service Name" VisibleIndex="0" FieldName="ServiceName">
															<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
															<EditCellStyle CssClass="GridCss">
															</EditCellStyle>
															<EditFormCaptionStyle CssClass="GridCss">
															</EditFormCaptionStyle>
															<HeaderStyle CssClass="GridCssHeader" />
															<CellStyle CssClass="GridCss">
															</CellStyle> 
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Startup Mode" VisibleIndex="2" FieldName="StartMode"
															Width="180px">
															<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
															<EditCellStyle CssClass="GridCss">
															</EditCellStyle>
															<EditFormCaptionStyle CssClass="GridCss">
															</EditFormCaptionStyle>
															<HeaderStyle CssClass="GridCssHeader" />
															<CellStyle CssClass="GridCss">
															</CellStyle>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Status" VisibleIndex="2" FieldName="Result"
															Width="180px">
															<Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
															<EditCellStyle CssClass="GridCss">
															</EditCellStyle>
															<EditFormCaptionStyle CssClass="GridCss">
															</EditFormCaptionStyle>
															<HeaderStyle CssClass="GridCssHeader" />
															<CellStyle CssClass="GridCss">
															</CellStyle>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Currently Monitored" VisibleIndex="1" FieldName="Monitored" Visible="false">
															<HeaderStyle CssClass="GridCssHeader" />
															<Settings AllowAutoFilter="False" />
															<CellStyle CssClass="GridCss1" HorizontalAlign="Center">
															</CellStyle>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="ID" VisibleIndex="1" FieldName="ID" Visible="false">
															<HeaderStyle CssClass="GridCssHeader" />
															<Settings AllowAutoFilter="False" />
															<CellStyle CssClass="GridCss1" HorizontalAlign="Center">
															</CellStyle>
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="isSelected" FieldName="isSelected" 
                                                            VisibleIndex="4" Visible="false">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Type" FieldName="Type" 
															VisibleIndex="4" Visible="false">
														</dx:GridViewDataTextColumn>
                                                    </Columns>

													<Templates>
														<GroupRowContent>
															<%# Container.GroupText %>
														</GroupRowContent>
													</Templates>



													<SettingsBehavior AllowDragDrop="False" AutoExpandAllGroups="True" AllowSort="true" />


													<Settings ShowFilterRow="True" />
													<SettingsPager PageSize=20>
                                                        <PageSizeItemSettings Visible="True">
                                                        </PageSizeItemSettings>
                                                    </SettingsPager>
                                                    <Styles>
                                                        <AlternatingRow CssClass="GridAltRow">
                                                        </AlternatingRow>
														<GroupRow Font-Bold="true" />
                                                    </Styles>
                                                </dx:ASPxGridView>
        </td></tr>

        <tr>
            <td>
            <div id="Div1" class="alert alert-danger" runat="server" style="display: none">Error.
            <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                                        </div>
                <table>
                    <tr>
                        <td>
                        
                            <dx:ASPxButton ID="FormOkButton" runat="server" Text="OK" CssClass="sysButton" onclick="FormOkButton_Click" >
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="FormCancelButton" runat="server" Text="Cancel" CssClass="sysButton" onclick="FormCancelButton_Click" >
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
	</table>
</asp:Content>
