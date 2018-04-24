import {Component, ComponentFactoryResolver, OnInit, Injector} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import {ServiceTab} from '../../../services/models/service-tab.interface';

import {IBMSametimeGrid} from './ibm-sametime-grid.component';

declare var injectSVG: any;

@Component({
    selector: 'tab-chats',
    templateUrl: '/app/dashboards/components/ibm-sametime/ibm-sametime-chats-tab.component.html'
})
export class IBMSametimeChatsTab extends WidgetController implements OnInit, ServiceTab {

    widgets: WidgetContract[];
    serviceId: string;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {

        super(resolver, widgetService);

    }
    
    onPropertyChanged(key: string, value: any) {  

        if (key === 'serviceId') {

            this.serviceId = value;

            this.widgetService.refreshWidget('nWayChats', `/services/summarystats?statName=TotalnWayChats&deviceid=${this.serviceId}`)
                .catch(error => console.log(error));

            this.widgetService.refreshWidget('peakNWayChats', `/services/summarystats?statName=PeaknWayChats&deviceid=${this.serviceId}`)
                .catch(error => console.log(error));

            this.widgetService.refreshWidget('peakChatSessions', `/services/summarystats?statName=Peak2WayChats&deviceid=${this.serviceId}`)
                .catch(error => console.log(error));

            this.widgetService.refreshWidget('totalChatMessages', `/services/summarystats?statName=Total2WayChats&deviceid=${this.serviceId}`)
                .catch(error => console.log(error));

        }

        super.onPropertyChanged(key, value);

    }

    ngOnInit() {
        var serviceId = this.widgetService.getProperty('serviceId');
        if (serviceId) {
            this.serviceId = serviceId;
        }

        //this.serviceId = this.widgetService.getProperty('serviceId');

        this.widgets = [
            {
                id: 'totalChatMessages',
                title: 'Total 2-way Chat Messages',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                settings: {
                    url: `/services/summarystats?statName=Total2WayChats&deviceid=${this.serviceId}`,
                    dateformat: 'date',
                    chart: {
                        chart: {
                            renderTo: 'totalChatMessages',
                            type: 'areaspline',
                            height: 300
                        },
                        //colors: ['#5fbe7f'],
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
                id: 'nWayChats',
                title: 'Total N-way Chat Messages',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                settings: {
                    //url: `/services/statistics?statName=Numberofnwaychats&deviceId=${this.serviceId}&operation=hourly`,
                    url: `/services/summarystats?statName=TotalnWayChats&deviceid=${this.serviceId}`,
                    dateformat: 'date',
                    chart: {
                        chart: {
                            renderTo: 'nWayChats',
                            type: 'areaspline',
                            height: 300
                        },
                        //colors: ['#5fbe7f'],
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
                id: 'peakChatSessions',
                title: 'Peak 2-way Chat Messages',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                settings: {
                    url: `/services/summarystats?statName=Peak2WayChats&deviceid=${this.serviceId}`,
                    dateformat: 'date',
                    chart: {
                        chart: {
                            renderTo: 'peakChatSessions',
                            type: 'areaspline',
                            height: 300
                        },
                        //colors: ['#5fbe7f'],
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
                id: 'peakNWayChats',
                title: 'Peak N-way Chat Messages',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                settings: {
                    //url: `/services/statistics?statName=Numberofactivenwaychats&deviceId=${this.serviceId}&operation=hourly`,
                    url: `/services/summarystats?statName=PeaknWayChats&deviceid=${this.serviceId}`,
                    dateformat: 'date',
                    chart: {
                        chart: {
                            renderTo: 'peakNWayChats',
                            type: 'areaspline',
                            height: 300
                        },
                        //colors: ['#5fbe7f'],
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
            }
         
        ];
    
        injectSVG();
    }

}