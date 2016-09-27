import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import 'rxjs/Rx';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../../core/widgets';
import {AppNavigator} from '../../../navigation/app.navigator.component';

declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    selector: 'traveler-dashboard',
    templateUrl: '/app/dashboards/components/ibm-traveler/ibm-traveler-dashboard.component.html',
    providers: [WidgetService]
})
export class IBMTravelerDashboard extends WidgetController implements OnInit {

    widgets: WidgetContract[] = [
        {
            id: 'mobileUsersGrid',
            title: 'Mobile users',
            name: 'MobileUsersGrid',
            css: 'col-xs-12 col-sm-12 col-md-12 col-lg-8',
            settings: {}
        },
        {
            id: 'mobileDevicesChart',
            title: 'Mobile devices',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
            settings: {
                url: '/DashBoard/mobile_user_devices/count_by_type',
                chart: {
                    chart: {
                        renderTo: 'mobileDevicesChart',
                        type: 'pie',
                        height: 300
                    },
                    title: { text: '' },
                    subtitle: { text: '' },
                    xAxis: {
                        categories: []
                    },
                    yAxis: {
                        min: 0,
                        endOnTick: false,
                        allowDecimals: false,
                        title: {
                            enabled: false
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
                    series: []
                }
            }
        },
        {
            id: 'mobileDevicesOSChart',
            title: 'Mobile devices OS for all Servers',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
            settings: {
                url: '/DashBoard/mobile_user_devices/count_by_os',
                chart: {
                    chart: {
                        renderTo: 'mobileDevicesOSChart',
                        type: 'pie',
                        height: 300
                    },
                    title: { text: '' },
                    subtitle: { text: '' },
                    xAxis: {
                        categories: []
                    },
                    yAxis: {
                        min: 0,
                        endOnTick: false,
                        allowDecimals: false,
                        title: {
                            enabled: false
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
                    series: []
                }
            }
        },
        {
            id: 'syncTimeChart',
            title: 'Sync times',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-2',
            settings: {
                url: '/DashBoard/mobile_user_devices/group_by_sync_interval',
                chart: {
                    chart: {
                        renderTo: 'syncTimeChart',
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
                            showInLegend: true,
                            innerSize: '70%'
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
                    series: []
                }
            }
        },
        {
            id: 'deviceCountUserChart',
            title: 'Device count / user',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-2',
            settings: {
                url: '/DashBoard/mobile_user_devices/count_per_user',
                chart: {
                    chart: {
                        renderTo: 'deviceCountUserChart',
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
                            showInLegend: true,
                            innerSize: '70%'
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
                    series: []
                }
            }
        }
    ]

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {
        super(resolver, widgetService);
    }

    ngOnInit() {
        injectSVG();
        bootstrapNavigator();
    }

}