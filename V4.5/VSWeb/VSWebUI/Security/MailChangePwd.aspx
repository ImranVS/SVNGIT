<%@ Page Title="VitalSigns Plus - Change Password" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="MailChangePwd.aspx.cs" Inherits="VSWebUI.Security.MailChangePwd" %>
<%@ MasterType virtualpath="~/Site1.Master" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
        <div class="header" id="servernamelbldisp" runat="server">Reset Password</div>
    </td>
    </tr>
    <tr>
                        <td>
                            <div id="successDiv" runat="server" class="alert alert-success" style="display: none">Password was reset successully.
                            <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                </div>
                        </td>
                    </tr>
            <tr>
                <td>
                    <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server"
                        Width="100%" HeaderText="Details" Theme="Glass">
                        <PanelCollection>
<dx:PanelContent runat="server">
    <table class="navbarTbl">
        <tr>
            <td>
                <dx:ASPxLabel ID="unamedispLbl" runat="server" CssClass="lblsmallFont" 
                    Text="User Name:" Wrap="False">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxLabel ID="unameLbl" runat="server" CssClass="lblsmallFont" Wrap="False">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="lblsmallFont" 
                    Text="Send e-mail notification:">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxCheckBox ID="chkSendMail" runat="server" Text="Yes">
                </dx:ASPxCheckBox>
            </td>
        </tr>
    </table>
                            </dx:PanelContent>
</PanelCollection>
                    </dx:ASPxRoundPanel>
                </td>
            </tr>
        </table>
        <table>
    <tr>
        <td>
        <div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">Passwords don't match. Please make sure you enter the same password in the New and Confirm Password fields.
        <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
        </div>
                                                            <dx:ASPxLabel runat="server" 
                ForeColor="Red" ID="ErrorMsg"></dx:ASPxLabel>

        </td>
    </tr>
    <tr>
        <td>
            <table>
                <tr>
                    <td>
                                                                        <dx:ASPxButton ID="SaveButton" runat="server" CssClass="sysButton"
                                                                            OnClick="SaveButton_Click" 
                                                                            Text="OK">
                                                                        </dx:ASPxButton>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxButton ID="CancelButton" runat="server" CausesValidation="False" CssClass="sysButton"
                                                                            OnClick="CancelButton_Click" 
                                                                            Text="Cancel">
                                                                        </dx:ASPxButton>
                                                                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Content>
