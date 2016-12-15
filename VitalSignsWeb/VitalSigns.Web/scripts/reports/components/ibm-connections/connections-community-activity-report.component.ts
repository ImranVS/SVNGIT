import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import {WidgetController, WidgetContract, WidgetService} from '../../../core/widgets';

import {RESTService} from '../../../core/services/rest.service';

declare var injectSVG: any;
declare var bootstrapNavigator: any;


@Component({
    templateUrl: '/app/reports/components/ibm-connections/connections-community-activity-report.component.html',
    providers: [
        WidgetService,
        RESTService
    ]
})
export class ConnectionsCommunityActivityReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];

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
                    id: 'connectionsCommunityActivityReport',
                    title: '',
                    name: 'CommunityActivityList',
                    settings: { url: `/reports/connections/community_activity`}
                }
            ];
            injectSVG();
            bootstrapNavigator();

        }

}