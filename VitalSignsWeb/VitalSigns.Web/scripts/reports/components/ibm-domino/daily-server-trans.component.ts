import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {WidgetController, WidgetContract, WidgetService} from '../../../core/widgets';

import {RESTService} from '../../../core/services/rest.service';
import * as helpers from '../../../core/services/helpers/helpers';
declare var injectSVG: any;
declare var bootstrapNavigator: any;


@Component({
    templateUrl: '/app/reports/components/domino/daily-server-trans.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
    ]
})
export class DailyServerTrans extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];

    currentHideServerControl: boolean = false;
    currentHideDatePanel: boolean = false;
    currentDeviceType: string = "Domino";
    currentWidgetName: string = `dailyservertranschart`;
    currentWidgetURL: string = `/reports/summarystats_chart?statName=Server.Trans.PerMinute&deviceId=`;
    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService, private route: ActivatedRoute, protected urlHelpers: helpers.UrlHelperService) {

        super(resolver, widgetService);

    }

    ngOnInit() {

        this.service.get('/navigation/sitemaps/domino_reports')
            .subscribe
            (
            data => this.contextMenuSiteMap = data,
            error => console.log(error)
            );
        this.widgets = [
            {
                id: 'dailyservertranschart',
                title: '',
                name: 'ChartComponent',
                settings: {
                    //deviceid=57ace45abf46711cd4681e15&
                    url: '/reports/summarystats_chart?statName=Server.Trans.PerMinute',
                    chart: {
                        chart: {
                            renderTo: 'dailyservertranschart',
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