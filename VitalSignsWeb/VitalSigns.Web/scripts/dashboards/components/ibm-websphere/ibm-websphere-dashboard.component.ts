import {Component, ComponentFactoryResolver, Input, OnInit} from '@angular/core';

import 'rxjs/Rx';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../../core/widgets';
import {AppNavigator} from '../../../navigation/app.navigator.component';

declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    selector: 'websphere-dashboard',
    templateUrl: '/app/dashboards/components/ibm-websphere/ibm-websphere-dashboard.component.html',
    providers: [WidgetService]
})

export class IBMWebsphereDashboard extends WidgetController implements OnInit {

    widgets: WidgetContract[] = [
        {
            id: 'websphereGrid',
            title: 'Cells',
            name: 'IBMWebsphereGrid',
            css: 'col-xs-12 col-sm-12  col-md-12 col-lg-6',
            settings: {

            }
        },
        {
            id: 'serverStatus',
            title: 'Status',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-2',
            settings: {
                url: '/services/status_count?type=WebSphere&docfield=status_code',
                chart: {
                    chart: {
                        renderTo: 'serverStatus',
                        type: 'pie',
                        height: 240
                    },
                    title: { text: '' },
                    subtitle: { text: '' },
                    xAxis: {
                        categories: []
                    },
                    yAxis: {

                    },
                    plotOptions: {
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
                    series: []
                }
            }
        },
        {
            id: 'activeThreads',
            title: 'Active Thread Count',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-2',
            settings: {
                url: '/services/status_count?type=WebSphere&docfield=status_code',
                chart: {
                    chart: {
                        renderTo: 'activeThreads',
                        type: 'pie',
                        height: 240
                    },
                    title: { text: '' },
                    subtitle: { text: '' },
                    xAxis: {
                        categories: []
                    },
                    yAxis: {

                    },
                    plotOptions: {
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
                    series: []
                }
            }
        },
        {
            id: 'hungThreads',
            title: 'Hung Thread Count',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-2',
            settings: {
                url: '/services/status_count?type=WebSphere&docfield=status_code',
                chart: {
                    chart: {
                        renderTo: 'hungThreads',
                        type: 'pie',
                        height: 240
                    },
                    title: { text: '' },
                    subtitle: { text: '' },
                    xAxis: {
                        categories: []
                    },
                    yAxis: {

                    },
                    plotOptions: {
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
                    series: []
                }
            }
        },
        {
            id: 'websphereServerGrid',
            title: 'Servers',
            name: 'IBMWebsphereServerGrid',
            css: 'col-xs-12 col-sm-12  col-md-12 col-lg-7',
            settings: {

            }
        },
        {
            id: 'websphereNodeGrid',
            title: 'Nodes',
            name: 'IBMWebsphereNodeGrid',
            css: 'col-xs-12 col-sm-12  col-md-12 col-lg-5',
            settings: {

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