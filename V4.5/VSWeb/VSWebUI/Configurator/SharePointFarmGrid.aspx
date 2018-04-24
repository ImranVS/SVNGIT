<%@ Page Title="VitalSigns Plus - SharePoint Farms" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="SharePointFarmGrid.aspx.cs" Inherits="VSWebUI.Configurator.SharePointFarmGrid" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />

    <style type="text/css">
        .dxrpControl_Office2003Blue .dxrpTE, .dxrpControl_Office2003Blue .dxrpNHTE, .dxrpControlGB_Office2003Blue .dxrpNHTE
        {
            border-top: 1px solid #002D96;
        }
        .dxrpControl_Office2003Blue .dxrpTE
        {
            background-image: none;
            background-color: #bbd4f5;
        }
        .dxrpControl_Office2003Blue .dxrpHeader_Office2003Blue, .dxrpControl_Office2003Blue .dxrpHLE, .dxrpControl_Office2003Blue .dxrpHeader_Office2003Blue, .dxrpControl_Office2003Blue .dxrpBE, .dxrpControl_Office2003Blue .dxrpHLE, .dxrpControl_Office2003Blue .dxrpHRE, .dxrpControlGB_Office2003Blue .dxrpBE
        {
            border-bottom: 1px solid #002D96;
        }
        .dxrpControl_Office2003Blue .dxrpLE, .dxrpControl_Office2003Blue .dxrpHLE, .dxrpControlGB_Office2003Blue .dxrpLE
        {
            border-left: 1px solid #002D96;
        }
        .dxrpControl_Office2003Blue .dxrpHeader_Office2003Blue, .dxrpControl_Office2003Blue .dxrpHLE, .dxrpControl_Office2003Blue .dxrpHRE
        {
            background-image: none;
            background-color: #7BA4E0;
        }
        .dxrpControl_Office2003Blue .dxrpRE, .dxrpControl_Office2003Blue .dxrpHRE, .dxrpControlGB_Office2003Blue .dxrpRE
        {
            border-right: 1px solid #002D96;
        }
        .dxrpControl_Office2003Blue .dxrpcontent, .dxrpControl_Office2003Blue .dxrpLE, .dxrpControl_Office2003Blue .dxrpRE, .dxrpControl_Office2003Blue .dxrpBE, .dxrpControl_Office2003Blue .dxrpNHTE, .dxrpControlGB_Office2003Blue .dxrpcontent, .dxrpControlGB_Office2003Blue .dxrpLE, .dxrpControlGB_Office2003Blue .dxrpRE, .dxrpControlGB_Office2003Blue .dxrpBE, .dxrpControlGB_Office2003Blue .dxrpNHTE, .dxrpControlGB_Office2003Blue span.dxrpHeader_Office2003Blue
        {
            background-image: none;
            background-color: #DDECFE;
        }
        .dxrpControl_Office2003Blue .dxrpcontent, .dxrpControlGB_Office2003Blue .dxrpcontent
        {
            background-image: none;
            vertical-align: top;
        }
        .style1
        {
            height: 18px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%">
    <tr>
        <td>
            <div class="header" id="lblServer" runat="server">Microsoft SharePoint Farms</div>
        </td>
    </tr>
    <tr>
		<td>
        
            <div id="successDiv" class="success" runat="server" style="display: none">Success.
			</div>
                <dx:ASPxGridView runat="server" KeyFieldName="ID" AutoGenerateColumns="False"
                    Width="100%" ID="SPFarmGrid"  OnPageSizeChanged="SPFarmGrid_PageSizeChanged" 
                                
					OnHtmlRowCreated="SPFarmGrid_HtmlRowCreated" Cursor="pointer" 
					EnableTheming="True" Theme="Office2003Blue" >
                    <Columns>
						<dx:GridViewCommandColumn ButtonType="Image" Caption="Actions"
                            VisibleIndex="0" Width="60px">
                            <EditButton Visible="True" >
                                <Image Url="../images/edit.png">
                                </Image>
                            </EditButton>
                            <CellStyle CssClass="GridCss1">
                                <Paddings Padding="3px" />
							<Paddings Padding="3px"></Paddings>
                            </CellStyle>
                            <ClearFilterButton Visible="True">
                                <Image Url="~/images/clear.png">
                                </Image>
                            </ClearFilterButton>
                            <HeaderStyle CssClass="GridCssHeader" />
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn Caption="Name" FieldName="Farm"
                            VisibleIndex="2">
                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss"  HorizontalAlign="Left">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="ID" FieldName="ID"
                            Visible="False" VisibleIndex="7">
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                    </Columns>
					<SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" 
                        AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True" 
                        AllowSelectSingleRowOnly="True">
					</SettingsBehavior>
                    <Settings ShowFilterRow="True" ShowGroupPanel="True" />

                    <SettingsText ConfirmDelete="Are you sure you want to delete?"></SettingsText>
                    <Styles CssPostfix="Office2010Silver" CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css">
                        <Header ImageSpacing="5px" SortingImageSpacing="5px" CssClass="GridCssHeader">
                        </Header>
                            <GroupRow Font-Bold="True">
                        </GroupRow>
						<AlternatingRow CssClass="GridAltRow" Enabled="True">
						</AlternatingRow>
                        <Cell CssClass="GridCss">
                        </Cell>
                        <LoadingPanel ImageSpacing="5px">
                        </LoadingPanel>
                    </Styles>
                    <StylesEditors ButtonEditCellSpacing="0">
                        <ProgressBar Height="21px">
                        </ProgressBar>
                    </StylesEditors>
                    <SettingsPager PageSize="50" SEOFriendly="Enabled" >
					<PageSizeItemSettings Visible="true" />
					<PageSizeItemSettings Visible="True"></PageSizeItemSettings>
					</SettingsPager>
                                
				</dx:ASPxGridView>
        
			</td>
		</tr>
	</table>
</asp:Content>
