import {Component, ComponentResolver, OnInit} from '@angular/core';
import {ROUTER_DIRECTIVES} from '@angular/router';

import {WidgetController, WidgetContainer, WidgetContract} from '../../core/widgets';
import {AppNavigator} from '../../navigation/app.navigator.component';

declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    templateUrl: '/app/dashboards/components/office365-mail-statistics.component.html',
    directives: [ROUTER_DIRECTIVES, WidgetContainer, AppNavigator]
})
export class OfficeMailStatistics extends WidgetController implements OnInit {

    widgets: WidgetContract[] = [
        {
            id: 'top5ActiveMailboxes',
            title: 'Top 5 mailboxes',
            path: '/app/widgets/charts/components/chart.component',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-8',
            settings: {
                url: '/mobile_user_devices/count_by_type',
                chart: {
                    chart: {
                        renderTo: 'top5ActiveMailboxes',
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
            id: 'activeUsers',
            title: 'Active users',
            path: '/app/widgets/charts/components/chart.component',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-4',
            settings: {
                url: '/mobile_user_devices/count_by_type',
                chart: {
                    chart: {
                        renderTo: 'activeUsers',
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
                        },
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
            id: 'top5InactiveMailboxes',
            title: 'Top 5 inactive mailboxes',
            path: '/app/widgets/charts/components/chart.component',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-8',
            settings: {
                url: '/mobile_user_devices/count_by_type',
                chart: {
                    chart: {
                        renderTo: 'top5InactiveMailboxes',
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
            id: 'inactiveUsers',
            title: 'Inactive users',
            path: '/app/widgets/charts/components/chart.component',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-4',
            settings: {
                url: '/mobile_user_devices/count_by_type',
                chart: {
                    chart: {
                        renderTo: 'inactiveUsers',
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
                        },
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
    }

}