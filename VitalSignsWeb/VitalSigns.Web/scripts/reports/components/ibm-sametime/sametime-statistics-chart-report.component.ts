import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {WidgetController, WidgetContract, WidgetService} from '../../../core/widgets';
import {RESTService} from '../../../core/services/rest.service';

import * as helpers from '../../../core/services/helpers/helpers';

declare var injectSVG: any;
declare var bootstrapNavigator: any;


@Component({
    templateUrl: '/app/reports/components/ibm-sametime/sametime-statistics-chart-report.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
    ]
})
export class SametimeStatisticsChartReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];
    url: string = '/reports/summarystats_chart';

    currentHideStatDropdown: boolean = false;
    currentWidgetName: string = `report`;
    currentWidgetURL: string = this.url;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService,
        protected urlHelpers: helpers.UrlHelperService) {

        super(resolver, widgetService);

    }

    ngOnInit() {

        this.service.get('/navigation/sitemaps/sametime_reports')
            .subscribe
            (
            data => {
                this.contextMenuSiteMap = data;
                console.log(data)
            }
            ,
            error => console.log(error)
            );
        this.widgets = [
            {
                id: 'report',
                //title: `${this.title}`,
                name: 'ChartComponent',
                settings: {
                    url: `${this.url}?statName=TotalnWayChats`,
                    chart: {
                        chart: {
                            renderTo: 'report',
                            type: 'bar',
                            height: 540
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
        ];
        injectSVG();
        bootstrapNavigator();

    }
}