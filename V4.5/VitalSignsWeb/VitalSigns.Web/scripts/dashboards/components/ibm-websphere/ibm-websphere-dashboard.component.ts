import {Component, ComponentFactoryResolver, Input, OnInit} from '@angular/core';

import 'rxjs/Rx';

import {WidgetController, WidgetContainer, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import { AppNavigator } from '../../../navigation/app.navigator.component';
import { RESTService } from '../../../core/services';

declare var injectSVG: any;


@Component({
    selector: 'websphere-dashboard',
    templateUrl: '/app/dashboards/components/ibm-websphere/ibm-websphere-dashboard.component.html',
    providers: [WidgetService, RESTService]
})

export class IBMWebsphereDashboard extends WidgetController implements OnInit {
    serviceId: string;
    multiplier: number = 500;
     data: wijmo.collections.CollectionView;
     widgets: WidgetContract[];
     rowCount: number = 25;
    
    constructor(private service: RESTService, protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {
        super(resolver, widgetService);
    }

    ngOnInit() {
        this.serviceId = this.widgetService.getProperty('serviceId');

        this.service.get('/services/status_list?type=WebSphere')
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                this.rowCount = this.data.itemCount;
                console.log("Found " + this.rowCount + " websphere cells");
            }
         
            );



  
        this.widgets = [
            {
                id: 'websphereGrid',
                title: 'Cells',
                name: 'IBMWebsphereGrid',
                css: 'col-xs-12 col-sm-12  col-md-12 col-lg-12',
                settings: {

                }
            },
            {
                id: 'websphereNodeGrid',
                title: 'Nodes',
                name: 'IBMWebsphereNodeGrid',
                css: 'col-xs-12 col-sm-12  col-md-12 col-lg-12',
                settings: {

                }
            },
            {
                id: 'websphereServerGrid',
                title: 'Servers',
                name: 'IBMWebsphereServerGrid',
                css: 'col-xs-12 col-sm-12  col-md-12 col-lg-12',
                settings: {

                }
            },
            {
                id: 'serverStatus',
                title: 'Status',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
                settings: {
                    url: '/services/status_count?type=[WebSphereCell,WebSphereNode,WebSphere]&docfield=status_code',
                    chart: {
                        chart: {
                            renderTo: 'serverStatus',
                            type: 'pie',
                            height: 350
                        },
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            categories: []
                        },
                        yAxis: {

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
                id: 'activeThreads',
                title: 'Active Thread Count',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
                settings: {
                    url: `/services/statistics?statName=ActiveThreadCount&operation=AVG&isChart=true`,
                    chart: {
                        chart: {
                            renderTo: 'activeThreads',
                            type: 'bar',
                            height: this.rowCount * 15
                        },
                        colors: ['#5fbe7f'],
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            labels: {
                                step: 1
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
                id: 'hungThreads',
                title: 'Hung Thread Count',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
                settings: {
                    url: `/services/statistics?statName=CurrentHungThreadCount&operation=AVG&isChart=true`,
                    chart: {
                        chart: {
                            renderTo: 'hungThreads',
                            type: 'bar',
                            height: this.rowCount * 15
                        },
                        colors: ['#5fbe7f'],
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            labels: {
                                step: 1
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

    onSelect(serviceId: string) {

        this.serviceId = serviceId;

    }

}