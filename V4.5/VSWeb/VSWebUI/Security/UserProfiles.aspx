<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="UserProfiles.aspx.cs" Inherits="VSWebUI.Security.UserProfiles" %>
	<%@ MasterType virtualpath="~/Site1.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx1" %>




    <%@ Register Assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">

.dxbButton_Office2010Blue
{
	color: #1e395b;
	font: normal 11px Verdana;
	vertical-align: middle;
	border: 1px solid #abbad0;
	padding: 1px;
	cursor: pointer;
}
    .dxtlHeader_Office2003Blue
{
	border: 1px solid #4f93e3;
	padding: 4px 6px 5px;
	font-weight: normal;
}
.dxtl__B2 
{
	border-top-style: none !important;
	border-right-style: none !important;
	border-left-style: none !important;
}
.dxtl__I
{
	width: 0.1%;	
}

.dxtl__I, .dxtl__IM, .dxtl__I8
{
	text-align: center;		
	font-size: 2px !important;
	line-height: 0 !important;	
}
        
        .dxtl__I
        {
            text-align: center;
            font-size: 2px !important;
            line-height: 0 !important;
        }
        .dxtl__I
        {
            width: 0.1%;
        }
        
        .dxtl__B2
        {
            border-top-style: none !important;
            border-right-style: none !important;
            border-left-style: none !important;
        }
        .dxtl__B3 
{
	border-top-style: none !important;
	border-right-style: none !important;
}
        .dxtl__B3
        {
            border-top-style: none !important;
            border-right-style: none !important;
        }
        .dxtlNode_Office2003Blue
{
	background: white none;
}
.dxtlLineFirst_Office2003Blue
{

}
.dxtlIndent_Office2003Blue
{
	padding: 0 11px;
}
.dxtlIndent_Office2003Blue,
.dxtlIndentWithButton_Office2003Blue
{
	vertical-align: top;
	background: white none no-repeat top center;
}
.dxtlSelectionCell_Office2003Blue
{
	border: 1px solid #bfd3ee;
}

.dxtlAltNode_Office2003Blue
{
	background: #edf5ff none;
}
.dxtlLineMiddle_Office2003Blue
{

}
.dxtlLineLast_Office2003Blue
{
	

.dxtl__B1 
{	
	border-top-style: none !important;
	border-right-style: none !important;
	border-bottom-style: none!important;	
}
.dxtlPagerTopPanel_Office2003Blue,
.dxtlPagerBottomPanel_Office2003Blue
{
	background: white none;
	padding-top: 1px;
}

.dxtlPagerBottomPanel_Office2003Blue
{
	border-top: 1px solid #4f93e3;
}
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Manage Server Profiles" Font-Bold="True"
                    Font-Size="Large" Style="color: #000000; font-family: Verdana"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="color: Black" id="tdmsg" runat="server" align="center">
            </td>
        </tr>
        <tr>
            <td style="color: Black" id="td1" runat="server" align="left">
                <table>
                    <tr>
                        <td><asp:Label ID="Label2" runat="server" Text="Profile Name" ></asp:Label>
                        </td>
                        <td>
                            <dx:ASPxTextBox ID="ProfileTextBox" runat="server" Width="170px">
                                <ValidationSettings ErrorText="Enter Profile Name" SetFocusOnError="True">
                                    <RequiredField IsRequired="True" />
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                        </td>
                        <td><asp:Label ID="Label3" runat="server" Text="Server Type" ></asp:Label>
                        </td>
                        <td>
                            <dx:ASPxComboBox ID="ServerTypeComboBox" runat="server" AutoPostBack=true
                                onselectedindexchanged="ServerTypeComboBox_SelectedIndexChanged" 
                                ValueType="System.String">
                                <ValidationSettings ErrorText="Select Server Type">
                                    <RequiredField IsRequired="True" />
                                </ValidationSettings>
                            </dx:ASPxComboBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <dx:ASPxGridView runat="server" KeyFieldName="ID" AutoGenerateColumns="False" ID="ProfilesGridView"
        ClientInstanceName="ProfilesGridView" Theme="Office2003Blue" 
        EnableTheming="True" Width="500px" OnPageSizeChanged="ProfilesGridView_PageSizeChanged">
        <Columns>
            <dx:GridViewCommandColumn Caption="Select" ShowInCustomizationForm="True" ShowSelectCheckbox="True"
                VisibleIndex="0" Width="20px">
                <HeaderStyle CssClass="GridCssHeader" />
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn Caption="Attribute Name" VisibleIndex="1"  FieldName="AttributeName">
               
               <HeaderStyle CssClass="GridCssHeader" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn VisibleIndex="2" Caption="Value"  FieldName="Value">
               <DataItemTemplate>
               <dx:ASPxTextBox ID="txtValue" Width="100%" runat="server" Value='<%# Eval("AttributeMeasurement")%>'></dx:ASPxTextBox>
               </DataItemTemplate>
               <HeaderStyle CssClass="GridCssHeader" />
            </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="ID" VisibleIndex="3"  FieldName="ID" Visible="false">
               <HeaderStyle CssClass="GridCssHeader" />
            </dx:GridViewDataTextColumn>

        </Columns>
        <SettingsBehavior ConfirmDelete="True"></SettingsBehavior>
        <SettingsText ConfirmDelete="Are you sure you want to delete this Profile?"></SettingsText>
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
            <Header SortingImageSpacing="5px" ImageSpacing="5px">
            </Header>
            <LoadingPanel ImageSpacing="5px">
            </LoadingPanel>
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
        <SettingsPager PageSize="10" SEOFriendly="Enabled" Visible="False">
            <PageSizeItemSettings Visible="true" />
        </SettingsPager>
    </dx:ASPxGridView>
    <br />
                                        <table>
                                            <tr >
                                                <td >
                                                    <dx:ASPxButton runat="server" Text="Save Profile" Theme="Office2010Blue" 
                                                        Width="125px" ID="OKButton" OnClick="OKButton_Click" style="height: 27px">
                                                    </dx:ASPxButton>

                                                </td>
                                                <td >
                                                    <dx:ASPxButton runat="server" CausesValidation="False" Text="Cancel" 
                                                        Theme="Office2010Blue" Width="75px" ID="CancelButton" 
                                                        OnClick="CancelButton_Click"></dx:ASPxButton>

                                                </td>
                                                <td  style="color: Black"  runat="server" align="left">
                                                    <asp:Label ID="lblMessage" runat="server" ></asp:Label>

                                                </td>
                                            </tr>
                                            <tr >
                                                <td >
                                                  

                                                </td>
                                                <td >
                                                    
                                                </td>
                                                <td >
                                                    
                                                    &nbsp;</td>
                                            </tr>
                                            <tr >
                                                <td colspan="2" >
                                                    <dx:ASPxButton runat="server" Text="Apply Profile to Selected Servers" Theme="Office2010Blue" 
                                                        Width="225px" ID="ApplyServersButton">
                                                    </dx:ASPxButton>

                                                </td>
                                                <td >
                                                    &nbsp;</td>
                                            </tr>
                                        </table>

      <table id="tblServer" runat="server">
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td style="color: Black; text-align: left;" id="td2" runat="server" 
                align="center">
                <asp:Label ID="Label4" runat="server" Text="Servers" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="color: Black" id="td3" runat="server" align="left">
                <table>
                    <tr>
                        <td>
    <dx:ASPxGridView runat="server" KeyFieldName="ID" AutoGenerateColumns="False" ID="ServersGridView"
        ClientInstanceName="ServersGridView" Theme="Office2003Blue" 
        EnableTheming="True" Width="500px" OnPageSizeChanged="ServersGridView_PageSizeChanged">
        <Columns>
            <dx:GridViewCommandColumn Caption="Select" ShowInCustomizationForm="True" ShowSelectCheckbox="True"
                VisibleIndex="0" Width="20px">
                <HeaderStyle CssClass="GridCssHeader" />
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn Caption="Server" VisibleIndex="1" FieldName=ServerName >
             
               <HeaderStyle CssClass="GridCssHeader" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn VisibleIndex="2" Caption="Description" 
                FieldName=Description  >
              
               <HeaderStyle CssClass="GridCssHeader" />
            </dx:GridViewDataTextColumn>
        </Columns>
        <SettingsBehavior ConfirmDelete="True"></SettingsBehavior>
        <Settings ShowFilterRow="True" />
        <SettingsText ConfirmDelete="Are you sure you want to delete this Profile?"></SettingsText>
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
            <Header SortingImageSpacing="5px" ImageSpacing="5px">
            </Header>
            <LoadingPanel ImageSpacing="5px">
            </LoadingPanel>
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
        <SettingsPager PageSize="10" SEOFriendly="Enabled" Visible="False">
            <PageSizeItemSettings Visible="true" />
        </SettingsPager>
    </dx:ASPxGridView>


                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>


    <br />
    <br />
                                        </asp:Content>
