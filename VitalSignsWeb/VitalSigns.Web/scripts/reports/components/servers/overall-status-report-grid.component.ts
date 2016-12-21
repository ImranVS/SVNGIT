import {Component, Input, Output, OnInit, EventEmitter, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';

@Component({
    selector: 'vs-connections-stats-grid',
    templateUrl: './app/reports/components/servers/overall-status-report-grid.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class OverallStatusReportGrid implements WidgetComponent, OnInit {
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

        this.service.get(`/services/status_list`)
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                this.data.pageSize = 10;
                this.data.groupDescriptions.push(new wijmo.collections.PropertyGroupDescription("device_type"));
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
       // this.flex.group
    }
    

}