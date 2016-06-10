<%@ Page Title="VitalSigns Plus - Biggest Mail Files" Language="C#" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="BiggestMailFiles.aspx.cs" Inherits="VSWebUI.BiggestMailFiles" %>
<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }     
        .style2
       {
            width: 126px;
        }        
    </style>

    <script type="text/javascript">
    function Resized() {
        var callbackState = document.getElementById('ContentPlaceHolder1_callbackState').value;

        if (callbackState == 0)
            DoCallback();
    }

    function DoCallback() {
        document.getElementById('ContentPlaceHolder1_chartWidth').value = document.body.offsetWidth - 105;
        //cSelectServerRoundPanel.style.width = document.getElementById('ContentPlaceHolder1__chartWidth').value + "px";
        CBigmail.PerformCallback();    

    }

    function ResizeChart(s, e) {
        document.getElementById('ContentPlaceHolder1_callbackState').value = 0;
        s.GetMainElement().style.width = document.getElementById('ContentPlaceHolder1__chartWidth').value + "px";
        //cSelectServerRoundPanel.style.width = document.getElementById('ContentPlaceHolder1__chartWidth').value + "px";
    }

    function ResetCallbackState() {
        window.document.form1.elements['ContentPlaceHolder1_callbackState'].value = 1;
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input id="chartWidth" type="hidden" runat="server" value="400" />
        <input id="callbackState" type="hidden" runat="server" value="0" />
    <table class="style1">
        <tr>
            <td> 
			<table>
			 <tr>
        <td>
            <div class="header" id="titleDiv" runat="server">Biggest Mail Files for Domino</div>
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
	</td>           
        </tr>
         <%-- 15/2/2016 Sowmya Added for VSPLUS 2610--%>
        <tr>
        <td>
        <table>
        <tr>
              <td>
                  <dx:ASPxLabel ID="TypeLabel" runat="server" CssClass="lblsmallFont" 
                Text="Type:" >
            </dx:ASPxLabel>
       
              </td>
              <td>
            <dx:ASPxComboBox ID="TypeComboBox" runat="server"  onselectedindexchanged="TypeComboBox_SelectedIndexChanged"  AutoPostBack="True" >
			<Items>
                    <dx:ListEditItem Text="Select Type" Value="0" />
                </Items>
            </dx:ASPxComboBox>
             </td>
             <td class="style2">
             &nbsp;
           </td>
        <td>
			<dx:ASPxComboBox ID="ServerComboBox" runat="server" ValueType="System.String" 
                        AutoPostBack="True" 
                        onselectedindexchanged="ServerComboBox_SelectedIndexChanged">
                    </dx:ASPxComboBox>
					</td>				
        </tr>
        </table>
      </td>
      </tr>
    
        <tr>
            <td width="100%">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <dxchartsui:WebChartControl ID="BigMailChartControl" runat="server" 
                    oncustomcallback="BigMailChartControl_CustomCallback" Height="600px" 
                    Width="1030px" ClientInstanceName="CBigmail" 
                        AppearanceNameSerializable="Chameleon" CrosshairEnabled="True">
                        <diagramserializable>
                            <cc1:XYDiagram>
                                <axisx visibleinpanesserializable="-1">
                                    <label staggered="True">
                                    <resolveoverlappingoptions allowhide="False" />
                                    </label>
                                </axisx>
                                <axisy visibleinpanesserializable="-1">
                                </axisy>
                            </cc1:XYDiagram>
                        </diagramserializable>
                        <seriesserializable>
                            <cc1:Series Name="Series 1">
                            </cc1:Series>
                            <cc1:Series Name="Series 2">
                            </cc1:Series>
                        </seriesserializable>
                    </dxchartsui:WebChartControl>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ServerComboBox" />
                </Triggers>
                </asp:UpdatePanel>
                
                    </td>
        </tr>
    </table>
</asp:Content>
