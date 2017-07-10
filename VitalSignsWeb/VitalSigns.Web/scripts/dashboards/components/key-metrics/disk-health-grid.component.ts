import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as helpers from '../../../core/services/helpers/helpers';
import * as gridHelpers from '../../../core/services/helpers/gridutils';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';

@Component({
    selector: 'vs-disk-health-grid',
    templateUrl: '/app/dashboards/components/key-metrics/disk-health-grid.component.html',
    providers: [
        HttpModule,
        RESTService,
        helpers.DateTimeHelper,
        gridHelpers.CommonUtils
    ]
})
export class DiskHealthGrid implements WidgetComponent, OnInit {
    @Input() settings: any;
    @ViewChild("flex") wjFlexGrid: wijmo.grid.FlexGrid;
    data: wijmo.collections.CollectionView;
    errorMessage: string;
    _serviceId: string;
    currentPageSize: any = 20;

    get serviceId(): string {
        return this._serviceId;
    }
    constructor(private service: RESTService, private widgetService: WidgetService, protected datetimeHelpers: helpers.DateTimeHelper, protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) { }

    get pageSize(): number {
        return this.data.pageSize;
    }

    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("DiskHealthGrid", this.authService.CurrentUser.email),
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

        this.service.get(`/dashboard/disk_health`)
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDateTime(data.data)));
                this.data.pageSize = this.currentPageSize;
                //var tgd = new wijmo.collections.PropertyGroupDescription('device_name',
                //    function (item, propName) {
                //        var value = item[propName];

                //        // special treatmanent for empty/null
                //        if (value == null || value == '' || value.trim() == '') {
                //            return '(empty)';
                //        }

                //        // other values
                //        return value;
                //    });

                var groupDesc = new wijmo.collections.PropertyGroupDescription('device_name');
                this.data.groupDescriptions.push(groupDesc);
            },
            (error) => this.errorMessage = <any>error
            );
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("DiskHealthGrid", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            );
    }

    //ImportExcel(event) {
    //    let flex = this.flex;
    //    let fileEle = <HTMLInputElement>$("#importFile")[0];
    //    wijmo.grid.xlsx.FlexGridXlsxConverter.load(this.flex, fileEle.files[0], { includeColumnHeaders: true });
    //}
    //// Save Funciton
    //ExportExcel(event) {
    //    let flex = this.flex;
    //    wijmo.grid.xlsx.FlexGridXlsxConverter.save(this.flex, { includeColumnHeaders: true, includeCellStyles: false }, "FlexGrid.xlsx");
    //}
}