import {Component, Input, Output, OnInit, EventEmitter, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as gridHelpers from '../../../core/services/helpers/gridutils';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as helpers from '../../../core/services/helpers/helpers';

@Component({
    selector: 'vs-ms-database-availablity-group-grid',
    templateUrl: './app/dashboards/components/ms-database-availablity-group/ms-database-availablity-group-grid.component.html',
    providers: [
        HttpModule,
        RESTService,
        helpers.GridTooltip,
        gridHelpers.CommonUtils
    ]
})
export class DAGHealthGrid implements WidgetComponent, OnInit {
    @Input() settings: any;
    @Output() select: EventEmitter<string> = new EventEmitter<string>();
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;  
    data: wijmo.collections.CollectionView;
    errorMessage: string;
    currentPageSize: any = 20;
    
    get serviceId(): string {

        return this.widgetService.getProperty('serviceId');

    }

    set serviceId(id: string) {

        this.widgetService.setProperty('serviceId', id);

        this.select.emit(this.widgetService.getProperty('serviceId'));

    }


    constructor(private service: RESTService, private widgetService: WidgetService, protected toolTip: helpers.GridTooltip, protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) { }

    

    ngOnInit() {
        this.service.get('/services/status_list?type=Database Availability Group')
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                this.data.pageSize = this.currentPageSize;
                this.data.moveCurrentToPosition(0);
                this.serviceId = this.data.currentItem.device_id;
            },
            (error) => this.errorMessage = <any>error
            );
 
    }

    

    onSelectionChanged(event: wijmo.grid.CellRangeEventArgs) {
        this.serviceId = event.panel.grid.selectedItems[0].device_id;
    }
    
}