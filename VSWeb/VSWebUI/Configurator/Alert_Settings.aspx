<%@ Page Title="VitalSigns Plus - Alert Settings" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Alert_Settings.aspx.cs" Inherits="VSWebUI.WebForm8" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>










<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
    <link rel="stylesheet" href="../css/intlTelInput.css">
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <style type="text/css">
        .hide {display:none}
        .dispError {color:Red}
        .dispValid {color: Green}
        .iti-flag {background-image: url("../images/flags.png");}
        .style1
        {
            width: 100%;
        }
        .dxm-horizontal
        {
            float: right!important;
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
        //4/4/2014 NS added
        function OnCheckedChanged(s, e) {
            var ischecked = chkBoxPersistent.GetChecked();
            if (ischecked) {
                lblDuration.SetVisible(true);
                lblInterval.SetVisible(true);
                comboBoxInterval.SetVisible(true);
                lblMin.SetVisible(true);
                lblHr.SetVisible(true);
            }
            else {
                lblDuration.SetVisible(false);
                lblInterval.SetVisible(false);
                comboBoxInterval.SetVisible(false);
                lblMin.SetVisible(false);
                lblHr.SetVisible(false);
            }
        }
        //7/10/2014 NS added for VSPLUS-812
        function OnCheckedChangedLimits(s, e) {
            var ischecked = chkBoxAlertLimits.GetChecked();
            if (ischecked) {
                lblMaxDef.SetVisible(true);
                txtBoxPerDef.SetVisible(true);
                lblMaxDay.SetVisible(true);
                txtBoxPerDay.SetVisible(true);
            }
            else {
                lblMaxDef.SetVisible(false);
                txtBoxPerDef.SetVisible(false);
                lblMaxDay.SetVisible(false);
                txtBoxPerDay.SetVisible(false);
            }
        }
        //10/20/2014 NS added for VSPLUS-730
        function OnRepeatAlertCheckedChanged(s, e) {
            var ischecked = cbRepeatOccur.GetChecked();
            if (ischecked) {
                lblRepeatOccur.SetVisible(true);
                txtRepeatOccur.SetVisible(true);
                grid.SetVisible(true);
            }
            else {
                lblRepeatOccur.SetVisible(false);
                txtRepeatOccur.SetVisible(false);
                grid.SetVisible(false);
            }
        }
        function OnSaveClick(s, e) {
            grid.UpdateEdit();
        }
        function OnItemClick(s, e) {
            if (e.item.parent == s.GetRootItem())
                e.processOnServer = false;
        }
        //6/23/2015 NS added for VSPLUS-1862
        //7/1/2015 NS modified for VSPLUS-1927
        function OnCheckedChangedSNMP(s, e) {
            var ischecked = SNMPCheckBox.GetChecked();
            SNMPTextBox.SetEnabled(ischecked);
        }

        //7/17/2015 NS added for VSPLUS-1562
        var visibleIndex;
        function OnCustomButtonClick(s, e) {
            visibleIndex = e.visibleIndex;

            if (e.buttonID == "deleteButton")
                EmergencyUsersGridView.GetRowValues(e.visibleIndex, 'Email', OnGetRowValues);

            function OnGetRowValues(values) {
                var id = values[0];
                var name = values[1];
                var OK = (confirm('Are you sure you want to delete the email - ' + values + '?'))
                if (OK == true) {
                    EmergencyUsersGridView.DeleteRow(visibleIndex);
                }
            }
        }
   </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <table width="99%">
        <tr>
            <td valign="top">    
                <div class="header" id="servernamelbldisp" runat="server">Alert Settings</div>
                <div id="successDiv" runat="server" class="alert alert-success" style="display: none">Alert Settings were successully updated.
                        <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                </div>
				<div id="successDiv2" runat="server" class="alert alert-success" style="display: none"></div>
                <div id="errorDiv2" runat="server" class="alert alert-danger" style="display: none">Error
                </div>
            </td>
            <td valign="top" align="right">
                <table>
                    <tr>
                        <td>
                            <div id="alertsOn" runat="server" class="success" style="display: none">Alerts are ON.
                                </div>
                                <div id="alertsOff" runat="server" class="error" style="display: none;">Alerts are OFF.
                                </div>
                            <dx:ASPxImage ID="AlertsImg" runat="server" 
                                ImageUrl="~/images/icons/redbell.png" ShowLoadingImage="true" Visible="False" 
                                Width="30px" Height="30px"
                                ImageAlign="Right">
                            </dx:ASPxImage>
                        </td>
                        <td valign="top">
                            <dx:ASPxMenu ID="ASPxMenu1" runat="server" EnableTheming="True" 
                    HorizontalAlign="Right" onitemclick="ASPxMenu1_ItemClick" ShowAsToolbar="True" 
                    Theme="Moderno">
                    <ClientSideEvents ItemClick="OnItemClick" />
                    <Items>
                        <dx:MenuItem Name="MainItem">
                            <Items>
                                <dx:MenuItem Name="TurnOnItem" Text="Turn Alerts On">
                                </dx:MenuItem>
                                <dx:MenuItem Name="TurnOffItem" Text="Turn Alerts Off">
                                </dx:MenuItem>
                                <dx:MenuItem Name="ViewHistoryItem" Text="View Alert History">
                                </dx:MenuItem>
                                <dx:MenuItem Name="ClearAlertsItem" Text="Clear Alerts">
                                </dx:MenuItem>
                                <dx:MenuItem Name="DeleteAlertsItem" Text="Delete ALL Alerts">
                                </dx:MenuItem>
                                <dx:MenuItem Name="DeleteAlerts30Item" Text="Delete Alerts Older Than 30 Days">
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
        <%--   <tr>
            <td colspan="2">
                <dx:ASPxRoundPanel ID="AlertRoundPanel" runat="server" 
                    HeaderText="Default Settings" Theme="Glass" Width="99%" Height="101px">
                    <PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
    <table width="50%" >
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Primary MailServer:" 
                    CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxComboBox ID="PrimaryComboBox" runat="server">
                </dx:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Secondary Server:" 
                    CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxComboBox ID="SecondaryComboBox" runat="server">
                </dx:ASPxComboBox>
            </td>
        </tr>
    </table>
                        </dx:PanelContent>
</PanelCollection>
                </dx:ASPxRoundPanel>
            </td>
        </tr>--%><%-- Attempted use of toggle switch - there is more to it as there are some server side operations that need to happen
               <tr>
            <td colspan="2">
                <div class="container">
                    <div class="switch switch-blue">
                      <input type="radio" class="switch-input" name="view2" value="alertOn" id="alertOn" checked="checked" />
                      <label for="alertOn" class="switch-label switch-label-off">Alerts On</label>
                      <input type="radio" class="switch-input" name="view2" value="alertOff" id="alertOff" />
                      <label for="alertOff" class="switch-label switch-label-on">Alerts Off</label>
                      <span class="switch-selection"></span>
                    </div>
                </div>

            </td>
        </tr>
        --%>
        <tr>
            <td colspan="2" valign="top">
                <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
                    Theme="Glass" Width="100%" EnableHierarchyRecreation="False">
                    <TabPages>
                        <dx:TabPage Text="Alerting Options">
                            <ContentCollection>
                                <dx:ContentControl runat="server">
                                    <table width="100%">
                                        <tr>
                                            <td valign="top" width="50%">
                                                <dx:ASPxRoundPanel ID="LogRoundPanel" runat="server" 
                                                    HeaderText="Advanced Alerting" Theme="Glass" Width="100%" 
                                                    EnableHierarchyRecreation="False">
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent1" runat="server">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <div ID="infoDivLog" class="info" style="display: none">
                                                                            By default, alerts are sent via e-mail. If you enable the flag below, all alerts 
                                                                            will be logged in the Windows Events Log instead.
                                                                        </div>
                                                                        <dx:ASPxCheckBox ID="WinLogCheckBox" runat="server" CheckState="Unchecked" 
                                                                            Text="Write alerts to Windows log" Visible="False">
                                                                        </dx:ASPxCheckBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <div ID="infoDivPersistent" class="info">
                                                                            By default, an alert is sent at the time of an exception and when it is closed. 
                                                                            Enabling persistent alerting will allow you to receive notifications repeatedly 
                                                                            until an exception is cleared or for the duration in hours specified below.
                                                                        </div>
                                                                        <dx:ASPxCheckBox ID="PersistentCheckBox" runat="server" AutoPostBack="True" 
                                                                            Checked="True" CheckState="Checked" ClientInstanceName="chkBoxPersistent" 
                                                                            OnCheckedChanged="PersistentCheckBox_CheckedChanged" 
                                                                            Text="Enable Persitent Alerting">
                                                                            <ClientSideEvents CheckedChanged="function(s,e){OnCheckedChanged(s,e)}" />
<ClientSideEvents CheckedChanged="function(s,e){OnCheckedChanged(s,e)}"></ClientSideEvents>
                                                                        </dx:ASPxCheckBox>
                                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <dx:ASPxLabel ID="IntervalLabel" runat="server" 
                                                                                                ClientInstanceName="lblInterval" CssClass="lblsmallFont" Text="Alert Interval:" 
                                                                                                Wrap="False">
                                                                                            </dx:ASPxLabel>
                                                                                        </td>
                                                                                        <td>
                                                                                            <dx:ASPxComboBox ID="IntervalComboBox" runat="server" 
                                                                                                ClientInstanceName="comboBoxInterval" SelectedIndex="0">
                                                                                                <Items>
                                                                                                    <dx:ListEditItem Selected="True" Text="15" Value="15" />
                                                                                                    <dx:ListEditItem Text="30" Value="30" />
                                                                                                    <dx:ListEditItem Text="45" Value="45" />
                                                                                                    <dx:ListEditItem Text="60" Value="60" />
                                                                                                    <dx:ListEditItem Text="90" Value="90" />
                                                                                                    <dx:ListEditItem Text="120" Value="120" />
                                                                                                </Items>
                                                                                            </dx:ASPxComboBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <dx:ASPxLabel ID="MinLabel" runat="server" ClientInstanceName="lblMin" 
                                                                                                ClientVisible="False" CssClass="lblsmallFont" Text="minutes">
                                                                                            </dx:ASPxLabel>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <dx:ASPxLabel ID="DurationLabel" runat="server" 
                                                                                                ClientInstanceName="lblDuration" CssClass="lblsmallFont" Text="Alert Duration:">
                                                                                            </dx:ASPxLabel>
                                                                                        </td>
                                                                                        <td>
                                                                                            <dx:ASPxComboBox ID="DurationComboBox" runat="server" SelectedIndex="1">
                                                                                                <Items>
                                                                                                    <dx:ListEditItem Text="Until Cleared" Value="0" />
                                                                                                    <dx:ListEditItem Selected="True" Text="1" Value="1" />
                                                                                                    <dx:ListEditItem Text="2" Value="2" />
                                                                                                    <dx:ListEditItem Text="3" Value="3" />
                                                                                                    <dx:ListEditItem Text="4" Value="4" />
                                                                                                    <dx:ListEditItem Text="5" Value="5" />
                                                                                                    <dx:ListEditItem Text="6" Value="6" />
                                                                                                    <dx:ListEditItem Text="7" Value="7" />
                                                                                                    <dx:ListEditItem Text="8" Value="8" />
                                                                                                    <dx:ListEditItem Text="9" Value="9" />
                                                                                                    <dx:ListEditItem Text="10" Value="10" />
                                                                                                    <dx:ListEditItem Text="11" Value="11" />
                                                                                                    <dx:ListEditItem Text="12" Value="12" />
                                                                                                </Items>
                                                                                            </dx:ASPxComboBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <dx:ASPxLabel ID="HoursLabel" runat="server" ClientInstanceName="lblHr" 
                                                                                                CssClass="lblsmallFont" Text="hour(s)">
                                                                                            </dx:ASPxLabel>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ContentTemplate>
                                                                            <Triggers>
                                                                                <asp:AsyncPostBackTrigger ControlID="PersistentCheckBox" />
                                                                            </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>
                                                                
                                                            </table>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dx:ASPxRoundPanel>
                                                
                                            </td>
                                            <td valign="top" width="50%">   
                                                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server"
                                                    Width="100%" HeaderText="Emergency Alerting" Theme="Glass">
                                                    <PanelCollection>
<dx:PanelContent runat="server">
<div id="infoEmergencyDiv" class="info">Enter emergency contact email address(es) below.</div>
                                                
    <dx1:ASPxGridView ID="EmergencyUsersGridView" ClientInstanceName="EmergencyUsersGridView" runat="server" 
        Theme="Office2003Blue" Width="100%" AutoGenerateColumns="False" 
        OnRowDeleting="EmergencyUsersGridView_RowDeleting" 
        OnRowInserting="EmergencyUsersGridView_RowInserting" 
        OnRowUpdating="EmergencyUsersGridView_RowUpdating" KeyFieldName="ID">
        <ClientSideEvents CustomButtonClick="OnCustomButtonClick" />
        <Columns>
            <dx1:GridViewCommandColumn ButtonType="Image" Caption="Actions" 
                ShowInCustomizationForm="True" VisibleIndex="0" Width="60px">
                <HeaderStyle CssClass="GridCssHeader1" />
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
                </CellStyle>
            </dx1:GridViewCommandColumn>
            <dx1:GridViewCommandColumn ButtonType="Image" Caption="Delete" 
                ShowInCustomizationForm="True" VisibleIndex="1" Width="60px">
                <CustomButtons>
                    <dx1:GridViewCommandColumnCustomButton ID="deleteButton" Image-Url="../images/delete.png"
                        Text="Delete">
                        <Image Url="../images/delete.png">
                        </Image>
                    </dx1:GridViewCommandColumnCustomButton>
                </CustomButtons>
                <HeaderStyle CssClass="GridCssHeader1" />
                <CellStyle CssClass="GridCss1">
                </CellStyle>
            </dx1:GridViewCommandColumn>
            <dx1:GridViewDataTextColumn Caption="E-mail" FieldName="Email" 
                ShowInCustomizationForm="True" VisibleIndex="2">
                <PropertiesTextEdit>
                    







<ValidationSettings CausesValidation="True" SetFocusOnError="True">
					 















<RegularExpression ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                       ErrorText="Invalid e-mail"/>
                        







<RequiredField ErrorText="You must enter an e-mail address." IsRequired="True" />
                    







</ValidationSettings>
                







</PropertiesTextEdit>
            </dx1:GridViewDataTextColumn>
            <dx1:GridViewDataTextColumn Caption="ID" FieldName="ID" 
                ShowInCustomizationForm="True" Visible="False" VisibleIndex="3">
            </dx1:GridViewDataTextColumn>
        </Columns>
        <Styles>
            <AlternatingRow CssClass="GridAltRow">
            </AlternatingRow>
            <Header CssClass="GridCssHeader">
            </Header>
            <Cell CssClass="GridCss">
            </Cell>
            <EditForm BackColor="White">
            </EditForm>
        </Styles>
    </dx1:ASPxGridView>
                                                        </dx:PanelContent>
</PanelCollection>
                                                </dx:ASPxRoundPanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <dx:ASPxRoundPanel ID="LogRoundPanel0" runat="server" HeaderText="Alert Limits" 
                                                    Theme="Glass" Width="100%" EnableHierarchyRecreation="False">
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent2" runat="server">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td colspan="2">
                                                                                            <div ID="infoDivAlertLimits" class="info">
                                                                                                When alert limits are enabled, a value of &#39;0&#39; in each of the alert limit text 
                                                                                                boxes should be interpreted as &#39;none&#39;. I.e., if total maximum alerts per 
                                                                                                definition is set to 0 and total maximum alerts per day is set to 50, there will 
                                                                                                be no more than 50 alerts generated in a given day.
                                                                                            </div>
                                                                                            <dx:ASPxCheckBox ID="AlertLimitsCheckBox" runat="server" 
                                                                                                ClientInstanceName="chkBoxAlertLimits" 
                                                                                                oncheckedchanged="AlertLimitsCheckBox_CheckedChanged" 
                                                                                                Text="Enable Alert Limits">
                                                                                                <ClientSideEvents CheckedChanged="function(s,e){OnCheckedChangedLimits(s,e)}" />
                                                                                            </dx:ASPxCheckBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="2">
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <dx:ASPxLabel ID="MaxDefLabel" runat="server" ClientInstanceName="lblMaxDef" 
                                                                                                            CssClass="lblsmallFont" Text="Total Maximum Alerts per Definition:" 
                                                                                                            Wrap="False">
                                                                                                        </dx:ASPxLabel>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <dx:ASPxTextBox ID="AlertsPerDefTextBox" runat="server" 
                                                                                                            ClientInstanceName="txtBoxPerDef" Width="170px">
                                                                                                            <MaskSettings Mask="&lt;0..99999&gt;" />
                                                                                                            <MaskSettings Mask="&lt;0..99999&gt;" />
                                                                                                        </dx:ASPxTextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <dx:ASPxLabel ID="MaxDayLabel" runat="server" ClientInstanceName="lblMaxDay" 
                                                                                                            CssClass="lblsmallFont" Text="Total Maximum Alerts per Day:" Wrap="False">
                                                                                                        </dx:ASPxLabel>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <dx:ASPxTextBox ID="AlertsPerDayTextBox" runat="server" 
                                                                                                            ClientInstanceName="txtBoxPerDay" Width="170px">
                                                                                                            <MaskSettings Mask="&lt;0..99999&gt;" />
                                                                                                            <MaskSettings Mask="&lt;0..99999&gt;" />
                                                                                                        </dx:ASPxTextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dx:ASPxRoundPanel>
                                            </td>
                                            <td valign="top">
                                                <dx:ASPxRoundPanel ID="SNMPRoundPanel" runat="server" 
                                                    HeaderText="SNMP Listener" Theme="Glass" Width="100%" 
                                                    EnableHierarchyRecreation="False">
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent5" runat="server">
                                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        <dx:ASPxCheckBox ID="SNMPCheckBox" runat="server" ClientInstanceName="SNMPCheckBox" 
                                                                            OnCheckedChanged="SNMPCheckBox_CheckedChanged" 
                                                                            Text="Enable SNMP traps">
                                                                            <ClientSideEvents CheckedChanged="function(s,e){OnCheckedChangedSNMP(s,e)}" />
                                                                        </dx:ASPxCheckBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel14" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Host Name:" Wrap="False">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="SNMPTextBox" ClientInstanceName="SNMPTextBox" runat="server" Width="170px">
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="SNMPCheckBox" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                            
                                                            
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dx:ASPxRoundPanel>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="E-mail Alerting">
                            <ContentCollection>
                                <dx:ContentControl runat="server">
                                    <table class="navbarTbl">
                                        <tr>
                                            <td>
                                                
                                                <dx:ASPxRoundPanel ID="SMTPRoundPanel1" runat="server" 
                                                    HeaderText="Primary SMTP Server" Theme="Glass" Width="100%" 
                                                    Enabled="False">
                                                    <PanelCollection>
                                                        <dx:PanelContent runat="server">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Host Name:" Wrap="False">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="HostNameTextBox" runat="server" Width="170px">
                                                                            <ValidationSettings  ValidationGroup="p" ErrorDisplayMode="ImageWithTooltip">
																				<RequiredField IsRequired="true" ErrorText="Enter HostName" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel7" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Port:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="PortTextBox" runat="server" Width="70px">
                                                                            <MaskSettings Mask="&lt;0..99999&gt;" />
<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>

                                                                            <ValidationSettings CausesValidation="True" ValidationGroup="p" ErrorDisplayMode="ImageWithTooltip" 
                                                                                SetFocusOnError="True">
																					<RequiredField IsRequired="true" ErrorText="Enter Portno" />
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                    ValidationExpression="^\d+$" />
<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." ValidationExpression="^\d+$"></RegularExpression>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dx1:ASPxButton ID="TestConnectionButton1" runat="server" 
                                                                            OnClick="TestConnectionButton1_Click" Text="Test Connection" 
                                                                            CssClass="sysButton">
                                                                        </dx1:ASPxButton>
                                                                    </td>
                                                                     <td style="width:20px">
                                                                    </td>
                                                                    <td>
                                                                     <dx1:ASPxButton ID="Test1Btn" runat="server" Text="Test Message" 
                                                                            CssClass="sysButton" OnClick="Test1Btn_Click" ValidationGroup="p">
                                                                        </dx1:ASPxButton>
                                                                        </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="lblsmallFont" 
                                                                            Text="From:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="FromTextBox" runat="server" Width="170px">
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
																				<RequiredField IsRequired="true" ErrorText="Enter From " />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <dx:ASPxCheckBox ID="AuthCheckBox" runat="server" CheckState="Unchecked" 
                                                                            Text="Requires Authentication" Wrap="False">
                                                                        </dx:ASPxCheckBox>
                                                                    </td>
                                                                    <td rowspan="2">
                                                                        <div id="successTest1Div" class="alert alert-success" runat="server" style="display: none">Success</div>
                                                                        <div id="errorTest1Div" class="alert alert-danger" runat="server" style="display: none">Error</div>
                                                                    </td>
                                                               
                                                                      <td rowspan ="2">
                                                                    </td>
                                                                     <td rowspan="2">
                                                                        <div id="TMsgsuccess1" class="alert alert-success" runat="server" style="display: none">Success</div>
                                                                        <div id="TMsgerror1" class="alert alert-danger" runat="server" style="display: none">Error</div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel5" runat="server" CssClass="lblsmallFont" 
                                                                            Text="User ID:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="UserIDTextBox" runat="server" Width="170px">
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="p">
																			<RequiredField IsRequired="true" ErrorText="Enter UserID" />
                                                                                <RegularExpression ErrorText="Enter valid Email ID " 
                                                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
<RegularExpression ErrorText="Enter valid Email ID " ValidationExpression="\w+([-+.&#39;]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></RegularExpression>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <dx:ASPxCheckBox ID="SSLCheckBox" runat="server" CheckState="Unchecked" 
                                                                            Height="16px" Text="Requires SSL">
                                                                        </dx:ASPxCheckBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel6" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Password:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxButton ID="RegisterButton1" runat="server" ValidationGroup="p" CssClass="sysButton"
                                                                            OnClick="RegisterButton1_Click" 
                                                                            Text="Enter SMTP Password" Wrap="False">
                                                                        </dx:ASPxButton>
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <div ID="successDivPwd" runat="server" class="alert alert-success" 
                                                                            style="display: none">
                                                                            Password saved.
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                </tr>
                                                            </table>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dx:ASPxRoundPanel>
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dx:ASPxRoundPanel ID="SMTPRoundPanel2" runat="server" 
                                                    HeaderText="Secondary SMTP Server" Theme="Glass" Width="100%" 
                                                    Enabled="False">
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent3" runat="server">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel8" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Host Name:" Wrap="False">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="HostNameTextBox1" runat="server"  Width="170px">
                                                                            <ValidationSettings  ErrorDisplayMode="ImageWithTooltip"  ValidationGroup="s">
																			<RequiredField IsRequired="true" ErrorText="Enter HostName " />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel9" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Port:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="PortTextBox1" runat="server" Width="70px">
                                                                            <MaskSettings Mask="&lt;0..99999&gt;" />
<MaskSettings Mask="&lt;0..99999&gt;"></MaskSettings>

                                                                            <ValidationSettings ValidationGroup="s" ErrorDisplayMode="ImageWithTooltip" 
                                                                                SetFocusOnError="True">
																				<RequiredField IsRequired="true" ErrorText="Enter Port  " />
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                                                                    ValidationExpression="^\d+$" />
<RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." ValidationExpression="^\d+$"></RegularExpression>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        <dx1:ASPxButton ID="TestConnectionButton2" runat="server"  CausesValidation ="true" ValidationGroup="s"
                                                                            OnClick="TestConnectionButton2_Click" Text="Test Connection" 
                                                                            CssClass="sysButton">
                                                                        </dx1:ASPxButton>
                                                                    </td>

                                                                     <td style="width:20px">
                                                                    </td>
                                                                    <td>
                                                                     <dx1:ASPxButton ID="Test2Btn" runat="server" Text="Test Message"
                                                                            CssClass="sysButton" ValidationGroup="s" OnClick="Test2Btn_Click">
                                                                        </dx1:ASPxButton>
                                                                        </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel10" runat="server" CssClass="lblsmallFont" 
                                                                            Text="From:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="FromTextBox1" runat="server"  Width="170px">
                                                                            <ValidationSettings  ValidationGroup="s" ErrorDisplayMode="ImageWithTooltip">
																				<RequiredField IsRequired="true" ErrorText=" Enter Form" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <dx:ASPxCheckBox ID="AuthCheckBox1" runat="server" CheckState="Unchecked" 
                                                                            Text="Requires Authentication" Wrap="False">
                                                                        </dx:ASPxCheckBox>
                                                                    </td>
                                                                    <td rowspan="2">
                                                                        <div id="successTest2Div" class="alert alert-success" runat="server" style="display: none">Success</div>
                                                                        <div id="errorTest2Div" class="alert alert-danger" runat="server" style="display: none">Error</div>
                                                                    </td>       
                                                                      <td rowspan ="2">
                                                                    </td>
                                                                     <td rowspan="2">
                                                                        <div id="TMsgsuccess2" class="alert alert-success" runat="server" style="display: none">Success</div>
                                                                        <div id="TMsgerror2" class="alert alert-danger" runat="server" style="display: none">Error</div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel11" runat="server" CssClass="lblsmallFont" 
                                                                            Text="User ID:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="UserIDTextBox1" runat="server"  Width="170px">
                                                                            <ValidationSettings  ValidationGroup="s"  ErrorDisplayMode="ImageWithTooltip">
																				<RequiredField IsRequired="true" ErrorText="Enter UserID " />
                                                                                <RegularExpression ErrorText="Enter valid Email Id" 
                                                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
<RegularExpression ErrorText="Enter valid Email Id" ValidationExpression="\w+([-+.&#39;]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></RegularExpression>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <dx:ASPxCheckBox ID="SSLCheckBox1" runat="server" CheckState="Unchecked" 
                                                                            Text="Requires SSL">
                                                                        </dx:ASPxCheckBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxLabel ID="ASPxLabel12" runat="server" CssClass="lblsmallFont" 
                                                                            Text="Password:">
                                                                        </dx:ASPxLabel>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxButton ID="RegisterButton2" runat="server" CssClass="sysButton"
                                                                            OnClick="RegisterButton2_Click" 
                                                                            Text="Enter SMTP Password" Wrap="False" ValidationGroup="s"  CausesValidation="true">
																			
                                                                        </dx:ASPxButton>
                                                                    </td>
                                                                    <td colspan="2">
                                                                        &nbsp;</td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                </tr>
                                                            </table>
                                                            
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dx:ASPxRoundPanel>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="SMS/Text Alerting">
                            <ContentCollection>
                                <dx:ContentControl runat="server">
                                    <table class="navbarTbl">
                                        <tr>
                                            <td>
                                                <div id="smsinfoDiv" class="info">
                                                    <table>
                                                        <tr>
                                                            <td valign="top">
                                                                In order to utilize the SMS capability within 
                                                    VitalSigns, please sign up for an account with Twilio at 
                                                    <a class="viewby" target="_blank"  href="https://www.twilio.com/sms">https://www.twilio.com/sms </a>. Once you have established an active account, fill 
                                                    out the fields below and configure your individual Alert Definitions to receive 
                                                    SMS as needed. The Phone Number From field will contain a telephone number 
                                                    issued by Twilio that will appear on your SMS as a sender.
                                                            </td>
                                                            <td>
                                                                <img alt="" src="../images/icons/twilio.png" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                            
                                                <table>
                                                    <tr>
                                                        <td>
                                                            
                                                        </td>
                                                        <td>
                                                        
                                                        <dx:ASPxTextBox ID="SMSFromTextBox" ClientEnabled="true" ClientInstanceName="SMSFromTextBox" runat="server" Width="170px" Visible="false">
                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                                    <RegularExpression ErrorText="Invalid phone number. The field may only contain numeric values 0 through 9." 
                                                                        ValidationExpression="^\d+$" />
<RegularExpression ErrorText="Invalid phone number. The field may only contain numeric values 0 through 9." ValidationExpression="^\d+$"></RegularExpression>
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                            
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" 
                                                                Text="Account Sid:">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox ID="SMSSidTextBox" runat="server" Width="170px">
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Auth Token:" 
                                                                CssClass="lblsmallFont">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxTextBox ID="SMSAuthTokenTextBox" runat="server" Width="170px">
                                                            </dx:ASPxTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">
                                                            <dx:ASPxLabel ID="ASPxLabel18" runat="server" CssClass="lblsmallFont" 
                                                                Text="Phone Number From:">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                        <td valign="top">
                                                            <input type="tel" id="phone" />
                                                        <input type="hidden" id="hidden_phone" name="hidden_phone" value="" runat="server" />
                                                        <span id="valid-msg" class="hide">✓ Valid</span>
                                                        <span id="error-msg" class="hide">Invalid number</span>
                                                        
<script src="//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
<script src="../js/intlTelInput.min.js"></script>
<script>
    var telInput = $("#phone"),
        errorMsg = $("#error-msg"),
        validMsg = $("#valid-msg");
    var mJSVariable = <%:SMSFromNumber%>;
    var hiddenPhone = $("#hidden_phone");
    
    // initialise plugin
    telInput.intlTelInput({
        utilsScript: "../js/utils.js"
    });
    
    $("document").ready(function () {
        telInput.intlTelInput("setNumber", '+' + mJSVariable);
        hiddenPhone.val(mJSVariable);
        //alert(hiddenPhone.val);
    });
    var invalid = 1;
    // on blur: validate
    telInput.blur(function () {
        if ($.trim(telInput.val())) {
            if (telInput.intlTelInput("isValidNumber")) {
                validMsg.removeClass("hide");
                validMsg.addClass("dispValid");
                errorMsg.addClass("hide");
                invalid = 0;
            } else {
                errorMsg.removeClass("hide");
                errorMsg.addClass("dispError");
                validMsg.addClass("hide");
            }
        }
        var num=telInput.intlTelInput("getNumber");
        if (invalid == 0)
            document.getElementById("ContentPlaceHolder1_ASPxPageControl1_hidden_phone").value = num;
        else
            document.getElementById("ContentPlaceHolder1_ASPxPageControl1_hidden_phone").value = 'invalid';
    });

    // on keydown: reset
telInput.keydown(function() {
  errorMsg.addClass("hide");
  validMsg.addClass("hide");
});
</script>
                                                                                                               
                                               </td>
                                                     </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Recurring Alert Options">
                            <ContentCollection>
                                <dx:ContentControl runat="server">
                                    
                                            <table class="navbarTbl">
                                                <tr>
                                                    <td>
                                                        <div id="infoRepeatDiv" class="info">
                                                            Enable the flag &#39;Alert about recurrences only&#39; when you want to be notified of 
                                                            events that happen on a frequent basis and are not considered critical. The 
                                                            alerting service will issue a notification when the number of recurrences of an 
                                                            event reaches the value specified in the &#39;Number of recurrences field&#39; within 
                                                            the last hour of the service run time.
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <dx:ASPxCheckBox ID="RepeatOccurCheckBox" runat="server" ClientInstanceName="cbRepeatOccur"
                                                                        Text="Alert about recurrences only" 
                                                                        OnCheckedChanged="RepeatOccurCheckBox_CheckedChanged" AutoPostBack="True">
                                                                        
                                                                    </dx:ASPxCheckBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <dx:ASPxLabel ID="RepeatOccurLabel" runat="server" ClientInstanceName="lblRepeatOccur"
                                                                        Text="Number of recurrences (per hour):" CssClass="lblsmallFont">
                                                                    </dx:ASPxLabel>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxTextBox ID="RepeatOccurTextBox" runat="server" ClientInstanceName="txtRepeatOccur" Width="50px" Text="3">
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                        <tr>
                                            <td>
                                                <dx:ASPxButton ID="CollapseAllButton" runat="server" CssClass="sysButton"
                                                    ClientInstanceName="collapseAll" OnClick="CollapseAllButton_Click" 
                                                    Text="Collapse All" Wrap="False">
                                                    <Image Url="~/images/icons/forbidden.png">
                                                    </Image>
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                                <tr>
                                                    <td>
                                                        <dx:ASPxGridView ID="EventsGridView" runat="server" AutoGenerateColumns="False" 
                                                            ClientInstanceName="grid" EnableTheming="True" KeyFieldName="ID" 
                                                            OnCustomCallback="EventsGridView_CustomCallback" Theme="Office2003Blue" OnPageSizeChanged="EventsGridView_PageSizeChanged">
                                                            <Columns>
                                                                <dx:GridViewDataTextColumn Caption="Events" FieldName="Name" Name="Events" 
                                                                    ShowInCustomizationForm="True" VisibleIndex="1">
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataCheckColumn Caption="Alert about Event Recurrences" 
                                                                    FieldName="ConsecutiveFailures" Name="ConsecutiveFailures" 
                                                                    ShowInCustomizationForm="True" VisibleIndex="2">
                                                                    <DataItemTemplate>
                                                                        <dx:ASPxCheckBox ID="chkBoxCF" runat="server" 
                                                                            Checked='<%# Eval("ConsecutiveFailures") %>' ClientInstanceName="cb" 
                                                                            OnInit="chkBoxCF_Init">
                                                                        </dx:ASPxCheckBox>
                                                                    </DataItemTemplate>
                                                                </dx:GridViewDataCheckColumn>
                                                                <dx:GridViewDataTextColumn FieldName="ID" Name="ID" 
                                                                    ShowInCustomizationForm="True" Visible="False" VisibleIndex="3">
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn FieldName="ServerType" Name="ServerType" 
                                                                    ShowInCustomizationForm="True" VisibleIndex="0">
                                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                                    <CellStyle CssClass="GridCss">
                                                                    </CellStyle>
                                                                    <GroupFooterCellStyle CssClass="GridCss">
                                                                    </GroupFooterCellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                            </Columns>
                                                            <SettingsBehavior AutoExpandAllGroups="True" />

<SettingsBehavior AutoExpandAllGroups="True"></SettingsBehavior>

                                                            <SettingsPager AlwaysShowPager="True" >
                                                                <PageSizeItemSettings Visible="True">
                                                                </PageSizeItemSettings>
                                                            </SettingsPager>
                                                            <SettingsEditing Mode="Inline">
                                                            </SettingsEditing>
                                                            <Styles>
                                                                <Header CssClass="GridCssHeader">
                                                                </Header>
                                                                <GroupRow Font-Bold="True">
                                                                </GroupRow>
                                                                <AlternatingRow CssClass="GridAltRow">
                                                                </AlternatingRow>
                                                                <Cell CssClass="GridCss">
                                                                </Cell>
                                                            </Styles>
                                                            <Templates>
                                                                <GroupRowContent>
                                                                    <%# Container.GroupText%>
                                                                </GroupRowContent>
                                                            </Templates>
                                                        </dx:ASPxGridView>
                                                    </td>
                                                </tr>
                                    </table>
                                        
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                    </TabPages>
                </dx:ASPxPageControl>
            </td>
        </tr>
        <tr>
            <td colspan="2">
            <div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">The following fields were not updated:
                </div>
                <table >
                    <tr>
                        <td>
                            <dx:ASPxButton ID="OKButton" runat="server" Text="OK" CssClass="sysButton"
                               OnClick="OKButton_Click" Enabled="False">
                            </dx:ASPxButton>
                           </td>
                        <td>
                            <dx:ASPxButton ID="CancelButton" runat="server" Text="Cancel" CssClass="sysButton"
                                CausesValidation="False" 
                                OnClick="CancelButton_Click">
                            </dx:ASPxButton>
                           </td>
                    </tr>
                </table>
    <dx:ASPxLabel ID="AlertsOnLabel" runat="server" CssClass="lblsmallFont" 
                    Font-Bold="True" Text="Alerts On" Visible="false">
                </dx:ASPxLabel>

            </td>
          
        </tr>
    </table>
    <dx:ASPxPopupControl ID="MsgPopupControl" runat="server" 
                    HeaderText="Information" Modal="True" PopupHorizontalAlign="WindowCenter" 
                    PopupVerticalAlign="WindowCenter" Theme="Glass" Width="250px">
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
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="MsgButton" runat="server" OnClick="MsgButton_Click" 
                                            Text="OK" CssClass="sysButton">
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                
                <dx:ASPxPopupControl runat="server" PopupHorizontalAlign="WindowCenter" 
                    PopupVerticalAlign="WindowCenter" Modal="True" 
                    HeaderText="Enter a Password:" Theme="MetropolisBlue" 
                    ID="ResetPwdPopupControl">
					
     <ClientSideEvents PopUp="function(s, e) {
        ASPxClientEdit.ClearEditorsInContainerById('addcontentdiv');
    }" />
                    <ContentCollection>
<dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">
<div id="addcontentdiv">
                            <table>
                                <tr>
                                    <td colspan="2">
                                        <dx:ASPxTextBox ID="ResetPwdTextBox"  runat="server" Password="True" 
                                            Width="170px" ClientInstanceName="resetPwdTxtBox">
											 <ValidationSettings ErrorDisplayMode="ImageWithTooltip" > <RequiredField IsRequired="True" ErrorText="Enter Password" ></RequiredField></ValidationSettings>
                                            <ClientSideEvents KeyDown="function(s, e) {OnKeyDown(s, e);}"></ClientSideEvents>
                                        </dx:ASPxTextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxButton runat="server" ClientInstanceName="goButton" CausesValidation="true" AutoPostBack="false"  Text="OK" CssClass="sysButton"
                                            ID="ResetPwdOKBtn" 
                                            OnClick="ResetPwdOKBtn_Click"></dx:ASPxButton>

                                    </td>
                                    <td>
                                        <dx:ASPxButton runat="server" Text="Cancel" CssClass="sysButton" CausesValidation="false"
                                            ID="ResetPwdCancelBtn" OnClick="ResetPwdCancelBtn_Click"></dx:ASPxButton>

                                    </td>
                                </tr>
                            </table>
    <dx:ASPxLabel ID="SetWhichPwd" runat="server" Text="" Visible="false">
    </dx:ASPxLabel>
	</div>
                        </dx:PopupControlContentControl>
</ContentCollection>
</dx:ASPxPopupControl>

<dx:ASPxPopupControl runat="server" PopupHorizontalAlign="WindowCenter" 
                    PopupVerticalAlign="WindowCenter" Modal="True" 
                    HeaderText="Test Message" Theme="MetropolisBlue" 
                    ID="TestMsgPopupControl1" Height="100px" Width="300px">
					
     <ClientSideEvents PopUp="function(s, e) {
        ASPxClientEdit.ClearEditorsInContainerById('addcontentdivs');
        }" />

         <ClientSideEvents Closing="function(s, e) {
        ASPxClientEdit.ClearEditorsInContainerById('addcontentdivs');
    }" />
   
                    <ContentCollection>
<dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server" SupportsDisabledAttribute="True">
<div id="addcontentdivs">
                            <table>
                                <tr>
                                      <td class="style2">
                <dx:ASPxLabel ID="ASPxLabel19" runat="server" CssClass="lblsmallFont" 
                    Text="Enter Email" Wrap="False">
                </dx:ASPxLabel>
            </td>
            <td class="style2">

            <dx:ASPxTextBox ID="TestMail"  runat="server" 
                                            Width="170px">
											 <ValidationSettings ErrorDisplayMode="ImageWithTooltip" > <RequiredField IsRequired="True" ></RequiredField>
                                             <RegularExpression ErrorText="Enter valid Email ID " ValidationExpression="\w+([-+.&#39;]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></RegularExpression>
                                             </ValidationSettings>
                                            <ClientSideEvents KeyDown="function(s, e) {OnKeyDown(s, e);}"></ClientSideEvents>
                                        </dx:ASPxTextBox>
            </td>
                                </tr>
                                <tr>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxButton runat="server" ClientInstanceName="goButton" 
                                            CausesValidation="true" AutoPostBack="false"  Text="Send" CssClass="sysButton"
                                            ID="SendTest1" OnClick="SendTest1_Click"  ></dx:ASPxButton>

                                    </td>
                                    <td>
                                        <dx:ASPxButton runat="server" Text="Cancel" CssClass="sysButton" CausesValidation="false"
                                            ID="CancelTest1" OnClick="CancelTest1_Click"  ></dx:ASPxButton>

                                    </td>
                                </tr>
                            </table>
    
	</div>
                        </dx:PopupControlContentControl>
</ContentCollection>
</dx:ASPxPopupControl>


                                    <dx:ASPxButton ID="AlertsOffButton" runat="server" 
                    onclick="AlertsOffButton_Click" Text="Turn Off" Theme="Office2010Blue" 
                    Wrap="False" Enabled="False" Visible="False">
                </dx:ASPxButton>
                <dx:ASPxButton ID="AlertsOnButton" runat="server" 
                    onclick="AlertsOnButton_Click" Text="Turn On" Theme="Office2010Blue" 
                    Wrap="False" Enabled="False" Visible="False">
                </dx:ASPxButton>
                <dx:ASPxRoundPanel ID="AlertActionsRoundPanel" runat="server" 
                    HeaderText="Alert Actions" Theme="Glass" Width="100%" Visible="False">
                    <PanelCollection>
<dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
    <table class="navbarTbl">
        <tr>
            <td>
                <dx:ASPxButton ID="ViewAlertHistoryButton" runat="server" 
                    OnClick="ViewAlertHistoryButton_Click" Text="View Alert History" 
                    Theme="Office2010Blue" Width="140px">
                </dx:ASPxButton>
            </td>
            <td>
                &nbsp;</td>
            <td>
                <dx:ASPxLabel ID="ASPxLabel15" runat="server" CssClass="lblsmallFont" 
                    Text="Displays the alert history page in the VitalSigns Dashboard.">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxButton ID="ClearAlertsButton" runat="server" Text="Clear Alerts" 
                    Theme="Office2010Blue" Width="140px" OnClick="ClearAlertsButton_Click">
                </dx:ASPxButton>
            </td>
            <td>
                &nbsp;</td>
            <td>
                <dx:ASPxLabel ID="ASPxLabel16" runat="server" CssClass="lblsmallFont" 
                    Text="Clears all currently active alerts using today's date. The alert information is NOT removed from the history table." 
                    Wrap="True">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxButton ID="DeleteAlertsButton" runat="server" Text="Delete Alerts" 
                    Theme="Office2010Blue" Width="140px" OnClick="DeleteAlertsButton_Click">
                </dx:ASPxButton>
            </td>
            <td>&nbsp;</td>
            <td>
                <dx:ASPxLabel ID="ASPxLabel17" runat="server" CssClass="lblsmallFont" 
                    Text="Deletes all alerts. The alert information is removed from the history table." 
                    Wrap="True">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="DeletePriorLabel" runat="server" CssClass="lblsmallFont" 
                    Text="Delete Alerts prior to:" Wrap="True">
                </dx:ASPxLabel>
            </td>
            <td colspan="2">
            <table>
                <tr>
                    <td align="right">
                        <dx:ASPxTextBox ID="DeleteAlertsTextBox" runat = "server" Width="50px" Text="30">
                            <MaskSettings Mask="&lt;1..99999&gt;" />
                            <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                    SetFocusOnError="True">
                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (0-9)." 
                                    ValidationExpression="^\d+$" />
                            </ValidationSettings>
                        </dx:ASPxTextBox>
                    </td>
                    <td align="left">
                        <dx:ASPxLabel ID="DeleteDaysLabel" runat="server" CssClass="lblsmallFont" 
                            Text="days" Wrap="True">
                        </dx:ASPxLabel>
                    </td>
                </tr>
            </table>                    
            </td>
        </tr>
    </table>
                        </dx:PanelContent>
</PanelCollection>
                </dx:ASPxRoundPanel>
</asp:Content>
