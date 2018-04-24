import {Component, ComponentFactoryResolver, OnInit, ViewChild} from '@angular/core';

import { WidgetService } from '../../../core/widgets/services/widget.service';
import {AppNavigator} from '../../../navigation/app.navigator.component';

import {OverallDatabaseDetails} from './overall-database-details.component';

declare var injectSVG: any;

@Component({
    templateUrl: '/app/dashboards/components/key-metrics/overall-database-dashboard.component.html',
    providers: [WidgetService]
})
export class OverallDatabaseDashboard implements OnInit {

    serviceId: string;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) { }

    ngOnInit() {
        this.widgetService.setProperty("ismailpage", "False");
        this.widgetService.setProperty("exceptions", "False");
        this.widgetService.setProperty("istemplate", "False");

        injectSVG();
        
    }

    onSelect(serviceId: string) {

        this.serviceId = serviceId;

    }

}