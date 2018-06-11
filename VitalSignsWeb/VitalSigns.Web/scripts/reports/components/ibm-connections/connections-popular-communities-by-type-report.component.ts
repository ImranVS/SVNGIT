import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import {WidgetController, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import {RESTService} from '../../../core/services/rest.service';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/ibm-connections/connections-popular-communities-by-type-report.component.html',
    providers: [
        RESTService
    ]
})
export class ConnectionsPopularCommunitiesByTypeReport extends WidgetController {
    contextMenuSiteMap: any;
    chartData: any;
    errorMessage: string;
    widgets: WidgetContract[];
    currentWidgetName: string = `connectionsPopularContentReport`;
    currentWidgetURL: string = `/reports/connections/most_popular_communities_by_type`;

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
                id: 'connectionsPopularCommunitiesReport',
                title: '',
                name: 'ChartComponent',
                settings: {
                    url: `/reports/connections/most_popular_communities_by_type`,
                    dateformat: "date",
                    chart: {
                        chart: {
                            renderTo: 'connectionsPopularCommunitiesReport',
                            type: 'pie',
                            height: 540,
                            plotBorderWidth: 0,
                            zoomType: 'xy',
                            events: {
                                drilldown: function (e) {
                                    this.setTitle({ text: "Top 25 Communities" });
                                },
                                drillup: function (e) {
                                    this.setTitle({ text: "" });
                                }
                            }
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
                            pie: {
                                allowPointSelect: true,
                                cursor: 'pointer',
                                dataLabels: {
                                    enabled: true
                                },
                                showInLegend: true,
                                innerSize: '70%'
                            }
                        }
                    }
                }
            }
        ];
        injectSVG();
    }
}