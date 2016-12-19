import {Component, ComponentFactoryResolver, Input, OnInit} from '@angular/core';

import 'rxjs/Rx';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../../core/widgets';
import {AppNavigator} from '../../../navigation/app.navigator.component';

declare var injectSVG: any;

@Component({
    selector: 'websphere-dashboard',
    templateUrl: '/app/dashboards/components/ibm-websphere/ibm-websphere-dashboard.component.html',
    providers: [WidgetService]
})

export class IBMWebsphereDashboard extends WidgetController implements OnInit {
    serviceId: string;

    widgets: WidgetContract[] = [
        {
            id: 'websphereGrid',
            title: 'Cells',
            name: 'IBMWebsphereGrid',
            css: 'col-xs-12 col-sm-12  col-md-12 col-lg-8',
            settings: {

            }
        },
        {
            id: 'serverStatus',
            title: 'Status',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
            settings: {
                url: '/services/status_count?type=[WebSphereCell,WebSphereNode,WebSphere]&docfield=status_code',
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
            id: 'websphereNodeGrid',
            title: 'Nodes',
            name: 'IBMWebsphereNodeGrid',
            css: 'col-xs-12 col-sm-12  col-md-12 col-lg-8',
            settings: {

            }
        },
        {
            id: 'activeThreads',
            title: 'Active Thread Count',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
            settings: {
                url: '/services/statistics?statName=ActiveThreadCount&operation=AVG&isChart=true',
                chart: {
                    chart: {
                        renderTo: 'activeThreads',
                        type: 'bar',
                        height: 240
                    },
                    colors: ['#5fbe7f'],
                    title: { text: '' },
                    subtitle: { text: '' },
                    xAxis: {
                        labels: {
                            step: 1
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
            id: 'websphereServerGrid',
            title: 'Servers',
            name: 'IBMWebsphereServerGrid',
            css: 'col-xs-12 col-sm-12  col-md-12 col-lg-8',
            settings: {

            }
        },
        {
            id: 'hungThreads',
            title: 'Hung Thread Count',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
            settings: {
                url: '/services/statistics?statName=CurrentHungThreadCount&operation=AVG&isChart=true',
                chart: {
                    chart: {
                        renderTo: 'hungThreads',
                        type: 'bar',
                        height: 240
                    },
                    colors: ['#5fbe7f'],
                    title: { text: '' },
                    subtitle: { text: '' },
                    xAxis: {
                        labels: {
                            step: 1
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
        }
    ]
    
    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {
        super(resolver, widgetService);
    }

    ngOnInit() {
        injectSVG();
        
    }

    onSelect(serviceId: string) {

        this.serviceId = serviceId;

    }
}