﻿import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as helpers from '../../../core/services/helpers/helpers';

@Component({
    templateUrl: './app/dashboards/components/ibm-domino/ibm-domino-grid.component.html',
    providers: [
        HttpModule,
        RESTService,
        helpers.GridTooltip
    ]
})
export class IBMDominoGrid implements WidgetComponent, OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @Input() settings: any;

    data: wijmo.collections.CollectionView;
    errorMessage: string;

    constructor(private service: RESTService, protected toolTip: helpers.GridTooltip) { }

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

        this.service.get('/services/status_list?type=Domino')
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                this.data.pageSize = 20;
            },
            (error) => this.errorMessage = <any>error
        );
        //this.flex.autoSizeRow(0);

        // Create custom tooltip
        this.toolTip.getTooltip(this.flex, 0, 8);
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
    
    collapse(flex) {
        flex.collapseGroupsToLevel(0);
    }

    expand(flex) {
        var rows = flex.rows;
        for (var rowIdx = 0; rowIdx < rows.length; rowIdx++) {
            var rootRow = rows[rowIdx];
            if (rootRow.hasChildren) { rootRow.isCollapsed = false; }
        }
    }

    onItemsSourceChanged() {
        var row = this.flex.columnHeaders.rows[0];
        row.wordWrap = true;
        // autosize first header row
        this.flex.autoSizeRow(0, true);

    }

}