import { Component, ComponentFactoryResolver, OnInit, Injector } from '@angular/core';

import { WidgetController, WidgetContainer, WidgetContract } from '../../../core/widgets';
import { WidgetService } from '../../../core/widgets/services/widget.service';
import { AppNavigator } from '../../../navigation/app.navigator.component';
import { Office365Grid } from './office365-grid.component';
import { ActivatedRoute } from '@angular/router';
import { ServiceTab } from '../../../services/models/service-tab.interface';
declare var injectSVG: any;


@Component({
    templateUrl: '/app/dashboards/components/office365/office365-password-settings.component.html'
    //providers: [WidgetService]
})
export class Office365PasswordSettingsTab extends WidgetController implements OnInit, ServiceTab {

    widgets: WidgetContract[];
    serviceId: string;
    nodeName: string;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private route: ActivatedRoute) {
        super(resolver, widgetService);

    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            if (params['service']) {
                this.serviceId = params['service'];
                var res = this.serviceId.split(';');
                if (res.length > 1) {
                    this.nodeName = res[1];
                }
                this.serviceId = res[0];
            }
        });
        var url1 = `/services/user_pwd_expires?deviceId=${this.serviceId}&nodeName=${this.nodeName}`;
        var url2 = `/services/strong_pwd?deviceId=${this.serviceId}&nodeName=${this.nodeName}`;
        this.widgets = [
            {
                id: 'passwordExpires',
                title: 'Password expires',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6',
                settings: {
                    url: url1,
                    dateformat: 'time',
                    chart: {
                        chart: {
                            renderTo: 'passwordExpires',
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
                id: 'passwordRequired',
                title: 'Strong password required',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6',
                settings: {
                    url: url2,
                    dateformat: 'time',
                    chart: {
                        chart: {
                            renderTo: 'passwordRequired',
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
                id: 'passwordsGrid',
                title: '',
                name: 'Office365PasswordsGrid',
                css: 'col-xs-12 col-sm-12 col-md-12 col-lg-12'
            }
        ]
        injectSVG();
    }

    onPropertyChanged(key: string, value: any) {

        if (key === 'serviceId') {

            this.serviceId = value;
            var res = this.serviceId.split(';');
            if (res.length > 1) {
                this.nodeName = res[1];
            }
            this.serviceId = res[0];

            var url1 = `/services/user_pwd_expires?deviceId=${this.serviceId}&nodeName=${this.nodeName}`;
            var url2 = `/services/strong_pwd?deviceId=${this.serviceId}&nodeName=${this.nodeName}`;

            this.widgetService.refreshWidget('passwordExpires', url1)
                .catch(error => console.log(error));
            this.widgetService.refreshWidget('passwordRequired', url2)
                .catch(error => console.log(error));

        }

    }

}