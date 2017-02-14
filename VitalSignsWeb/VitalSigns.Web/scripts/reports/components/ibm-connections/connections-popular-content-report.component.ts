import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import {WidgetController, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import {RESTService} from '../../../core/services/rest.service';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/ibm-connections/connections-popular-content-report.component.html',
    providers: [
        WidgetService,
        RESTService
    ]
})
export class ConnectionsPopularContentReport extends WidgetController {
    contextMenuSiteMap: any;
    chartData: any;
    errorMessage: string;
    widgets: WidgetContract[];
    currentWidgetName: string = `connectionsPopularContentReport`;
    currentWidgetURL: string = `/reports/connections/most_popular_content`;

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
                id: 'connectionsPopularContentReport',
                title: '',
                name: 'ChartComponent',
                settings: {
                    url: `/reports/connections/most_popular_content`,
                    dateformat: "date",
                    chart: {
                        chart: {
                            renderTo: 'connectionsPopularContentReport',
                            type: 'pie',
                            height: 540,
                            plotBorderWidth: 0,
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
                            enabled: false
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