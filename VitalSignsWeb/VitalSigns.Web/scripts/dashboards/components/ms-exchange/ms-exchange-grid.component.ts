﻿import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as gridHelpers from '../../../core/services/helpers/gridutils';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as helpers from '../../../core/services/helpers/helpers';

@Component({
    templateUrl: './app/dashboards/components/ms-exchange/ms-exchange-grid.component.html',
    providers: [
        HttpModule,
        RESTService,
        helpers.GridTooltip,
        gridHelpers.CommonUtils
    ]
})
export class MSExchangeGrid implements WidgetComponent, OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @Input() settings: any;

    data: wijmo.collections.CollectionView;
    errorMessage: string;
    currentPageSize: any = 20;
    Mailobj: any;

    constructor(private service: RESTService, protected toolTip: helpers.GridTooltip, protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) { }

    get pageSize(): number {
        return this.data.pageSize;
    }

    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("IBMDominoGrid", this.authService.CurrentUser.email),
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
        this.service.get('/services/status_list?type=Exchange&isenabled=true')

            .subscribe(
            (data) => {
                for (var x = 0; x < data.data.length; x++) {
                    if (data.data[x].server_roles && data.data[x].server_roles.indexOf("CAS") > -1) {
                        data.data[x]["CAS"] = true;
                    }
                    else { data.data[x]["CAS"] = false; }

                    if (data.data[x].server_roles && data.data[x].server_roles.indexOf("Mailbox") > -1) {
                        data.data[x]["Mailbox"] = true;
                    }
                    else { data.data[x]["Mailbox"] = false; }

                    if (data.data[x].server_roles && data.data[x].server_roles.indexOf("EDGE") > -1) {
                        data.data[x]["EDGE"] = true;
                    }
                    else { data.data[x]["EDGE"] = false; }

                    if (data.data[x].server_roles && data.data[x].server_roles.indexOf("HUB") > -1) {
                        data.data[x]["HUB"] = true;
                    }
                    else { data.data[x]["HUB"] = false; }
                }
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            );
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("MSExchangeGrid", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            );

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
    
    

    onItemsSourceChanged() {
        var row = this.flex.columnHeaders.rows[0];
        row.wordWrap = true;
        // autosize first header row
        this.flex.autoSizeRow(0, true);

    }

}