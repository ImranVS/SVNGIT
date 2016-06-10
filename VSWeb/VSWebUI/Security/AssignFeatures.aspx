<%@ Page Title="VitalSigns Plus - Assign Features" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="AssignFeatures.aspx.cs" Inherits="VSWebUI.Security.AssignFeatures" %>
<%@ MasterType virtualpath="~/Site1.Master" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<%@ Register assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dx" %>




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
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
    <tr>
        <td>
            <div class="header" id="servernamelbldisp" runat="server">Assign Features</div>
        </td>
    </tr>
    <tr>
        <td valign="top">
            <div id="successDiv" runat="server" class="alert alert-success" style="display: none">
                Selected features were successfully assigned.
                <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
            </div>
        </td>
    </tr>
    <tr>
        <td>
        
            <table>
                <tr>
        <td>
        
            <dx:ASPxRoundPanel ID="NavigatorRoundPanel" runat="server" HeaderText="Features" 
                Theme="Glass" Width="200px">
                <PanelCollection>
<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
    <table class="tableWidth100Percent">
        <tr>
            <td>
                
                        <table class="tableWidth100Percent">
                            <tr>
                                <td valign="top">
                                    <dx:ASPxTreeList ID="ConfiguratorMenus" runat="server" 
                                        AutoGenerateColumns="False" CssClass="lblsmallFont" 
                                        CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                        CssPostfix="Office2010Blue" KeyFieldName="ID" 
                                      ParentFieldName="ParentID" 
                                        Theme="Office2003Blue">
                                        <Columns>
                                            <dx:TreeListTextColumn FieldName="Name" 
                                                HeaderStyle-CssClass="lblsmallFont" ShowInCustomizationForm="True" 
                                                VisibleIndex="0">
                                                <HeaderStyle CssClass="lblsmallFont" />
                                            </dx:TreeListTextColumn>
                                        </Columns>
                                        <Settings GridLines="Both" />
                                        <SettingsBehavior AllowDragDrop="False" AutoExpandAllNodes="False" />
                                        <settingsselection enabled="True" Recursive="True" />
                                        <styles cssfilepath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                            csspostfix="Office2010Blue">
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
                        </table>
                    
            </td>
        </tr>
    </table>
                    </dx:PanelContent>
</PanelCollection>
            </dx:ASPxRoundPanel>
                                        
        </td>
    </tr>
            </table>
                            
        </td>
        
    </tr>
    <tr>
        <td valign="top">
        <table>
            <tr>
                <td valign="top" colspan="2">
                     <div id="errorDiv" class="alert alert-danger" runat="server" style="display: none">
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                <dx:ASPxButton ID="AssignMenuButton" runat="server" Text="Assign" CssClass="sysButton"
                        onclick="AssignMenuButton_Click">
                            </dx:ASPxButton>
                </td>
                <td>
                    <dx:ASPxButton ID="ResetMenuButton" runat="server" Text="Reset" CssClass="sysButton" OnClick="ResetMenuButton_Click" >
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
                                                            <dx:ASPxButton ID="OKButton" runat="server"  Text="OK" 
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
    </asp:Content>

