import { Component, ComponentFactoryResolver, OnInit } from '@angular/core';
import { WidgetService } from '../../../core/widgets/services/widget.service';
import { WidgetController, WidgetContract } from '../../../core/widgets';
import { RESTService } from '../../../core/services/rest.service';
import * as helpers from '../../../core/services/helpers/helpers';
declare var Highcharts: any;

declare var injectSVG: any;

@Component({
    templateUrl: '/app/reports/components/disk/disk-space-consumption.component.html',
    providers: [
        WidgetService,
        RESTService,
        helpers.UrlHelperService
    ]
})

export class DiskSpaceConsumptionReport extends WidgetController implements OnInit {
    contextMenuSiteMap: any;
    widgets: WidgetContract[];
    currentHideServerControl: boolean = false;
    currentHideDatePanel: boolean = true;
    currentshowdiskdropdown: boolean = true;
    currentHideSingleDTControl: boolean = true;
    currentDeviceType: string = "Domino";
    currentWidgetName: string = `DiskConsumption`;
    currentWidgetURL: string = `/dashboard/overall/disk-space`;
   
  

    

    constructor(protected resolver: ComponentFactoryResolver, private service: RESTService, protected widgetService: WidgetService,
        protected urlHelpers: helpers.UrlHelperService) {
        super(resolver, widgetService);
    }

    ngOnInit() {
        this.service.get('/navigation/sitemaps/disk_reports')
            .subscribe
            (
            data => this.contextMenuSiteMap = data,
            error => console.log(error)
            );
        this.widgets = [

            {
                id: 'DiskConsumption',
                name: 'DiskSpaceWidgetReport',

            }
        ];
        injectSVG();
        

    }
}