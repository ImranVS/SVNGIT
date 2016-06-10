<%@ Page Title="VitalSigns Plus - Maintenance Windows" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="MaintenanceWinList.aspx.cs" Inherits="VSWebUI.Configurator.MaintenanceWinList" %>

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
            MaintWinListGridView.GetRowValues(e.visibleIndex, 'Name', OnGetRowValues);

        function OnGetRowValues(values) {
            var id = values[0];
            var name = values[1];
            var OK = (confirm('Are you sure you want to delete the maintenance window - ' + values + '?'))
            if (OK == true) {
                MaintWinListGridView.DeleteRow(visibleIndex);
            }
        }
    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%">
        <tr>
		<td>
		<div id="successDiv" class="alert alert-success" runat="server" style="display: none">
                    Success.
                </div>
		</td>
		</tr>
		<tr>
		
            <td>
                <div class="header"  id="servernamelbldisp" runat="server">Maintenance Windows</div>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxButton ID="NewButton" runat="server" Text="New" CssClass="sysButton" 
                    onclick="NewButton_Click">
                    <Image Url="~/images/icons/add.png">
                                            </Image>
                </dx:ASPxButton>
                <dx:ASPxButton ID="DeletePastButton" runat="server" CssClass="sysButton" 
                    onclick="DeletePastButton_Click" Text="Delete Past Maintenance Windows" 
                    >
                </dx:ASPxButton>
            </td>
        </tr>
        <tr>
            <td>
                <%--  <dx:ASPxRoundPanel ID="MListRoundPanel" runat="server" 
                                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                        CssPostfix="Glass" 
                                        GroupBoxCaptionOffsetY="-24px" HeaderText="Maintenance Windows List" 
                                        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
                                        Width="100%" Height="250px">
                                        <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                                        <HeaderStyle Height="23px">
                                        <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                        </HeaderStyle>
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
--%>
                        <dx:ASPxGridView ID="MaintWinListGridView" runat="server" AutoGenerateColumns="False" ClientInstanceName="MaintWinListGridView"
                    Width="100%" OnPageSizeChanged="MaintWinListGridView_PageSizeChanged"
                            KeyFieldName="ID" OnHtmlRowCreated="MaintWinListGridView_HtmlRowCreated" 
                                                    
                    OnRowDeleting="MaintWinListGridView_RowDeleting" Theme="Office2003Blue">
                    <ClientSideEvents CustomButtonClick="OnCustomButtonClick" />
                            <Columns>
                                <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions"
                                    VisibleIndex="0" Width="70px">
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
                                    <CellStyle>
                                        <Paddings Padding="3px" />
                                    </CellStyle>
                                    <ClearFilterButton Visible="True">
                                        <Image Url="~/images/clear.png">
                                            </Image>
                                    </ClearFilterButton>
                                    <HeaderStyle CssClass="GridCssHeader" >
                                    <Paddings Padding="5px" />
                                    </HeaderStyle>
                                </dx:GridViewCommandColumn>
                                <dx:GridViewDataTextColumn VisibleIndex="3" Width="60px" Caption="Copy" 
                                    CellStyle-HorizontalAlign="Center" ReadOnly="true" 
                                    >
                                    <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader1" />
                                    <Settings AllowAutoFilter="False" />
                                    <EditFormSettings Visible="False" />
                                    <DataItemTemplate>           
                                        <%--<asp:Label ID="lblid" runat="server" Text='<%#Eval("ID") %>' Visible="false"></asp:Label>--%>
                                        <asp:ImageButton ID="btnrp" runat="server" Text="RP" OnClick="btn_click" CommandName="RP" ToolTip="Make a Copy " CommandArgument='<%#Eval("ID") %>' Width="15px" ImageUrl="~/Images/icons/page_white_stack.png" />
                                    </DataItemTemplate>
                                    <CellStyle CssClass="GridCss1"></CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True"
                                    Visible="False" VisibleIndex="2">
                                    <EditFormSettings Visible="False" />
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="Name" VisibleIndex="4" 
                                    SortOrder="Ascending">
                                    <PropertiesTextEdit DisplayFormatString="{0}">
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
                                <dx:GridViewDataDateColumn FieldName="StartDate" VisibleIndex="5">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataDateColumn FieldName="StartTime" VisibleIndex="6">
								<Settings AllowAutoFilter="False" />
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                    <PropertiesDateEdit DisplayFormatString="t">
                                    </PropertiesDateEdit>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataTextColumn FieldName="Duration" VisibleIndex="7">
								<Settings AllowAutoFilter="False" />
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataDateColumn FieldName="EndDate" VisibleIndex="8">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataTextColumn FieldName="MaintType" ReadOnly="True"
                                    VisibleIndex="9" Caption="Type">
                                    <EditCellStyle CssClass="GridCss">
                                    </EditCellStyle>
                                    <EditFormCaptionStyle CssClass="GridCss">
                                    </EditFormCaptionStyle>
                                    <HeaderStyle CssClass="GridCssHeader" />
                                    <CellStyle CssClass="GridCss">
                                    </CellStyle>
                                    <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewCommandColumn ButtonType="Image" Caption="Delete" VisibleIndex="1" 
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
                            <SettingsBehavior AllowSort="true" ConfirmDelete="True" ColumnResizeMode="NextColumn" />
                            <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                            <SettingsText ConfirmDelete="Are you sure you want to delete this record?" />
                            <Styles>
                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                </Header>
                                <LoadingPanel ImageSpacing="5px">
                                </LoadingPanel>
                                <GroupRow Font-Bold="True">
                                </GroupRow>
                                <AlternatingRow CssClass="GridAltRow" Enabled="True">
                        </AlternatingRow>
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
                            <SettingsPager PageSize="50" SEOFriendly="Enabled" >
            <PageSizeItemSettings Visible="true" />
        </SettingsPager>
                        </dx:ASPxGridView>
                <%--<asp:Label ID="lblid" runat="server" Text='<%#Eval("ID") %>' Visible="false"></asp:Label>--%>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxPopupControl ID="DeletePopupControl" runat="server" 
                    HeaderText="Delete Past Maintenance Windows?" Theme="Glass" Modal="True" 
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
                    Width="200px">
                    <ContentCollection>
<dx:PopupControlContentControl runat="server">
    <table>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Would you like to delete the maintenance windows that are past their end date?">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxButton ID="OKButton" runat="server" OnClick="OKButton_Click" Text="OK" 
                    Theme="Office2010Blue">
                </dx:ASPxButton>
                <dx:ASPxButton ID="CancelButton" runat="server" OnClick="CancelButton_Click" 
                    Text="Cancel" Theme="Office2010Blue">
                </dx:ASPxButton>
            </td>
        </tr>
    </table>
                        </dx:PopupControlContentControl>
</ContentCollection>
                </dx:ASPxPopupControl>
            </td>
        </tr>
    </table> 
</asp:Content>
