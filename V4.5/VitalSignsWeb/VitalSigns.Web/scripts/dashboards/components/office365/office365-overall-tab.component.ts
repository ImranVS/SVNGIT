import { Component, ComponentFactoryResolver, OnInit, Injector} from '@angular/core';
import { RESTService } from '../../../core/services';
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
    service: any;
    errorMessage: string;

    constructor(private dataProvider: RESTService, protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private route: ActivatedRoute) {
        super(resolver, widgetService);

    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            if (params['service']) {
                var res: string[] = params['service'].split(';');
                if (res.length > 1) {
                    this.nodeName = res[1];
                }
                this.serviceId = res[0];
            }
            else {
                var res: string[] = this.serviceId.split(';');
                if (res.length > 1) {
                    this.nodeName = res[1];
                }
                this.serviceId = res[0];
            }
        });


        let obj = this.processUrls();
        
        this.widgets = [
            {
                id: 'upTimeHourly',
                title: 'Last 24 hours availability',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
                settings: {
                    url: obj.urluptimehourly,
                    dateformat: 'time',
                    chart: {
                        chart: {
                            renderTo: 'upTimeHourly',
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
                                text: 'percent'
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
                id: 'mailServices',
                title: 'Mail services',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
                settings: {
                    url: obj.url,
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
                id: 'upTimeDaily',
                title: 'This week\'s availability',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
                settings: {
                    url: obj.urluptimedaily,
                    dateformat: 'date',
                    chart: {
                        chart: {
                            renderTo: 'upTimeDaily',
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
                                text: 'percent'
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
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
                settings: {
                    url: obj.userLogins,
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
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
                settings: {
                    url: obj.lastLogin,
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

    processUrls() {
        var url = "";
        var urluptimehourly = "";
        var urluptimedaily = "";
        urluptimehourly = `/services/statistics?deviceId=${this.serviceId}&statName=[Services.HourlyUpTimePercent.SkypeForBusiness@${this.nodeName},Services.HourlyUpTimePercent.Exchange@${this.nodeName},Services.HourlyUpTimePercent.OneDrive@${this.nodeName},Services.HourlyUpTimePercent.SharePoint@${this.nodeName}]&operation=HOURLY&isChart=true`;
        urluptimedaily = `/services/summarystats?deviceId=${this.serviceId}&statName=[Services.HourlyUpTimePercent.SkypeForBusiness@${this.nodeName},Services.HourlyUpTimePercent.Exchange@${this.nodeName},Services.HourlyUpTimePercent.OneDrive@${this.nodeName},Services.HourlyUpTimePercent.SharePoint@${this.nodeName}]&includeLastDay=false`;

        this.dataProvider.get(`/services/enabled_office365_tests?deviceId=${this.serviceId}`)
            .subscribe(
            data => {
                this.service = data.data;

                if (this.service.indexOf('Create Site') == -1) {
                    urluptimehourly = `/services/statistics?deviceId=${this.serviceId}&statName=[Services.HourlyUpTimePercent.SkypeForBusiness@${this.nodeName},Services.HourlyUpTimePercent.Exchange@${this.nodeName},Services.HourlyUpTimePercent.OneDrive@${this.nodeName}]&operation=HOURLY&isChart=true&getNode=true`;
                    urluptimedaily = `/services/summarystats?deviceId=${this.serviceId}&statName=[Services.HourlyUpTimePercent.SkypeForBusiness@${this.nodeName},Services.HourlyUpTimePercent.Exchange@${this.nodeName},Services.HourlyUpTimePercent.OneDrive@${this.nodeName}]&getNode=true&includeLastDay=false`;

                    this.widgetService.refreshWidget('upTimeDaily', urluptimedaily)

                    this.widgetService.refreshWidget('upTimeHourly', urluptimehourly);
                }


            },
            error => this.errorMessage = <any>error
            );




        if (this.nodeName) {
            url = `/services/statistics?deviceId=${this.serviceId}&statName=[POP@${this.nodeName},IMAP@${this.nodeName},SMTP@${this.nodeName}]&operation=HOURLY&isChart=true`;
        }
        else {
            url = `/services/statistics?deviceId=${this.serviceId}&statName=[POP@${this.nodeName},IMAP@${this.nodeName},SMTP@${this.nodeName}]&operation=HOURLY&isChart=true&getNode=true`;
            urluptimehourly = `/services/statistics?deviceId=${this.serviceId}&statName=[Services.HourlyUpTimePercent.SkypeForBusiness@${this.nodeName},Services.HourlyUpTimePercent.Exchange@${this.nodeName},Services.HourlyUpTimePercent.OneDrive@${this.nodeName},Services.HourlyUpTimePercent.SharePoint@${this.nodeName}]&operation=HOURLY&isChart=true&getNode=true`;
            urluptimedaily = `/services/summarystats?deviceId=${this.serviceId}&statName=[Services.HourlyUpTimePercent.SkypeForBusiness@${this.nodeName},Services.HourlyUpTimePercent.Exchange@${this.nodeName},Services.HourlyUpTimePercent.OneDrive@${this.nodeName},Services.HourlyUpTimePercent.SharePoint@${this.nodeName}]&getNode=true&includeLastDay=false`;
        }

        return {
            url: url,
            urluptimehourly: urluptimehourly,
            urluptimedaily: urluptimedaily,
            userLogins: `/services/summarystats?deviceId=${this.serviceId}&statName=ActiveUsersCount`,
            lastLogin: `/services/last_logon?deviceId=${this.serviceId}`
        };
    }


    onPropertyChanged(key: string, value: any) {

        if (key === 'serviceId') {
            
            var res = value.split(';');
            if (res.length > 1) {
                this.nodeName = res[1];
            }
            this.serviceId = res[0];

            let obj = this.processUrls();

            this.widgetService.refreshWidget('upTimeHourly', obj.urluptimehourly)
                .catch(error => console.log(error));
            this.widgetService.refreshWidget('mailServices', obj.url)
                .catch(error => console.log(error));
            this.widgetService.refreshWidget('upTimeDaily', obj.urluptimedaily)
                .catch(error => console.log(error));
            this.widgetService.refreshWidget('dailyUserLogins', obj.userLogins)
                .catch(error => console.log(error));
            this.widgetService.refreshWidget('lastLogon', obj.lastLogin)
                .catch(error => console.log(error));

        //this.customizeGraphs;
        }

    }

}