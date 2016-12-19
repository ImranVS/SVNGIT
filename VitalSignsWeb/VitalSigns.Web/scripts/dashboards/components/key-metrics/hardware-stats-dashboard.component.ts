import {Component, ComponentFactoryResolver, Input, OnInit} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../../core/widgets';
import {AppNavigator} from '../../../navigation/app.navigator.component';
import {RESTService} from '../../../core/services';

declare var injectSVG: any;

@Component({
    selector: 'sample-dashboard',
    templateUrl: '/app/dashboards/components/key-metrics/hardware-stats-dashboard.component.html',
    providers: [WidgetService]
})
export class HardwareStatsDashboard extends WidgetController implements OnInit {

    widgets: WidgetContract[] = [
        {
            id: 'responseTime',
            title: 'Response Time',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-12',
            settings: {
                url: '/services/status_list?type=Domino&docfield=response_time&isChart=true',
                chart: {
                    chart: {
                        renderTo: 'responseTime',
                        type: 'bar',
                        height: 400
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
                            text: 'Milliseconds'
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
            id: 'hardwareStatsGrid',
            title: 'CPU/Memory Stats',
            name: 'HardwareStatisticsGrid',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-12'
        },
        {
            id: 'diskHealthGrid',
            title: 'Disk Health',
            name: 'DiskHealthGrid',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-12'
        }

    ]
    
    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {
        super(resolver, widgetService);
    }

    ngOnInit() {
        injectSVG();
        
    }

    setSort1(byTime: boolean) {
        if (byTime) {
            this.widgetService.refreshWidget('responseTime', `/services/status_list?type=Domino&docfield=response_time&sortby=ResponseTime&isChart=true`)
                .catch(error => console.log(error));
        }
        else {
            this.widgetService.refreshWidget('responseTime', `/services/status_list?type=Domino&docfield=response_time&isChart=true`)
                .catch(error => console.log(error));
        }
    }
}