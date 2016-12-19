import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../core/widgets';
import {AppNavigator} from '../../navigation/app.navigator.component';

declare var injectSVG: any;

@Component({
    templateUrl: '/app/dashboards/components/status-map-dashboard.component.html',
    providers: [WidgetService]
})
export class StatusMapDashboard extends WidgetController implements OnInit {

    widgets: WidgetContract[] = []
    
    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {
        super(resolver, widgetService);
    }

    ngOnInit() {
        injectSVG();
        
    }

}