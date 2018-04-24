import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import {WidgetController, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import {RESTService} from '../../../core/services/rest.service';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/ibm-connections/connections-executive-overview-report.component.html',
    providers: [
        WidgetService,
        RESTService
    ]
})
export class ConnectionsExecutiveOverviewReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];

    currentHideServerControl: boolean = true;
    currentHideDatePanel: boolean = true;
    currentShowSingleDatePanel: boolean = true;
    currentDeviceType: string = "IBM Connections";
    currentWidgetName: string = `connectionsExecutiveOverviewList`;
    currentWidgetURL: string = `/reports/connections/executive_overview`;

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
                    id: 'connectionsExecutiveOverviewList',
                    title: '',
                    name: 'ConnectionsExecutiveOverviewList',
                    settings: { url: this.currentWidgetURL}
                }
            ];
            injectSVG();
            

        }

}