import {Component, ComponentFactoryResolver, OnInit, Injector } from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import {ServiceTab} from '../../../services/models/service-tab.interface';


import {ActivatedRoute} from '@angular/router';

declare var injectSVG: any;

@Component({
    templateUrl: '/app/configurator/components/MicrosoftSettings/exchange-mail-probe-tab.component.html'
})

export class ExchangeMailProbeTab extends WidgetController implements OnInit, ServiceTab {

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
                id: 'MailProbeGrid',
                name: 'ExchangeMailProbeGrid',
                css: 'col-xs-12 col-sm-12 col-md-12 col-lg-12',
            }
        ];
        injectSVG();
    }


}