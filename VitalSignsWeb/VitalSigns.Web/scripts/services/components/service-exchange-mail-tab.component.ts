import {Component, ComponentFactoryResolver, OnInit, Injector} from '@angular/core';
import { RESTService } from '../../core/services';
import { ActivatedRoute } from '@angular/router';

import {WidgetController, WidgetContainer, WidgetContract} from '../../core/widgets';
import {WidgetService} from '../../core/widgets/services/widget.service';

import {ServiceTab} from '../models/service-tab.interface';

declare var injectSVG: any;

@Component({
    selector: 'tab-overall',
    templateUrl: '/app/services/components/service-exchange-mail-tab.component.html',
    providers: [WidgetService]
})
export class ServiceExchangeMailTab extends WidgetController implements OnInit, ServiceTab {

    widgets: WidgetContract[];
    serviceId: string;
    deviceId: any;
    service: any;
    errorMessage: string;
    submissionQueue: string;
    unreachableQueue: string;
    shadowQueue: string;

    constructor(private dataProvider: RESTService, protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private route: ActivatedRoute) {
        super(resolver, widgetService);
    }
    
    ngOnInit() {
        this.route.params.subscribe(params => {
            //IMPORTANT: the # symbol must be escaped in the URL, otherwise query string gets cut off at the first occurrence
            this.dataProvider.get(`/services/statistics?deviceId=${this.serviceId}&statName=[CAS@Unreachable%23Queues,CAS@Submission%23Queues,CAS@Shadow%23Queues]&operation=AVG`)
                .subscribe(
                data => {
                    this.service = data.data;
                    console.log(this.service['StatName']);
                },
                error => this.errorMessage = <any>error
                );

        });
        this.widgets = [
            {
                id: 'queueHourly',
                title: 'Queues',
                name: 'ChartComponent',
                css: 'col-xs-12 col-sm-12 col-md-12 col-lg-12',
                settings: {
                    url: `/services/statistics?deviceId=${this.serviceId}&statName=[CAS@Unreachable%23Queues,CAS@Submission%23Queues,CAS@Shadow%23Queues]&operation=HOURLY&isChart=true`,
                    dateformat: 'time',
                    chart: {
                        chart: {
                            renderTo: 'queueHourly',
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
                                text: 'value'
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
            }
        ]
        injectSVG();
    }

}