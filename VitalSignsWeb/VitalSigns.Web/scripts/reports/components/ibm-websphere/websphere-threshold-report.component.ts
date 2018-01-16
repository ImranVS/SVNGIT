import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import {WidgetController, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import {RESTService} from '../../../core/services/rest.service';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/ibm-websphere/websphere-threshold-report.component.html',
    providers: [
        WidgetService,
        RESTService
    ]
})
export class WebsphereThresholdReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService) {

        super(resolver, widgetService);

    }

        ngOnInit() {

            this.service.get('/navigation/sitemaps/websphere_reports')
                .subscribe
                (
                data => this.contextMenuSiteMap = data,
                error => console.log(error)
                );
            this.widgets = [
                {
                    id: 'websphereThresholdReport',
                    title: 'IBM WebSphere Threshold Report',
                    name: 'WebsphereThresholdList', 
                    settings: {}
                }
            ];
            injectSVG();
            

        }

}