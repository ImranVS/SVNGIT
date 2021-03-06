﻿import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {WidgetController, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services/rest.service';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';

import * as helpers from '../../../core/services/helpers/helpers';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/mobile-users/mobile-users-report.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
    ]
})
export class MobileUsersReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];
    title: string;
    paramtype: string;
    url: string = `/dashboard/mobile_user_devices`;

    currentWidgetName: string = `mobileUsersGrid`;
    currentWidgetURL: string = this.url;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService,
        private router: Router, private route: ActivatedRoute,
        protected urlHelpers: helpers.UrlHelperService) {

        super(resolver, widgetService, true, router, route);

    }

    ngOnInit() {
        super.ngOnInit();
        this.service.get('/navigation/sitemaps/mobile_users')
            .subscribe
            (
            data => this.contextMenuSiteMap = data,
            error => console.log(error)
            );
        this.route.queryParams.subscribe(params => {
            this.paramtype = params['isinactive'];
        });
        if (this.paramtype == 'true') {
            this.title = "Inactive";
        }
        this.widgets = [
            {
                id: 'mobileUsersGrid',
                title: this.title,
                name: 'MobileUsersReportGrid',
                settings: {}
            }
        ];
        injectSVG();
        

    }

    onPropertyChanged(key: string, value: any) {
        if (key === 'widgetTitle') {
            this.widgets[0].title = value;
        }
    }
}