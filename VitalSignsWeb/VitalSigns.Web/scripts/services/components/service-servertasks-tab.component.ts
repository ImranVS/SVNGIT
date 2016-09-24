import {Component, ComponentResolver, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../core/widgets';
import {AppNavigator} from '../../navigation/app.navigator.component';


import {ServiceTab} from '../models/service-tab.interface';

declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    templateUrl: '/app/services/components/service-servertasks-tab.component.html',
    directives: [WidgetContainer, AppNavigator],
    providers: [WidgetService]
})
export class ServiceTasksTab extends WidgetController implements OnInit {
    deviceId: any;
    widgets: WidgetContract[]; 

    constructor(protected resolver: ComponentResolver, protected widgetService: WidgetService, private route: ActivatedRoute) {
        super(resolver, widgetService);
    }

    ngOnInit() {
        
        this.route.params.subscribe(params => {
            this.deviceId = params['service'];
            
        });
       
       

        this.widgets = [
            {
                id: 'ServiceServerTasksGrid',
                title: 'Monitored Tasks',
                path: '/app/services/components/service-servertasks-grid.component',
                name: 'ServiceServerTasksGrid',
                css: 'col-xs-12'
            },
            {
                id: 'ServiceNMServerTasksGrid',
                title: 'Non Monitored Tasks',
                path: '/app/services/components/service-nm-servertasks-grid.component',
                name: 'ServiceNMServerTasksGrid',
                css: 'col-xs-12'
            }
           
        ]

           
        injectSVG();
        bootstrapNavigator();

    }

}



