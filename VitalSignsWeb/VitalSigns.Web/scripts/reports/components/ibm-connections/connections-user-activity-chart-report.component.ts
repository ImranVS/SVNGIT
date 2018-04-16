import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import {WidgetController, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import {RESTService} from '../../../core/services/rest.service';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/ibm-connections/connections-user-activity-chart-report.component.html',
    providers: [
        WidgetService,
        RESTService
    ]
})
export class ConnectionsUserActivityChartReport extends WidgetController {
    contextMenuSiteMap: any;
    chartData: any;
    errorMessage: string;
    widgets: WidgetContract[];

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
                id: 'connectionsUserActivityChartReport',
                title: '',
                name: 'BubbleChartComponent',
                settings: {
                    url: `/reports/connections/user_activity?isChart=true&topX=10`,
                    chart: {
                        chart: {
                            renderTo: 'connectionsUserActivityChartReport',
                            type: 'bubble',
                            height: 540,
                            plotBorderWidth: 1,
                            zoomType: 'xy'
                        },
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            labels: {
                                
                            }
                        },
                        yAxis: {
                            startOnTick: false,
                            endOnTick: false,
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
                            series: {
                                marker: {
                                    fillColor: {
                                        radialGradient: { cx: 0.4, cy: 0.3, r: 0.7 },
                                        stops: [
                                            [0, 'rgba(255,255,255,0.5)'],
                                            [1, 'rgba(69,114,167,0.5)']
                                        ]
                                    }
                                }
                            },
                            bubble: {
                                tooltip: {
                                    headerFormat: '<b>{point.key.x}</b><br>',
                                    pointFormat: '{point.z} items'

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