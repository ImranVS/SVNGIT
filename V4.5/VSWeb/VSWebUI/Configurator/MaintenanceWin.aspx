<%@ Page Title="VitalSigns Plus-MaintenanceWin" Language="C#" MasterPageFile="~/Site1.Master"
    AutoEventWireup="true" CodeBehind="MaintenanceWin.aspx.cs" Inherits="VSWebUI.Configurator.MaintenanceWin" %>
    <%@ Register Assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>






<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        
        function showendtime() {

            lStartTime = document.getElementById("ContentPlaceHolder1_MaitenanceRoundPanel_MaintStartTimeEdit_I");
            lDuration = document.getElementById("ContentPlaceHolder1_MaitenanceRoundPanel_MaintDurationTextBox_Raw");            
            lEndTime = document.getElementById("ContentPlaceHolder1_MaitenanceRoundPanel_lblEndTime");
            lblDuration = document.getElementById("ContentPlaceHolder1_MaitenanceRoundPanel_lblDuration");
            //9/16/2014 NS added for VSPLUS-911
            lstartDate = document.getElementById("ContentPlaceHolder1_MaitenanceRoundPanel_MaintStartDateEdit_I");
            lendDate = document.getElementById("ContentPlaceHolder1_MaitenanceRoundPanel_lblEndDate");
            
            var d = new Date(lstartDate.value);
            //alert(d);
            var curr_date = d.getDate();
            var curr_month = d.getMonth() + 1; //Months are zero based
            var curr_year = d.getFullYear();

            var d1 = new Date(curr_month + "/" + curr_date + "/" + curr_year + ' ' + lStartTime.value);
            //alert(d1);

            var d2 = new Date(d1.getTime() + (lDuration.value * 60000));
            //9/16/2014 NS added for VSPLUS-911
            //alert(d2);
            lendDate.innerHTML = d2.toLocaleDateString();
            //alert(d2);
            var hours = d2.getHours();
            //alert(hours);
            var minutes = d2.getMinutes();
            //alert(minutes);
            var ampm = hours >= 12 ? 'PM' : 'AM';
            hours = hours % 12;
            hours = hours ? hours : 12; // the hour '0' should be '12'
            minutes = minutes < 10 ? '0' + minutes : minutes;
            var strTime = hours + ':' + minutes + ' ' + ampm;


            lEndTime.innerHTML = strTime;
            //2/17/2014 NS modified - hours are calculated incorrectly
            //lblDuration.innerHTML = d2.getHours() + ' hour(s) ' + d2.getMinutes() + ' minutes';
            lblDuration.innerHTML = Math.floor(lDuration.value / 60) + ' hour(s) ' + (lDuration.value % 60) + ' minutes';
            // (d1.getHours() < 10 ? '0' + d1.getHours() : d1.getHours()) + ':' + (d1.getMinutes() < 10 ? '0' + d1.getMinutes() : d1.getMinutes());
             }
             function OnCbAllCheckedChanged(s, e) {
                 if (s.GetChecked()) {
                     cbList.SelectAll();
                 } else
                     cbList.UnselectAll();
             }

             //11/24/2015 NS added for VSPLUS-2227
             function OnSelectAllClick(s, e) {
                 cbList.SelectAll();
             }
             function OnClearAllClick(s, e) {
                 cbList.UnselectAll();
             }
             function MaintRepeatCheckBoxList_SelectIndexedChanged(s, e) {
                 var selectedItemsCount = s.GetSelectedItems().length;
                 cbAll.SetChecked(selectedItemsCount == s.GetItemCount());
             }
             function OnTextChanged(s, e) {
                 trackBar.SetPosition(s.GetText());
                 showendtime();
             }
    </script>
  
    <style type="text/css">
        .style1
        {
            height: 23px;
        }
        </style>
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
        <tr>
            <td colspan="2">
                <div class="header" id="servernamelbldisp" runat="server">Maintenance Window
	            </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <dx:ASPxRoundPanel ID="MaitenanceRoundPanel" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
        CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Maintenance Window"
        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%" 
        Height="250px" EnableHierarchyRecreation="False">
        <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
        <HeaderStyle Height="23px">
        </HeaderStyle>
        <PanelCollection>
            <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="DurationTrackBar" />
                    </Triggers>
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                    <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" Text="Name:">
                                    </dx:ASPxLabel>
                                </td>
                                            <td>
                                    <dx:ASPxTextBox ID="NameTextBox" runat="server" Width="170px">
                                        <ValidationSettings>
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2"></td>
                            </tr>
                            <tr>
                                <td valign="top" width="50%">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <div ID="infoDivDuration" runat="server" class="info" style="display: block">
                                        Enter duration in minutes below or use the slider control to set the duration.
                                    </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                    <table>
                                        <tr>
                                            <td>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <dx:ASPxLabel ID="lblDurationType" runat="server" CssClass="lblsmallFont" 
                                                Text="Duration Type:" Wrap="False">
                                            </dx:ASPxLabel>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="MaintRepeatRadioButtonList" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                            <td>
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <dx:ASPxRadioButtonList ID="RadioButtonListEndDate" runat="server" 
                                                AutoPostBack="True" CssClass="lblsmallFont" CssFilePath="~/css/vswebforms.css" 
                                                OnSelectedIndexChanged="RadioButtonListEndDate_SelectedIndexChanged" 
                                                RepeatDirection="Horizontal" SelectedIndex="0" TextWrap="False">
                                                <Items>
                                                    <dx:ListEditItem Selected="True" Text="Continue Until" Value="1" />
                                                    <dx:ListEditItem Text="Forever" Value="2" />
                                                </Items>
                                            </dx:ASPxRadioButtonList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="MaintRepeatRadioButtonList" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                    <dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" 
                                        Text="Start Date:" Wrap="False">
                                    </dx:ASPxLabel>
                                </td>
                                            <td>
                                    <dx:ASPxDateEdit ID="MaintStartDateEdit" runat="server" Width="100px">
                                        <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                            SetFocusOnError="True">
                                            <RequiredField ErrorText="Start Date is a required field" IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxDateEdit>
                                </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <dx:ASPxLabel ID="ASPxLabelEndDate" runat="server" CssClass="lblsmallFont" 
                                                            Text="End Date:" Wrap="False">
                                                        </dx:ASPxLabel>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="RadioButtonListEndDate" />
                                                        <asp:AsyncPostBackTrigger ControlID="MaintRepeatRadioButtonList" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <dx:ASPxDateEdit ID="MaintEndDateEdit" runat="server" Width="100px">
                                                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                SetFocusOnError="True">
                                                                <RequiredField ErrorText="End Date is a required field" IsRequired="True" />
                                                            </ValidationSettings>
                                                        </dx:ASPxDateEdit>
                                                        <dx:ASPxLabel ID="lblEndDate" runat="server" CssClass="lblsmallFont">
                                                        </dx:ASPxLabel>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="MaintRepeatRadioButtonList" />
                                                        <asp:AsyncPostBackTrigger ControlID="RadioButtonListEndDate" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dx:ASPxLabel ID="ASPxLabel7" runat="server" CssClass="lblsmallFont" 
                                                    Text="Start Time:" Wrap="False">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>
                                                <dx:ASPxTimeEdit ID="MaintStartTimeEdit"  runat="server" AutoPostBack="True" 
                                        ClientInstanceName="StartTime"
                                        onvaluechanged="MaintStartTimeEdit_ValueChanged" Width="100px" EditFormat="Time" DateTime="1/2/2015">
										 <ValidationSettings>
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                                    <%--<ClientSideEvents DateChanged="OnDateChanged" />--%>
                                                </dx:ASPxTimeEdit>
                                            </td>
                                            <td>
                                                <dx:ASPxLabel ID="ASPxLabel10" runat="server" CssClass="lblsmallFont" 
                                                    Text="End Time:" Wrap="False">
                                                </dx:ASPxLabel>
                                    </td>
                                            <td>
                                                <dx:ASPxLabel ID="lblEndTime" runat="server" ClientInstanceName="EndTime" 
                                                    CssClass="lblsmallFont" Wrap="False">
                                                </dx:ASPxLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dx:ASPxLabel ID="ASPxLabel8" runat="server" CssClass="lblsmallFont" 
                                                    Text="Duration:" Wrap="False">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>
                                               <dx:ASPxTextBox ID="MaintDurationTextBox" runat="server" 
                                                    ClientInstanceName="MaintDuration" Visible="True" Width="100px">
                                                    <MaskSettings Mask="&lt;0..1440&gt;" />
                                                    <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                        SetFocusOnError="True">
                                                        <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                            ValidationExpression="^\d+$" />
                                                    </ValidationSettings>
                                                    <ClientSideEvents LostFocus="OnTextChanged" />
                                                    <%--<Border BorderStyle="None" />--%>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td colspan="2">
                                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" 
                                                    Text="minutes=" Wrap="False">
                                                </dx:ASPxLabel>
                                                <dx:ASPxLabel ID="lblDuration" runat="server" ClientInstanceName="Duration" 
                                                    CssClass="lblsmallFont" Text="minutes" Wrap="False">
                                                </dx:ASPxLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <dx:ASPxTrackBar ID="DurationTrackBar" runat="server" AutoPostBack="True" 
                                                    ClientInstanceName="trackBar" 
                                                    CssFilePath="~/App_Themes/Office2010Black/{0}/styles.css" 
                                                    CssPostfix="Office2010Black" EnableViewState="False" LargeTickEndValue="1440" 
                                                    LargeTickInterval="240" MaxValue="1440" 
                                                    OnPositionChanged="DurationTrackBar_PositionChanged" Position="0" 
                                                    PositionStart="0" ScalePosition="LeftOrTop" SmallTickFrequency="5" 
                                                    SpriteCssFilePath="~/App_Themes/Office2010Black/{0}/sprite.css" Step="5" 
                                                    Theme="Office2010Blue" Width="350px">
                                                    <ValueToolTipStyle BackColor="White">
                                                    </ValueToolTipStyle>
                                                    <%--<ClientSideEvents PositionChanged="OnPositionChanged" />--%>
                                                </dx:ASPxTrackBar>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                        </tr>
                                    </table>
                                 </td>
                                <td valign="top" width="50%">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div id="infoDivOnce" class="info" runat="server" style="display: block">Maintenance window will only occur <b>once</b>.
                                    </div>
                                    <div id="infoDivDaily" class="info" runat="server" style="display: none">Maintenance window will occur <b>every day of the week</b>.
                                    </div>
                                    <div id="infoDivWeekly" class="info" runat="server" style="display: none">Maintenance window will occur <b>every number of weeks</b> specified in the text box <b>on selected days of the week</b>.
                                    </div>
                                    <div id="infoDivMonthly" class="info" runat="server" style="display: none">Maintenence window will occur <b>on selected day(s) of the week on the first, second, third, or last week of the month</b> (depending on selection) 
                                        <b>every month</b> or <b>on a specific day of the month</b>.
                                    </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="MaintRepeatRadioButtonList" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                    <table width="100%">
                    <tr>
                        <td valign="top">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                            <tr>
                                                <td>
                                                    <dx:ASPxRadioButtonList ID="MaintRepeatRadioButtonList" runat="server" 
                                                                    AutoPostBack="True" CssClass="lblsmallFont" CssFilePath="~/css/vswebforms.css" 
                                                                    OnSelectedIndexChanged="MaintRepeatRadioButtonList_SelectedIndexChanged" 
                                                                    SelectedIndex="0" TextWrap="False" 
                                                        RepeatDirection="Horizontal">
                                                                    <Items>
                                                                        <dx:ListEditItem Selected="True" Text="One time" Value="1" />
                                                                        <dx:ListEditItem Text="Daily" Value="2" />
                                                                        <dx:ListEditItem Text="Weekly" Value="3" />
                                                                        <dx:ListEditItem Text="Monthly" Value="4" />
                                                                    </Items>
                                                                </dx:ASPxRadioButtonList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <table>
                                                            <tr>
                                                                <td align="right">
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <dx:ASPxLabel ID="MaintRepeatEveryLabel" runat="server" CssClass="lblsmallFont" 
                                                                                    Text="Repeat every:" Visible="False">
                                                                                </dx:ASPxLabel>
                                                                    
                                                                            </td>
                                                                            <td>
                                                                                <dx:ASPxTextBox ID="MaintRepeatWeeksTextBox" runat="server" Visible="False" 
                                                                                    Width="30px">
                                                                                </dx:ASPxTextBox>
                                                                            </td>
                                                                            <td>
                                                                                <dx:ASPxLabel ID="MaintRepeatWeeksLabel" runat="server" CssClass="lblsmallFont" 
                                                                        Text="weeks on" Visible="False">
                                                                    </dx:ASPxLabel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <dx:ASPxLabel ID="MaintRepeatMonthlyLabel" runat="server" 
                                                                        CssClass="lblsmallFont" Text="Repeat every month on:" Visible="False" 
                                                                        Wrap="False">
                                                                    </dx:ASPxLabel>
                                                                    <dx:ASPxLabel ID="MaintRepeatDailyLabel" runat="server" CssClass="lblsmallFont" 
                                                                        Text="Repeat every day on:" Visible="False" Wrap="False">
                                                                    </dx:ASPxLabel>
                                                                            </td>
                                                                            <td>
                                                                                <dx:ASPxComboBox ID="MaintRepeatMonthlyComboBox" runat="server" 
                                                                        AutoPostBack="True" 
                                                                        OnSelectedIndexChanged="MaintRepeatMonthlyComboBox_SelectedIndexChanged" 
                                                                        Visible="False" Width="150px">
                                                                        <Items>
                                                                            <dx:ListEditItem Text="First" Value="1" />
                                                                            <dx:ListEditItem Text="Second" Value="2" />
                                                                            <dx:ListEditItem Text="Third" Value="3" />
                                                                            <dx:ListEditItem Text="Last" Value="4" />
                                                                            <dx:ListEditItem Text="Specific Day" Value="5" />
                                                                        </Items>
                                                                    </dx:ASPxComboBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="right">
                                                                                <dx:ASPxButton ID="SelectAllBtn" runat="server" 
                                                                        ClientInstanceName="btnSelectAll" CssClass="sysButton" Text="Select All" 
                                                                        Visible="false">
                                                                        <ClientSideEvents Click="OnSelectAllClick" />
                                                                    </dx:ASPxButton>
                                                                    
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="right">
                                                                                <dx:ASPxButton ID="ClearAllBtn" runat="server" ClientInstanceName="btnClearAll" 
                                                                        CssClass="sysButton" Text="Clear All" Visible="false">
                                                                        <ClientSideEvents Click="OnClearAllClick" />
                                                                    </dx:ASPxButton>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td rowspan="2">
                                                                    <dx:ASPxCheckBoxList ID="MaintRepeatCheckBoxList" runat="server" 
                                                                        ClientInstanceName="cbList" CssClass="lblsmallFont" RepeatColumns="1" 
                                                                        Visible="False">
                                                                        <ClientSideEvents SelectedIndexChanged="MaintRepeatCheckBoxList_SelectIndexedChanged" />
                                                                        <%--<ClientSideEvents SelectedIndexChanged="abc" />--%>
                                                                        <Items>
                                                                            <%--<dx:ListEditItem Text="Select All" Value="0" />--%>
                                                                            <dx:ListEditItem Text="Sunday" Value="7" />
                                                                            <dx:ListEditItem Text="Monday" Value="1" />
                                                                            <dx:ListEditItem Text="Tuesday" Value="2" />
                                                                            <dx:ListEditItem Text="Wednesday" Value="3" />
                                                                            <dx:ListEditItem Text="Thursday" Value="4" />
                                                                            <dx:ListEditItem Text="Friday" Value="5" />
                                                                            <dx:ListEditItem Text="Saturday" Value="6" />
                                                                        </Items>
                                                                    </dx:ASPxCheckBoxList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <dx:ASPxLabel ID="MaintRepeatDayLabel" runat="server" CssClass="lblsmallFont" 
                                                                                    Text="Day of the month:" Visible="False">
                                                                                </dx:ASPxLabel>
                                                                            </td>
                                                                            <td>
                                                                                <dx:ASPxTextBox ID="MaintRepeatDayTextBox" runat="server" Visible="False" 
                                                                                    Width="30px">
                                                                                    <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                                                        SetFocusOnError="True">
                                                                                        <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                            ValidationExpression="^\d+$" />
                                                                                    </ValidationSettings>
                                                                                </dx:ASPxTextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <dx:ASPxCheckBox ID="ASPxCheckBoxSelectAll" runat="server" 
                                                                        CheckState="Unchecked" ClientInstanceName="cbAll" Text="Select All" 
                                                                        Visible="false">
                                                                        <ClientSideEvents CheckedChanged="OnCbAllCheckedChanged" />
                                                                    </dx:ASPxCheckBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                </td>
                                            </tr>
                                        </table>    
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="MaintRepeatRadioButtonList" />
                                        </Triggers>
                                        </asp:UpdatePanel>
                            <%--<dx:ASPxComboBox ID="MaintNameComboBox" runat="server" AutoPostBack="True" DropDownStyle="DropDown" 
                                            OnSelectedIndexChanged="MaintNameComboBox_SelectedIndexChanged" 
                                            TextField="Name" ValueField="Name" EnableIncrementalFiltering="True" 
                                            IncrementalFilteringMode="StartsWith">
                                        </dx:ASPxComboBox>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxPopupControl ID="MaintenancePopupControl" runat="server" HeaderText="Information"
                                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                Theme="Glass" Width="300px">
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                                        <table class="style1">
                                            <tr>
                                                <td>
                                                    <dx:ASPxLabel ID="MsgLabel" runat="server" Text="ASPxLabel">
                                                    </dx:ASPxLabel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dx:ASPxButton ID="OKButton" runat="server" OnClick="OKButton_Click" Text="OK" Theme="Office2010Blue"
                                                        Width="60px">
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
                                </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </dx:PanelContent>

        </PanelCollection>
    </dx:ASPxRoundPanel>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" HeaderText="Servers"
                                
        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="100%">
                                <ContentPaddings PaddingBottom="10px" PaddingLeft="4px" PaddingTop="10px" />
                                <ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>
                                <HeaderStyle Height="23px">
                                </HeaderStyle>
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                                        <dx:ASPxGridView ID="MaintSrvGridView" runat="server" AutoGenerateColumns="False"
                                            KeyFieldName="__Key" Theme="Office2003Blue" 
                                            OnCustomUnboundColumnData="MaintSrvGridView_CustomUnboundColumnData" 
                                            Visible="False" OnPageSizeChanged="MaintSrvGridView_PageSizeChanged">
                                            <Columns>
                                                <dx:GridViewCommandColumn Caption="Select" ShowInCustomizationForm="True" ShowSelectCheckbox="True"
                                                    VisibleIndex="0">
                                                    <ClearFilterButton Visible="True">
                                                    </ClearFilterButton>
                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                    <CellStyle CssClass="GridCss1">
                                                    </CellStyle>
                                                </dx:GridViewCommandColumn>
                                                <dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True" ShowInCustomizationForm="True"
                                                    Visible="False" VisibleIndex="1">
                                                    <EditFormSettings Visible="False" />
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                    <CellStyle CssClass="GridCss">
                                                    </CellStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="ServerName" ShowInCustomizationForm="True"
                                                    VisibleIndex="2">
                                                    <Settings AutoFilterCondition="Contains" />
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCssHeader">
                                                        <Paddings Padding="5px" />
                                                    </HeaderStyle>
                                                    <CellStyle CssClass="GridCss">
                                                    </CellStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Server Type ID" FieldName="ServerTypeID" ShowInCustomizationForm="True"
                                                    Visible="False" VisibleIndex="4">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="ServerType" ShowInCustomizationForm="True"
                                                    VisibleIndex="3">
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                    <CellStyle CssClass="GridCss">
                                                    </CellStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="Description" ShowInCustomizationForm="True"
                                                    VisibleIndex="5">
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                    <CellStyle CssClass="GridCss">
                                                    </CellStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="Location" ShowInCustomizationForm="True" VisibleIndex="6">
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                    <CellStyle CssClass="GridCss">
                                                    </CellStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="IPAddress" ShowInCustomizationForm="True" Visible="False"
                                                    VisibleIndex="7">
                                                    <EditCellStyle CssClass="GridCss">
                                                    </EditCellStyle>
                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                    </EditFormCaptionStyle>
                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                    <CellStyle CssClass="GridCss">
                                                    </CellStyle>
                                                </dx:GridViewDataTextColumn>
                                            </Columns>
                                            <SettingsBehavior SortMode="Custom" />
                                            <SettingsPager PageSize="20">
                                                <PageSizeItemSettings Visible="True">
                                                </PageSizeItemSettings>
                                            </SettingsPager>
                                            <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                                            <Images SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css">
                                                <LoadingPanelOnStatusBar Url="~/App_Themes/Office2010Blue/GridView/Loading.gif">
                                                </LoadingPanelOnStatusBar>
                                                <LoadingPanel Url="~/App_Themes/Office2010Blue/GridView/Loading.gif">
                                                </LoadingPanel>
                                            </Images>
                                            <ImagesFilterControl>
                                                <LoadingPanel Url="~/App_Themes/Office2010Blue/GridView/Loading.gif">
                                                </LoadingPanel>
                                            </ImagesFilterControl>
                                            <Styles>
                                                <GroupRow Font-Bold="True">
                                                </GroupRow>
                                                <AlternatingRow CssClass="GridAltrow" Enabled="True">
                                                </AlternatingRow>
                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                </Header>
                                                <Cell Wrap="True">
                                                </Cell>
                                                <LoadingPanel ImageSpacing="5px">
                                                </LoadingPanel>
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
                                        </dx:ASPxGridView>
                                        <dx:ASPxButton ID="CollapseAllSrvButton" runat="server" 
                                             Text="Collapse All" CssClass="sysButton"
                                            Wrap="False" CausesValidation="False" OnClick="CollapseAllSrvButton_Click">
                                            <Image Url="~/images/icons/forbidden.png">
                                            </Image>
                                        </dx:ASPxButton>
                                        <br /><br />
                                       <dx:ASPxTreeList ID="ServersTreeList" runat="server" 
                                                                    AutoGenerateColumns="False" 
                                                                    KeyFieldName="Id" ParentFieldName="LocId" 
                                                                    Theme="Office2003Blue" Width="100%" OnDataBound="DataBound">
                                                                    <Columns>
                                                                        <dx:TreeListTextColumn Caption="Servers  " FieldName="Name" Name="Servers" 
                                                                            ShowInCustomizationForm="True" VisibleIndex="0">
                                                                            <CellStyle CssClass="GridCss">
                                                                            </CellStyle>
                                                                        </dx:TreeListTextColumn>
                                                                        <dx:TreeListTextColumn FieldName="actid" Name="actid" 
                                                                            ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
                                                                        </dx:TreeListTextColumn>
                                                                        <dx:TreeListTextColumn FieldName="tbl" Name="tbl" 
                                                                            ShowInCustomizationForm="True" Visible="False" VisibleIndex="2">
                                                                        </dx:TreeListTextColumn>
                                                                        <dx:TreeListTextColumn FieldName="LocId" Name="LocId" 
                                                                            ShowInCustomizationForm="True" Visible="False" VisibleIndex="3">
                                                                        </dx:TreeListTextColumn>
                                                                        <dx:TreeListTextColumn FieldName="ServerType" ShowInCustomizationForm="True" 
                                                                            VisibleIndex="4">
                                                                            <CellStyle CssClass="GridCss">
                                                                            </CellStyle>
                                                                        </dx:TreeListTextColumn>
                                                                        <dx:TreeListTextColumn FieldName="Description" ShowInCustomizationForm="True" 
                                                                            VisibleIndex="5">
                                                                            <CellStyle CssClass="GridCss">
                                                                            </CellStyle>
                                                                        </dx:TreeListTextColumn>
                                                                        <dx:TreeListTextColumn FieldName="srvtypeid" Name="srvtypeid" 
                                                                            ShowInCustomizationForm="True" Visible="False" VisibleIndex="6">
                                                                        </dx:TreeListTextColumn>
                                                                    </Columns>
                                                                       <Summary>
                                                        <dx:TreeListSummaryItem   FieldName="Name" SummaryType="Count" ShowInColumn="Name" DisplayFormat="{0} Item(s)" /></Summary>
                                                                    <Settings GridLines="Both" SuppressOuterGridLines="True" />
                                                                    <SettingsBehavior AllowDragDrop="False" AutoExpandAllNodes="True" />
                                                                    <settings gridlines="Both" suppressoutergridlines="True" />
                                                                    <settingsbehavior allowdragdrop="False" autoexpandallnodes="True" />

<Settings GridLines="Both" SuppressOuterGridLines="True"></Settings>

<SettingsBehavior AutoExpandAllNodes="True" AllowDragDrop="False"></SettingsBehavior>

                                                                    <SettingsPager AlwaysShowPager="True" Mode="ShowPager" PageSize="20">
                                                                        <PageSizeItemSettings Visible="True">
                                                                        </PageSizeItemSettings>
                                                                    </SettingsPager>
                                                                    <SettingsSelection AllowSelectAll="True" Enabled="True" Recursive="True" />
                                                                    
                                                                    <settingsselection allowselectall="True" enabled="True" recursive="True" />
                                                                    
<SettingsSelection Enabled="True" AllowSelectAll="True" Recursive="True"></SettingsSelection>

                                                                    <Styles>
                                                                        <LoadingPanel ImageSpacing="5px">
                                                                        </LoadingPanel>
                                                                        <Header CssClass="GridCssHeader">
                                                                        </Header>
                                                                        <Node CssClass="GridCss">
                                                                        </Node>
                                                                        <AlternatingNode Enabled="True">
                                                                        </AlternatingNode>
                                                                        <Cell CssClass="GridCss" Wrap="True">
                                                                        </Cell>
                                                                    </Styles>
                                                                    <StylesPager>
                                                                        <PageNumber ForeColor="#3E4846">
                                                                        </PageNumber>
                                                                        <Summary ForeColor="#1E395B">
                                                                        </Summary>
                                                                    </StylesPager>
                                                                    <StylesEditors ButtonEditCellSpacing="0">
                                                                    </StylesEditors>
                                                                </dx:ASPxTreeList>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>
            </td>
            <td valign="top">
                <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" HeaderText="Key Users" 
                    Theme="Glass" Width="100%">
                    <PanelCollection>
<dx:PanelContent runat="server">
    <dx:ASPxGridView ID="KeyUsersGridView" runat="server" Theme="Office2003Blue" 
        Width="100%" AutoGenerateColumns="False" KeyFieldName="DeviceID">
        <Columns>
            <dx:GridViewCommandColumn SelectAllCheckboxMode="Page" 
                ShowInCustomizationForm="True" ShowSelectCheckbox="True" VisibleIndex="0">
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn Caption="User Name" FieldName="UserName" 
                ShowInCustomizationForm="True" VisibleIndex="1">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Device Name" FieldName="DeviceName" 
                ShowInCustomizationForm="True" VisibleIndex="2">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Last Sync Time" FieldName="LastSyncTime" 
                ShowInCustomizationForm="True" VisibleIndex="3">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="ID" FieldName="DeviceID" 
                ShowInCustomizationForm="True" Visible="False" VisibleIndex="4">
            </dx:GridViewDataTextColumn>
        </Columns>
        <SettingsPager AlwaysShowPager="True">
            <PageSizeItemSettings Visible="True">
            </PageSizeItemSettings>
        </SettingsPager>
        <Styles>
            <Header CssClass="GridCssHeader">
            </Header>
            <AlternatingRow CssClass="GridAltRow">
            </AlternatingRow>
            <Cell CssClass="GridCss" Wrap="False">
            </Cell>
        </Styles>
    </dx:ASPxGridView>
                        </dx:PanelContent>
</PanelCollection>
                </dx:ASPxRoundPanel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div class="alert alert-danger" id="errorDiv" style="display: none" runat="server">
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <table>
        <tr>
            <td valign="top">
                <dx:ASPxButton ID="MaintAddButton" runat="server" CssClass="sysButton"
                    Text="Submit" OnClick="MaintAddButton_Click">
                </dx:ASPxButton>
            </td>
            <td valign="top">
                <dx:ASPxButton ID="MaintResetButton" runat="server" CssClass="sysButton"
                    OnClick="MaintResetButton_Click"
                    Text="Reset" CausesValidation="False">
                </dx:ASPxButton>
            </td>
            <td valign="top">
                <dx:ASPxButton ID="MaintCancelButton" runat="server" CssClass="sysButton"
                    Text="Cancel" OnClick="MaintCancelButton_Click" CausesValidation="False">
                </dx:ASPxButton>
            </td>
        </tr>
    </table>
            </td>
            <td>
                &nbsp;
                </td>
        </tr>
    </table>
</asp:Content>
