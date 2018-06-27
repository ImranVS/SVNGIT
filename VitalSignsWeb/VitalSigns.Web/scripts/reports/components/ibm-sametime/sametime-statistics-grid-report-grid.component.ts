import {Component, Input, Output, OnInit, EventEmitter, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as gridHelpers from '../../../core/services/helpers/gridutils';
import * as helpers from '../../../core/services/helpers/helpers';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';

@Component({
    selector: 'vs-connections-stats-grid',
    templateUrl: './app/reports/components/ibm-sametime/sametime-statistics-grid-report-grid.component.html',
    providers: [
        HttpModule,
        RESTService,
        helpers.DateTimeHelper,
        gridHelpers.CommonUtils
    ]
})
export class SametimeStatisticGridReportGrid implements WidgetComponent, OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @Input() settings: any;

    gridUrl: string = '/reports/sametime_stats_grid';

    @Output() select: EventEmitter<string> = new EventEmitter<string>();

    data: wijmo.collections.CollectionView;
    errorMessage: string;
    currentPageSize: any = 20;
    isLoading: boolean = true;

    constructor(private service: RESTService, private widgetService: WidgetService, protected datetimeHelpers: helpers.DateTimeHelper,
        protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) { }

    get pageSize(): number {
        return this.data.pageSize;
    }

    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("SametimeStatisticGridReportGrid", this.authService.CurrentUser.email),
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

        var displayDate = (new Date()).toISOString().slice(0, 10);
		this.isLoading = true;
        this.service.get(this.gridUrl)
            .finally(() => this.isLoading = false)
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDate(data.data)));
                this.data.pageSize = this.currentPageSize;
            },
            (error) => this.errorMessage = <any>error
            );


    }

    itemsSourceChangedHandler() {
        this.flex.autoSizeColumns();
        //console.log(this.flex.columns.getColumn('device_name').dataMap.getDisplayValues().keys)
       // this.flex.group
    }
    

    onPropertyChanged(key: string, value: any) {

        if (key === 'gridUrl') {

            this.gridUrl = value;
            this.isLoading = true;
            this.service.get(this.gridUrl)
                .finally(() => this.isLoading = false)
                .subscribe(
                (data) => {
                    this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDate(data.data)));
                    this.data.pageSize = this.currentPageSize;
                },
                (error) => this.errorMessage = <any>error
                );
            this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("SametimeStatisticGridReportGrid", this.authService.CurrentUser.email)}`)
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
}