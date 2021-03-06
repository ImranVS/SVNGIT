﻿import {Component, ComponentFactoryResolver, OnInit, Injector} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import {ServiceTab} from '../../../services/models/service-tab.interface';

import {IBMSametimeGrid} from './ibm-sametime-grid.component';

declare var injectSVG: any;

@Component({
    selector: 'tab-meetings',
    templateUrl: '/app/dashboards/components/ibm-sametime/ibm-sametime-meetings-tab.component.html'
})
export class IBMSametimeMeetingsTab extends WidgetController implements OnInit, ServiceTab {

    widgets: WidgetContract[];
    serviceId: string;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {
        super(resolver, widgetService);
    }
    
    ngOnInit() {
        var serviceId = this.widgetService.getProperty('serviceId');
        if (serviceId) {
            this.serviceId = serviceId;
        }

        //this.serviceId = this.widgetService.getProperty('serviceId');

        this.widgets = [
            {
                id: 'activeMeetingsUsers',
                title: 'Active Meetings/Users in Meetings',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                settings: {
                    url: `/services/statistics?statName=[Numberofactivemeetings,Currentnumberofusersinsidemeetings]&deviceId=${this.serviceId}&operation=hourly`,
                    dateformat: 'time',
                    chart: {
                        chart: {
                            renderTo: 'activeMeetingsUsers',
                            type: 'areaspline',
                            height: 300
                        },
                        //colors: ['#5fbe7f'],
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            labels: {
                                step: 6
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
            }
        ];
    
        injectSVG();
    }

    onPropertyChanged(key: string, value: any) {

        if (key === 'serviceId') {

            this.serviceId = value;

            this.widgetService.refreshWidget('activeMeetingsUsers', `/services/statistics?statName=[Numberofactivemeetings,Currentnumberofusersinsidemeetings]&deviceId=${this.serviceId}&operation=hourly`)
                .catch(error => console.log(error));

        }

        super.onPropertyChanged(key, value);
        
    }

}