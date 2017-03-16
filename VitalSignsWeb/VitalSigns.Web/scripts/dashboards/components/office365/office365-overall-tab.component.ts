import { Component, ComponentFactoryResolver, OnInit, Injector} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import { AppNavigator } from '../../../navigation/app.navigator.component';
import { Office365Grid } from './office365-grid.component';
import { ActivatedRoute } from '@angular/router';
import { ServiceTab } from '../../../services/models/service-tab.interface';
declare var injectSVG: any;


@Component({
    templateUrl: '/app/dashboards/components/office365/office365-overall-tab.component.html'
    //providers: [WidgetService]
})
export class Office365OverallTab extends WidgetController implements OnInit, ServiceTab {

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
        var url = "";
        if (this.nodeName) {
            url = `/services/statistics?deviceId=${this.serviceId}&statName=[POP@` + this.nodeName + `,IMAP@` + this.nodeName + `,SMTP@` + this.nodeName + `]&operation=HOURLY&isChart=true`;
        }
        else {
            url = `/services/statistics?deviceId=${this.serviceId}&statName=[POP@null,IMAP@null,SMTP@null]&operation=HOURLY&isChart=true&getNode=true`;
        }
        this.widgets = [
            {
                id: 'mailServices',
                title: 'Mail services',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-4',
                settings: {
                    url: url,
                    dateformat: 'time',
                    chart: {
                        chart: {
                            renderTo: 'mailServices',
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
                id: 'dailyUserLogins',
                title: 'Daily user logins',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-4',
                settings: {
                    url: `/services/summarystats?deviceId=${this.serviceId}&statName=ActiveUsersCount`,
                    dateformat: 'date',
                    chart: {
                        chart: {
                            renderTo: 'dailyUserLogins',
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
                id: 'lastLogon',
                title: 'Last logon',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-4',
                settings: {
                    url: `/services/last_logon?deviceId=${this.serviceId}`,
                    chart: {
                        chart: {
                            renderTo: 'lastLogon',
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
                            bar: {
                                dataLabels: {
                                    enabled: false
                                },
                                groupPadding: 0.1,
                                borderWidth: 0
                            },
                            series: {
                                pointPadding: 0
                            },
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
                url = `/services/statistics?deviceId=${this.serviceId}&statName=[POP@` + this.nodeName + `,IMAP@` + this.nodeName + `,SMTP@` + this.nodeName + `]&operation=HOURLY&isChart=true`;
            }
            else {
                url = `/services/statistics?deviceId=${this.serviceId}&statName=[POP@null,IMAP@null,SMTP@null]&operation=HOURLY&isChart=true&getNode=true`;
            }
            this.widgetService.refreshWidget('mailServices', url)
                .catch(error => console.log(error));

        }

    }

}