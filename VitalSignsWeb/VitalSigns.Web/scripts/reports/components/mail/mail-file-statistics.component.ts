import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import {WidgetController, WidgetContract, WidgetService} from '../../../core/widgets';

import {RESTService} from '../../../core/services/rest.service';

declare var injectSVG: any;
declare var bootstrapNavigator: any;


@Component({
    templateUrl: '/app/reports/components/mail/mail-file-statistics.component.html',
    providers: [
        WidgetService,
        RESTService
    ]
})
export class MailFileStatisticsReport extends WidgetController {
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
                id: 'mailFileStatsTable',
                title: '',
                name: 'MailFileStatisticsList',
                settings: {}
            }
        ];
        injectSVG();
        bootstrapNavigator();

    }
}