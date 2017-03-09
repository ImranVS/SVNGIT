import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import {WidgetController, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import {RESTService} from '../../../core/services/rest.service';

import * as helpers from '../../../core/services/helpers/helpers';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/disk/cpu-utilization.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
    ]
})
export class CPUUtilizationReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];

    currentHideDTControl: boolean = false;
    currentHideSingleDTControl: boolean = true;
    currentHideServerControl: boolean = true;
    currentHideIntervalControl: boolean = true;
    currentHideMailServerControl: boolean = true;
    currentHideAllServerControl: boolean = false;
    currentHideAggregationControl: boolean = false;
    currentWidgetName: string = `cpuChart`;
    currentWidgetURL: string;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService,
        protected urlHelpers: helpers.UrlHelperService) {

        super(resolver, widgetService);

    }

    ngOnInit() {

        this.service.get('/navigation/sitemaps/disk_reports')
            .subscribe
            (
            data => this.contextMenuSiteMap = data,
            error => console.log(error)
        );
        this.currentWidgetURL = `/reports/summarystats_chart?statName=Platform.System.PctCombinedCpuUtil`;
        this.widgets = [
            {
                id: 'cpuChart',
                title: '',
                name: 'ChartComponent',
                settings: {
                    url: this.currentWidgetURL + `&aggregation=AVG`,
                    dateformat: 'date',
                    chart: {
                        chart: {
                            renderTo: 'cpuChart',
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
                                text: 'Percent'
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