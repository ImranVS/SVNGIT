import {Component, ComponentFactoryResolver, Input, OnInit} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {AppNavigator} from '../../../navigation/app.navigator.component';
import {RESTService} from '../../../core/services';

declare var injectSVG: any;


@Component({
    selector: 'sample-dashboard',
    templateUrl: '/app/dashboards/components/key-metrics/key-metrics-dashboard.component.html',
    providers: [WidgetService]
})
export class KeyMetricsDashboard extends WidgetController implements OnInit {

    widgets: WidgetContract[] = [
        {
            id: 'keymetricsGrid',
            title: 'Alphabetical Order (by File Name)',
            name: 'OverallDatabaseGrid',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-8'
        },
        {
            id: 'byTemplate',
            title: 'Mail Templates',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
            settings: {
                url: '/dashboard/database?filter_by=IsMailFile&filter_value=true&group_by=design_template_name&get_chart=true',
                chart: {
                    chart: {
                        renderTo: 'byTemplate',
                        type: 'pie',
                        height: 400
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
                            text: 'Size (MB)'
                        }
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
                    legend: {
                        labelFormatter: function () {
                            return '<div style="font-size: 10px; font-weight: normal;">' + this.name + '</div>';
                        }
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
            id: 'statisticsGrid',
            title: 'Statistics',
            name: 'KeyMetricsStatisticsGrid',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-12'
        },
        {
            id: 'biggestMailFiles',
            title: 'Top 20 Largest Mail Files',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
            settings: {
                url: '/dashboard/database?filter_by=IsMailFile&filter_value=true&order_by=FileSize&order_type=desc&top_x=20&get_chart=true',
                chart: {
                    chart: {
                        renderTo: 'biggestMailFiles',
                        type: 'bar',
                        height: 400
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
                            text: 'Size (MB)'
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
            id: 'biggestMailFilesQuota',
            title: 'Top 20 Largest Mail Files as a % of Quota',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
            settings: {
                url: '/dashboard/database?filter_by=IsMailFile&filter_value=true&order_by=PercentQuota&order_type=desc&top_x=20&get_chart=true',
                chart: {
                    chart: {
                        renderTo: 'biggestMailFilesQuota',
                        type: 'bar',
                        height: 400
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
                            text: 'Percent of Quota'
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
    
    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {
        super(resolver, widgetService);
    }

    ngOnInit() {
        this.widgetService.setProperty("ismailpage", "True");
        injectSVG();
        
    }

}