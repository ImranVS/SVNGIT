<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ManageProfiles.aspx.cs" Inherits="VSWebUI.Security.ManageProfiles" %>
<%@ MasterType virtualpath="~/Site1.Master" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx1" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx11" %>





<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function OnSelectedIndexChanged(s, e) {
          //  alert(s.GetValue());
            ProfilesGridView.GetEditor("RelatedField").PerformCallback(s.GetValue().toString());
        }
        function OnRoleTypeSelectedIndexChanged(s, e) {
            //alert(s.GetValue());
            ProfilesGridView.GetEditor("RoleType").PerformCallback(s.GetValue().toString());
        }
    function hidepopup() {

        var popup = document.getElementById('ContentPlaceHolder1_pnlAreaDtls');
        popup.style.visibility = 'hidden';

    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="pnlAreaDtls" style="height: 100%; width: 100%;visibility:hidden" runat="server" class="pnlDetails12">
                
                <table align="center" width="30%" style="height: 100%">
                    <tr>
                        <td align="center" valign="middle" style="height: auto;">
                            <table border="1" cellspacing="0" class="csline" cellpadding="2px" id="table_txt_edit" style="border-width:1px; border-style: solid;  border-collapse: collapse;  border-color: silver; background-color: #F8F8FF"  
                                width="100%">
                                <tr style="background-color:White">
                                  
                                    <td align="left">
                                      <div class="subheading">Delete Server</div>
                                    </td>
                                </tr>
                                <tr>
                                
                                    <td align="center">
                                    <table><tr><td valign="top">
                                
                                    </td><td align="center">
                                        <div style="overflow: auto; height: 60px; font-size: 12px; font-weight: normal; font-family: Arial, Helvetica, sans-serif; text-align:left; color:black; width:350px;"  id="divmsg" runat="server"></div>
                                              <asp:Button ID="btnok1" runat="server"  OnClick="btn_OkClick" OnClientClick="hidepopup()" Text="OK" Width="50px" Font-Names="Arial" Font-Size="Small" />
                                              <asp:Button ID="btncancel1" runat="server" OnClick="btn_CancelClick"  OnClientClick="hidepopup()" Text="Cancel"  Width="70px" Font-Names="Arial" Font-Size="Small" />
                                           
                                        </td>
                                        
                                        </tr></table>
                                        
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Manage Profiles Master" 
                                                    Font-Bold="True" Font-Size="Large" 
                            style="color: #000000; font-family: Verdana"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="color:Black" id="tdmsg" runat="server" align="center">
            </td>
                </tr>
            </table>
            
    <dx:ASPxGridView runat="server" KeyFieldName="ID" 
    AutoGenerateColumns="False" ID="ProfilesGridView" ClientInstanceName="ProfilesGridView" 
    onrowdeleting="ProfilesGridView_RowDeleting" 
    onrowinserting="ProfilesGridView_RowInserting" 
    onrowupdating="ProfilesGridView_RowUpdating" 
     oncelleditorinitialize="ProfilesGridView_CellEditorInitialize"
        OnAutoFilterCellEditorInitialize="ProfilesGridView_AutoFilterCellEditorInitialize" 
        Theme="Office2003Blue" EnableTheming="True"  OnPageSizeChanged="ProfilesGridView_PageSizeChanged"
        oncustomerrortext="ProfilesGridView_CustomErrorText">
        <Columns>
            <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" FixedStyle="Left"
                                        VisibleIndex="0" >
                                        <EditButton Visible="True">
                                            <Image Url="../images/edit.png">
                                            </Image>
                                        </EditButton>
                                        <NewButton Visible="True">
                                            <Image Url="../images/icons/add.png">
                                            </Image>
                                        </NewButton>
                                        <DeleteButton Visible="False">
                                            <Image Url="../images/delete.png">
                                            </Image>
                                        </DeleteButton>
                                        <CancelButton Visible="True">
                                            <Image Url="~/images/cancel.gif">
                                            </Image>
                                        </CancelButton>
                                        <UpdateButton Visible="True">
                                            <Image Url="~/images/update.gif">
                                            </Image>
                                        </UpdateButton>
                                        <CellStyle CssClass="GridCss1">
                                            <Paddings Padding="3px" />
                                        </CellStyle>
                                        <ClearFilterButton Visible="True">
                                            <Image Url="~/images/clear.png">
                                            </Image>
                                        </ClearFilterButton>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                    </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn Caption="Delete" VisibleIndex="1" CellStyle-HorizontalAlign="Center" >
            <DataItemTemplate>
            
              <asp:ImageButton ID="btndele" runat="server" ImageUrl="../images/delete.png" Width="15px" Height="15px" CommandName="Delete" CommandArgument='<%#Eval("ID") %>' AlternateText='<%#Eval("AttributeName") %>' ToolTip="Delete" OnClick="btn_Click"/>
           
           
              
              
            </DataItemTemplate>
            <EditFormSettings Visible="False"/>
            <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss1"></CellStyle>
<CellStyle HorizontalAlign="Center"></CellStyle>
         </dx:GridViewDataTextColumn>
            <dx:GridViewDataComboBoxColumn Caption="ServerType" FieldName="ServerType" VisibleIndex="3" UnboundType="String">
                <PropertiesComboBox  TextField="ServerType"
                    ValueField="ServerType">
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </PropertiesComboBox>
                <Settings AllowAutoFilter="True"  />
                <HeaderStyle CssClass="GridCssHeader" />
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <EditCellStyle CssClass="GridCss"></EditCellStyle><CellStyle CssClass="GridCss"></CellStyle>
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn FieldName="RelatedTable" VisibleIndex="5" Caption="Related Table" UnboundType="String">
                
                 <PropertiesComboBox>
                    <Items>
                        <dx:ListEditItem Text="Servers" Value="Servers" />
                        <dx:ListEditItem Text="DominoServers" Value="DominoServers" />
                        <dx:ListEditItem Text="SametimeServers" Value="SametimeServers" />
                        <dx:ListEditItem Text="Network Devices" Value="Network Devices" />
                        <dx:ListEditItem Text="MailServices" Value="MailServices" />
                        <%--<dx:ListEditItem Text="SharePoint" Value="BlackBerryServers" />
                        <dx:ListEditItem Text="Exchange" Value="ExchangeSettings" />--%>
                        <dx:ListEditItem Text="Exchange" Value="ExchangeSettings" />
                        <dx:ListEditItem Text="URL" Value="URLs" /> 
                        <dx:ListEditItem Text="BES Servers" Value="BlackBerryServers" />
                        <dx:ListEditItem Text="NotesDatabases" Value="NotesDatabases" />
                    </Items>
                    <ClientSideEvents SelectedIndexChanged="function(s,e){OnSelectedIndexChanged(s,e)}" />
                </PropertiesComboBox>
                <Settings AutoFilterCondition="Contains" />
                <HeaderStyle CssClass="GridCssHeader" />
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
            </dx:GridViewDataComboBoxColumn>
            <dx:gridviewdatatextcolumn 
                UnboundType="String" FieldName="AttributeName" 
                VisibleIndex="2" Caption="Attribute Name">
              <PropertiesTextEdit>
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss">
                </CellStyle>

            </dx:gridviewdatatextcolumn>
            <dx:GridViewDataTextColumn Caption="Unit of Measurement" 
                FieldName="UnitOfMeasurement" 
                UnboundType="String" VisibleIndex="4">
                <PropertiesTextEdit>
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Default Value" 
                FieldName="DefaultValue" 
                UnboundType="String" VisibleIndex="6">
                <PropertiesTextEdit>
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
                <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataTextColumn>    
            <dx:GridViewDataComboBoxColumn Caption="Related Field" FieldName="RelatedField" VisibleIndex="7" UnboundType="String">
                
               <Settings AllowAutoFilter="True" AutoFilterCondition="Contains" />
                <EditCellStyle CssClass="GridCss">
                </EditCellStyle>
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
                <HeaderStyle CssClass="GridCssHeader" />
                <CellStyle CssClass="GridCss">
                </CellStyle>
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn FieldName="RoleType" VisibleIndex="8" Caption="Role Type" UnboundType="String">
                
                 <PropertiesComboBox>
                    <Items>
                        <dx:ListEditItem Text="No Role" Value="" />
                        <dx:ListEditItem Text="CAS" Value="CAS" />
                        <dx:ListEditItem Text="EDGE" Value="EDGE" />
                        <dx:ListEditItem Text="MailBox" Value="MailBox" />
                        <dx:ListEditItem Text="HUB" Value="HUB" />
                    </Items>
                    <ClientSideEvents SelectedIndexChanged="function(s,e){OnRoleTypeSelectedIndexChanged(s,e)}" />
                </PropertiesComboBox>
                <Settings AutoFilterCondition="Contains" />
                <HeaderStyle CssClass="GridCssHeader" />
                <EditFormCaptionStyle CssClass="GridCss">
                </EditFormCaptionStyle>
            </dx:GridViewDataComboBoxColumn>
        </Columns>
        <SettingsBehavior ConfirmDelete="True">
        </SettingsBehavior>
        <Settings ShowFilterRow="True" />
        <SettingsText ConfirmDelete="Are you sure you want to delete this Profile?">
        </SettingsText>
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
         <SettingsPager PageSize="10" SEOFriendly="Enabled" >
            <PageSizeItemSettings Visible="true" />
        </SettingsPager>
    </dx:ASPxGridView>
   
                                    <dx:ASPxPopupControl ID="NavigatorPopupControl" runat="server" ClientInstanceName="popup1" 
                                        HeaderText="Information" Modal="True" PopupHorizontalAlign="WindowCenter" 
                                        PopupVerticalAlign="WindowCenter" Theme="Glass">
                                        <ContentCollection>
                                            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
                                                <table class="style1">
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxLabel ID="MsgLabel" runat="server" ClientInstanceName="poplbl">
                                                            </dx:ASPxLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            </td>

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
    
</asp:Content>

