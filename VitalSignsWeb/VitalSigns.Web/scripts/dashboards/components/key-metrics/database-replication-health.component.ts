import {Component, ComponentFactoryResolver, Input, OnInit} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {AppNavigator} from '../../../navigation/app.navigator.component';
import {RESTService} from '../../../core/services';
import {DominoServerInfo} from '../../../widgets/main-dashboard/models/domino-server-info';
import { ActivatedRoute } from '@angular/router';
declare var injectSVG: any;


@Component({
    selector: 'database-replication-health',
    templateUrl: '/app/dashboards/components/key-metrics/database-replication-health.component.html',
    providers: [WidgetService]
})
export class DatabaseReplicationHealth extends WidgetController implements OnInit {
    widgets: WidgetContract[];
    serviceId: string;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private route: ActivatedRoute) {
        super(resolver, widgetService);
    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            if (params['service'])
                this.serviceId = params['service'];
            else {
                this.serviceId = this.widgetService.getProperty('serviceId');
            }
        });
        this.widgets = [
            {
                id: 'databaseReplicationGrid',
                title: '',
                name: 'DatabaseReplicationGrid',
                css: 'col-xs-12 col-sm-12  col-md-12 col-lg-12',
                settings: {

                }
            },
            {
                id: 'databaseProblemsGrid',
                title: 'Potential Replication Problems',
                name: 'DatabaseProblemsGrid',
                css: 'col-xs-12 col-sm-12  col-md-12 col-lg-12',
                settings: {

                }
            },
            {
                id: 'documentCount',
                title: 'Document Count',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                settings: {
                    url: `/dashboard/database-problems?clusterId=Mail&isChart=true&isDocCount=true`,
                    chart: {
                        chart: {
                            renderTo: 'documentCount',
                            type: 'bar',
                            height: 1040
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

                        },
                        legend: {
                            labelFormatter: function () {
                                return '<div style="font-size: 10px; font-weight: normal;">' + this.name + '</div>';
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
            },
            {
                id: 'databaseSize',
                title: 'Database Size',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                settings: {
                    url: `/dashboard/database-problems?clusterId=Mail&isChart=true&isDocCount=false`,
                    chart: {
                        chart: {
                            renderTo: 'databaseSize',
                            type: 'bar',
                            height: 1040
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

                        },
                        legend: {
                            labelFormatter: function () {
                                return '<div style="font-size: 10px; font-weight: normal;">' + this.name + '</div>';
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