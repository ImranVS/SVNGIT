import {Component, ComponentResolver, OnInit, Injector} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../core/widgets';

import {ServiceTab} from '../models/service-tab.interface';

declare var injectSVG: any;

@Component({
    selector: 'tab-overall',
    templateUrl: '/app/services/components/service-overall-tab.component.html',
    directives: [WidgetContainer],
    providers: [WidgetService]
})
export class ServiceOverallTab extends WidgetController implements OnInit, ServiceTab {

    widgets: WidgetContract[];
    serviceId: string;
    deviceId: any;

    constructor(protected resolver: ComponentResolver, protected widgetService: WidgetService) {
        super(resolver, widgetService);
    }
    
    ngOnInit() {
    
        this.widgets = [
            {
                id: 'usersConnectionsDuringTheDay',
                title: 'Users connections during the day',
                path: '/app/widgets/charts/components/chart.component',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                settings: {
                    url: '/services/statistics?statname=Server.Users&deviceId=57ace45abf46711cd4681e01&operation=hourly',
                    chart: {
                        chart: {
                            renderTo: 'usersConnectionsDuringTheDay',
                            type: 'areaspline',
                            height: 300
                        },
                        colors: ['#5fbe7f'],
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            labels: {
                                step: 6
                            },
                            categories: []
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
                        series: []
                    }
                }
            },
            {
                id: 'diskSpace',
                title: 'Disk space',
                path: '/app/widgets/charts/components/chart.component',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                settings: {
                    url: '/services/1/overall/disk-space',
                    chart: {
                        chart: {
                            renderTo: 'diskSpace',
                            type: 'pie',
                            height: 300
                        },
                        title: { text: '' },
                        subtitle: { text: '' },
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
                                data: [],
                                innerSize: '70%'
                            }
                        ]
                    }
                }
            },
            {
                id: 'cpuUsage',
                title: 'CPU Usage',
                path: '/app/widgets/charts/components/chart.component',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                settings: {
                    url: `/services/statistics?statname=Platform.System.PctCombinedCpuUtil&deviceId=${this.serviceId}&operation=hourly`,
                    chart: {
                        chart: {
                            renderTo: 'cpuUsage',
                            type: 'areaspline',
                            height: 300
                        },
                        colors: ['#ef3a24'],
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            labels: {
                                step: 6
                            },
                            categories: []
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
                            data: []
                        }]
                    }
                }
            },
            {
                id: 'memoryUsage',
                title: 'Memory Usage',
                path: '/app/widgets/charts/components/chart.component',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                settings: {
                    url: `/services/statistics?statname=Mem.PercentUsed&deviceId=${this.serviceId}&operation=hourly`,
                    chart: {
                        chart: {
                            renderTo: 'memoryUsage',
                            type: 'areaspline',
                            height: 300
                        },
                        colors: ['#848484'],
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            labels: {
                                step: 6
                            },
                            categories: []
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
                            data: []
                        }]
                    }
                }
            }
        ];
    
        injectSVG();
    }

}