import {Component, ComponentResolver, OnInit} from '@angular/core';
import {ROUTER_DIRECTIVES} from '@angular/router';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../../core/widgets';
import {AppNavigator} from '../../../navigation/app.navigator.component';
import {ServiceTab} from '../../../services/models/service-tab.interface';

declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    templateUrl: '/app/dashboards/components/ibm-connections/ibm-connections-dashboard.component.html',
    directives: [ROUTER_DIRECTIVES, WidgetContainer, AppNavigator],
    providers: [WidgetService]
})
export class IBMConnectionsDashboard extends WidgetController implements OnInit {

    widgets: WidgetContract[] = [
        {
            id: 'connectionsGrid',
            title: 'Connections Info',
            path: '/app/dashboards/components/ibm-connections/ibm-connections-grid.component',
            name: 'IBMConnectionsGrid',
            css: 'col-xs-12',
            settings: {}
        },
        {
            id: 'connectionsDetails',
            title: null,
            path: '/app/dashboards/components/ibm-connections/ibm-connections-details.component',
            name: 'IBMConnectionsDetails',
            css: 'col-xs-12 col-sm-12 col-md-12 col-lg-12',
            settings: {

            }
        }
    ]
    
    constructor(protected resolver: ComponentResolver, protected widgetService: WidgetService) {
        super(resolver, widgetService);
    }

    ngOnInit() {
        injectSVG();
        bootstrapNavigator();
    }

}