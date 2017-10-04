import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {ActivatedRoute, Router, UrlSegment} from '@angular/router';
import {WidgetController, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services/rest.service';
import * as helpers from '../../../core/services/helpers/helpers';
declare var injectSVG: any;



@Component({
    templateUrl: '/app/reports/components/ms-exchange/ms-exchange-mailbox.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
    ]
})
export class exchnagemaillistreport extends WidgetController {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];
    paramtype: string;
    title: string;

    currentHideServerControl: boolean = false;
    currentHideDatePanel: boolean = false;
    currentShowSingleDatePanel: boolean = true;
    currentDeviceType: string = "Exchange";
    currentWidgetName: string = `ExchangemailboxList`;
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
        this.service.get('/navigation/sitemaps/exchange_reports')
            .subscribe
            (
            data => this.contextMenuSiteMap = data ,
            error => console.log(error)
        );
        
        this.widgets = [
            {
                id: 'exchangemailboxReport',
                title: '',
                name: 'ExchnagemailboxList',
            }
        ];
        injectSVG();
        

    }
}