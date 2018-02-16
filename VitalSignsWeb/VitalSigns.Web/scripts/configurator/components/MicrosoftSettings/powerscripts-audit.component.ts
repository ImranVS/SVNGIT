import {Component, Input, Output, OnInit, EventEmitter, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {RESTService} from '../../../core/services';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as gridHelpers from '../../../core/services/helpers/gridutils';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as helpers from '../../../core/services/helpers/helpers';

@Component({
    templateUrl: '/app/configurator/components/MicrosoftSettings/powerscripts-audit.component.html',
    providers: [
        HttpModule,
        RESTService,
        gridHelpers.CommonUtils
    ]
})
export class PowerScriptsAudit implements OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @ViewChild('moreDetailsPopup') dlg: wijmo.input.Popup
    data: wijmo.collections.CollectionView = new wijmo.collections.CollectionView();
    datetimeHelpers: helpers.DateTimeHelper = new helpers.DateTimeHelper();
    errorMessage: string;
    currentPageSize: any = 20;

    selectedItem: any;

    constructor(private service: RESTService, protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) { }

    get pageSize(): number {
        return this.data.pageSize;
    }

    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("PowerScriptsAudit", this.authService.CurrentUser.email),
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
        this.service.get('/services/get_powerscripts_audit_log')
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDateTime(data.data)));
                this.data.pageSize = this.currentPageSize;
            },
            (error) => this.errorMessage = <any>error
            );
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("PowerScriptsAudit", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            );
    }

    moreDetails() {
        this.selectedItem = this.flex.collectionView.currentItem;
        this.dlg.show();
    }
    closePopup() {
        this.dlg.hide();
    }

    itemsSourceChangedHandler() {
        this.flex.autoSizeColumns();
    }

}