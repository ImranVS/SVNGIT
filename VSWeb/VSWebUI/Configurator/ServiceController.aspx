<%@ Page Title="VitalSigns Plus - Service Controller" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ServiceController.aspx.cs" Inherits="VSWebUI.WebForm27" %><%@ Register assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.alert-success').delay(10000).fadeOut("slow", function () {
            });
            $('.alert-danger').delay(10000).fadeOut("slow", function () {
            });
        });
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
      .tblHeader
      {
          background-color: #045FB4;   
          color: White;
          font-weight: normal;
          font-family:Arial, Helvetica, sans-serif;
          text-align: center;
          padding: 5px 5px 5px 5px;
      }
      .tblTxt
      {
          padding: 0px 2px 0px 2px;
      }
    .style1
    {
        height: 76px;
    }
    #tbl
    {
        margin-bottom: 15px;
    }
</style>
    <div class="header" id="servernamelbldisp" runat="server">Service Controller</div>
   <table width="90%">
        <tr>
            <td>
                <div id="infoDiv" runat="server" class="info" style="display: block">In general, you only need to start and stop the VitalSigns Master Service, which should be set as Automatic.  The master service will start and stop the other services as needed, which should all be set as Manual.
</div>
<div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">Error
</div>    
            </td>
        </tr>
        <tr>
            <td>
<dx:ASPxRoundPanel ID="ServiceControllerRoundPanel" runat="server" 
                                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                        CssPostfix="Glass" 
                                        GroupBoxCaptionOffsetY="-24px" HeaderText="Service Controller Details" 
                                        SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css" 
                                        Width="100%" Height="250px">
                                        <ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
<ContentPaddings PaddingLeft="4px" PaddingTop="10px" PaddingBottom="10px"></ContentPaddings>

                                        <HeaderStyle Height="23px">
                                        <Paddings PaddingBottom="0px" PaddingLeft="2px" PaddingTop="0px" />
<Paddings PaddingLeft="2px" PaddingTop="0px" PaddingBottom="0px"></Paddings>
                                        </HeaderStyle>
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent1" runat="server">

    <table class="style1" id="tbl">
       
        <tr>
            <td valign="top">
                <table border="1" style="border-collapse: collapse; border-color: #D8D8D8;">
                    <tr>
                        <td class="tblHeader">
                            <div style="text-align: center">
                            <dx:ASPxLabel ID="ASPxLabel15" runat="server" CssClass="tblHeader" 
                                Text="Service">
                            </dx:ASPxLabel>
                            </div>
                        </td>
                        <td class="tblHeader">
                            <div style="text-align: center">
                                <dx:ASPxLabel ID="ASPxLabel20" runat="server" CssClass="tblHeader"
                                Text="Status">
                            </dx:ASPxLabel>
                            </div>
                            
                        </td>
                        <td class="tblHeader">
                            <div style="text-align: center">
                            <dx:ASPxLabel ID="ASPxLabel22" runat="server" CssClass="tblHeader"
                                Text="Last Started">
                            </dx:ASPxLabel>
                            </div>
                        </td>
                        <td class="tblHeader">
                            <dx:ASPxLabel ID="ASPxLabel23" runat="server" CssClass="tblHeader" 
                                Text="Description">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr id="trMaster" runat="server">
                        <td align="center">
                            <asp:Label ID="ASPLabel24" runat="server" CssClass="lblsmallFont"
                                Text="Master Service"></asp:Label>
                        </td>
                        <td>
                            <div style="text-align: center">
                            <dx:ASPxLabel ID="lblS4" runat="server" CssClass="tblTxt">
                            </dx:ASPxLabel>
                            </div>
                        </td>
                        <td>
                            <div style="text-align: center">
                            <dx:ASPxLabel ID="lblStarted4" runat="server" CssClass="tblTxt" Wrap="False">
                            </dx:ASPxLabel>
                            </div>
                        </td>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel25" runat="server" CssClass="tblTxt" 
                                Text="This service oversees and manages all the other VitalSigns services.">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr id="trDomino" runat="server" visible="false">
                        <td align="center">
                            <asp:Label ID="ASPLabel26" runat="server" CssClass="lblsmallFont"
                                Text="VitalSigns for Domino"></asp:Label>
                        </td>
                        <td align="center">
                            <dx:ASPxLabel ID="lblS1" runat="server" CssClass="tblTxt">
                            </dx:ASPxLabel>
                        </td>
                        <td align="center">
                            <dx:ASPxLabel ID="lblStarted1" runat="server" CssClass="tblTxt" Wrap="False">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel27" runat="server" CssClass="tblTxt" 
                                Text="This service provides monitoring for IBM Domino servers and Notes databases.">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr id="trMicrosoft" runat="server" visible="false">
                        <td align="center">
                            <asp:Label ID="ASPLabel38" runat="server" CssClass="lblsmallFont"
                                Text="VitalSigns for Microsoft"></asp:Label>
                        </td>
                        <td align="center">
                            <dx:ASPxLabel ID="lblS9" runat="server" CssClass="tblTxt">
                            </dx:ASPxLabel>
                        </td>
                        <td align="center">
                            <dx:ASPxLabel ID="lblStarted9" runat="server" CssClass="tblTxt" Wrap="False">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel ID="ASPLabel39" runat="server" CssClass="tblTxt" 
                                Text="This service provides monitoring for Microsoft servers.">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr id="trSharePoint" runat="server" visible="false">
                        <td align="center">
                            <asp:Label ID="ASPLabel40" runat="server" CssClass="lblsmallFont"
                                Text="VitalSigns for SharePoint"></asp:Label>
                        </td>
                        <td align="center">
                            <dx:ASPxLabel ID="lblS10" runat="server" CssClass="tblTxt">
                            </dx:ASPxLabel>
                        </td>
                        <td align="center">
                            <dx:ASPxLabel ID="lblStarted10" runat="server" CssClass="tblTxt" Wrap="False">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel ID="ASPLabel41" runat="server" CssClass="tblTxt" 
                                Text="This service provides monitoring forSharePoint servers.">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr id="trCore" runat="server">
                        <td align="center">
                            <asp:Label ID="ASPLabel28" runat="server" CssClass="lblsmallFont"
                                Text="Core Features"></asp:Label>
                        </td>
                        <td align="center">
                            <dx:ASPxLabel ID="lblS6" runat="server" CssClass="tblTxt">
                            </dx:ASPxLabel>
                        </td>
                        <td align="center">
                            <dx:ASPxLabel ID="lblStarted6" runat="server" CssClass="tblTxt">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel29" runat="server" CssClass="tblTxt" 
                                Text="This service provides monitoring for industry standard protocols, such as URLs, mail services, and network devices. ">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr id="trAlerting" runat="server">
                        <td align="center">
                            <asp:Label ID="ASPLabel30" runat="server" CssClass="lblsmallFont"
                                Text="Alerting"></asp:Label>
                        </td>
                        <td align="center">
                            <dx:ASPxLabel ID="lblS7" runat="server" CssClass="tblTxt">
                            </dx:ASPxLabel>
                        </td>
                        <td align="center">
                            <dx:ASPxLabel ID="lblStarted7" runat="server" CssClass="tblTxt">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel31" runat="server" CssClass="tblTxt" 
                                Text="This service sends alerts when trouble conditions are detected by the other services. ">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr id="trRemote" runat="server" visible="false">
                        <td align="center">
                            <asp:Label ID="ASPLabel32" runat="server" CssClass="lblsmallFont"
                               Text="Remote Console Commands" Wrap="False"></asp:Label>
                        </td>
                        <td align="center">
                            <dx:ASPxLabel ID="lblS8" runat="server" CssClass="tblTxt">
                            </dx:ASPxLabel>
                        </td>
                        <td align="center">
                            <dx:ASPxLabel ID="lblStarted8" runat="server">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel33" runat="server" CssClass="tblTxt" 
                                Text="This service sends remote console commands to IBM Domino servers for authorized users. ">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr id="trDaily" runat="server">
                        <td align="center">
                            <asp:Label ID="ASPLabel34" runat="server" CssClass="lblsmallFont" 
                                >Daily Tasks <sup>*</sup></asp:Label>
                        </td>
                        <td align="center">
                            <dx:ASPxLabel ID="lbls2" runat="server" CssClass="tblTxt">
                            </dx:ASPxLabel>
                        </td>
                        <td align="center">
                            <dx:ASPxLabel ID="lblStarted2" runat="server" CssClass="tblTxt" Wrap="False">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel35" runat="server" CssClass="tblTxt" 
                                Text="This service performs nightly data consolidation and analysis. ">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr id="trDatabase" runat="server" visible="false">
                        <td align="center">
                            <asp:Label ID="ASPLabel36" runat="server" CssClass="lblsmallFont"
                                >Database Health <sup>*</sup></asp:Label>
                        </td>
                        <td align="center">
                            <dx:ASPxLabel ID="lblS3" runat="server" CssClass="tblTxt">
                            </dx:ASPxLabel>
                        </td>
                        <td align="center">
                            <dx:ASPxLabel ID="lblStarted3" runat="server" CssClass="tblTxt" Wrap="False">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel ID="ASPxLabel37" runat="server" CssClass="tblTxt" 
                                Text="This service performs nightly inventory and health analysis for Notes databases.">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                      <tr id="trCore64Bit" runat="server">
                        <td align="center">
                            <asp:Label ID="CoreService" runat="server" CssClass="lblsmallFont"
                                Text="Core Services (64 bit)"></asp:Label>
                        </td>
                        <td align="center">
                            <dx:ASPxLabel ID="CoreServicesStatus" runat="server" CssClass="tblTxt">
                            </dx:ASPxLabel>
                        </td>
                        <td align="center">
                            <dx:ASPxLabel ID="CoreServiceslaststart" runat="server" CssClass="tblTxt">
                            </dx:ASPxLabel>
                        </td>
                        <td>
                            <dx:ASPxLabel ID="CoreServicesDescription" runat="server" CssClass="tblTxt" 
                                Text="This service provides monitoring for Core Services(64 bit). ">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div  id="infoDiv2" runat="server" class="info" style="display: block">
    Services marked with an asterisk are normally expected to be stopped.  These services run at night and shut down when their work is finished.
            </div>
    </dx:PanelContent>
                                        </PanelCollection>
                                    </dx:ASPxRoundPanel>

            </td>
        </tr>
    </table>

</asp:Content>
