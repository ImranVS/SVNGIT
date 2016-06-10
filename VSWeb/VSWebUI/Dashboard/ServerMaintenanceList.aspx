<%@ Page Title="VitalSigns Plus - Server Maintenance List" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="ServerMaintenanceList.aspx.cs" Inherits="VSWebUI.Configurator.ServerMaintenanceList" %>

<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">  
        .focus   
        {  
            background-color: #DDDDFF;  
            border: solid 1px blue;  
        }  
        .invalid   
        {  
            background-color: #FF0000;  
            border: solid 1px #FF0000;  
        }  
        .negative  
        {  
            background-color: #FF5555;  
        }  
    </style>
    <script type="text/javascript">
        //10/30/2013 NS added - fix for when an Enter key is pressed within the text box on the page - redirect the
        //submit function to the actual Go button on the page instead of performing a whole page submit
        function OnKeyDown() {
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
        <td>
            <div class="header" id="servernamelbldisp" runat="server">Server Maintenance</div>
        </td>
        <td>
            &nbsp;
        </td>    
        <td align="right">
            <table>
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
        </td>        
    </tr>
</table>
<table width="100%">
        <tr>
            <td valign="top">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table width="60%">
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" 
                                Text="From Date:" Wrap="False">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <asp:TextBox ID="txtfromdate" runat="server" onkeypress="OnKeyDown();"></asp:TextBox>
                            <cc1:CalendarExtender ID="calextender" runat="server" Enabled="True" 
                                Format="MM/dd/yyyy" TargetControlID="txtfromdate">
                            </cc1:CalendarExtender>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvtxtfromdate" runat="server" 
                                ControlToValidate="txtfromdate" ErrorMessage="Enter From Date" 
                                ForeColor="#FF3300"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel2" runat="server" CssClass="lblsmallFont" 
                                Text="From Time:" Wrap="False">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxTimeEdit ID="ASPxTimeEdit1" runat="server">
                            </dx:ASPxTimeEdit>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            <dx:ASPxButton ID="ClearButton" runat="server" OnClick="ClearButton_Click" 
                                Text="Clear" CssClass="sysButton" Width="80px">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel3" runat="server" CssClass="lblsmallFont" 
                                Text="To Date:" Wrap="False">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <asp:TextBox ID="txttodate" runat="server" onkeypress="OnKeyDown();"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" 
                                Format="MM/dd/yyyy" TargetControlID="txttodate">
                            </cc1:CalendarExtender>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RFvtxttodate" runat="server" 
                                ControlToValidate="txttodate" ErrorMessage="Enter To Date" 
                                ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel4" runat="server" CssClass="lblsmallFont" 
                                Text="To Time:" Wrap="False">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxTimeEdit ID="ASPxTimeEdit2" runat="server" 
                                OnDateChanged="ASPxTimeEdit2_DateChanged">
                            </dx:ASPxTimeEdit>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            <dx:ASPxButton ID="btnsearch" runat="server" OnClick="btnsearch_Click" 
                                Text="Search" CssClass="sysButton" Width="80px" 
                                ClientInstanceName="goButton">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
                </ContentTemplate>
                </asp:UpdatePanel>
                
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <dx:ASPxGridView ID="maintenancegrid" runat="server" 
                    AutoGenerateColumns="False" OnPageSizeChanged="maintenancegrid_PageSizeChanged"
                    CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                    CssPostfix="Office2010Silver" KeyFieldName="TypeandName" Width="100%" 
                        Theme="Office2003Blue">
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="Server Name" FieldName="servername" 
                            ShowInCustomizationForm="True" VisibleIndex="0">
                           <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader">
                            <Paddings Padding="5px" />
                            </HeaderStyle>
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Server Type" FieldName="ServerType" 
                            ShowInCustomizationForm="True" VisibleIndex="1">
                            <Settings AutoFilterCondition="Contains" AllowAutoFilterTextInputTimer="False" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Start Date" FieldName="StartDate" 
                            ShowInCustomizationForm="True" VisibleIndex="2">
                            <Settings AllowAutoFilter="False" AllowDragDrop="True" 
                                AutoFilterCondition="Contains" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Start Time" FieldName="StartTime" 
                            ShowInCustomizationForm="True" VisibleIndex="3">
                            <Settings AllowAutoFilter="False" AllowDragDrop="True" 
                                AutoFilterCondition="Contains" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Duration" FieldName="Duration" 
                            ShowInCustomizationForm="True" VisibleIndex="4">
                            <Settings AllowAutoFilter="False" AllowDragDrop="True" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="End Date" FieldName="EndDate" 
                            ShowInCustomizationForm="True" VisibleIndex="5">
                            <Settings AllowAutoFilter="False" AllowDragDrop="True" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Maintenance Type" FieldName="MaintType" 
                            ShowInCustomizationForm="True" VisibleIndex="6">
                            <Settings AllowAutoFilter="False" AllowDragDrop="True" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Maintenance Days List" 
                            FieldName="MaintDaysList" ShowInCustomizationForm="True" VisibleIndex="7">
                            <Settings AllowAutoFilter="False" AllowDragDrop="True" />
                            <EditCellStyle CssClass="GridCss">
                            </EditCellStyle>
                            <EditFormCaptionStyle CssClass="GridCss">
                            </EditFormCaptionStyle>
                            <HeaderStyle CssClass="GridCssHeader" />
                            <CellStyle CssClass="GridCss">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsBehavior ColumnResizeMode="NextColumn" />
                    <SettingsPager SEOFriendly="Enabled" PageSize="100">
                        <PageSizeItemSettings Visible="True">
                        </PageSizeItemSettings>
                    </SettingsPager>
                    <Settings ShowFilterRow="True" ShowGroupPanel="True" />
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
                    <Styles CssFilePath="~/App_Themes/Office2010Silver/{0}/styles.css" 
                        CssPostfix="Office2010Silver">
                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                        </Header>
                        <GroupRow Font-Bold="True" Font-Italic="False">
                        </GroupRow>
                        <AlternatingRow CssClass="GridAltRow" Enabled="True">
                        </AlternatingRow>
                        <GroupFooter Font-Bold="True">
                        </GroupFooter>
                        <GroupPanel Font-Bold="False">
                        </GroupPanel>
                        <LoadingPanel ImageSpacing="5px">
                        </LoadingPanel>
                    </Styles>
                    <StylesEditors ButtonEditCellSpacing="0">
                        <ProgressBar Height="21px">
                        </ProgressBar>
                    </StylesEditors>
                </dx:ASPxGridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnsearch" />
                    <asp:AsyncPostBackTrigger ControlID="ClearButton" />
                </Triggers>
                </asp:UpdatePanel>
                
                <br />
                <br />
            </td>
        </tr>
        <tr>
        <td>
     
            <dx:ASPxPopupControl ID="MsgPopupControl" runat="server" 
                    HeaderText="Information" Modal="True" PopupHorizontalAlign="WindowCenter" 
                    PopupVerticalAlign="WindowCenter" Theme="Glass">
                    <ContentCollection>
<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
    <table class="style1">
        <tr>
            <td>
                <dx:ASPxLabel ID="ErrmsgLabel" runat="server">
                </dx:ASPxLabel>
            </td>
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
</asp:Content>
