import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import {WidgetController, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import {RESTService} from '../../../core/services/rest.service';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/ibm-domino/database-inventory-report.component.html',
    providers: [
        WidgetService,
        RESTService
    ]
})
export class DatabaseInventoryReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];

    currentHideServerControl: boolean = false;
    currentHideDatePanel: boolean = true;
    currentDeviceType: string = "Domino";
    currentWidgetName: string = `databaseInventoryReport`;
    currentWidgetURL: string = `/reports/database_inventory`;

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
                    id: 'databaseInventoryReport',
                    title: 'Database Inventory',
                    name: 'DatabaseInventoryList',
                    settings: { url: `/reports/database_inventory`}
                }
            ];
            injectSVG();
            

        }

}