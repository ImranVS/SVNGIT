import { Component, ComponentFactoryResolver, OnInit } from '@angular/core';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { WidgetController, WidgetContract, WidgetService } from '../../../core/widgets';

import { RESTService } from '../../../core/services/rest.service';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as helpers from '../../../core/services/helpers/helpers';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/ibm-traveler/traveler-stats.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
    ]
})
export class TravelerStatsReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];
    paramtype: string;
    paramvalue: string;
    selectedInterval: any;

    currentHideDTControl: boolean = true;
    currentHideSingleDTControl: boolean = true;
    currentHideServerControl: boolean = false;
    currentHideIntervalControl: boolean = false;
    currentHideMailServerControl: boolean = false;
    currentHideAllServerControl: boolean = true;
    currentWidgetName: string = `travelerStatsChart`;
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

        this.service.get('/navigation/sitemaps/traveler_reports')
            .subscribe
            (
            data => this.contextMenuSiteMap = data,
            error => console.log(error)
            );

        this.route.queryParams.subscribe(params => {
            this.paramtype = params['type'];
            this.paramvalue = params['value']; 
        });

        if (this.paramtype == "interval") {
            this.currentHideIntervalControl = false;
            this.currentHideMailServerControl = true;
        }
        else {
            this.currentHideIntervalControl = true;
            this.currentHideMailServerControl = false;
        }

        this.currentWidgetURL = `/reports/traveler_stats?paramtype=${this.paramtype}`;
        
        this.widgets = [
            {
                id: 'travelerStatsChart',
                title: '',
                name: 'ChartComponent',
                settings: {
                    url: this.currentWidgetURL,
                    dateformat: 'time',
                    chart: {
                        chart: {
                            renderTo: 'travelerStatsChart',
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
                                text: ''
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