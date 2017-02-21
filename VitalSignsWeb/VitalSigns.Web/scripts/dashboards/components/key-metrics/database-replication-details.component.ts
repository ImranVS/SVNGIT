import {Component, ComponentFactoryResolver, OnInit, Injector} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import {ServiceTab} from '../../../services/models/service-tab.interface';
declare var injectSVG: any;

@Component({
    selector: 'database-replication-details',
    templateUrl: '/app/dashboards/components/key-metrics/database-replication-details.component.html'
})
export class DatabaseReplicationDetails extends WidgetController implements OnInit {

    widgets: WidgetContract[];
    serviceId: string;
    
    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {
        super(resolver, widgetService);
    }
    
    ngOnInit() {
        this.serviceId = this.widgetService.getProperty('serviceId');
        this.widgets = [
            {
                id: 'documentCount',
                title: 'Document Count',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                settings: {
                    url: `/dashboard/database-problems?clusterId=${this.serviceId}&isChart=true&isDocCount=true`,
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
                    url: `/dashboard/database-problems?clusterId=${this.serviceId}&isChart=true&isDocCount=false`,
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

    onPropertyChanged(key: string, value: any) {

        if (key === 'serviceId') {

            this.serviceId = value;

            var date = new Date();
            var displayDate = new Date(date.getFullYear(), date.getMonth(), date.getDate()).toISOString();

            this.widgetService.refreshWidget('activities', `/services/summarystats?statName=[NUM_OF_ACTIVITIES_ACTIVITIES_CREATED_YESTERDAY,NUM_OF_ACTIVITIES_ACTIVITIES_FOLLOWED_YESTERDAY,ACTIVITY_LOGINS_LAST_DAY]&deviceid=${this.serviceId}`)
                .catch(error => console.log(error));

            this.widgetService.refreshWidget('top5CommunitiesActivities', `/dashboard/connections/most_active_object?deviceid=${this.serviceId}&type=Activity&count=5`)
                .catch(error => console.log(error));

        }

    }
}