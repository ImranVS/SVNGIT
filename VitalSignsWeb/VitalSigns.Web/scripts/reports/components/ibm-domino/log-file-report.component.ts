import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import {WidgetController, WidgetContract, WidgetService} from '../../../core/widgets';

import {RESTService} from '../../../core/services/rest.service';

declare var injectSVG: any;


@Component({
    templateUrl: '/app/reports/components/ibm-domino/log-file-report.component.html',
    providers: [
        WidgetService,
        RESTService
    ]
})
export class LogFileReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService) {

        super(resolver, widgetService);

    }

        ngOnInit() {

            this.service.get('/navigation/sitemaps/domino_reports')
                .subscribe
                (
                data => this.contextMenuSiteMap = data,
                error => console.log(error)
                );
            this.widgets = [
                {
                    id: 'logFileReport',
                    title: 'Log File Report',
                    name: 'LogFileList',
                    settings: {}
                }
            ];
            injectSVG();
            

        }

}