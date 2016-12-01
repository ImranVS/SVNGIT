import {Component, ComponentFactoryResolver, OnInit, Injector} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../core/widgets';

import {ServiceTab} from '../models/service-tab.interface';

declare var injectSVG: any;

@Component({
    selector: 'tab-overall',
    templateUrl: '/app/services/components/service-overall-tab.component.html',
    providers: [WidgetService]
})
export class ServiceOverallTab extends WidgetController implements OnInit, ServiceTab {

    widgets: WidgetContract[];
    serviceId: string;
    deviceId: any;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {
        super(resolver, widgetService);
    }
    
    ngOnInit() {
    
        this.widgets = [
            {
                id: 'usersConnectionsDuringTheDay',
                title: 'Users connections during the day',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                settings: {
                    url: `/services/statistics?statname=Server.Users&deviceId=${this.serviceId}&operation=hourly`,
                    dateformat: 'time',
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
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                settings: {
                    url: `/services/disk_space?deviceid=${this.serviceId}`,
                    dateformat: 'time',
                   
                    chart: {
                        chart: {
                            renderTo: 'diskSpace',
                            type: 'bar',
                            height: 300
                        },
                        title: { text: '' },
                        subtitle: { text: '' },
                        credits: {
                            enabled: false
                        },
                        xAxis: {
                             categories: []
                        },
                        yAxis: {
                            min: 0,
                            title: {
                                text: 'Disk Space (GB)'
                            }
                        },
                        exporting: {
                            enabled: false
                        },
                        plotOptions: {
                            pie: {
                                allowPointSelect: true,
                                cursor: 'pointer',
                                dataLabels: {
                                    enabled: true,
                                    format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                                    style: {
                                        color: 'black'
                                    }
                                }
                            },
                            series: {
                                stacking: 'normal'
                            }
                        },
                        tooltip: {
                            formatter: function () {
                                return '<div style="font-size: 11px; font-weight: normal;">' + this.series.name + '<br /><strong>' + this.y.toFixed(2) + '</strong> (' + this.percentage.toFixed(1) + '%)</div>';
                            },
                            useHTML: true
                        },
                        series: [],
                        colors: ['#5FBE7F', '#EF3A24']
                    }
                }
            },
            {
                id: 'cpuUsage',
                title: 'CPU Usage',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                settings: {
                    url: `/services/statistics?statname=Platform.System.PctCombinedCpuUtil&deviceId=${this.serviceId}&operation=hourly`,
                    dateformat: 'time',
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
                        yAxis: {
                            title: {
                                text: 'Percent'
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
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                settings: {
                    url: `/services/statistics?statname=Mem.PercentUsed&deviceId=${this.serviceId}&operation=hourly`,
                    dateformat: 'time',
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
                        yAxis: {
                            title: {
                                text: 'Percent'
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