import {Component, ComponentFactoryResolver, Input, OnInit} from '@angular/core';

import {WidgetController, WidgetContainer, WidgetContract} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {AppNavigator} from '../../../navigation/app.navigator.component';
declare var injectSVG: any;


@Component({
    selector: 'database-replication-health',
    templateUrl: '/app/dashboards/components/key-metrics/database-replication-health.component.html',
    providers: [WidgetService]
})
export class DatabaseReplicationHealth extends WidgetController implements OnInit {
    widgets: WidgetContract[];
    serviceId: string;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService) {
        super(resolver, widgetService);
    }

    ngOnInit() {
        this.serviceId = this.widgetService.getProperty('serviceId');
        this.widgets = [
            {
                id: 'databaseReplicationGrid',
                title: '',
                name: 'DatabaseReplicationGrid',
                css: 'col-xs-12 col-sm-12  col-md-12 col-lg-12',
                settings: {

                }
            },
            {
                id: 'databaseProblemsGrid',
                title: 'Potential Replication Problems',
                name: 'DatabaseProblemsGrid',
                css: 'col-xs-12 col-sm-12  col-md-12 col-lg-12',
                settings: {

                }
            }
        ];
        injectSVG();
        
    }

    onSelect(serviceId: string) {

        this.serviceId = serviceId;

    }
}