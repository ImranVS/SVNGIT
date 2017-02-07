import {Component, ComponentFactoryResolver, OnInit, Injector } from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import {ServiceTab} from '../../../services/models/service-tab.interface';

import {IBMConnectionsGrid} from './ibm-connections-grid.component';
import {ActivatedRoute} from '@angular/router';

declare var injectSVG: any;

@Component({
    selector: 'tab-communities',
    templateUrl: '/app/dashboards/components/ibm-connections/ibm-connections-communities-tab.component.html'
})

export class IBMConnectionsCommunitiesTab extends WidgetController implements OnInit, ServiceTab {

    widgets: WidgetContract[];
    serviceId: string;
    today: Date;

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
        var date = new Date();
        var displayDate = new Date(date.getFullYear(), date.getMonth(), date.getDate()).toISOString();

        this.widgets = [
            {
                id: 'communitiesByType',
                title: 'Communities by Type',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-4 col-lg-4',
                settings: {
                    url: `/services/summarystats?statName=[COMMUNITY_TYPE_PRIVATE,COMMUNITY_TYPE_PUBLIC,COMMUNITY_TYPE_PUBLICINVITEONLY]&deviceid=${this.serviceId}&startDate=${displayDate}&endDate=${displayDate}`,
                    dateformat: "date",
                    chart: {
                        chart: {
                            renderTo: 'communitiesByType',
                            type: 'pie',
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
                            pie: {
                                allowPointSelect: true,
                                cursor: 'pointer',
                                dataLabels: {
                                    enabled: false
                                },
                                showInLegend: true,
                                innerSize: '70%'
                            }
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
                id: 'top5Communities',
                title: 'Top 5 Most Active Communities',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-4 col-lg-4',
                settings: {
                    url: `/dashboard/connections/top_communities?deviceid=${this.serviceId}`,
                    chart: {
                        chart: {
                            renderTo: 'top5Communities',
                            type: 'bar',
                            height: 340
                        },
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            labels: {
                                step: 1
                            },
                            categories: [],
                            title: {
                                //text: 'Time'
                            }
                        },
                        yAxis: {
                            title: {
                                //text: 'Time'
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
                        plotOptions: {
                            series: {
                                stacking: 'normal'
                            }
                        },
                        series: []
                    }
                }
            },
            {
                id: 'mostActiveCommunity',
                title: 'Most Active Community',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-4 col-lg-4',
                settings: {
                    url: `/dashboard/connections/top_communities?deviceid=${this.serviceId}&count=1`,
                    callback: (chart) => this.widgets[2].title = 'Most Active Community is "' + chart.series[0].name + '"',
                    chart: {
                        chart: {
                            renderTo: 'mostActiveCommunity',
                            type: 'pie',
                            height: 340
                        },
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            labels: {
                                step: 1
                            },
                            categories: []
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
                        plotOptions: {
                            pie: {
                                allowPointSelect: true,
                                cursor: 'pointer',
                                dataLabels: {
                                    enabled: false
                                },
                                showInLegend: true,
                                innerSize: '70%'
                            }
                        },
                        series: []
                    }
                }
            },
            {
                id: 'userComparison',
                title: 'User Comparison',
                name: 'IBMConnectionsUserComparison',
                css: 'col-xs-12 col-sm-12 col-md-12 col-lg-12',
            }
        ];
        injectSVG();
    }

    onPropertyChanged(key: string, value: any) {

        if (key === 'serviceId') {

            this.serviceId = value;

            var date = new Date();
            var displayDate = new Date(date.getFullYear(), date.getMonth(), date.getDate()).toISOString();

            this.widgetService.refreshWidget('communitiesByType', `/services/summarystats?statName=[COMMUNITY_TYPE_PRIVATE,COMMUNITY_TYPE_PUBLIC,COMMUNITY_TYPE_PUBLICINVITEONLY]&deviceid=${this.serviceId}&startDate=${displayDate}&endDate=${displayDate}`)
                .catch(error => console.log(error));

            this.widgetService.refreshWidget('top5Communities', `/dashboard/connections/top_communities?deviceid=${this.serviceId}`)
                .catch(error => console.log(error));

            this.widgetService.refreshWidget('mostActiveCommunity', `/dashboard/connections/top_communities?deviceid=${this.serviceId}&count=1`)
                .catch(error => console.log(error));

            this.widgetService.refreshWidget('userComparison', `/dashboard/connections/users?deviceid=${this.serviceId}`)
                .catch(error => console.log(error));
            
        }

    }
}