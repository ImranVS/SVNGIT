﻿import {Component, ComponentResolver, OnInit} from '@angular/core';
import {ROUTER_DIRECTIVES} from '@angular/router';

import 'rxjs/Rx';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../core/widgets';
import {AppNavigator} from '../../navigation/app.navigator.component';

declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    selector: 'traveler-dashboard',
    templateUrl: '/app/dashboards/components/overall-dashboard.component.html',
    directives: [ROUTER_DIRECTIVES, WidgetContainer, AppNavigator],
    providers: [WidgetService]
})
export class OverallDashboard extends WidgetController implements OnInit {

    widgetAppsStatus: WidgetContract[] = [
        {
            id: 'widgetOnPremisesApps1',
            path: '/app/widgets/main-dashboard/components/app-status.component',
            name: 'AppStatus',
            settings: {
                serviceId: 1
            }
        },
        {
            id: 'widgetOnPremisesApps2',
            path: '/app/widgets/main-dashboard/components/app-status.component',
            name: 'AppStatus',
            settings: {
                serviceId: 2
            }
        },
        {
            id: 'widgetOnPremisesApps3',
            path: '/app/widgets/main-dashboard/components/app-status.component',
            name: 'AppStatus',
            settings: {
                serviceId: 3
            }
        },
        {
            id: 'widgetOnPremisesApps4',
            path: '/app/widgets/main-dashboard/components/app-status.component',
            name: 'AppStatus',
            settings: {
                serviceId: 4
            }
        }
    ]

    widgetOnPremisesApps: WidgetContract = {
        id: 'widgetOnPremisesApps',
        title: 'On premises applications',
        path: '/app/widgets/main-dashboard/components/on-premises-apps.component',
        name: 'OnPremisesApps',
        css: 'col-md-6 col-lg-12',
        settings: {}
    }

    widgetStatusSummary: WidgetContract = {
        id: 'widgetStatusSummary',
        path: '/app/widgets/main-dashboard/components/status-summary.component',
        name: 'StatusSummary',
        settings: {}
    }

    weeklyEvents: WidgetContract = {
        id: 'weeklyEventsRepartitionChartWrapper',
        title: 'Weekly events repartition',
        path: '/app/widgets/charts/components/chart.component',
        name: 'ChartComponent',
        css: 'col-lg-12 col-md-6 col-sm-6',
        settings: {
            url: '/server_health/weekly_events',
            chart: {
                chart: {
                    renderTo: 'weeklyEventsRepartitionChartWrapper',
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
    }

    lastMonthEvents: WidgetContract = {
        id: 'eventsCountChartWrapper',
        title: 'Last month events',
        path: '/app/widgets/charts/components/chart.component',
        name: 'ChartComponent',
        css: 'col-md-6 col-lg-12',
        settings: {
            url: '/server_health/last_month_events',
            chart: {
                chart: {
                    renderTo: 'eventsCountChartWrapper',
                    type: 'column',
                    height: 270
                },
                title: { text: '' },
                subtitle: { text: '' },
                credits: { enabled: false },
                exporting: { enabled: false },
                legend: { enabled: false },
                tooltip: {
                    formatter: function () {
                        return '<div style="font-size: 11px; font-weight: normal;">' + this.x + '<br /><strong>' + this.y + '</strong></div>';
                    },
                    useHTML: true
                },
                xAxis: {
                    categories: []
                },
                yAxis: {
                    title: { text: '' }
                },
                series: []
            }
        }
    }

    constructor(protected resolver: ComponentResolver, protected widgetService: WidgetService) {

        super(resolver, widgetService);
        
    }

    ngOnInit() {
    
        injectSVG();
        bootstrapNavigator();

    }

}