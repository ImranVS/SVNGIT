import {Component, ComponentFactoryResolver, OnInit, Injector} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../../core/widgets';

import {ServiceTab} from '../../../services/models/service-tab.interface';

import {IBMConnectionsGrid} from './ibm-connections-grid.component';
import {ActivatedRoute} from '@angular/router';

declare var injectSVG: any;

@Component({
    selector: 'tab-profiles',
    templateUrl: '/app/dashboards/components/ibm-connections/ibm-connections-profiles-tab.component.html'
})
export class IBMConnectionsProfilesTab extends WidgetController implements OnInit, ServiceTab {

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

        var displayDate = (new Date()).toISOString().slice(0, 10);

        this.widgets = [
            {
                id: 'managersNonManagers',
                title: 'Managers/Non Managers',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
                settings: {
                    url: `/services/summarystats?statName=[NUM_OF_PROFILES_WITH_MANAGERS,NUM_OF_PROFILES_WITH_NO_MANAGER]&deviceid=${this.serviceId}&startDate=${displayDate}&endDate=${displayDate}`,
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
                    url: `/services/summarystats?statName=[NUM_OF_PROFILES_WITH_NO_PICTURE,NUM_OF_PROFILES_WITH_PICTURE]&deviceid=${this.serviceId}&startDate=${displayDate}&endDate=${displayDate}`,
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
                    url: `/services/summarystats?statName=[NUM_OF_PROFILES_WITH_JOB_HIERARCHY,NUM_OF_PROFILES_WITH_NO_JOB_HIERARCHY]&deviceid=${this.serviceId}&startDate=${displayDate}&endDate=${displayDate}`,
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

    onPropertyChanged(key: string, value: any) {

        if (key === 'serviceId') {

            this.serviceId = value;

            var displayDate = (new Date()).toISOString().slice(0, 10);

            this.widgetService.refreshWidget('managersNonManagers', `/services/summarystats?statName=[NUM_OF_PROFILES_WITH_MANAGERS,NUM_OF_PROFILES_WITH_NO_MANAGER]&deviceid=${this.serviceId}&startDate=${displayDate}&endDate=${displayDate}`)
                .catch(error => console.log(error));

            this.widgetService.refreshWidget('pictureNoPicture', `/services/summarystats?statName=[NUM_OF_PROFILES_WITH_NO_PICTURE,NUM_OF_PROFILES_WITH_PICTURE]&deviceid=${this.serviceId}&startDate=${displayDate}&endDate=${displayDate}`)
                .catch(error => console.log(error));

            this.widgetService.refreshWidget('jobHierarchyNoJobHierarchy', `/services/summarystats?statName=[NUM_OF_PROFILES_WITH_JOB_HIERARCHY,NUM_OF_PROFILES_WITH_NO_JOB_HIERARCHY]&deviceid=${this.serviceId}&startDate=${displayDate}&endDate=${displayDate}`)
                .catch(error => console.log(error));

        }

    }
}