import {Component, ComponentFactoryResolver, OnInit, ViewChild} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../../core/widgets';
import {AppNavigator} from '../../../navigation/app.navigator.component';

declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    templateUrl: '/app/dashboards/components/key-metrics/users-dashboard.component.html',
    providers: [WidgetService]
})
export class UsersDashboard extends WidgetController implements OnInit  {
    widgets: WidgetContract[]
    

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {
        super(resolver, widgetService);
    }

    ngOnInit() {

        var todaysDate = new Date();
        //var endDate = todaysDate.toISOString().slice(0, 10);
        var endDate = todaysDate.toISOString();
        todaysDate.setMonth(todaysDate.getMonth() - 1);
        //var startDate = todaysDate.toISOString().slice(0, 10);
        var startDate = todaysDate.toISOString();
        //console.log(startDate);
        this.widgets = [
            {
                id: 'userCount',
                title: 'User Count',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                settings: {
                    url: '/services/status_list?type=Domino&docfield=user_count&isChart=true',
                    chart: {
                        chart: {
                            renderTo: 'userCount',
                            type: 'bar',
                            height: 440
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
                        plotOptions: {
                            bar: {
                                dataLabels: {
                                    enabled: false
                                },
                                groupPadding: 0.1,
                                borderWidth: 0
                            },
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
                id: 'costPerUser',
                title: 'Cost per User Served',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                settings: {
                    url: `/dashboard/users/cost_per_user?startDate=${startDate}&endDate=${endDate}&isChart=true`,
                    chart: {
                        chart: {
                            renderTo: 'costPerUser',
                            type: 'bar',
                            height: 440
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
                        plotOptions: {
                            bar: {
                                dataLabels: {
                                    enabled: false
                                },
                                groupPadding: 0.1,
                                borderWidth: 0
                            },
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
            }
        ]
        injectSVG();
        bootstrapNavigator();
    }

    setSort1(byCount: boolean) {
        //console.log(byCount);
        if (byCount) {
            this.widgetService.refreshWidget('userCount', `/services/status_list?type=Domino&docfield=user_count&sortby=UserCount&isChart=true`)
                .catch(error => console.log(error));
        }
        else {
            this.widgetService.refreshWidget('userCount', `/services/status_list?type=Domino&docfield=user_count&isChart=true`)
                .catch(error => console.log(error));
        }
    }

    setSort2(byCost: boolean) {
        //console.log(byCount);
        var todaysDate = new Date();
        var endDate = todaysDate.toISOString();
        todaysDate.setMonth(todaysDate.getMonth() - 1);
        var startDate = todaysDate.toISOString();

        if (byCost) {
            //console.log(`/dashboard/users/cost_per_user?sortby=cost_per_user&startDate=${startDate}&endDate=${endDate}&isChart=true`);
            this.widgetService.refreshWidget('costPerUser', `/dashboard/users/cost_per_user?sortby=cost_per_user&startDate=${startDate}&endDate=${endDate}&isChart=true`)
                .catch(error => console.log(error));
        }
        else {
            this.widgetService.refreshWidget('costPerUser', `/dashboard/users/cost_per_user?startDate=${startDate}&endDate=${endDate}&isChart=true`)
                .catch(error => console.log(error));
        }
    }
}