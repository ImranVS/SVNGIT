import {Component, ComponentFactoryResolver, OnInit, OnDestroy} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

import 'rxjs/Rx';

import {WidgetController, WidgetContainer, WidgetContract} from '../../core/widgets';
import {WidgetService} from '../../core/widgets/services/widget.service';
import {AppNavigator} from '../../navigation/app.navigator.component';

declare var injectSVG: any;


@Component({
    selector: 'traveler-dashboard',
    templateUrl: '/app/dashboards/components/overall-dashboard.component.html',
    providers: [WidgetService]
})
export class OverallDashboard extends WidgetController implements OnInit {

    status: string;
    timer: any;

    widgetOnPremisesApps: WidgetContract = {
        id: 'widgetOnPremisesApps',
        title: 'All Applications and Servers',
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

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private route: ActivatedRoute) {

        super(resolver, widgetService);

    }

    ngOnInit() {
        
        injectSVG();
        this.timer = window.setInterval(() => {
            this.refreshdata();
        }, 30000);
        
    }

    ngOnDestroy() {
        clearInterval(this.timer);
    }

    refreshdata() {
        this.widgetService.refreshWidget('widgetOnPremisesApps')
            .catch(error => console.log(error));
        this.widgetService.refreshWidget('widgetStatusSummary')
            .catch(error => console.log(error));
        this.widgetService.refreshWidget('widgetUsersSessions')
            .catch(error => console.log(error));

    }

}