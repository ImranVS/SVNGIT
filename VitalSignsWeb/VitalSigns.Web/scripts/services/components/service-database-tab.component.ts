﻿import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

import {WidgetController, WidgetContainer, WidgetContract} from '../../core/widgets';
import {WidgetService} from '../../core/widgets/services/widget.service';
import {AppNavigator} from '../../navigation/app.navigator.component';


import {ServiceTab} from '../models/service-tab.interface';

declare var injectSVG: any;


@Component({
    templateUrl: '/app/services/components/service-database-tab.component.html',
    providers: [WidgetService]
})
export class ServiceDatabaseTab extends WidgetController implements OnInit {
    deviceId: any;
    widgets: WidgetContract[]; 

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private route: ActivatedRoute) {
        super(resolver, widgetService);
    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.deviceId = params['service'];

        });
        this.widgets = [
            {
                id: 'ServiceDatabaseGrid',
                title: 'All Databases',
                name: 'ServiceDatabaseGrid',
                css: 'col-xs-12'               
            }
        ]
        injectSVG();
        

    }

}