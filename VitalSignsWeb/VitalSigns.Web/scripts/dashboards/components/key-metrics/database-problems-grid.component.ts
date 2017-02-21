import { Component, Input, OnInit, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';
import { WidgetService } from '../../../core/widgets/services/widget.service';
import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as helpers from '../../../core/services/helpers/helpers';

@Component({
    selector: 'vs-db-rep-problems-grid',
    templateUrl: './app/dashboards/components/key-metrics/database-problems-grid.component.html',
    providers: [
        HttpModule,
        RESTService,
        helpers.GridTooltip,
        helpers.DateTimeHelper
    ]
})
export class DatabaseProblemsGrid implements WidgetComponent, OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @Input() settings: any;
    data: wijmo.collections.CollectionView;
    errorMessage: string;
    customColumnHeader: boolean = true;
    serviceId: string;

    constructor(private service: RESTService, private widgetService: WidgetService, protected toolTip: helpers.GridTooltip,
        protected datetimeHelpers: helpers.DateTimeHelper) { }

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
        this.serviceId = this.widgetService.getProperty('serviceId');
        this.service.get(`/dashboard/database-problems?clusterId=${this.serviceId}`)
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDateTime(data.data)));
                this.data.pageSize = 10;
                this.flex.columns[2].header = this.data.items[0].domino_server_a + ' (document count)';
                this.flex.columns[3].header = this.data.items[0].domino_server_b + ' (document count)';
                if (this.data.items[0].domino_server_c == null || this.data.items[0].domino_server_c == "") {
                    var cols = this.flex.columns;
                    cols[4].visible = false;
                    cols[7].visible = false;
                }
                else {
                    this.flex.columns[4].header = this.data.items[0].domino_server_c + ' (document count)';
                    this.flex.columns[7].header = this.data.items[0].domino_server_c + ' (database size)';
                }
                this.flex.columns[5].header = this.data.items[0].domino_server_a + ' (database size)';
                this.flex.columns[6].header = this.data.items[0].domino_server_b + ' (database size)';
            },
            (error) => this.errorMessage = <any>error
        );
        this.toolTip.getTooltip(this.flex, 0, 3);
    }

    onItemsSourceChanged() {

        var row = this.flex.columnHeaders.rows[0];
        row.wordWrap = true;

        // autosize first header row
        this.flex.autoSizeRow(0, true);

    }

    onPropertyChanged(key: string, value: any) {

        if (key === 'serviceId') {

            this.serviceId = value;

            this.service.get(`/dashboard/database-problems?clusterId=${this.serviceId}`)
                .subscribe(
                (data) => {
                    this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                    this.data.pageSize = 10;
                    this.flex.columns[2].header = this.data.items[0].domino_server_a + ' (document count)';
                    this.flex.columns[3].header = this.data.items[0].domino_server_b + ' (document count)';
                    if (this.data.items[0].domino_server_c == null || this.data.items[0].domino_server_c == "") {
                        var cols = this.flex.columns;
                        cols[4].visible = false;
                        cols[7].visible = false;
                    }
                    else {
                        this.flex.columns[4].header = this.data.items[0].domino_server_c + ' (document count)';
                        this.flex.columns[7].header = this.data.items[0].domino_server_c + ' (database size)';
                    }
                    this.flex.columns[5].header = this.data.items[0].domino_server_a + ' (database size)';
                    this.flex.columns[6].header = this.data.items[0].domino_server_b + ' (database size)';
                },
                (error) => this.errorMessage = <any>error
                );

        }

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