import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {WidgetController, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services/rest.service';

import * as helpers from '../../../core/services/helpers/helpers';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/configuration/server-list-type-report.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
    ]
})
export class ServerListTypeReport extends WidgetController {
    contextMenuSiteMap: any;
    docField: string = "device_type";
    widgets: WidgetContract[];

    currentWidgetName: string = `serverListGrid`;
    currentWidgetURL: string = `/reports/server_list`;
    currentDocField: string = "device_type";

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
                title: 'Server List by Type',
                name: 'ServerListTypeReportGrid',
                settings: {}
            }
        ];
        injectSVG();
        

    }
}