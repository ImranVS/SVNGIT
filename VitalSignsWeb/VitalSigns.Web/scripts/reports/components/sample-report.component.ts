import {Component, ComponentFactoryResolver} from '@angular/core';

import {WidgetController, WidgetContract, WidgetService} from '../../core/widgets';

@Component({
    templateUrl: '/app/reports/components/sample-report.component.html',
    providers: [WidgetService]
})
export class SampleReport extends WidgetController {

    widgets: WidgetContract[] = [
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
    ]

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {

        super(resolver, widgetService);

    }

}