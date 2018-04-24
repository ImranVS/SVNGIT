<%@ Page Title="VitalSigns Plus -Domino  Event Log Definitions" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="DominoELSDefinitions_Grid.aspx.cs" Inherits="VSWebUI.Configurator.DominoELSDefinitions_Grid" %>
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
    	function OnKeyDown(s, e) {
    		var keyCode = window.event.keyCode;
    		if (keyCode == 13)
    			goButton.DoClick();
    	}
    	var visibleIndex;
    	function OnCustomButtonClick(s, e) {
    		visibleIndex = e.visibleIndex;

    		if (e.buttonID == "deleteButton")
    			DELSDefGridView.GetRowValues(e.visibleIndex, 'Name', OnGetRowValues);

    		function OnGetRowValues(values) {
    			var id = values[0];
    			var name = values[1];
    			var OK = (confirm('Are you sure you want to delete the Event definition - ' + values + '?'))
    			if (OK == true) {
    				DELSDefGridView.DeleteRow(visibleIndex);
    			}
    		}
    	}
   </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
        <tr>
            <td>
                <div class="header" id="servernamelbldisp" runat="server">Domino Event Definitions</div>
            </td>
        </tr>
        <tr><td>
        <div id="successDiv" runat="server" class="alert alert-success" style="display: none">Success.
                </div>
        <div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">Error.
                </div>
        <div class="info">The list of available Event Definitions is displayed below. In order to add new Event Definitions or configure existing alerts, please use the buttons in the 'Actions' column.
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
                   
                </tr>
            </table>
    </td></tr>
        <tr>
            <td>
                <dx:ASPxGridView ID="DELSDefGridView" runat="server" AutoGenerateColumns="False" 
                    KeyFieldName="ID" ClientInstanceName="DELSDefGridView"
                    OnHtmlDataCellPrepared="DELSDefGridView_HtmlDataCellPrepared" 
                    OnHtmlRowCreated="DELSDefGridView_HtmlRowCreated" 
                    OnRowDeleting="DELSDefGridView_RowDeleting" Width="100%" OnPageSizeChanged="DELSDefGridView_PageSizeChanged"
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
                        <dx:GridViewDataTextColumn Caption="Event Definition Name" 
                            VisibleIndex="2" FieldName="Name">
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
            </td>
        </tr>
    </table>
</asp:Content>
