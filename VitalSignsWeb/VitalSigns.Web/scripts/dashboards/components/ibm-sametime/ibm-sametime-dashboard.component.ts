import {Component, ComponentResolver, OnInit} from '@angular/core';
import {ROUTER_DIRECTIVES} from '@angular/router';

import {WidgetController, WidgetContainer, WidgetContract} from '../../../core/widgets';
import {AppNavigator} from '../../../navigation/app.navigator.component';
import {ServiceTab} from '../../../services/models/service-tab.interface';

declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    templateUrl: '/app/dashboards/components/ibm-sametime-dashboard.component.html',
    directives: [ROUTER_DIRECTIVES, WidgetContainer, AppNavigator]
})
export class IBMSametimeDashboard extends WidgetController implements OnInit {

    widgets: WidgetContract[] = [
        {
            id: 'sampleGrid',
            title: 'Sametime Info',
            path: '/app/widgets/grid/components/sample-grid.component',
            name: 'SampleGrid',
            css: 'col-xs-12',
            settings: {}
        },
        {
            id: 'sametimeDetails',
            title: '',
            path: '/app/dashboards/components/ibm-sametime/ibm-sametime-details.component',
            name: 'IBMSametimeDetails',
            css: 'col-xs-12 col-sm-12 col-md-12 col-lg-12',

            settings: {}
        }
    ]
    
    constructor(protected resolver: ComponentResolver) {
        super(resolver);
    }

    ngOnInit() {
        injectSVG();
        bootstrapNavigator();
    }

}