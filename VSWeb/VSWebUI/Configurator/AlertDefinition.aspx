<%@ Page Title="VitalSigns Plus-AlertDefinition" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="AlertDefinition.aspx.cs" Inherits="VSWebUI.AlertDefinition" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>






<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .dxeEditAreaSys
        {
            border: 0px !important;
            padding: 0px;
        }
        
        .dxeEditAreaSys
        {
            width: 100%;
            background-position: 0 0; /*iOS Safari*/
        }
        
        .dxeBase
        {
            font: 12px Tahoma;
        }
        .dxICheckBox
        {
            margin: auto;
            display: inline-block;
            vertical-align: middle;
        }
        .dxWeb_edtCheckBoxUnchecked
        {
            background-position: -41px -99px;
        }
        
        .dxWeb_edtCheckBoxChecked, .dxWeb_edtCheckBoxUnchecked, .dxWeb_edtCheckBoxGrayed, .dxWeb_edtCheckBoxCheckedDisabled, .dxWeb_edtCheckBoxUncheckedDisabled, .dxWeb_edtCheckBoxGrayedDisabled
        {
            background-repeat: no-repeat;
            background-color: transparent;
            width: 15px;
            height: 15px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<table><tr><td valign="top">
<dx:ASPxRoundPanel ID="AlertsRoundPanel" runat="server" 
                                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                        CssPostfix="Glass" 
                                        GroupBoxCaptionOffsetY="-24px" HeaderText="Alerts" 
                                        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
                                        Width="430px" Height="250px">
                                        <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                                        <HeaderStyle Height="23px">
                                        <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
                                        </HeaderStyle>
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">

       <table>
        <tr>
            <td>

            <dx:ASPxPageControl Font-Bold="True"  ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                    CssPostfix="Glass" Height="308px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                    Width="617px" TabSpacing="0px" EnableHierarchyRecreation="False">
                    <TabPages>
                        <dx:TabPage Text="Basics">
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">
                                    <table style="width: 100%;" align="left">
                                        <tr>
                                            <td>
                                                <dx:ASPxLabel ID="ASPxLabel17" runat="server" Text="Alert Name" CssClass="lblsmallFont">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>
                                                <dx:ASPxTextBox ID="ASPxTextBox14" runat="server" CssClass="txtmed" >
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>
                                                <dx:ASPxCheckBox ID="ASPxCheckBox7" runat="server" CheckState="Unchecked" Text="Enable this Alert" CssClass="lblsmallFont">
                                                </dx:ASPxCheckBox>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dx:ASPxLabel ID="ASPxLabel18" runat="server" Text="Type" CssClass="lblsmallFont">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>
                                                <dx:ASPxComboBox ID="ASPxComboBox1" runat="server" CssClass="lblsmallFont">
                                                </dx:ASPxComboBox>
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
                                                <dx:ASPxLabel ID="ASPxLabel19" runat="server" Text="eMail Address" CssClass="lblsmallFont">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>
                                                <dx:ASPxTextBox ID="ASPxTextBox16" runat="server" CssClass="txtmed">
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>
                                                <dx:ASPxButton ID="ASPxButton1" runat="server" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
                                                    CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
                                                    Text="Select">
                                                </dx:ASPxButton>
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
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <dx:ASPxLabel ID="ASPxLabel20" runat="server" Text="Send this Alert during:" CssClass="lblsmallFont">
                                                </dx:ASPxLabel>
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
                                                <dx:ASPxCheckBox ID="ASPxCheckBox8" runat="server" CheckState="Unchecked" Text="Business Hours" CssClass="lblsmallFont">
                                                </dx:ASPxCheckBox>
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
                                                <dx:ASPxCheckBox ID="ASPxCheckBox9" runat="server" CheckState="Unchecked" Text="Off Hours" CssClass="lblsmallFont">
                                                </dx:ASPxCheckBox>
                                            </td>
                                            <td>
                                                <dx:ASPxLabel ID="ASPxLabel21" runat="server" Text="Begin using this alert at:" CssClass="lblsmallFont">
                                                </dx:ASPxLabel>
                                            </td>
                                            <td>
                                                <dx:ASPxLabel ID="ASPxLabel22" runat="server" Text="Stop using this alert at:" CssClass="lblsmallFont">
                                                </dx:ASPxLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <dx:ASPxRadioButton ID="ASPxRadioButton1" runat="server" Text="Specific Hours" CssClass="lblsmallFont">
                                                </dx:ASPxRadioButton>
                                            </td>
                                            <td>
                                                <dx:ASPxTimeEdit ID="ASPxTimeEdit1" runat="server" Width="90px" 
                                                    CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                                    CssPostfix="Office2010Blue" Spacing="0" 
                                                    SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css">
                                                </dx:ASPxTimeEdit>
                                            </td>
                                            <td>
                                                <dx:ASPxTimeEdit ID="ASPxTimeEdit2" runat="server" Width="90px" 
                                                    CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                                    CssPostfix="Office2010Blue" Spacing="0" 
                                                    SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css">
                                                </dx:ASPxTimeEdit>
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
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="left">
                                                <dx:ASPxLabel ID="ASPxLabel24" runat="server" 
                                                    Text="NotesMail alerts are sent via the primary or secondary Domino servers as defined in Domino preferences.  Information about sending text messages is in the help.  In brief the solution is to send them a NotesMail message addressed similar to:" 
                                                    Width="100%" CssClass="lblsmallFont">
                                                </dx:ASPxLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="left">
                                                <dx:ASPxLabel ID="ASPxLabel25" runat="server" 
                                                    Text="#@vtext.com (Verizon) or #@txt.att.net (ATT)" Width="100%" CssClass="lblsmallFont">
                                                </dx:ASPxLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="left">
                                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" 
                                                    Text="#@messaging.sprintpcs.com (Sprint) " 
                                                    Width="100%" CssClass="lblsmallFont">
                                                </dx:ASPxLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="4">
                                                <dx:ASPxLabel ID="ASPxLabel26" runat="server" Text="#@tmomail.net (T-Mobile)">
                                                </dx:ASPxLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="4">
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="left">
                                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="#@tmomail.net (T-Mobile)" 
                                                    Width="100%" CssClass="lblsmallFont">
                                                </dx:ASPxLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="left">
                                                <dx:ASPxLabel ID="ASPxLabel3" runat="server" 
                                                    Text="...depending on the provider of the recipient." Width="100%" CssClass="lblsmallFont">
                                                </dx:ASPxLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Applies To">
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td valign="top" rowspan="2">
                                                <dx:ASPxRoundPanel ID="ASPxRoundPanel4" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                    CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px"
                                                    HeaderText="Applies To" Height="16px" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
                                                    Width="323px">
                                                    <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
                                                    <HeaderStyle Height="23px">
                                                        <Paddings PaddingBottom="0px" PaddingLeft="2px" 
                                                        PaddingTop="0px" />
                                                    </HeaderStyle>
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxCheckBox ID="ASPxCheckBox5" runat="server" Checked="True" CheckState="Checked"
                                                                            Text="All Server Types" CssClass="lblsmallFont">
                                                                        </dx:ASPxCheckBox>
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
                                                                        <dx:ASPxCheckBox ID="ASPxCheckBox10" runat="server" CheckState="Unchecked" Text="Down Server" CssClass="lblsmallFont">
                                                                        </dx:ASPxCheckBox>
                                                                    </td>
                                                                    <td colspan="2">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3">
                                                                        <dx:ASPxCheckBox ID="ASPxCheckBox11" runat="server" CheckState="Unchecked" Text="Down Server" CssClass="lblsmallFont">
                                                                        </dx:ASPxCheckBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <dx:ASPxCheckBox ID="ASPxCheckBox12" runat="server" CheckState="Unchecked" Text="Down Server" CssClass="lblsmallFont">
                                                                        </dx:ASPxCheckBox>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <dx:ASPxCheckBox ID="ASPxCheckBox13" runat="server" CheckState="Unchecked" Text="Down Server" CssClass="lblsmallFont">
                                                                        </dx:ASPxCheckBox>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
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
                                                                    <td align="right" colspan="2">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <br />
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dx:ASPxRoundPanel>
                                            </td>
                                            <td valign="top">
                                                <dx:ASPxLabel ID="ASPxLabel23" runat="server" Text="If Checked, then only send this alert for the following servers" CssClass="lblsmallFont">
                                                </dx:ASPxLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <dx:ASPxCheckBoxList ID="ASPxCheckBoxList1" runat="server" CssClass="lblsmallFont">
                                                    <Items>
                                                        <dx:ListEditItem Text="ListEditItem" />
                                                        <dx:ListEditItem Text="ListEditItem" />
                                                        <dx:ListEditItem Text="ListEditItem" />
                                                        <dx:ListEditItem Text="ListEditItem" />
                                                        <dx:ListEditItem Text="ListEditItem" />
                                                        <dx:ListEditItem Text="ListEditItem" />
                                                        <dx:ListEditItem Text="ListEditItem" />
                                                        <dx:ListEditItem Text="ListEditItem" />
                                                        <dx:ListEditItem Text="ListEditItem" />
                                                        <dx:ListEditItem Text="ListEditItem" />
                                                    </Items>
                                                </dx:ASPxCheckBoxList>
                                            </td>
                                        </tr>
                                        <tr>
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
                                        </tr>
                                    </table>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                    </TabPages>
                    <LoadingPanelImage Url="~/App_Themes/Glass/Web/Loading.gif">
                    </LoadingPanelImage>
                    <Paddings PaddingLeft="0px" />
                    <ContentStyle>
                        <Border BorderColor="#4986A2"></Border>
                        <Border BorderColor="#9DA0AA" BorderStyle="Solid" BorderWidth="1px" />
                    </ContentStyle>
                </dx:ASPxPageControl>
            </td>
        </tr>
        <tr>            
                
                <td align="left">
                    <table>
                        <tr>
                            <td>
                                <dx:ASPxButton ID="FormOkButton" runat="server" Text="Ok" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
                                    CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
                                    Width="75px" >
                                </dx:ASPxButton>
                            </td>
                            <td>
                                <dx:ASPxButton ID="FormCancelButton" runat="server" Text="Cancel" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
                                    CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
                                    Width="75px">
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </td>
               
        </tr>
    </table>
     </dx:PanelContent>
                                        </PanelCollection>
                                    </dx:ASPxRoundPanel>


    </td></tr></table>
</asp:Content>
