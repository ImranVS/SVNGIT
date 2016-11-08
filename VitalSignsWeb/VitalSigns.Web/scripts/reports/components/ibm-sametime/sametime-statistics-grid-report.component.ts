﻿import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {WidgetController, WidgetContract, WidgetService} from '../../../core/widgets';
import {RESTService} from '../../../core/services/rest.service';

import * as helpers from '../../../core/services/helpers/helpers';

declare var injectSVG: any;
declare var bootstrapNavigator: any;


@Component({
    templateUrl: '/app/reports/components/ibm-sametime/sametime-statistics-grid-report.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
    ]
})
export class SametimeStatisticsGridReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService, 
        protected urlHelpers: helpers.UrlHelperService) {

        super(resolver, widgetService);

    }

    ngOnInit() {

        this.service.get('/navigation/sitemaps/sametime_reports')
            .subscribe
            (
            data => {
                this.contextMenuSiteMap = data;
                console.log(data)
            }
            ,
            error => console.log(error)
        );

        this.widgets = [
            {
                id: 'sametimeStatisticsGrid',
                title: 'Sametime Statistics',
                name: 'SametimeStatisticGridReportGrid',
                settings: {}
            }
        ];
        injectSVG();
        bootstrapNavigator();

    }
}