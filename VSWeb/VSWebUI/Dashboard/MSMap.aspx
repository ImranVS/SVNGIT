<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MSMap.aspx.cs" Inherits="VSWebUI.Dashboard.MSMap" MasterPageFile="~/DashboardSite.Master" %>
<%@ MasterType virtualpath="~/DashboardSite.Master" %>
<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="http://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=7.0"></script>
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
<script src="http://www.bootstrap-switch.org/dist/js/bootstrap-switch.js"></script>
<link href="http://www.bootstrap-switch.org/docs/css/bootstrap.min.css" rel="Stylesheet" />
<link href="http://www.bootstrap-switch.org/dist/css/bootstrap3/bootstrap-switch.css" rel="Stylesheet" />
<script src="http://www.bootstrap-switch.org/docs/js/bootstrap.min.js"></script>


<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<script>
    function Ok() {
    awegawyhg
   }
   function OnItemClick(s, e) {
   	if (e.item.parent == s.GetRootItem())
   		e.processOnServer = false;
   }
</script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript">

	var arr = new Array();
	var lastZoom = 0;

	var CityArray = [];
	var StateArray = [];
	var CountryArray = [];
	var CountyArray = [];

	var CityRegionArray = [];
	var StateRegionArray = [];
	var CountryRegionArray = [];
	var CountyRegionArray = [];

	var Office365TestsArray = [];
	var Office365TestsThresholds = [];

	var SelectedMapType = 1;

	var OrderOfBingIdentifiers = ['CountryRegion', 'AdminDivision1', 'AdminDivision2', 'PopulatedPlace'];

	var numOfCountries = 0;
	var dataLayer;
	var infoboxLayer;
	var infobox;

	var numOfCalls = 0;

	var infoBoxClickHandler = null;


    function loadAdvancedShapeModule() {
		Microsoft.Maps.loadModule('Microsoft.Maps.AdvancedShapes', { callback: getBoundary });
	}

	function ParseEncodedValue(value) {
		var list = new Array();
		var index = 0;
		var xsum = 0;
		var ysum = 0;
		var max = 4294967296;
		var safeCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_-";
		while (index < value.length) {
			var n = 0;
			var k = 0;

			while (1) {
				if (index >= value.length) {
					return null;
				}
				var b = safeCharacters.indexOf(value.charAt(index++));
				if (b == -1) {
					return null;
				}
				var tmp = ((b & 31) * (Math.pow(2, k)));

				var ht = tmp / max;
				var lt = tmp % max;

				var hn = n / max;
				var ln = n % max;

				var nl = (lt | ln) >>> 0;
				n = (ht | hn) * max + nl;
				k += 5;
				if (b < 32) break;
			}

			var diagonal = parseInt((Math.sqrt(8 * n + 5) - 1) / 2);
			n -= diagonal * (diagonal + 1) / 2;
			var ny = parseInt(n);
			var nx = diagonal - ny;
			nx = (nx >> 1) ^ -(nx & 1);
			ny = (ny >> 1) ^ -(ny & 1);
			xsum += nx;
			ysum += ny;
			var lat = ysum * 0.00001;
			var lon = xsum * 0.00001
			list.push(new Microsoft.Maps.Location(lat, lon));
		}
		return list;
	}




	//result: xml from bing
	//location: location string.  ex. Rockaway, New Jersey, United States
	//count: number of servers
	//Issues, NoIssues, InMaintenance, NotResponding: number of servers in the respected catagories
	//VSLocation: Location Alias
	function boundaryCallback(result, location, typeID, avg, count, Issues, NoIssues, InMaintenance, NotResponding, VSLocation, LocID, O365Values, O365Counts) {
		var SkipToEnd = false;
		var entity = result.d.results[0];
		if (result.error)
			alert('here');
		if (typeof entity === 'undefined') {
			var newTypeID = typeID;
			for (var i = 0; i < OrderOfBingIdentifiers.length; i++) {
				if (OrderOfBingIdentifiers[i] == typeID && i != OrderOfBingIdentifiers.length - 1)
					newTypeID = OrderOfBingIdentifiers[i + 1];
				
			}
			if (newTypeID == typeID) {
				SkipToEnd = true;
			}
			if (!SkipToEnd) {
				var boundaryUrl = baseUrl + "GetBoundary('" + location + "',1,'" + newTypeID + "',0,0,'en','US')&$format=json&key=" + creds + "&jsonp=?";
				$.getJSON(boundaryUrl, function (results) { boundaryCallback(results, location, typeID, avg, count, Issues, NoIssues, InMaintenance, NotResponding, VSLocation, LocID, O365Values, O365Counts); });
				return;
			}
		}
		else {
			var entityName = entity.Name.EntityName;
			var primitives = entity.Primitives;
		}

		var polygoncolor = null;
		var strokecolor = null;
		var boundaryVertices = null;
		var numOfVertices = 0;
		var infobox

		if (!SkipToEnd) {
			var polygonArray = new Array();
			for (var i = 0; i < primitives.length; i++) {
			
				var ringStr = primitives[i].Shape;
				var ringArray = ringStr.split(",");

				for (var j = 1; j < ringArray.length; j++) {
					var array = ParseEncodedValue(ringArray[j]);

					if (array.length > numOfVertices) {
						numOfVertices = array.length;
						boundaryVertices = array;
					}
					polygonArray.push(array);
				}

				var polygon = new Microsoft.Maps.Polygon(polygonArray[0], {});

				if (NoIssues == ""){
					SelectedMapType = 0;
				}

				polygon.MetaData = { 

					title: entityName,
					description: '',
					avgRT: avg == "" ? NaN : Math.round(avg),
					Office365Colors: [],
					NumOfPoints: count == "" ? NaN : Math.round(count),
					Issues: Issues == "" ? NaN : Math.round(Issues),
					NoIssues: NoIssues == "" ? NaN : Math.round(NoIssues),
					InMaintenance: InMaintenance == "" ? NaN : Math.round(InMaintenance),
					NotResponding: NotResponding == "" ? NaN : Math.round(NotResponding),
					OnPremisesColor: '',
					LocID: LocID,
					Office365Values: O365Values,
					Office365Counts: O365Counts
				};

				var polygonRegional = new Microsoft.Maps.Polygon(polygonArray[0], {});

				if (typeID == "AdminDivision1") {
					if (typeof StateRegionArray[VSLocation] === 'undefined')
						StateRegionArray[VSLocation] = [];
					StateArray.push(polygon);

				}
				else if (typeID == "CountryRegion") {
					if (typeof CountryRegionArray[VSLocation] === 'undefined')
						CountryRegionArray[VSLocation] = [];
					CountryArray.push(polygon);
				}
				else if (typeID == "AdminDivision2") {
					if (typeof CountyRegionArray[VSLocation] === 'undefined')
						CountyRegionArray[VSLocation] = [];
					CountyArray.push(polygon);
				}
				else {
					if (typeof CityRegionArray[VSLocation] === 'undefined')
						CityRegionArray[VSLocation] = [];
					CityArray.push(polygon);
				}
			}
		}
		numOfCalls--;
		if (numOfCalls <= 0) {			
			CleanUpPolygons(CountryArray);
			CleanUpPolygons(StateArray);
			CleanUpPolygons(CountyArray);
			CleanUpPolygons(CityArray);

			ShowCountries();
		}
	}

	function CleanUpPolygons(arr) {
		
		var Red = new Microsoft.Maps.Color(100, 255, 0, 0);
		var Yellow = new Microsoft.Maps.Color(100, 255, 255, 0);
		var Green = new Microsoft.Maps.Color(100, 0, 128, 0);
		var Gray = new Microsoft.Maps.Color(100, 111, 111, 111);

		arr.forEach(function (a) {
			if (a.MetaData == null)
				alert('here'); 
		});

		arr.sort(function (a, b) { return a.MetaData.title > b.MetaData.title ? 1 : -1 });

		for (var i = 0; i < arr.length - 1; i++) {
			if (arr[i].MetaData.title == arr[i + 1].MetaData.title) {

//				if (!isNaN(arr[i].MetaData.avgRT)) {
//					var oldCount = arr[i].MetaData.NumOfPoints;
//					arr[i].MetaData.NumOfPoints = Math.round(arr[i].MetaData.NumOfPoints) + Math.round(arr[i + 1].MetaData.NumOfPoints);
//					arr[i].MetaData.avgRT = Math.round(((arr[i].MetaData.avgRT * oldCount) + (arr[i + 1].MetaData.avgRT * arr[i + 1].MetaData.NumOfPoints)) / (arr[i].MetaData.NumOfPoints));

				var isNull = true;
				for (var key in arr[i].MetaData.Office365Values) {
					isNull = false;
					var oldCount = arr[i].MetaData.Office365Counts[key];
					arr[i].MetaData.Office365Counts[key] = Math.round(arr[i].MetaData.Office365Counts[key]) + Math.round(arr[i + 1].MetaData.Office365Counts[key]);
//						arr[i].MetaData.avgRT = Math.round(((arr[i].MetaData.avgRT * oldCount) + (arr[i + 1].MetaData.avgRT * arr[i + 1].MetaData.NumOfPoints)) / (arr[i].MetaData.NumOfPoints));

					arr[i].MetaData.Office365Values[key] = Math.round(((arr[i].MetaData.Office365Values[key] * oldCount) + (arr[i + 1].MetaData.Office365Values[key] * arr[i + 1].MetaData.Office365Counts[key])) / (arr[i].MetaData.Office365Counts[key]));
				}
				if (isNull) {
					arr[i].MetaData.Office365Counts = arr[i + 1].MetaData.Office365Counts;
					arr[i].MetaData.Office365Values = arr[i + 1].MetaData.Office365Values;

				}
//				}
//				else {
//					arr[i].MetaData.NumOfPoints = arr[i + 1].MetaData.NumOfPoints;
//					arr[i].MetaData.avgRT = arr[i + 1].MetaData.avgRT;
//				}

				if (!isNaN(arr[i].MetaData.Issues)) {
					arr[i].MetaData.Issues = Math.round(arr[i].MetaData.Issues) + Math.round(arr[i + 1].MetaData.Issues ? arr[i + 1].MetaData.Issues : 0);
					arr[i].MetaData.NoIssues = Math.round(arr[i].MetaData.NoIssues) + Math.round(arr[i + 1].MetaData.NoIssues ? arr[i + 1].MetaData.NoIssues : 0);
					arr[i].MetaData.InMaintenance = Math.round(arr[i].MetaData.InMaintenance) + Math.round(arr[i + 1].MetaData.InMaintenance ? arr[i + 1].MetaData.InMaintenance : 0);
					arr[i].MetaData.NotResponding = Math.round(arr[i].MetaData.NotResponding) + Math.round(arr[i + 1].MetaData.NotResponding ? arr[i + 1].MetaData.NotResponding : 0);
				}
				else {
					arr[i].MetaData.Issues = arr[i + 1].MetaData.Issues;
					arr[i].MetaData.NoIssues = arr[i + 1].MetaData.NoIssues;
					arr[i].MetaData.InMaintenance = arr[i + 1].MetaData.InMaintenance;
					arr[i].MetaData.NotResponding = arr[i + 1].MetaData.NotResponding;
				}

				arr[i].MetaData.LocID = arr[i].MetaData.LocID + ',' + arr[i + 1].MetaData.LocID;

				arr.splice(i + 1, 1);
				i--;

			}
			else {
				//if (!isNaN(arr[i].MetaData.avgRT)) {
				//	arr[i].MetaData.NumOfPoints = Math.round(arr[i].MetaData.NumOfPoints);
				//	arr[i].MetaData.avgRT = Math.round(arr[i].MetaData.avgRT);
				//}

				for (var key in arr[i].MetaData.Office365Values) {
					arr[i].MetaData.Office365Counts[key] = Math.round(arr[i].MetaData.Office365Counts[key]);
					arr[i].MetaData.Office365Values[key] = Math.round(arr[i].MetaData.Office365Values[key]);
				}
				if (!isNaN(arr[i].MetaData.Issues)) {
					arr[i].MetaData.Issues = Math.round(arr[i].MetaData.Issues);
					arr[i].MetaData.NoIssues = Math.round(arr[i].MetaData.NoIssues);
					arr[i].MetaData.InMaintenance = Math.round(arr[i].MetaData.InMaintenance);
					arr[i].MetaData.NotResponding = Math.round(arr[i].MetaData.NotResponding);
				}
			}
		}


		arr.forEach(function (poly) {
			var color;
			var meta = poly.MetaData;

			for (var key in meta.Office365Values) {
				var avg = meta.Office365Values[key];
				var threshold = Office365TestsThresholds[key];

				if ((avg < threshold && threshold > 0) || threshold == 0 ) {
					color = Green;
				}
				else if (avg > threshold) {
					color = Yellow;
				}
				else {
					color = Red;
				}
				meta.Office365Colors[key] = color;
			}


			if (!isNaN(meta.Issues)) {

				if (meta.NotResponding > 0) {
					color = Red;
				}
				else if (meta.Issues > 0) {
					color = Yellow;
				}
				else if (meta.NoIssues > 0) {
					color = Green;
				}
				else {
					color = Gray;
				}

				meta.OnPremisesColor = color;

			}

			Microsoft.Maps.Events.addHandler(poly, 'click', displayInfoBox);
			//Microsoft.Maps.Events.addHandler(poly, 'mouseout', hideInfoBox);
		});
		

	}
	
	

	function viewChanged(e) {
		if (map.getZoom() != lastZoom) {

			getRegionByZoom();
			lastZoom = map.getZoom();
		}


	}

	function ShowPolygon(polygon) {
		var meta = polygon.MetaData;
		var t = Office365StatNameComboBoxClientID.GetText();
		if (t == "") {
			t = Office365StatNameComboBoxClientID.GetItem(0).text;
		}
		if (SelectedMapType == 0 && !isNaN(polygon.MetaData.Office365Values[t])) {
			polygon.setOptions({ fillColor: meta.Office365Colors[t], strokeColor: meta.Office365Colors[t] });
			dataLayer.push(polygon);
		}
		if (SelectedMapType == 1 && !isNaN(polygon.MetaData.Issues)) {
			polygon.setOptions({ fillColor: meta.OnPremisesColor, strokeColor: meta.OnPremisesColor });
			dataLayer.push(polygon);	
		}
	}

	function ShowCountries() {
		CountryArray.forEach(function (polygon) {

			ShowPolygon(polygon);

		});
	}

	function ShowStates() {
		StateArray.forEach(function (polygon) {

			ShowPolygon(polygon);

		});
	}

	function ShowCities() {
		CityArray.forEach(function (polygon) {

			ShowPolygon(polygon);

		});
	}

	function ShowCountys() {
		CountyArray.forEach(function (polygon) {

			ShowPolygon(polygon);

		});
	}

	function displayInfoBox(e) {
		if (e.target) {
			if (infoBoxClickHandler != null)
				Microsoft.Maps.Events.removeHandler(infoBoxClickHandler);

			infoBoxClickHandler = Microsoft.Maps.Events.addHandler(infobox, 'click', function (e) {
				window.location.href = e.target.redirectTo;
			});

			var point = new Microsoft.Maps.Point(e.getX(), e.getY());
			var loc = map.tryPixelToLocation(point);

			infobox.setLocation(loc);

			var meta = e.target.MetaData;

			if (SelectedMapType == 0) {
				var t = Office365StatNameComboBoxClientID.GetText();
				if (t == "") {
					t = Office365StatNameComboBoxClientID.GetItem(0).text;
				}
				meta.description =	t + ': ' +
									'Response Time: ' + meta.Office365Values[t] + ' ms <br \>' +
									'<font size="1"> Click for more info </font>'; ;
				meta.width = 190;
				meta.height = 120;
			}
			else if(SelectedMapType == 1){
				meta.description =	'Not Responding: ' + meta.NotResponding + '<br />' +
									'Issues: ' + meta.Issues + '<br />' +
									'No Issues: ' + meta.NoIssues + '<br />' +
									'In Maintenance: ' + meta.InMaintenance + '<br \>' + 
									'<font size="1"> Click for more info </font>';
				meta.width = 200;
				meta.height = 140;
			}

			if (meta) {
				meta.offset = new Microsoft.Maps.Point(0, 0);
				meta.visible = true;
				infobox.setOptions(meta);
				infobox.redirectTo = "DeviceTypeList.aspx?MapLoc=" + meta.LocID;
			}
		}
	}

	function hideInfoBox() {
		//console.log('hiding at ' + (new Date()).getTime());
		infobox.setOptions({ visible: false });
	}


	function getRegionByZoom() {
		infobox.setOptions({ visible: false });
		dataLayer.clear();

		if (map.getZoom() > 9) {
			
			 ShowCities();
		}
		else if(map.getZoom() > 7){
		
			ShowCountys();
		}
		else if (map.getZoom() > 3) {
		
			ShowStates();
		}
		else {
		
			ShowCountries();
		}
	}

	function MapTypeChanged() {
		//var selectedVal = $('#<%=MapSelectionRBL.ClientID %> input[type=radio]:checked').val();
		//SelectedMapType = 1; //selectedVal;

		//selectedVal = $('#<%=RegionOrLocationRBL.ClientID %> input[type=radio]:checked').val();
		//SelectedRegionOrLocationType = 0; //selectedVal;

		getRegionByZoom();
		//alert(selectedVal);
	}

	





</script>

   
	 <table runat="server" width="100%">
        <tr>
            <td>
                <div class="header" id="lblTitle" runat="server">Health Map</div>
            </td>
            <td>
                <table align="right" runat="server" id="table1" >
					<tr>
						<td align="right">
							<dx:ASPxMenu ID="MenuForKey" runat="server" EnableTheming="True" 
								HorizontalAlign="Right" onitemclick="MenuForKey_ItemClick" ShowAsToolbar="True" 
								Theme="Moderno">
								<ClientSideEvents ItemClick="OnItemClick" />
								<Items>
									<dx:MenuItem Name="MainItem">
										<Items>
											<dx:MenuItem Name="ChangeKey" Text="Change Key" />
                                
										</Items>
										<Image Url="~/images/icons/Gear.png">
										</Image>
									</dx:MenuItem>
								</Items>
							</dx:ASPxMenu>
						</td>
					</tr>
				</table>
            </td>
        </tr>
		<tr>
			<td colspan="2">
				<table align="left" runat="server" id="table2" >
					<tr>
						<td align="left" valign="top">

						<dx:ASPxRadioButtonList runat="server" ID="MapSelectionRBL" ClientInstanceName="MapSeelctionRBLClient" RepeatDirection="Vertical">
							<Border BorderStyle="Solid" />
							<ClientSideEvents SelectedIndexChanged="function(s,e) { SelectedMapType=s.GetValue(); Office365StatNameComboBoxClientID.SetVisible(s.GetValue() == 0); MapTypeChanged(); }" 
								Init="function(s,e) { SelectedMapType=s.GetValue(); Office365StatNameComboBoxClientID.SetVisible(s.GetValue() == 0); MapTypeChanged(); }" />
							<Items>
								<dx:ListEditItem Text="Microsoft Office 365" Value="0" Selected="false" />
								<dx:ListEditItem Text="On-Premises" Value="1" Selected="true" />
							</Items>

						</dx:ASPxRadioButtonList>
						
						<%--<dx:ASPxComboBox runat="server" ID="Office365StatNameComboBox" ClientInstanceName="Office365StatNameComboBoxClientID">
								<ClientSideEvents SelectedIndexChanged="function(s,e) { MapTypeChanged(); }" />
						</dx:ASPxComboBox> --%>
						
						<%--
				<asp:RadioButtonList runat="server" id="MapSelectionRBL" ClientIDMode="Static" RepeatDirection="Horizontal" Visible="true"
					 >
					
					<asp:ListItem Text="Microsoft Office 365" Value=0 Enabled="true" Selected="False" />
					<asp:ListItem Text="On-Premises Applications" Value=1 Enabled="true" Selected="True" />  
					

				</asp:RadioButtonList>--%>

				

	<%--						<input id="Office365CheckBox" type="checkbox" name="Office365CheckBox" checked runat="server" onclick="SelectedMapType = this.checked; Office365StatNameComboBoxClientID.SetVisible(!this.checked); MapTypeChanged();">


							</td>--%>


						<%--	<div class="switch">
								<input id="Office365CheckBox" type="checkbox" name="Office365CheckBox" bs-switch checked runat="server" onclick="SelectedMapType = this.checked; Office365StatNameComboBoxClientID.SetVisible(!this.checked); MapTypeChanged();">
							</div>
						</td>--%>

							<%--<dx:ASPxComboBox runat="server" ID="Office365StatNameComboBox" ClientInstanceName="Office365StatNameComboBoxClientID">
								<ClientSideEvents SelectedIndexChanged="function(s,e) { MapTypeChanged(); }" />
							</dx:ASPxComboBox> --%>
							
							</td>
					</tr>
				</table>

				<table align="left" runat="server" id="table3" >
					<tr>
                        <td>&nbsp;</td>
						<td align="left" valign="top">

						<dx:ASPxComboBox runat="server" ID="Office365StatNameComboBox" ClientInstanceName="Office365StatNameComboBoxClientID">
								<ClientSideEvents SelectedIndexChanged="function(s,e) { MapTypeChanged(); }" />
						</dx:ASPxComboBox> 
					
							
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>		
			<td colspan=2>
				
				<asp:RadioButtonList runat="server" id="RegionOrLocationRBL" ClientIDMode="Static" RepeatDirection="Horizontal" Visible="false"
					>
					
					<asp:ListItem Text="Locations" Value=0 Enabled="true" Selected="True" /> 
					<asp:ListItem Text="Regions" Value=1 Enabled="true" />  

				</asp:RadioButtonList>
			</td>
		</tr>
        <tr>
            <td valign="middle" align="center">

				
				<div id='mapDiv' style="position:absolute; width: 800px; height: 400px;"></div>

				
			</td>
		</tr>
	</table>

	<script type = "text/javascript">

	    $(window).resize(function () {
	        var radioBtns = document.getElementById('<%= table1.ClientID %>').getBoundingClientRect().bottom;
	        var logo = document.getElementById('Vslogo');
	        var mapDiv = document.getElementById('mapDiv');
	        $('#mapDiv').height(($(window).height() - radioBtns - 120) + 'px');
	        //$('#mapDiv').width(($(window).width() - ($(window).width() - logo.getBoundingClientRect().right) - mapDiv.getBoundingClientRect().left) + 'px');
	        $('#mapDiv').width(($(window).width() - 80) + 'px');
	    });


		var radioBtns = document.getElementById('<%= table1.ClientID %>').getBoundingClientRect().bottom;
		var logo = document.getElementById('Vslogo');
		var mapDiv = document.getElementById('mapDiv');
		$('#mapDiv').height(($(window).height() - radioBtns - 120) + 'px');
		//$('#mapDiv').width(($(window).width() - ($(window).width() - logo.getBoundingClientRect().right) - mapDiv.getBoundingClientRect().left) + 'px');
		$('#mapDiv').width(($(window).width() - 80) + 'px');

		//(function ($) {
		//$("input[type=\"checkbox\"]").bootstrapSwitch();
		//})(jQuery);
		//(function ($) {
		//document.getElementById("Office365CheckBox").bootstrapSwitch();
		//})(jQuery);
		//$(function () {
		//alert('hi');
			//$("#Checkbox1").bootstrapSwitch();
		//});
	</script>
	
	<asp:Literal ID="Literal1" runat="server">
				</asp:Literal>
</asp:Content>
