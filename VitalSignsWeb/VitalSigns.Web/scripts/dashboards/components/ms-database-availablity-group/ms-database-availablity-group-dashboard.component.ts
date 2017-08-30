import {Component, ComponentFactoryResolver, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';
import { WidgetService } from '../../../core/widgets/services/widget.service';
import {WidgetController, WidgetContainer, WidgetContract} from '../../../core/widgets';
import {AppNavigator} from '../../../navigation/app.navigator.component';
import {RESTService} from '../../../core/services/rest.service';
import { DAGHealthGrid } from './ms-database-availablity-group-grid.component';
import { DAGDetails } from './ms-database-availablity-group-details.component';
import { DatabaseStatusGrid } from './ms-database-availablity-group-DatabaseStatus-grid.component';
import { MembersGrid } from './ms-database-availablity-group-members-grid.component';
import { DatabaseGrid } from './ms-database-availablity-group-databases-grid.component';


declare var injectSVG: any;


@Component({
    templateUrl: '/app/dashboards/components/ms-database-availablity-group/ms-database-availablity-group-dashboard.component.html',
    providers: [
        HttpModule,
        RESTService,
        WidgetService
    ]
})
export class DAGDashboard implements OnInit {

    serviceId: string;

    ngOnInit() {
        injectSVG();
    }

    onSelect(serviceId: string) {

        this.serviceId = serviceId;

    }
}