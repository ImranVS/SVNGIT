import { Component, Input, OnInit, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import * as helpers from '../../../core/services/helpers/helpers';
import * as gridHelpers from '../../../core/services/helpers/gridutils';
import { AuthenticationService } from '../../../profiles/services/authentication.service';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';

@Component({
    templateUrl: './app/dashboards/components/mobile-users/mobile-users-grid.component.html',
    providers: [
        HttpModule,
        RESTService,
        helpers.DateTimeHelper,
        gridHelpers.CommonUtils
    ]
})
export class MobileUsersGrid implements WidgetComponent, OnInit {
    @Input() settings: any;
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @ViewChild('filter') filter: wijmo.grid.filter.FlexGridFilter;

    data: wijmo.collections.CollectionView;
    errorMessage: string;

    currentPageSize: any = 20;
    
    constructor(private service: RESTService, protected datetimeHelpers: helpers.DateTimeHelper, protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) { }

    get pageSize(): number {
        return this.data.pageSize;
    }

    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();


            var obj = {
                name: this.gridHelpers.getGridPageName("MobileUsersGrid", this.authService.CurrentUser.email),
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

        this.service.get('/dashboard/mobile_user_devices')
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDateTime(data.data)));
                this.data.pageSize = this.currentPageSize;
            },
            (error) => this.errorMessage = <any>error
            );

        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("MobileUsersGrid", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                if (this.data) {
                    this.data.pageSize = this.currentPageSize;
                    this.data.refresh();
                }
            },
            (error) => this.errorMessage = <any>error
            );
           
    }

    ngAfterViewChecked() {
        this.filter.getColumnFilter(9).filterType = 1;
        this.filter.getColumnFilter(0).filterType = 2;
    }
    
    
    ExportExcel(event) {
        this.gridHelpers.ExportExcel(this.flex, "MobileUsers.xlsx")
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