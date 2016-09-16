﻿import {Component, Input, OnInit} from '@angular/core';
import {HTTP_PROVIDERS}    from '@angular/http';

import {WidgetComponent, WidgetService} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';

@Component({
    templateUrl: './app/dashboards/components/ibm-connections/ibm-connections-grid.component.html',
    directives: [
        wjFlexGrid.WjFlexGrid,
        wjFlexGrid.WjFlexGridColumn,
        wjFlexGrid.WjFlexGridCellTemplate,
        wjFlexGridFilter.WjFlexGridFilter,
        wjFlexGridGroup.WjGroupPanel,
        wjFlexInput.WjMenu,
        wjFlexInput.WjMenuItem
    ],
    providers: [
        HTTP_PROVIDERS,
        RESTService
    ]
})
export class IBMConnectionsGrid implements WidgetComponent, OnInit {
    @Input() settings: any;

    data: wijmo.collections.CollectionView;
    errorMessage: string;
    
    constructor(private service: RESTService, private widgetService: WidgetService) { }

    get pageSize(): number {
        return this.data.pageSize;
    }

    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
        }
    }

    ngOnInit() {

        this.service.get('/services/status_list?type=IBM%20Connections')
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                this.data.pageSize = 10;
            },
            (error) => this.errorMessage = <any>error
            );

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

    refreshChart(event: wijmo.grid.CellRangeEventArgs) {

        console.log(`services/summarystats?statName=[BLOGS_CREATED_LAST_DAY,COMMENT_CREATED_LAST_DAY,ENTRY_CREATED_LAST_DAY]&deviceid=${event.panel.grid.selectedItems[0].device_id}`);
        this.widgetService.refreshWidget('dailyActivities', `services/summarystats?statName=[BLOGS_CREATED_LAST_DAY,COMMENT_CREATED_LAST_DAY,ENTRY_CREATED_LAST_DAY]&deviceid=${event.panel.grid.selectedItems[0].device_id}`)
            .catch(error => console.log(error));
    }
}