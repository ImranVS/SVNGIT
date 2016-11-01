import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import {WidgetController, WidgetContract, WidgetService} from '../../core/widgets';

import {RESTService} from '../../core/services/rest.service';

declare var injectSVG: any;
declare var bootstrapNavigator: any;


@Component({
    templateUrl: '/app/reports/components/sample-report.component.html',
    providers: [
        WidgetService,
        RESTService
    ]
})
export class SampleReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService) {

        super(resolver, widgetService);

    }

    ngOnInit() {

        this.service.get('/navigation/sitemaps/traveler_reports')
            .subscribe
            (
            data => this.contextMenuSiteMap = data,
            error => console.log(error)
            );
        this.widgets = [
            {
                id: 'mobileDevicesChart',
                title: 'Mobile devices',
                name: 'ChartComponent',
                settings: {
                    url: '/DashBoard/mobile_user_devices/count_by_type',
                    chart: {
                        chart: {
                            renderTo: 'mobileDevicesChart',
                            type: 'pie',
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
                id: 'mobileDevicesOSChart',
                title: 'Mobile devices OS for all Servers',
                name: 'ChartComponent',
                settings: {
                    url: '/DashBoard/mobile_user_devices/count_by_os',
                    chart: {
                        chart: {
                            renderTo: 'mobileDevicesOSChart',
                            type: 'pie',
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
                id: 'mobileUsersTable',
                title: 'Mobile users',
                name: 'MobileUsers',
                settings: {}
            }
        ];
        injectSVG();
        bootstrapNavigator();

    }
}