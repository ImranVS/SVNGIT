import {Component, ComponentResolver, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

import {WidgetController, WidgetContainer, WidgetContract} from '../../core/widgets';
import {AppNavigator} from '../../navigation/app.navigator.component';


import {ServiceTab} from '../models/service-tab.interface';

declare var injectSVG: any;
declare var bootstrapNavigator: any;

@Component({
    templateUrl: '/app/services/components/service-mainhealth-tab.component.html',
    directives: [ WidgetContainer, AppNavigator]
})
export class ServiceMainHealthTab extends WidgetController implements OnInit {
    serviceId: any;
    service: any;
    
    widgets: WidgetContract[];
   
    
    constructor(protected resolver: ComponentResolver, private route: ActivatedRoute) {
        super(resolver);
    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.serviceId = params['service'];

        });
        this.widgets = [
            {
                id: 'dynamicGrid',
                title: 'Health Assessment',
                path: '/app/widgets/grid/components/dynamic-grid.component',
                name: 'DynamicGrid',
                css: 'col-xs-12',
                settings: {
                   // url: '/DashBoard/'+this.serviceId+'/health-assessment',
                    url: '/DashBoard/5786a3ddb4ec91be7589c822/health-assessment',
                    columns: [{ header: "Category", binding: "category", name: "category", width: "*" },
                        { header: "Test Name", binding: "test_name", name: "test_name", width: "*" },
                        { header: "Result", binding: "result", name: "result", width: "*" },
                        { header: "Details", binding: "details", name: "details", width: "*" },
                        { header: "Last Scan", binding: "last_scan", name: "last_scan", width: "*" }
                    ]

                }
            }

        ]
        //alert(this.serviceId);
        injectSVG();
        bootstrapNavigator();
    }

}