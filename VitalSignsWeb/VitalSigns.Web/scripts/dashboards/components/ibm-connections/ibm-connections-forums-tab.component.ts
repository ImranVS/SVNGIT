import {Component, ComponentFactoryResolver, OnInit, Injector} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../../core/widgets';

import {ServiceTab} from '../../../services/models/service-tab.interface';

import {IBMConnectionsGrid} from './ibm-connections-grid.component';
import {ActivatedRoute} from '@angular/router';

declare var injectSVG: any;

@Component({
    selector: 'tab-forums',
    templateUrl: '/app/dashboards/components/ibm-connections/ibm-connections-forums-tab.component.html'
})
export class IBMConnectionsForumsTab extends WidgetController implements OnInit, ServiceTab {

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

        this.widgetService.setProperty("tabname", "FORUMS");

        this.widgets = [
            {
                id: 'forums',
                title: 'Forums',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                settings: {
                    url: `/services/summarystats?statName=NUM_OF_FORUMS_*_YESTERDAY&deviceid=${this.serviceId}`,
                    dateformat: "date",
                    chart: {
                        chart: {
                            renderTo: 'forums',
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
                id: 'forumsGrid',
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

            this.widgetService.refreshWidget('forums', `/services/summarystats?statName=NUM_OF_FORUMS_*_YESTERDAY&deviceid=${this.serviceId}`)
                .catch(error => console.log(error));

            this.widgetService.refreshWidget('forumsGrid', `/services/summarystats?statName=NUM_OF_${this.widgetService.getProperty("tabname")}_*&deviceId=${this.serviceId}&isChart=false&startDate=${displayDate}&endDate=${displayDate}`)
                .catch(error => console.log(error));
        }

    }
}