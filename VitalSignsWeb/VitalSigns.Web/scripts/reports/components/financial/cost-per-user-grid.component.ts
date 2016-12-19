import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import {WidgetController, WidgetContract, WidgetService} from '../../../core/widgets';

import {RESTService} from '../../../core/services/rest.service';

declare var injectSVG: any;


@Component({
    templateUrl: '/app/reports/components/financial/cost-per-user-grid.component.html',
    providers: [
        WidgetService,
        RESTService
    ]
})
export class CostPerUserGridReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService) {

        super(resolver, widgetService);

    }

    ngOnInit() {

        this.service.get('/navigation/sitemaps/financial_reports')
            .subscribe
            (
            data => this.contextMenuSiteMap = data,
            error => console.log(error)
            );
        this.widgets = [
            {
                id: 'costPerUserGrid',
                title: '',
                name: 'CostPerUserPivotGrid',
                settings: {}
            }
        ];
        injectSVG();
        

    }
}