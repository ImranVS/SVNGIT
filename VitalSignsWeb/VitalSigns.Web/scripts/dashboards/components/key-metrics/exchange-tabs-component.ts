import {Component, OnInit, ComponentFactoryResolver, ComponentFactory, ElementRef, ComponentRef, ViewChild, ViewContainerRef} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';

import {RESTService} from '../../../core/services';

import * as ServiceTabs from './exchange-tab.collection';

import { WidgetController, WidgetContainer, WidgetContract } from '../../../core/widgets';
import { WidgetComponent } from '../../../core/widgets';
import { WidgetService } from '../../../core/widgets/services/widget.service';
//import 'reflect-metadata'
//import '../../../../node_modules/reflect-metadata/Reflect';

declare var injectSVG: any;


//function InheritPropMetadata() {
//    return (target: Function) => {
//        const targetProps = Reflect.getMetadata('propMetadata', target);
//        const parentTarget = Object.getPrototypeOf(target.prototype).constructor;
//        const parentProps = Reflect.getMetadata('propMetadata', parentTarget);

//        const mergedProps = Object.assign(targetProps, parentProps);

//        Reflect.defineMetadata('propMetadata', mergedProps, target);
//    };
//};


@Component({
    templateUrl: '/app/dashboards/components/key-metrics/exchange-tabs-component.html',
    providers: [
        HttpModule,
        RESTService,
        WidgetService
    ]
})
//export class ExchnagesTab extends WidgetController implements OnInit {
export class ExchnagesTab implements OnInit {
    @ViewChild('tab', { read: ViewContainerRef }) target: ViewContainerRef;

    tabsData: any;
    activeTabComponent: ComponentRef<{}>;

    widgets: WidgetContract[] = [
        {
            id: 'mailboxTypes',
            title: 'Mailbox Types',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
            settings: {
                url: '/dashboard/exchange_mailboxes_types_chart',
                chart: {
                    chart: {
                        renderTo: 'mailboxTypes',
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
            id: 'lastLogins',
            title: 'Last Logon',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
            settings: {
                url: '/dashboard/exchange_mailboxes_last_logon',
                chart: {
                    chart: {
                        renderTo: 'lastLogins',
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
            id: 'mailboxDatabases',
            title: 'Mailboxes in Databases',
            name: 'ChartComponent',
            css: 'col-xs-12 col-sm-6 col-md-6 col-lg-4',
            settings: {
                url: '/dashboard/exchange_mailbox_database',
                chart: {
                    chart: {
                        renderTo: 'mailboxDatabases',
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

    ]

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService,
        private dataProvider: RESTService,
        private route: ActivatedRoute) {
        //super(resolver, widgetService);
    }
    //constructor(private resolver: ComponentFactoryResolver, private elementRef: ElementRef) { }
    selectTab(tab: any) {
        // Activate selected tab
        this.tabsData.forEach(tab => tab.active = false);
        tab.active = true;
        // Dispose current tab if one already active
        if (this.activeTabComponent)
            this.activeTabComponent.destroy();
        let factory = this.resolver.resolveComponentFactory(ServiceTabs[tab.component]);
        this.activeTabComponent = this.target.createComponent(factory);
    }
    ngOnInit() {
        this.tabsData = [
            {
                "title": "Mailboxes",
                "component": "ExchangemailstatisticsviewGrid",
                "path": "/app/dashboards/components/key-metrics/exchange-mailbox-view.component",
                "active": false
            },
            {
                "title": "Users ",
                "component": "ExchangemailAccessviewGrid",
                "path": "/app/dashboards/components/key-metrics/exchange-mail-access-view.component",
                "active": false
            },   
        ];
        this.selectTab(this.tabsData[0]);

        injectSVG();
    };
    ngAfterViewChecked() {
        injectSVG();
    }
}