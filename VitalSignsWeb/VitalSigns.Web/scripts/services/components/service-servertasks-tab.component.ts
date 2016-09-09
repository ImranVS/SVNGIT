import {Component, ComponentResolver, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

import {WidgetController, WidgetContainer, WidgetContract} from '../../core/widgets';
import {AppNavigator} from '../../navigation/app.navigator.component';


import {ServiceTab} from '../models/service-tab.interface';

declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    templateUrl: '/app/services/components/service-servertasks-tab.component.html',
    directives: [WidgetContainer, AppNavigator]
})
export class ServiceTasksTab extends WidgetController implements OnInit {
    deviceId: any;
    widgets: WidgetContract[]; 

    constructor(protected resolver: ComponentResolver, private route: ActivatedRoute) {
        super(resolver);
    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.deviceId = params['service'];

        });
        this.widgets = [
            {
                id: 'dynamicGrid',
                title: 'Monitered Tasks',
                path: '/app/widgets/grid/components/dynamic-grid.component',
                name: 'DynamicGrid',
                css: 'col-xs-12',
                settings: {
                    //url: '/DashBoard/' + this.deviceId +'/monitored_tasks',
                    url: '/DashBoard/57af28415c6c6c02d4fce747/monitored_tasks',
                    columns: [
                        { header: "TaskName", binding: "task_name", name: "task_name", width: "*" },
                        { header: "PrimaryStatus", binding: "primary_status", name: "primary_status", width: "*" },
                        { header: "SecondaryStatus", binding: "secondary_status", name: "secondary_status", width: "*" },
                        { header: "LastUpdated", binding: "last_updated", name: "last_updated", width: "*" }
                        
                    ]

                }
            },

            {
                id: 'dynamicGrid',
                title: 'All Databases',
                path: '/app/widgets/grid/components/dynamic-grid.component',
                name: 'DynamicGrid',
                css: 'col-xs-12',
                settings: {
                    url: '/DashBoard/' + this.deviceId + '/database',
                    //url: '/DashBoard/57af28415c6c6c02d4fce747/database',
                    columns: [
                        { header: "TaskName", binding: "task_name", name: "task_name", width: "*" },
                        { header: "PrimaryStatus", binding: "primary_status", name: "primary_status", width: "*" },
                        { header: "SecondaryStatus", binding: "secondary_status", name: "secondary_status", width: "*" },
                        { header: "LastUpdated", binding: "last_updated", name: "last_updated", width: "*" },
                        { header: "Monitored", binding: "monitored", name: "monitored", width: "*" }

                    ]

                }
            }


        ]
        injectSVG();
        bootstrapNavigator();

    }

}



