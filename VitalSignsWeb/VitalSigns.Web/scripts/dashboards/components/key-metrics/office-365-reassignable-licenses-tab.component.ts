import {Component, ComponentFactoryResolver, OnInit, Injector} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import {ServiceTab} from '../../../services/models/service-tab.interface';

//import {IBMConnectionsGrid} from './ibm-connections-grid.component';
import {ActivatedRoute} from '@angular/router';

declare var injectSVG: any;

@Component({
    templateUrl: '/app/dashboards/components/key-metrics/office-365-reassignable-licenses-tab.component.html',
    providers: [WidgetService]
})
export class Office365ReassignableLicensesTab extends WidgetController implements OnInit, ServiceTab {

    widgets: WidgetContract[];
    serviceId: string;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private route: ActivatedRoute) {
        super(resolver, widgetService);
    }
    
    ngOnInit() {
        var date = new Date();
        var displayDate = (new Date(date.getFullYear(), date.getMonth(), date.getDate())).toISOString();
        this.widgets = [
            {
                id: 'reassignableLicensesGrid',
                title: 'Licenses Eligible for Reassignment',
                name: 'Office365ReassignableLicensesGrid',
                css: 'col-xs-12'
            }
        ]
    
        injectSVG();
    }

}