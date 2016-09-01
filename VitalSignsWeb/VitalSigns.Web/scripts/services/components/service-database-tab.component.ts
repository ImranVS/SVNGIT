import {Component, ComponentResolver, OnInit} from '@angular/core';
import {ROUTER_DIRECTIVES} from '@angular/router';

import {WidgetController, WidgetContainer, WidgetContract} from '../../core/widgets';
import {AppNavigator} from '../../navigation/app.navigator.component';


import {ServiceTab} from '../models/service-tab.interface';

declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    templateUrl: '/app/services/components/service-database-tab.component.html',
    directives: [ROUTER_DIRECTIVES, WidgetContainer, AppNavigator]
})
export class ServiceDatabaseTab extends WidgetController implements OnInit {

    widgets: WidgetContract[] = [
        {
            id: 'sampleGrid',
            title: 'All Databases',
            path: '/app/widgets/grid/components/sample-grid.component',
            name: 'SampleGrid',
            css: 'col-xs-12',
            settings: {
                url: 'http://localhost:1234/DashBoard/57af28415c6c6c02d4fce747/database'

            }
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