import { Component, OnInit, ComponentFactoryResolver, ComponentFactory, ElementRef, ComponentRef, ViewChild, ViewContainerRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpModule } from '@angular/http';
import { WidgetComponent } from '../../../core/widgets';
import { WidgetService } from '../../../core/widgets/services/widget.service';
import { RESTService } from '../../../core/services';
import { ServiceTab } from '../../../services/models/service-tab.interface';
import * as ServiceTabs from '../../../services/service-tab.collection';
import { WidgetController, WidgetContainer, WidgetContract } from '../../../core/widgets';
import { MsExchangeDetail } from './ms-exchange-details.component';
declare var injectSVG: any;


@Component({
    templateUrl: '/app/dashboards/components/ms-exchange/ms-exchange-dashboard.component.html',
    providers: [WidgetService, HttpModule,
        RESTService]
})
export class MSExchangeDashboard extends WidgetController implements OnInit {

    widgets: WidgetContract[] = [
        {
            id: 'serverRoles',
            title: 'Total Server Roles',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
            settings: {
                url: '/services/status_count?type=Exchange&docfield=response_time',
                chart: {
                    chart: {
                        renderTo: 'serverRoles',
                        type: 'pie',
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
            id: 'serverStatus',
            title: 'By Status',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
            settings: {
                url: '/services/status_count?type=Exchange&docfield=status_code',
                chart: {
                    chart: {
                        renderTo: 'serverStatus',
                        type: 'pie',
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
            id: 'serverOs',
            title: 'Operating Systems',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
            settings: {
                url: '/services/status_count?type=Exchange&docfield=operating_system',
                chart: {
                    chart: {
                        renderTo: 'serverOs',
                        type: 'pie',
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
        }
        //{
        //    id: 'msexchange details',
        //    name: 'MSExchangeGrid',
        //    css: 'col-xs-12 col-sm-12 col-md-12 col-lg-12',
        //}

    ]



    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService,
        private dataProvider: RESTService,
        private route: ActivatedRoute) {
        super(resolver, widgetService);
    }

    ngOnInit() {

        injectSVG();

    }
    ngAfterViewChecked() {
        injectSVG();
    }

}