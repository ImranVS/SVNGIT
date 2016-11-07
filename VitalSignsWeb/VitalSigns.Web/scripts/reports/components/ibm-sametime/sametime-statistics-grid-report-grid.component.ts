import {Component, Input, Output, OnInit, EventEmitter, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent, WidgetService} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';

@Component({
    selector: 'vs-connections-stats-grid',
    templateUrl: './app/reports/components/ibm-sametime/sametime-statistics-grid-report-grid.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class SametimeStatisticGridReportGrid implements WidgetComponent, OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @Input() settings: any;

    @Output() select: EventEmitter<string> = new EventEmitter<string>();

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

        var displayDate = (new Date()).toISOString().slice(0, 10);

        this.service.get(`/reports/sametime_stats_grid`)
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                this.data.pageSize = 10;
                this.flex.rowHeaders.columns.push(new wijmo.grid.Column());
                
                //this.flex.rowHeaders.setCellData(0, 0, this.flex.columns.getColumn('server_name').dataMap.getDisplayValue(""));
                //this.flex.rowHeaders.setCellData(0, 0, this.flex.columns.getColumn('server_name').dataMap.getDisplayValue();

                //this.flex.columnHeaders.rows.push(new wijmo.grid.Row());
                //this.flex.columnHeaders.setCellData(0, 0, "Server Name");
                //this.flex.columnHeaders.setCellData(0, 1, "Status");
                //this.flex.columnHeaders.setCellData(0, 2, "Details");
                //this.flex.columnHeaders.setCellData(0, 3, "Description");
                //this.flex.columnHeaders.setCellData(0, 4, "Location");
                //this.flex.columnHeaders.setCellData(0, 5, "Mail");
                //this.flex.columnHeaders.setCellData(0, 6, "Mail");
                //this.flex.columnHeaders.setCellData(0, 7, "Mail");
                //this.flex.columnHeaders.setCellData(0, 8, "CPU");
                //this.flex.allowMerging = wijmo.grid.AllowMerging.ColumnHeaders;
            },
            (error) => this.errorMessage = <any>error
            );


    }

    itemsSourceChangedHandler() {
        this.flex.autoSizeColumns();
        console.log(this.flex.columns.getColumn('device_name').dataMap.getDisplayValues().keys)
       // this.flex.group
    }
    

}