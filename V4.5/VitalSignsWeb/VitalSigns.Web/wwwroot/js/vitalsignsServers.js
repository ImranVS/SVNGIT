$(function () {
	

	// Force the wharts to update thier width to fit their containers
	// when the menu is toggled
	zeus.eventManager.on('toggleMenu', 'updateCharts', function () {

		window.dispatchEvent(new Event('resize'));

	});


	$('#zeusContent')
	.off('click', '#serversListWrapper .aServer')
	.on('click', '#serversListWrapper .aServer', function () {

		$('.aServer').removeClass('selected');
		$(this).addClass('selected');

		if ($('#noServerSelectedWrapper').is(":visible")) {
			$('#noServerSelectedWrapper').hide();
		}

		if (!$('#serverDetail').is(":visible")) {
			$('#serverDetail').show();
		}

		if (zeus.interface.resolution().isMobile) {
			$('#zeusWrapper').animate({
				scrollTop: $('.serversList').height() + $('#zeusContext').height() + $('#zeusHeader').height() + 21
			}, 500);
		}
		

		window.dispatchEvent(new Event('resize'));

	});




	var usersConnectionDuringTheDay = new Highcharts.Chart({
		chart: {
			renderTo: 'usersConnectionsDuringTheDay',
			type: 'areaspline',
			height: 300
		},
		colors: ['#5fbe7f'],
		title: {text: ''},
		subtitle: {text: ''},
		xAxis: {
			labels: {
				step: 6
			},
			categories: ['12 AM','01 AM','02 AM','03 AM','04 AM','05 AM','06 AM','07 AM','08 AM','09 AM','10 AM','11 AM','12 PM','01 PM','02 PM','03 PM','04 PM','05 PM','06 PM','07 PM','08 PM','09 PM','10 PM','11 PM']
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
		series: [{
			name: 'Connections',
			data: [236, 198, 180, 150, 99, 88, 89, 150, 260, 1699, 1365, 789, 899, 563, 236, 895, 1268, 1785, 2635, 2234, 695, 236, 153, 89]
		}]
	});


	var diskSpace = new Highcharts.Chart({
		chart: {
			renderTo: 'diskSpace',
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
				name: 'Go',
				data: [
					{
						name: 'Free space',
						y: 12.63,
						color: '#5fbe7f'
					},
					{
						name: 'Used space',
						y: 168,
						color: '#ef3a24'
					}
				],
				innerSize: '70%'
			}
		]
	});




	var cpuUsage = new Highcharts.Chart({
		chart: {
			renderTo: 'cpuUsage',
			type: 'areaspline',
			height: 300
		},
		colors: ['#ef3a24'],
		title: {text: ''},
		subtitle: {text: ''},
		xAxis: {
			labels: {
				step: 6
			},
			categories: ['12 AM','01 AM','02 AM','03 AM','04 AM','05 AM','06 AM','07 AM','08 AM','09 AM','10 AM','11 AM','12 PM','01 PM','02 PM','03 PM','04 PM','05 PM','06 PM','07 PM','08 PM','09 PM','10 PM','11 PM']
		},
		yAxis: {
			min:0,
			max: 100
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
		series: [{
			name: '% Usage',
			data: [12, 18, 63, 25, 22, 29, 45, 26, 31, 78, 80, 65, 52, 25, 12, 36, 68, 89, 95, 62, 21, 15, 5, 3]
		}]
	});


	var memoryUsage = new Highcharts.Chart({
		chart: {
			renderTo: 'memoryUsage',
			type: 'areaspline',
			height: 300
		},
		colors: ['#848484'],
		title: {text: ''},
		subtitle: {text: ''},
		xAxis: {
			labels: {
				step: 6
			},
			categories: ['12 AM','01 AM','02 AM','03 AM','04 AM','05 AM','06 AM','07 AM','08 AM','09 AM','10 AM','11 AM','12 PM','01 PM','02 PM','03 PM','04 PM','05 PM','06 PM','07 PM','08 PM','09 PM','10 PM','11 PM']
		},
		yAxis: {
			min:0,
			max: 32
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
		series: [{
			name: 'GB',
			data: [1.5, 4.9, 1.7, 1.8, 2.6, 4.8, 8.9, 5.2, 3.5, 18.2, 26.9, 30.5, 16.5, 8.4, 5.2, 2.5, 0.5, 5.9, 18.9, 25.6, 18.6, 4.3, 2.4, 3.6]
		}]
	});




});