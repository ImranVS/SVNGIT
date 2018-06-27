import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {ActivatedRoute, Router, UrlSegment} from '@angular/router';
import {WidgetController, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import {RESTService} from '../../../core/services/rest.service';

import * as helpers from '../../../core/services/helpers/helpers';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/office365/office365-shared-mailboxes-consuming-licenses.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
    ]
})
export class Office365SharedMailboxesConsumingLicensesReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];
    isLoading: boolean = true;
    data: any[];

    constructor(
        protected resolver: ComponentFactoryResolver,
        protected widgetService: WidgetService,
        private service: RESTService,
        private router: Router,
        private route: ActivatedRoute,
        protected urlHelpers: helpers.UrlHelperService) {

        super(resolver, widgetService, true, router, route);

    }

    ngOnInit() {
        this.isLoading = true;
        super.ngOnInit();
        this.service.get('/navigation/sitemaps/office365_reports')
            .subscribe
            (
            data => this.contextMenuSiteMap = data,
            error => console.log(error)
        );
        
        this.service.get('/reports/office365_shared_mailboxes_consuming_license')
            .finally(() => this.isLoading = false )
            .subscribe(
            data => this.data = data.data,
            error => console.log(error)
            );
        injectSVG();
        

    }
}