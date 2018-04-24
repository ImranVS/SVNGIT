import {Component, ComponentFactoryResolver, OnInit, ViewChild} from '@angular/core';

import { WidgetService } from '../../../core/widgets/services/widget.service';
import {AppNavigator} from '../../../navigation/app.navigator.component';

import {IBMSametimeGrid} from './ibm-sametime-grid.component';
import {IBMSametimeDetails} from './ibm-sametime-details.component';

declare var injectSVG: any;

@Component({
    templateUrl: '/app/dashboards/components/ibm-sametime/ibm-sametime-dashboard.component.html',
    providers: [WidgetService]
})
export class IBMSametimeDashboard implements OnInit {

    serviceId: string;

    ngOnInit() {

        injectSVG();
        

    }

    onSelect(serviceId: string) {

        this.serviceId = serviceId;

    }
    
}