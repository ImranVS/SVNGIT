﻿import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import {WidgetController, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import {RESTService} from '../../../core/services/rest.service';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/ibm-connections/connections-user-activity-monthly-chart-report.component.html',
    providers: [
        RESTService
    ]
})
export class ConnectionsUserActivityMonthlyChartReport extends WidgetController {
    contextMenuSiteMap: any;
    chartData: any;
    errorMessage: string;
    widgets: WidgetContract[];
    currentWidgetName: string = `connectionsUserActivityMonthlyChartReport`;
    currentWidgetURL: string = `/reports/connections/user_activity_monthly`;
    currentShowTopX: boolean = true;
    currentShowCommunityControl: boolean = true;
    currentShowServerControl: boolean = true;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService) {

        super(resolver, widgetService);

    }

    ngOnInit() {
        this.service.get('/navigation/sitemaps/connections_reports')
            .subscribe(
            (data) => {
                this.contextMenuSiteMap = data;
            },
            (error) => this.errorMessage = <any>error
            );
        this.widgets = [
            {
                id: 'connectionsUserActivityMonthlyChartReport',
                title: '',
                name: 'BubbleChartComponent',
                settings: {
                    url: `/reports/connections/user_activity_monthly`,
                    dateformat: "date",
                    chart: {
                        chart: {
                            renderTo: 'connectionsUserActivityMonthlyChartReport',
                            type: 'bubble',
                            height: 540,
                            plotBorderWidth: 1,
                            zoomType: 'xy'
                        },
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            min: null,
                            max: null,
                            startOnTick: false,
                            endOnTick: false,
                            showFirstLabel: true,
                            showLastLabel: true,
                            type: "category",
                            labels: {
                                
                            }
                        },
                        yAxis: {
                            min: null,
                            max: null,
                            startOnTick: false,
                            endOnTick: false,
                            showFirstLabel: true,
                            showLastLabel: true,
                            type: "category",
                            labels: {
                                
                            },
                            title: {
                                enabled: false
                            }
                        },
                        legend: {
                            enabled: true
                        },
                        credits: {
                            enabled: false
                        },
                        exporting: {
                            enabled: false
                        },
                        series: [],
                        plotOptions: {
                            //series: {
                            //    marker: {
                            //        fillColor: {
                            //            radialGradient: { cx: 0.4, cy: 0.3, r: 0.7 },
                            //            stops: [
                            //                [0, 'rgba(255,255,255,0.5)'],
                            //                [1, 'rgba(69,114,167,0.5)']
                            //            ]
                            //        }
                            //    }
                            //},
                            bubble: {
                                tooltip: {
                                    headerFormat: '<b>{point.key.y}</b><br>{point.key.x}<br>',
                                    pointFormat: '{point.z} {series.name} items'

                                }
                            }
                        }
                        //tooltip: {
                        //    formatter: function () {
                        //        var tooltip;
                        //        tooltip = '<span>' + this.series.name + '</span>: <b>' + this.z + '</b><br/>';
                        //        return tooltip;
                        //    }
                        //},
                    }
                }
            }
        ];
        injectSVG();
    }
}