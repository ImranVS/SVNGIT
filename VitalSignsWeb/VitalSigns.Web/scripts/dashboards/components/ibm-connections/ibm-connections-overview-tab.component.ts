﻿import {Component, ComponentFactoryResolver, OnInit, Injector} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../../core/widgets';

import {ServiceTab} from '../../../services/models/service-tab.interface';

declare var injectSVG: any;

@Component({
    selector: 'tab-overall',
    templateUrl: '/app/dashboards/components/ibm-connections/ibm-connections-overview-tab.component.html',
    providers: [WidgetService]
})
export class IBMConnectionsOverviewTab extends WidgetController implements OnInit, ServiceTab {

    widgets: WidgetContract[];
    serviceId: string;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {
        super(resolver, widgetService);
    }
    
    ngOnInit() {
    
        this.widgets = [
            {
                id: 'dailyActivities',
                title: 'Daily Activities',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                settings: {
                    url: `/services/summarystats?statName=[BLOGS_CREATED_LAST_DAY,COMMENT_CREATED_LAST_DAY,ENTRY_CREATED_LAST_DAY]&deviceid=57ace43fbf46711cd4681dcf`,
                    chart: {
                        chart: {
                            renderTo: 'dailyActivities',
                            type: 'spline',
                            height: 240
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
            {
                id: 'top5Tags',
                title: 'Top 5 Tags',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                settings: {
                    url: '/Services/top_tags?deviceId=57ace43fbf46711cd4681dcf',
                    chart: {
                        chart: {
                            renderTo: 'top5Tags',
                            type: 'bar',
                            height: 240
                        },
                        colors: ['#5fbe7f'],
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            labels: {
                                step: 1
                            },
                            categories: []
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
            }
        ];
    
        injectSVG();
    }

}