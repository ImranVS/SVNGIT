import {Component, Input, Output, OnInit, EventEmitter, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services';

import * as helpers from '../../../core/services/helpers/helpers';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';

@Component({
    selector: 'vs-connections-stats-grid',
    templateUrl: './app/reports/components/mobile-users/mobile-users-report-grid.component.html',
    providers: [
        HttpModule,
        RESTService,
        helpers.DateTimeHelper
    ]
})
export class MobileUsersReportGrid implements WidgetComponent, OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @Input() settings: any;

    gridUrl: string = `/dashboard/mobile_user_devices`

    @Output() select: EventEmitter<string> = new EventEmitter<string>();

    data: wijmo.collections.CollectionView;
    errorMessage: string;
    columns: any;

    constructor(private service: RESTService, private widgetService: WidgetService, protected datetimeHelpers: helpers.DateTimeHelper) { }

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
        //this.gridUrl = this.widgetService.getProperty('gridUrl');
        this.gridUrl = `/dashboard/mobile_user_devices`;
        var displayDate = (new Date()).toISOString().slice(0, 10);
        this.service.get(this.gridUrl)
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDate(data.data)));
                //this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDate(data.data)));
                //this.data.pageSize = 10;
            },
            (error) => this.errorMessage = <any>error
            );
    }

    itemsSourceChangedHandler() {
        this.flex.autoSizeColumns();
    }

    onPropertyChanged(key: string, value: any) {

        if (key === 'gridUrl') {

            this.gridUrl = value;

            this.service.get(this.gridUrl)
                .subscribe(
                (data) => {
                    this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDate(data.data)));
                },
                (error) => this.errorMessage = <any>error
                );

        }
    }

    ExportExcel(event) {
        let flex = this.flex;
        wijmo.grid.xlsx.FlexGridXlsxConverter.save(this.flex, { includeColumnHeaders: true, includeCellStyles: false }, "MobileUsers.xlsx");
    }
}