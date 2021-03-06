﻿import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import {WidgetController, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {Router, ActivatedRoute} from '@angular/router';
import {RESTService} from '../../../core/services/rest.service';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/ibm-domino/daily-server-trans.component.html',
    providers: [
        WidgetService,
        RESTService
    ]
})
export class DailyServerTrans extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];
    servers: string;

    currentHideServerControl: boolean = false;
    currentHideDatePanel: boolean = false;
    currentDeviceType: string = "Domino";
    currentWidgetName: string = `avgcpuutilchart`;
    currentWidgetURL: string = `/reports/summarystats_chart?statName=Server.Trans.PerMinute`;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService, private router: Router, private route: ActivatedRoute) {

        super(resolver, widgetService);

    }

    ngOnInit() {
        this.route.queryParams.subscribe(params => {
            this.servers = params['server-type'];
        });
        this.service.get('/navigation/sitemaps/domino_reports')
            .subscribe
            (
            data => this.contextMenuSiteMap = data,
            error => console.log(error)
            );
        this.widgets = [
            {
                id: 'avgcpuutilchart',
                title: '',
                name: 'ChartComponent',
                settings: {
                    url: `/reports/summarystats_chart?statName=Server.Trans.PerMinute`,
                    dateformat: "date",
                    chart: {
                        chart: {
                            renderTo: 'avgcpuutilchart',
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
                                text: 'Transactions'
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