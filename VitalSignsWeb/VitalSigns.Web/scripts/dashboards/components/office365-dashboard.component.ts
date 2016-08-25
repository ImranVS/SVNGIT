import {Component, ComponentResolver, OnInit} from '@angular/core';
import {ROUTER_DIRECTIVES} from '@angular/router';

import {WidgetController, WidgetContainer, WidgetContract} from '../../core/widgets';
import {AppNavigator} from '../../navigation/app.navigator.component';

declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    templateUrl: '/app/dashboards/components/office365-dashboard.component.html',
    directives: [ROUTER_DIRECTIVES, AppNavigator]
})
export class Office365Dashboard implements OnInit {

    ngOnInit() {
        injectSVG();
        bootstrapNavigator();
    }

}