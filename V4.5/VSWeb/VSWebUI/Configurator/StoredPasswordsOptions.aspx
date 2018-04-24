<%@ Page Title="VitalSigns Plus-StoredPasswordsOptions" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="StoredPasswordsOptions.aspx.cs" Inherits="VSWebUI.WebForm25" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">

.header
{
background-color:Gray;
font-family:Arial Black;
font-size:16.5pt;font-family:Microsoft Sans Serif;font-weight:normal;
}
.header1
{
background-color:Silver;
font-family:Arial Black;
font-size:16.5pt;font-family:Microsoft Sans Serif;font-weight:normal;color:#15428B
}
.content
{
 border-style:dashed;
 background-color:Silver;
  border-color:Navy;
   border-top-style:none;
 }
</style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    <p>
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">  
    </asp:ToolkitScriptManager>  
    <asp:Accordion   
    ID="Accordion1"   
    runat="server" HeaderCssClass="header" HeaderSelectedCssClass="header1" ContentCssClass="content">  
<Panes>  
    <asp:AccordionPane ID="AccordionPane1" runat="server">  
        <Header>IBM Domino Settings</Header>  
        <Content>  
        <table>
        <tr>
            <td>
                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="200px" 
                    HeaderText="NotesID">
                <HeaderStyle HorizontalAlign="Center" />
<PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
    <table class="style1">
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" 
                    Text="Notes Program Directory Path" Width="149px" CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxTextBox ID="ASPxTextBox1" runat="server" Width="170px">
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <dx:ASPxLabel ID="ASPxLabel4" runat="server" 
                    Text="For example:C:/programfiles/Lotus/Notes" CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Notes ID file" Height="16px" CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxTextBox ID="ASPxTextBox2" runat="server" Width="170px">
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <dx:ASPxLabel ID="ASPxLabel5" runat="server" 
                    Text="For example:C:/programfiles/Lotus/Notes/Data/myname.ID" CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Notes.INI" CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxTextBox ID="ASPxTextBox3" runat="server" Width="170px">
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <dx:ASPxLabel ID="ASPxLabel6" runat="server" 
                    Text="For example:C:/programfiles/Lotus/Notes/notes.ini" CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table class="style1">
                    <tr>
                        <td>
                            <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Register Notes Password">
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxPanel ID="ASPxPanel1" runat="server" Width="200px">
                                <PanelCollection>
                                    <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                        VitalSings is using the ID:Alan Forbs. The password as been entered, tested.and 
                                        encripted for use by the backGround Monitoring Service.
                                        <br />
                                        Impotant: If you switch IDs using the Lotus Notes client. You must provide the 
                                        New password for VitalSign.
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxPanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </dx:PanelContent>
</PanelCollection>
                </dx:ASPxRoundPanel>
            </td>
            <td colspan="2" valign="top">
                <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" Width="200px" 
                    HeaderText="Other Settings">
                <HeaderStyle HorizontalAlign="Center" />
<PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
    <table class="style1">
        <tr>
            <td>
                <dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server" CheckState="Unchecked" 
                    Text="Prompt for password or sencitive operations" CssClass="lblsmallFont">
                </dx:ASPxCheckBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxCheckBox ID="ASPxCheckBox2" runat="server" CheckState="Unchecked" 
                    Text="Alert on server. ExpansionFactor" CssClass="lblsmallFont">
                </dx:ASPxCheckBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxCheckBox ID="ASPxCheckBox3" runat="server" CheckState="Unchecked" 
                    Text="SuppressMultiThrededOperations" CssClass="lblsmallFont">
                </dx:ASPxCheckBox>
            </td>
        </tr>
    </table>
    </dx:PanelContent>
</PanelCollection>
                </dx:ASPxRoundPanel>
            </td>
            <td valign="top">
                <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" Width="200px" 
                    HeaderText="OutBound Email">
                <HeaderStyle HorizontalAlign="Center" />
<PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
    <table class="style1">
        <tr>
            <td colspan="2">
                <dx:ASPxCheckBox ID="ASPxCheckBox4" runat="server" CheckState="Unchecked" 
                    Text="Alert on 'stuck' pending message" CssClass="lblsmallFont">
                </dx:ASPxCheckBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <dx:ASPxPanel ID="ASPxPanel2" runat="server" Width="200px">
                    <PanelCollection>
                        <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                            Send pending mail Alert if a specific message has been outbound mail queue 
                            longer than:
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxTextBox ID="ASPxTextBox4" runat="server" Width="40px">
                </dx:ASPxTextBox>
            </td>
            <td>
                <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="minutes" CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
        </tr>
    </table>
    </dx:PanelContent>
</PanelCollection>
                </dx:ASPxRoundPanel>
            </td>
        </tr>
        </table>
      <%-- Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.  
        Nunc viverra imperdiet enim. Fusce est. Vivamus a tellus.  
        Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Proin pharetra nonummy pede. Mauris et orci.  --%>            
        </Content>  
    </asp:AccordionPane>  
    <asp:AccordionPane ID="AccordionPane2" runat="server">  
        <Header>IBM Sametime Settings</Header>  
        <Content>  
        <table class="style1">
                    <tr>
                        <td>
                            <dx:ASPxRoundPanel ID="ASPxRoundPanel4" runat="server" Width="200px" 
                                HeaderText="IBM Sametime account">
                            <HeaderStyle HorizontalAlign="Center" />
<PanelCollection>
<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
    <table class="style1">
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="User name" CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
            <td>
                <dx:ASPxCheckBox ID="ASPxCheckBox5" runat="server" CheckState="Unchecked" 
                    Text="Collect Extended sametime Statistics" CssClass="lblsmallFont">
                </dx:ASPxCheckBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxTextBox ID="ASPxTextBox5" runat="server" Width="170px" >
                </dx:ASPxTextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <dx:ASPxButton ID="ASPxButton2" runat="server" Text="Enter sametime password">
                </dx:ASPxButton>
            </td>
            <td rowspan="3">
                <dx:ASPxPanel ID="ASPxPanel4" runat="server" Width="200px">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server" SupportsDisabledAttribute="True">
                            The &quot;Advanced&quot; sametime Statistics arecaptured by quering the statistics servlet 
                            instaled on the sametime server at <a href="http://hostname/servlets/statistics">
                            http://hostname/servlets/statistics</a><br />
                            <br />
                            Normally, calling this servlet requires authentication.<br />
                            <br />
                            For more information please google &quot;sametime statistics and Monitoring toolkit&quot;.
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <dx:ASPxPanel ID="ASPxPanel3" runat="server" Width="200px">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
                            The sametime password is stored in the Registry as encripted byte streem.
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>
            </td>
        </tr>
    </table>
    </dx:PanelContent>
</PanelCollection>
                            </dx:ASPxRoundPanel>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
        <%--Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.  
        Nunc viverra imperdiet enim. Fusce est. Vivamus a tellus.  
        Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Proin pharetra nonummy pede. Mauris et orci.  --%>
        </Content>  
    </asp:AccordionPane>  
    <asp:AccordionPane ID="AccordionPane3" runat="server">  
        <Header>License Information</Header>  
        <Content> <table>
        <tr>
            <td colspan="2">
                <dx:ASPxRoundPanel ID="ASPxRoundPanel5" runat="server" Width="200px" 
                    HeaderText="Enabled Monitoring of:">
               <HeaderStyle HorizontalAlign="Center" />
<PanelCollection>
<dx:PanelContent ID="PanelContent4" runat="server" SupportsDisabledAttribute="True">
    <table >
        <tr>
            <td>
                <dx:ASPxCheckBox ID="ASPxCheckBox6" runat="server" CheckState="Unchecked" 
                    Text="DNS Server" CssClass="lblsmallFont">
                </dx:ASPxCheckBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxCheckBox ID="ASPxCheckBox7" runat="server" CheckState="Unchecked" 
                    Text="Domino servers" CssClass="lblsmallFont">
                </dx:ASPxCheckBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxCheckBox ID="ASPxCheckBox8" runat="server" CheckState="Unchecked" 
                    Text="Notes Mailprobes" CssClass="lblsmallFont">
                </dx:ASPxCheckBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxCheckBox ID="ASPxCheckBox9" runat="server" CheckState="Unchecked" 
                    Text="Notes Database" CssClass="lblsmallFont">
                </dx:ASPxCheckBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxCheckBox ID="ASPxCheckBox10" runat="server" CheckState="Unchecked" 
                    Text="Domino Clusters" CssClass="lblsmallFont">
                </dx:ASPxCheckBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxCheckBox ID="ASPxCheckBox11" runat="server" CheckState="Unchecked" 
                    Text="Sheduled Domino Console commands" CssClass="lblsmallFont">
                </dx:ASPxCheckBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxCheckBox ID="ASPxCheckBox12" runat="server" CheckState="Unchecked" 
                    Text="Black Berry Device Probs" CssClass="lblsmallFont">
                </dx:ASPxCheckBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxCheckBox ID="ASPxCheckBox13" runat="server" CheckState="Unchecked" 
                    Text="BlackBerry MessageQueues" CssClass="lblsmallFont">
                </dx:ASPxCheckBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxCheckBox ID="ASPxCheckBox14" runat="server" CheckState="Unchecked" 
                    Text="Black Berry Enterprize Servers*" CssClass="lblsmallFont">
                </dx:ASPxCheckBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxCheckBox ID="ASPxCheckBox15" runat="server" CheckState="Unchecked" 
                    Text="BlackBerry users*" CssClass="lblsmallFont">
                </dx:ASPxCheckBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxCheckBox ID="ASPxCheckBox16" runat="server" CheckState="Unchecked" 
                    Text="URLs*" CssClass="lblsmallFont">
                </dx:ASPxCheckBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxCheckBox ID="ASPxCheckBox17" runat="server" CheckState="Unchecked" 
                    Text="MailServices*" CssClass="lblsmallFont">
                </dx:ASPxCheckBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxCheckBox ID="ASPxCheckBox18" runat="server" CheckState="Unchecked" 
                    Text="Network Devices*" CssClass="lblsmallFont">
                </dx:ASPxCheckBox>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxCheckBox ID="ASPxCheckBox19" runat="server" CheckState="Unchecked" 
                    Text="sametime*" CssClass="lblsmallFont">
                </dx:ASPxCheckBox>
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
            <td colspan="2" valign="top">
                <dx:ASPxRoundPanel ID="ASPxRoundPanel6" runat="server" Width="200px" 
                    HeaderText="License information">
               <HeaderStyle HorizontalAlign="Center" />
<PanelCollection>
<dx:PanelContent ID="PanelContent5" runat="server" SupportsDisabledAttribute="True">
    <table class="style1">
        <tr>
            <td>
                <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="Up to 40 DNS severs " CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
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
        </tr> 
       <%-- Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.  
        Nunc viverra imperdiet enim. Fusce est. Vivamus a tellus.  
        Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Proin pharetra nonummy pede. Mauris et orci.  --%>
       </table>
        </Content>  
    </asp:AccordionPane>  
</Panes>  
</asp:Accordion>  
         
        <br />
    </p>
       
</asp:Content>
