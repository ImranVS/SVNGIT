import {Component, ComponentFactoryResolver, Input, OnInit} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../../core/widgets';
import {AppNavigator} from '../../../navigation/app.navigator.component';
import {RESTService} from '../../../core/services';
import {DominoServerInfo} from '../../../widgets/main-dashboard/models/domino-server-info';

declare var injectSVG: any;
declare var bootstrapNavigator: any;

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
        bootstrapNavigator();
    }

}