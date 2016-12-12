import {Component, ComponentFactoryResolver, OnInit, Injector} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../../core/widgets';

import {ServiceTab} from '../../../services/models/service-tab.interface';

import {IBMConnectionsGrid} from './ibm-connections-grid.component';
import {ActivatedRoute} from '@angular/router';


declare var injectSVG: any;

@Component({
    selector: 'tab-overall',
    templateUrl: '/app/dashboards/components/ibm-connections/ibm-connections-overview-tab.component.html'
})
export class IBMConnectionsOverviewTab extends WidgetController implements OnInit, ServiceTab {

    widgets: WidgetContract[];
    serviceId: string;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private route: ActivatedRoute) {
        super(resolver, widgetService);
     
    }
    
    ngOnInit() {
        
        this.route.params.subscribe(params => {
            if (params['service'])
                this.serviceId = params['service'];
            else
                this.serviceId = this.widgetService.getProperty('serviceId');
        });

        this.widgetService.setProperty("tabname", "OVERVIEW");

        this.widgets = [
            {
                id: 'dailyActivities',
                title: 'Daily Activities',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
                settings: {
                    url: `/services/summarystats?statName=*_CREATED_LAST_DAY&deviceid=${this.serviceId}`,
                    dateformat: "date",
                    chart: {
                        chart: {
                            renderTo: 'dailyActivities',
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
                id: 'top5Tags',
                title: 'Top 5 Tags',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
                settings: {
                    url: `/dashboard/connections/top_tags?deviceid=${this.serviceId}&type=Bookmark&count=5`,
                    chart: {
                        chart: {
                            renderTo: 'top5Tags',
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
                id: 'overviewGrid',
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

            this.widgetService.refreshWidget('dailyActivities', `/services/summarystats?statName=*_CREATED_LAST_DAY&deviceid=${this.serviceId}`)
                .catch(error => console.log(error));

            this.widgetService.refreshWidget('top5Tags', `/dashboard/connections/top_tags?deviceid=${this.serviceId}&type=Bookmark&count=5`)
                .catch(error => console.log(error));

        }

    }
}