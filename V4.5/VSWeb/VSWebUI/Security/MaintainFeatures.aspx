<%@ Page Title="VitalSigns Plus - Maintain Features" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"
    CodeBehind="MaintainFeatures.aspx.cs" Inherits="VSWebUI.Security.MaintainFeatures" %>
	<%@ MasterType virtualpath="~/Site1.Master" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>










<%@ Register assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dx" %>







<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function HidePopup() {
            var popup = document.getElementById('ContentPlaceHolder1_pnlAreaDtls');
            popup.style.visibility = 'hidden';
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="pnlAreaDtls" style="height: 100%; width: 100%;visibility:hidden;" runat="server" class="pnlDetails12">                
        <table align="center" width="30%" style="height: 100%">
            <tr>
                <td align="center" valign="middle" style="height: auto;">
                    <table border="1" cellspacing="0" class="csline" cellpadding="2px" id="table_txt_edit" style="border-width:1px; border-style: solid;  border-collapse: collapse;  border-color: silver; background-color: #F8F8FF"  
                        width="100%">
                        <tr style="background-color:White">                                  
                            <td align="left">
                                <div class="subheading">Delete Feature</div>
                            </td>
                        </tr>
                        <tr>                                
                            <td align="center">
                                <table cellpadding="2px">
                                    <tr>
                                        
                                        <td align="center">
                                            <div style="overflow: auto; height: 60px; font-size: 12px; font-weight: normal; font-family: Arial, Helvetica, sans-serif; text-align:left; color:black; width:350px;"  id="divmsg" runat="server"></div>
                                            <asp:Button ID="bttnOK" runat="server" OnClick="bttnOK_Click" 
                                                OnClientClick="HidePopup()" Text="OK" Width="50px" Font-Names="Arial" Font-Size="Small" />
                                            <asp:Button ID="bttnCancel" runat="server" OnClick="bttnCancel_Click" 
                                                OnClientClick="HidePopup()" Text="Cancel"  Width="70px" Font-Names="Arial" Font-Size="Small" />                                           
                                        </td>                                        
                                    </tr>
                                </table>                                        
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>

   <%-- <dx:ASPxRoundPanel ID="FeaturesRoundPanel" runat="server" 
        CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
        CssPostfix="Glass" 
        GroupBoxCaptionOffsetY="-24px" HeaderText="Features" 
        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
        Width="493px" Height="309px">
        <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
        <HeaderStyle Height="23px">
        <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
        </HeaderStyle>
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">--%>
                <table class="style1">
                     <tr>
                   <td>
                   <asp:Label ID="Label1" runat="server" Text="Maintain Features" 
                                                    Font-Bold="True" Font-Size="Large" 
                           style="color: #000000; font-family: Verdana"></asp:Label>
                   </td>
                </tr>
                     <tr>
                   <td id="tdmsgforServertype" runat="server" style="color:Red; font-size:15px" align="center">
                            </td>
                </tr>
                    <tr>
                        <td>
                            <dx:ASPxGridView runat="server" 
                                CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                CssPostfix="Office2010Blue" KeyFieldName="ID" AutoGenerateColumns="False" 
                                ID="FeaturesGridView" OnRowDeleting="FeaturesGridView_RowDeleting" 
                                OnRowInserting="FeaturesGridView_RowInserting" OnPageSizeChanged="FeaturesGridView_PageSizeChanged" 
                                OnRowUpdating="FeaturesGridView_RowUpdating" Theme="Office2003Blue">
                                <Columns>
                                    <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" 
                                        FixedStyle="Left" VisibleIndex="0" Width="10px">
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
                                    <dx:GridViewDataTextColumn Caption="Delete" VisibleIndex="1" CellStyle-HorizontalAlign="Center" Width="30px" CellStyle-VerticalAlign="Middle">
                                        <DataItemTemplate>            
                                            <asp:Label ID="lblFeature" runat="server" Text='<%#Eval("Name") %>' Visible="False"></asp:Label>
                                            <asp:ImageButton ID="bttnDelete" runat="server" ImageUrl="../images/delete.png" Width="15px" Height="15px" CommandName="Delete" CommandArgument='<%#Eval("ID") %>' AlternateText='<%#Eval("Name") %>' ToolTip="Delete" OnClick="bttnDelete_Click"/>
                                        </DataItemTemplate>
                                        <EditFormSettings Visible="False"/>
                                        <EditCellStyle CssClass="GridCss"></EditCellStyle>
                                        <EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle>
                                        <HeaderStyle CssClass="GridCssHeader" />
                                        <CellStyle CssClass="GridCss1"></CellStyle>
                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True" Visible="False" 
                                    VisibleIndex="2">
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="Name" VisibleIndex="3">
                                        <PropertiesTextEdit>
                                            <ValidationSettings CausesValidation="True" SetFocusOnError="True">
                                                <RequiredField ErrorText="Enter Feature" IsRequired="True" />
                                            </ValidationSettings>
                                        </PropertiesTextEdit>
                                        <Settings AutoFilterCondition="Contains" />
                                        <EditCellStyle CssClass="GridCss"></EditCellStyle><EditFormCaptionStyle CssClass="GridCss"></EditFormCaptionStyle><HeaderStyle CssClass="GridCssHeader" /><CellStyle CssClass="GridCss"></CellStyle>
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                                <Settings ShowFilterRow="True" />
                                <Images SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css">
                                    <LoadingPanelOnStatusBar Url="~/App_Themes/Office2010Blue/GridView/Loading.gif"></LoadingPanelOnStatusBar>

                                    <LoadingPanel Url="~/App_Themes/Office2010Blue/GridView/Loading.gif"></LoadingPanel>
                                </Images>
                                <ImagesFilterControl>
                                    <LoadingPanel Url="~/App_Themes/Office2010Blue/GridView/Loading.gif"></LoadingPanel>
                                </ImagesFilterControl>
                                <Styles>
                                    <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
                                    <LoadingPanel ImageSpacing="5px"></LoadingPanel>
                                     <AlternatingRow CssClass="GridAltRow" Enabled="True">
                        </AlternatingRow>
                                </Styles>
                                <StylesPager>
                                    <PageNumber ForeColor="#3E4846"></PageNumber>
                                    <Summary ForeColor="#1E395B"></Summary>
                                </StylesPager>
                                <StylesEditors ButtonEditCellSpacing="0">
                                    <ProgressBar Height="21px"></ProgressBar>
                                </StylesEditors>
                                    <SettingsBehavior ConfirmDelete="True" />
                                    <SettingsPager PageSize="10" SEOFriendly="Enabled" >
                                    <PageSizeItemSettings Visible="true" />
                                    </SettingsPager>
                            </dx:ASPxGridView>
                        </td>
                    </tr>
                </table>
      <%--  </dx:PanelContent>
    </PanelCollection>
    </dx:ASPxRoundPanel>--%>
    <dx:ASPxPopupControl ID="ErrorMessagePopupControl" runat="server" 
    AllowDragging="True" ClientInstanceName="pcErrorMessage" 
    CloseAction="CloseButton" CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
    CssPostfix="Glass" EnableAnimation="False" EnableViewState="False" 
    HeaderText="Validation Failure" Height="150px" Modal="True" 
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" Width="300px">
    <LoadingPanelImage Url="~/App_Themes/Glass/Web/Loading.gif">
    </LoadingPanelImage>
    <HeaderStyle>
    <Paddings PaddingLeft="10px" PaddingRight="6px" PaddingTop="1px" />
    </HeaderStyle>
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
            <dx:ASPxPanel ID="Panel1" runat="server" DefaultButton="btOK">
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent5" runat="server">
                        <div style="min-height: 70px;">
                            <dx:ASPxLabel ID="ErrorMessageLabel" runat="server">
                            </dx:ASPxLabel>
                        </div>
                        <div>
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align="left">
                                        <dx:ASPxButton ID="ValidationOkButton" runat="server" AutoPostBack="False" 
                                            CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                            CssPostfix="Office2010Blue" 
                                            SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="OK" 
                                            Width="80px">
                                            <ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }" />
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="ValidationUpdatedButton" runat="server" AutoPostBack="False" 
                                            CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css" 
                                            CssPostfix="Office2010Blue"  
                                            SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css" Text="OK" 
                                            Visible="False" Width="80px" OnClick="ValidationUpdatedButton_Click">
                                            <ClientSideEvents Click="function(s, e) { pcErrorMessage.Hide(); }" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxPanel>
        </dx:PopupControlContentControl>
    </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="NavigatorPopupControl" runat="server" ClientInstanceName="popup1" 
    HeaderText="Information" Modal="True" PopupHorizontalAlign="WindowCenter" 
    PopupVerticalAlign="WindowCenter" Theme="Glass">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">
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
                            <dx:ASPxButton ID="OKButton" runat="server" OnClick="subbttnOK_Click" Text="OK" 
                                Theme="Office2010Blue">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>