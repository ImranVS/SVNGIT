import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {WidgetController, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services/rest.service';

import * as helpers from '../../../core/services/helpers/helpers';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/ibm-connections/connections-activity-report.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
    ]
})
export class ConnectionsActivityReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];
    statname: string;
    title: string;
    currentStatType: string = "NUM_OF_ACTIVITIES";
    currentHideServerControl: boolean = false;
    currentHideDatePanel: boolean = false;
    currentDeviceType: string = "IBM Connections";
    currentWidgetName: string = `connectionsActivity`;
    currentWidgetURL: string = `/reports/summarystats_chart`;


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
                id: 'connectionsActivity',
                title: '',
                name: 'ChartComponent',
                settings: {
                    url: `/reports/summarystats_chart?statName=NUM_OF_ACTIVITIES_ACTIVITIES`,
                    dateformat: "date",
                    chart: {
                        chart: {
                            renderTo: 'connectionsActivity',
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
    //    

    //}
}