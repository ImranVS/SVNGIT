<%@ Page Title="VitalSigns Plus - Manage Server Settings" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="ApplyServerSettings.aspx.cs" Inherits="VSWebUI.Security.ApplyServerSettings" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx1" %>



<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx2" %>

    






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
        .dxtlIndent_Office2003Blue, .dxtlIndentWithButton_Office2003Blue
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
        }
        .dxtl__B1
        {
            border-top-style: none !important;
            border-right-style: none !important;
            border-bottom-style: none !important;
        }
        .dxtlPagerTopPanel_Office2003Blue, .dxtlPagerBottomPanel_Office2003Blue
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
                <td colspan="2">
    <dx:ASPxPageControl Font-Bold="True"  ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
        CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
        CssPostfix="Glass" Height="280px"        
        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" TabSpacing="0px" >
                       
        <TabPages>
        <dx:TabPage Text="Server Attributes">
            <TabImage Url="~/images/information.png"/>
                <ContentCollection>
                    <dx:ContentControl runat="server" SupportsDisabledAttribute="True">

                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Server Settings Editor" Font-Bold="True"
                    Font-Size="Large" Style="color: #000000; font-family: Verdana"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <div class="info">The server settings editor allows you to change one or more settings across multiple servers. 
                First, select a server type from the drop down menu.  <br />
                Then select the attributes you wish to change and specify the value.
                Finally, select the servers you wish to change and click the 'Apply' button.
                </div>
            </td>
        </tr>
        <tr>
            <td id="td1" runat="server" align="left">
                <table>
                    <tr>
                        <%--<td><asp:Label ID="Label2" runat="server" Text="Profile Name" ></asp:Label>
                        </td>
                        <td>
                            <dx:ASPxTextBox ID="ProfileTextBox" runat="server" Width="170px">
                                <ValidationSettings ErrorText="Enter Profile Name" SetFocusOnError="True">
                                    <RequiredField IsRequired="True" />
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                        </td>--%>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Select server type:" 
                                CssClass="lblsmallFont"></asp:Label>
                        </td>
                        <td>
                            <dx:ASPxComboBox ID="ServerTypeComboBox" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ServerTypeComboBox_SelectedIndexChanged"
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
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ApplyServersButton" />
    </Triggers>
    </asp:UpdatePanel>    

                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <table>
        <tr>
            <td>
                <dx:ASPxGridView runat="server" KeyFieldName="ID" AutoGenerateColumns="False" ID="ProfilesGridView"
        ClientInstanceName="ProfilesGridView" Theme="Office2003Blue" EnableTheming="True"
        Width="500px">
        <Columns>
            <dx:GridViewCommandColumn Caption="Select" ShowSelectCheckbox="True" VisibleIndex="0"
                Width="20px">
                <HeaderStyle CssClass="GridCssHeader" />
            </dx:GridViewCommandColumn>
            <dx:GridViewDataColumn Caption="Attribute Name" VisibleIndex="1" FieldName="AttributeName">
                <DataItemTemplate>
                    <dx:ASPxLabel ID="lblAttribute" Width="100%" runat="server" Value='<%# Eval("AttributeName")%>'>
                    </dx:ASPxLabel>
                    <%--<dx:ASPxTextBox ID="txtValue" Width="100%" runat="server" Value='<%# Eval("AttributeName")%>'></dx:ASPxTextBox>--%>
                </DataItemTemplate>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss" Wrap="False">
                </CellStyle>
            </dx:GridViewDataColumn>
            <%--<dx:GridViewDataTextColumn Caption="Attribute Name" VisibleIndex="1"  FieldName="AttributeName">
               <DataItemTemplate>
               <dx:ASPxTextBox ID="txtValue" Width="100%" Enabled="false" runat="server" Value='<%# Eval("AttributeName")%>'></dx:ASPxTextBox>
               </DataItemTemplate>
               <HeaderStyle CssClass="GridCssHeader" />
            </dx:GridViewDataTextColumn>--%>
            <dx:GridViewDataTextColumn VisibleIndex="2" Caption="Default Value" FieldName="DefaultValue">
                <DataItemTemplate>
                    <dx:ASPxTextBox ID="txtDefaultValue" Width="100%" runat="server" Value='<%# Eval("DefaultValue")%>'>
                    </dx:ASPxTextBox>
                </DataItemTemplate>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataColumn VisibleIndex="3" Caption="Unit Of Measurement" FieldName="UnitOfMeasurement">
                <DataItemTemplate>
                    <dx:ASPxLabel ID="lblUnitOfMeasurement" Width="100%" runat="server" Value='<%# Eval("UnitOfMeasurement")%>'>
                    </dx:ASPxLabel>
                </DataItemTemplate>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss" Wrap="False">
                </CellStyle>
            </dx:GridViewDataColumn>
            <dx:GridViewDataColumn VisibleIndex="5" Caption="RelatedTable" FieldName="RelatedTable"
                Visible="false">
                <DataItemTemplate>
                    <dx:ASPxLabel ID="lblRelatedTable" Width="100%" ForeColor="Black" runat="server" Value='<%# Eval("RelatedTable")%>'>
                    </dx:ASPxLabel>
                </DataItemTemplate>
                <HeaderStyle CssClass="GridCssHeader" />
            </dx:GridViewDataColumn>
            <dx:GridViewDataColumn VisibleIndex="5" Caption="RelatedField" FieldName="RelatedField"
                Visible="false">
                <DataItemTemplate>
                    <dx:ASPxLabel ID="tlblRelatedField" Width="100%" ForeColor="Black" runat="server" Value='<%# Eval("RelatedField")%>'>
                    </dx:ASPxLabel>
                </DataItemTemplate>
                <HeaderStyle CssClass="GridCssHeader" />
            </dx:GridViewDataColumn>
            <%--<dx:GridViewDataTextColumn VisibleIndex="3" Caption="Unit Of Measurement"  FieldName="UnitOfMeasurement">
               <DataItemTemplate>
               <dx:ASPxTextBox ID="txtValue" Enabled="false" Width="100%" runat="server" Value='<%# Eval("UnitOfMeasurement")%>'></dx:ASPxTextBox>
               </DataItemTemplate>
               <HeaderStyle CssClass="GridCssHeader" />
            </dx:GridViewDataTextColumn>--%>
            <dx:GridViewDataTextColumn Caption="ID" VisibleIndex="4" FieldName="ID" Visible="false">
                <HeaderStyle CssClass="GridCssHeader" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="isSelected" VisibleIndex="5" FieldName="isSelected" Visible="false">
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
        <SettingsPager PageSize="10">
            <PageSizeItemSettings Visible="true" />
        </SettingsPager>
    </dx:ASPxGridView>
            </td>
        </tr>
        <tr>
            <td>
                <div id="successDiv" runat="server" class="success" style="display: none">Settings for selected servers were successully updated.
                </div>
            </td>
        </tr>
    </table>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ServerTypeComboBox" />
        <asp:AsyncPostBackTrigger ControlID="ApplyServersButton" />
    </Triggers>
    </asp:UpdatePanel>

                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table id="tblServer" runat="server">
        <tr>
            <td></td>
        </tr>
        <tr>
            <td id="Td2"   runat="server" align="left">
                <dx:ASPxButton ID="CollapseAllButton" runat="server" 
                    ClientInstanceName="collapseAll" EnableClientSideAPI="False" 
                    onclick="CollapseAllButton_Click" Text="Collapse All" Theme="Office2010Blue" 
                    Wrap="False">
                    <Image Url="~/images/icons/forbidden.png">
                    </Image>
                </dx:ASPxButton>
            </td>
        </tr>
        <tr>
            <td>
               <dx:ASPxTreeList runat="server" KeyFieldName="Id" ParentFieldName="LocId" AutoGenerateColumns="False"
                                Theme="Office2003Blue" Width="100%" ID="ServersTreeList" 
                    CssClass="lblsmallFont" style="margin-top: 0px">
                                <Columns>
                                    <dx:TreeListTextColumn FieldName="Name" Name="Servers"
                                        Caption="Servers  " VisibleIndex="0">
                                        <HeaderStyle CssClass="GridCssHeader"></HeaderStyle>
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:TreeListTextColumn>
                                    <dx:TreeListTextColumn FieldName="actid" Name="actid"
                                        Visible="False" VisibleIndex="1">
                                    </dx:TreeListTextColumn>
                                    <dx:TreeListTextColumn FieldName="tbl" Name="tbl"
                                        Visible="False" VisibleIndex="2">
                                    </dx:TreeListTextColumn>
                                    <dx:TreeListTextColumn FieldName="LocId" Name="LocId"
                                        Visible="False" VisibleIndex="3">
                                    </dx:TreeListTextColumn>
                                    <dx:TreeListTextColumn FieldName="ServerType" VisibleIndex="4">
                                        <HeaderStyle CssClass="GridCssHeader"></HeaderStyle>
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:TreeListTextColumn>
                                    <dx:TreeListTextColumn FieldName="Description" VisibleIndex="5">
                                        <HeaderStyle CssClass="GridCssHeader"></HeaderStyle>
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:TreeListTextColumn>
                                </Columns>
                                <Settings GridLines="Both"></Settings>
                                <SettingsBehavior AutoExpandAllNodes="True" AllowDragDrop="False"></SettingsBehavior>
                                <SettingsPager Mode="ShowPager" AlwaysShowPager="True" PageSize="20">
                                    <PageSizeItemSettings Visible="True">
                                    </PageSizeItemSettings>
                                </SettingsPager>
                                <SettingsSelection Enabled="True" AllowSelectAll="True" Recursive="True"></SettingsSelection>
                                <Styles>
                                    <LoadingPanel ImageSpacing="5px">
                                    </LoadingPanel>
                                    <AlternatingNode Enabled="True">
                                    </AlternatingNode>
                                </Styles>
                                <StylesPager>
                                    <PageNumber ForeColor="#3E4846">
                                    </PageNumber>
                                    <Summary ForeColor="#1E395B">
                                    </Summary>
                                </StylesPager>
                                <StylesEditors ButtonEditCellSpacing="0">
                                </StylesEditors>
                            </dx:ASPxTreeList>
            </td>
        </tr>
    </table>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ServerTypeComboBox" />
        <asp:AsyncPostBackTrigger ControlID="ApplyServersButton" />
    </Triggers>
    </asp:UpdatePanel>

                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table>
        <tr>
            <%--<dx:ASPxTextBox ID="txtValue" Width="100%" runat="server" Value='<%# Eval("AttributeName")%>'></dx:ASPxTextBox>--%>
            <td id="Td4" style="color: Black" runat="server" align="left">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <div id="errorDiv" runat="server" class="error" style="display: none">Please select at least one Attribute and one Server in order to proceed.
                </div>
                <div id="errorDiv2" runat="server" class="error" style="display: none">Settings for selected servers were NOT updated.
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <dx:ASPxButton runat="server" Text="Apply" Theme="Office2010Blue"
                    Width="50px" ID="ApplyServersButton" OnClick="ApplyToServers_Click">
                </dx:ASPxButton>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ServerTypeComboBox" />
    </Triggers>
    </asp:UpdatePanel>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
            <dx:TabPage Text="Domino Server Tasks">
            <TabImage Url="~/images/information.png"/>
                <ContentCollection>
                    <dx:ContentControl runat="server" SupportsDisabledAttribute="True">

                        <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <table>
        <tr>
            <td>
                <dx:ASPxGridView runat="server" KeyFieldName="TaskID" AutoGenerateColumns="False" ID="DominoTasksGridView"
        ClientInstanceName="DominoTasksGridView" Theme="Office2003Blue" EnableTheming="True"
        Width="500px">
        <Columns>
            <dx:GridViewCommandColumn Caption="Select" ShowSelectCheckbox="True" VisibleIndex="0"
                Width="20px">
                <HeaderStyle CssClass="GridCssHeader" />
            </dx:GridViewCommandColumn>
            <dx:GridViewDataColumn Visible="false" FieldName="TaskID">
            <CellStyle CssClass="GridCss" Wrap="False">
                </CellStyle></dx:GridViewDataColumn>
            <dx:GridViewDataColumn Caption="Task Name" VisibleIndex="1" FieldName="TaskName">
                <DataItemTemplate>
                    <dx:ASPxLabel ID="txtTaskName" Width="100%" runat="server" Value='<%# Eval("TaskName")%>'>
                    </dx:ASPxLabel>
                    <%--<dx:ASPxTextBox ID="txtValue" Width="100%" runat="server" Value='<%# Eval("AttributeName")%>'></dx:ASPxTextBox>--%>
                </DataItemTemplate>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss" Wrap="False">
                </CellStyle>
            </dx:GridViewDataColumn>
            <dx:GridViewDataTextColumn Caption="isSelected" VisibleIndex="2" FieldName="isSelected" Visible="false">
                <HeaderStyle CssClass="GridCssHeader" />
            </dx:GridViewDataTextColumn>
           <%--<dx:GridViewDataTextColumn Caption="Attribute Name" VisibleIndex="1"  FieldName="AttributeName">
               <DataItemTemplate>
               <dx:ASPxTextBox ID="txtValue" Width="100%" Enabled="false" runat="server" Value='<%# Eval("AttributeName")%>'></dx:ASPxTextBox>
               </DataItemTemplate>
               <HeaderStyle CssClass="GridCssHeader" />
            </dx:GridViewDataTextColumn>--%>
            <%--<dx:GridViewDataTextColumn VisibleIndex="2" Caption="Default Value" FieldName="DefaultValue">
                <DataItemTemplate>
                    <dx:ASPxTextBox ID="txtValue" Width="100%" runat="server" Value='<%# Eval("DefaultValue")%>'>
                    </dx:ASPxTextBox>
                </DataItemTemplate>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataColumn VisibleIndex="3" Caption="Unit Of Measurement" FieldName="UnitOfMeasurement">
                <DataItemTemplate>
                    <dx:ASPxLabel ID="txtValue" Width="100%" runat="server" Value='<%# Eval("UnitOfMeasurement")%>'>
                    </dx:ASPxLabel>
                </DataItemTemplate>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss" Wrap="False">
                </CellStyle>
            </dx:GridViewDataColumn>
            <dx:GridViewDataColumn VisibleIndex="5" Caption="RelatedTable" FieldName="RelatedTable"
                Visible="false">
                <DataItemTemplate>
                    <dx:ASPxLabel ID="txtValue" Width="100%" ForeColor="Black" runat="server" Value='<%# Eval("RelatedTable")%>'>
                    </dx:ASPxLabel>
                </DataItemTemplate>
                <HeaderStyle CssClass="GridCssHeader" />
            </dx:GridViewDataColumn>
            <dx:GridViewDataColumn VisibleIndex="5" Caption="RelatedField" FieldName="RelatedField"
                Visible="false">
                <DataItemTemplate>
                    <dx:ASPxLabel ID="txtValue" Width="100%" ForeColor="Black" runat="server" Value='<%# Eval("RelatedField")%>'>
                    </dx:ASPxLabel>
                </DataItemTemplate>
                <HeaderStyle CssClass="GridCssHeader" />
            </dx:GridViewDataColumn>--%>
            <%--<dx:GridViewDataTextColumn VisibleIndex="3" Caption="Unit Of Measurement"  FieldName="UnitOfMeasurement">
               <DataItemTemplate>
               <dx:ASPxTextBox ID="txtValue" Enabled="false" Width="100%" runat="server" Value='<%# Eval("UnitOfMeasurement")%>'></dx:ASPxTextBox>
               </DataItemTemplate>
               <HeaderStyle CssClass="GridCssHeader" />
            </dx:GridViewDataTextColumn>--%>
            <%--<dx:GridViewDataTextColumn Caption="ID" VisibleIndex="4" FieldName="ID" Visible="false">
                <HeaderStyle CssClass="GridCssHeader" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="isSelected" VisibleIndex="5" FieldName="isSelected" Visible="false">
                <HeaderStyle CssClass="GridCssHeader" />
            </dx:GridViewDataxtColumn>--%>
            
             <dx:GridViewDataCheckColumn Caption="Load" FieldName="SendLoadCommand" ShowInCustomizationForm="True"
                                                                                    VisibleIndex="3">
                                                                                    <DataItemTemplate>
                                                                                    <dx2:ASPxCheckBox ID="SendLoadCommand" runat="server" Checked="false">
                                                                                    
                                                                                    </dx2:ASPxCheckBox>
                                                                                    </DataItemTemplate>
                                                                                    
                                                                                    <Settings AllowAutoFilter="False"></Settings>
                                                                                    <EditFormSettings Caption="If Task not loaded, send &#39;Load&#39; command to server (i.e. try to start it)">
                                                                                    </EditFormSettings>
                                                                                    <Settings AllowAutoFilter="False" />
                                                                                    <EditFormSettings Caption="If Task not loaded, send 'Load' command to server (i.e. try to start it)" />
                                                                                    <EditCellStyle CssClass="GridCss">
                                                                                    </EditCellStyle>
                                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                                    </EditFormCaptionStyle>
                                                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                                                    <CellStyle CssClass="GridCss">
                                                                                    </CellStyle>
                                                                                </dx:GridViewDataCheckColumn>
             <dx:GridViewDataCheckColumn Caption="Restart ASAP" FieldName="SendRestartCommand" UnboundType="Boolean"
                                                                                    ShowInCustomizationForm="True" VisibleIndex="4">
                                                                                     <DataItemTemplate>
                                                                                    <dx2:ASPxCheckBox Id="SendRestartCommand" runat="server" Checked="false"></dx2:ASPxCheckBox>
                                                                                    </DataItemTemplate>
                                                                                    <Settings AllowAutoFilter="False"></Settings>
                                                                                    <EditFormSettings Caption="If Task is HUNG, or LOAD TASK fails, send &#39;Tell Server Restart&#39; command, AS SOON AS POSSIBLE">
                                                                                    </EditFormSettings>
                                                                                    <Settings AllowAutoFilter="False" />
                                                                                    <EditFormSettings Caption="If Task is HUNG, or LOAD TASK fails, send 'Tell Server Restart' command, AS SOON AS POSSIBLE" />
                                                                                    <EditCellStyle CssClass="GridCss">
                                                                                    </EditCellStyle>
                                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                                    </EditFormCaptionStyle>
                                                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                                                    <CellStyle CssClass="GridCss1">
                                                                                    </CellStyle>
                                                                                </dx:GridViewDataCheckColumn>
             <dx:GridViewDataCheckColumn Caption="Restart Later" FieldName="RestartOffHours" ShowInCustomizationForm="True"
                                                                                    VisibleIndex="5">
                                                                                     <DataItemTemplate>
                                                                                    <dx2:ASPxCheckBox Id="RestartOffHours" runat="server" Checked="false"></dx2:ASPxCheckBox>
                                                                                    </DataItemTemplate>
                                                                                    <Settings AllowAutoFilter="False"></Settings>
                                                                                    <EditFormSettings Caption="If Task is HUNG, or LOAD TASK fails, send &#39;Tell Server Restart&#39; command, OFF PEAK HOURS ONLY">
                                                                                    </EditFormSettings>
                                                                                    <Settings AllowAutoFilter="False" />
                                                                                    <EditFormSettings Caption="If Task is HUNG, or LOAD TASK fails, send 'Tell Server Restart' command, OFF PEAK HOURS ONLY" />
                                                                                    <EditCellStyle CssClass="GridCss">
                                                                                    </EditCellStyle>
                                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                                    </EditFormCaptionStyle>
                                                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                                                    <CellStyle CssClass="GridCss1">
                                                                                    </CellStyle>
                                                                                </dx:GridViewDataCheckColumn>
             <dx:GridViewDataCheckColumn Caption="Disallow" FieldName="SendExitCommand" ShowInCustomizationForm="True"
                                                                                    VisibleIndex="6">
                                                                                    <DataItemTemplate>
                                                                                    <dx2:ASPxCheckBox Id="SendExitCommand" runat="server" Checked="false"></dx2:ASPxCheckBox>
                                                                                    </DataItemTemplate>
                                                                                    <Settings AllowAutoFilter="False"></Settings>
                                                                                    <EditFormSettings Visible="True" Caption="If Task is running, send &#39;Task exit&#39; command (i.e prohibit this command)">
                                                                                    </EditFormSettings>
                                                                                    <PropertiesCheckEdit>
                                                                                        


<CheckBoxStyle HorizontalAlign="Left" Wrap="True" />
                                                                                        





<Style HorizontalAlign="Left" Wrap="True">
                                                                                            
                                                                                        </Style>
                                                                                    


</PropertiesCheckEdit>
                                                                                    <Settings AllowAutoFilter="False" />
                                                                                    <EditFormSettings Caption="If Task is running, send 'Task exit' command (i.e prohibit this command)"
                                                                                        Visible="True" />
                                                                                    <EditCellStyle CssClass="GridCss">
                                                                                    </EditCellStyle>
                                                                                    <EditFormCaptionStyle CssClass="GridCss">
                                                                                    </EditFormCaptionStyle>
                                                                                    <HeaderStyle CssClass="GridCssHeader" />
                                                                                    <CellStyle CssClass="GridCss1">
                                                                                    </CellStyle>
                                                                                </dx:GridViewDataCheckColumn>
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
        <SettingsPager PageSize="10">
            <PageSizeItemSettings Visible="true" />
        </SettingsPager>
    </dx:ASPxGridView>
            </td>
        </tr>
        <tr>
            <td>
                <div id="successDivDomino" runat="server" class="success" style="display: none">Settings for selected servers were successully updated.
                </div>
            </td>
        </tr>
    </table>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="DominoServerTasksApply" />
        <asp:AsyncPostBackTrigger ControlID="DominoServerTasksClear" />
    </Triggers>
    </asp:UpdatePanel>

                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table id="tabletasks" runat="server">
        <tr>
            <td></td>
        </tr>
        <tr>
            <td id="Td3"   runat="server" align="left">
                <dx:ASPxButton ID="CollapseAllButton_Domino" runat="server" 
                    ClientInstanceName="collapseAll" EnableClientSideAPI="False" 
                    onclick="CollapseAllButton_Domino_Click" Text="Collapse All" Theme="Office2010Blue" 
                    Wrap="False">
                    <Image Url="~/images/icons/forbidden.png">
                    </Image>
                </dx:ASPxButton>
            </td>
        </tr>
        <tr>
            <td>
               <dx:ASPxTreeList runat="server" KeyFieldName="Id" ParentFieldName="LocId" AutoGenerateColumns="False"
                                Theme="Office2003Blue" Width="100%" ID="DominoServerTreeList" 
                    CssClass="lblsmallFont" style="margin-top: 0px">
                                <Columns>
                                    <dx:TreeListTextColumn FieldName="Name" Name="Servers"
                                        Caption="Servers  " VisibleIndex="0">
                                        <HeaderStyle CssClass="GridCssHeader"></HeaderStyle>
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:TreeListTextColumn>
                                    <dx:TreeListTextColumn FieldName="actid" Name="actid"
                                        Visible="False" VisibleIndex="1">
                                    </dx:TreeListTextColumn>
                                    <dx:TreeListTextColumn FieldName="tbl" Name="tbl"
                                        Visible="False" VisibleIndex="2">
                                    </dx:TreeListTextColumn>
                                    <dx:TreeListTextColumn FieldName="LocId" Name="LocId"
                                        Visible="False" VisibleIndex="3">
                                    </dx:TreeListTextColumn>
                                    <dx:TreeListTextColumn FieldName="ServerType" VisibleIndex="4">
                                        <HeaderStyle CssClass="GridCssHeader"></HeaderStyle>
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:TreeListTextColumn>
                                    <dx:TreeListTextColumn FieldName="Description" VisibleIndex="5">
                                        <HeaderStyle CssClass="GridCssHeader"></HeaderStyle>
                                        <CellStyle CssClass="GridCss">
                                        </CellStyle>
                                    </dx:TreeListTextColumn>
                                </Columns>
                                <Settings GridLines="Both"></Settings>
                                <SettingsBehavior AutoExpandAllNodes="True" AllowDragDrop="False"></SettingsBehavior>
                                <SettingsPager Mode="ShowPager" AlwaysShowPager="True" PageSize="20">
                                    <PageSizeItemSettings Visible="True">
                                    </PageSizeItemSettings>
                                </SettingsPager>
                                <SettingsSelection Enabled="True" AllowSelectAll="True" Recursive="True"></SettingsSelection>
                                <Styles>
                                    <LoadingPanel ImageSpacing="5px">
                                    </LoadingPanel>
                                    <AlternatingNode Enabled="True">
                                    </AlternatingNode>
                                </Styles>
                                <StylesPager>
                                    <PageNumber ForeColor="#3E4846">
                                    </PageNumber>
                                    <Summary ForeColor="#1E395B">
                                    </Summary>
                                </StylesPager>
                                <StylesEditors ButtonEditCellSpacing="0">
                                </StylesEditors>
                            </dx:ASPxTreeList>
            </td>
        </tr>
    </table>
    </ContentTemplate>
    <Triggers>        
        <asp:AsyncPostBackTrigger ControlID="DominoServerTasksApply" />
        <asp:AsyncPostBackTrigger ControlID="DominoServerTasksClear" />
    </Triggers>
    </asp:UpdatePanel>
        
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table>
        <tr>
            <%--<dx:ASPxTextBox ID="txtValue" Width="100%" runat="server" Value='<%# Eval("AttributeName")%>'></dx:ASPxTextBox>--%>
            <td id="Td5" style="color: Black" runat="server" align="left">
                <asp:Label ID="lblMessageDomino" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <div id="errorDiv3" runat="server" class="error" style="display: none">Please select at least one Task and one Server in order to proceed.
                </div>
                <div id="errorDiv4" runat="server" class="error" style="display: none">Settings for selected servers were NOT updated.
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="1">
                <dx:ASPxButton runat="server" Text="Add Task(s)" Theme="Office2010Blue"
                    Width="50px" ID="DominoServerTasksApply" OnClick="AddTasksToDominoServers_Click">
                </dx:ASPxButton>
            </td>
            <td colspan="1">
                <dx:ASPxButton runat="server" Text="Remove Task(s)" Theme="Office2010Blue"
                    Width="50px" ID="DominoServerTasksClear" OnClick="RemoveTasksFromDominoServers_Click">
                </dx:ASPxButton>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    </ContentTemplate>
    
    </asp:UpdatePanel>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
        </TabPages>
        <LoadingPanelImage Url="~/App_Themes/Glass/Web/Loading.gif">
        </LoadingPanelImage>
        <Paddings PaddingLeft="0px" />
        <ContentStyle>
            <Border BorderColor="#4986A2" />
<Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px"></Border>
        </ContentStyle>
    </dx:ASPxPageControl>

    </td>
            </tr>
</asp:Content>
