<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailStatus.ascx.cs" Inherits="VSWebUI.Controls.MailStatus" %>
<link href="../css/control.css" type="text/css" rel="Stylesheet" />

    <script type="text/javascript" language="javascript">
        function Visible() {
            var s = document.getElementById('div1').style.visibility = 'visible';
            //s.style.visibility=
        }

        function InVisible() {
            var s = document.getElementById('div1').style.visibility = 'hidden';
        }

    </script>
    <table id="tbl" runat="server" onmouseover="javascript:Visible();" onmouseout="javascript:InVisible();">
        <tr>
            <td class="circle1">
            </td>
            <td class="circle2">
            </td>
            <td class="circle3">
            </td>
        </tr>
    </table>
    <div id="div1" style="z-index:9999; visibility:hidden;width:100px;height:auto;padding:5px; border:1px;background-color:#ccc; font-size:11px;font-family:Arial;">
        <asp:Label ID="lblPendingMail" runat="server"></asp:Label>
        <asp:Label ID="lblDeadMail" runat="server"></asp:Label>
        <asp:Label ID="lblHeldMail" runat="server"></asp:Label>
    </div>