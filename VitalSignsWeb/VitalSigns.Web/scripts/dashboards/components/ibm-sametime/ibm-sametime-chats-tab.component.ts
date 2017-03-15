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

            this.widgetService.refreshWidget('nWayChats', `/services/statistics?statName=Numberofnwaychats&deviceId=${this.serviceId}&operation=hourly`)
                .catch(error => console.log(error));

            this.widgetService.refreshWidget('activeNWayChats', `/services/statistics?statName=Numberofactivenwaychats&deviceId=${this.serviceId}&operation=hourly`)
                .catch(error => console.log(error));

            this.widgetService.refreshWidget('openChatSessions', `/services/statistics?statName=Numberofopenchatsessions&deviceId=${this.serviceId}&operation=hourly`)
                .catch(error => console.log(error));

            this.widgetService.refreshWidget('chatMessages', `/services/statistics?statName=Numberofchatmessages&deviceId=${this.serviceId}&operation=hourly`)
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
                id: 'nWayChats',
                title: 'N-way Chats',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                settings: {
                    url: `/services/statistics?statName=Numberofnwaychats&deviceId=${this.serviceId}&operation=hourly`,
                    dateformat: 'time',
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
            },
            {
                id: 'activeNWayChats',
                title: 'Active N-way Chats',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                settings: {
                    url: `/services/statistics?statName=Numberofactivenwaychats&deviceId=${this.serviceId}&operation=hourly`,
                    dateformat: 'time',
                    chart: {
                        chart: {
                            renderTo: 'activeNWayChats',
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
            },
            {
                id: 'openChatSessions',
                title: 'Open Chat Sessions',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                settings: {
                    url: `/services/statistics?statName=Numberofopenchatsessions&deviceId=${this.serviceId}&operation=hourly`,
                    dateformat: 'time',
                    chart: {
                        chart: {
                            renderTo: 'openChatSessions',
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
            },
            {
                id: 'chatMessages',
                title: 'Chat Messages',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                settings: {
                    url: `/services/statistics?statName=Numberofchatmessages&deviceId=${this.serviceId}&operation=hourly`,
                    dateformat: 'time',
                    chart: {
                        chart: {
                            renderTo: 'chatMessages',
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

}