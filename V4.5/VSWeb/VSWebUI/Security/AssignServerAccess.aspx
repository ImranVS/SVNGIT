<%@ Page Title="VitalSigns Plus - Assign Server Access" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="AssignServerAccess.aspx.cs" Inherits="VSWebUI.Security.AssignServerAccess" %>
<%@ MasterType virtualpath="~/Site1.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>











<%@ Register assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
    <script type="text/javascript">
    	  	$(document).ready(function () {
    	  		$('.alert-success').delay(10000).fadeOut("slow", function () {
    	  		});
    	  	});
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%">
       <tr>
            <td>
                <div class="header" id="servernamelbldisp" runat="server">Assign Server Access</div>
            </td>
        </tr>
		<tr>
		<td>
		<div  id="Sucessdiv" class="alert alert-success" runat="server" style="display: none">Success.
                            </div>
		</td>
		</tr>
        <tr>
            <td>
                <dx:ASPxGridView ID="ServerAccessGridView" runat="server" 
                    AutoGenerateColumns="False" EnableTheming="True" KeyFieldName="ID" 
                    Theme="Office2003Blue" 
                    onhtmlrowcreated="ServerAccessGridView_HtmlRowCreated" 
                    onpagesizechanged="ServerAccessGridView_PageSizeChanged" Width="100%">
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" Visible="False" 
                            VisibleIndex="0">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="User Name" FieldName="FullName" 
                            VisibleIndex="2">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" VisibleIndex="1" 
                            Width="60px">
                            <EditButton Visible="True">
                                <Image Url="../images/edit.png">
                                </Image>
                            </EditButton>
                            <HeaderStyle CssClass="GridCssHeader1" />
                            <CellStyle CssClass="GridCss1">
                            </CellStyle>
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn Caption="Is Super Admin?" FieldName="SuperAdmin" 
                            VisibleIndex="3">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Restrictions by Server/Location" 
                            FieldName="Restrictions" Name="Restrictions" VisibleIndex="4">
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
                        <Cell CssClass="GridCss">
                        </Cell>
                    </Styles>
                </dx:ASPxGridView>
            </td>
        </tr>
        <tr>
            <td>
                <div id="successDiv" runat="server" class="alert alert-success" style="display: none">Server Access was successully updated.
                        <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="99%" 
                    Theme="Glass" HeaderText="User/Locations/Servers" Visible="False">
                    <PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
    <table>
        <tr>
            <td width="110">
                <dx:ASPxLabel ID="ASPxLabel6" runat="server" CssClass="lblsmallFont" 
                    EnableDefaultAppearance="False" Text="User Name:">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxComboBox ID="UserNameComboBox" runat="server" AutoPostBack="True" 
                    CssClass="lblsmallFont" 
                    OnSelectedIndexChanged="UserNameComboBox_SelectedIndexChanged" 
                    TextField="FullName" ValueField="FullName">
                    <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                        SetFocusOnError="True">
                        <RequiredField ErrorText="Select User Name" IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxComboBox>
            </td>
        </tr>
    </table>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" OnUnload="UpdatePanel_Unload">
                                                    <ContentTemplate>
    <dx:ASPxRadioButtonList ID="LocRadioButtonList" runat="server" AutoPostBack="True"
                                                    CssFilePath="~/css/vswebforms.css" 
                                                    SelectedIndex="0" 
                                                    OnSelectedIndexChanged="LocRadioButtonList_SelectedIndexChanged" 
                                                    CssClass="lblsmallFont" EnableTheming="True">
                                                    <Items>
                                                        <dx:ListEditItem Selected="True" 
                                                            Text="Allow user to administer servers in any Location" Value="0" />
                                                        <dx:ListEditItem Text="Limit user to servers in the following Locations:" 
                                                            Value="1" />
                                                    </Items>
                                                </dx:ASPxRadioButtonList>
                                                <br />
                                                
                                                        <table width="50%">
                                                    <tr>
                                                        <td valign="top" align="center" width="40%">
                                                            <dx:ASPxLabel ID="LocVisibleLabel" runat="server" Text="Visible to this user" 
                                                                CssClass="lblsmallFont" CssFilePath="~/css/vswebforms.css" 
                                                                EnableDefaultAppearance="False">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                        <td align="center">
                                                            &nbsp;</td>
                                                        <td valign="top" align="center" width="40%">
                                                            <dx:ASPxLabel ID="LocNotVisibleLabel" runat="server" 
                                                                Text="NOT Visible to this user" CssClass="lblsmallFont" 
                                                                CssFilePath="~/css/vswebforms.css" EnableDefaultAppearance="False">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" valign="top" width="40%">
                                                            <dx:ASPxListBox ID="LocVisibleListBox" runat="server" 
                                                                OnLoad="LocVisibleListBox_Load" SelectionMode="Multiple" TextField="Location" 
                                                                ValueField="Location" ValueType="System.String" CssClass="lblsmallFont">
                                                            </dx:ASPxListBox>
                                                        </td>
                                                        <td align="center" valign="top">
                                                            <dx:ASPxButton ID="LocMoveVisibleButton" runat="server" CssClass="sysButton"
                                                                OnClick="LocMoveVisibleButton_Click" 
                                                                Text="&lt; -- Move" Wrap="False">
                                                            </dx:ASPxButton>
                                                            <br /><br />
                                                            <dx:ASPxButton ID="LocMoveNotVisibleButton" runat="server" CssClass="sysButton"
                                                                OnClick="LocMoveNotVisibleButton_Click" 
                                                                Text="Move -- &gt;" Wrap="False">
                                                            </dx:ASPxButton>
                                                        </td>
                                                        <td align="center" valign="top" width="40%">
                                                            <dx:ASPxListBox ID="LocNotVisibleListBox" runat="server" CssClass="lblsmallFont"
                                                                SelectionMode="Multiple" ValueField="Location" TextField="Location"
                                                                ValueType="System.String">
                                                            </dx:ASPxListBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                    
<dx:ASPxLabel ID="ASPxLabel11" runat="server" 
                                                    
                                                    Text="In addition to the restrictions above, also hide the following SPECIFIC servers:" 
                                                    CssClass="lblsmallFont" CssFilePath="~/css/vswebforms.css" 
                                                    EnableDefaultAppearance="False">
                                                </dx:ASPxLabel>
                                                <br />
                                                <br />
                                                <table>
                                                    <tr>
                                                        <td align="center" valign="top">
                                                            <dx:ASPxLabel ID="ASPxLabel16" runat="server" CssClass="lblsmallFont" 
                                                                CssFilePath="~/css/vswebforms.css" EnableDefaultAppearance="False" 
                                                                Text="Visible to this user">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                        <td align="center">
                                                            &nbsp;</td>
                                                        <td align="center" valign="top">
                                                            <dx:ASPxLabel ID="ASPxLabel17" runat="server" CssClass="lblsmallFont" 
                                                                CssFilePath="~/css/vswebforms.css" EnableDefaultAppearance="False" 
                                                                Text="NOT Visible to this user">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" valign="top">
                                                            <dx:ASPxGridView ID="SpecificServersVisibleGridView" runat="server" 
                                                                AutoGenerateColumns="False" 
                                                                CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" OnPageSizeChanged="SpecificServersVisibleGridView_PageSizeChanged" 
                                                                CssPostfix="Office2010Silver" KeyFieldName="ID" Theme="Office2003Blue" 
                                                                Width="100%">
                                                             <Columns>
                                                                    <dx:GridViewCommandColumn Caption="Select" ShowInCustomizationForm="True" 
                                                                        ShowSelectCheckbox="True" VisibleIndex="0">
                                                                        <HeaderStyle CssClass="GridCssHeader" />

                                                                   </dx:GridViewCommandColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Server Name" FieldName="ServerName" 
                                                                        ShowInCustomizationForm="True" VisibleIndex="1">
                                                                        <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>


                                                                        <Settings AllowDragDrop="False" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Location" FieldName="Location" 
                                                                        ShowInCustomizationForm="True" VisibleIndex="2">
                                                                        <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>


                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Server Type" FieldName="ServerType" 
                                                                        ShowInCustomizationForm="True" VisibleIndex="3">
                                                                        <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>


                                                                    </dx:GridViewDataTextColumn>
                                                                </Columns>
                                                                <SettingsPager AlwaysShowPager="True">
                                                                    <PageSizeItemSettings Visible="True">
                                                                    </PageSizeItemSettings>
                                                                </SettingsPager>
                                                                <Settings ShowGroupPanel="True" />
                                                                <Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                                                                    CssPostfix="Office2010Silver">
                                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                    </Header>
                                                                    <LoadingPanel ImageSpacing="5px">
                                                                    </LoadingPanel>
                                                                    <AlternatingRow CssClass="GridAltRow" Enabled="True"></AlternatingRow>
                                                                </Styles>
                                                                <StylesEditors ButtonEditCellSpacing="0">
                                                                    <ProgressBar Height="21px">
                                                                    </ProgressBar>
                                                                </StylesEditors>
                                                            </dx:ASPxGridView>
                                                        </td>
                                                        <td align="center" valign="top">
                                                            <dx:ASPxButton ID="SpecificServersMoveVisibleButton" runat="server" CssClass="sysButton"
                                                                OnClick="SpecificServersMoveVisibleButton_Click" 
                                                                Text="&lt; -- Move" Wrap="False">
                                                            </dx:ASPxButton>
                                                            <br /><br />
                                                            <dx:ASPxButton ID="SpecificServersMoveNotVisibleButton" runat="server" CssClass="sysButton"
                                                                OnClick="SpecificServersMoveNotVisibleButton_Click" 
                                                                Text="Move -- &gt;" Wrap="False">
                                                            </dx:ASPxButton>
                                                        </td>
                                                        <td align="center" valign="top">
                                                            <dx:ASPxGridView ID="SpecificServersNotVisibleGridView" runat="server" 
                                                                AutoGenerateColumns="False" 
                                                                CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" OnPageSizeChanged="SpecificServersNotVisibleGridView_PageSizeChanged"
                                                                CssPostfix="Office2010Silver" KeyFieldName="ID" Theme="Office2003Blue" 
                                                                Width="100%">
                                                                <Columns>
                                                                    <dx:GridViewCommandColumn Caption="Select" ShowInCustomizationForm="True" 
                                                                        ShowSelectCheckbox="True" VisibleIndex="0">
                                                                         <HeaderStyle CssClass="GridCssHeader" />
                                                                    </dx:GridViewCommandColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Server Name" FieldName="ServerName" 
                                                                        ShowInCustomizationForm="True" VisibleIndex="1">
                                                                        <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>


                                                                        <Settings AllowDragDrop="False" />
                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Location" FieldName="Location" 
                                                                        ShowInCustomizationForm="True" VisibleIndex="2">
                                                                        <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>


                                                                    </dx:GridViewDataTextColumn>
                                                                    <dx:GridViewDataTextColumn Caption="Server Type" FieldName="ServerType" 
                                                                        ShowInCustomizationForm="True" VisibleIndex="3">
                                                                        <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>


                                                                    </dx:GridViewDataTextColumn>
                                                                </Columns>
                                                                <SettingsPager AlwaysShowPager="True">
                                                                    <PageSizeItemSettings Visible="True">
                                                                    </PageSizeItemSettings>
                                                                </SettingsPager>
                                                                <Settings ShowGroupPanel="True" />
                                                                <Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                                                                    CssPostfix="Office2010Silver">
                                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                    </Header>
                                                                    <LoadingPanel ImageSpacing="5px">
                                                                    </LoadingPanel>
                                                                    <AlternatingRow CssClass="GridAltRow" Enabled="True"></AlternatingRow>
                                                                </Styles>
                                                                <StylesEditors ButtonEditCellSpacing="0">
                                                                    <ProgressBar Height="21px">
                                                                    </ProgressBar>
                                                                </StylesEditors>
                                                            </dx:ASPxGridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <br />
                                                </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="LocRadioButtonList" />
                                                        <asp:AsyncPostBackTrigger ControlID="UserNameComboBox" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                        </dx:PanelContent>
</PanelCollection>
            </dx:ASPxRoundPanel>
            </td>
        </tr>
        <tr>
            <td>
                                    <dx:ASPxPopupControl ID="ServerAccessPopupControl" runat="server" 
                                        HeaderText="Information" Modal="True" PopupHorizontalAlign="WindowCenter" 
                                        PopupVerticalAlign="WindowCenter" Theme="Glass">
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
                <dx:ASPxButton ID="OKButton" runat="server" OnClick="OKButton_Click" Text="OK" 
                    Theme="Office2010Blue" Width="60px">
                </dx:ASPxButton>
            </td>
        </tr>
    </table>
                                            </dx:PopupControlContentControl>
</ContentCollection>
                                    </dx:ASPxPopupControl>
                                </td>
        </tr>
        <tr>
            <td>        
                        <table>
                            <tr>
                                <td>
                                    <div id="errorDiv" class="alert alert-danger" runat="server" style="display: none">Error.
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <dx:ASPxButton ID="AssignServerAccessButton" runat="server" CssClass="sysButton"
                                        OnClick="AssignServerAccessButton_Click" 
                                        Text="Assign" Visible="False">
                                    </dx:ASPxButton>
                                </td>
                                <td>
                                    <dx:ASPxButton ID="ResetServerAccessButton" runat="server" CssClass="sysButton" 
                                        Text="Reset" OnClick="ResetServerAccessButton_Click" Visible="False">
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
            </td>
        </tr>
    </table>

</asp:Content>
