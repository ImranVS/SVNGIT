﻿import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import {WidgetController, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import {RESTService} from '../../../core/services/rest.service';

import * as helpers from '../../../core/services/helpers/helpers';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/ibm-domino/cluster-sec-queue.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
    ]
})
export class ClusterSecQueue extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];

    currentHideServerControl: boolean = false;
    currentHideDatePanel: boolean = true;
    currentShowSingleDatePanel: boolean = true;
    currentDeviceType: string = "Domino";
    currentWidgetName: string = `clusterQueue`;
    currentWidgetURL: string = `/reports/dailystats_hourly_chart?statName=Replica.Cluster.SecondsOnQueue`;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService, protected urlHelpers: helpers.UrlHelperService) {

        super(resolver, widgetService);

    }

    ngOnInit() {
        var tempDate = new Date();
        var date = new Date(tempDate.getFullYear(),tempDate.getMonth(),tempDate.getDate());
        this.service.get('/navigation/sitemaps/domino_reports')
            .subscribe
            (
            data => this.contextMenuSiteMap = data,
            error => console.log(error)
            );
        this.widgets = [
            {
                id: 'clusterQueue',
                title: '',
                name: 'ChartComponent',
                settings: {
                    url: `/reports/dailystats_hourly_chart?statName=Replica.Cluster.SecondsOnQueue&date=${date.toISOString()}`,
                    dateformat: "hour",
                    chart: {
                        chart: {
                            renderTo: 'clusterQueue',
                            type: 'spline',
                            height: 300
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