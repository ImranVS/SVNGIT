<%@ Page Title="VitalSigns Plus - Scheduled Reports" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="VSWebUI.Configurator.Reports" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
<link href="../css/bootstrap1.min.css" rel="stylesheet" />  
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
         function OnCheckChanged(s, e) {
             var visible = false;
             if (runSched.GetChecked()) {
                 visible = true;
             }
             crptSubjectLbl.SetVisible(visible);
             crptSubjectTextBox.SetVisible(visible);
             //3/19/2014 NS added for VSPLUS-486
             if (visible) {
                 crptSubjectTextBox.SetText(cRptListComboBox.GetSelectedItem().text);
             }
             crptFrequencyLabel.SetVisible(visible);
             crptFrequencyRadio.SetVisible(visible);
             crptSendToLabel.SetVisible(visible);
             crptSendToTextBox.SetVisible(visible);
             crptCopyToLabel.SetVisible(visible);
             crptCopyToTextBox.SetVisible(visible);
             crptBlindCopyToLabel.SetVisible(visible);
             crptBlindCopyToTextBox.SetVisible(visible);
             crptBodyLabel.SetVisible(visible);
             //crptBodyOptLabel.SetVisible(visible);
             crptBodyMemo.SetVisible(visible);
             crptFormatLabel.SetVisible(visible);
             crptFormatComboBox.SetVisible(visible);
             if (crptFrequencyRadio.GetSelectedItem().value == "1" && visible == true) {
                 //4/15/2014 NS added for VSPLUS-559
                 crptFrequencyCheckBox.SetVisible(visible);
                 crptFrequencyDayLabel.SetVisible(!visible);
                 crptFrequencyDayTextBox.SetVisible(!visible);
             }
             else if (crptFrequencyRadio.GetSelectedItem().value == "2" && visible == true) {
                 crptFrequencyDayLabel.SetVisible(visible);
                 crptFrequencyDayTextBox.SetVisible(visible);
                 crptFrequencyCheckBox.SetVisible(!visible);
             }
             else if (crptFrequencyRadio.GetSelectedItem().value == "0" && visible == true) {
                 crptFrequencyCheckBox.SetVisible(!visible);
                 crptFrequencyDayLabel.SetVisible(!visible);
                 crptFrequencyDayTextBox.SetVisible(!visible);
             }
             else {
                 crptFrequencyCheckBox.SetVisible(visible);
                 crptFrequencyDayLabel.SetVisible(visible);
                 crptFrequencyDayTextBox.SetVisible(visible);
             }
         }
         function OnSelectedIndexChanged(s, e) {
             var visible = false;
             var visible2 = false;
             if (crptFrequencyRadio.GetSelectedItem().value == "1") {
                 visible = true;
             }
             else if (crptFrequencyRadio.GetSelectedItem().value == "2") {
                 visible2 = true;
                 crptFrequencyCheckBox.UnselectAll();
             }
             crptFrequencyCheckBox.SetVisible(visible);
             crptFrequencyDayLabel.SetVisible(visible2);
             crptFrequencyDayTextBox.SetVisible(visible2);
         }
         var visibleIndex;
         function OnCustomButtonClick(s, e) {
             visibleIndex = e.visibleIndex;

             if (e.buttonID == "deleteButton")
                 RptGridView.GetRowValues(e.visibleIndex, 'ReportTitle', OnGetRowValues);

             function OnGetRowValues(values) {
                 var id = values[0];
                 var name = values[1];
                 var OK = (confirm('Are you sure you want to delete the scheduled report - ' + values + '?'))
                 if (OK == true) {
                     RptGridView.DeleteRow(visibleIndex);
                 }
             }
         }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%">
        <tr>
            <td>
                <div class="header" id="servernamelbldisp" runat="server">Scheduled Reports</div>
                <div class="input-prepend">&nbsp;</div>
            </td>
            <td align="right">
                <table>
                    <tr>
                        <td>
                            <dx:ASPxMenu ID="ASPxMenu1" runat="server" EnableTheming="True" 
                    HorizontalAlign="Right" onitemclick="ASPxMenu1_ItemClick" ShowAsToolbar="True" 
                    Theme="Moderno">
                    <ClientSideEvents ItemClick="OnItemClick" />
                    <Items>
                        <dx:MenuItem Name="MainItem">
                            <Items>
                                <dx:MenuItem Name="ViewAllItem" Text="View All Reports">
                                </dx:MenuItem>
                            </Items>
                            <Image Url="~/images/icons/Gear.png">
                            </Image>
                        </dx:MenuItem>
                    </Items>
                </dx:ASPxMenu>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>    
            <td>
                <div id="successDiv" class="alert alert-success" runat="server" style="display: none">Report has been successfully scheduled.
                </div>                    
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxButton ID="NewButton" runat="server" Text="New" CssClass="sysButton" 
                            onclick="NewButton_Click">
                    <Image Url="~/images/icons/add.png">
                    </Image>
                </dx:ASPxButton>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxGridView ID="RptGridView" ClientInstanceName="RptGridView" 
                    runat="server" Theme="Office2003Blue" 
                    Width="100%" AutoGenerateColumns="False" 
                    onhtmlrowcreated="RptGridView_HtmlRowCreated" 
                    onrowdeleting="RptGridView_RowDeleting" KeyFieldName="ID" 
                    onpagesizechanged="RptGridView_PageSizeChanged">
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
                            <ClearFilterButton Visible="True">
                                <Image Url="~/images/clear.png">
                                </Image>
                            </ClearFilterButton>                                                
                            <HeaderStyle CssClass="GridCssHeader1" />
                            <CellStyle CssClass="GridCss1">
                            </CellStyle>
                        </dx:GridViewCommandColumn>
                        <dx:GridViewCommandColumn ButtonType="Image" Caption="Delete" VisibleIndex="1" 
                            Width="50px">
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
                        <dx:GridViewDataTextColumn Caption="Report Title" FieldName="Name" 
                            VisibleIndex="2">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Frequency" FieldName="FrequencyDisp" 
                            VisibleIndex="3">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Send To" FieldName="SendTo" 
                            VisibleIndex="4">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Copy To" FieldName="CopyTo" 
                            VisibleIndex="5">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Blind Copy To" FieldName="BlindCopyTo" 
                            VisibleIndex="6">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="File Format" FieldName="FileFormat" 
                            VisibleIndex="7" Width="50px">
                            <HeaderStyle Wrap="True" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" Visible="False" 
                            VisibleIndex="8">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" />
                    <SettingsPager AlwaysShowPager="True">
                        <PageSizeItemSettings Visible="True">
                        </PageSizeItemSettings>
                    </SettingsPager>
                    <Settings ShowFilterRow="True" />
                    <SettingsText ConfirmDelete="Are you sure you  want to delete this record?" />
                    <Styles>
                        <Header CssClass="GridCssHeader">
                        </Header>
                        <Row CssClass="GridCss">
                        </Row>
                        <AlternatingRow CssClass="GridAltRow">
                        </AlternatingRow>
                    </Styles>
                </dx:ASPxGridView>
            </td>
        </tr>
    </table>
 </asp:Content>
