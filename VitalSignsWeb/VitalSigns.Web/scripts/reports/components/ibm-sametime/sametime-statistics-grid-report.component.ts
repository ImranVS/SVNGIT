import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {WidgetController, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services/rest.service';

import * as helpers from '../../../core/services/helpers/helpers';

declare var injectSVG: any;




@Component({
    templateUrl: '/app/reports/components/ibm-sametime/sametime-statistics-grid-report.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
    ]
})
export class SametimeStatisticsGridReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];
    url = `/reports/sametime_stats_grid`;

    currentHideStatDropdown: boolean = true;
    currentWidgetName: string = `sametimeStatisticsGrid`;
    currentWidgetURL: string = this.url;
    currentDeviceType: string = "IBM Sametime";


    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService, 
        protected urlHelpers: helpers.UrlHelperService) {

        super(resolver, widgetService);

    }

    ngOnInit() {

        this.service.get('/navigation/sitemaps/sametime_reports')
            .subscribe
            (
            data => {
                this.contextMenuSiteMap = data;
                console.log(data)
            }
            ,
            error => console.log(error)
        );

        this.widgets = [
            {
                id: 'sametimeStatisticsGrid',
                title: 'Sametime Statistics',
                name: 'SametimeStatisticGridReportGrid',
                settings: {}
            }
        ];
        injectSVG();
        

    }
}