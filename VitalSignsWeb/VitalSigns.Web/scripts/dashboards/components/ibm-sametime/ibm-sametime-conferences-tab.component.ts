import {Component, ComponentFactoryResolver, OnInit, Injector} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import {ServiceTab} from '../../../services/models/service-tab.interface';

import {IBMSametimeGrid} from './ibm-sametime-grid.component';

declare var injectSVG: any;

@Component({
    selector: 'tab-conferences',
    templateUrl: '/app/dashboards/components/ibm-sametime/ibm-sametime-conferences-tab.component.html'
})
export class IBMSametimeConferencesTab extends WidgetController implements OnInit, ServiceTab {

    widgets: WidgetContract[];
    serviceId: string;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {
        super(resolver, widgetService);
    }
    
    ngOnInit() {

        this.serviceId = this.widgetService.getProperty('serviceId');

        this.widgets = [
            {
                id: 'oneOnOneCalls',
                title: 'One-on-one Calls',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                settings: {
                    url: `/services/statistics?statName=Totalcountofall1x1calls&deviceid=${this.serviceId}&operation=hourly`,
                    chart: {
                        chart: {
                            renderTo: 'oneOnOneCalls',
                            type: 'areaspline',
                            height: 300
                        },
                        colors: ['#5fbe7f'],
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            labels: {
                                step: 7
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
                id: 'multiUserCalls',
                title: 'Multi-user Calls',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                settings: {
                    url: `/services/statistics?statName=Totalcountofallmultiusercalls&deviceid=${this.serviceId}&operation=hourly`,
                    chart: {
                        chart: {
                            renderTo: 'multiUserCalls',
                            type: 'areaspline',
                            height: 300
                        },
                        colors: ['#5fbe7f'],
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            labels: {
                                step: 7
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
                id: 'allCalls',
                title: 'All Calls',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                settings: {
                    url: `/services/statistics?statName=Totalcountofallcalls&deviceid=${this.serviceId}&operation=hourly`,
                    chart: {
                        chart: {
                            renderTo: 'allCalls',
                            type: 'areaspline',
                            height: 300
                        },
                        colors: ['#5fbe7f'],
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            labels: {
                                step: 7
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
                id: 'allCallsAllUsers',
                title: 'All Calls/All Users',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                settings: {
                    url: `/services/statistics?statName=[Countofallcalls,Countofallusers]&deviceid=${this.serviceId}&operation=hourly`,
                    chart: {
                        chart: {
                            renderTo: 'allCallsAllUsers',
                            type: 'areaspline',
                            height: 300
                        },
                        colors: ['#5fbe7f'],
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            labels: {
                                step: 7
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
                id: 'allOneOnOneCallsUsers',
                title: 'All One-on-one Calls/All One-on-one Call Users',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                settings: {
                    url: `/services/statistics?statName=[Countofall1x1calls,Countofall1x1users]&deviceid=${this.serviceId}&operation=hourly`,
                    chart: {
                        chart: {
                            renderTo: 'allOneOnOneCallsUsers',
                            type: 'areaspline',
                            height: 300
                        },
                        colors: ['#5fbe7f'],
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            labels: {
                                step: 7
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
                id: 'allMultiUserCallsUsers',
                title: 'All Multi-user Calls/All Multi-user Call Users',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-6',
                settings: {
                    url: `/services/statistics?statName=[Countofallmultiusercalls,Countofallmultiuserusers]&deviceid=${this.serviceId}&operation=hourly`,
                    chart: {
                        chart: {
                            renderTo: 'allMultiUserCallsUsers',
                            type: 'areaspline',
                            height: 300
                        },
                        colors: ['#5fbe7f'],
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            labels: {
                                step: 7
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

            this.widgetService.refreshWidget('multiUserCalls', `/services/statistics?statName=Totalcountofallmultiusercalls&deviceid=${this.serviceId}&operation=hourly`)
                .catch(error => console.log(error));

            this.widgetService.refreshWidget('allCalls', `/services/statistics?statName=Totalcountofallcalls&deviceid=${this.serviceId}&operation=hourly`)
                .catch(error => console.log(error));

            this.widgetService.refreshWidget('allCallsAllUsers', `/services/statistics?statName=[Countofallcalls,Countofallusers]&deviceid=${this.serviceId}&operation=hourly`)
                .catch(error => console.log(error));

            this.widgetService.refreshWidget('allOneOnOneCallsUsers', `/services/statistics?statName=[Countofall1x1calls,Countofall1x1users]&deviceid=${this.serviceId}&operation=hourly`)
                .catch(error => console.log(error));

            this.widgetService.refreshWidget('allMultiUserCallsUsers', `/services/statistics?statName=[Countofallmultiusercalls,Countofallmultiuserusers]&deviceid=${this.serviceId}&operation=hourly`)
                .catch(error => console.log(error));

        }

        super.onPropertyChanged(key, value);

    }

}