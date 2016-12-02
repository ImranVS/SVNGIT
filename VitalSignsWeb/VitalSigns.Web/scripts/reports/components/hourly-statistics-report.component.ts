import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {WidgetController, WidgetContract, WidgetService} from '../../core/widgets';
import {RESTService} from '../../core/services/rest.service';

import * as helpers from '../../core/services/helpers/helpers';

declare var injectSVG: any;
declare var bootstrapNavigator: any;


@Component({
    templateUrl: '/app/reports/components/hourly-statistics-report.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
    ]
})
export class HourlyStatisticsReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];
    statname: string;
    title: string;

    currentHideServerControl: boolean = false;
    currentHideDatePanel: boolean = true;
    currentShowSingleDatePanel: boolean = true;
    currentDeviceType: string = "Domino";
    currentWidgetName: string = `report`;
    currentWidgetURL: string = `/reports/dailystats_hourly_chart?statName=${this.statname}`;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService, private route: ActivatedRoute,
        protected urlHelpers: helpers.UrlHelperService) {

        super(resolver, widgetService);

    }

    ngOnInit() {

        this.route.queryParams.subscribe(params => {
            this.statname = params['statname'];
            this.title = params['title'];
        });
        this.currentWidgetURL = `/reports/dailystats_hourly_chart?statName=${this.statname}`;
        console.log(this.statname);
        var localDate = new Date()
        var date = new Date(localDate.getFullYear(), localDate.getMonth(), localDate.getDate());
        this.service.get('/navigation/sitemaps/server_reports')
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
                    url: `/reports/dailystats_hourly_chart?statName=${this.statname}&date=${date.toISOString()}`,
                    dateformat: "hour",
                    chart: {
                        chart: {
                            renderTo: 'report',
                            type: 'spline',
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