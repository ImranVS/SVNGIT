﻿import { Component, ComponentFactoryResolver, OnInit } from '@angular/core';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { WidgetController, WidgetContract } from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import { RESTService } from '../../../core/services/rest.service';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as helpers from '../../../core/services/helpers/helpers';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/ibm-traveler/traveler-http-sessions.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
    ]
})
export class TravelerHTTPSessionsReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];
    paramtype: string;
    paramvalue: string;
    selectedInterval: any;

    currentHideDTControl: boolean = false;
    currentHideSingleDTControl: boolean = true;
    currentHideServerControl: boolean = false;
    currentHideIntervalControl: boolean = true;
    currentHideMailServerControl: boolean = true;
    currentHideAllServerControl: boolean = true;
    currentWidgetName: string = `travelerHttpChart`;
    currentWidgetURL: string;

    servers: any;
    errorMessage: any;

    constructor(
        protected resolver: ComponentFactoryResolver,
        protected widgetService: WidgetService,
        private service: RESTService,
        private router: Router,
        private route: ActivatedRoute,
        protected urlHelpers: helpers.UrlHelperService) {

        super(resolver, widgetService, true, router, route);
    }

    ngOnInit() {

        super.ngOnInit();

        this.service.get('/navigation/sitemaps/traveler_reports')
            .subscribe
            (
            data => this.contextMenuSiteMap = data,
            error => console.log(error)
            );
        this.service.get(`/services/status_list?type=Traveler`)
            .subscribe(
            (response) => {
                this.servers = response.data;
            },
            (error) => this.errorMessage = <any>error
            );
        this.currentWidgetURL = `/reports/summarystats_chart?type=Traveler&statName=Http.CurrentConnections&seriesTitle=devicename`;
        
        this.widgets = [
            {
                id: 'travelerHttpChart',
                title: '',
                name: 'ChartComponent',
                settings: {
                    url: this.currentWidgetURL,
                    dateformat: 'date',
                    chart: {
                        chart: {
                            renderTo: 'travelerHttpChart',
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
                                text: 'HTTP Sessions'
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
            }
        ];
        
        injectSVG();
        

    }
}