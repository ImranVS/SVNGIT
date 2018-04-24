import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';
import { WidgetService } from '../../../core/widgets/services/widget.service';
import {WidgetController, WidgetContainer, WidgetContract} from '../../../core/widgets';
import {AppNavigator} from '../../../navigation/app.navigator.component';
import {RESTService} from '../../../core/services/rest.service';

import { Office365Grid } from './office365-grid.component';
import { Office365Details } from './office365-details.component';

declare var injectSVG: any;


@Component({
    templateUrl: '/app/dashboards/components/office365/office365-dashboard.component.html',
    providers: [
        HttpModule,
        RESTService,
        WidgetService
    ]
})
export class Office365Dashboard implements OnInit {

    serviceId: string;

    ngOnInit() {
        injectSVG();
    }

    onSelect(serviceId: string) {

        this.serviceId = serviceId;

    }
}