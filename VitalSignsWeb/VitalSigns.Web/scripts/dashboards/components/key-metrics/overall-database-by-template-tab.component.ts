import {Component, ComponentFactoryResolver, OnInit, Injector} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';

import {ServiceTab} from '../../../services/models/service-tab.interface';

import {OverallDatabaseGrid} from './overall-database-grid.component';

declare var injectSVG: any;

@Component({
    selector: 'tab-databases-by-template',
    templateUrl: '/app/dashboards/components/key-metrics/overall-database-by-template-tab.component.html'
})
export class OverallDatabaseByTemplateTab extends WidgetController implements OnInit, ServiceTab {

    widgets: WidgetContract[];
    serviceId: string;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {
        super(resolver, widgetService);
    }
    
    ngOnInit() {
        this.widgetService.setProperty("exceptions", "False");
        this.widgetService.setProperty("istemplate", "True");
        this.widgets = [
            {
                id: 'byTemplateGrid',
                title: 'By Template',
                name: 'OverallDatabaseGrid',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-12'
            }
        ];
    
        injectSVG();
    }

}