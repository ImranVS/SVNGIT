<%@ Page Title="VitalSigns Plus - Welcome Users" Language="C#" MasterPageFile="~/Site1.Master"
    AutoEventWireup="true" CodeBehind="Welcome Users.aspx.cs" Inherits="VSWebUI.Security.Welcome" %>
	<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>








<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

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

    </script>

	<table>
        <tr>
           <td valign="top">
										<div class="header" id="servernamelbldisp" runat="server">
										</div>
									</td>
										
											
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <div id="successDiv" runat="server" class="success" style="display: none">
                                Account settings were successully updated.
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
                                    <td>
                                        <div id="infoDivLoad" class="info">
                                           
                                        Enter the following details below in order to retrieve a forgotten password.
                                        </div>
                                    </td>
                                </tr>
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


														
				
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxLabel ID="UserSecQuestion1Label" runat="server" CssClass="lblsmallFont" CssFilePath="~/css/vswebforms.css"
                                                                    Text="Security Question 1:">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxComboBox ID="UserSecQuestion1ComboBox" runat="server" TextField="SecurityQuestion" ClientIDMode="Static"
                                                                    ValueField="SecurityQuestion" ValueType="System.String"  OnValidation="UserSecQuestion1ComboBox_Validation">
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
                                                                    ValueField="SecurityQuestion" ValueType="System.String" >
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




                                                       
                                                
                                                      
                                                    </table>
                                                    
                                                    <script type="text/javascript">
                                                        var aspxPreviewImgSrc = getPreviewImageElement().src;
                                                    </script>
                                                </td>
                                               <%-- //9/3/2016 sowmya removed for VSPLUS-2661--%>
                                                <%--<td class="imagePreviewCell" align="right">
                                                    <img runat="server" id="previewImage" alt="" height="150" width="100" />
                                                    
                                                </td>--%>
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
