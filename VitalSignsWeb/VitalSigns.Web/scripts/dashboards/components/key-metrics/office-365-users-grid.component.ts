﻿import { Component, Input, OnInit, ViewChild, ComponentFactoryResolver} from '@angular/core';
import {HttpModule}    from '@angular/http';
import { WidgetComponent } from '../../../core/widgets';
import { WidgetService } from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as gridHelpers from '../../../core/services/helpers/gridutils';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as helpers from '../../../core/services/helpers/helpers';
import { MicrosoftPowerShellScripts, initSettings } from '../../../services/components/microsoft-powershell-scripts.component';

declare var injectSVG: any;

@Component({
    templateUrl: './app/dashboards/components/key-metrics/office-365-users-grid.component.html',
    providers: [
        WidgetService,
        HttpModule,
        RESTService,
        helpers.GridTooltip,
        gridHelpers.CommonUtils,
        helpers.DateTimeHelper,
    ]
})
export class Office365UsersGrid implements WidgetComponent, OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @ViewChild('flex1') flex1: wijmo.grid.FlexGrid;
    @ViewChild('moredetailsPopup') dlg: wijmo.input.Popup
    @ViewChild('powershellPopup') powerscriptsPopup: wijmo.input.Popup
    @ViewChild(MicrosoftPowerShellScripts) powershellWindow: MicrosoftPowerShellScripts
    @Input() settings: any;
    selectedrow: any = null;
    data: wijmo.collections.CollectionView;
    detailsdata: wijmo.collections.CollectionView;
    errorMessage: string;
    currentPageSize: any = 10;
    DefaultValues?: Map<string, string>;
    isLoading: boolean = true;
    showPowerScripts: boolean = false;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService, protected toolTip: helpers.GridTooltip,
        protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService, protected datetimeHelpers: helpers.DateTimeHelper) {
    }
    get pageSize(): number {
        return this.data.pageSize;
    }
    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("Office365UsersGrid", this.authService.CurrentUser.email),
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

    ExportExcel(event) {
        this.gridHelpers.ExportExcel(this.flex, "Office 365 Users Grid.xlsx")
    }
    loadData() {
        this.isLoading = true;
        this.service.get(`/dashboard/office_365_users`)
            .subscribe(
            (response) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDateTime(response.data)));
               
                this.data.pageSize = this.currentPageSize;
                this.isLoading = false;
            },
            (error) => { this.errorMessage = <any>error; this.isLoading = false; }
            );
    }

    ngOnInit() {
        this.loadData();
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("ExchangemailAccessviewGrid", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            );
        this.showPowerScripts = this.authService.isCurrentUserInRole("PowerScripts");
    }
    moredetails() {
        this.selectedrow = this.flex.collectionView.currentItem;
        this.selectedrow.mailbox_size_mb;
        this.detailsdata = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDateTime(this.selectedrow.mailboxes)));
        this.dlg.show();
    }
    closePopup() {
        this.dlg.hide();
    }

    gridSourceChanged() {
        this.flex.autoSizeColumns();
    }

    PowerShellScripts() {
        var currRow = this.flex.collectionView.currentItem;

        var initParams: initSettings = {
            DeviceType: 'Office365',
            DefaultValues: new Map([['UserPrincipalName', currRow.user_principal_name]]),
            SubTypes: ["User"]
        }
        this.powershellWindow.initValues(initParams);
        this.powerscriptsPopup.show();
    }
}