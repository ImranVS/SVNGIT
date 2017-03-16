import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import { Office365Grid } from './office365-grid.component';
import { ActivatedRoute } from '@angular/router'; 

import {WidgetController, WidgetContainer, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {AppNavigator} from '../../../navigation/app.navigator.component';

declare var injectSVG: any;


@Component({
    templateUrl: '/app/dashboards/components/office365/office365-mail-stats-tab.component.html'
    //providers: [WidgetService]
})
export class Office365MailStatsTab extends WidgetController implements OnInit {
    serviceId: string;
    widgets: WidgetContract[]
    
    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private route: ActivatedRoute) {
        super(resolver, widgetService);
    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            if (params['service'])
                this.serviceId = params['service'];
            else {
                var res = this.serviceId.split(';');
                this.serviceId = res[0];
            }
        });
        this.widgets = [
            {
                id: 'top5ActiveMailboxes',
                title: 'Top 5 mailboxes',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6',
                settings: {
                    url: `/services/top_mailboxes?deviceId=${this.serviceId}`,
                    chart: {
                        chart: {
                            renderTo: 'top5ActiveMailboxes',
                            type: 'bar',
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
                                text: 'GB'
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
                id: 'top5InactiveMailboxes',
                title: 'Top 5 inactive mailboxes',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6',
                settings: {
                    url: `/services/top_inactive_mailboxes?deviceId=${this.serviceId}`,
                    chart: {
                        chart: {
                            renderTo: 'top5InactiveMailboxes',
                            type: 'bar',
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
                                text: 'days since last login'
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
                id: 'mailboxTypes',
                title: 'Mailbox types',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6',
                settings: {
                    url: `/services/mailbox_types?deviceId=${this.serviceId}`,
                    chart: {
                        chart: {
                            renderTo: 'mailboxTypes',
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
                id: 'activeInactiveUsers',
                title: 'Active/Inactive users',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6',
                settings: {
                    url: `/services/active_inactive_users?deviceId=${this.serviceId}`,
                    chart: {
                        chart: {
                            renderTo: 'activeInactiveUsers',
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
        ];
        injectSVG();
    }

}