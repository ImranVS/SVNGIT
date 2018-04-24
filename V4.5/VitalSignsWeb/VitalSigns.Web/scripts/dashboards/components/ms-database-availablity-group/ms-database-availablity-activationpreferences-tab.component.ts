﻿import {Component, ComponentFactoryResolver, OnInit, Injector } from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import { DAGHealthGrid } from './ms-database-availablity-group-grid.component';

import {ServiceTab} from '../../../services/models/service-tab.interface';


import {ActivatedRoute} from '@angular/router';

declare var injectSVG: any;

@Component({
    selector: 'tab-Msactivationpreferenceavailablity',
    templateUrl: '/app/dashboards/components/ms-database-availablity-group/ms-database-availablity-activationpreferences-tab.component.html'
})

export class ActivationPreferencesTab extends WidgetController implements OnInit, ServiceTab {

    widgets: WidgetContract[];
    serviceId: string;
    today: Date;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private route: ActivatedRoute) {
        super(resolver, widgetService);
    }

    
    
    ngOnInit() {
        this.route.params.subscribe(params => {
            if (params['service'])
                this.serviceId = params['service'];
            else {
                this.serviceId = this.widgetService.getProperty('serviceId');
            }
        });
        //var date = new Date();
        //var displayDate = new Date(date.getFullYear(), date.getMonth(), date.getDate()).toISOString();

        this.widgets = [
         
            {
                id: 'Database',
                name: 'ActivationPreferencesGrid',
                css: 'col-xs-12 col-sm-12 col-md-12 col-lg-12',
            }
        ];
        injectSVG();
    }


}