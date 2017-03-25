import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

import {WidgetController, WidgetContainer, WidgetContract} from '../../core/widgets';
import {WidgetService} from '../../core/widgets/services/widget.service';
import {AppNavigator} from '../../navigation/app.navigator.component';


import {ServiceTab} from '../models/service-tab.interface';

declare var injectSVG: any;


@Component({
    templateUrl: '/app/services/components/service-events-tab.component.html',
    //providers: [WidgetService]
})
export class ServiceEventsTab extends WidgetController implements OnInit {
    deviceId: any;
    serviceId: string;
    widgets: WidgetContract[];

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private route: ActivatedRoute) {
        super(resolver, widgetService);
    }

    ngOnInit() {
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
                id: 'ServiceEventsGrid',
                title: '',
                name: 'ServiceEventsGrid',
                css: 'col-xs-12',
            }

        ]
        injectSVG();
        

    }

}


