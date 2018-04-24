<%@ Page Title="VitalSigns Plus - Assign Navigator Access" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="AssignNavigator.aspx.cs" Inherits="VSWebUI.Security.AssignNavigator" %>
<%@ MasterType virtualpath="~/Site1.Master" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>







<%@ Register assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dx" %>





<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table>
    <tr>
        <td colspan="2">
            <div class="header" id="servernamelbldisp" runat="server">Assign Navigator Access</div>
        </td>
    </tr>
    <tr>
        <td>
        
            <table>
                <tr>
        <td>
        
            <dx:ASPxRoundPanel ID="NavigatorRoundPanel" runat="server" HeaderText="Navigator" 
                Theme="Glass" Width="200px">
                <PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
    <table class="tableWidth100Percent">
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table class="tableWidth100Percent">
                            <tr>
                                <td colspan="3">
                                    <table>
                                        <tr>
                                            <td>
                                    <dx:ASPxLabel ID="ASPxLabel18" runat="server" CssClass="lblsmallFont" 
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
                                            <RequiredField ErrorText="Please select user name." IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    </td>
                                <td>
                                    </td>
                                <td>
                                    </td>
                            </tr>
                            <tr>
                                <td>
                                    <dx:ASPxLabel ID="NavigatorAllLabel" runat="server" CssClass="lblsmallFont" 
                                        CssFilePath="~/css/vswebforms.css" EnableDefaultAppearance="False" 
                                        Text="All navigator items:">
                                    </dx:ASPxLabel>
                                </td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <dx:ASPxLabel ID="NavigatorNotVisibleLabel" runat="server" 
                                        CssClass="lblsmallFont" CssFilePath="~/css/vswebforms.css" 
                                        EnableDefaultAppearance="False" 
                                        Text="Navigator items NOT visible to this user:" Wrap="False">
                                    </dx:ASPxLabel>
                                </td>

                            </tr>
             <%--5/2/2016 sowmya Added for VSPLUS 2573--%>
                            <tr>
                            <td colspan ="3">
                           <table>
								<tr>
									<td align="left">
										<dx:ASPxButton ID="CollapseButton" runat="server" OnClick="CollapseButton_Click"
											Text="Collapse All Rows" CssClass="sysButton" CausesValidation="false">
											<Image Url="~/images/icons/forbidden.png">
											</Image>
										</dx:ASPxButton>
									</td>
									
									<td align="left">
										<dx:ASPxButton ID="ExpandButton" runat="server"  Style="padding-right: 5px"
											Text="Expand All Rows" Theme="Office2010Blue" Visible="False" CausesValidation="false">
											<Image Url="~/images/icons/forbidden.png">
											</Image>
										</dx:ASPxButton>
									</td>
								</tr>
							</table>
                           </td>
                            </tr>
                            <tr>

                                <td rowspan="2" valign="top">
                                    <dx:ASPxTreeList ID="NavigatorVisibleTreeList" runat="server" 
                                        AutoGenerateColumns="False" CssClass="lblsmallFont" 
                                        CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                        CssPostfix="Office2010Blue" KeyFieldName="ID" 
                                        OnDataBound="NavigatorVisibleTreeList_DataBound" ParentFieldName="ParentID" 
                                        Theme="Office2003Blue">
                                        <Columns>
                                            <dx:TreeListTextColumn Caption="Menu Item" FieldName="DisplayText" 
                                                HeaderStyle-CssClass="lblsmallFont" ShowInCustomizationForm="True" 
                                                VisibleIndex="0">
                                                <HeaderStyle CssClass="lblsmallFont" />
                                            </dx:TreeListTextColumn>
                                            <dx:TreeListTextColumn FieldName="OrderNum" ShowInCustomizationForm="True" 
                                                Visible="False" VisibleIndex="1">
                                            </dx:TreeListTextColumn>
                                            <dx:TreeListTextColumn FieldName="PageLink" ShowInCustomizationForm="True" 
                                                Visible="False" VisibleIndex="2">
                                            </dx:TreeListTextColumn>
                                        </Columns>
                                        <Settings GridLines="Both" />
                                        <SettingsBehavior AllowDragDrop="False" AutoExpandAllNodes="True" />
                                        <settingsselection enabled="True" />
                                        <styles cssfilepath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                            csspostfix="Office2010Blue">
                                            <Header CssClass="GridCssHeader">
                                            </Header>
                                            <Cell CssClass="GridCss">
                                            </Cell>
                                            <loadingpanel imagespacing="5px">
                                            </loadingpanel>
                                            <AlternatingNode CssClass="GridAltRow" Enabled="True">
                                            </AlternatingNode>
                                        </styles>
                                        <stylespager>
                                            <pagenumber forecolor="#3E4846">
                                            </pagenumber>
                                            <summary forecolor="#1E395B">
                                            </summary>
                                        </stylespager>
                                        <styleseditors buttoneditcellspacing="0">
                                        </styleseditors>
                                    </dx:ASPxTreeList>
                                </td>
                                <td valign="top">
                                    <dx:ASPxButton ID="NavigatorMoveVisibleButton" runat="server" CssClass="sysButton"
                                        OnClick="NavigatorMoveVisibleButton_Click" 
                                        Text="&lt; -- Move" Wrap="False">
                                    </dx:ASPxButton>
                                    <br /><br />
                                    <dx:ASPxButton ID="NavigatorMoveNotVisibleButton" runat="server" CssClass="sysButton"
                                        OnClick="NavigatorMoveNotVisibleButton_Click" 
                                        Text="Move -- &gt;" Wrap="False">
                                    </dx:ASPxButton>
                                </td>
                                <td rowspan="2" valign="top">
                                    <dx:ASPxTreeList ID="NavigatorNotVisibleTreeList" runat="server" 
                                        AutoGenerateColumns="False" CssClass="lblsmallFont" 
                                        CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                        CssPostfix="Office2010Blue" KeyFieldName="ID" ParentFieldName="ParentID" 
                                        Theme="Office2003Blue">
                                        <Columns>
                                            <dx:TreeListTextColumn Caption="Menu Item" FieldName="DisplayText" 
                                                HeaderStyle-CssClass="lblsmallFont" ShowInCustomizationForm="True" 
                                                VisibleIndex="0">
                                                <HeaderStyle CssClass="lblsmallFont" />
                                            </dx:TreeListTextColumn>
                                            <dx:TreeListTextColumn FieldName="OrderNum" ShowInCustomizationForm="True" 
                                                Visible="False" VisibleIndex="1">
                                            </dx:TreeListTextColumn>
                                            <dx:TreeListTextColumn FieldName="PageLink" ShowInCustomizationForm="True" 
                                                Visible="False" VisibleIndex="2">
                                            </dx:TreeListTextColumn>
                                        </Columns>
                                        <settings suppressoutergridlines="True" GridLines="Both" />
                                        <settingsselection enabled="True" />
                                        <styles cssfilepath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                            csspostfix="Office2010Blue">
                                            <Header CssClass="GridCssHeader">
                                            </Header>
                                            <Cell CssClass="GridCss">
                                            </Cell>
                                            <loadingpanel imagespacing="5px">
                                            </loadingpanel>
                                            <AlternatingNode CssClass="GridAltRow" Enabled="True">
                                            </AlternatingNode>
                                        </styles>
                                        <stylespager>
                                            <pagenumber forecolor="#3E4846">
                                            </pagenumber>
                                            <summary forecolor="#1E395B">
                                            </summary>
                                        </stylespager>
                                        <styleseditors buttoneditcellspacing="0">
                                        </styleseditors>
                                    </dx:ASPxTreeList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="UserNameComboBox" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
                    </dx:PanelContent>
</PanelCollection>
            </dx:ASPxRoundPanel>
                                        
        </td>
    </tr>
    <tr>
        <td>
            <dx:ASPxPopupControl ID="NavigatorPopupControl" runat="server" 
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
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxButton ID="OKButton" runat="server" OnClick="OKButton_Click" Text="OK" 
                                                                Theme="Office2010Blue">
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
    <tr>
        <td>
        <table>
            <tr>
                <td>
                <dx:ASPxButton ID="AssignServerAccessButton" runat="server" CssClass="sysButton"
                                        OnClick="AssignServerAccessButton_Click"                                       
                                        Text="Assign">
                            </dx:ASPxButton>
                </td>
                <td>
                    <dx:ASPxButton ID="ResetServerAccessButton" runat="server" CssClass="sysButton"
                                        Text="Reset" OnClick="ResetServerAccessButton_Click" CausesValidation="false">
                            </dx:ASPxButton>
                </td>
				 <td>
                                        <dx:ASPxButton ID="CancelButton" runat="server" CssClass="sysButton"
                                        OnClick="CancelButton_Click"
                                            Text="Cancel" CausesValidation="False">
                                        </dx:ASPxButton>
                                    </td>
            </tr>
        </table>
        </td>
    </tr>
</table>
    </asp:Content>
