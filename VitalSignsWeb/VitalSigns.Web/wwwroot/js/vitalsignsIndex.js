$(function () {
										

	// Force the wharts to update thier width to fit their containers
	// when the menu is toggled
	zeus.eventManager.on('toggleMenu', 'updateCharts', function () {

		window.dispatchEvent(new Event('resize'));

	});


	var mobileDevices = new Highcharts.Chart({
		chart: {
			renderTo: 'mobileDevicesChart',
			type: 'bar',
			height: 240
		},
		title: {text: ''},
		subtitle: {text: ''},
		xAxis: {
			categories: [
				'iPhone 5S',
				'iPhone 4S',
				'Samsung Galaxy S4',
				'Unknown',
				'Samsung Galaxy Note',
				'iPhone 6+',
				'iphone 5',
				'iPhone 4',
				'iPad 3',
				'iPad 2',
				'Apple-iPhone8C2',
				'Apple-iPhone8C1'
			]
		},
		yAxis: {
			min: 0,
			endOnTick: false,
			allowDecimals: false,
			title: {
				enabled: false
			}
		},
		plotOptions: {
			bar: {
				dataLabels: {
					enabled: false
				},
				groupPadding: 0.1,
				borderWidth: 0
			},
			series: {
				pointPadding: 0
			}
		},
		legend: {
			enabled: false
		},
		credits: {
			enabled: false
		},
		exporting: {
			enabled: false
		},
		series: [
			{
				name: '',
				data: [
					{y: 3,color: 'rgba(95, 190, 127, 1)'},
					{y: 3,color: 'rgba(95, 190, 127, 1)'},
					{y: 2,color: 'rgba(95, 190, 127, 0.75)'},
					{y: 2,color: 'rgba(95, 190, 127, 0.75)'},
					{y: 2,color: 'rgba(95, 190, 127, 0.75)'},
					{y: 1,color: 'rgba(95, 190, 127, 0.50)'},
					{y: 1,color: 'rgba(95, 190, 127, 0.50)'},
					{y: 1,color: 'rgba(95, 190, 127, 0.50)'},
					{y: 1,color: 'rgba(95, 190, 127, 0.50)'},
					{y: 1,color: 'rgba(95, 190, 127, 0.50)'},
					{y: 1,color: 'rgba(95, 190, 127, 0.50)'},
					{y: 1,color: 'rgba(95, 190, 127, 0.50)'}
				]
			}
		]
	});


	var mobileDevicesOS = new Highcharts.Chart({
		chart: {
			renderTo: 'mobileDevicesOSChart',
			type: 'bar',
			height: 240
		},
		title: {text: ''},
		subtitle: {text: ''},
		xAxis: {
			categories: [
				'iOS 9.1',
				'iOS 8.4',
				'Android 4.4',
				'iOS 9.0',
				'iOS 8.3',
				'iOS 8.2',
				'iOS 7.2',
				'Android 5.1',
				'Android 5.0'
			]
		},
		yAxis: {
			min: 0,
			endOnTick: false,
			allowDecimals: false,
			title: {
				enabled: false
			}
		},
		plotOptions: {
			bar: {
				dataLabels: {
					enabled: false
				},
				groupPadding: 0.1,
				borderWidth: 0
			},
			series: {
				pointPadding: 0
			}
		},
		legend: {
			enabled: false
		},
		credits: {
			enabled: false
		},
		exporting: {
			enabled: false
		},
		series: [
			{
				name: '',
				data: [
					{y: 5,color: 'rgba(95, 190, 127, 1)'},
					{y: 3,color: 'rgba(95, 190, 127, 1)'},
					{y: 2,color: 'rgba(95, 190, 127, 0.75)'},
					{y: 1,color: 'rgba(95, 190, 127, 0.75)'},
					{y: 1,color: 'rgba(95, 190, 127, 0.75)'},
					{y: 1,color: 'rgba(95, 190, 127, 0.50)'},
					{y: 1,color: 'rgba(95, 190, 127, 0.50)'},
					{y: 1,color: 'rgba(95, 190, 127, 0.50)'},
					{y: 1,color: 'rgba(95, 190, 127, 0.50)'}
				]
			}
		]
	});


	
	var syncTimeChart = new Highcharts.Chart({
		chart: {
			renderTo: 'syncTimeChart',
			type: 'pie',
			height: 300
		},
		title: {text: ''},
		subtitle: {text: ''},
		credits: {
			enabled: false
		},
		exporting: {
			enabled: false
		},
		plotOptions: {
			pie: {
				allowPointSelect: true,
				cursor: 'pointer',
				dataLabels: {
					enabled: false
				},
				showInLegend: true
			}
		},
		tooltip: {
			formatter: function () {
				return '<div style="font-size: 11px; font-weight: normal;">' + this.key + '<br /><strong>' + this.y + '</strong> (' + this.percentage.toFixed(1) + '%)</div>';
			},
			useHTML: true
		},
		legend: {
			labelFormatter: function () {
				return '<div style="font-size: 10px; font-weight: normal;">' + this.name + '</div>';
			}
		},
		series: [
			{
				name: 'Sync time',
				data: [
					{
						name: 'Greater than 120 mins.',
						y: 120,
						color: '#5fbe7f'
					},
					{
						name: 'Between 15-30 mins.',
						y: 50,
						color: '#666766'
					},
					{
						name: 'Within 15 mins.',
						y: 30,
						color: '#ef3a24'
					}
				],
				innerSize: '70%'
			}
		]
	});


	var deviceCountUserChart = new Highcharts.Chart({
		chart: {
			renderTo: 'deviceCountUserChart',
			type: 'pie',
			height: 300
		},
		title: {text: ''},
		subtitle: {text: ''},
		credits: {
			enabled: false
		},
		exporting: {
			enabled: false
		},
		plotOptions: {
			pie: {
				allowPointSelect: true,
				cursor: 'pointer',
				dataLabels: {
					enabled: false
				},
				showInLegend: true
			}
		},
		tooltip: {
			formatter: function () {
				return '<div style="font-size: 11px; font-weight: normal;">' + this.key + '<br /><strong>' + this.y + '</strong> (' + this.percentage.toFixed(1) + '%)</div>';
			}
		},
		legend: {
			labelFormatter: function () {
				return '<div style="font-size: 10px; font-weight: normal;">' + this.name + '</div>';
			}
		},
		series: [
			{
				name: 'Sync time',
				data: [
					{
						name: 'Users with only 1 device',
						y: 250,
						color: '#5fbe7f'
					},
					{
						name: 'Users with 2 devices',
						y: 60,
						color: '#ef3a24'
					}
				],
				innerSize: '70%'
			}
		]
	});

	

});