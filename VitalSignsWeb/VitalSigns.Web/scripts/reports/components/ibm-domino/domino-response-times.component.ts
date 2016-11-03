import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import {WidgetController, WidgetContract, WidgetService} from '../../../core/widgets';

import {RESTService} from '../../../core/services/rest.service';

declare var injectSVG: any;
declare var bootstrapNavigator: any;


@Component({
    templateUrl: '/app/reports/components/ibm-domino/domino-response-times.component.html',
    providers: [
        WidgetService,
        RESTService
    ]
})
export class DominoResponseTimes extends WidgetController {
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
                id: 'dailyservertranschart',
                title: '',
                name: 'ChartComponent',
                settings: {
                    url: '/reports/summarystats_chart?statName=ResponseTime',
                    chart: {
                        chart: {
                            renderTo: 'dailyservertranschart',
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
        bootstrapNavigator();

    }

    //ngOnInit() {

    //    this.service.get('/navigation/sitemaps/domino_reports')
    //        .subscribe
    //        (
    //        data => this.contextMenuSiteMap = data,
    //        error => console.log(error)
    //        );
    //    this.widgets = [
    //        {
    //            id: 'mobileDevicesChart',
    //            title: '',
    //            name: 'ChartComponent',
    //            settings: {
    //                url: '/reports/summarystats_chart?statName=ResponseTime',
    //                chart: {
    //                    chart: {
    //                        renderTo: 'mobileDevicesChart',
    //                        type: 'spline',
    //                        height: 300
    //                    },
    //                    title: { text: '' },
    //                    subtitle: { text: '' },
    //                    xAxis: {
    //                        categories: []
    //                    },
    //                    yAxis: {
    //                        min: 0,
    //                        endOnTick: false,
    //                        allowDecimals: false,
    //                        title: {
    //                            enabled: false
    //                        }
    //                    },
    //                    legend: {
    //                        enabled: false
    //                    },
    //                    credits: {
    //                        enabled: false
    //                    },
    //                    exporting: {
    //                        enabled: false
    //                    },
    //                    series: []
    //                }
    //            }
    //        }
    //    ];
    //    injectSVG();
    //    bootstrapNavigator();

    //}
}