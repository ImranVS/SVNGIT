<%@ Page Title="VitalSigns Plus - License Information" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="LicenseInformation.aspx.cs" Inherits="VSWebUI.Configurator.LicenseInformation" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx1" %>
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
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        </style>
    <script type="text/javascript">
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
   </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table width="100%">
        <tr>
            <td colspan="2">
                <div class="header" id="servernamelbldisp" runat="server">License Information</div>
                <div id="successDiv" runat="server" class="alert alert-success" style="display: none">License Information was successfully updated.
                <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                </div>
            </td>
        </tr>
        <tr>
            <%--<td colspan="2">
                
                <dx:ASPxLabel runat="server" ID="LicenseCodeLabel" CssClass="lblsmallFont"></dx:ASPxLabel>
                
            </td>--%>
        </tr>
        <%--<tr>
            <td valign="top">
                <dx:ASPxRoundPanel runat="server" GroupBoxCaptionOffsetY="-24px" 
                    HeaderText="Enabled Monitoring" Width="100%" 
                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" CssPostfix="Glass" 
                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" ID="ASPxRoundPanel5">
<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
<PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                            <table>
                                <tr>
                                    <td>
                                        <dx:ASPxCheckBox runat="server" CheckState="Unchecked" Text="Domino Servers" 
                                            CssClass="lblsmallFont" ID="DominoServerCheckBox"></dx:ASPxCheckBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxCheckBox runat="server" CheckState="Unchecked" Text="NotesMail Probes" 
                                            CssClass="lblsmallFont" ID="NotesMailCheckBox"></dx:ASPxCheckBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxCheckBox runat="server" CheckState="Unchecked" Text="Notes Databases" 
                                            CssClass="lblsmallFont" ID="NotesDatabasesCheckBox"></dx:ASPxCheckBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxCheckBox runat="server" CheckState="Unchecked" Text="Domino Clusters" 
                                            CssClass="lblsmallFont" ID="DominoClusterCheckBox"></dx:ASPxCheckBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxCheckBox runat="server" CheckState="Unchecked" 
                                            Text="Scheduled Domino Console Commands" CssClass="lblsmallFont" 
                                            ID="SheduledDominoCheckBox"></dx:ASPxCheckBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxCheckBox runat="server" CheckState="Unchecked" 
                                            Text="BlackBerry Device Probes" CssClass="lblsmallFont" 
                                            ID="BBDeviceProbeCheckBox"></dx:ASPxCheckBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxCheckBox runat="server" CheckState="Unchecked" 
                                            Text="BlackBerry Message Queues" CssClass="lblsmallFont" ID="BBMsgQCheckBox"></dx:ASPxCheckBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxCheckBox runat="server" CheckState="Unchecked" 
                                            Text="BlackBerry Enterprise Servers*" CssClass="lblsmallFont" 
                                            ID="BBEnterprizeServerCheckBox"></dx:ASPxCheckBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxCheckBox runat="server" CheckState="Unchecked" Text="BlackBerry Users*" 
                                            CssClass="lblsmallFont" ID="BBUsersCheckBox"></dx:ASPxCheckBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxCheckBox runat="server" CheckState="Unchecked" Text="URLs*" 
                                            CssClass="lblsmallFont" ID="URLsCheckBox"></dx:ASPxCheckBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxCheckBox runat="server" CheckState="Unchecked" Text="Mail Services*" 
                                            CssClass="lblsmallFont" ID="MailServicesCheckBox"></dx:ASPxCheckBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxCheckBox runat="server" CheckState="Unchecked" Text="Network Devices*" 
                                            CssClass="lblsmallFont" ID="NetworkDevicesCheckBox"></dx:ASPxCheckBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxCheckBox runat="server" CheckState="Unchecked" Text="Sametime*" 
                                            CssClass="lblsmallFont" ID="SameTimeCheckBox"></dx:ASPxCheckBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div id="infoDivUncheck" class="info">Uncheck if not needed to boost  performance.  Changes take effect the next time the service is started.
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </dx:PanelContent>
</PanelCollection>
</dx:ASPxRoundPanel>

            </td>
            <td valign="top">
                <dx:ASPxRoundPanel runat="server" GroupBoxCaptionOffsetY="-24px" 
                    HeaderText="License Information" Width="100%" 
                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" CssPostfix="Glass" 
                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" ID="ASPxRoundPanel6">
<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
<PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                            <table>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel runat="server" ID="DominoServerLabel" CssClass="lblsmallFont"></dx:ASPxLabel>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel runat="server" ID="NotesMailProbeLabel" CssClass="lblsmallFont"></dx:ASPxLabel>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel runat="server" ID="NotesDatabasesLabel" CssClass="lblsmallFont"></dx:ASPxLabel>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel runat="server" ID="DominoClusterLabel" CssClass="lblsmallFont"></dx:ASPxLabel>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel runat="server" ID="ConsolecmdsLabel" CssClass="lblsmallFont"></dx:ASPxLabel>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel runat="server" ID="BlackBerryDeviceLabel" CssClass="lblsmallFont"></dx:ASPxLabel>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel runat="server" ID="BBMsgQLabel" CssClass="lblsmallFont"></dx:ASPxLabel>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel runat="server" ID="BBEnterpriseLabel" CssClass="lblsmallFont"></dx:ASPxLabel>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel runat="server" ID="BlackBerryUsersLabel" CssClass="lblsmallFont"></dx:ASPxLabel>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel runat="server" ID="URLsLabel" CssClass="lblsmallFont"></dx:ASPxLabel>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel runat="server" ID="MailServicesLabel" CssClass="lblsmallFont"></dx:ASPxLabel>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel runat="server" ID="NetworkDeviceLabel" CssClass="lblsmallFont"></dx:ASPxLabel>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel runat="server" ID="SametimeLabel" CssClass="lblsmallFont"></dx:ASPxLabel>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </dx:PanelContent>
</PanelCollection>
</dx:ASPxRoundPanel>

           

            </td>
        </tr>--%>
        <tr>
            <td colspan="2">
            <div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">The following fields were not updated:
                </div>
            <table width="100%">
                    <tr>
					<td valign="top">
					    <div class="info">
                            <dx:ASPxLabel ID="LicenseTypeLabel" CssClass="lblsmallFont" runat="server"></dx:ASPxLabel>
							<dx:ASPxLabel ID="LicenseExirationLabel" CssClass="lblsmallFont" runat="server" />
                            <asp:Label runat="server" ID="LicenseCodeLabel" CssClass="lblsmallFont" 
                                Font-Bold="True"></asp:Label>
                            <%--<dx:ASPxLabel ID="LicenseCountLabel" CssClass="lblsmallFont" runat="server" Text="License Count: "></dx:ASPxLabel>
                            <dx:ASPxLabel ID="LicenseCountValueLabel" CssClass="lblsmallFont" 
                                runat="server" Font-Bold="True"></dx:ASPxLabel>--%>
                            <asp:Label runat="server" ID="Tlunitspurchase" CssClass="lblsmallFont" Text="Total License Units:" Visible="true"></asp:Label>
                            <asp:Label runat="server" ID="Totalunitspurchased" CssClass="lblsmallFont" 
                                Font-Bold="True" Visible="true" ></asp:Label>
                            <asp:Label runat="server" ID="Tlunitsused" CssClass="lblsmallFont" Text="Total Units Used:" Visible="false"></asp:Label>
                            <asp:Label runat="server" ID="Totalunitsused" CssClass="lblsmallFont" 
                                Font-Bold="True" Visible="false" ></asp:Label>
				            <asp:Label runat="server" ID="Label1" CssClass="lblsmallFont" Text="Total Units Remaining:" Visible="false"></asp:Label>
                            <asp:Label runat="server" ID="TotalUnitsRemaining" CssClass="lblsmallFont" 
                                Font-Bold="True" Visible="false" ></asp:Label>
				
                <%--<dx:ASPxLabel runat="server" ID="LicenseCodeLabel" Font-Bold="true" CssClass="lblsmallFont"></dx:ASPxLabel>--%>
                </div>
           
                       <%--<b> Subscription valid Until:2/6/2015&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; License Count:30</b>
                      </div>--%>
					</td></tr>
					<tr>
                        <td>
                            <dx:ASPxButton ID="LicenseCodeButton2" runat="server" CssClass="sysButton"
                                Text="Enter License Code" onclick="LicenseCodeButton2_Click" CausesValidation="false">
                            </dx:ASPxButton>
                        </td>
                        <%--<td>
                            <dx:ASPxButton ID="SaveButton" runat="server" 
                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                CssPostfix="Office2010Blue" 
                                SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="Save" 
                                onclick="SaveButton_Click">
                            </dx:ASPxButton>
                        </td>--%>
                        <%--<td>
                            <dx:ASPxButton ID="CancelButton" runat="server" 
                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                CssPostfix="Office2010Blue" 
                                SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" 
                                Text="Cancel" onclick="CancelButton_Click" >
                            </dx:ASPxButton>
                        </td>--%>
                    </tr>
                </table>
            </td>
        </tr>
		<tr>
		<td>
	
							<dx:ASPxGridView runat="server" CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css"
                                CssPostfix="Office2010Silver" KeyFieldName="ID" AutoGenerateColumns="False"
                                Width="400px" ID="LicenseUsage"   
                                 Cursor="pointer" 
                                   EnableTheming="True" Theme="Office2003Blue" >
								    
                                <Columns>
                                      
                                    <dx:GridViewDataTextColumn Caption="Server Types" FieldName="ServerType" 
                                        VisibleIndex="0">
                                       <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Servers Configured" FieldName="noofservers"
                                        VisibleIndex="1">
                                        <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss"  HorizontalAlign="Left">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader2" />
                                        <CellStyle CssClass="GridCss2">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Servers/Devices Allocated" FieldName="noofunits"
                                        VisibleIndex="2" Visible="false">
                                        <Settings AutoFilterCondition="Contains" />
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss"  HorizontalAlign="Left">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader2" />
                                        <CellStyle CssClass="GridCss2">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Units Used" FieldName="totalcost"
                                        VisibleIndex="3" Visible="false">
                                        <Settings AutoFilterCondition="Contains" />
                                        <EditCellStyle CssClass="GridCss">
                                        </EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss"  HorizontalAlign="Left">
                                        </EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader2" />
                                        <CellStyle CssClass="GridCss2">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                   
                                    <dx1:GridViewDataTextColumn Caption="Unit Cost" FieldName="UnitCost" 
                                        VisibleIndex="4" Visible="false">
                                        <HeaderStyle CssClass="GridCssHeader2" />
                                        <CellStyle CssClass="GridCss2">
                                        </CellStyle>
                                    </dx1:GridViewDataTextColumn>
                                   
                                </Columns>
                     <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" 
                                    AllowSelectByRowClick="True" ProcessSelectionChangedOnServer="True" 
                                    AllowSelectSingleRowOnly="True"></SettingsBehavior>
                                <Settings ShowFilterRow="True" ShowGroupPanel="False" />

                            <Settings ShowFilterRow="True" ShowGroupPanel="False"></Settings>

                                <SettingsText ConfirmDelete="Are you sure you want to delete?"></SettingsText>
                                <Images SpriteCssFilePath="~/App_Themes/Office2010Silver/{0}/sprite.css">
                                    <LoadingPanelOnStatusBar Url="~/App_Themes/Office2010Silver/GridView/Loading.gif">
                                    </LoadingPanelOnStatusBar>
                                    <LoadingPanel Url="~/App_Themes/Office2010Silver/GridView/Loading.gif">
                                    </LoadingPanel>
                                </Images>
                                <ImagesFilterControl>
                                    <LoadingPanel Url="~/App_Themes/Office2010Silver/GridView/Loading.gif">
                                    </LoadingPanel>
                                </ImagesFilterControl>
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

    <dx:ASPxPopupControl ID="LicensePopupControl" runat="server" 
                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" 
                    HeaderText="License Key" 
                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Modal="True" 
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
                    Theme="MetropolisBlue">
                    <LoadingPanelImage Url="~/App_Themes/Glass/Web/Loading.gif">
                    </LoadingPanelImage>
                    <HeaderStyle>
                    <Paddings PaddingLeft="10px" PaddingRight="6px" PaddingTop="1px" />
                    </HeaderStyle>
                    <ContentCollection>
<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
    <table class="style1">
        <tr>
            <td>
                <dx:ASPxLabel ID="passwordLabel" runat="server" 
                    Text="Please enter your license key:">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxTextBox ID="KeyTextBox" runat="server" 
                    Width="170px" ClientInstanceName="keyTxtBox">
					<ValidationSettings ErrorDisplayMode="ImageWithTooltip">
					<RequiredField  ErrorText="Please enter your license key." IsRequired="true"/>
					</ValidationSettings>
                    <ClientSideEvents KeyDown="function(s, e) {OnKeyDown(s, e);}" />
                </dx:ASPxTextBox>
            </td>
        </tr>
		<tr>
		<td>
		<div id="erordivinpopup" runat="server" class="alert alert-danger" style="display: none">The following fields were not updated:
                </div>
		</td>
		</tr>
        <tr>
            <td>
                <dx:ASPxButton ID="KeyOK" runat="server" CssClass="sysButton"
                    Text="OK" 
                    OnClick="KeyOK_Click" ClientInstanceName="goButton">
                </dx:ASPxButton>
                <dx:ASPxButton ID="KeyOKSave" runat="server" OnClick="KeyOKSave_Click" 
                    Text="OK" CssClass="sysButton">
                </dx:ASPxButton>
            </td>
        </tr>
		
    </table>
                        </dx:PopupControlContentControl>
</ContentCollection>
                </dx:ASPxPopupControl>
</asp:Content>
