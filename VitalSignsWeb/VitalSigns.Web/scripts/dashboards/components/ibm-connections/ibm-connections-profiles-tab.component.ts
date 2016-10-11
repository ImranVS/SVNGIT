import {Component, ComponentFactoryResolver, OnInit, Injector} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../../core/widgets';

import {ServiceTab} from '../../../services/models/service-tab.interface';

import {IBMConnectionsGrid} from './ibm-connections-grid.component';

declare var injectSVG: any;

@Component({
    selector: 'tab-profiles',
    templateUrl: '/app/dashboards/components/ibm-connections/ibm-connections-profiles-tab.component.html'
})
export class IBMConnectionsProfilesTab extends WidgetController implements OnInit, ServiceTab {

    widgets: WidgetContract[];
    serviceId: string;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {
        super(resolver, widgetService);
    }
    
    ngOnInit() {

        this.serviceId = this.widgetService.getProperty('serviceId');

        this.widgets = [
            {
                id: 'managersNonManagers',
                title: 'Managers/Non Managers',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
                settings: {
                    url: `/services/summarystats?statName=[NUM_OF_PROFILES_MANAGERS,NUM_OF_PROFILES_PROFILES]&deviceid=${this.serviceId}&startDate=2016-10-10&endDate=2016-10-10`,
                    chart: {
                        chart: {
                            renderTo: 'managersNonManagers',
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
                id: 'pictureNoPicture',
                title: 'Picture/No Picture',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
                settings: {
                    url: `/services/summarystats?statName=[NUM_OF_PROFILES_WITH_NO_PICTURE,NUM_OF_PROFILES_PROFILES]&deviceid=${this.serviceId}&startDate=2016-10-10&endDate=2016-10-10`,
                    chart: {
                        chart: {
                            renderTo: 'pictureNoPicture',
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
                id: 'jobHierarchyNoJobHierarchy',
                title: 'Job Hierarchy/No Job Hierarchy',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
                settings: {
                    url: `/services/summarystats?statName=[NUM_OF_PROFILES_WITH_JOB_HIERARCHY,NUM_OF_PROFILES_WITH_NO_JOB_HIERARCHY]&deviceid=${this.serviceId}&startDate=2016-10-10&endDate=2016-10-10`,
                    chart: {
                        chart: {
                            renderTo: 'jobHierarchyNoJobHierarchy',
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
            }
        ];
    
        injectSVG();
    }

}