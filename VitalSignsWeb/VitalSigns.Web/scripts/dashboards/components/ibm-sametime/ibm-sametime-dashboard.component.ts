import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import {WidgetService} from '../../../core/widgets';
import {AppNavigator} from '../../../navigation/app.navigator.component';

import {IBMSametimeGrid} from './ibm-sametime-grid.component';
import {IBMSametimeDetails} from './ibm-sametime-details.component';

declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    templateUrl: '/app/dashboards/components/ibm-sametime/ibm-sametime-dashboard.component.html',
    providers: [WidgetService]
})
export class IBMSametimeDashboard implements OnInit {

    ngOnInit() {
        injectSVG();
        bootstrapNavigator();
    }

}