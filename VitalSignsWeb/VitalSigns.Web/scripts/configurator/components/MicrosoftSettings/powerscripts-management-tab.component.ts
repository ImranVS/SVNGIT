import { Component, ComponentFactoryResolver, OnInit, Injector } from '@angular/core';

import { WidgetController, WidgetContainer, WidgetContract } from '../../../core/widgets';
import { WidgetService } from '../../../core/widgets/services/widget.service';

import { ServiceTab } from '../../../services/models/service-tab.interface';


import { ActivatedRoute } from '@angular/router';

declare var injectSVG: any;

@Component({
    templateUrl: '/app/configurator/components/MicrosoftSettings/powerscripts-management-tab.component.html',
    providers: [WidgetService]
})

export class PowerScriptsManagementTab extends WidgetController implements OnInit, ServiceTab {

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


        this.widgets = [
            {
                id: 'powerScriptsRoleManagement',
                name: 'PowerScriptsRoleManagement',
                css: 'col-xs-12 col-sm-12 col-md-12 col-lg-12',
            }
        ];
        injectSVG();
    }


}