import {Component, ComponentFactoryResolver, OnInit, Injector } from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {ServiceTab} from '../../../services/models/service-tab.interface';


import {ActivatedRoute} from '@angular/router';

declare var injectSVG: any;

@Component({
    selector: 'tab-MSExchangequeuestab',
    templateUrl: '/app/dashboards/components/ms-exchange/ms-exchange-queues-tab.component.html'
})

export class MSExchangequeuestab extends WidgetController implements OnInit, ServiceTab {

    widgets: WidgetContract[];
    serviceId: string;
    today: Date;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private route: ActivatedRoute) {
        super(resolver, widgetService);
    }

    
    
    ngOnInit() {

        this.widgets = [
         
            {
                id: 'MSqueues Grid',
                name: 'MSQueuesGrid',
                css: 'col-xs-12 col-sm-12 col-md-12 col-lg-12',
            }
        ];
        injectSVG();
    }


}