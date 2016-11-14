import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import {WidgetController, WidgetContract, WidgetService} from '../../../core/widgets';
import {Router, ActivatedRoute} from '@angular/router';
import {RESTService} from '../../../core/services/rest.service';

declare var injectSVG: any;
declare var bootstrapNavigator: any;


@Component({
    templateUrl: '/app/reports/components/ibm-domino/avg-cpu-util.component.html',
    providers: [
        WidgetService,
        RESTService
    ]
})
export class AvgCPUUtil extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];
    servers: string;

    currentHideServerControl: boolean = false;
    currentHideDatePanel: boolean = false;
    currentDeviceType: string = "Domino";
    currentWidgetName: string = `avgcpuutilchart`;
    currentWidgetURL: string = `/reports/summarystats_chart?statName=Platform.System.PctCombinedCpuUtil&deviceId=`;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService, private router: Router, private route: ActivatedRoute) {

        super(resolver, widgetService);

    }

    ngOnInit() {
        this.route.queryParams.subscribe(params => {
            this.servers = params['server-type'];
        });
        this.service.get('/navigation/sitemaps/domino_reports')
            .subscribe
            (
            data => this.contextMenuSiteMap = data,
            error => console.log(error)
            );
        this.widgets = [
            //{
            //    id: 'filter',
            //    title: 'filter',
            //    name: 'ServerFilter',
            //    device
                
            //},
            {
                id: 'avgcpuutilchart',
                title: '',
                name: 'ChartComponent',
                settings: {
                    url: `/reports/summarystats_chart?statName=Platform.System.PctCombinedCpuUtil`,
                    chart: {
                        chart: {
                            renderTo: 'avgcpuutilchart',
                            type: 'spline',
                            height: 300
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
        bootstrapNavigator();

    }
}