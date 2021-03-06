﻿import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {WidgetController, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services/rest.service';

import * as helpers from '../../../core/services/helpers/helpers';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/configuration/server-list-location-report.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
    ]
})
export class ServerListLocationReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];

    currentWidgetName: string = `serverListGrid`;
    currentWidgetURL: string = `/reports/server_list`;
    currentDocField: string = "location_id";

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService,
        protected urlHelpers: helpers.UrlHelperService) {

        super(resolver, widgetService);

    }

    ngOnInit() {

        this.service.get('/navigation/sitemaps/configuration_reports')
            .subscribe
            (
            data => this.contextMenuSiteMap = data,
            error => console.log(error)
            );
        this.widgets = [
            {
                id: 'serverListGrid',
                title: 'Server List by Location',
                name: 'ServerListLocationReportGrid',
                settings: {}
            }
        ];
        injectSVG();
        

    }
}