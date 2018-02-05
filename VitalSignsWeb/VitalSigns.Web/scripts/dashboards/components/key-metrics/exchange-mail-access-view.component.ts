import { Component, Input, OnInit, ViewChild, ComponentFactoryResolver} from '@angular/core';
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

declare var injectSVG: any;

@Component({
    templateUrl: './app/dashboards/components/key-metrics/exchange-mail-access-view.component.html',
    providers: [
        WidgetService,
        HttpModule,
        RESTService,
        helpers.GridTooltip,
        gridHelpers.CommonUtils,
        helpers.DateTimeHelper,
    ]
})
export class ExchangemailAccessviewGrid implements WidgetComponent, OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @ViewChild('flex1') flex1: wijmo.grid.FlexGrid;
    @ViewChild('moredetailsPopup') dlg: wijmo.input.Popup
    @Input() settings: any;
    data: wijmo.collections.CollectionView;
    detailsdata: wijmo.collections.CollectionView;
    currentDeviceType: string = "Exchange";
    errorMessage: string;
    currentPageSize: any = 10;
    DefaultValues?: Map<string, string>;
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
                name: this.gridHelpers.getGridPageName("ExchangemailAccessviewGrid", this.authService.CurrentUser.email),
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
        this.gridHelpers.ExportExcel(this.flex, "Exchang Mail Access View.xlsx")
    }
    loadData() {
        this.service.get(`/reports/usergroup?deviceType=Exchange&type=User`)
            .subscribe(
            (response) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDateTime(response.data)));
               
                this.data.pageSize = this.currentPageSize;
            },
            (error) => this.errorMessage = <any>error
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
        
    }
    moredetails() {
        var currRow = this.flex.collectionView.currentItem;
        currRow.mailbox_size_mb;
        this.detailsdata = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDateTime(currRow.mailboxes)));
        this.dlg.show();
    }
    closePopup() {
        this.dlg.hide();
    }
}