﻿import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {ActivatedRoute,Router} from '@angular/router';
import {WidgetController, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services/rest.service';

import * as helpers from '../../../core/services/helpers/helpers';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/office365/office365-statistics-report.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
    ]
})
export class Office365StatisticsReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];
    statname: string;
    title: string;

    currentHideServerControl: boolean = false;
    currentHideDatePanel: boolean = false;
    currentDeviceType: string = "Office365";
    currentWidgetName: string = `report`;
    currentWidgetURL: string = `/reports/summarystats_chart?statName=${this.statname}`;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService,
        private router: Router,
        private route: ActivatedRoute,
        protected urlHelpers: helpers.UrlHelperService) {

        super(resolver, widgetService, true, router, route);

    }

    ngOnInit() {

        super.ngOnInit();

        this.route.queryParams.subscribe(params => {
            this.statname = params['statname'];
            this.title = params['title'];
        });

        this.currentWidgetURL = `/reports/summarystats_chart?statName=${this.statname}@null&getNode=true`;

        this.service.get('/navigation/sitemaps/office365_reports')
            .subscribe
            (
            data => {
                this.contextMenuSiteMap = data;
                console.log(data)
            }
            ,
            error => console.log(error)
        );

        var startDate = new Date(2016, 10, 5).toISOString();
        var endDate = new Date(2016, 10, 20).toISOString();

        this.widgets = [
            {
                id: 'report',
                //title: `${this.title}`,
                name: 'ChartComponent',
                settings: {
                    //url: `/reports/summarystats_chart?statName=${this.statname}&startDate=${startDate}&endDate=${endDate}`,
                    url: this.currentWidgetURL,
                    dateformat: "date",
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
                                enabled: true,
                                text: 'Milliseconds (ms)'
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
                            enabled: true
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