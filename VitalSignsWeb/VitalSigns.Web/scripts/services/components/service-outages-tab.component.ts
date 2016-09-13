import {Component, ComponentResolver, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../core/widgets';
import {AppNavigator} from '../../navigation/app.navigator.component';


import {ServiceTab} from '../models/service-tab.interface';

declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    templateUrl: '/app/services/components/service-outages-tab.component.html',
    directives: [WidgetContainer, AppNavigator],
    providers: [WidgetService]
})
export class ServiceOutagesTab extends WidgetController implements OnInit {
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
                title: '',
                path: '/app/widgets/grid/components/dynamic-grid.component',
                name: 'DynamicGrid',
                css: 'col-xs-12',
                settings: {
                    url: '/DashBoard/' + this.deviceId + '/outages',
                    //url: '/DashBoard/57a3caeb5c6c6c28b8eed352/outages',
                    columns: [
                        { header: "DeviceName", binding: "device_name", name: "device_name", width: "*" },
                        { header: "DateTimeDown", binding: "date_time_down", name: "date_time_down", width: "*" },
                        { header: "DateTimeUp", binding: "date_time_up", name: "date_time_up", width: "*" }
                       
                    ]

                }
            }

        ]
        injectSVG();
        bootstrapNavigator();

    }

}


