<%@ Page Title="VitalSigns Plus - Manage Profiles" Language="C#" MasterPageFile="~/Site1.Master"
    AutoEventWireup="true" CodeBehind="ManageProfiles.aspx.cs" Inherits="VSWebUI.ManageProfiles" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx1" %>
<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxScheduler" TagPrefix="dxwschs" %>
<%@ Register Assembly="DevExpress.XtraScheduler.v14.2.Core, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraScheduler" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet'
        type='text/css' />
    <link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
    <script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript">


        var visibleIndex;
        function OnCustomButtonClick(s, e) {
            visibleIndex = e.visibleIndex;

            if (e.buttonID == "deleteButton")
                ProfileGridView.GetRowValues(e.visibleIndex, 'ProfileName', OnGetRowValues);

            function OnGetRowValues(values) {
                var id = values[0];
                var name = values[1];
                var OK = (confirm('Are you sure you want to delete the profile - ' + values + '?'))
                if (OK == true) {
                    ProfileGridView.DeleteRow(visibleIndex);

                    //alert('The Server ' + values + ' was Successfully Deleted');
                    //ScriptManager.RegisterClientScriptBlock(base.Page, this.GetType(), "FooterRequired", "alert('Notification : Record deleted successfully');", true);
                }

                else {
                }

            }
        }
        $(document).ready(function () {
            $('.alert-success').delay(10000).fadeOut("slow", function () {
            });
            $('.alert-danger').delay(10000).fadeOut("slow", function () {
            });
        });
        function HidePopup() {
            var popup = document.getElementById('ContentPlaceHolder1_pnlAreaDtls');
            popup.style.visibility = 'hidden';
        }
    </script>
	<style>
  .dxgvFilterRow_Office2003Blue a.dxgvCommandColumnItem_Office2003Blue {
    display: inline-block;
    margin-top: 1px;
    visibility: hidden;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Button ID="btnDisable" runat="server" OnClientClick="return false;" Style="display: none;"
        UseSubmitBehavior="true" />
    <div id="pnlAreaDtls" style="height: 100%; width: 100%; visibility: hidden" runat="server"
        class="pnlDetails12">
        <table align="center" width="30%" style="height: 100%">
            <tr>
                <td align="center" valign="middle" style="height: auto;">
                    <table border="1" cellspacing="0" class="csline" cellpadding="2px" id="table_txt_edit"
                        style="border-width: 1px; border-style: solid; border-collapse: collapse; border-color: silver;
                        background-color: #F8F8FF" width="100%">
                        <tr style="background-color: White">
                            <td align="left">
                                <div class="subheading">
                                    Delete Profiles</div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table>
                                    <tr>
                                        <td valign="top">
                                        </td>
                                        <td align="center">
                                            <div style="overflow: auto; height: 60px; font-size: 12px; font-weight: normal; font-family: Arial, Helvetica, sans-serif;
                                                text-align: left; color: black; width: 350px;" id="divmsg" runat="server">
                                            </div>
                                            <asp:Button ID="btnok1" runat="server" OnClick="btn_OkClick" OnClientClick="hidepopup()"
                                                Text="OK" Width="50px" Font-Names="Arial" Font-Size="Small" />
                                            <asp:Button ID="btncancel1" runat="server" OnClick="btn_CancelClick" OnClientClick="hidepopup()"
                                                Text="Cancel" Width="70px" Font-Names="Arial" Font-Size="Small" />
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
    <table>
        <tr>
            <td>
                <div class="header" id="servernamelbldisp" runat="server">
                    Manage Profiles</div>
            </td>
        </tr>
        <tr>
            <td style="color: Black" id="tdmsg" runat="server" align="center">
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxButton ID="NewButton" runat="server" Text="New" CssClass="sysButton" AutoPostBack="False">
                    <ClientSideEvents Click="function() { ProfileGridView.AddNewRow(); }" />
                                <Image Url="~/images/icons/add.png">
                                </Image>
                </dx:ASPxButton>
            </td>
        </tr>
        <tr>
            <dx:ASPxGridView runat="server" KeyFieldName="ID" AutoGenerateColumns="False" ClientInstanceName="ProfileGridView"
                ID="ProfileGridView" OnRowDeleting="ProfileGridView_RowDeleting" OnRowInserting="ProfileGridView_RowInserting"
                OnPageSizeChanged="ProfileGridView_PageSizeChanged" OnRowUpdating="ProfileGridView_RowUpdating"
                Theme="Office2003Blue" OnCustomButtonInitialize="ProfileGridView_CustomButtonInitialize">
                <ClientSideEvents CustomButtonClick="OnCustomButtonClick" />
                <Columns>
                    <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" FixedStyle="Left"
                        VisibleIndex="0" Width="10px">
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
                        <HeaderStyle CssClass="GridCssHeader" />
                    </dx:GridViewCommandColumn>
                    <%-- <dx:GridViewDataTextColumn Caption="Delete" VisibleIndex="1" CellStyle-HorizontalAlign="Center" Width="30px" CellStyle-VerticalAlign="Middle">
                                        <DataItemTemplate>            
                                            <asp:Label ID="lblLocation" runat="server" Text='<%#Eval("ProfileName") %>' Visible="False"></asp:Label>
                                            <asp:ImageButton ID="bttnDelete" runat="server" ImageUrl="../images/delete.png" Width="15px" Height="15px" CommandName="Delete" CommandArgument='<%#Eval("ID") %>' AlternateText='<%#Eval("ProfileName") %>' ToolTip="Delete" OnClick="bttnDelete_Click" />
                                        </DataItemTemplate>
                                        <EditFormSettings Visible="False"/>
                                        <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss1"></CellStyle>
                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                    </dx:GridViewDataTextColumn>--%>
                    <%--<dx:GridViewDataTextColumn Caption="Delete" VisibleIndex="1" CellStyle-HorizontalAlign="Center" Width="30px">
            <DataItemTemplate>
             <asp:Label ID="lblProfileName" runat="server" Text='<%#Eval("ProfileName") %>' Visible="false"></asp:Label>
              <asp:ImageButton ID="btndele" runat="server" ImageUrl="../images/delete.png" Width="15px" Height="15px" CommandName="Delete" CommandArgument='<%#Eval("ID") %>' AlternateText='<%#Eval("ProfileName") %>' ToolTip="Delete" OnClick="btn_Clickdelete" />
            </DataItemTemplate>
            <EditFormSettings Visible="False"/>
            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss1"></CellStyle>
            <CellStyle HorizontalAlign="Center"></CellStyle>
         </dx:GridViewDataTextColumn>--%>
                    <dx:GridViewCommandColumn Caption="Delete" ButtonType="Image" VisibleIndex="1">
                        <CustomButtons>
                            <dx:GridViewCommandColumnCustomButton ID="deleteButton" Image-Url="../images/delete.png"
                                Text="Delete">
                                <Image Url="../images/delete.png">
                                </Image>
                            </dx:GridViewCommandColumnCustomButton>
                        </CustomButtons>
                        <HeaderStyle CssClass="GridCssHeader" />
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataTextColumn VisibleIndex="2" Width="70px" Caption="Copy Profile "
                        CellStyle-HorizontalAlign="Center" ReadOnly="true" FieldName="ProfileName">
                        <EditCellStyle CssClass="GridCss">
                        </EditCellStyle>
                        <EditFormCaptionStyle CssClass="GridCss">
                        </EditFormCaptionStyle>
                        <HeaderStyle CssClass="GridCssHeader" />
                        <Settings AllowAutoFilter="False" />
                        <EditFormSettings Visible="False" />
                        <DataItemTemplate>
                            <%--<asp:Label ID="lblid" runat="server" Text='<%#Eval("ID") %>' Visible="false"></asp:Label>--%>
                            <asp:ImageButton ID="btnrp" runat="server" Text="RP" OnClick="btn_clickcopyprofile"
                                CommandName="RP" ToolTip="Copy Profile" CommandArgument='<%#Eval("ID") %>' Width="15px"
                                ImageUrl="~/Images/icons/reset.png" AlternateText='<%#Eval("ProfileName") %>' />
                        </DataItemTemplate>
                        <CellStyle HorizontalAlign="Center" CssClass="Gridcss">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Edit Server Attributes" VisibleIndex="3" CellStyle-HorizontalAlign="Center"
                        Width="30px">
                        <DataItemTemplate>
                            <asp:Label ID="lblserverbutton" runat="server" Text='<%#Eval("ProfileName") %>' Visible="false"></asp:Label>
                            <asp:ImageButton ID="btnserverattributes" runat="server" ImageUrl="~/Images/icons/reset.png"
                                Width="15px" Height="15px" CommandName="Delete" CommandArgument='<%#Eval("ID") %>'
                                AlternateText='<%#Eval("ProfileName") %>' ToolTip="Edit Server attributes" OnClick="btn_Clickeditserver" />
                        </DataItemTemplate>
                        <EditFormSettings Visible="False" />
                        <EditCellStyle CssClass="GridCss">
                        </EditCellStyle>
                        <EditFormCaptionStyle CssClass="GridCss">
                        </EditFormCaptionStyle>
                        <HeaderStyle CssClass="GridCssHeader" />
                        <CellStyle CssClass="GridCss1">
                        </CellStyle>
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True" Visible="False" VisibleIndex="4">
                        <EditFormSettings Visible="False"></EditFormSettings>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ProfileName" VisibleIndex="5">
                        <PropertiesTextEdit>
                            <ValidationSettings CausesValidation="True" SetFocusOnError="True">
                                <RequiredField ErrorText="Enter Profile" IsRequired="True" />
                            </ValidationSettings>
                        </PropertiesTextEdit>
                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                        <EditCellStyle CssClass="GridCss">
                        </EditCellStyle>
                        <EditFormCaptionStyle CssClass="GridCss">
                        </EditFormCaptionStyle>
                        <HeaderStyle CssClass="GridCssHeader" />
                        <CellStyle CssClass="GridCss">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                </Columns>
                <Settings ShowFilterRow="True" />
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
                <SettingsBehavior ConfirmDelete="True" />
                <SettingsPager PageSize="10" SEOFriendly="Enabled">
                    <PageSizeItemSettings Visible="true" />
                </SettingsPager>
            </dx:ASPxGridView>
        </tr>
    </table>
    <dx:ASPxPopupControl ID="CopyProfilePopupControl" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
        CssPostfix="Glass" HeaderText="Copy Profile" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        Theme="MetropolisBlue">
        <LoadingPanelImage Url="~/App_Themes/Glass/Web/Loading.gif">
        </LoadingPanelImage>
        <HeaderStyle>
            <Paddings PaddingLeft="10px" PaddingRight="6px" PaddingTop="1px" />
        </HeaderStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                <table class="style1">
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="CopyProfileLabel" runat="server" Text="Please enter your Profile Name:">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxTextBox ID="CopyProfileTextBox" runat="server" Width="170px" ClientInstanceName="CopyProfileTxtBox">
                                <ClientSideEvents KeyDown="function(s, e) {OnKeyDown(s, e);}" />
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxButton ID="OKCopy" runat="server" CssClass="sysButton" Text="OK" OnClick="OKCopy_Click"
                                ClientInstanceName="goButton">
                            </dx:ASPxButton>
                            <dx:ASPxButton ID="KeyOKSave" runat="server" OnClick="KeyOKSave_Click" Text="OK"
                                CssClass="sysButton">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
