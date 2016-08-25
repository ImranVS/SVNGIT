import {Component, ComponentResolver, OnInit} from '@angular/core';
import {ROUTER_DIRECTIVES} from '@angular/router';

import {WidgetController, WidgetContainer, WidgetContract} from '../../core/widgets';
import {AppNavigator} from '../../navigation/app.navigator.component';

declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    selector: 'sample-dashboard',
    templateUrl: '/app/dashboards/components/ibm-domino-dashboard.component.html',
    directives: [ROUTER_DIRECTIVES, WidgetContainer, AppNavigator]
})
export class IBMDominoDashboard extends WidgetController implements OnInit {

    widgets: WidgetContract[] = [
        {
            id: 'greetings',
            title: 'Say hello',
            path: '/app/widgets/charts/components/chart.component',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-12 col-md-6',
            settings: {
                url: '/mobile_user_devices/count_by_type',
                chart: {
                    chart: {
                        renderTo: 'greetings',
                        type: 'bar',
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
            id: 'greetings2',
            title: 'Say hello 2',
            path: '/app/widgets/charts/components/chart.component',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-12 col-md-6',
            settings: {
                url: '/mobile_user_devices/count_by_type',
                chart: {
                    chart: {
                        renderTo: 'greetings2',
                        type: 'line',
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
    
    constructor(protected resolver: ComponentResolver) {
        super(resolver);
    }

    ngOnInit() {
        injectSVG();
        bootstrapNavigator();
    }

}