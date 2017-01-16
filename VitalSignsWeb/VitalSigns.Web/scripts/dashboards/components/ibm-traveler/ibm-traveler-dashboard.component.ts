import {Component, ComponentFactoryResolver, Input, OnInit} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {AppNavigator} from '../../../navigation/app.navigator.component';
import {RESTService} from '../../../core/services';
import {DominoServerInfo} from '../../../widgets/main-dashboard/models/domino-server-info';

declare var injectSVG: any;


@Component({
    selector: 'ibm-traveler-dashboard',
    templateUrl: '/app/dashboards/components/ibm-traveler/ibm-traveler-dashboard.component.html',
    providers: [WidgetService]
})
export class IBMTravelerDashboard extends WidgetController implements OnInit {
    widgets: WidgetContract[] = [
        {
            id: 'travelerGrid',
            title: 'Traveler Info',
            name: 'IBMTravelerGrid',
            css: 'col-xs-12 col-sm-12  col-md-12 col-lg-11',
            settings: {
                
            }
        },
        {
            id: 'travelerStatus',
            title: 'Status',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-3',
            settings: {
                url: '/services/status_count?type=Domino&docfield=traveler_status',
                chart: {
                    chart: {
                        renderTo: 'travelerStatus',
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
            id: 'travelerVersion',
            title: 'Versions',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-3',
            settings: {
                url: '/services/status_count?type=Domino&docfield=traveler_version',
                chart: {
                    chart: {
                        renderTo: 'travelerVersion',
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
            id: 'travelerUsers',
            title: 'Users',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-5',
            settings: {
                url: '/services/status_list?type=Traveler&docfield=traveler_users&isChart=true',
                chart: {
                    chart: {
                        renderTo: 'travelerUsers',
                        type: 'bar',
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
        }
    ]
    
    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {
        super(resolver, widgetService);
    }

    ngOnInit() {
        injectSVG();
        
    }
}