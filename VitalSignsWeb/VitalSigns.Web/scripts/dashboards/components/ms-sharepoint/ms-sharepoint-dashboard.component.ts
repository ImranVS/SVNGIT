﻿import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {AppNavigator} from '../../../navigation/app.navigator.component';

declare var injectSVG: any;


@Component({
    templateUrl: '/app/dashboards/components/ms-sharepoint-dashboard.component.html',
    providers: [WidgetService]
})
export class MSSharePointDashboard extends WidgetController implements OnInit {

    widgets: WidgetContract[] = []
    
    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {
        super(resolver, widgetService);
    }

    ngOnInit() {
        injectSVG();
        
    }

}