import { Component, Input, OnInit, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import * as helpers from '../../../core/services/helpers/helpers';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';

@Component({
    templateUrl: './app/dashboards/components/mobile-users/mobile-users-grid.component.html',
    providers: [
        HttpModule,
        RESTService,
        helpers.DateTimeHelper
    ]
})
export class MobileUsersGrid implements WidgetComponent, OnInit {
    @Input() settings: any;
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;

    data: wijmo.collections.CollectionView;
    errorMessage: string;
    
    constructor(private service: RESTService, protected datetimeHelpers: helpers.DateTimeHelper) { }

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

        this.service.get('/dashboard/mobile_user_devices')
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDateTime(data.data)));
                this.data.pageSize = 20;
            },
            (error) => this.errorMessage = <any>error
            );

    }

    ExportExcel(event) {
        let flex = this.flex;
        wijmo.grid.xlsx.FlexGridXlsxConverter.save(this.flex, { includeColumnHeaders: true, includeCellStyles: false }, "MobileUsers.xlsx");
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