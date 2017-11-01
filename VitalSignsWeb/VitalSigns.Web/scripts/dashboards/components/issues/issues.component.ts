import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services';
import {AppNavigator} from '../../../navigation/app.navigator.component';
import { AuthenticationService } from '../../../profiles/services/authentication.service';

import * as gridHelpers from '../../../core/services/helpers/gridutils';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as helpers from '../../../core/services/helpers/helpers';

declare var injectSVG: any;


@Component({
    templateUrl: '/app/dashboards/components/issues/issues.component.html',
    providers: [
        HttpModule,
        RESTService,
        helpers.GridTooltip,
        helpers.DateTimeHelper,
        gridHelpers.CommonUtils
      

    ]
})
export class Issues implements OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @Input() settings: any;
    deviceId: any;
    data: wijmo.collections.CollectionView;
    errorMessage: string;
    currentPageSize: any = 20;
    private isResized = false;

    

    constructor(private service: RESTService, private route: ActivatedRoute, protected toolTip: helpers.GridTooltip, protected datetimeHelpers: helpers.DateTimeHelper,
        protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) { }

    get pageSize(): number {
        return this.data.pageSize;
    }
    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("Issues", this.authService.CurrentUser.email),
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
        this.gridHelpers.ExportExcel(this.flex, "issues.xlsx")
    }

    ngOnInit() {
        this.service.get('/Configurator/get_all_open_issues/')
            .subscribe(
            (response) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDateTime(response.data)));
                this.data.pageSize = this.currentPageSize;
            },
            (error) => this.errorMessage = <any>error
            );
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("Issues", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            );
        window.addEventListener('resize', () => {
        this.isResized = true;
        });
        this.toolTip.getTooltip(this.flex, 0, 3);
    }
    initialized(grid) {
        setTimeout(() => {
            grid.autoSizeRows();
        }, 100);
    }

    loadedRows(grid) {
        if (grid.isInitialized) {
            grid.autoSizeRows();
        }
    }
    resizedColumn(grid, e) {
     grid.autoSizeRows();
    }
    updatedLayout(grid) {
            if (this.isResized) {
            this.isResized = false;
            grid.autoSizeRows();
        }
    }
     ngAfterViewChecked() {
        injectSVG();
    }
}



