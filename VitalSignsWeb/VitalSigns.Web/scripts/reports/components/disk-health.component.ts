﻿import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import {WidgetController, WidgetContract, WidgetService} from '../../core/widgets';

import {RESTService} from '../../core/services/rest.service';

declare var injectSVG: any;
declare var bootstrapNavigator: any;


@Component({
    templateUrl: '/app/reports/components/disk-health.component.html',
    providers: [
        WidgetService,
        RESTService
    ]
})
export class DiskHealthReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService) {

        super(resolver, widgetService);

    }

    ngOnInit() {

        this.service.get('/navigation/sitemaps/domino_reports')
            .subscribe
            (
            data => this.contextMenuSiteMap = data,
            error => console.log(error)
            );
        this.widgets = [
            {
                id: 'diskHealthChart',
                title: '',
                name: 'ChartComponent',
                settings: {
                    url: '/services/disk_space',
                    chart: {
                        chart: {
                            renderTo: 'diskHealthChart',
                            type: 'bar',
                            height: 600
                        },
                        title: { text: '' },
                        subtitle: { text: '' },
                        credits: {
                            enabled: false
                        },
                        xAxis: {
                            categories: []
                        },
                        yAxis: {
                            min: 0,
                            title: {
                                text: 'Disk Space (GB)'
                            }
                        },
                        exporting: {
                            enabled: false
                        },
                        plotOptions: {
                            pie: {
                                allowPointSelect: true,
                                cursor: 'pointer',
                                dataLabels: {
                                    enabled: true,
                                    format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                                    style: {
                                        color: 'black'
                                    }
                                }
                            },
                            series: {
                                stacking: 'normal'
                            }
                        },
                        tooltip: {
                            formatter: function () {
                                return '<div style="font-size: 11px; font-weight: normal;">' + this.series.name + '<br /><strong>' + this.y.toFixed(2) + '</strong> (' + this.percentage.toFixed(1) + '%)</div>';
                            },
                            useHTML: true
                        },
                        series: [],
                        colors: ['#5FBE7F', '#EF3A24']
                    }
                }
            }
        ];
        injectSVG();
        bootstrapNavigator();

    }
}