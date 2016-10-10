import {Component, ComponentFactoryResolver, OnInit, Injector} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../../core/widgets';

import {ServiceTab} from '../../../services/models/service-tab.interface';

import {IBMConnectionsGrid} from './ibm-connections-grid.component';

declare var injectSVG: any;

@Component({
    selector: 'tab-communities',
    templateUrl: '/app/dashboards/components/ibm-connections/ibm-connections-communities-tab.component.html'
})
export class IBMConnectionsCommunitiesTab extends WidgetController implements OnInit, ServiceTab {

    widgets: WidgetContract[];
    serviceId: string;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {
        super(resolver, widgetService);
    }
    
    ngOnInit() {

        this.serviceId = this.widgetService.getProperty('serviceId');

        this.widgets = [
            {
                id: 'communitiesByType',
                title: 'Communities by Type',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-4 col-lg-4',
                settings: {
                    url: `/services/summarystats?statName=[COMMUNITY_TYPE_PRIVATE,COMMUNITY_TYPE_PUBLIC,COMMUNITY_TYPE_PUBLICINVITEONLY]&deviceid=${this.serviceId}`,
                    chart: {
                        chart: {
                            renderTo: 'communitiesByType',
                            type: 'pie',
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
                id: 'top5Communities',
                title: 'Top 5 Most Active Communities',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-4 col-lg-4',
                settings: {
                    url: 'http://private-ad10c-ibm.apiary-mock.com/connections/top_5_communities',
                    chart: {
                        chart: {
                            renderTo: 'top5Communities',
                            type: 'bar',
                            height: 240
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
                title: 'Most Active Community is \"VS Dev\"',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-4 col-lg-4',
                settings: {
                    url: '/connections/most_active_community',
                    chart: {
                        chart: {
                            renderTo: 'mostActiveCommunity',
                            type: 'pie',
                            height: 240
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
            }
        ];
    
        injectSVG();
    }

}