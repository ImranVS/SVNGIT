import { Component, ComponentFactoryResolver, OnInit, Injector} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import { AppNavigator } from '../../../navigation/app.navigator.component';
import { Office365Grid } from './office365-grid.component';
import { ActivatedRoute } from '@angular/router';
import { ServiceTab } from '../../../services/models/service-tab.interface';
declare var injectSVG: any;


@Component({
    templateUrl: '/app/dashboards/components/office365/office365-user-scenario-tests-tab.component.html'
    //providers: [WidgetService]
})
export class Office365UserScenarioTestsTab extends WidgetController implements OnInit, ServiceTab {

    widgets: WidgetContract[];
    serviceId: string;
    nodeName: string;
    
    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private route: ActivatedRoute) {
        super(resolver, widgetService);

    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            if (params['service'])
                this.serviceId = params['service'];
            else {
                var res = this.serviceId.split(';');
                if (res.length > 1) {
                    this.nodeName = res[1];
                }
                this.serviceId = res[0];
            }
        });
        var urlmail = "";
        if (this.nodeName) {
            urlmail = `/services/statistics?deviceId=${this.serviceId}&statName=[MailLatency@` + this.nodeName + `,MailFlow@` + this.nodeName + `,Inbox@` + this.nodeName + `,ComposeEmail@` + this.nodeName + `]&operation=HOURLY&isChart=true`;
        }
        else {
            urlmail = `/services/statistics?deviceId=${this.serviceId}&statName=[MailLatency@null,MailFlow@null,Inbox@null,ComposeEmail@null]&operation=HOURLY&isChart=true&getNode=true`;
        }
        var urlsite = "";
        if (this.nodeName) {
            urlsite = `/services/statistics?deviceId=${this.serviceId}&statName=[CreateSite@` + this.nodeName + `]&operation=HOURLY&isChart=true`;
        }
        else {
            urlsite = `/services/statistics?deviceId=${this.serviceId}&statName=[CreateSite@null]&operation=HOURLY&isChart=true&getNode=true`;
        }
        var urlfolder = "";
        if (this.nodeName) {
            urlfolder = `/services/statistics?deviceId=${this.serviceId}&statName=[CreateTask@` + this.nodeName + `,CreateFolder@` + this.nodeName + `]&operation=HOURLY&isChart=true`;
        }
        else {
            urlfolder = `/services/statistics?deviceId=${this.serviceId}&statName=[CreateTask@null,CreateFolder@null]&operation=HOURLY&isChart=true&getNode=true`;
        }
        this.widgets = [
            {
                id: 'mailScenarioTests',
                title: 'Mail scenario tests',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-4',
                settings: {
                    url: urlmail,
                    dateformat: 'time',
                    chart: {
                        chart: {
                            renderTo: 'mailScenarioTests',
                            type: 'line',
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
                                enabled: true,
                                text: 'ms'
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
                            enabled: true
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
                id: 'siteTests',
                title: 'Site tests',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-4',
                settings: {
                    url: urlsite,
                    dateformat: 'time',
                    chart: {
                        chart: {
                            renderTo: 'siteTests',
                            type: 'line',
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
                                enabled: true,
                                text: 'ms'
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
                            enabled: true
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
                id: 'folderTests',
                title: 'Folder creation tests',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-4',
                settings: {
                    url: urlfolder,
                    dateformat: 'time',
                    chart: {
                        chart: {
                            renderTo: 'folderTests',
                            type: 'line',
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
                                enabled: true,
                                text: 'ms'
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
                            enabled: true
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

        ]
        injectSVG();
    }

    onPropertyChanged(key: string, value: any) {

        if (key === 'serviceId') {

            var serviceId = value;
            var res = serviceId.split(';');
            if (res.length > 1) {
                this.nodeName = res[1];
            }
            var url = "";
            if (this.nodeName) {
                url = `/services/statistics?deviceId=${this.serviceId}&statName=[MailLatency@` + this.nodeName + `,MailFlow@` + this.nodeName + `,Inbox@` + this.nodeName + `,ComposeEmail@` + this.nodeName + `]&operation=HOURLY&isChart=true`;
            }
            else {
                url = `/services/statistics?deviceId=${this.serviceId}&statName=[MailLatency@null,MailFlow@null,Inbox@null,ComposeEmail@null]&operation=HOURLY&isChart=true&getNode=true`;
            }
            this.widgetService.refreshWidget('mailScenarioTests', url)
                .catch(error => console.log(error));

        }

    }

}