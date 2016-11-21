import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {WidgetController, WidgetContract, WidgetService} from '../../../core/widgets';
import {RESTService} from '../../../core/services/rest.service';

import * as helpers from '../../../core/services/helpers/helpers';

declare var injectSVG: any;
declare var bootstrapNavigator: any;


@Component({
    templateUrl: '/app/reports/components/ibm-connections/connections-tags-report.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
    ]
})
export class ConnectionsTagsReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];
    statname: string;
    title: string;
    currentStatType: string = "COMMUNITY_TAG";
    currentHideServerControl: boolean = false;
    currentHideDatePanel: boolean = false;
    currentDeviceType: string = "IBM Connections";
    currentWidgetName: string = `connectionsTags`;
    currentWidgetURL: string = `/reports/summarystats_chart?deviceId=`;


    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService, private route: ActivatedRoute, protected urlHelpers: helpers.UrlHelperService) {

        super(resolver, widgetService);

    }

    ngOnInit() {
        this.route.queryParams.subscribe(params => {
            this.statname = params['statname'];
            this.title = params['title'];
        });
        //currentWidgetURL = this.statname;
        this.service.get('/navigation/sitemaps/connections_reports')
            .subscribe
            (
            data => this.contextMenuSiteMap = data,
            error => console.log(error)
            );
        this.widgets = [
            {
                id: 'connectionsTags',
                title: '',
                name: 'ChartComponent',
                settings: {
                    url: `/reports/summarystats_chart?statName=COMMUNITY_TAG_COUNT`,
                    dateformat: "date",
                    chart: {
                        chart: {
                            renderTo: 'connectionsTags',
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