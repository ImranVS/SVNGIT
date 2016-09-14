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
                id: 'dynamicGrid',
                title: 'Monitored Tasks',
                path: '/app/widgets/grid/components/dynamic-grid.component',
                name: 'DynamicGrid',
                css: 'col-xs-12',
                settings: {
                    url: '/DashBoard/' + this.deviceId +'/monitoredtasks',
                   // url: '/DashBoard/57ace45abf46711cd4681e24/monitoredtasks',
                    columns: [
                        { header: "TaskName", binding: "task_name", name: "task_name", width: "*" },
                        { header: "Monitored", binding: "monitored", name: "monitored", width: "*" },
                        { header: "PrimaryStatus", binding: "primary_status", name: "primary_status", width: "*" },
                        { header: "SecondaryStatus", binding: "secondary_status", name: "secondary_status", width: "*" }
                        //{ header: "LastUpdated", binding: "last_updated", name: "last_updated", width: "*" }
                        
                    ]

                }
            }

            //{
            //    id: 'dynamicGrid',
            //    title: 'Non Monitored Tasks',
            //    path: '/app/widgets/grid/components/dynamic-grid.component',
            //    name: 'DynamicGrid',
            //    css: 'col-xs-12',
            //    settings: {
            //        //url: '/DashBoard/' + this.deviceId + '/database',
            //        url: '/DashBoard/57ace45abf46711cd4681e24/monitoredtasks',
            //        columns: [
            //            { header: "TaskName", binding: "task_name", name: "task_name", width: "*" },
            //            { header: "PrimaryStatus", binding: "primary_status", name: "primary_status", width: "*" },
            //            { header: "SecondaryStatus", binding: "secondary_status", name: "secondary_status", width: "*" },
            //            { header: "LastUpdated", binding: "last_updated", name: "last_updated", width: "*" },
            //            { header: "Monitored", binding: "monitored", name: "monitored", width: "*" }

            //        ]

            //    }
            //}


        ]
        injectSVG();
        bootstrapNavigator();

    }

}



