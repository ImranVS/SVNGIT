import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {WidgetController, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services/rest.service';

import * as helpers from '../../../core/services/helpers/helpers';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/mail/mail-volume-report.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
    ]
})
export class MailVolumeReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];

    gridUrl: string = `/reports/summarystats_aggregation`;

    currentWidgetName: string = `anyStatisticsGrid`;
    currentWidgetURL: string = this.gridUrl;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService,
        protected urlHelpers: helpers.UrlHelperService) {

        super(resolver, widgetService);

    }

    ngOnInit() {

        this.service.get('/navigation/sitemaps/mail_reports')
            .subscribe
            (
            data => this.contextMenuSiteMap = data,
            error => console.log(error)
            );
        var todaysDate = new Date();
        var endDate = todaysDate.toISOString();
        todaysDate.setMonth(todaysDate.getMonth() - 1);
        var startDate = todaysDate.toISOString();
        this.gridUrl = `/reports/summarystats_aggregation?type=Domino&aggregationType=sum&statName=Mail.TotalRouted&startDate=${startDate}&endDate=${endDate}`;
        this.widgets = [
            {
                id: 'anyStatisticsGrid',
                title: '',
                name: 'AnyStatisticReportGrid',
                settings: {}
            }
        ];
        injectSVG();
        

    }
}