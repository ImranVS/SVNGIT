import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {ActivatedRoute, Router, UrlSegment} from '@angular/router';
import {WidgetController, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import {RESTService} from '../../../core/services/rest.service';

import * as helpers from '../../../core/services/helpers/helpers';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/office365/office365-mail-latency.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
    ]
})
export class Office365MailLatencyReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];
    paramtype: string;

    currentHideServerControl: boolean = false;
    currentHideDatePanel: boolean = false;
    currentShowSingleDatePanel: boolean = true;
    currentDeviceType: string = "Office365";
    currentWidgetName: string = `mailLatencyChart`;
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

        this.service.get('/navigation/sitemaps/office365_reports')
            .subscribe
            (
            data => this.contextMenuSiteMap = data ,
            error => console.log(error)
        );

        this.currentWidgetURL = `/reports/summarystats_chart?statName=[MailLatency@null]&aggregation=AVG&getNode=true`;
        
        this.widgets = [
            {
                id: 'mailLatencyChart',
                title: '',
                name: 'ChartComponent',
                settings: {
                    url: this.currentWidgetURL,
                    dateformat: 'date',
                    chart: {
                        chart: {
                            renderTo: 'mailLatencyChart',
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
                                text: 'Seconds (ms)'
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