import { Component, ComponentFactoryResolver, OnInit } from '@angular/core';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { WidgetController, WidgetContract } from '../../../core/widgets';
import { WidgetService } from '../../../core/widgets/services/widget.service';

import { RESTService } from '../../../core/services/rest.service';

import * as helpers from '../../../core/services/helpers/helpers';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/ms-exchange/ms-exchange-mailbox-report.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
    ]
})
export class ExchangeMailboxReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];
    statname: string;
    title: string;

    currentHideServerControl: boolean = false;
    currentHideDatePanel: boolean = false;
    currentShowSingleDatePanel: boolean = false;
    currentHideSingleDTControl: boolean = true;
    currentDeviceType: string = "Exchange";
    currentWidgetName: string = `report`;
    currentWidgetURL: string = `/reports/exchange_mailbox_type?statName=${this.statname}`;

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
        this.currentWidgetURL = `/reports/exchange_mailbox_type?statName=${this.statname}`;
        this.service.get('/navigation/sitemaps/exchange_reports')
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
                name: 'ChartComponent',
                settings: {
                    url: this.currentWidgetURL,
                    dateformat: "date",
                    chart: {
                        chart: {
                            renderTo: 'report',
                            type: 'bar',
                            height: 540
                        },
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            type: "category"
                        },
                        yAxis: {
                            min: 0,
                            endOnTick: false,
                            allowDecimals: false,
                            title: {
                                enabled: true,
                                text: this.statname.indexOf('mb')>=0 ?'Size (MB)':'Amount'
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
                        series: [],
                        tooltip: {
                            followPointer: true,
                         }
                    }
                }
            }
        ];
        injectSVG();

        
    }

}