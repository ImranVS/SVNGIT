import {Component, ComponentFactoryResolver, OnInit, Injector} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract, WidgetService} from '../../../core/widgets';

import {ServiceTab} from '../../../services/models/service-tab.interface';

import {OverallDatabaseGrid} from './overall-database-grid.component';

declare var injectSVG: any;

@Component({
    selector: 'tab-problems',
    templateUrl: '/app/dashboards/components/key-metrics/overall-database-problems-tab.component.html'
})
export class OverallDatabaseProblemsTab extends WidgetController implements OnInit, ServiceTab {

    widgets: WidgetContract[];
    serviceId: string;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {
        super(resolver, widgetService);
    }
    
    ngOnInit() {
        this.widgetService.setProperty("exceptions", "True");
        this.widgetService.setProperty("istemplate", "False");

        this.widgets = [
            {
                id: 'problemsGrid',
                title: 'Databases with Problems',
                name: 'OverallDatabaseGrid',
                css: 'col-xs-12 col-sm-6 col-md-6 col-lg-12'
            }
        ];
    
        injectSVG();
    }

}