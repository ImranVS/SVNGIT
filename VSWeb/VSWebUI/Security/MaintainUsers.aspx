<%@ Page Title="VitalSigns Plus - Maintain Users" Language="C#" MasterPageFile="~/Site1.Master"
    AutoEventWireup="true" CodeBehind="MaintainUsers.aspx.cs" Inherits="VSWebUI.Security.MaintainUsers" %>

<%@ MasterType VirtualPath="~/Site1.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
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
        function HidePopup() {
            var popup = document.getElementById('ContentPlaceHolder1_pnlAreaDtls');
            popup.style.visibility = 'hidden';
        }

        var visibleIndex;
        function OnCustomButtonClick(s, e) {
            visibleIndex = e.visibleIndex;

            if (e.buttonID == "deleteButton")
                UsersGridView.GetRowValues(e.visibleIndex, 'FullName', OnGetRowValues);

            function OnGetRowValues(values) {
                var id = values[0];
                var name = values[1];
                var OK = (confirm('Are you sure you want to delete the user - ' + values + '?'))
                if (OK == true) {
                    UsersGridView.DeleteRow(visibleIndex);

                    //alert('The Server ' + values + ' was Successfully Deleted');
                    //ScriptManager.RegisterClientScriptBlock(base.Page, this.GetType(), "FooterRequired", "alert('Notification : Record deleted successfully');", true);
                }

                else {
                }

            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Button ID="btnDisable" runat="server" OnClientClick="return false;" Style="display: none;"
        UseSubmitBehavior="true" />
    <div id="pnlAreaDtls" style="height: 100%; width: 100%; visibility: visible" runat="server"
        class="pnlDetails12">
        F<table align="center" width="30%" style="height: 100%">
            <tr>
                <td align='center' valign='middle' style="height: auto;">
                    <table border="1" cellspacing="0" class="csline" cellpadding="2px" id="table_txt_edit"
                        style="border-width: 1px; border-style: solid; border-collapse: collapse; border-color: silver;
                        background-color: #F8F8FF" width="100%">
                        <tr style="background-color: white">
                            <td class="csline">
                                <div class="subheading" id="divheader" runat="server">
                                    Delete User</div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table>
                                    <tr>
                                        <td valign="top">
                                            <%-- <asp:Image  ID="ImageButton1" runat="server" ImageUrl="~/Images/added.gif"/>--%>
                                        </td>
                                        <td align="center">
                                            <div style="overflow: auto; height: 60px; font-size: 12px; font-weight: normal; font-family: Arial, Helvetica, sans-serif;
                                                text-align: left; color: black; width: 350px;" id="divmsg" runat="server">
                                            </div>
                                            <asp:Button ID="bttnOK" runat="server" OnClick="bttnOK_Click" OnClientClick="HidePopup()"
                                                Text="OK" Width="50px" Font-Names="Arial" Font-Size="Small" UseSubmitBehavior="false" />
                                            <asp:Button ID="btnpwdOK" runat="server" OnClick="btnpwdOK_Click" OnClientClick="HidePopup()"
                                                Text="OK" Width="50px" Font-Names="Arial" Font-Size="Small" />
                                            <asp:Button ID="bttnCancel" runat="server" OnClick="bttnCancel_Click" OnClientClick="HidePopup()"
                                                Text="Cancel" Width="70px" Font-Names="Arial" Font-Size="Small" UseSubmitBehavior="false" />
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
    <table width="100%">
        <tr>
            <td>
                <div class="header" id="servernamelbldisp" runat="server">
                    Maintain Users</div>
            </td>
        </tr>
        <tr>
            <td>
                <div id="successDiv" runat="server" class="alert alert-success" style="display: none">
                    Settings for selected servers were successully updated.
                    <button type="button" class="close" data-dismiss="alert">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                </div>
                <div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">
                    Please select at least one Attribute and one Server in order to proceed.
                    <button type="button" class="close" data-dismiss="alert">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxButton ID="NewButton" runat="server" Text="New" CssClass="sysButton" AutoPostBack="False">
                    <ClientSideEvents Click="function() { UsersGridView.AddNewRow(); }" />
                    <Image Url="~/images/icons/add.png">
                    </Image>
                </dx:ASPxButton>
            </td>
        </tr>
    </table>
    <%-- <dx:ASPxRoundPanel ID="LocationsRoundPanel" runat="server" 
        CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
        CssPostfix="Glass" 
        GroupBoxCaptionOffsetY="-24px" HeaderText="Maintain Users" 
        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
        Width="850px">
        <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
        <HeaderStyle Height="23px">
        <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
        </HeaderStyle>
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">--%>
    <dx:ASPxGridView runat="server" KeyFieldName="ID" AutoGenerateColumns="False" ID="UsersGridView"
        OnRowDeleting="UsersGridView_RowDeleting" OnRowInserting="UsersGridView_RowInserting"
        OnRowUpdating="UsersGridView_RowUpdating" Width="100%" ClientInstanceName="UsersGridView"
        OnCellEditorInitialize="UsersGridView_CellEditorInitialize" OnPageSizeChanged="UsersGridView_PageSizeChanged"
        OnRowCommand="UsersGridView_RowCommand" Theme="Office2003Blue" OnInitNewRow="UsersGridView_InitNewRow"
        OnBeforeGetCallbackResult="UsersGridView_BeforeGetCallbackResult" OnCustomErrorText="UsersGridView_CustomErrorText">
        <ClientSideEvents CustomButtonClick="OnCustomButtonClick" />
        <Columns>
            <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" FixedStyle="Left"
                VisibleIndex="0" Width="60px">
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
                </CellStyle>
                <ClearFilterButton Visible="True">
                    <Image Url="~/images/clear.png">
                    </Image>
                </ClearFilterButton>
                <HeaderStyle CssClass="GridCssHeader1"></HeaderStyle>
            </dx:GridViewCommandColumn>
            <%--<dx:GridViewDataButtonEditColumn Caption="Reset"  VisibleIndex=8  CellStyle-HorizontalAlign="Center">
                        <DataItemTemplate>
                            <asp:ImageButton ID="ResetBtn" runat="server" AlternateText="Edit" ImageUrl="~/Images/Reset.jpg" Width="20px"  Height="20px"/>
                        </DataItemTemplate>
                        <CellStyle HorizontalAlign="Center"></CellStyle>
                        </dx:GridViewDataButtonEditColumn>--%>
            <%--<dx:GridViewDataTextColumn Caption="Delete" VisibleIndex="1" 
                            CellStyle-HorizontalAlign="Center" Width="60px" 
                            CellStyle-VerticalAlign="Middle">
                            <DataItemTemplate>            
                                <asp:Label ID="lblServerName" runat="server" Text='<%#Eval("FullName") %>' Visible="False"></asp:Label>
                                <asp:ImageButton ID="bttnDelete" runat="server" ImageUrl="../images/delete.png" Width="15px" Height="15px" CommandName="Delete" CommandArgument='<%#Eval("ID") %>' AlternateText='<%#Eval("FullName") %>' ToolTip="Delete" OnClick="bttnDelete_Click"/>
                            </DataItemTemplate>
                            <EditFormSettings Visible="False"/>
                            <EditCellStyle CssClass="GridCss"></EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss1"></CellStyle>
                            <CellStyle HorizontalAlign="Center"></CellStyle>
                        </dx:GridViewDataTextColumn>--%>
            <dx:GridViewCommandColumn Caption="Delete" ButtonType="Image" VisibleIndex="1" Width="50px">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="deleteButton" Image-Url="../images/delete.png"
                        Text="Delete">
                        <Image Url="../images/delete.png">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
                <HeaderStyle CssClass="GridCssHeader1" />
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn VisibleIndex="2" Width="70px" Caption="Reset Pwd" CellStyle-HorizontalAlign="Center"
                ReadOnly="true" FieldName="IsConfigurator">
                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader" />
                <Settings AllowAutoFilter="False" />
                <EditFormSettings Visible="False" />
                <DataItemTemplate>
                    <%--<asp:Label ID="lblid" runat="server" Text='<%#Eval("ID") %>' Visible="false"></asp:Label>--%>
                    <asp:ImageButton ID="btnrp" runat="server" Text="RP" OnClick="btn_click" CommandName="RP"
                        ToolTip="Reset Password" CommandArgument='<%#Eval("ID") %>' Width="15px" ImageUrl="~/Images/icons/reset.png"
                        AlternateText='<%#Eval("FullName") %>' />
                </DataItemTemplate>
                <CellStyle HorizontalAlign="Center" CssClass="Gridcss">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True" VisibleIndex="3" Visible="false">
                <EditFormSettings Visible="False"></EditFormSettings>
                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="LoginName" VisibleIndex="4" Width="110px">
                <PropertiesTextEdit>
                    <ValidationSettings CausesValidation="True" SetFocusOnError="True">
                        <RequiredField ErrorText="You must enter a Login Name." IsRequired="True" />
						<%--<RegularExpression ValidationExpression="^\d*[a-zA-Z][a-zA-Z0-9]*$"/>--%>
                    </ValidationSettings>
                </PropertiesTextEdit>
                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader"></HeaderStyle>
                <CellStyle CssClass="GridCss">
                </CellStyle>
                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="FullName" VisibleIndex="5">
                <PropertiesTextEdit>
                    <ValidationSettings CausesValidation="True" SetFocusOnError="True">
                        <RequiredField ErrorText="You must enter a Full Name." IsRequired="True" />
						<RegularExpression ValidationExpression="[a-zA-Z ]*$"/>
                    </ValidationSettings>
                </PropertiesTextEdit>
                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
                <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Email" VisibleIndex="6">
                <PropertiesTextEdit>
                    <ValidationSettings CausesValidation="True" SetFocusOnError="True">
					 <RegularExpression ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                       ErrorText="Invalid e-mail"/>
                        <RequiredField ErrorText="You must enter an Email." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
                <Settings AllowAutoFilter="False" />
                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataComboBoxColumn FieldName="Status" VisibleIndex="7" Width="80px">
                <Settings AllowAutoFilter="False" />
                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss1">
                </CellStyle>
                <PropertiesComboBox>
                    <Items>
                        <dx:ListEditItem Text="Active" Value="Active"></dx:ListEditItem>
                        <dx:ListEditItem Text="Inactive" Value="Inactive"></dx:ListEditItem>
                    </Items>
                    <ValidationSettings CausesValidation="True" SetFocusOnError="True">
                        <RequiredField ErrorText="Select Status" IsRequired="True" />
                    </ValidationSettings>
                </PropertiesComboBox>
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn FieldName="SuperAdmin" VisibleIndex="8" Width="90px">
                <Settings AllowAutoFilter="False" />
                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss1">
                </CellStyle>
                <PropertiesComboBox>
                    <Items>
                        <dx:ListEditItem Text="Yes" Value="Y"></dx:ListEditItem>
                        <dx:ListEditItem Text="No" Value="N"></dx:ListEditItem>
                    </Items>
                    <ValidationSettings CausesValidation="True" SetFocusOnError="True">
                        <RequiredField ErrorText="You must select a value for the Super Admin field." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesComboBox>
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataCheckColumn Caption="Configurator Access" VisibleIndex="9" FieldName="IsConfigurator"
                Width="80px">
                <PropertiesCheckEdit>
                    <Style HorizontalAlign="Left">
                        
                    </Style>
                </PropertiesCheckEdit>
                <Settings AllowAutoFilter="False" />
                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                <CellStyle CssClass="GridCss1" HorizontalAlign="Left">
                </CellStyle>
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataCheckColumn Caption="Dashboard Access" VisibleIndex="10" FieldName="IsDashboard"
                Width="80px" Visible="false">
                <Settings AllowAutoFilter="False" />
                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                <CellStyle CssClass="GridCss1" HorizontalAlign="Left">
                </CellStyle>
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataCheckColumn Caption="Console Command Access" FieldName="IsConsoleComm"
                Visible="False" VisibleIndex="11" Width="80px">
                <Settings AllowAutoFilter="False" />
                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader" Wrap="True" />
                <CellStyle CssClass="GridCss1">
                </CellStyle>
            </dx:GridViewDataCheckColumn>
        </Columns>
        <Settings ShowFilterRow="True" />
        <SettingsText ConfirmDelete="'Are sure you want to Delete this record?'" />
        <Styles>
            <Header SortingImageSpacing="5px" ImageSpacing="5px">
            </Header>
            <LoadingPanel ImageSpacing="5px">
            </LoadingPanel>
            <AlternatingRow CssClass="GridAltRow" Enabled="True">
            </AlternatingRow>
            <EditForm BackColor="White">
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
        <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" />
        <SettingsPager PageSize="10" SEOFriendly="Enabled">
            <PageSizeItemSettings Visible="true" />
        </SettingsPager>
    </dx:ASPxGridView>
    <%--  </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxRoundPanel>--%>
    <br />
    <dx:ASPxPopupControl ID="NavigatorPopupControl" runat="server" ClientInstanceName="popup1"
        HeaderText="Information" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        Theme="Glass">
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
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxButton ID="OKButton" runat="server" OnClick="subbttnOK_Click" Text="OK" Theme="Office2010Blue">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
