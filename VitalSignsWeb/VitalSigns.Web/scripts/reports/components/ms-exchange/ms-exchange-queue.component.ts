﻿import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {ActivatedRoute, Router, UrlSegment} from '@angular/router';
import {WidgetController, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import {RESTService} from '../../../core/services/rest.service';

import * as helpers from '../../../core/services/helpers/helpers';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/ms-exchange/ms-exchange-queue.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
    ]
})
export class exchangequeuereport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];
    paramtype: string;

    currentHideServerControl: boolean = false;
    currentHideDatePanel: boolean = false;
    currentShowSingleDatePanel: boolean = false;
    currentHideSingleDTControl: boolean = true;
    currentDeviceType: string = "Exchange";
    currentWidgetName: string = `QueueChart`;
    currentWidgetURL: string;

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

        this.service.get('/navigation/sitemaps/exchange_reports')
            .subscribe
            (
            data => this.contextMenuSiteMap = data ,
            error => console.log(error)
        );

        this.currentWidgetURL = `/reports/summarystats_chart?statName=[CAS@Shadow%23Queues,CAS@Submission%23Queues,CAS@Unreachable%23Queues]`;
        
        this.widgets = [
            {
                id: 'QueueChart',
                title: '',
                name: 'ChartComponent',
                settings: {
                    url: this.currentWidgetURL,
                    dateformat: 'date',
                    chart: {
                        chart: {
                            renderTo: 'QueueChart',
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
                                text: 'Count'
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