import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import 'rxjs/Rx';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../core/widgets';
import {AppNavigator} from '../../navigation/app.navigator.component';

declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    selector: 'traveler-dashboard',
    templateUrl: '/app/dashboards/components/overall-dashboard.component.html',
    providers: [WidgetService]
})
export class OverallDashboard extends WidgetController implements OnInit {

    widgetAppsStatus: WidgetContract[] = [
        {
            id: 'widgetOnPremisesApps1',
            name: 'AppStatus',
            settings: {
                serviceId: 1
            }
        },
        {
            id: 'widgetOnPremisesApps2',
            name: 'AppStatus',
            settings: {
                serviceId: 2
            }
        },
        {
            id: 'widgetOnPremisesApps3',
            name: 'AppStatus',
            settings: {
                serviceId: 3
            }
        },
        {
            id: 'widgetOnPremisesApps4',
            name: 'AppStatus',
            settings: {
                serviceId: 4
            }
        }
    ]

    widgetOnPremisesApps: WidgetContract = {
        id: 'widgetOnPremisesApps',
        title: 'On premises applications',
        name: 'OnPremisesApps',
        css: 'col-md-6 col-lg-12',
        settings: {}
    }

    widgetStatusSummary: WidgetContract = {
        id: 'widgetStatusSummary',
        name: 'StatusSummary',
        settings: {}
    }

    widgetUsersSessions: WidgetContract = 
        {
        id: 'widgetUserSessions',
        name: 'UserSessions',
        settings: {}
        }
       

    weeklyEvents: WidgetContract = {
        id: 'weeklyEventsRepartitionChartWrapper',
        title: 'Weekly events repartition',
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

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {

        super(resolver, widgetService);
        
    }

    ngOnInit() {
    
        injectSVG();
        bootstrapNavigator();

    }

}