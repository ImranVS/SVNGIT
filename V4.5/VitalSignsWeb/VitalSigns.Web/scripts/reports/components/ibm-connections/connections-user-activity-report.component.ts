import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import {WidgetController, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import {RESTService} from '../../../core/services/rest.service';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/ibm-connections/connections-user-activity-report.component.html',
    providers: [
        WidgetService,
        RESTService
    ]
})
export class ConnectionsUserActivityReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];
    currentHideServerControl: boolean = false;
    currentHideDatePanel: boolean = true;
    currentHideStatControl: boolean = true;
    currentDeviceType: string = "IBM Connections";
    currentWidgetName: string = `connectionsUserActivityReport`;
    currentWidgetURL: string = `/reports/connections/user_activity`;

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
                    id: 'connectionsUserActivityReport',
                    title: '',
                    name: 'UserActivityList',
                    settings: { url: this.currentWidgetURL}
                }
            ];
            injectSVG();
            

        }

}