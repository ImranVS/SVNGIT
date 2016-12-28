import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {WidgetController, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import {RESTService} from '../../../core/services/rest.service';

import * as helpers from '../../../core/services/helpers/helpers';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/ibm-websphere/websphere-heap-size.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
    ]
})
export class WebSphereCurrentHeapSize extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];
    param: string;

    currentHideServerControl: boolean = false;
    currentHideDatePanel: boolean = false;
    currentDeviceType: string = "WebSphere";
    currentWidgetName: string = `heapsizechart`;
    currentWidgetURL: string = `/reports/summarystats_chart?statName=CurrentHeapSize&type=WebSphere`;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService,
        private route: ActivatedRoute, protected urlHelpers: helpers.UrlHelperService) {

        super(resolver, widgetService);

    }

    ngOnInit() {
        let paramtype = null;
        this.route.queryParams.subscribe(params => paramtype = params['type']);
        this.param = paramtype;
        this.service.get('/navigation/sitemaps/websphere_reports')
            .subscribe
            (
            data => this.contextMenuSiteMap = data,
            error => console.log(error)
            );


        this.widgets = [
            {
                id: 'heapsizechart',
                title: '',
                name: 'ChartComponent',
                settings: {
                    url: this.currentWidgetURL,
                    dateformat: "date",
                    chart: {
                        chart: {
                            renderTo: 'heapsizechart',
                            type: 'spline',
                            height: 540
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
                                text: 'Current Heap Size'
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
        ];
        injectSVG();
        

    }
}