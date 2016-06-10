<%@ Page Title="VitalSigns Plus - My Account" Language="C#" MasterPageFile="~/Site1.Master"
    AutoEventWireup="true" CodeBehind="MyAccount.aspx.cs" Inherits="VSWebUI.Security.MyAccount" %>
	<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>








<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
 <script type="text/javascript" language="javascript">
// 	function Validate(s, e) {
// 		if (ASPxClientEdit.ValidateGroup('testGroup'))
// 			ClientCallbackPanelDemo.PerformCallback('');
// 	}
// 	function UserSecQuestion1ComboBox_Validation(s, e) {
// 		var selectedlitem = document.getElementById("UserSecQuestion1ComboBox");
// 		var selectedlitem2 = document.getElementById("UserSecQuestion1AnsTextBox");
// 		if ((selectedlitem != null) && (selectedlitem2 == null)) {

// 			alert("true");
// 		}
// 		else {
// 			alert("Fail");
// 		}
// 		
// 		if (selecteditem == null || selecteditem == false)
// 			return;
// 		var currentitem = new item();
// 		if (currentitem.getFullYear() != selecteditem.getFullYear() || currentDate.getMonth() != selectedDate.getMonth())
// 			e.isValid = false;
 	// 	}

 	function validate() {

 		// 		alert(document.getElementById('UserSecQuestion1ComboBox_I').value);
 		// 		alert(document.getElementById('UserSecQuestion1AnsTextBox_I').value);

 		var noErr = true;
 		if (document.getElementById('UserSecQuestion1ComboBox_I').value != "") {
 			if (document.getElementById('UserSecQuestion1AnsTextBox_I').value == "") {
// 				alert("Enter Answer to First Question");
 				noErr = false;
 				return false;
 			}
 		}

 		if (document.getElementById('ContentPlaceHolder1_LocationsRoundPanel_UserSecQuestion2ComboBox_I').value != "") {
 			if (document.getElementById('ContentPlaceHolder1_LocationsRoundPanel_UserSecQuestion2AnsTextBox_I').value == "") {
// 				alert("Enter Answer to Second Question");
 				noErr = false;
 				return false;
 			}
 		}
 		if (noErr) {
 			//alert(noErr);
 			return true;
 		} 
 	}

	 function OnCountryChanged(UserSecQuestion1ComboBox) {

                    UserSecQuestion1ComboBox.PerformCallback(UserSecQuestion1ComboBox.GetValue().toString());

                }


                $(document).ready(function () {
                    $('.alert-success').delay(10000).fadeOut("slow", function () {
                    });
                });

    </script>

	<table>
        <tr>
            <td>
                <div class="header" id="servernamelbldisp" runat="server">My Account</div>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <div id="successDiv" runat="server" class="alert alert-success" style="display: none">
                                
                               
                            </div>
                            
                        </td>
                    </tr>
                    <tr>
                        <td>
					
                            <dx:ASPxRoundPanel ID="LocationsRoundPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Account Info"
                                SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css">
                                <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                                <HeaderStyle Height="23px">
                                </HeaderStyle>
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                        <table>
                                            <tr>
                                                <td align="left">
                                                    <table class="style1">
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxLabel ID="UserLoginNameLabel" runat="server" CssClass="lblsmallFont" CssFilePath="~/css/vswebforms.css"
                                                                    Text="Login Name:">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxTextBox ID="UserLoginNameTextBox" runat="server" Width="170px">
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                                        <RegularExpression ErrorText="" />
                                                                        <RequiredField IsRequired="True" ErrorText="Enter Login Name" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                            </td>
															<td>
															</td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxLabel ID="UserFullNameLabel" runat="server" CssClass="lblsmallFont" CssFilePath="~/css/vswebforms.css"
                                                                    Text="Full Name:">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxTextBox ID="UserFullNameTextBox" runat="server" Width="170px">
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                                        <RequiredField IsRequired="True" ErrorText="Enter Full Name" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxLabel ID="UserEmailLabel" runat="server" CssClass="lblsmallFont" CssFilePath="~/css/vswebforms.css"
                                                                    Text="Email:">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxTextBox ID="UserEmailTextBox" runat="server" Width="170px">
                                                                    <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithTooltip">
                                                                        <RequiredField ErrorText="Enter email-ID" IsRequired="True" />
                                                                        <RegularExpression ErrorText="Please enter valid E-mail ID" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>


														<%--<div style="float: left; margin-left: 2%" id="summaryContainer">
        <dx:ASPxValidationSummary ID="vsValidationSummary1" runat="server" RenderMode="BulletedList"
            Width="250px" ClientInstanceName="validationSummary">
        </dx:ASPxValidationSummary>
    </div>
--%>

														 <%--<dx:ASPxCallbackPanel ID="ASPxCallbackPanelDemo" runat="server"  HideContentOnCallback="False"
            ClientInstanceName="ClientCallbackPanelDemo" OnCallback="ASPxCallbackPanelDemo_Callback">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent2" runat="server">--%>
				
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxLabel ID="UserSecQuestion1Label" runat="server" CssClass="lblsmallFont" CssFilePath="~/css/vswebforms.css"
                                                                    Text="Security Question 1:">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxComboBox ID="UserSecQuestion1ComboBox" runat="server" TextField="SecurityQuestion" ClientIDMode="Static"
                                                                    ValueField="SecurityQuestion"  ValueType="System.String"  
                                                                     OnValidation="UserSecQuestion1ComboBox_Validation">
                                                                    <LoadingPanelImage Url="~/App_Themes/Office2010Blue/Editors/Loading.gif">
                                                                    </LoadingPanelImage>
                                                                    <LoadingPanelStyle ImageSpacing="5px">
                                                                    </LoadingPanelStyle>
                                                                    <ButtonStyle Width="13px">
                                                                    </ButtonStyle>
                        <InvalidStyle BackColor="LightPink" />
						                                             <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                                        <RequiredField IsRequired="True" ErrorText="It's required" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxComboBox>
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxLabel ID="UserSecQuestion1AnsLabel" runat="server" CssClass="lblsmallFont"
                                                                    CssFilePath="~/css/vswebforms.css" Text="Security Question 1 Answer:">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxTextBox ID="UserSecQuestion1AnsTextBox" ClientIDMode="Static" runat="server" Width="170px" >
                                                                   <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                                        <RequiredField IsRequired="True" ErrorText="It's required" />
                                                                    </ValidationSettings>  
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxLabel ID="UserSecQuestion2Label" runat="server" CssClass="lblsmallFont" CssFilePath="~/css/vswebforms.css"
                                                                    Text="Security Question 2:">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxComboBox ID="UserSecQuestion2ComboBox" runat="server" TextField="SecurityQuestion"
                                                                    ValueField="SecurityQuestion" ValueType="System.String" 
                                                                    >
                                                                    <LoadingPanelImage Url="~/App_Themes/Office2010Blue/Editors/Loading.gif">
                                                                    </LoadingPanelImage>
                                                                    <LoadingPanelStyle ImageSpacing="5px">
                                                                    </LoadingPanelStyle>
                                                                    <ButtonStyle Width="13px">
                                                                    </ButtonStyle>
                                                                     <ValidationSettings ErrorDisplayMode="ImageWithTooltip"  SetFocusOnError="True">
                                                                        <RequiredField IsRequired="True" ErrorText="It's required" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxComboBox>
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxLabel ID="UserSecQuestion2AnsLabel" runat="server" CssClass="lblsmallFont"
                                                                    CssFilePath="~/css/vswebforms.css" Text="Security Question 2 Answer:">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxTextBox ID="UserSecQuestion2AnsTextBox" runat="server" Width="170px">
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                                        <RequiredField IsRequired="True" ErrorText="It's required" />
                                                                    </ValidationSettings> 
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>


				<%--										
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxCallbackPanel>--%>




                                                        <tr>
                                                            <td>
                                                                <dx:ASPxLabel ID="RefreshLabel" runat="server" CssClass="lblsmallFont" Text="Refresh Time:">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxTextBox ID="RefreshTextBox" runat="server" Width="170px">
                                                                    <MaskSettings Mask="&lt;1..99999&gt;" />
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" CausesValidation="True" SetFocusOnError="True">
                                                                        <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)."
                                                                            ValidationExpression="^\d+$" />
                                                                        <RequiredField ErrorText="It's required" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
															
                                                            </td>
                                                            
                                                           <td>
														   <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="seconds" CssClass="lblsmallFont">
                                                                </dx:ASPxLabel>
														   </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxLabel ID="StartupURLLabel" runat="server" CssClass="lblsmallFont" Text="Startup Page:">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxComboBox ID="StartupURLCombobox" runat="server" TextField="Name" ValueField="URL"
                                                                    ValueType="System.String" >
                                                                    <LoadingPanelImage Url="~/App_Themes/Office2010Blue/Editors/Loading.gif">
                                                                    </LoadingPanelImage>
                                                                    <LoadingPanelStyle ImageSpacing="5px">
                                                                    </LoadingPanelStyle>
                                                                    <ButtonStyle Width="13px">
                                                                    </ButtonStyle>
                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                                        <RequiredField IsRequired="True" ErrorText="It's required" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">
                                                                <dx:ASPxPopupControl ID="ErrorMessagePopupControl" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                                    CssPostfix="Glass" HeaderText="Information" Height="150px" Modal="True" PopupHorizontalAlign="WindowCenter"
                                                                    PopupVerticalAlign="WindowCenter" Width="300px">
                                                                    <ContentCollection>
                                                                        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                                                                            <table class="tableWidth100Percent">
                                                                                <tr>
                                                                                    <td>
                                                                                        <dx:ASPxLabel ID="ErrorMessageLabel" runat="server" Text="ASPxLabel">
                                                                                        </dx:ASPxLabel>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="center">
                                                                                        <dx:ASPxButton ID="ValidationOKButton" runat="server" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
                                                                                            CssPostfix="Office2010Blue" OnClick="ValidationOKButton_Click" Text="OK" Width="80px">
                                                                                        </dx:ASPxButton>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </dx:PopupControlContentControl>
                                                                    </ContentCollection>
                                                                </dx:ASPxPopupControl>
                                                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" ForeColor="#CC0099">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table>
														<tr>
														<td>
                                                                <dx:ASPxCheckBox ID="checkbx" runat="server" Text="Custom Background" OnCheckedChanged="checkbx_CheckedChanged"
                                                                    AutoPostBack="true">
                                                                </dx:ASPxCheckBox>
                                                            </td>
														</tr>
                                                        <tr>
                                                            <td class="caption">
                                                                <dx:ASPxLabel ID="lblSelectImage" runat="server" CssClass="lblsmallFont" Text="Select Image:">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxUploadControl ID="uplImage" runat="server" ClientInstanceName="uploader"
                                                                    ShowProgressPanel="false" NullText="Click here to browse files..." Size="20"
                                                                    Width="205" OnFileUploadComplete="uplImage_FileUploadComplete">
                                                                    <ClientSideEvents FileUploadComplete="function(s, e) { Uploader_OnFileUploadComplete(e); }"
                                                                        FilesUploadComplete="function(s, e) { Uploader_OnFilesUploadComplete(e); }" FileUploadStart="function(s, e) { Uploader_OnUploadStart(); }"
                                                                        TextChanged="function(s, e) { UpdateUploadButton(); }"></ClientSideEvents>
                                                                    <ValidationSettings MaxFileSize="4194304" AllowedFileExtensions=".jpg,.jpeg,.jpe,.gif">
                                                                    </ValidationSettings>
                                                                </dx:ASPxUploadControl>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td class="note">
                                                                <dx:ASPxLabel ID="lblAllowebMimeType" runat="server" CssClass="lblsmallFont" Text="Allowed image types: jpeg, gif"
                                                                    Font-Size="8pt">
                                                                </dx:ASPxLabel>
                                                                <br />
                                                                <dx:ASPxLabel ID="lblMaxFileSize" runat="server" Text="Maximum file size: 4Mb" CssClass="lblsmallFont"
                                                                    Font-Size="8pt">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" class="buttonCell">
                                                                <dx:ASPxButton ID="btnUpload" runat="server" AutoPostBack="true" Text="Upload" ClientInstanceName="btnUpload"
                                                                    Width="100px" ClientEnabled="False" Style="margin: 0 auto;" Visible="false">
                                                                    <ClientSideEvents Click="function(s, e) { uploader.Upload(); }" />
                                                                    <ClientSideEvents Click="function(s, e) { uploader.Upload(); }"></ClientSideEvents>
                                                                </dx:ASPxButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <script type="text/javascript">
                                                        var aspxPreviewImgSrc = getPreviewImageElement().src;
                                                    </script>
                                                </td>
                                                <td class="imagePreviewCell" align="right">
                                                    <img runat="server" id="previewImage" alt="" height="150" width="100" />
                                                    
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
          
           <div class="info">If none of the elements are selected, all of them will be displayed on the Dashboard by default.
                </div>
                    </td>
                    </tr>
					<tr>
					<td>
					<dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" 
                                                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                                                    GroupBoxCaptionOffsetY="-24px" HeaderText="Customize Elements of Dashboard" 
                                                    
                            SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css">
                                                    <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                                                    <HeaderStyle Height="23px">
                                                    </HeaderStyle>
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                                                            <table>
                                                                <tr>
								                                    <td>
								                                         <dx:ASPxCheckBox ID="CloudApplicationscheckbx" runat="server" Text="Cloud Applications" OnCheckedChanged="checkbx_CheckedChanged"  AutoPostBack="false">
								                                           </dx:ASPxCheckBox>
								                                                                
								                                                            </td>
																							 <td>
								                                                                <dx:ASPxCheckBox ID="OnPremisesApplicationscheckbx" runat="server" Text="On-Premises Applications" OnCheckedChanged="checkbx_CheckedChanged"
								                                                                    AutoPostBack="false">
								                                                                </dx:ASPxCheckBox>
								                                                            </td>
								                                                        </tr>
																						<tr>
								                                                            <td>
								                                                                <dx:ASPxCheckBox ID="NetworkInfrastructurecheckbx" runat="server" Text="Network Infrastructure" OnCheckedChanged="checkbx_CheckedChanged"
								                                                                    AutoPostBack="false">
								                                                                </dx:ASPxCheckBox>
								                                                            </td>
																							<td>
								                                                                <dx:ASPxCheckBox ID="DominoServerMetricscheckbx" runat="server" Text="Domino Server Metrics" OnCheckedChanged="checkbx_CheckedChanged"
								                                                                    AutoPostBack="false">
								                                                                </dx:ASPxCheckBox>
								                                                            </td>
																							
                                                        </tr>
                                                            </table>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                               </dx:ASPxRoundPanel>

					</td>
					</tr>
                </table>
            </td>
        </tr>		
        <tr>
            <td>
           
                <table>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="UserInfoSaveButton" runat="server" CssClass="sysButton"
                                            Text="Save" OnClick="UserInfoSaveButton_Click" >
											 <%--<ClientSideEvents Click="validate"  />--%>
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="CancelButton" runat="server" CssClass="sysButton"
                                        OnClick="CancelButton_Click"
                                            Text="Cancel" CausesValidation="False">
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
