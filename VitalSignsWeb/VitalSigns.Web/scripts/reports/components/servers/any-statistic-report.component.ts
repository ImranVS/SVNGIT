﻿import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {WidgetController, WidgetContract, WidgetService} from '../../../core/widgets';
import {RESTService} from '../../../core/services/rest.service';

import * as helpers from '../../../core/services/helpers/helpers';

declare var injectSVG: any;
declare var bootstrapNavigator: any;


@Component({
    templateUrl: '/app/reports/components/servers/any-statistic-report.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
    ]
})
export class AnyStatisticReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];

    url: string = `/reports/summarystats_aggregation`;

    currentWidgetName: string = `anyStatisticsGrid`;
    currentWidgetURL: string = this.url;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService,
        protected urlHelpers: helpers.UrlHelperService) {

        super(resolver, widgetService);

    }

    ngOnInit() {

        this.service.get('/navigation/sitemaps/server_reports')
            .subscribe
            (
            data => this.contextMenuSiteMap = data,
            error => console.log(error)
            );
        this.widgets = [
            {
                id: 'anyStatisticsGrid',
                title: 'Any Statistic',
                name: 'AnyStatisticReportGrid',
                settings: {}
            }
        ];
        injectSVG();
        bootstrapNavigator();

    }
}