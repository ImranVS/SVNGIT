import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract} from '../../../core/widgets';
import {AppNavigator} from '../../../navigation/app.navigator.component';

declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    templateUrl: '/app/dashboards/components/office365-dashboard.component.html',
})
export class Office365Dashboard implements OnInit {

    ngOnInit() {
        injectSVG();
        bootstrapNavigator();
    }

}