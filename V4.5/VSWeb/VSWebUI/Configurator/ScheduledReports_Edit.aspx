<%@ Page Title="VitalSigns Plus - Scheduled Reports" MasterPageFile="~/Site1.Master" Language="C#" AutoEventWireup="true" CodeBehind="ScheduledReports_Edit.aspx.cs" Inherits="VSWebUI.Configurator.ScheduledReports_Edit" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
<link href="../css/bootstrap1.min.css" rel="stylesheet" />  
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
<script src="../js/bootstrap.min.js" type="text/javascript"></script>
<script type="text/javascript">
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
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
        <tr>
            <td>
                <div class="header" id="servernamelbldisp" runat="server">Scheduled Report</div>
                <div class="input-prepend">&nbsp;</div>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td>
                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" ShowCollapseButton="true"
                    Width="100%" Theme="Glass" HeaderText="Scheduled Report Definition">
                    <PanelCollection>
<dx:PanelContent ID="PanelContent1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" 
                                                        Text="Report Title:" Wrap="False">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td>
                                                    <dx:ASPxComboBox ID="RptListComboBox" runat="server" AutoPostBack="True" 
                                                        ClientInstanceName="cRptListComboBox" TextField="Name">
                                                    </dx:ASPxComboBox>
                                                </td>
                                                <td>
                                                    &nbsp;</td>
                                                <td colspan="2">
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
                                        <tr>
                                            <td colspan="9">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <dx:ASPxLabel ID="RptSubjectLabel" runat="server" CssClass="lblsmallFont" 
                                                    Text="Report Subject:" Wrap="False" 
                                                    ClientInstanceName="crptSubjectLbl">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td valign="top">
                                                <dx:ASPxTextBox ID="RptSubjectTextBox" runat="server" Width="170px" 
                                                    ClientInstanceName="crptSubjectTextBox">
                                                    <ValidationSettings CausesValidation="true" ErrorDisplayMode="ImageWithTooltip" 
                                                                    ErrorText="Report subject is a required field.">
                                                        <RequiredField ErrorText="Report subject is a required field." 
                                                                  IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>
                                                &nbsp;</td>
                                            <td rowspan="4" valign="top">
                                                <dx:ASPxLabel ID="RptFrequencyLabel" runat="server" 
                                                    ClientInstanceName="crptFrequencyLabel" CssClass="lblsmallFont" 
                                                    Text="Report Frequency:" Wrap="False">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td rowspan="4" valign="top">
                                                <dx:ASPxRadioButtonList ID="RptFrequencyRadioButtonList" runat="server" 
                                                    ClientInstanceName="crptFrequencyRadio" 
                                                    onselectedindexchanged="RptFrequencyRadioButtonList_SelectedIndexChanged" 
                                                    SelectedIndex="0">
                                                    <Items>
                                                        <dx:ListEditItem Selected="True" Text="Daily" Value="0" />
                                                        <dx:ListEditItem Text="Weekly" Value="1" />
                                                        <dx:ListEditItem Text="Monthly" Value="2" />
                                                    </Items>
                                                    <ClientSideEvents SelectedIndexChanged="function(s,e){OnSelectedIndexChanged(s,e)}" />
                                                </dx:ASPxRadioButtonList>
                                            </td>
                                            <td rowspan="4" valign="top">
                                                <dx:ASPxCheckBoxList ID="RptFrequencyCheckBoxList" runat="server" 
                                                    ClientInstanceName="crptFrequencyCheckBox">
                                                    <Items>
                                                        <dx:ListEditItem Text="Monday" Value="1" />
                                                        <dx:ListEditItem Text="Tuesday" Value="2" />
                                                        <dx:ListEditItem Text="Wednesday" Value="3" />
                                                        <dx:ListEditItem Text="Thursday" Value="4" />
                                                        <dx:ListEditItem Text="Friday" Value="5" />
                                                        <dx:ListEditItem Text="Saturday" Value="6" />
                                                        <dx:ListEditItem Text="Sunday" Value="7" />
                                                    </Items>
                                                </dx:ASPxCheckBoxList>
                                                <dx:ASPxLabel ID="RptDayLabel" runat="server" 
                                                    ClientInstanceName="crptFrequencyDayLabel" CssClass="lblsmallFont" 
                                                    Height="16px" Text="Day of the Month:">
                                                </dx:ASPxLabel><br />
                                                <dx:ASPxTextBox ID="RptDayTextBox" runat="server" 
                                                    ClientInstanceName="crptFrequencyDayTextBox" Width="110px" Text="1">
                                                    <MaskSettings Mask="&lt;1..28&gt;" />
                                                    <ValidationSettings ErrorDisplayMode="None" 
                                                                    ErrorText="Day of the month is required.">
                                                       <RegularExpression ErrorText="Please enter a numeric value using the numbers 0-9 only." 
                                                                    ValidationExpression="^\d+$" />
                                                     </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>
                                                &nbsp;</td>
                                            <td valign="top">
                                                <dx:ASPxLabel ID="RptFormatLabel" runat="server" CssClass="lblsmallFont" 
                                                    Text="File Format:" Wrap="False" 
                                                    ClientInstanceName="crptFormatLabel">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td valign="top">
                                                <dx:ASPxComboBox ID="RptFileFormatComboBox" runat="server" SelectedIndex="0" 
                                                    ClientInstanceName="crptFormatComboBox">
                                                    <Items>
                                                        <dx:ListEditItem Selected="True" Text="Pdf" Value="0" />
                                                        <dx:ListEditItem Text="Xls" Value="1" />
                                                        <dx:ListEditItem Text="Xlsx" Value="2" />
                                                        <dx:ListEditItem Text="Csv" Value="3" />
                                                    </Items>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                            <tr>
                                                <td rowspan="3" valign="top">
                                                    <dx:ASPxLabel ID="RptBodyLabel" runat="server" 
                                                        ClientInstanceName="crptBodyLabel" CssClass="lblsmallFont" Text="Report Body:" 
                                                        Wrap="False">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td rowspan="3" valign="top">
                                                    <dx:ASPxMemo ID="RptBodyMemo" runat="server" ClientInstanceName="crptBodyMemo" 
                                                        Height="100px" Width="170px">
                                                    </dx:ASPxMemo>
                                                </td>
                                                <td>
                                                    &nbsp;</td>
                                                <td>
                                                    &nbsp;</td>
                                                <td valign="top">
                                                    <dx:ASPxLabel ID="RptSendToLabel" runat="server" 
                                                        ClientInstanceName="crptSendToLabel" CssClass="lblsmallFont" Text="Send To:" 
                                                        Wrap="False">
                                                    </dx:ASPxLabel>
                                                </td>
                                                <td valign="top">
                                                    <dx:ASPxTextBox ID="RptSendToTextBox" runat="server" 
                                                        ClientInstanceName="crptSendToTextBox" Width="170px">
                                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                        <RegularExpression ErrorText="This is not a valid email address." 
                                                             ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                            <RequiredField ErrorText="Send to is a required field." 
                                                                IsRequired="True" />
                                                    </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td valign="top">
                                                <dx:ASPxLabel ID="RptCopyToLabel" runat="server" 
                                                    ClientInstanceName="crptCopyToLabel" CssClass="lblsmallFont" Text="Copy To:" 
                                                    Wrap="False">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td valign="top">
                                                <dx:ASPxTextBox ID="RptCopyToTextBox" runat="server" 
                                                    ClientInstanceName="crptCopyToTextBox" Width="170px">
                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                        <RegularExpression ErrorText="This is not a valid email address." 
                                                             ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td valign="top">
                                                <dx:ASPxLabel ID="RptBlindCopyToLabel" runat="server" 
                                                    ClientInstanceName="crptBlindCopyToLabel" CssClass="lblsmallFont" Height="16px" 
                                                    Text="Blind Copy To:" Wrap="False">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td valign="top">
                                                <dx:ASPxTextBox ID="RptBlindCopyToTextBox" runat="server" 
                                                    ClientInstanceName="crptBlindCopyToTextBox" Width="170px">
                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip">
                                                        <RegularExpression ErrorText="This is not a valid email address." 
                                                             ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
        <tr>
            <td>
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
            <td>
                <table>
                <tr>
            <td colspan="2">
                <div id="errorDiv" class="alert alert-danger" runat="server" style="display: none">Report scheduling has failed.
                </div>                        
            </td>
        </tr>
        
                <tr>
                    <td>
                <dx:ASPxButton ID="RptApplyButton" ClientInstanceName="cRptApplyButton" runat="server" Text="OK" 
                    CssClass="sysButton" onclick="RptApplyButton_Click">
                </dx:ASPxButton>                    
            </td>
            <td>
                <dx:ASPxButton ID="CancelButton" runat="server" CausesValidation="False" OnClick="CancelButton_Click"
			        Text="Cancel" CssClass="sysButton">
			    </dx:ASPxButton>
            </td>
                </tr>
            </table>
            </td>
        </tr>
    </table>
</asp:Content>
