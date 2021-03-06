﻿import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import { RESTService, AppComponentService} from '../../../core/services';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as gridHelpers from '../../../core/services/helpers/gridutils';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';

@Component({
    templateUrl: './app/dashboards/components/ibm-websphere/ibm-websphere-node-grid.component.html',
    providers: [
        HttpModule,
        RESTService,
        gridHelpers.CommonUtils
    ]
})
export class IBMWebsphereNodeGrid implements WidgetComponent, OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @Input() settings: any;
    serviceId: string;
    data: wijmo.collections.CollectionView;
    errorMessage: string;
    currentPageSize: any = 20;
    
    constructor(private service: RESTService, protected widgetService: WidgetService, protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService,
        private appComponentService: AppComponentService) { }

    get pageSize(): number {
        return this.data.pageSize;
    }

    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("IBMWebsphereNodeGrid", this.authService.CurrentUser.email),
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
        this.appComponentService.showProgressBar();
        this.serviceId = this.widgetService.getProperty('serviceId');

        this.service.get(`/services/websphere_devices?parentid=${this.serviceId}&devicetype=WebSphereNode`)
            .subscribe(
            (data) => {
                this.appComponentService.hideProgressBar();
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                this.data.pageSize = this.currentPageSize
;
            },
            (error) => {
                this.appComponentService.hideProgressBar();
                this.errorMessage = <any>error
            });
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("IBMWebsphereNodeGrid", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
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

    onPropertyChanged(key: string, value: any) {

        if (key === 'serviceId') {

            this.serviceId = value;

            this.service.get(`/services/websphere_devices?parentid=${this.serviceId}&devicetype=WebSphereNode`)
                .subscribe(
                (data) => {
                    this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                    this.data.pageSize = this.currentPageSize
;
                },
                (error) => this.errorMessage = <any>error
                );

        }

        //super.onPropertyChanged(key, value);

    }

    onItemsSourceChanged() {
        var row = this.flex.columnHeaders.rows[0];
        row.wordWrap = true;
        // autosize first header row
        this.flex.autoSizeRow(0, true);

    }
}