import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../core/widgets';
import {AppNavigator} from '../../navigation/app.navigator.component';


import {ServiceTab} from '../models/service-tab.interface';

declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    templateUrl: '/app/services/components/service-clusterhealth-tab.component.html',
    providers: [WidgetService]
})
export class ServiceClusterHealthTab extends WidgetController implements OnInit {
    deviceId: any;
    service: any;

    widgets: WidgetContract[];

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private route: ActivatedRoute) {
        super(resolver, widgetService);
    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.deviceId = params['service'];

        });
        this.widgets = [
            {
                id: 'ServiceClusterHealthGrid',
                title: 'Cluster Replicator Health',
                name: 'ServiceClusterHealthGrid',
                css: 'col-xs-12',
            },
            {
                id: 'domcluster',
                title: 'Cluster Seconds On Queue',
                name: 'ChartComponent',
                css: 'col-xs-12',
                settings: {
                    url: '/services/statistics?statName=Replica.Cluster.SecondsOnQueue&deviceid=' + this.deviceId + '&operation=hourly',
                    chart: {
                        chart: {
                            renderTo: 'domcluster',
                            type: 'areaspline',
                            height: 300
                        },
                        colors: ['#5fbe7f'],
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            labels: {
                                step: 6
                            },
                            categories: []
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
        injectSVG();
        bootstrapNavigator();
    }

}