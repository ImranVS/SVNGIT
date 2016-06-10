<%@ Page Title="VitalSigns Plus - Traveler Data Store" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="TravelerDataStore.aspx.cs" Inherits="VSWebUI.Configurator.TravelerDataStore" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.alert-success').delay(10000).fadeOut("slow", function () {
            });
        });
        //5/21/2015 NS added for VSPLUS-1771
        var visibleIndex;
        function OnCustomButtonClick(s, e) {
            visibleIndex = e.visibleIndex;

            if (e.buttonID == "deleteButton")
                TravelerHAGrid.GetRowValues(e.visibleIndex, 'TravelerServicePoolName', OnGetRowValues);

            function OnGetRowValues(values) {
                var id = values[0];
                var name = values[1];
                var OK = (confirm('Are you sure you want to delete the data store - ' + values + '?'))
                if (OK == true) {
                    TravelerHAGrid.DeleteRow(visibleIndex);
                }
            }
        }
        </script>
    <style type="text/css">
        .tblpadded
        {
            background-color: transparent;
            padding: 10px 10px 30px 10px;
        }
    </style>
    <script type="text/javascript">
        
        function UpdateText() {
            var selected = checkBoxList.GetSelectedItems();
            
            var textForBox = [];
            for (var i = 0; i < selected.length; i++) {
                textForBox.push(selected[i].text);
            }

            checkComboBox.SetText(textForBox.join(","));
            
        }

        function SyncBoxValues(dropDown, args) {
            checkBoxList.UnselectAll();
            var selectedValues = [];
            var items = dropDown.GetText().split(',');
            var currItem;
            for (var i = 0; i < items.length; i++) {
                currItem = checkBoxList.FindItemByText(items[i]);
                if (currItem != null) {
                    selectedValues.push(currItem.value);
                }
            }
            checkBoxList.SelectValues(selectedValues);
            UpdateText();
            console.log("text" + checkComboBox.GetText());
            if (checkComboBox.GetText() == "") {
                console.log("here");
                return true;
            }
            return false;
        }

        function Validate(sender, args) {
            if (checkComboBox.GetText() == null || checkComboBox.GetText() == "")
                console.log("here");
            console.log("here1");
            args.IsValid = true;
        }
    
    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table>
        <tr>
            <td>
                <div class="header" id="servernamelbldisp" runat="server">Notes Traveler HA - Backend Data Store</div>
                <div class="info">Traveler HA servers share a common backend data store, either SQL Server or DB2.
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxButton ID="NewButton" runat="server" Text="New" CssClass="sysButton" AutoPostBack="False">
                    <ClientSideEvents Click="function() { TravelerHAGrid.AddNewRow(); }" />
                                <Image Url="~/images/icons/add.png">
                                </Image>
                </dx:ASPxButton>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxGridView ID="TravelerHAGrid" runat="server" AutoGenerateColumns="False" 
                    EnableTheming="True" Theme="Office2003Blue" Width="100%" ClientInstanceName="TravelerHAGrid" 
                    onrowupdating="TravelerHAGrid_RowUpdating" 
                    onrowinserting="TravelerHAGrid_RowInserting" KeyFieldName="ID" 
                    onhtmlrowcreated="TravelerHAGrid_HtmlRowCreated" 
                    onrowdeleting="TravelerHAGrid_RowDeleting" OnPageSizeChanged="TravelerHAGrid_PageSizeChanged"
                    OnCellEditorInitialize="TravelerHAGrid_CellEditorInitialize">
                    <ClientSideEvents CustomButtonClick="OnCustomButtonClick" />
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" Visible="False" 
                            VisibleIndex="3">
                            <EditFormSettings Visible="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" VisibleIndex="1" 
                            Width="70px">
                            <EditButton Visible="True">
                                <Image Url="~/images/edit.png">
                                </Image>
                            </EditButton>
                            <NewButton Visible="True">
                                <Image Url="~/images/icons/add.png">
                                </Image>
                            </NewButton>
                            <DeleteButton Visible="False">
                                <Image Url="~/images/delete.png">
                                </Image>
                            </DeleteButton>
                            <CancelButton>
                                <Image Url="~/images/cancel.gif">
                                </Image>
                            </CancelButton>
                            <UpdateButton Visible="True">
                                <Image Url="~/images/update.gif">
                                </Image>
                            </UpdateButton>
                            <ClearFilterButton Visible="True">
                            </ClearFilterButton>
                            <HeaderStyle CssClass="GridCssHeader1" />
                            <CellStyle CssClass="GridCss1">
                            </CellStyle>
                        </dx:GridViewCommandColumn>
							<dx:GridViewDataTextColumn Width="70px" Caption="Register Pwd" CellStyle-HorizontalAlign="Center"
							ReadOnly="true" FieldName="Password" VisibleIndex="4">
							<EditCellStyle CssClass="GridCss">
							</EditCellStyle>
							<EditFormCaptionStyle CssClass="GridCss">
							</EditFormCaptionStyle>
							<HeaderStyle CssClass="GridCssHeader" />
							<Settings AllowAutoFilter="False" />
							<EditFormSettings Visible="True" />
							<DataItemTemplate>
								<asp:ImageButton ID="btnrp" runat="server" Text="RP" CommandName="RP" CommandArgument='<%#Eval("ID") %>'  AlternateText='<%#Eval("TravelerServicePoolName") %>' ToolTip="Register Pwd"
									Width="15px" ImageUrl="~/Images/icons/reset.png" Height="16px" OnClick="btnrp_Click" />
							</DataItemTemplate>
							<CellStyle HorizontalAlign="Center" CssClass="Gridcss">
							</CellStyle>
						</dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Traveler Service Pool Name" 
                            VisibleIndex="5" FieldName="TravelerServicePoolName">
                            <EditFormSettings VisibleIndex="0" Visible="True" />
                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Server Name" 
                            VisibleIndex="6" FieldName="ServerName" Visible="False">
                            <EditFormSettings Visible="True" />
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Data Store" 
                            VisibleIndex="7" FieldName="DataStore" Visible="False">
                            <EditFormSettings Visible="True" />
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Port" 
                            VisibleIndex="9" FieldName="Port" Visible="False">
                            <EditFormSettings Visible="True" />
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="User Name" VisibleIndex="10" 
                            FieldName="UserName" Visible="False">
                            <EditFormSettings Visible="True" />
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Password" VisibleIndex="11" 
                            FieldName="Password" Visible="False">
                            <EditFormSettings Visible="True" />
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Authentication Type" 
                            VisibleIndex="12" FieldName="IntegratedSecurity" Visible="False">
                            <EditFormSettings Visible="True" />
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Test when scanning Traveler Server" 
                            VisibleIndex="13" FieldName="TestScanServer" Visible="False">
                            <EditFormSettings Visible="True" />
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Used by Servers" 
                            VisibleIndex="14" FieldName="UsedByServers" Visible="False">
                            <EditFormSettings Visible="True" />
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Database" FieldName="DatabaseName" 
                            Visible="False" VisibleIndex="8">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewCommandColumn ButtonType="Image" Caption="Delete" VisibleIndex="2" 
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
                    <SettingsText ConfirmDelete="Are you sure you want to delete this record?" />
                    <Styles>
                        <AlternatingRow CssClass="GridAltRow">
                        </AlternatingRow>
                        <EditForm CssClass="GridCssEditForm">
                        </EditForm>
                    </Styles>
                    <Templates>
                        <EditForm>
                            <table class="tblpadded">
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="TravelerPoolNameLabel" runat="server" CssClass="lblsmallFont" Text="Traveler Service Pool Name:">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="TravelerPoolNameTextBox" runat="server" Width="170px"
                                        Value='<%# Bind("TravelerServicePoolName") %>' ValidationSettings-ValidationGroup='<%# Container.ValidationGroup %>'
                                        ClientInstanceName="TravelerPoolNameTextBox">
                                        <ValidationSettings CausesValidation="true">
                                            <RequiredField IsRequired="true" />
                                        </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <dx:ASPxLabel ID="ServerNameLabel" runat="server" CssClass="lblsmallFont" Text="Server Name:">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="ServerNameTextBox" runat="server" CssClass="lblsmallFont" Width="170px"
                                        Value='<%# Bind("ServerName") %>' ValidationSettings-ValidationGroup='<%# Container.ValidationGroup %>'
                                        ClientInstanceName="ServerNameTextBox">
                                        <ValidationSettings CausesValidation="true">
                                            <RequiredField IsRequired="true" />
                                        </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="DataStoreLabel" runat="server" CssClass="lblsmallFont" Text="Data Store:">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxComboBox ID="DataStoreComboBox" runat="server" ValueType="System.String"
                                        Value='<%# Bind("DataStore") %>' ValidationSettings-ValidationGroup='<%# Container.ValidationGroup %>'
                                        ClientInstanceName="DataStoreComboBox">
                                        <ValidationSettings CausesValidation="true">
                                            <RequiredField IsRequired="true" />
                                        </ValidationSettings>
                                            <Items>
                                                <dx:ListEditItem Text="DB2" Value="DB2" />
                                                <dx:ListEditItem Text="SQL Server" Value="SQL Server" />
                                            </Items>
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <dx:ASPxLabel ID="DatabaseLabel" runat="server" Text="Database:" CssClass="lblsmallFont">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="DatabseTextBox" runat="server" Width="170px" ValueType="System.String"
                                        Value='<%# Bind("DatabaseName") %>' ValidationSettings-ValidationGroup='<%# Container.ValidationGroup %>'
                                        ClientInstanceName="DatabaseTextBox">
                                        <ValidationSettings CausesValidation="true">
                                            <RequiredField IsRequired="true" />
                                        </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="PortLabel" runat="server" CssClass="lblsmallFont" Text="Port:">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="PortTextBox" runat="server" Width="170px"
                                        Value='<%# Bind("Port") %>' ValidationSettings-ValidationGroup='<%# Container.ValidationGroup %>'
                                        ClientInstanceName="PortTextBox">
                                        <ValidationSettings CausesValidation="true">
                                            <RequiredField IsRequired="true" />
                                        </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td colspan="3">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="UserNameLabel" runat="server" CssClass="lblsmallFont" Text="User Name:">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="UserNameTextBox" runat="server" Width="170px"
                                        Value='<%# Bind("UserName") %>' ValidationSettings-ValidationGroup='<%# Container.ValidationGroup %>'
                                        ClientInstanceName="UserNameTextBox">
                                        <ValidationSettings CausesValidation="true">
                                            <RequiredField IsRequired="true" />
                                        </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <dx:ASPxLabel ID="PasswordLabel" runat="server" CssClass="lblsmallFont" Text="Password:">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTextBox ID="PasswordTextBox" runat="server" Password="true" Width="170px"
                                        Value='<%# Bind("Password") %>' ValidationSettings-ValidationGroup='<%# Container.ValidationGroup %>'
                                        ClientInstanceName="PasswordTextBox">
                                        <ValidationSettings CausesValidation="true">
                                            <RequiredField IsRequired="true" />
                                        </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="IntegratedSecurityLabel" runat="server" CssClass="lblsmallFont" Text="Authentication Type:">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxComboBox ID="IntegratedSecurityComboBox" runat="server" ValueType="System.String"
                                        Value='<%# Bind("IntegratedSecurity") %>' ValidationSettings-ValidationGroup='<%# Container.ValidationGroup %>'
                                        ClientInstanceName="IntegratedSecurityComboBox">
                                        <ValidationSettings CausesValidation="true">
                                            <RequiredField IsRequired="true" />
                                        </ValidationSettings>
                                            <Items>
                                                <dx:ListEditItem Text="SQL Server" Value="0" />
                                                <dx:ListEditItem Text="Windows" Value="1" />
                                            </Items>
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <dx:ASPxLabel ID="TestScanLabel" runat="server" CssClass="lblsmallFont" Text="Test when scanning Traveler Server:">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxComboBox ID="TestScanComboBox" runat="server" ValueType="System.String"
                                        Value='<%# Bind("TestScanServer") %>' ValidationSettings-ValidationGroup='<%# Container.ValidationGroup %>'
                                        ClientInstanceName="TestScanComboBox">
                                        <%--<ValidationSettings CausesValidation="true">
                                            <RequiredField IsRequired="true" />
                                        </ValidationSettings>--%>
                                        </dx:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="UsedByServersLabel" runat="server" CssClass="lblsmallFont" Text="Used by Servers:">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        


                                        <dx:ASPxDropDownEdit ClientInstanceName="checkComboBox" ID="UsedByServersComboBox" runat="server"  
                                        ValidationSettings-ValidationGroup='<%# Container.ValidationGroup %>' Text='<%# Bind("UsedByServers") %>'
                                         OnDataBinding="UsedByServersComboBox_OnDataBinding"  Width="200px">
                                            
                                            <DropDownWindowTemplate>
                                            
                                                <dx:ASPxListBox ID="UsedByServersList" SelectionMode="CheckColumn" ClientInstanceName="checkBoxList" 
                                                runat="server" Width="100%" OnLoad="UsedByServersListoBox_OnDataBinding">
                                                
                                                    <Border BorderStyle="None" />
                                                    <ClientSideEvents SelectedIndexChanged="UpdateText" />
                                                </dx:ASPxListBox>

                                            </DropDownWindowTemplate>
                                        
                                            <ClientSideEvents TextChanged="SyncBoxValues" DropDown="SyncBoxValues" Validation="function(s,e){ e.IsValid=false; }"
                                            />
                                            
                                            
                                        </dx:ASPxDropDownEdit>

                                       <asp:CustomValidator ID="CustomValadtor1" runat="server" ErrorMessage="Please ensure all fields are filled out"
                                        ClientValidationFunction="Validate" />
                                 
                                    </td>
                                    <td>
                                        &nbsp;
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
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                     <td>
                                        <dx:ASPxGridViewTemplateReplacement ID="bttnUpdate" runat="server" ReplacementType="EditFormUpdateButton" />
                                        <dx:ASPxGridViewTemplateReplacement ID="bttnCancel" runat="server" ReplacementType="EditFormCancelButton" />
                                    </td>
                                </tr>
                            </table>
                        </EditForm>
                    </Templates>
                </dx:ASPxGridView>
                <div id="emptyDiv" runat="server" style="display: block">&nbsp;</div>
                <div id="errorDiv" class="alert alert-danger" runat="server" style="display: none">Error.
                </div>
            </td>
        </tr>
</table>
</asp:Content>