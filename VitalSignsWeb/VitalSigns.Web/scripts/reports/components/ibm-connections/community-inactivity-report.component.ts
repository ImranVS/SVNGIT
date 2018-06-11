import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import {WidgetController, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import {RESTService} from '../../../core/services/rest.service';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/ibm-connections/community-inactivity-report.component.html',
    providers: [
        RESTService
    ]
})
export class ConnectionsInactiveCommunityReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];
    currentHideServerControl: boolean = false;
    currentHideDatePanel: boolean = true;
    currentDeviceType: string = "IBM Connections";
    currentWidgetName: string = `communityList`;
    currentWidgetURL: string = '/reports/connections/inactive_community_list';
    currentHideStatControl: boolean = true;

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
                    id: 'communityList',
                    title: 'Inactive Communities',
                    name: 'InactiveCommunityList',
                    settings: {}
                }
            ];
            injectSVG();
            

        }

}