﻿import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import 'rxjs/Rx';

import {WidgetController, WidgetContainer, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {AppNavigator} from '../../../navigation/app.navigator.component';

declare var injectSVG: any;


@Component({
    selector: 'mobile-users-dashboard',
    templateUrl: '/app/dashboards/components/mobile-users/mobile-users-dashboard.component.html',
    providers: [WidgetService]
})
export class MobileUsersDashboard extends WidgetController implements OnInit {

    dashboardTitle: string = 'IBM Mobile Users';
    dashboardSubtitle: string = 'IBM Mobile Users dashboard';

    widgets: WidgetContract[] = [
        {
            id: 'mobileUsersGrid',
            title: 'All Mobile Users',
            name: 'MobileUsersGrid',
            css: 'col-xs-12 col-sm-12 col-md-12 col-lg-6',
            settings: {}
        },
        {
            id: 'mobileUsersKeyUserGrid',
            title: 'Key Mobile Users',
            name: 'MobileUsersKeyUserGrid',
            css: 'col-xs-12 col-sm-12 col-md-12 col-lg-6',
            settings: {}
        },
        {
            id: 'mobileDevicesChart',
            title: 'Mobile devices',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-3',
            settings: {
                url: '/DashBoard/mobile_user_devices/count_by_type',
                chart: {
                    chart: {
                        renderTo: 'mobileDevicesChart',
                        type: 'pie',
                        height: 240
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
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-3',
            settings: {
                url: '/DashBoard/mobile_user_devices/count_by_os',
                chart: {
                    chart: {
                        renderTo: 'mobileDevicesOSChart',
                        type: 'pie',
                        height: 240
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
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-3',
            settings: {
                url: '/DashBoard/mobile_user_devices/group_by_sync_interval',
                chart: {
                    chart: {
                        renderTo: 'syncTimeChart',
                        type: 'pie',
                        height: 240
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
            id: 'deviceCountUserChart',
            title: 'Device count / user',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-3',
            settings: {
                url: '/DashBoard/mobile_user_devices/count_per_user',
                chart: {
                    chart: {
                        renderTo: 'deviceCountUserChart',
                        type: 'pie',
                        height: 240
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
        }
    ]

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {

        super(resolver, widgetService);

    }

    ngOnInit() {

        injectSVG();
        

    }

    printReport() {

        var doc = new wijmo.PrintDocument({
            title: this.dashboardTitle
        });

        doc.append(`<h1>${this.dashboardSubtitle}</h1>`);

        this.widgets.forEach(widget => {

            if (widget.name == 'ChartComponent') {

                doc.append(`<h2>${widget.title}</h2>`);

                var view = <HTMLElement>document.querySelector(`#${widget.id}`);
                
                for (var i = 0; i < view.children.length; i++) {

                    doc.append(view.children[i]);

                }

            }

        });
        
        doc.print();

    }

}