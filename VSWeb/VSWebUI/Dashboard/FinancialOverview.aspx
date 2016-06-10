<%@ Page Language="C#" Title="VitalSigns Plus - Financial Overview" MasterPageFile="~/DashboardSite.Master" AutoEventWireup="true" CodeBehind="FinancialOverview.aspx.cs" Inherits="VSWebUI.Dashboard.FinancialOverview" %>

<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>



<%@ Register assembly="DevExpress.XtraCharts.v14.2.Web, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts.Web" tagprefix="dxchartsui" %>
<%@ Register assembly="DevExpress.XtraCharts.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet' type='text/css' />
    <link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
    <script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function Resized() {
            var callbackState = document.getElementById('ContentPlaceHolder1_callbackState').value;

            if (callbackState == 0)
                DoCallback();
        }
        function DoCallback() {
        document.getElementById('ContentPlaceHolder1_chartWidth').value = Math.round(document.body.offsetWidth/2) - 20;

    }
    
   
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input id="chartWidth" type="hidden" runat="server" value="400" />
<input id="callbackState" type="hidden" runat="server" value="0" />
    <table width="100%">
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                        <img alt="" src="../images/icons/financial.png" height="20px" width="20px" />
                    </td>
                    <td>
                        <div class="header" id="lblTitle" runat="server">Financial Overview</div>
                    </td>
                    </tr>
                </table>
            </td>
        </tr>
      
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td>
          
                  <table>
                                        <tr>
                                         
                                          
                                            <td valign="top">
                                                 <dxchartsui:WebChartControl ID="MonthlyExpenditurebyTypeChart" runat="server" 
                                                    CrosshairEnabled="True"  PaletteName="Module" Height="200px" Width="400px" >
                                                    <diagramserializable>
                                                        <cc1:XYDiagram Rotated="True">
                                                        <axisx visibleinpanesserializable="-1" reverse="True" title-text="" 
                                                                title-visibility="True">
                                                                <tickmarks minorvisible="False" />
<Tickmarks MinorVisible="False"></Tickmarks>
                                                            </axisx>
                                                        <axisy visibleinpanesserializable="-1" title-text="" title-visibility="True">
                                                            </axisy>
                                                        </cc1:XYDiagram>
                                                    </diagramserializable>
                                                    <legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" 
                                                        maxverticalpercentage="30" Visibility="False"></legend>
                                                    <seriesserializable>
                                                        <cc1:Series Name="Series 1">
                                                        </cc1:Series>
                                                    </seriesserializable>
                                                    <titles>
                                                        <cc1:ChartTitle Font="Tahoma, 12pt" Text="Monthly Expenditure by Type" />
                                                    </titles>
                                                </dxchartsui:WebChartControl>
                                            </td>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                 <td valign="top">
                                                 <dxchartsui:WebChartControl ID="MonthlyExpenditurebyLocationChart" runat="server" 
                                                    CrosshairEnabled="True"  PaletteName="Module" Height="200px" Width="400px" >
                                                    <diagramserializable>
                                                        <cc1:XYDiagram Rotated="True">
                                                   <axisx visibleinpanesserializable="-1" reverse="True">
                                                                <tickmarks minorvisible="False" />
<Tickmarks MinorVisible="False"></Tickmarks>
                                                            </axisx>
                                                        <axisy visibleinpanesserializable="-1" title-text="" title-visibility="True">
                                                            </axisy>
                                                        </cc1:XYDiagram>
                                                    </diagramserializable>
                                                    <legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" 
                                                        maxverticalpercentage="30" Visibility="False"></legend>
                                                    <seriesserializable>
                                                        <cc1:Series Name="Series 1">
                                                        </cc1:Series>
                                                    </seriesserializable>
                                                    <titles>
                                                        <cc1:ChartTitle Font="Tahoma, 12pt" Text="Monthly Expenditure by Location" />
                                                    </titles>
                                                </dxchartsui:WebChartControl>
                                            </td>
                                             <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                 <td valign="top">
                                                 <dxchartsui:WebChartControl ID="MonthlyExpenditurebyCategoryChart" runat="server" 
                                                    CrosshairEnabled="True"  PaletteName="Module" Height="200px" Width="400px" >
                                                    <diagramserializable>
                                                    <cc1:XYDiagram Rotated="True">
                                                        <axisx visibleinpanesserializable="-1" reverse="True">
                                                                <tickmarks minorvisible="False" />
<Tickmarks MinorVisible="False"></Tickmarks>
                                                            </axisx>
                                                         <axisy visibleinpanesserializable="-1" title-text="" title-visibility="True">
                                                            </axisy>
                                                        </cc1:XYDiagram>
                                                    </diagramserializable>
                                                    <legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" 
                                                        maxverticalpercentage="30" Visibility="False"></legend>
                                                    <seriesserializable>
                                                        <cc1:Series Name="Series 1">
                                                        </cc1:Series>
                                                    </seriesserializable>
                                                    <titles>
                                                        <cc1:ChartTitle Font="Tahoma, 12pt" Text="Monthly Expenditure by Category" />
                                                    </titles>
                                                </dxchartsui:WebChartControl>
                                            </td>
                                        </tr>
                                        <tr><td>&nbsp;</td></tr>
                                        <tr>
                                         
                                          
                                            <td valign="top">
                                                 <dxchartsui:WebChartControl ID="MostUtilizedServersChart" runat="server" 
                                                    CrosshairEnabled="True"   PaletteName="Module" Height="200px" Width="400px" >
                                                      <diagramserializable>
                                                        <cc1:XYDiagram Rotated="True" >
                                                        
                                                        <axisx visibleinpanesserializable="-1" visible="True"   
                                                                reverse="True" title-visibility="Default" >
                                                                <tickmarks minorvisible="False"  />
<Tickmarks MinorVisible="False"></Tickmarks>
                                                            </axisx>
                                                        <axisy visibleinpanesserializable="-1" title-text="Percentage" 
                                                                title-visibility="True"  > 
                                                            </axisy >
                                                        </cc1:XYDiagram>
                                                    </diagramserializable>
                                                    <legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" 
                                                        maxverticalpercentage="30" Visibility="False"></legend>
                                                    <seriesserializable>
                                                        <cc1:Series Name="Series 1">
                                                        </cc1:Series>
                                                    </seriesserializable>
                                                    <titles>
                                                        <cc1:ChartTitle Font="Tahoma, 12pt"  Text="Most Utilized Servers" 
                                                            Antialiasing="False"/>
                                                    </titles>
                                                </dxchartsui:WebChartControl>
                                            </td>
                                              <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                 <td valign="top">
                                                 <dxchartsui:WebChartControl ID="LeastUtilizedServersChart" runat="server" 
                                                    CrosshairEnabled="True"  PaletteName="Module" Height="200px" Width="400px" >
                                                    <diagramserializable>
                                                        <cc1:XYDiagram Rotated="True">
                                                   <axisx visibleinpanesserializable="-1" reverse="True" title-text="" 
                                                                title-visibility="True" visibility="True">
                                                                <tickmarks minorvisible="False" />
<Tickmarks MinorVisible="False"></Tickmarks>
                                                            </axisx>
                                                        <axisy visibleinpanesserializable="-1" title-text="" title-visibility="True" 
                                                                visibility="True">
                                                            </axisy>
                                                        </cc1:XYDiagram>
                                                    </diagramserializable>
                                                    <legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" 
                                                        maxverticalpercentage="30" Visibility="False"></legend>
                                                    <seriesserializable>
                                                        <cc1:Series Name="Series 1">
                                                        </cc1:Series>
                                                    </seriesserializable>
                                                    <titles>
                                                        <cc1:ChartTitle Font="Tahoma, 12pt"  Text="Least Utilized Servers"  />
                                                    </titles>
                                                </dxchartsui:WebChartControl>
                                            </td>
                                              <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                 <td valign="top">
                                                 <dxchartsui:WebChartControl ID="CostPerUserServedChart" runat="server" 
                                                    CrosshairEnabled="True"  PaletteName="Module" Height="200px" Width="400px" >
                                                    <diagramserializable>
                                                    <cc1:XYDiagram Rotated="True">
                                                        <axisx visibleinpanesserializable="-1" reverse="True">
                                                                <tickmarks minorvisible="False" />
<Tickmarks MinorVisible="False"></Tickmarks>
                                                            </axisx>
                                                         <axisy visibleinpanesserializable="-1" title-text="" title-visibility="True">
                                                            </axisy>
                                                        </cc1:XYDiagram>
                                                    </diagramserializable>
                                                    <legend alignmenthorizontal="Center" alignmentvertical="BottomOutside" 
                                                        maxverticalpercentage="30" Visibility="False"></legend>
                                                    <seriesserializable>
                                                        <cc1:Series Name="Series 1">
                                                        </cc1:Series>
                                                    </seriesserializable>
                                                    <titles>
                                                        <cc1:ChartTitle Font="Tahoma, 12pt" Text="Cost Per User Served" />
                                                    </titles>
                                                </dxchartsui:WebChartControl>
                                            </td>
                                        </tr>
                                    </table>
             
            </td>
        </tr>
        </table>
</asp:Content>
