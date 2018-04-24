<%@ Page Title="VitalSigns Plus - Business Hours" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="MaintenanceBusinessHrs.aspx.cs" Inherits="VSWebUI.WebForm12" %>
<%--﻿<%@ Page Title="VitalSigns Plus-MaintenanceBusinessHrs" Language="C#" MasterPageFile="~/Site1.Master"
	AutoEventWireup="true" CodeBehind="MaintenanceBusinessHrs.aspx.cs" Inherits="VSWebUI.WebForm12" %>--%>

<%--﻿<%@ Page Title="VitalSigns Plus - Business Hours" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="MaintenanceBusinessHrs.aspx.cs" Inherits="VSWebUI.WebForm12" %>
--%>


<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>


<%@ Register Assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxGauges" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxGauges.Gauges" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxGauges.Gauges.Linear" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxGauges.Gauges.Circular" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxGauges.Gauges.State" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxGauges.Gauges.Digital" TagPrefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<link href='http://fonts.googleapis.com/css?family=Francois One' rel='stylesheet'
		type='text/css' />
	<link rel="stylesheet" type="text/css" href="../css/vswebforms.css" />
	<script src="../js/jquery-1.9.1.js" type="text/javascript"></script>
	<script src="../js/bootstrap.min.js" type="text/javascript"></script>
	<script type="text/javascript">
		$(document).ready(function () {
			$('.alert-success').delay(10000).fadeOut("slow", function () {
			});
		});
		function check_checkboxes() {
			alert("hi");
			var c = document.getElementsByName("check");
			//var c = document.getElementsByTagName('input');
			for (var i = 0; i < c.length; i++) {
				
					if (c[i].checked) { return true }
				
			}
			return false;
		}
	</script>
	 <script type="text/javascript">

	 	function showendtime() {

	 		lStartTime = document.getElementById("ContentPlaceHolder1_ASPxRoundPanel2_MaintStartTimeEdit_I");


	 		lDuration = document.getElementById("ContentPlaceHolder1_ASPxRoundPanel2_MaintDurationTextBox_I");
	 	
	 		lEndTime = document.getElementById("ContentPlaceHolder1_ASPxRoundPanel2_lblEndTime");
	 		lblDuration = document.getElementById("ContentPlaceHolder1_ASPxRoundPanel2_lblDuration");

	 		var d = new Date();//  new Date(lstartDate.value);
	 		
	 		var curr_date = d.getDate();
	 		var curr_month = d.getMonth() + 1; //Months are zero based
	 		var curr_year = d.getFullYear();
	 		
	 		var d1 = new Date(curr_month + "/" + curr_date + "/" + curr_year + ' ' + lStartTime.value);

	 		var d2 = new Date(d1.getTime() + (lDuration.value * 60000));
	 		var hours = d2.getHours();
	 		//alert(hours);
	 		var minutes = d2.getMinutes();
	 		//alert(minutes);
	 		var ampm = hours >= 12 ? 'PM' : 'AM';
	 		hours = hours % 12;
	 		hours = hours ? hours : 12; // the hour '0' should be '12'
	 		minutes = minutes < 10 ? '0' + minutes : minutes;
	 		var strTime = hours + ':' + minutes + ' ' + ampm;


	 		lEndTime.innerHTML = strTime;
	         lblDuration.innerHTML = Math.floor(lDuration.value / 60) + ' hour(s) ' + (lDuration.value % 60) + ' minutes';
	 		
	 	}
	 	
	 	function OnTextChanged(s, e) {
	 		trackBar.SetPosition(s.GetText());
	 		//alert(s.GetText());
	 		showendtime();
	 	}
    </script>
	<style type="text/css">
		.style1
		{
			width: 100%;
		}
	
	
	    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--﻿<%@ Page Title="VitalSigns Plus-MaintenanceBusinessHrs" Language="C#" MasterPageFile="~/Site1.Master"
	AutoEventWireup="true" CodeBehind="MaintenanceBusinessHrs.aspx.cs" Inherits="VSWebUI.WebForm12" %>--%>
	<table>
		<tr>
			<td>
				<div class="header" id="servernamelbldisp" runat="server">
					Hours Definition</div>
				<div id="successDiv" runat="server" class="alert alert-success" style="display: none">
					Business Hours were successully updated.
					<button type="button" class="close" data-dismiss="alert">
						<span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
				</div>
			</td>
		</tr>
        <tr>
            <td>
            <table>
                <tr>
                    <td>
										<dx:ASPxLabel runat="server" ID="timezone" Text="Name:" CssClass="lblsmallFont">
										</dx:ASPxLabel>
									</td>
									<td>
										<dx:ASPxTextBox runat="server" ID="txttimezone">
										<ValidationSettings ErrorDisplayMode="ImageWithTooltip" >
                                                            <RequiredField ErrorText="Enter Business Hours Name" IsRequired="True" />
                                                  </ValidationSettings>
          </dx:ASPxTextBox>
										
									</td>
                                    <td>
                                        &nbsp;</td>
                                    <td align="right">
                                        <dx:ASPxRadioButtonList ID="UseDaysRadioButtonList" runat="server" 
                                        RepeatDirection="Horizontal" SelectedIndex="2" Width="100%">
                                        <Items>
                                            <dx:ListEditItem Text="Use for Alerts Only" 
                                                Value="0" />
                                            <dx:ListEditItem Text="Use for Servers Only" Value="1" />
                                            <dx:ListEditItem Selected="True" Text="Use for both" Value="2" />
                                        </Items>
                                    </dx:ASPxRadioButtonList>
                                    </td>
                </tr>
            </table>
            </td>
        </tr>
		<tr>
			<td valign="top">
				<dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
					CssPostfix="Glass" GroupBoxCaptionOffsetY="-24px" 
                    HeaderText="Start and End Times, Duration, Days" SpriteCssFilePath="~/App_Themes/Glass/{0}/sprite.css"
					Width="610px">
					<ContentPaddings PaddingBottom="10px" PaddingTop="10px" PaddingLeft="4px" />
					<HeaderStyle Height="23px">
					</HeaderStyle>
					<PanelCollection>
						<dx:PanelContent ID="PanelContent3" runat="server" SupportsDisabledAttribute="True">
							<table>
								    <tr>
                                        <td colspan="3">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                        <td valign="top" colspan="5">
                                            <div class="info" runat="server" id="infoDiv">Enter start time and duration in minutes below or use the slider control to set duration in minutes. End time will be computed automatically.</div>
                                        </td>
                                        
                                </tr>
                                                        <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel7" runat="server" CssClass="lblsmallFont" 
                                            Text="Start Time:" Wrap="False">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxTimeEdit ID="MaintStartTimeEdit" runat="server" AutoPostBack="True" 
                                            ClientInstanceName="StartTime" OnValueChanged="MaintStartTimeEdit_ValueChanged"  EditFormat="Time" DateTime="1/2/2015"
                                            Width="100px">
											<ValidationSettings ErrorDisplayMode="ImageWithTooltip" >
                                                            <RequiredField ErrorText="Enter Business Hours Name" IsRequired="True" />
                                                  </ValidationSettings>
                                        </dx:ASPxTimeEdit>
                                    </td>
                                                            <td>
                                                                <dx:ASPxLabel ID="ASPxLabel10" runat="server" CssClass="lblsmallFont" 
                                                                    Text="End Time:" Wrap="False">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxLabel ID="lblEndTime" runat="server" ClientInstanceName="EndTime" 
                                                                    CssClass="lblsmallFont" Wrap="False">
                                                                </dx:ASPxLabel>
                                                            </td>
                                                            <td rowspan="4">
                                                                <table class="style1">
								<tr>
									<td>
										<dx:ASPxCheckBox ID="AllCheckBox" runat="server" AutoPostBack="True" CheckState="Unchecked"
											OnCheckedChanged="AllCheckBox_CheckedChanged" Text="Select All" ClientInstanceName="check">
											<ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
                                                            <RequiredField ErrorText="Enter Alert Name" IsRequired="True" />
                                                  </ValidationSettings>
										</dx:ASPxCheckBox>
									</td>
								</tr>
								<tr>
									<td>
										<dx:ASPxCheckBox ID="MonCheckBox" runat="server" CheckState="Unchecked" Text="Monday" ClientInstanceName="check">
										<ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
                                                            <RequiredField ErrorText="Enter Alert Name" IsRequired="True" />
                                                  </ValidationSettings>
										</dx:ASPxCheckBox>
									</td>
								</tr>
								<tr>
									<td>
										<dx:ASPxCheckBox ID="TueCheckBox" runat="server" CheckState="Unchecked" Text="Tuesday" ClientInstanceName="check">
										</dx:ASPxCheckBox>
									</td>
								</tr>
								<tr>
									<td>
										<dx:ASPxCheckBox ID="WedCheckBox" runat="server" CheckState="Unchecked" Text="Wednesday" ClientInstanceName="check">
										<ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
                                                            <RequiredField ErrorText="Enter Alert Name" IsRequired="True" />
                                                  </ValidationSettings>
										</dx:ASPxCheckBox>
									</td>
								</tr>
								<tr>
									<td> 
										<dx:ASPxCheckBox ID="ThusCheckBox" runat="server" CheckState="Unchecked" Text="Thrusday" ClientInstanceName="check">
										<ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
                                                            <RequiredField ErrorText="Enter Alert Name" IsRequired="True" />
                                                  </ValidationSettings>
										</dx:ASPxCheckBox>
									</td>
								</tr>
								<tr>
									<td>
										<dx:ASPxCheckBox ID="FriCheckBox" runat="server" CheckState="Unchecked" Text="Friday" ClientInstanceName="check">
										<ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
                                                            <RequiredField ErrorText="Enter Alert Name" IsRequired="True" />
                                                  </ValidationSettings>
										</dx:ASPxCheckBox>
									</td>
								</tr>
								<tr>
									<td>
										<dx:ASPxCheckBox ID="SatCheckBox" runat="server" CheckState="Unchecked" Text="Saturday" ClientInstanceName="check">
										<ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
                                                            <RequiredField ErrorText="Enter Alert Name" IsRequired="True" />
                                                  </ValidationSettings>
										</dx:ASPxCheckBox>
									</td>
								</tr>
								<tr>
									<td>
										<dx:ASPxCheckBox ID="SunCheckBox" runat="server" CheckState="Unchecked" Text="Sunday" ClientInstanceName="check">
										<ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
                                                            <RequiredField ErrorText="Enter Alert Name" IsRequired="True" />
                                                  </ValidationSettings>
										</dx:ASPxCheckBox>
									</td>
								</tr>
							</table>
                                                            </td>
                                </tr>
								    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td colspan="3">
                                            &nbsp;</td>
                                </tr>
								    <tr>
                                        <td>
                                            <dx:ASPxLabel ID="ASPxLabel8" runat="server" CssClass="lblsmallFont" 
                                                Text="Duration:" Wrap="False">
                                            </dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="MaintDurationTextBox" runat="server" 
                                                ClientInstanceName="MaintDuration" Width="100px">
                                                <ClientSideEvents LostFocus="OnTextChanged" />
                                                <MaskSettings Mask="&lt;0..1440&gt;" />
                                                <ValidationSettings CausesValidation="True" ErrorDisplayMode="ImageWithTooltip" 
                                                    SetFocusOnError="True">
                                                    <RequiredField ErrorText="Please Enter Duration" IsRequired="True" />
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td colspan="2">
                                            <dx:ASPxLabel ID="lblDuration" runat="server" ClientInstanceName="Duration" 
                                            CssClass="lblsmallFont" Text="minutes" Wrap="False">
                                        </dx:ASPxLabel>
                                        </td>
                                </tr>
                                <tr>
                                    <td colspan="4">

                                        <dx:ASPxTrackBar ID="DurationTrackBar" runat="server" AutoPostBack="True" 
                                            ClientInstanceName="trackBar" 
                                            CssFilePath="~/App_Themes/Office2010Black/{0}/styles.css" 
                                            CssPostfix="Office2010Black" EnableViewState="False" LargeTickEndValue="1440" 
                                            LargeTickInterval="240" MaxValue="1440" 
                                            OnPositionChanged="DurationTrackBar_PositionChanged" Position="0" 
                                            PositionStart="0" ScalePosition="LeftOrTop" 
                                            SpriteCssFilePath="~/App_Themes/Office2010Black/{0}/sprite.css" Step="5" 
                                            Theme="Office2010Blue" Width="100%">
                                            <ValueToolTipStyle>
                                            </ValueToolTipStyle>
                                        </dx:ASPxTrackBar>
                                    </td>
                                </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="UseDaysRadioButtonList" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                </tr>
							</table>
						</dx:PanelContent>
					</PanelCollection>
				</dx:ASPxRoundPanel>
			</td>
		</tr>
		<tr>
			<td>
				<div id="errorDiv" runat="server" class="alert alert-danger" style="display: none">
					The following fields were not updated:
				</div>
				<div id="Div1" runat="server" class="alert alert-danger" style="display: none">
				Error attempting to update the status table.
			</div>
				<table>
					<tr>
						<td>
							<dx:ASPxButton ID="OKButton" runat="server" OnClick="OKButton_Click" Text="OK" CssClass="sysButton"
								ClientIDMode="Static" >
								<%--<ClientSideEvents Click="check_checkboxes()" />--%>
								</dx:ASPxButton>
                            <%--﻿<%@ Page Title="VitalSigns Plus - Business Hours" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="MaintenanceBusinessHrs.aspx.cs" Inherits="VSWebUI.WebForm12" %>
--%>
						</td>
						<td>
							<dx:ASPxButton ID="CancelButton" runat="server" OnClick="CancelButton_Click" Text="Cancel" CssClass="sysButton"
								CausesValidation="false">
							</dx:ASPxButton>
						</td>
						
						<td>
				<dx:ASPxPopupControl runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
					Modal="True" HeaderText="Information" Theme="Glass" ID="MsgPopupControl">
					<ContentCollection>
						<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" SupportsDisabledAttribute="True">
							<table class="style1">
								<tr>
									<td>
										<dx:ASPxLabel runat="server" Text="ASPxLabel" Theme="Default" CssClass="lblsmallFont"
											ID="MsgLabel">
										</dx:ASPxLabel>
									</td>
								</tr>
								<tr>
									<td>
										<dx:ASPxButton runat="server" Text="OK" Theme="Office2010Blue" ID="ASPxButton1" OnClick="ASPxButton1_Click">
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
			</td>
			
		</tr>
	</table>
	
</asp:Content>
								
										
								
								
						
						
		
						
