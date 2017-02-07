import {Component, ComponentFactoryResolver, OnInit, Injector} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract} from '../../core/widgets';
import {WidgetService} from '../../core/widgets/services/widget.service';

import {ServiceTab} from '../models/service-tab.interface';

declare var injectSVG: any;

@Component({
    selector: 'tab-overall',
    templateUrl: '/app/services/components/service-websphere-overall-tab.component.html',
    providers: [WidgetService]
})
export class WebSphereOverallTab extends WidgetController implements OnInit, ServiceTab {

    widgets: WidgetContract[];
    serviceId: string;
    deviceId: any;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {
        super(resolver, widgetService);
    }
    
    ngOnInit() {
    
        this.widgets = [
            {
                id: 'heapSize',
                title: 'Heap Size',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                settings: {
                    url: `/services/statistics?statname=CurrentHeapSize&deviceId=${this.serviceId}&operation=hourly`,
                    dateformat: 'time',
                    chart: {
                        chart: {
                            renderTo: 'heapSize',
                            type: 'areaspline',
                            height: 300
                        },
                        //colors: ['#5fbe7f'],
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            labels: {
                                step: 6
                            },
                            categories: [],
                            title: {
                                //text: 'Time'
                            }
                        },
                        yAxis: {
                            title: {
                                text: 'Memory (MB)'
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
                id: 'poolCount',
                title: 'Pool Count',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                settings: {
                    url: `/services/statistics?statname=AverageThreadCount&deviceId=${this.serviceId}&operation=hourly`,
                    dateformat: 'time',
                    chart: {
                        chart: {
                            renderTo: 'poolCount',
                            type: 'areaspline',
                            height: 300
                        },
                        //colors: ['#5fbe7f'],
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            labels: {
                                step: 6
                            },
                            categories: [],
                            title: {
                                //text: 'Time'
                            }
                        },
                        yAxis: {
                            title: {
                                text: 'Count'
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
                id: 'responseTime',
                title: 'Response Time',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                settings: {
                    url: `/services/statistics?statname=ResponseTime&deviceId=${this.serviceId}&operation=hourly`,
                    dateformat: 'time',
                    chart: {
                        chart: {
                            renderTo: 'responseTime',
                            type: 'areaspline',
                            height: 300
                        },
                        //colors: ['#5fbe7f'],
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            labels: {
                                step: 6
                            },
                            categories: [],
                            title: {
                                //text: 'Time'
                            }
                        },
                        yAxis: {
                            title: {
                                text: 'ms'
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
                id: 'cpuUsage',
                title: 'CPU Usage',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-12 col-md-6 col-lg-6',
                settings: {
                    url: `/services/statistics?statname=ProcessCPUUsage&deviceId=${this.serviceId}&operation=hourly`,
                    dateformat: 'time',
                    chart: {
                        chart: {
                            renderTo: 'cpuUsage',
                            type: 'areaspline',
                            height: 300
                        },
                        //colors: ['#5fbe7f'],
                        title: { text: '' },
                        subtitle: { text: '' },
                        xAxis: {
                            labels: {
                                step: 6
                            },
                            categories: [],
                            title: {
                                //text: 'Time'
                            }
                        },
                        yAxis: {
                            title: {
                                text: 'Percent'
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
        ];
    
        injectSVG();
    }

}