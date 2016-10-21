import {Component, ComponentFactoryResolver, OnInit, Injector} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../../core/widgets';

import {ServiceTab} from '../../../services/models/service-tab.interface';

import {IBMConnectionsGrid} from './ibm-connections-grid.component';

declare var injectSVG: any;

@Component({
    selector: 'tab-libraries',
    templateUrl: '/app/dashboards/components/ibm-connections/ibm-connections-libraries-tab.component.html'
})
export class IBMConnectionsLibrariesTab extends WidgetController implements OnInit, ServiceTab {

    widgets: WidgetContract[];
    serviceId: string;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {
        super(resolver, widgetService);
    }
    
    ngOnInit() {

        this.widgetService.setProperty("tabname", "LIBRARIES");

        this.serviceId = this.widgetService.getProperty('serviceId');

        this.widgets = [
            {
                id: 'libraries',
                title: 'Libraries',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                settings: {
                    url: `/services/summarystats?statName=NUM_OF_LIBRARIES_*_YESTERDAY&deviceid=${this.serviceId}`,
                    chart: {
                        chart: {
                            renderTo: 'libraries',
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
                id: 'librariesGrid',
                title: '',
                name: 'IBMConnectionsStatsGrid',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6'
            }
        ];
    
        injectSVG();
    }

    onPropertyChanged(key: string, value: any) {

        if (key === 'serviceId') {

            this.serviceId = value;

            var displayDate = (new Date()).toISOString().slice(0, 10);

            this.widgetService.refreshWidget('libraries', `/services/summarystats?statName=NUM_OF_LIBRARIES_*_YESTERDAY&deviceid=${this.serviceId}`)
                .catch(error => console.log(error));

            this.widgetService.refreshWidget('librariesGrid', `/services/summarystats?statName=NUM_OF_${this.widgetService.getProperty("tabname")}_*&deviceId=${this.serviceId}&isChart=false&startDate=${displayDate}&endDate=${displayDate}`)
                .catch(error => console.log(error));
        }

    }
}