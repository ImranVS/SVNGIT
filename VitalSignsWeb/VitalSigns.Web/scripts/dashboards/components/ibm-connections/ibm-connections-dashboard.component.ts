﻿import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import {WidgetService} from '../../../core/widgets';
import {AppNavigator} from '../../../navigation/app.navigator.component';

import {IBMConnectionsGrid} from './ibm-connections-grid.component';
import {IBMConnectionsDetails} from './ibm-connections-details.component';

declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    templateUrl: '/app/dashboards/components/ibm-connections/ibm-connections-dashboard.component.html',
    providers: [WidgetService]
})
export class IBMConnectionsDashboard implements OnInit {

    ngOnInit() {
        injectSVG();
        bootstrapNavigator();
    }

}