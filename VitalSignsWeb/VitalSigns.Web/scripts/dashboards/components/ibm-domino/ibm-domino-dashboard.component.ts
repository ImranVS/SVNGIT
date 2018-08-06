import {Component, ComponentFactoryResolver, Input, OnInit} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {AppNavigator} from '../../../navigation/app.navigator.component';
import {RESTService} from '../../../core/services';
import {DominoServerInfo} from '../../../widgets/main-dashboard/models/domino-server-info';

declare var injectSVG: any;


@Component({
    selector: 'sample-dashboard',
    templateUrl: '/app/dashboards/components/ibm-domino/ibm-domino-dashboard.component.html',
    providers: [WidgetService]
})
export class IBMDominoDashboard extends WidgetController implements OnInit {
    widgets: WidgetContract[] = [
        {
            id: 'dominoGrid',
            title: 'Domino Info',
            name: 'IBMDominoGrid',
            css: 'col-xs-12 col-sm-12  col-md-12 col-lg-8',
            settings: {
                
            }
        },
        {
            id: 'serverRoles',
            title: 'Roles',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-3 col-md-3 col-lg-3',
            settings: {
                url: '/services/status_count?type=Domino&docfield=secondary_role',
                chart: {
                    chart: {
                        renderTo: 'serverRoles',
                        type: 'pie',
                        height: 145
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
                            innerSize: '70%',
                            size: '200%'
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
            title: 'Status',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-3 col-md-3 col-lg-3',
            settings: {
                url: '/services/status_count?type=Domino&docfield=status_code',
                chart: {
                    chart: {
                        renderTo: 'serverStatus',
                        type: 'pie',
                        height: 145
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
                            innerSize: '70%',
                            size: '260%'
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
            id: 'category',
            title: 'Category',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-3 col-md-3 col-lg-3',
            settings: {
                url: '/services/status_count?type=Domino&docfield=category',
                chart: {
                    chart: {
                        renderTo: 'category',
                        type: 'pie',
                        height: 145
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
                            innerSize: '70%',
                            size: '200%'
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
            css: 'col-xs-12 col-sm-3 col-md-3 col-lg-3',
            settings: {
                url: '/services/status_count?type=Domino&docfield=operating_system',
                chart: {
                    chart: {
                        renderTo: 'serverOs',
                        type: 'pie',
                        height: 145
                    },
                    title: { text: '' },
                    subtitle: { text: '' },
                    xAxis: {
                        categories: []
                    },
                    yAxis: {
                        min: 500,
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
                            innerSize: '70%',
                            size:'260%'
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

    ]
    
    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {
        super(resolver, widgetService);
    }

    ngOnInit() {
        injectSVG();
        
    }

}