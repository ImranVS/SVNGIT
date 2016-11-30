import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {AlertService} from '../../core/services/alert.service';

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

    status: string;

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

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private route: ActivatedRoute, private alerts: AlertService) {

        super(resolver, widgetService);

    }

    ngOnInit() {
        
        injectSVG();
        bootstrapNavigator();

    }

    showAlert() {

        this.alerts.showAlertMessage('warning', 'Hello World!');

    }

}