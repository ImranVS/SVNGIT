import {Component, ComponentFactoryResolver, OnInit, Injector} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

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

        var date = new Date();
        var displayDate = (new Date(date.getFullYear(), date.getMonth(), date.getDate())).toISOString();
        this.widgets = [
            {
                id: 'managersNonManagers',
                title: 'Managers/Non Managers',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
                settings: {
                    url: `/services/summarystats?statName=[NUM_OF_PROFILES_WITH_MANAGERS,NUM_OF_PROFILES_WITH_NO_MANAGER]&deviceid=${this.serviceId}&startDate=${displayDate}&endDate=${displayDate}`,
                    dateformat: "date",
                    chart: {
                        chart: {
                            renderTo: 'managersNonManagers',
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
                id: 'pictureNoPicture',
                title: 'Picture/No Picture',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
                settings: {
                    url: `/services/summarystats?statName=[NUM_OF_PROFILES_WITH_NO_PICTURE,NUM_OF_PROFILES_WITH_PICTURE]&deviceid=${this.serviceId}&startDate=${displayDate}&endDate=${displayDate}`,
                    dateformat: "date",
                    chart: {
                        chart: {
                            renderTo: 'pictureNoPicture',
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
                id: 'jobHierarchyNoJobHierarchy',
                title: 'Job Hierarchy/No Job Hierarchy',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
                settings: {
                    url: `/services/summarystats?statName=[NUM_OF_PROFILES_WITH_JOB_HIERARCHY,NUM_OF_PROFILES_WITH_NO_JOB_HIERARCHY]&deviceid=${this.serviceId}&startDate=${displayDate}&endDate=${displayDate}`,
                    dateformat: "date",
                    chart: {
                        chart: {
                            renderTo: 'jobHierarchyNoJobHierarchy',
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
            }
        ];
    
        injectSVG();
    }

    onPropertyChanged(key: string, value: any) {

        if (key === 'serviceId') {

            this.serviceId = value;

            var date = new Date();
            var displayDate = (new Date(date.getFullYear(), date.getMonth(), date.getDate())).toISOString();

            this.widgetService.refreshWidget('managersNonManagers', `/services/summarystats?statName=[NUM_OF_PROFILES_WITH_MANAGERS,NUM_OF_PROFILES_WITH_NO_MANAGER]&deviceid=${this.serviceId}&startDate=${displayDate}&endDate=${displayDate}`)
                .catch(error => console.log(error));

            this.widgetService.refreshWidget('pictureNoPicture', `/services/summarystats?statName=[NUM_OF_PROFILES_WITH_NO_PICTURE,NUM_OF_PROFILES_WITH_PICTURE]&deviceid=${this.serviceId}&startDate=${displayDate}&endDate=${displayDate}`)
                .catch(error => console.log(error));

            this.widgetService.refreshWidget('jobHierarchyNoJobHierarchy', `/services/summarystats?statName=[NUM_OF_PROFILES_WITH_JOB_HIERARCHY,NUM_OF_PROFILES_WITH_NO_JOB_HIERARCHY]&deviceid=${this.serviceId}&startDate=${displayDate}&endDate=${displayDate}`)
                .catch(error => console.log(error));

        }

    }
}