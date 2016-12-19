import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {WidgetController, WidgetContract, WidgetService} from '../../../core/widgets';
import {RESTService} from '../../../core/services/rest.service';

import * as helpers from '../../../core/services/helpers/helpers';

declare var injectSVG: any;


@Component({
    templateUrl: '/app/reports/components/servers/server-availability-report.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
    ]
})
export class ServerAvailabilityReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];
    statname: string;
    title: string;

    currentHideMinValuePanel: boolean = false;
    currentWidgetURL: string = `/reports/server_availability?statName=${this.statname}`;
    currentWidgetName: string = "report";

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService, private route: ActivatedRoute,
        protected urlHelpers: helpers.UrlHelperService) {

        super(resolver, widgetService);

    }

    ngOnInit() {

        this.route.queryParams.subscribe(params => {
            this.statname = params['statname'];
            this.title = params['title'];
        });

        this.currentWidgetURL = `/reports/server_availability?statName=${this.statname}`;

        this.service.get('/navigation/sitemaps/server_reports')
            .subscribe
            (
            data => this.contextMenuSiteMap = data,
            error => console.log(error)
            );
        this.widgets = [
            {
                id: 'report',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
                settings: {
                    url: `/reports/server_availability?statName=${this.statname}`,
                    chart: {
                        chart: {
                            renderTo: 'blogs',
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
        ];
        injectSVG();
        

    }
}