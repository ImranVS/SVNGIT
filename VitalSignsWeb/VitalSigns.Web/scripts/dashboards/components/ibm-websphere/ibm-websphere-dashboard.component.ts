import {Component, ComponentFactoryResolver, Input, OnInit} from '@angular/core';

import 'rxjs/Rx';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../../core/widgets';
import {AppNavigator} from '../../../navigation/app.navigator.component';

declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    selector: 'websphere-dashboard',
    templateUrl: '/app/dashboards/components/ibm-websphere/ibm-websphere-dashboard.component.html',
    providers: [WidgetService]
})

export class IBMWebsphereDashboard extends WidgetController implements OnInit {

    widgets: WidgetContract[] = [
        {
            id: 'websphereGrid',
            title: 'Websphere Info',
            name: 'IBMWebsphereGrid',
            css: 'col-xs-12 col-sm-12  col-md-12 col-lg-8',
            settings: {

            }
        },
        {
            id: 'serverStatus',
            title: 'Status',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
            settings: {
                url: '/services/status_count?type=WebSphere&docfield=status_code',
                chart: {
                    chart: {
                        renderTo: 'serverStatus',
                        type: 'pie',
                        height: 240
                    },
                    title: { text: '' },
                    subtitle: { text: '' },
                    xAxis: {
                        categories: []
                    },
                    yAxis: {

                    },
                    plotOptions: {
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
    
    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {
        super(resolver, widgetService);
    }

    ngOnInit() {
        injectSVG();
        bootstrapNavigator();
    }

}