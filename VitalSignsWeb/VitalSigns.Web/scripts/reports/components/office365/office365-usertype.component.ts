import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {ActivatedRoute, Router, UrlSegment} from '@angular/router';
import {WidgetController, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import {RESTService} from '../../../core/services/rest.service';

import * as helpers from '../../../core/services/helpers/helpers';

declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/office365/office365-usertype.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
    ]
})
export class Office365UserTypeReport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];
    paramtype: string;
    title: string;
    currentHideServerControl: boolean = false;
    currentHideDatePanel: boolean = false;
    currentShowSingleDatePanel: boolean = true;
    //currentDeviceType: string = "Office365";
    currentWidgetName: string = `usertypeList`;
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
        super.ngOnInit();
        //let deviceType = "";
        let inactive = "";
        this.route.queryParams.subscribe(params => {
           // deviceType = params['deviceType'];
            inactive = params['inactive'];
            this.title = params['title'];
        });

        this.service.get('/navigation/sitemaps/office365_reports')
            .subscribe
            (
            data => this.contextMenuSiteMap = data ,
            error => console.log(error)
        );
        
        this.widgets = [
            {
                id: 'usertypeList',
                title: '',
                name: 'UserTypeList',
                css: 'col-xs-12 col-sm-12  col-md-12 col-lg-12',
                settings: { inactive: inactive }
            }
        ];
        injectSVG();
        

    }
}