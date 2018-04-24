<%@ Page Title="VitalSigns Plus-Skype for Business Server" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="LyncServer.aspx.cs" Inherits="VSWebUI.Configurator.LyncServer" %>

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


           function OnItemClick(s, e) {
           	if (e.item.parent == s.GetRootItem())
           		e.processOnServer = false;
           }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table valign="top" align="right">
<tr><td >
                            <dx:ASPxMenu ID="ASPxMenu1" runat="server" EnableTheming="True" 
                    HorizontalAlign="Right"  onitemclick="ASPxMenu1_ItemClick" ShowAsToolbar="True" 
                    Theme="Moderno">
                    <ClientSideEvents ItemClick="OnItemClick" />
                    <Items>
                        <dx:MenuItem Name="MainItem">
                            <Items>
                                
                                <dx:MenuItem Name="ServerDetailsPage" Text="View Server Detailed Page">
                                </dx:MenuItem>
                               
                            </Items>
                            <Image Url="~/images/icons/Gear.png">
                            </Image>
                        </dx:MenuItem>
                    </Items>
                </dx:ASPxMenu>
                        </td></tr>
</table>
    <table width="100%">
        <tr>
            <td>
                <div class="header" id="lblServer" runat="server">Skype for Business Server</div>
                <asp:Label ID="lblServerId" runat="server" Font-Bold="False" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td>
                                    <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
                            EnableTheming="True" Font-Bold="True" Theme="Glass" Width="100%" EnableHierarchyRecreation="False">
                            <TabPages>
                                <dx:TabPage Text="Server Attributes">
                                    <TabImage Url="~/images/information.png">
                                    </TabImage>
                                    <ContentCollection>
                                        <dx:ContentControl ID="ContentControl1" runat="server">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" 
                                                HeaderText="Server Settings" Theme="Glass" Width="100%">
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent1" runat="server">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" 
                                                                        Text="Name:">
                                                                    </dx:ASPxLabel>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxTextBox ID="NameTextBox" runat="server" Width="170px">
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                                <td>
                                                                    &nbsp;</td>
                                                                <td>
                                                                    <dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" 
                                                                        Text="Description:">
                                                                    </dx:ASPxLabel>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxTextBox ID="DescTextBox" runat="server" Width="170px">
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                                <td>
                                                                    &nbsp;</td>
                                                                <td>
                                                                    <dx:ASPxCheckBox ID="EnabledCheckBox" runat="server" CheckState="Unchecked" 
                                                                        Text="Enabled for scanning" Wrap="False">
                                                                    </dx:ASPxCheckBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" 
                                                                        Text="Location:">
                                                                    </dx:ASPxLabel>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxTextBox ID="LocationTextBox" runat="server" Width="170px">
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                                <td>
                                                                    &nbsp;</td>
                                                                <td>
                                                                    <dx:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="lblsmallFont" 
                                                                        Text="Category:">
                                                                    </dx:ASPxLabel>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxTextBox ID="CategoryTextBox" runat="server" Width="170px">
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                                <td>
                                                                    &nbsp;</td>
                                                                <td>
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <dx:ASPxLabel ID="ASPxLabel14" runat="server" CssClass="lblsmallFont" 
                                                                        Text="Credentials:">
                                                                    </dx:ASPxLabel>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxComboBox ID="CredentialsComboBox" runat="server">
                                                                    </dx:ASPxComboBox>
                                                                </td>
                                                                <td>
                                                                    &nbsp;</td>
                                                                <td>
                                                                    &nbsp;</td>
                                                                <td>
                                                                    &nbsp;</td>
                                                                <td>
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
                                                <tr>
                                                    <td>
                                                        
                                                        <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" 
                                                            HeaderText="Scan Settings" Theme="Glass" Width="100%">
                                                            <PanelCollection>
                                                                <dx:PanelContent runat="server">
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <dx:ASPxLabel ID="ASPxLabel5" runat="server" CssClass="lblsmallFont" 
                                                                                    Text="Scan Interval:">
                                                                                </dx:ASPxLabel>
                                                                            </td>
                                                                            <td>
                                                                                <dx:ASPxTextBox ID="ScanIntvlTextBox" runat="server" Width="40px">
                                                                                </dx:ASPxTextBox>
                                                                            </td>
                                                                            <td>
                                                                                <dx:ASPxLabel ID="ASPxLabel6" runat="server" CssClass="lblsmallFont" 
                                                                                    Text="minutes">
                                                                                </dx:ASPxLabel>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;</td>
                                                                            <td>
                                                                                <dx:ASPxLabel ID="ASPxLabel11" runat="server" CssClass="lblsmallFont" 
                                                                                    Text="Response Threshold:">
                                                                                </dx:ASPxLabel>
                                                                            </td>
                                                                            <td>
                                                                                <dx:ASPxTextBox ID="ResponseTextBox" runat="server" Width="40px">
                                                                                </dx:ASPxTextBox>
                                                                            </td>
                                                                            <td>
                                                                                <dx:ASPxLabel ID="ASPxLabel12" runat="server" CssClass="lblsmallFont" 
                                                                                    Text="milliseconds">
                                                                                </dx:ASPxLabel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <dx:ASPxLabel ID="ASPxLabel7" runat="server" CssClass="lblsmallFont" 
                                                                                    Text="Retry Interval:">
                                                                                </dx:ASPxLabel>
                                                                            </td>
                                                                            <td>
                                                                                <dx:ASPxTextBox ID="RetryTextBox" runat="server" Width="40px">
																				<MaskSettings Mask="&lt;1..99999&gt;"></MaskSettings>
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" CausesValidation="True" SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Please enter a numeric value using the numbers only (1-9)."
                                                                                    ValidationExpression="^\d+$"></RegularExpression>
                                                                                <RequiredField IsRequired="True"></RequiredField>
                                                                            </ValidationSettings>
                                                                                </dx:ASPxTextBox>
                                                                            </td>
                                                                            <td>
                                                                                <dx:ASPxLabel ID="ASPxLabel8" runat="server" CssClass="lblsmallFont" 
                                                                                    Text="minutes">
                                                                                </dx:ASPxLabel>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;</td>
                                                                            <td>
                                                                                <dx:ASPxLabel ID="ASPxLabel13" runat="server" CssClass="lblsmallFont" 
                                                                                    Text="Failures before Alert:">
                                                                                </dx:ASPxLabel>
                                                                            </td>
                                                                            <td>
                                                                                <dx:ASPxTextBox ID="ConsFailuresBefAlertTextBox" runat="server" Width="40px">
                                                                                </dx:ASPxTextBox>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <dx:ASPxLabel ID="ASPxLabel9" runat="server" CssClass="lblsmallFont" 
                                                                                    Text="Off-Hours Scan Interval:">
                                                                                </dx:ASPxLabel>
                                                                            </td>
                                                                            <td>
                                                                                <dx:ASPxTextBox ID="OffscanTextBox" runat="server" Width="40px">
                                                                                </dx:ASPxTextBox>
                                                                            </td>
                                                                            <td>
                                                                                <dx:ASPxLabel ID="ASPxLabel10" runat="server" CssClass="lblsmallFont" 
                                                                                    Text="minutes">
                                                                                </dx:ASPxLabel>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;</td>
                                                                            <td>
                                                                                &nbsp;</td>
                                                                            <td>
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
                                <dx:TabPage Text="Disk Settings">
                                    <TabImage Url="~/images/drive.png">
                                    </TabImage>
                                    <ContentCollection>
                                        <dx:ContentControl ID="ContentControl2" runat="server">
                                            <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" 
                                                HeaderText="Disk Settings" Theme="Glass" Width="100%">
                                                <PanelCollection>
                                                    <dx:PanelContent runat="server">
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="SelCriteriaRadioButtonList" />
                                                            </Triggers>
                                                            <ContentTemplate>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td valign="top">
                                                                            
                                                                            <dx:ASPxRadioButtonList ID="SelCriteriaRadioButtonList" runat="server" 
                                                                                SelectedIndex="0" TextWrap="False" AutoPostBack="True" 
                                                                                onselectedindexchanged="SelCriteriaRadioButtonList_SelectedIndexChanged">
                                                                                <Items>
                                                                                    <dx:ListEditItem Selected="True" Text="All Disks - By Percentage" Value="0" />
                                                                                    <dx:ListEditItem Text="All Disks - By GB" Value="3" />
                                                                                    <dx:ListEditItem Text="Selected Disks" Value="1" />
                                                                                    <dx:ListEditItem Text="No Disk Alerts" Value="2" />
                                                                                </Items>
                                                                            </dx:ASPxRadioButtonList>
                                                                            
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div id="infoDiskDiv" class="info" runat="server" style="display: none">The column 'Free Space Threshold' should be set to a value in GB or Percent
                                                                                    (1 to 100) of the remaining free space when an alert should be generated. The column 'Threshold Type' should be set accordingly in either 
                                                                                    percent or GB.
                                                                                     </div>
                                                                                        <dx:ASPxLabel ID="DiskGridInfo" runat="server" Text="Please select disks from the grid below:" Visible="False" CssClass="lblsmallFont">
                                                                                        </dx:ASPxLabel>
                                                                                        <dx:ASPxLabel ID="DiskLabel" runat="server" Visible="True" CssClass="lblsmallFont">
                                                                                        </dx:ASPxLabel>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <dx:ASPxGridView ID="DiskGridView" ClientInstanceName="diskSettingsGrid" 
                                                                                            KeyFieldName="DiskName" Theme="Office2003Blue"
                                                                                            OnPreRender="DiskGridView_PreRender" 
                                                                                            onrowupdating="DiskGridView_RowUpdating" Visible="False" Width="100%" 
                                                                                            runat="server" AutoGenerateColumns="False">
                                                                                            <Columns>
                                                                                                <dx:GridViewCommandColumn Caption="Select" ShowSelectCheckbox="True" 
                                                                                                    VisibleIndex="0">
                                                                                                </dx:GridViewCommandColumn>
                                                                                                <dx:GridViewDataTextColumn Caption="Disk Name" VisibleIndex="1" Width="100px" 
                                                                                                    FieldName="DiskName">
                                                                                                </dx:GridViewDataTextColumn>
                                                                                                <dx:GridViewDataTextColumn Caption="Disk Size" VisibleIndex="2" 
                                                                                                    FieldName="DiskSize">
                                                                                                </dx:GridViewDataTextColumn>
                                                                                                <dx:GridViewDataTextColumn Caption="Disk Free Space" VisibleIndex="3" 
                                                                                                    FieldName="DiskFree">
                                                                                                </dx:GridViewDataTextColumn>
                                                                                                <dx:GridViewDataTextColumn Caption="Percent Free" VisibleIndex="4" 
                                                                                                    FieldName="PercentFree">
                                                                                                </dx:GridViewDataTextColumn>
                                                                                                <dx:GridViewDataTextColumn Caption="Free Space Threshold" VisibleIndex="5" 
                                                                                                    FieldName="Threshold">
                                                                                                    <PropertiesTextEdit DisplayFormatString="g" Width="50px">
                                                                                                        <ValidationSettings>
                                                                                                            <RegularExpression ValidationExpression="(^(100(?:\.0{1,2})?))|(?!^0*$)(?!^0*\.0*$)^\d{1,2}(\.\d{1,2})?$" />
                                                                                                        </ValidationSettings>
                                                                                                    </PropertiesTextEdit>
                                                                                                    <DataItemTemplate>
                                                                                                        <dx:ASPxTextBox ID="txtFreeSpaceThresholdValue" runat="server" Width="150px" Value='<%# Eval("Threshold") %>'>
                                                                                                        </dx:ASPxTextBox>
                                                                                                    </DataItemTemplate>
                                                                                                </dx:GridViewDataTextColumn>
                                                                                                <dx:GridViewDataTextColumn Caption="isSelected" Visible="False" 
                                                                                                    VisibleIndex="9" FieldName="isSelected">
                                                                                                </dx:GridViewDataTextColumn>
                                                                                                <dx:GridViewDataComboBoxColumn Caption="Threshold Type (% or GB)" 
                                                                                                    FieldName="ThresholdType" VisibleIndex="8">
                                                                                                    <PropertiesComboBox Width="50px">
                                                                                                    </PropertiesComboBox>
                                                                                                    <DataItemTemplate>
                                                                                                        <dx:ASPxComboBox ID="txtFreeSpaceThresholdType" runat="server" 
                                                                                                            TextField="ThresholdType" Value='<%# Eval("ThresholdType") %>' 
                                                                                                            ValueField="ThresholdType" ValueType="System.String">
                                                                                                            <Items>
                                                                                                                <dx:ListEditItem Text="Percent" Value="Percent" />
                                                                                                                <dx:ListEditItem Text="GB" Value="GB" />
                                                                                                            </Items>
                                                                                                        </dx:ASPxComboBox>
                                                                                                    </DataItemTemplate>
                                                                                                </dx:GridViewDataComboBoxColumn>
                                                                                            </Columns>
                                                                                            <SettingsPager PageSize="50" Mode="ShowAllRecords" SEOFriendly="Enabled">
                                                                                            </SettingsPager>
                                                                                            <SettingsEditing Mode="Inline">
                                                                                            </SettingsEditing>
                                                                                            <Styles>
                                                                                                <Header CssClass="GridCssHeader">
                                                                                                </Header>
                                                                                                <AlternatingRow CssClass="GridAltRow">
                                                                                                </AlternatingRow>
                                                                                                <Cell CssClass="GridCss">
                                                                                                </Cell>
                                                                                            </Styles>
                                                                                        </dx:ASPxGridView>
                                                                                        <dx:ASPxTrackBar ID="AdvDiskSpaceThTrackBar" runat="server" AutoPostBack="True" 
                                                                                            onvaluechanged="AdvDiskSpaceThTrackBar_ValueChanged" Position="10" 
                                                                                            ScalePosition="LeftOrTop" Theme="Office2010Blue" Width="100%">
                                                                                        </dx:ASPxTrackBar>
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <dx:ASPxLabel ID="GBTitle" runat="server" Text="Current threshold:" CssClass="lblsmallFont" 
                                                                                                        Visible="false">
                                                                                                    </dx:ASPxLabel> 
                                                                                                </td>
                                                                                                <td>
                                                                                                    <dx:ASPxTextBox ID="GBTextBox" runat="server" Width="170px" Visible="False">
                                                                                                    </dx:ASPxTextBox>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <dx:ASPxLabel ID="GBLabel" runat="server" Text="GB free space" CssClass="lblsmallFont" 
                                                                                                            Visible="False">
                                                                                                    </dx:ASPxLabel>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="3">
                                                                                                    &nbsp;
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                        <asp:Label ID="Label4" runat="server" ForeColor="#0033CC" CssClass="lblsmallFont" 
                                                                                                
                                                                                Text="&nbsp;&lt;b&gt;Disk Space &lt;/b&gt;alerts will trigger if any of the drives on the Skype for Business server fall below the threshold." 
                                                                                Visible="True"></asp:Label>
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
                                        </dx:ContentControl>
                                    </ContentCollection>
                                </dx:TabPage>
                                <dx:TabPage Text="Advanced">
                                    <TabImage Url="~/images/package_green.png">
                                    </TabImage>
                                    <ContentCollection>
                                        <dx:ContentControl ID="ContentControl3" runat="server">
                                            <table>
                                                <tr>
                                                    <td valign="top">
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <dx:ASPxRoundPanel ID="MemRoundPanel9" runat="server" 
                                                                    HeaderText="Memory Usage Alert" Theme="Glass" Width="400px">
                                                                    <PanelCollection>
                                                                        <dx:PanelContent ID="PanelContent2" runat="server">
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <dx:ASPxLabel ID="MemLabel" runat="server" Text="ASPxLabel" Visible="False">
                                                                                        </dx:ASPxLabel>
                                                                                        <dx:ASPxTrackBar ID="AdvMemoryThTrackBar" runat="server" 
                                                                                            OnPositionChanged="AdvMemoryThTrackBar_PositionChanged" Position="95" 
                                                                                            PositionStart="95" Step="1" Theme="Office2010Blue" Width="100%" 
                                                                                            AutoPostBack="True" EnableViewState="False" ScalePosition="LeftOrTop">
                                                                                            <ValueToolTipStyle BackColor="White">
                                                                                            </ValueToolTipStyle>
                                                                                        </dx:ASPxTrackBar>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left">
                                                                                        <asp:Label ID="Label5" runat="server" Style="color: #0033CC" Text="&lt;b&gt;Memory Utilization&lt;/b&gt; alerts will trigger if the percentage of memory in use on the server exceeeds this threshold.  &lt;br/&gt;&lt;br/&gt; If you don't want to get memory alerts, set the threshold to zero."></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </dx:PanelContent>
                                                                    </PanelCollection>
                                                                </dx:ASPxRoundPanel>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        
                                                    </td>
                                                    <td valign="top">
                                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                            <ContentTemplate>
                                                                <dx:ASPxRoundPanel ID="CPURoundPanel" runat="server" Theme="Glass" 
                                                                    Width="400px" HeaderText="CPU Utilization Alert">
                                                                    <PanelCollection>
                                                                        <dx:PanelContent ID="PanelContent3" runat="server">
                                                                            <table class="navbarTbl">
                                                                                <tr>
                                                                                    <td>
                                                                                        <dx:ASPxLabel ID="CpuLabel" runat="server" Text="ASPxLabel" Visible="False">
                                                                                        </dx:ASPxLabel>
                                                                                        <dx:ASPxTrackBar ID="AdvCPUThTrackBar" runat="server" Position="90" 
                                                                                            PositionStart="90" Step="1" Theme="Office2010Blue" Width="100%" 
                                                                                            OnValueChanged="AdvCPUThTrackBar_ValueChanged" AutoPostBack="True" 
                                                                                            EnableViewState="False" ScalePosition="LeftOrTop">
                                                                                        </dx:ASPxTrackBar>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left">
                                                                                        <asp:Label ID="Label6" runat="server" Style="color: #0033CC" Text="&lt;b&gt;CPU Utilization&lt;/b&gt; alerts will trigger if the CPU utilization rate exceeds this threshold. &lt;br/&gt;&lt;br/&gt;  If you don't want to get CPU alerts, set the threshold to zero. "></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </dx:PanelContent>
                                                                    </PanelCollection>
                                                                </dx:ASPxRoundPanel>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        
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
                        </table>
                        
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <div id="errorDiv" class="alert alert-danger" runat="server" style="display: none">Error.
                </div>
                <table>
                    <tr>
                        <td>            
                            <dx:ASPxButton ID="FormOkButton" runat="server" Text="OK" 
                                onclick="FormOkButton_Click" Theme="Office2010Blue" Width="75px">
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="FormCancelButton" runat="server" Text="Cancel" 
                                onclick="FormCancelButton_Click" Theme="Office2010Blue" Width="75px">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>