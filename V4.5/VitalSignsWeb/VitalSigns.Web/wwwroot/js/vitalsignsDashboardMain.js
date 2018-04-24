$(function () {

	// Force the wharts to update thier width to fit their containers
	// when the menu is toggled
	zeus.eventManager.on('toggleMenu', 'updateCharts', function () {

		window.dispatchEvent(new Event('resize'));

	});

	var eventsCountChart = new Highcharts.Chart({
		chart: {
			renderTo: 'eventsCountChartWrapper',
			type: 'column',
			height: 270
		},
		title: {text: ''},
		subtitle: {text: ''},
		credits: {enabled: false},
		exporting: {enabled: false},
		legend: { enabled: false },
		tooltip: {
			formatter: function () {
				return '<div style="font-size: 11px; font-weight: normal;">' + this.x + '<br /><strong>' + this.y + '</strong></div>';
			},
			useHTML: true
		},
		xAxis: {
			categories: ['No issues', 'Maintenance', 'Not responding', 'Issues']
		},
		yAxis: {
			title: { text:'' }
		},
		series: [
			{
				data: [
					{
						y: 12,
						color: '#5fbe7f'
					},
					{
						y: 8,
						color: '#666766'
					},
					{
						y: 7,
						color: '#ef3a24'
					},
					{
						y: 10,
						color: '#f99c1c'
					}


				]
			}
		]
	});

	var weeklyEventsRepartitionChart = new Highcharts.Chart({
		chart: {
			renderTo: 'weeklyEventsRepartitionChartWrapper',
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
						name: 'Not responding',
						y: 120,
						color: '#5fbe7f'
					},
					{
						name: 'Maintenance',
						y: 50,
						color: '#666766'
					},
					{
						name: 'No issues',
						y: 30,
						color: '#ef3a24'
					},
					{
						name: 'Issues',
						y: 30,
						color: '#f99c1c'
					}


				],
				innerSize: '70%'
			}
		]
	});
	
});