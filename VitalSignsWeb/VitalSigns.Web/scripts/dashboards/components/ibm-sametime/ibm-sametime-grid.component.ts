﻿import {Component, Input, Output, OnInit, EventEmitter, ViewChild} from '@angular/core';
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
    selector: 'vs-sametime-grid',
    templateUrl: './app/dashboards/components/ibm-sametime/ibm-sametime-grid.component.html',
    providers: [
        HttpModule,
        RESTService,
        helpers.GridTooltip,
        gridHelpers.CommonUtils

    ]
})
export class IBMSametimeGrid implements WidgetComponent, OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @Input() settings: any;

    @Output() select: EventEmitter<string> = new EventEmitter<string>();
    
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

    constructor(private service: RESTService, private widgetService: WidgetService, protected toolTip: helpers.GridTooltip,
    protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) { }


    get pageSize(): number {
        return this.data.pageSize;
    }

    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("IBMSametimeGrid", this.authService.CurrentUser.email),
                value: value
            };

            this.service.put(`/services/set_name_value`, obj)
                .subscribe(
                (data) => {

                },
                (error) => console.log(error)
                );
        }
    }

    ngOnInit() { 
        this.service.get('/services/status_list?type=Sametime&isenabled=true')
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                this.data.pageSize = this.currentPageSize;
                this.data.moveCurrentToPosition(0);
                this.serviceId = this.data.currentItem.device_id;
            },
            (error) => this.errorMessage = <any>error
            );
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("IBMSametimeGrid", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            );
        this.toolTip.getTooltip(this.flex, 0, 2);
    }

    onSelectionChanged(event: wijmo.grid.CellRangeEventArgs) {

        this.serviceId = event.panel.grid.selectedItems[0].device_id;

    }

    getAccessColor(access: string) {

        switch (access) {
            case 'Allow':
                return 'green';
            case 'Blocked':
                return 'red';
            default:
                return '';
        }

    }
    
}