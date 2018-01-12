import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {ActivatedRoute, Router, UrlSegment} from '@angular/router';
import {WidgetController, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import {RESTService} from '../../../core/services/rest.service';

import * as helpers from '../../../core/services/helpers/helpers';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/office365/office365-group-collection.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
    ]
})
export class O365GroupCollectionReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];
    paramtype: string;
    title: string;

    currentHideServerControl: boolean = false;
    currentHideDatePanel: boolean = false;
    currentShowSingleDatePanel: boolean = true;
    currentWidgetName: string = `GroupList`;
    currentWidgetURL: string;

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
        //super.ngOnInit();
        this.service.get('/navigation/sitemaps/office365_reports')
            .subscribe
            (
            data => this.contextMenuSiteMap = data ,
            error => console.log(error)
        );
        
        this.widgets = [
            {
                id: 'o365groupReport',
                title: '',
                name: 'O365GroupList',
                //settings: { deviceType: deviceType, mailboxType: mailboxType }
            }
        ];
        injectSVG();
        

    }
}