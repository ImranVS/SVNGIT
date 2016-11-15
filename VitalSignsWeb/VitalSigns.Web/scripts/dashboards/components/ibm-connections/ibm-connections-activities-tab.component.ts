import {Component, ComponentFactoryResolver, OnInit, Injector} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../../core/widgets';

import {ServiceTab} from '../../../services/models/service-tab.interface';

import {IBMConnectionsGrid} from './ibm-connections-grid.component';
import {IBMConnectionsStatsGrid} from './ibm-connections-stats-grid.component';
import {ActivatedRoute} from '@angular/router';

declare var injectSVG: any;

@Component({
    selector: 'tab-activities',
    templateUrl: '/app/dashboards/components/ibm-connections/ibm-connections-activities-tab.component.html'
})
export class IBMConnectionsActivitiesTab extends WidgetController implements OnInit, ServiceTab {

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
        this.widgetService.setProperty("tabname", "ACTIVITIES");
        this.widgets = [
            {
                id: 'activities',
                title: 'Activities',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
                settings: {
                    url: `/services/summarystats?statName=[NUM_OF_ACTIVITIES_ACTIVITIES_CREATED_YESTERDAY,NUM_OF_ACTIVITIES_ACTIVITIES_FOLLOWED_YESTERDAY,ACTIVITY_LOGINS_LAST_DAY]&deviceid=${this.serviceId}`,
                    chart: {
                        chart: {
                            renderTo: 'activities',
                            type: 'spline',
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
            },
            {
                id: 'top5CommunitiesActivities',
                title: 'Top 5 Communities for Activities',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
                settings: {
                    url: `/dashboard/connections/most_active_object?deviceid=${this.serviceId}&type=Activity&count=5`,
                    chart: {
                        chart: {
                            renderTo: 'top5CommunitiesActivities',
                            type: 'bar',
                            height: 240
                        },
                        colors: ['#5fbe7f'],
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            labels: {
                                step: 1
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
            {
                id: 'activitiesGrid',
                title: '',
                name: 'IBMConnectionsStatsGrid',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4'
            }
        ];
    
        injectSVG();
    }

    onPropertyChanged(key: string, value: any) {

        if (key === 'serviceId') {

            this.serviceId = value;

            var displayDate = (new Date()).toISOString().slice(0, 10);

            this.widgetService.refreshWidget('activities', `/services/summarystats?statName=[NUM_OF_ACTIVITIES_ACTIVITIES_CREATED_YESTERDAY,NUM_OF_ACTIVITIES_ACTIVITIES_FOLLOWED_YESTERDAY,ACTIVITY_LOGINS_LAST_DAY]&deviceid=${this.serviceId}`)
                .catch(error => console.log(error));

            this.widgetService.refreshWidget('top5CommunitiesActivities', `/dashboard/connections/most_active_object?deviceid=${this.serviceId}&type=Activity&count=5`)
                .catch(error => console.log(error));

            this.widgetService.refreshWidget('activitiesGrid', `/services/summarystats?statName=NUM_OF_${this.widgetService.getProperty("tabname")}_*&deviceId=${this.serviceId}&isChart=false&startDate=2016-10-20&endDate=2016-10-20`)
                .catch(error => console.log(error));
            

        }

    }
}