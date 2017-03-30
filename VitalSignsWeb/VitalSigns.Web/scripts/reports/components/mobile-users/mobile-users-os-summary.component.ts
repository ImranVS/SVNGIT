import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {WidgetController, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import {RESTService} from '../../../core/services/rest.service';

import * as helpers from '../../../core/services/helpers/helpers';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/mobile-users/mobile-users-os-summary.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
    ]
})
export class MobileDevicesSummaryOS extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];
    param: string;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService,
        private route: ActivatedRoute, protected urlHelpers: helpers.UrlHelperService) {

        super(resolver, widgetService);

    }

    ngOnInit() {
        this.service.get('/navigation/sitemaps/mobile_users')
            .subscribe
            (
            data => this.contextMenuSiteMap = data,
            error => console.log(error)
            );


        this.widgets = [
            {
                id: 'mobileDevicesOSChart',
                title: '',
                name: 'ChartComponent',
                settings: {
                    url: '/DashBoard/mobile_user_devices/count_by_os',
                    chart: {
                        chart: {
                            renderTo: 'mobileDevicesOSChart',
                            type: 'pie',
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
                            pie: {
                                allowPointSelect: true,
                                cursor: 'pointer',
                                dataLabels: {
                                    enabled: true,
                                    formatter: function () {
                                        return '<b>' + this.point.name + '</b>: ' + this.point.y;
                                    }
                                },
                                showInLegend: true,
                                innerSize: '70%'
                            }
                        },
                        legend: {
                            labelFormatter: function () {
                                return '<div style="font-size: 10px; font-weight: normal;">' + this.name + ' (' + this.percentage + '%)' + '</div>'; 
                            }
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