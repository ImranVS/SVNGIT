import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';

import 'rxjs/Rx';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../core/widgets';
import {AppNavigator} from '../../navigation/app.navigator.component';

declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    selector: 'traveler-dashboard',
    templateUrl: '/app/dashboards/components/overall-dashboard.component.html',
    providers: [WidgetService]
})
export class OverallDashboard extends WidgetController implements OnInit {

    widgetAppsStatus: WidgetContract[] = [
        {
            id: 'widgetOnPremisesApps1',
            name: 'AppStatus',
            settings: {
                serviceId: 1
            }
        },
        {
            id: 'widgetOnPremisesApps2',
            name: 'AppStatus',
            settings: {
                serviceId: 2
            }
        },
        {
            id: 'widgetOnPremisesApps3',
            name: 'AppStatus',
            settings: {
                serviceId: 3
            }
        },
        {
            id: 'widgetOnPremisesApps4',
            name: 'AppStatus',
            settings: {
                serviceId: 4
            }
        }
    ]

    widgetOnPremisesApps: WidgetContract = {
        id: 'widgetOnPremisesApps',
        title: 'On premises applications',
        name: 'OnPremisesApps',
        css: 'col-md-6 col-lg-12',
        settings: {}
    }

    widgetStatusSummary: WidgetContract = {
        id: 'widgetStatusSummary',
        name: 'StatusSummary',
        settings: {}
    }

    widgetUsersSessions: WidgetContract =
    {
        id: 'widgetUsersSessions',
        name: 'UserSessions',
        settings: {}
    }

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {

        super(resolver, widgetService);

    }

    ngOnInit() {

        injectSVG();
        bootstrapNavigator();

    }

}