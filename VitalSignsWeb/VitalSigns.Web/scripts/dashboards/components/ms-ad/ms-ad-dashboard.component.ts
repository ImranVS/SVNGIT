import {Component, ComponentResolver, OnInit} from '@angular/core';
import {ROUTER_DIRECTIVES} from '@angular/router';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../../core/widgets';
import {AppNavigator} from '../../../navigation/app.navigator.component';

declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    templateUrl: '/app/dashboards/components/ms-ad-dashboard.component.html',
    directives: [ROUTER_DIRECTIVES, WidgetContainer, AppNavigator],
    providers: [WidgetService]
})
export class MSActiveDirectoryDashboard extends WidgetController implements OnInit {

    widgets: WidgetContract[] = []
    
    constructor(protected resolver: ComponentResolver, protected widgetService: WidgetService) {
        super(resolver, widgetService);
    }

    ngOnInit() {
        injectSVG();
        bootstrapNavigator();
    }

}