import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

import {WidgetController, WidgetContainer, WidgetContract} from '../../core/widgets';
import {WidgetService} from '../../core/widgets/services/widget.service';
import {AppNavigator} from '../../navigation/app.navigator.component';

import { Office365Grid } from '../../dashboards/components/office365/office365-grid.component';
import {ServiceTab} from '../models/service-tab.interface';

declare var injectSVG: any;


@Component({
    templateUrl: '/app/services/components/service-mainhealth-tab.component.html'
    //providers: [WidgetService]
})
export class ServiceMainHealthTab extends WidgetController implements OnInit {
    deviceId: any;
    service: any;
    serviceId: string;
    widgets: WidgetContract[];
   
    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private route: ActivatedRoute) {
        super(resolver, widgetService);
    }

    ngOnInit() {
        //this.serviceId = this.widgetService.getProperty('serviceId');
        this.route.params.subscribe(params => {
            if (params['service'])
                this.deviceId = params['service'];
            else {
                if (this.serviceId) {
                    var res = this.serviceId.split(';');
                    this.deviceId = res[0];
                }
            }
        });
        this.widgets = [
            {
                id: 'ServiceMainHealthGrid',
                title: '',
                name: 'ServiceMainHealthGrid',
                css: 'col-xs-12',
            }
        ]
        injectSVG();
        
    }

}