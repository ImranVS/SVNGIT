<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Licensekey.aspx.cs" Inherits="License.licensekey" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>



<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>

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

		function OnItemClick(s, e) {
			if (e.item.parent == s.GetRootItem())
				e.processOnServer = false;
		}
		



	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table width="100%">
		<tr>
			
			<td>
				<div class="header" id="lblServer" runat="server">
					License Key Generator</div>
			</td>
			
		</tr>
	                                        <tr>
											<td>
												<dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
													CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="" 
													SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="95%">
													<ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
													<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>
													<PanelCollection>
													<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
													<table align="left" class="style3">
													<tr>
													<td >
													<dx:ASPxLabel ID="ASPxLabel68" runat="server" align="left" CssClass="lblsmallFont" Text="Company Name">
													</dx:ASPxLabel>
													</td>
													<td align="center">
													:
													</td>
												
													<td>
                                                    <dx:ASPxComboBox ID="CompanyNameComboBox" runat="server" AutoPostBack="true" Width="170px" OnSelectedIndexChanged="CompanyNameComboBox_SelectedIndexChanged"  >
                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True" 
                                                     ErrorText="Users may not be empty.">
                                                    <RequiredField IsRequired="True" ErrorText="" />
                                                    </ValidationSettings>
                                                    </dx:ASPxComboBox>
                                                    </td>
													</tr>
													<tr>
													<td >
													<dx:ASPxLabel ID="ASPxLabel52" runat="server" align="left" CssClass="lblsmallFont" Text="Install Type">
													</dx:ASPxLabel>
													</td>
													<td align="center">
													:
													</td>
													
													<td>
													<dx:ASPxRadioButtonList ID="InstallTypeList1" runat="server" AutoPostBack="False" 
													OnSelectedIndexChanged="InstallTypeRadioButton_SelectedIndexChanged" RepeatDirection="Horizontal" >
													<Items>
													<dx:ListEditItem Text="HA" Value="1" />
													<dx:ListEditItem Text="Standalone" Value="2" />
													</Items>
													<%--<ValidationSettings>
													<RequiredField IsRequired="true" ErrorText="" /></ValidationSettings>--%>
													<ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True" 
                                                     ErrorText="Users may not be empty.">
                                                    <RequiredField IsRequired="True" ErrorText="" />
                                                    </ValidationSettings>
													</dx:ASPxRadioButtonList>
													</td>
													<td align="left">
													&nbsp;
													</td>
													</tr>
													<tr>
													<td>
													<dx:ASPxLabel ID="ASPxLabel53" runat="server"  align="left" CssClass="lblsmallFont" Text="License Type">
													</dx:ASPxLabel>
													</td>
												    <td align="center">
													:
													</td>
													
													<td>
													<dx:ASPxRadioButtonList ID="LicenseType" runat="server" AutoPostBack="True" 
													OnSelectedIndexChanged="LicenseTypeRadioButton_SelectedIndexChanged" RepeatDirection="Horizontal" >
													<Items>
													<dx:ListEditItem Text="Evaluation" Value="1" />
													<dx:ListEditItem Text="Subscription" Value="2" />
													<dx:ListEditItem Text="Perpetual" Value="3" />
													</Items>
													 <ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True" 
                                                     ErrorText="Users may not be empty.">
                                                    <RequiredField IsRequired="True" ErrorText="" />
                                                    </ValidationSettings>
													</dx:ASPxRadioButtonList>
													</td>
													</tr>
													<tr>
													<td >
													<dx:ASPxLabel ID="ASPxLabel1" runat="server" align="left" CssClass="lblsmallFont" Text="Expiration Date">
													</dx:ASPxLabel>
													</td>
													<td align="center">
													:
													</td>
													<td>
													<%--<dx:ASPxRadioButtonList ID="ExpirationDateList" runat="server" AutoPostBack="True" 
													OnSelectedIndexChanged="ExpirationDateRadioButton_SelectedIndexChanged" RepeatDirection="Horizontal" >
													<Items>
													<dx:ListEditItem Text="1Month" Value="1" />
													<dx:ListEditItem Text="1Year" Value="2" />
													</Items>
													</dx:ASPxRadioButtonList>--%>
													<dx:ASPxButton ID="MonthButton1" runat="server" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
								                    CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
								                    Text="1Month"  CausesValidation="False" Theme="Office2010Blue" OnClick="MonthButton_Click">
							                        </dx:ASPxButton>
							                        <dx:ASPxButton ID="YearButton1" runat="server" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
								                    CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
								                    Text="1Year"  CausesValidation="False" Theme="Office2010Blue" OnClick="YearButton_Click">
							                        </dx:ASPxButton>
													
													
												   <dx:ASPxDateEdit ID="ExpirationDate" runat="server"  EditFormatString="MM/dd/yyyy">
														
														</dx:ASPxDateEdit>
													</td>
													
													</tr>
													<tr>
													<td >
													<dx:ASPxLabel ID="ASPxLabel2" runat="server" align="left" CssClass="lblsmallFont" Text="Units">
													</dx:ASPxLabel>
													</td>
													<td align="center">
													:
													</td>
													<td align="left">
												<%--	<dx:ASPxTextBox ID="UnitsTextBox" EnableClientSideAPI="True"  runat="server" AutoPostBack="True" CssClass="txtsmall"
													Width="170px"   ClientInstanceName="Units">--%>
												   
											
											
													<%--<ValidationSettings>
													
													<RequiredField IsRequired="True"></RequiredField>							
													
                                                    <RegularExpression ErrorText="Please enter a numeric value " ValidationExpression="^\d+$"></RegularExpression>
													</ValidationSettings>
													 
													</dx:ASPxTextBox>--%>
													
													<dx:ASPxSpinEdit ID="UnitsTextBox" runat="server">
                                                    <SpinButtons ShowIncrementButtons="False" ShowLargeIncrementButtons="false" />
				                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True" 
                                                     ErrorText="Users may not be empty.">
                                                    <RequiredField IsRequired="True" ErrorText="" />
                                                    </ValidationSettings>
                                                    </dx:ASPxSpinEdit>
			 
													</td>
													</tr>
													</table>
												    </dx:PanelContent>
													</PanelCollection>
												</dx:ASPxRoundPanel>
											</td>
										</tr>
		                               <tr>
			                           <td colspan="2">
				                       <div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">
					                    Error attempting to update the status table.
					                    <button type="button" class="close" data-dismiss="alert">
						                <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
				                       </div>
				<table>
					<tr>
						<td align="right">
							<dx:ASPxButton ID="okButton" runat="server" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
								CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
								Text="OK" OnClick="OK_Click"  Theme="Office2010Blue">
							</dx:ASPxButton>
						</td>
						<%--<td>
						<dx:ASPxButton ID="okButton" runat="server" Text="OK" OnClick="OK_Click" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css">
						</dx:ASPxButton>
						<dx:ASPxButton ID="okButton" runat="server" Text="OK" OnClick="OK_Click" AutoPostBack="true" Checked="true">
						</dx:ASPxButton>
						</td>--%>
						<td>
							<dx:ASPxButton ID="CancelButton" runat="server" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
								CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
								Text="Cancel"  CausesValidation="False" Theme="Office2010Blue" onclick="CancelButton_Click">
							</dx:ASPxButton>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	
	<div id="successDiv" runat="server" class="alert alert-success" style="display: none">
		Status information updated successfully.
		<button type="button" class="close" data-dismiss="alert">
			<span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
	</div>
</asp:Content>
