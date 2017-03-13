import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {WidgetController, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services/rest.service';

import * as helpers from '../../../core/services/helpers/helpers';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/servers/any-statistic-report.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
    ]
})
export class AnyStatisticReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];

    url: string = `/reports/summarystats_aggregation`;

    currentWidgetName: string = `anyStatisticsGrid`;
    currentWidgetURL: string = this.url;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService,
        protected urlHelpers: helpers.UrlHelperService) {

        super(resolver, widgetService);

    }

    ngOnInit() {

        this.service.get('/navigation/sitemaps/server_reports')
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
                settings: {}
            }
        ];
        injectSVG();
        

    }

    onPropertyChanged(key: string, value: any) {
        if (key === 'widgetTitle') {
            this.widgets[0].title = value;
        }
    }
}