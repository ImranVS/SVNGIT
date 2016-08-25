import {Component, ComponentResolver, OnInit} from '@angular/core';
import {ROUTER_DIRECTIVES} from '@angular/router';

import {WidgetController, WidgetContainer, WidgetContract} from '../../core/widgets';
import {AppNavigator} from '../../navigation/app.navigator.component';

declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    templateUrl: '/app/dashboards/components/office365-password-settings.component.html',
    directives: [ROUTER_DIRECTIVES, WidgetContainer, AppNavigator]
})
export class Office365PasswordSettings extends WidgetController implements OnInit {

    widgets: WidgetContract[] = [
        {
            id: 'greetings',
            title: 'Say hello',
            path: '/app/widgets/charts/components/chart.component',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-6',
            settings: {
                url: '/mobile_user_devices/count_by_type',
                chart: {
                    chart: {
                        renderTo: 'greetings',
                        type: 'line',
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
        }
    ]
    
    constructor(protected resolver: ComponentResolver) {
        super(resolver);
    }

    ngOnInit() {
        injectSVG();
    }

}