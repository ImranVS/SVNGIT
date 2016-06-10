<%@ Page Language="C#" Title="VitalSigns Plus - Script Definitions" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ScriptDefGrid.aspx.cs" Inherits="VSWebUI.Configurator.ScriptDefGrid" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>





<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $('.alert-success').delay(10000).fadeOut("slow", function () {
        });
    });
    //5/21/2015 NS added for VSPLUS-1771
    var visibleIndex;
    function OnCustomButtonClick(s, e) {
        visibleIndex = e.visibleIndex;

        if (e.buttonID == "deleteButton")
            ScriptDefGridView.GetRowValues(e.visibleIndex, 'ScriptName', OnGetRowValues);

        function OnGetRowValues(values) {
            var id = values[0];
            var name = values[1];
            var OK = (confirm('Are you sure you want to delete the script definition - ' + values + '?'))
            if (OK == true) {
                ScriptDefGridView.DeleteRow(visibleIndex);
            }
        }
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table>
        <tr>
            <td>
                <div class="header" id="servernamelbldisp" runat="server">Script Definitions</div>
                <dx:ASPxButton ID="NewButton" runat="server" Text="New" CssClass="sysButton" 
                    onclick="NewButton_Click">
                    <Image Url="~/images/icons/add.png">
                                            </Image>
                </dx:ASPxButton>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxGridView ID="ScriptDefGridView" runat="server" AutoGenerateColumns="False" 
                    EnableTheming="True" Theme="Office2003Blue" KeyFieldName="ID" ClientInstanceName="ScriptDefGridView"
                    onhtmlrowcreated="ScriptDefGridView_HtmlRowCreated" 
                    onrowdeleting="ScriptDefGridView_RowDeleting">
                    <ClientSideEvents CustomButtonClick="OnCustomButtonClick" />
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="Script Name" FieldName="ScriptName" 
                            VisibleIndex="3">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Command" FieldName="ScriptCommand" 
                            VisibleIndex="4">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" Visible="False" 
                            VisibleIndex="5">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewCommandColumn ButtonType="Image" Caption="Actions" VisibleIndex="0" 
                            Width="50px">
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
                            <ClearFilterButton Visible="True">
                                <Image Url="~/images/clear.png">
                                </Image>
                            </ClearFilterButton>
                            <HeaderStyle CssClass="GridCssHeader1" />
                            <CellStyle CssClass="GridCss1">
                            </CellStyle>
                        </dx:GridViewCommandColumn>
                        <dx:GridViewCommandColumn ButtonType="Image" Caption="Delete" VisibleIndex="1" 
                            Width="60px">
                            <CustomButtons>
                                <dx:GridViewCommandColumnCustomButton ID="deleteButton" Text="Delete">
                                    <Image Url="~/images/delete.png">
                                    </Image>
                                </dx:GridViewCommandColumnCustomButton>
                            </CustomButtons>
                            <HeaderStyle CssClass="GridCssHeader1" />
                            <CellStyle CssClass="GridCss1">
                            </CellStyle>
                        </dx:GridViewCommandColumn>
                        
                    </Columns>
                    <Styles>
                        <AlternatingRow CssClass="GridCssAltrow">
                        </AlternatingRow>
                        <Header CssClass="GridCssHeader">
                        </Header>
                        <Cell CssClass="GridCss">
                        </Cell>
                    </Styles>
                </dx:ASPxGridView>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" HeaderText="Warning" 
                    Theme="MetropolisBlue" Modal="True" PopupHorizontalAlign="WindowCenter" 
                    PopupVerticalAlign="WindowCenter" Width="300px">
                    <ContentCollection>
<dx:PopupControlContentControl runat="server">
    <table class="navbarTbl">
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" 
                    Text="There are Alert Definitions set to use the script you are trying to delete. Please make changes to the alert definitions prior to deleting the script.">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxButton ID="ASPxButton1" runat="server" OnClick="ASPxButton1_Click" 
                    Text="OK" Theme="Office2010Blue">
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