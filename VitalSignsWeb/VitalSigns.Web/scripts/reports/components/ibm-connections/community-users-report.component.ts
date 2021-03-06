﻿import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import {WidgetController, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import {RESTService} from '../../../core/services/rest.service';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/ibm-connections/community-users-report.component.html',
    providers: [
        RESTService
    ]
})
export class CommunityUsersReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];
    currentStatType: string = "";
    currentHideServerControl: boolean = false;
    currentHideDatePanel: boolean = true;
    currentHideStatControl: boolean = true;
    currentDeviceType: string = "IBM Connections";
    currentWidgetName: string = `communityUsers`;
    currentWidgetURL: string = `/reports/community_users`;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService) {

        super(resolver, widgetService);

    }

        ngOnInit() {

            this.service.get('/navigation/sitemaps/connections_reports')
                .subscribe
                (
                data => this.contextMenuSiteMap = data,
                error => console.log(error)
                );
            this.widgets = [
                {
                    id: 'communityUsers',
                    title: 'Community Users Report',
                    name: 'CommunityUsersList',
                    settings: {}
                }
            ];
            injectSVG();
            

        }

}