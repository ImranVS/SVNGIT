import { Component, Input, OnInit, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';
import { WidgetService } from '../../../core/widgets/services/widget.service';
import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as gridHelpers from '../../../core/services/helpers/gridutils';
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
        helpers.DateTimeHelper,
        gridHelpers.CommonUtils
    ]
})
export class DatabaseProblemsGrid implements WidgetComponent, OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @Input() settings: any;
    data: wijmo.collections.CollectionView;
    errorMessage: string;
    customColumnHeader: boolean = true;
    serviceId: string;
    currentPageSize: any = 20;

    constructor(private service: RESTService, private widgetService: WidgetService, protected toolTip: helpers.GridTooltip,
        protected datetimeHelpers: helpers.DateTimeHelper, protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) { }

    get pageSize(): number {
        return this.data.pageSize;
    }

    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("DatabaseProblemsGrid", this.authService.CurrentUser.email),
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
        this.serviceId = this.widgetService.getProperty('serviceId');
        this.service.get(`/dashboard/database-problems?clusterId=${this.serviceId}`)
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDateTime(data.data)));
                this.data.pageSize = this.currentPageSize;
                this.flex.columns[3].header = this.data.items[0].domino_server_a + ' (document count)';
                this.flex.columns[4].header = this.data.items[0].domino_server_b + ' (document count)';
                if (this.data.items[0].domino_server_c == null || this.data.items[0].domino_server_c == "") {
                    var cols = this.flex.columns;
                    cols[5].visible = false;
                    cols[8].visible = false;
                }
                else {
                    var cols = this.flex.columns;
                    cols[5].visible = true;
                    cols[8].visible = true;
                    this.flex.columns[5].header = this.data.items[0].domino_server_c + ' (document count)';
                    this.flex.columns[8].header = this.data.items[0].domino_server_c + ' (database size)';
                }
                this.flex.columns[6].header = this.data.items[0].domino_server_a + ' (database size)';
                this.flex.columns[7].header = this.data.items[0].domino_server_b + ' (database size)';
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
                    this.data.pageSize = this.currentPageSize;
                    this.flex.columns[3].header = this.data.items[0].domino_server_a + ' (document count)';
                    this.flex.columns[4].header = this.data.items[0].domino_server_b + ' (document count)';
                    if (this.data.items[0].domino_server_c == null || this.data.items[0].domino_server_c == "") {
                        var cols = this.flex.columns;
                        cols[5].visible = false;
                        cols[8].visible = false;
                    }
                    else {
                        var cols = this.flex.columns;
                        cols[5].visible = true;
                        cols[8].visible = true;
                        this.flex.columns[5].header = this.data.items[0].domino_server_c + ' (document count)';
                        this.flex.columns[8].header = this.data.items[0].domino_server_c + ' (database size)';
                    }
                    this.flex.columns[6].header = this.data.items[0].domino_server_a + ' (database size)';
                    this.flex.columns[7].header = this.data.items[0].domino_server_b + ' (database size)';
                },
                (error) => this.errorMessage = <any>error
                );
            this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("DatabaseProblemsGrid", this.authService.CurrentUser.email)}`)
                .subscribe(
                (data) => {
                    this.currentPageSize = Number(data.data.value);
                    this.data.pageSize = this.currentPageSize;
                    this.data.refresh();
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