import {Component, ComponentFactoryResolver, Input, OnInit} from '@angular/core';
import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../../core/widgets';
import {AppNavigator} from '../../../navigation/app.navigator.component';
import {RESTService} from '../../../core/services';
declare var injectSVG: any;

@Component({
    selector: 'sample-dashboard',
    templateUrl: '/app/dashboards/components/key-metrics/server-days-up-dashboard.component.html',
    providers: [WidgetService]
})
export class ServerDaysUp extends WidgetController implements OnInit {

    widgets: WidgetContract[] = [
        {
            id: 'serverdaysUp',
            title: 'Server Days Up',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-12',
            settings: {
                url: '/services/status_list?type=Domino&docfield=elapsed_days&isChart=true',
                chart: {
                    chart: {
                        renderTo: 'serverdaysUp',
                        type: 'bar',
                        height: 440
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
                            text: 'Days Up'
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

    ]

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {
        super(resolver, widgetService);
    }

    ngOnInit() {
        injectSVG();
        
    }

    setSort1(byCount: boolean) {
        if (byCount) {
            this.widgetService.refreshWidget('serverdaysUp', `/services/status_list?type=Domino&docfield=elapsed_days&isChart=true`)
                .catch(error => console.log(error));
        }
        else {
            this.widgetService.refreshWidget('serverdaysUp', `/services/status_list?type=Domino&docfield=elapsed_days&sortby=ElapsedDays&isChart=true`)
                .catch(error => console.log(error));
        }
    }
}