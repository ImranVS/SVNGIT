<%@ Page Title="VitalSigns Plus-Proxy" Language="C#" MasterPageFile="~/Site1.Master"AutoEventWireup="true" CodeBehind="Proxy.aspx.cs" Inherits="VSWebUI.Configurator.Proxy" %>
<%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="style1">
        <tr>
            <td>
                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" 
                    CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                    CssPostfix="Glass" 
                    GroupBoxCaptionOffsetY="-24px" 
                    SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
                    Width="400px" HeaderText="Proxy Settings">
                    <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                    <HeaderStyle Height="23px">
                    <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
<Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
                    </HeaderStyle>
                    <PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
    <table class="style1">
        <tr>
            <td>
                <dx:ASPxCheckBox ID="ProxyCheckBox" runat="server" CheckState="Unchecked" 
                    Text="Use Proxy Server" CssClass="lblsmallFont" 
                    OnCheckedChanged="ProxyCheckBox_CheckedChanged">
                </dx:ASPxCheckBox>
            </td>
        </tr>
        <tr>
            <td>
                <table >
                    <tr>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Proxy Port:" CssClass="lblsmallFont">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxTextBox ID="ProxyPortTextBox" runat="server" Width="40px">
                                <MaskSettings Mask="&lt;0..999999&gt;" />
<MaskSettings Mask="&lt;0..999999&gt;"></MaskSettings>
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td >
                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Proxy Server:" CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td  >
                <dx:ASPxTextBox ID="ProxyServerTextBox" runat="server" Width="170px">
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
            <td >
                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Proxy User:" CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
        </tr>
        <tr>
            <td >
                <dx:ASPxTextBox ID="ProxyUserTextBox" runat="server" Width="170px">
                </dx:ASPxTextBox>
            </td>
        </tr>
        <tr>
		    <td>
			<table>
			<tr>
			<td>
                <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Proxy Password:" 
                    CssClass="lblsmallFont">
                </dx:ASPxLabel>
            </td>
		<td>
			<asp:LinkButton ID="LinkButton1" runat="server" 
				OnClientClick="popup2.ShowAtElement(this); return false;" Visible="False" CssClass="lblsmallFont"><u>Edit password</u></asp:LinkButton>
				</td>
				</tr>
            </table>
		</td>
    </tr>
	<tr>
		  <td>
                <dx:ASPxTextBox ID="ProxyPasswordTextBox" runat="server" Width="170px" 
                    Password="True" Visible="false">
					  <validationsettings ErrorDisplayMode="ImageWithTooltip" ErrorText="Please enter Password">
						<RequiredField ErrorText="Please enter Password"	IsRequired="true" />
                          </validationsettings>
                </dx:ASPxTextBox>
            </td>
		
	</tr>
		<tr>
		<td>
		<dx:aspxpopupcontrol ID="ASPxPopupControl2" runat="server" HeaderText="Edit password" Width="307px" ClientInstanceName="popup2" CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                    CssPostfix="Glass" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"  Theme="MetropolisBlue"     GroupBoxCaptionOffsetY="-24px" >
            <contentcollection>
                <dx:popupcontrolcontentcontrol ID="Popupcontrolcontentcontrol3" runat="server">
                <table>
                  <tr>
                    <td>
					 <dx:ASPxLabel ID="newpwdlb" runat="server" Text="Enter new password:" 
                    CssClass="lblsmallFont">
                </dx:ASPxLabel>
					</td>
                    <td>
                      <dx:aspxtextbox ID="npsw" runat="server" Password="True"  ClientInstanceName="npsw">
                       <%-- <clientsideevents Validation="function(s, e) {e.isValid = (s.GetText().length>5)}" />--%>
						<clientsideevents Validation="function(s, e) {e.isValid = (s.GetText())}" />
<%--<ClientSideEvents Validation="function(s, e) {e.isValid = (s.GetText().length&gt;5)}"></ClientSideEvents>--%>


                          <validationsettings ErrorDisplayMode="ImageWithTooltip" ErrorText="Please enter Password">
						<RequiredField ErrorText="Please enter Password"	IsRequired="true" />
                          </validationsettings>
                      </dx:aspxtextbox>
                    </td>
                  </tr>
                  <tr>
                    <td>
					 <dx:ASPxLabel ID="Confirmnewpwdlb" runat="server" Text="Confirm new password:" 
                    CssClass="lblsmallFont">
                </dx:ASPxLabel>

					</td>
                    <td>
                      <dx:aspxtextbox ID="cnpsw" runat="server" Password="True" ClientInstanceName="cnpsw">
                          <clientsideevents Validation="function(s, e) {e.isValid = (s.GetText() == npsw.GetText());}" />

<ClientSideEvents Validation="function(s, e) {e.isValid = (s.GetText() == npsw.GetText());}"></ClientSideEvents>

                          <validationsettings ErrorDisplayMode="ImageWithTooltip" 
							  ErrorText="Please enter Password" 
							  display="Dynamic">
							  <RequiredField ErrorText="Please enter Password"	IsRequired="true" />
						    </validationsettings>
                      </dx:aspxtextbox>
                    </td>
                  </tr>
                </table>
				
				<dx:aspxbutton ID="confirmButton" runat="server" Text="Ok" AutoPostBack="False"  CausesValidation="true" OnClick="confirmButton_Click"  CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
                                    CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css">
				 </dx:aspxbutton>
                  
                
                </dx:popupcontrolcontentcontrol>
            </contentcollection>
        </dx:aspxpopupcontrol>
		</td>
		</tr>
    </table>
                        </dx:PanelContent>
</PanelCollection>
                </dx:ASPxRoundPanel>
            
    
        <tr>
            <td>
                 <table>
                        <tr>
                            <td>
                                <dx:ASPxButton ID="FormOkButton" runat="server" Text="Ok" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
                                    CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
                                    Width="75px" Height="30px" onclick="FormOkButton_Click" >
                                </dx:ASPxButton>
                            </td>
                            <td>
                                <dx:ASPxButton ID="FormCancelButton" runat="server" Text="Cancel" CssFilePath="~/App_Themes/Office2010Blue/{0}/styles.css"
                                    CssPostfix="Office2010Blue" SpriteCssFilePath="~/App_Themes/Office2010Blue/{0}/sprite.css"
                                    Width="75px" Height="30px" onclick="FormCancelButton_Click">
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>&nbsp;</td>
        </tr>
    </table>
</asp:Content>
