import { Component, ComponentFactoryResolver, OnInit } from '@angular/core';
import { WidgetController, WidgetContract } from '../../../core/widgets';
import { WidgetService } from '../../../core/widgets/services/widget.service';
import { RESTService } from '../../../core/services/rest.service';

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
    baseUrl: string = `/reports/summarystats_aggregation?type=Domino&statName=[Mail.Transferred,Mail.TotalRouted,Mail.Delivered]`
    gridUrl: string = this.baseUrl + `&aggregationType=sum`;
    currentWidgetName: string = `anyStatisticsGrid`;
    currentWidgetURL: string = this.baseUrl;
    currentHideAggregationControl: boolean = false;
    currentHideServerControl: boolean = true;
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
        this.widgets = [
            {
                id: 'anyStatisticsGrid',
                title: '',
                name: 'AnyStatisticReportGrid',
                settings: { url: this.gridUrl }
            }
        ];
        injectSVG();


    }

}