<%@ Page  Title="VitalSigns Plus-Server Credentials"  Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="PwrshellCredentials.aspx.cs" Inherits="VSWebUI.Configurator.PwrshellCredentials" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript">
//    	function OnTextBoxKeyPress(s, e) {
//    		//alert("hi");
//    		if (e.htmlEvent.keyCode == 13) {
//    			ASPxClientUtils.PreventEventAndBubble(e.htmlEvent);
//    		}
//    	}
    	var visibleIndex;

    	function OnCustomButtonClick(s, e) {
    		visibleIndex = e.visibleIndex;

    		if (e.buttonID == "deleteButton")
    			AliasGridView.GetRowValues(e.visibleIndex, 'AliasName', OnGetRowValues);

    		function OnGetRowValues(values) {
    			var id = values[0];
    			var name = values[1];
    			var OK = (confirm('Are sure you want to delete these credentials - ' + values + '?'))
    			if (OK == true) {
    				//					alert('Before Delete');
    				//					var name1 = ServersGridView.DeleteRow(visibleIndex);
    				AliasGridView.DeleteRow(visibleIndex);
    				//alert('The Server ' + values + ' was Successfully Deleted');
    				//					alert(name1);
    				//ScriptManager.RegisterClientScriptBlock(base.Page, this.GetType(), "FooterRequired", "alert('Notification : Record deleted successfully');", true);
    			}

    			else {
    			}

    		}
    	}

    	
					
        </script>
    <style type="text/css">
.dxeTextBoxSys, .dxeMemoSys
{
    border-collapse:separate!important;
}

        .style1
        {
            width: 100%;
            font-size: 0;
        }
        
    </style>
    
    <%--<script type="text/javascript">
        function HidePopup() {
            var popup = document.getElementById('ContentPlaceHolder1_pnlAreaDtls');
            popup.style.visibility = 'hidden';
        }
    </script>--%>
	 <script type="text/javascript">
	 	$(document).ready(function () {

	 		$('.alert-success').delay(10000).fadeOut("slow", function () {

	 		});
	 	});
        </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1"  runat="server">
	<asp:Button ID="btnDisable" runat="server" OnClientClick="return false;" style="display:none;" UseSubmitBehavior="true"/>
    <table>
	<tr>
	<td>
				    <div id="errordiv1" class="alert alert-danger" runat="server" style="display: none">
						                   </div>
				</td>
	</tr>
    <tr>
	
        <td>
            <div class="header" id="servernamelbldisp" runat="server">Server Credentials</div>
            <div id="successDiv" class="alert alert-success" runat="server" style="display: none">Success.
                </div>
            <asp:Label ID="lblmessage" runat="server" Style="font-weight: bold; color: #f00;"> </asp:Label>
            <div id="errorDiv" class="alert alert-danger" runat="server" style="display: none">Error.
            </div>
        </td>
    </tr>
            <tr>
                <td>
                    <dx:ASPxButton ID="NewButton" runat="server" Text="New" CssClass="sysButton" AutoPostBack="False">
                        <ClientSideEvents Click="function() { AliasGridView.AddNewRow(); }" />
                                <Image Url="~/images/icons/add.png">
                                </Image>
                    </dx:ASPxButton>
                </td>
            </tr>
			<tr>
			<td>
            <dx:ASPxGridView runat="server" 
                                KeyFieldName="ID" AutoGenerateColumns="False" 
                                ID="AliasGridView" 
				OnRowDeleting="AliasGridView_RowDeleting" ClientInstanceName="AliasGridView"
                                OnRowInserting="AliasGridView_RowInserting" 
                                OnRowUpdating="AliasGridView_RowUpdating" OnPageSizeChanged="AliasGridView_PageSizeChanged"
        Theme="Office2003Blue" EnableTheming="True" oncelleditorinitialize="AliasGridView_CellEditorInitialize" 
		OnAutoFilterCellEditorInitialize="AliasGridView_AutoFilterCellEditorInitialize" 
				CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
				CssPostfix="Office2010Blue" oncustomerrortext="AliasGridView_CustomErrorText" >
		<ClientSideEvents CustomButtonClick="OnCustomButtonClick" />
        <Columns>
            <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" 
                                        FixedStyle="Left" VisibleIndex="0" Width="10px">
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
                <CellStyle CssClass="GridCss1">
                    <Paddings Padding="3px" />
<Paddings Padding="3px"></Paddings>
                </CellStyle>
                <ClearFilterButton Visible="True">
                    <Image Url="~/images/clear.png">
                    </Image>
                </ClearFilterButton>
                <HeaderStyle CssClass="GridCssHeader1" />
            </dx:GridViewCommandColumn>
            <%--<dx:GridViewDataTextColumn Caption="Delete" VisibleIndex="1" CellStyle-HorizontalAlign="Center" Width="30px" CellStyle-VerticalAlign="Middle">
                <DataItemTemplate>
                    <asp:Label ID="lblLocation" runat="server" Text='<%#Eval("AliasName") %>' Visible="False"></asp:Label>
                    <asp:ImageButton ID="deleteButton" runat="server" ImageUrl="../images/delete.png" Width="15px" Height="15px" 
                    CommandName="Delete" CommandArgument='<%#Eval("ID") %>' AlternateText='<%#Eval("AliasName") %>' ToolTip="Delete" 
                    OnClick="bttnDelete_Click"/>
                </DataItemTemplate>
				<HeaderStyle CssClass="GridCssHeader" />
                <EditFormSettings Visible="False"/>
<EditFormSettings Visible="False"></EditFormSettings>

                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss1">
                </CellStyle>
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewDataTextColumn>--%>
			 <dx:GridViewCommandColumn   Caption="Delete" ButtonType="Image" 
				VisibleIndex="1">
                        <CustomButtons>
                            <dx:GridViewCommandColumnCustomButton ID="deleteButton" 
								Image-Url="../images/delete.png" Text="Delete" >
<Image Url="../images/delete.png"></Image>
							</dx:GridViewCommandColumnCustomButton>
                        </CustomButtons>
						<HeaderStyle CssClass="GridCssHeader1" />
                        <CellStyle CssClass="GridCss1">
                        </CellStyle>
                    </dx:GridViewCommandColumn>

            <dx:GridViewDataTextColumn FieldName="ID"  Visible="False" 
                                    VisibleIndex="2">
                <EditFormSettings Visible="False">
                </EditFormSettings>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="AliasName" VisibleIndex="3" Caption="Alias">
                <PropertiesTextEdit>
                    <ValidationSettings CausesValidation="True" SetFocusOnError="True">
                        <RequiredField ErrorText="Enter Alias" IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
               <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />

<Settings AutoFilterCondition="Contains"></Settings>

                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="User ID" FieldName="UserID" 
                VisibleIndex="4">
                <PropertiesTextEdit>
                    <ValidationSettings CausesValidation="True" SetFocusOnError="True">
                        <RequiredField ErrorText="Enter User ID" IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />

<Settings AutoFilterCondition="Contains"></Settings>

                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <%--<dx:GridViewDataTextColumn Caption="Password"  FieldName="Password" 
                VisibleIndex="6">
                <PropertiesTextEdit Password=true>
                    <ValidationSettings CausesValidation="True" SetFocusOnError="True">
                        <RequiredField ErrorText="Enter Password" IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>

<Settings AutoFilterCondition="Contains"></Settings>

                <Settings AutoFilterCondition="Contains" />

                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
				<EditItemTemplate>
				<dx:ASPxTextBox ID="CredPassword" Text='<%# Eval("Password") %>' runat="server" type="password"
						 />
				</EditItemTemplate>
               <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>--%>

			 <dx:GridViewDataTextColumn FieldName="Password" VisibleIndex="6">
               <%-- <PropertiesTextEdit Password="True" ClientInstanceName="psweditor">
                </PropertiesTextEdit>--%>
				 <PropertiesTextEdit Password=true>
                    <ValidationSettings CausesValidation="True" SetFocusOnError="True">
                        <RequiredField ErrorText="Enter Password" IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
				<Settings AutoFilterCondition="Contains"></Settings>
				<EditCellStyle CssClass="GridCss">
                </EditCellStyle>
			
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <EditItemTemplate>
                  
                  
                  <%--  <dx:aspxtextbox ID="pswtextbox" runat="server" Text='<%# Bind("Password") %>' 
                        Visible='<%# AliasGridView.IsNewRowEditing %>' Password="True">
                      
                    </dx:aspxtextbox>--%>
					<%--1/25/2016 Durga modified for VSPLUS-2500--%>
					<table><tr><td>
  <asp:linkbutton ID="LinkButton2" runat="server"   OnClientClick="popup3.ShowAtElement(this); return false;" 
					Visible='<%#AliasGridView.IsNewRowEditing%>'><u>Add password</u>
                    </asp:linkbutton>
				
                    <asp:linkbutton ID="LinkButton1" runat="server"  OnClientClick="popup2.ShowAtElement(this); return false;"
					Visible='<%#!AliasGridView.IsNewRowEditing%>'><u>Edit password</u></asp:linkbutton>
					
					</td></tr>
					<tr ><td colspan="1"><asp:Label ID="msg" runat="server" CssClass="alert alert-success" style="display: none"></asp:Label></td></tr>
					</table>
              </EditItemTemplate>
              
			  	<HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>


			        <dx:GridViewDataComboBoxColumn FieldName="ServerType" VisibleIndex="5" 
                UnboundType="String">
                <PropertiesComboBox  TextField="ServerType" 
                    ValueField="ServerType">
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                    
                </PropertiesComboBox>
                <Settings AllowAutoFilter="True" AllowHeaderFilter="False" />
<EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
            </dx:GridViewDataComboBoxColumn>

        </Columns>
        <SettingsBehavior ConfirmDelete="True">
        </SettingsBehavior>
        <Settings ShowFilterRow="True" />
        <SettingsText ConfirmDelete="Are you sure you want to delete this credential?">
        </SettingsText>
        <Styles CssPostfix="Office2010Blue" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css">
            <Header SortingImageSpacing="5px" ImageSpacing="5px">
            </Header>
            <LoadingPanel ImageSpacing="5px">
            </LoadingPanel>
            <AlternatingRow CssClass="GridAltRow" Enabled="True">
            </AlternatingRow>
            <EditForm CssClass="GridCssEditForm">
            </EditForm>
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
        <SettingsPager PageSize="10" SEOFriendly="Enabled" >
            <PageSizeItemSettings Visible="true" />
<PageSizeItemSettings Visible="True"></PageSizeItemSettings>
        </SettingsPager>
    </dx:ASPxGridView>
        </td>
    </tr>
</table>

<dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" Text="Are you sure?" ClientInstanceName="popup">
                <ContentCollection>
                    <dx:PopupControlContentControl>
                        <dx:ASPxButton ID="yesButton" runat="server" Text="Yes" AutoPostBack="false">
                           <%-- <ClientSideEvents Click="OnClickYes" />--%>
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="noButton" runat="server" Text="No" AutoPostBack="false">
                            <%--<ClientSideEvents Click="OnClickNo" />--%>
                        </dx:ASPxButton>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="NavigatorPopupControl" runat="server" ClientInstanceName="popup1" 
    HeaderText="Information" Modal="True" PopupHorizontalAlign="WindowCenter" 
    PopupVerticalAlign="WindowCenter" Theme="Glass">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">
                <table class="style1">
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="MsgLabel" runat="server" ClientInstanceName="poplbl">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxButton ID="OKButton" runat="server" OnClick="subbttnOK_Click" Text="OK" 
                                Theme="Office2010Blue">
                            </dx:ASPxButton>                                                            
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

	<dx:aspxpopupcontrol ID="ASPxPopupControl2" runat="server" 
        HeaderText="Edit password" Width="307px" ClientInstanceName="popup2" 
        Theme="MetropolisBlue">
         <ClientSideEvents Closing="function(s, e) {
        ASPxClientEdit.ClearEditorsInContainerById('contentDiv');
    }"/>
      <ClientSideEvents PopUp="function(s, e) {
        ASPxClientEdit.ClearEditorsInContainerById('contentDiv');
    }" />

            <contentcollection>

                <dx:popupcontrolcontentcontrol ID="Popupcontrolcontentcontrol3" runat="server">
                <div id="contentDiv">
                <table>
                  <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Enter new password:" 
                            Wrap="False">
                        </dx:ASPxLabel>
                        </td>
                    <td>
                      <dx:aspxtextbox ID="npsw" runat="server" Password="True"  ClientInstanceName="npsw">
                          <%--<clientsideevents Validation="function(s, e) {e.isValid = (s.GetText().length>5)}" />--%>
                          <validationsettings ErrorDisplayMode="ImageWithTooltip"   ErrorText="The password length should be more than 5 characters">
                          </validationsettings >
                          <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
<RequiredField IsRequired="True" ErrorText="Enter Password"></RequiredField>
</ValidationSettings>
                      </dx:aspxtextbox>
                    </td>
                  </tr>
                  <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Confirm new password:" 
                             Wrap="False">
                        </dx:ASPxLabel>
                        </td>
                    <td>
                      <dx:aspxtextbox ID="cnpsw" runat="server" Password="True" ClientInstanceName="cnpsw" >
                          <clientsideevents Validation="function(s, e) {e.isValid = (s.GetText() == npsw.GetText());}" />
                          <validationsettings ErrorDisplayMode="ImageWithTooltip" ErrorText="The password is incorrect">
                          </validationsettings>
                          <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
<RequiredField IsRequired="True" ErrorText="Enter Password"></RequiredField>
</ValidationSettings>

                      </dx:aspxtextbox>
                    </td>
                  </tr>
                </table>
                <dx:aspxbutton ID="confirmButton" runat="server" Text="OK" CssClass="sysButton" AutoPostBack="False"  CausesValidation="true" OnClick="confirmButton_Click">
                   
                </dx:aspxbutton>
                </div>
                </dx:popupcontrolcontentcontrol>
            </contentcollection>
        </dx:aspxpopupcontrol>
        <dx:aspxpopupcontrol ID="ASPxPopupControl3" runat="server" 
        HeaderText="Add password" Width="307px" ClientInstanceName="popup3" 
        Theme="MetropolisBlue">
           <ClientSideEvents Closing="function(s, e) {
        ASPxClientEdit.ClearEditorsInContainerById('addcontentdiv');
    }" />
     <ClientSideEvents PopUp="function(s, e) {
        ASPxClientEdit.ClearEditorsInContainerById('addcontentdiv');
    }" />
            <contentcollection>
                <dx:popupcontrolcontentcontrol ID="Popupcontrolcontentcontrol1" runat="server">
                <div id="addcontentdiv">
                <table>
                  <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Enter new password:" 
                            Wrap="False">
                        </dx:ASPxLabel>
                        </td>
                    <td>
                      <dx:aspxtextbox ID="newpass" runat="server" Password="True"  ClientInstanceName="newpass">
                          <%--<clientsideevents Validation="function(s, e) {e.isValid = (s.GetText().length>5)}" />--%>
                          <validationsettings  ErrorDisplayMode="ImageWithTooltip"  ErrorText="The password length should be more than 5 characters">
                          </validationsettings>
                          <ValidationSettings><RequiredField IsRequired="True" ErrorText="Enter Password"></RequiredField></ValidationSettings>
                            
                      </dx:aspxtextbox>
                    </td>
                  </tr>
                  <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Confirm new password:" 
                             Wrap="False">
                        </dx:ASPxLabel>
                        </td>
                    <td>
                      <dx:aspxtextbox ID="cnfrmpassword" runat="server" Password="True" ClientInstanceName="cnfrmpassword">
                          <clientsideevents Validation="function(s, e) {e.isValid = (s.GetText() == newpass.GetText());}" />
                          <validationsettings ErrorDisplayMode="ImageWithTooltip" ErrorText="The password is incorrect">
                         </validationsettings>
                           <ValidationSettings> <RequiredField IsRequired="True" ErrorText="Enter Password"></RequiredField></ValidationSettings>
                      </dx:aspxtextbox>
                    </td>
                  </tr>
                </table>
                <dx:aspxbutton ID="okbtn" runat="server" Text="OK" CssClass="sysButton" AutoPostBack="False"  CausesValidation="true" OnClick="okbtn_Click">
                   
                </dx:aspxbutton>
                </div>
                </dx:popupcontrolcontentcontrol>
            </contentcollection>
        </dx:aspxpopupcontrol>

   <div id="pnlAreaDtls" style="height: 100%; width: 100%;visibility:hidden;" runat="server" class="pnlDetails12">                
        <table align="center" width="30%" style="height: 100%">
            <tr>
                <td align="center" valign="middle" style="height: auto;">
                    <table border="1" cellspacing="0" class="csline" cellpadding="2px" id="table_txt_edit" style="border-width:1px; border-style: solid;  border-collapse: collapse;  border-color: silver; background-color: #F8F8FF"  
                        width="100%">
                        <tr style="background-color:White">                                  
                            <td align="left">
                                <div class="subheading">Delete Credential</div>
                            </td>
                        </tr>
                        <tr>                                
                            <td align="center">
                                <table cellpadding="2px">
                                    <tr>
                                        
                                        <td align="center">
                                            <div style="overflow: auto; height: 60px; font-size: 12px; font-weight: normal; font-family: Arial, Helvetica, sans-serif; text-align:left; color:black; width:350px;"  id="divmsg" runat="server"></div>
                                            <asp:Button ID="bttnOK" runat="server" OnClick="bttnOK_Click" 
                                                OnClientClick="HidePopup()" Text="OK" Width="50px" Font-Names="Arial" Font-Size="Small" />
                                            <asp:Button ID="bttnCancel" runat="server" OnClick="bttnCancel_Click" 
                                                OnClientClick="HidePopup()" Text="Cancel"  Width="70px" Font-Names="Arial" Font-Size="Small" />                                           
                                        </td>                                        
                                    </tr>
                                </table>                                        
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
		
    </div>

 
<table class="style1">
                <tr>
                    <td align="center">
                        <asp:Label id="tdmsgforlocation" runat="server" style="color:Red; font-size:12px" ></asp:Label> 
                    </td>
                </tr>
	
                </table>

				

</asp:Content>
