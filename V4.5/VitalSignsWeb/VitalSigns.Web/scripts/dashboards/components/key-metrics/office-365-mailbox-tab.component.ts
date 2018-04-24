import { Component, Input, OnInit, ViewChild, ComponentFactoryResolver } from '@angular/core';
import { WidgetController, WidgetContainer, WidgetContract } from '../../../core/widgets';
import { WidgetService } from '../../../core/widgets/services/widget.service';

import { MicrosoftPowerShellScripts, initSettings } from '../../../services/components/microsoft-powershell-scripts.component';

import {HttpModule} from '@angular/http';
import {WidgetComponent} from '../../../core/widgets';
import {RESTService} from '../../../core/services';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as gridHelpers from '../../../core/services/helpers/gridutils';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as helpers from '../../../core/services/helpers/helpers';

declare var injectSVG: any;

@Component({
    templateUrl: './app/dashboards/components/key-metrics/office-365-mailbox-tab.component.html',
    providers: [
        WidgetService,
        HttpModule,
        RESTService,
        helpers.GridTooltip,
        gridHelpers.CommonUtils,
        helpers.DateTimeHelper,
    ]
})
export class Office365MailboxViewTab implements OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @ViewChild('powershellPopup') dlg: wijmo.input.Popup
    @ViewChild(MicrosoftPowerShellScripts) powershellWindow: MicrosoftPowerShellScripts
    @Input() settings: any;
    data: wijmo.collections.CollectionView;
    errorMessage: string;
    currentPageSize: any = 20;
    widgets: WidgetContract[];
    showPowerScripts: boolean = false;
    isLoading: boolean = true;

    constructor(protected resolver: ComponentFactoryResolver, protected widgetService: WidgetService, private service: RESTService, protected toolTip: helpers.GridTooltip,
        protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService, protected datetimeHelpers: helpers.DateTimeHelper) {
        //super(resolver, widgetService);
    }

    get pageSize(): number {
        return this.data.pageSize;
    }
    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("Office365MailboxViewGrid", this.authService.CurrentUser.email),
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
        this.gridHelpers.ExportExcel(this.flex, "Office 365 Mailbox View.xlsx")
    }
    loadData() {
        this.isLoading = true;
        this.service.get(`/dashboard/office_365_mailboxes`)
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
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("Office365MailboxViewGrid", this.authService.CurrentUser.email)}`)
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

    ngAfterViewChecked() {
        injectSVG();
    }

    gridSourceChanged() {
        var row = this.flex.columnHeaders.rows[0];
        row.wordWrap = true;
        this.flex.autoSizeRow(0, true);

        this.flex.autoSizeColumns(0, 0);
        this.flex.autoSizeColumns(2, 2);
        this.flex.autoSizeColumns(16, 16);

        //Deliver to mailbox and foward
        //Foward Rule as Attachment

        this.flex.columns[11].width = 140;
        this.flex.columns[12].width = 120;
        this.flex.columns[13].width = 120;
        this.flex.columns[14].width = 80;
        this.flex.columns[15].width = 110;

        //lock 1st column
        this.flex.frozenColumns = 1;
    }

    PowerShellScripts() {
        var currRow = this.flex.collectionView.currentItem;

        var initParams: initSettings = {
            DeviceType: 'Office365',
            DefaultValues: new Map([['Name', currRow.display_name]]),
            SubTypes: ["Mailbox"]
        }
        this.powershellWindow.initValues(initParams);
        this.dlg.show();
    }

    closePopup() {
        this.dlg.hide();
    }
}