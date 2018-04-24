<%@ Page Title="VitalSigns Plus - Alert Definitions" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="AlertDefinitions_Grid.aspx.cs" Inherits="VSWebUI.AlertDefinitions_Grid" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>




<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.alert-success').delay(10000).fadeOut("slow", function () {
            });
        });
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
        //5/21/2015 NS added for VSPLUS-1771
		var visibleIndex;
		function OnCustomButtonClick(s, e) {
			visibleIndex = e.visibleIndex;

			if (e.buttonID == "deleteButton")
			    AlertDefGridView.GetRowValues(e.visibleIndex, 'AlertName', OnGetRowValues);

			function OnGetRowValues(values) {
				var id = values[0];
				var name = values[1];
				var OK = (confirm('Are you sure you want to delete the alert definition - ' + values + '?'))
				if (OK == true) {
				    AlertDefGridView.DeleteRow(visibleIndex);
				}
			}
		}
   </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
        <tr>
            <td>
                <div class="header" id="servernamelbldisp" runat="server">Alert Notification Definitions</div>
            </td>
        </tr>
        <tr><td>
        <div id="successDiv" runat="server" class="alert alert-success" style="display: none">Success.
                </div>
        <div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">Error.
                </div>
        <div class="info">The list of defined notifications is displayed below. When an alert condition matches a notification definition, a message will be sent out to recipients specified. In order to add new definitions or configure existing definitions, please use the buttons in the 'Actions' column.
        </div>
        <table>
                <tr>
                    <td>
                        <dx:ASPxButton ID="NewButton" runat="server" Text="New" CssClass="sysButton" 
                            onclick="NewButton_Click">
                            <Image Url="~/images/icons/add.png">
                                            </Image>
                        </dx:ASPxButton>
                    </td>
                    <td>
                        <dx:ASPxButton ID="SendTestAlertButton" runat="server" Text="Send Test Alert" CssClass="sysButton"
            Wrap="False" onclick="SendTestAlertButton_Click">
        </dx:ASPxButton>
    <dx:ASPxLabel ID="TestAlertSubmittedLabel" runat="server" Text="ASPxLabel" ClientInstanceName="TestAlertSubmittedLabel"
                Visible="False" Wrap="False">
            </dx:ASPxLabel>
                    </td>
                </tr>
            </table>
    </td></tr>
        <tr>
            <td>
                <dx:ASPxGridView ID="AlertDefGridView" runat="server" AutoGenerateColumns="False" 
                    KeyFieldName="AlertKey" ClientInstanceName="AlertDefGridView"
                    OnHtmlDataCellPrepared="AlertDefGridView_HtmlDataCellPrepared" 
                    OnHtmlRowCreated="AlertDefGridView_HtmlRowCreated" 
                    OnRowDeleting="AlertDefGridView_RowDeleting" Width="100%" OnPageSizeChanged="AlertDefGridView_PageSizeChanged"
                    EnableTheming="True" Theme="Office2003Blue">
                    <ClientSideEvents CustomButtonClick="OnCustomButtonClick" />
                    <Columns>
                        <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" VisibleIndex="0" 
                            Width="70px">
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
                                                                            <CellStyle  CssClass="GridCss1">
                                                                                <Paddings Padding="3px" />
                                                                            </CellStyle>
                                                                            <ClearFilterButton Visible="True">
                                                                                <Image Url="~/images/clear.png">
                                                                                </Image>
                                                                            </ClearFilterButton>
                                                                           <HeaderStyle CssClass="GridCssHeader1" >
                                                                            <Paddings Padding="5px" />
                                                                            </HeaderStyle>
                                                                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn Caption="Alert Name" 
                            VisibleIndex="2" FieldName="AlertName">
                              <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                              <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                                                            <HeaderStyle CssClass="GridCssHeader" 
                                  Wrap="True" >
                              <Paddings Padding="5px" />
                              </HeaderStyle>
                              <CellStyle CssClass="GridCss"></CellStyle>

                        </dx:GridViewDataTextColumn>
                        <dx1:GridViewCommandColumn ButtonType="Image" Caption="Delete" VisibleIndex="1" 
                            Width="70px">
                            <CustomButtons>
                                <dx1:GridViewCommandColumnCustomButton ID="deleteButton" Text="Delete">
                                    <Image Url="~/images/delete.png">
                                    </Image>
                                </dx1:GridViewCommandColumnCustomButton>
                            </CustomButtons>
                            <HeaderStyle CssClass="GridCssHeader1" />
                            <CellStyle CssClass="GridCss1">
                            </CellStyle>
                        </dx1:GridViewCommandColumn>
                    </Columns>
                    <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" />
                    <Settings ShowFilterRow="True" />
                    <SettingsText ConfirmDelete="Are you sure you  want to delete this record?" />
                    <Styles>
                        <AlternatingRow CssClass="GridAltRow" Enabled="True">
                        </AlternatingRow>
                        <LoadingPanel ImageSpacing="5px">
                        </LoadingPanel>
                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                        </Header>
                    </Styles>
                    <StylesEditors ButtonEditCellSpacing="0">
                        <ProgressBar Height="21px">
                        </ProgressBar>
                    </StylesEditors>
                      <SettingsPager PageSize="50" SEOFriendly="Enabled" >
            <PageSizeItemSettings Visible="true" />
        </SettingsPager>
                </dx:ASPxGridView>
                </td>
            </tr>
            <tr>
            <td>
                <dx:ASPxPopupControl ID="CreateTestAlertPopupControl" runat="server" ClientInstanceName="CreateTestAlertPopupControl"
                    HeaderText="Create Test Alert" PopupHorizontalAlign="WindowCenter" 
                    PopupVerticalAlign="WindowCenter" Theme="MetropolisBlue" Height="200px" Width="300px"
                    AllowDragging="True" Modal="True" EnableHierarchyRecreation="False">
                    <ClientSideEvents CloseUp="function(s, e) {
     CreateTestAlertPopupControl.Hide();
}" />

                    <ContentCollection>
<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
<%--  15/3/2016 Sowmya Added for VSPLUS 2728--%>
 <table>
    <tr>
      <td>
<div class="info">The test alert button queues a specific alert type to the message queue. Whether a notification actually goes out or not depends on the alert definitions and if there is a match for the specific alert type.
        </div>
      </td>
   </tr>
</table>
<asp:UpdatePanel ID="updatepan1" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<table>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" 
                    Text="Location:">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxComboBox ID="LocationComboBox" runat="server" AutoPostBack="True" 
                    ClientInstanceName="LocationComboBox" 
                    OnSelectedIndexChanged="LocationComboBox_SelectedIndexChanged">
                </dx:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" 
                    Text="Device Type:" Wrap="False">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxComboBox ID="DeviceTypeComboBox" runat="server" AutoPostBack="True" 
                    OnSelectedIndexChanged="DeviceTypeComboBox_SelectedIndexChanged">
                </dx:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="lblsmallFont" 
                    Text="Event Type:">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxComboBox ID="EventTypeComboBox" runat="server" Enabled="False">
                </dx:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel5" runat="server" CssClass="lblsmallFont" 
                    Text="Device Name:" Wrap="False">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxComboBox ID="ServerNameComboBox" runat="server" Enabled="False">
                </dx:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td>
               </td>
            <td>
               </td>
        </tr>
        <tr>
            <td colspan="2">
                <table>
                    <tr>
                        <td>
                <dx:ASPxButton ID="TestAlertButton" runat="server"
                    OnClick="TestAlertButton_Click" Text="Create" CssClass="sysButton"
                    ClientInstanceName="goButton">
                    <ClientSideEvents Click="function(s, e) {
	CreateTestAlertPopupControl.Hide();
}" />
                </dx:ASPxButton>
            </td>
            <td>
                <dx:ASPxButton ID="CancelButton" runat="server" CausesValidation="False" 
                    EnableClientSideAPI="true" Text="Cancel"  CssClass="sysButton"
                    onclick="CancelButton_Click">
                    <ClientSideEvents Click="function(s, e) {
	CreateTestAlertPopupControl.Hide();
}" />
                </dx:ASPxButton>
            </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</ContentTemplate>
</asp:UpdatePanel>
    
                        </dx:PopupControlContentControl>
</ContentCollection>
                </dx:ASPxPopupControl>
            </td>
        </tr>
    </table>
</asp:Content>
