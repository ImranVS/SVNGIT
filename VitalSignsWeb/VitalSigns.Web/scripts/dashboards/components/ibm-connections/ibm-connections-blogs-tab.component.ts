import {Component, ComponentFactoryResolver, OnInit, Injector} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import {ServiceTab} from '../../../services/models/service-tab.interface';

import {IBMConnectionsGrid} from './ibm-connections-grid.component';

import {ActivatedRoute} from '@angular/router';
declare var injectSVG: any;

@Component({
    selector: 'tab-blogs',
    templateUrl: '/app/dashboards/components/ibm-connections/ibm-connections-blogs-tab.component.html'
})
export class IBMConnectionsBlogsTab extends WidgetController implements OnInit, ServiceTab {

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
            {
                this.serviceId = this.widgetService.getProperty('serviceId'); }
        });

        this.widgetService.setProperty("tabname", "BLOGS");


        this.widgets = [
            {
                id: 'blogs',
                title: 'Blogs',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
                settings: {
                    url: `/services/summarystats?statName=NUM_OF_BLOGS_*_CREATED_YESTERDAY&deviceid=${this.serviceId}`,
                    dateformat: "date",
                    chart: {
                        chart: {
                            renderTo: 'blogs',
                            type: 'spline',
                            height: 340
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
                            enabled: true
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
                id: 'top5CommunitiesBlogs',
                title: 'Top 5 Communities for Blogs',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
                settings: {
                    url: `/dashboard/connections/most_active_object?deviceid=${this.serviceId}&type=Blog&count=5`,
                    chart: {
                        chart: {
                            renderTo: 'top5CommunitiesBlogs',
                            type: 'bar',
                            height: 340
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
                id: 'blogsGrid',
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

            this.widgetService.refreshWidget('blogs', `/services/summarystats?statName=NUM_OF_BLOGS_*_CREATED_YESTERDAY&deviceid=${this.serviceId}`)
                .catch(error => console.log(error));

            this.widgetService.refreshWidget('top5CommunitiesBlogs', `/dashboard/connections/most_active_object?deviceid=${this.serviceId}&type=Blog&count=5`)
                .catch(error => console.log(error));

            this.widgetService.refreshWidget('blogsGrid', `/services/summarystats?statName=NUM_OF_${this.widgetService.getProperty("tabname")}_*&deviceId=${this.serviceId}&isChart=false&startDate=${displayDate}&endDate=${displayDate}`)
                .catch(error => console.log(error));
            //console.log(`/services/summarystats?statName=NUM_OF_${this.widgetService.getProperty("tabname")}_*&deviceId=${this.serviceId}&isChart=false&startDate=${displayDate}&endDate=${displayDate}`);

        }

    }
}