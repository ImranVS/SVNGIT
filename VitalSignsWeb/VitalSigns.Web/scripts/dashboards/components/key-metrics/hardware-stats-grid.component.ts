import { Component, Input, OnInit, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services';
import * as gridHelpers from '../../../core/services/helpers/gridutils';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';

@Component({
    selector: 'vs-hardware-stats-grid',
    templateUrl: '/app/dashboards/components/key-metrics/hardware-stats-grid.component.html',
    providers: [
        HttpModule,
        RESTService,
        gridHelpers.CommonUtils
    ]
})
export class HardwareStatisticsGrid implements WidgetComponent, OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @Input() settings: any;

    data: wijmo.collections.CollectionView;
    errorMessage: string;
    _serviceId: string;
    currentPageSize: any = 20;

    get serviceId(): string {
        return this._serviceId;
    }
    constructor(private service: RESTService, private widgetService: WidgetService, protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) { }

    get pageSize(): number {
        return this.data.pageSize;
    }

    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("HardwareStatisticsGrid", this.authService.CurrentUser.email),
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

        this.service.get(`/dashboard/cpu_memory_health`)
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            );

        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("HardwareStatisticsGrid", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            );
    }

    ExportExcel(event) {
        let flex = this.flex;
        wijmo.grid.xlsx.FlexGridXlsxConverter.save(this.flex, { includeColumnHeaders: true, includeCellStyles: false }, "CPUMemoryHealth.xlsx");
    }
}