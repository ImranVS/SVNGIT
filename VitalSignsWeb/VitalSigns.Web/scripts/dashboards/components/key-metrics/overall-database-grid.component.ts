import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as gridHelpers from '../../../core/services/helpers/gridutils';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as helpers from '../../../core/services/helpers/helpers';

@Component({
    selector: 'vs-overall-database-grid',
    templateUrl: '/app/dashboards/components/key-metrics/overall-database-grid.component.html',
    providers: [
        HttpModule,
        RESTService,
        helpers.GridTooltip,
        gridHelpers.CommonUtils
    ]
})
export class OverallDatabaseGrid implements WidgetComponent, OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @Input() settings: any;

    data: wijmo.collections.CollectionView;
    errorMessage: string;
    _serviceId: string;
    currentPageSize: any = 20;

    get serviceId(): string {
        return this._serviceId;
    }
    constructor(private service: RESTService, private widgetService: WidgetService, protected toolTip: helpers.GridTooltip, protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService) { }

    get pageSize(): number {
        return this.data.pageSize;
    }

    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("OverallDatabaseGrid", this.authService.CurrentUser.email),
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
        let flex = this.flex;
        wijmo.grid.xlsx.FlexGridXlsxConverter.save(this.flex, { includeColumnHeaders: true, includeCellStyles: false }, "DominoDatabases.xlsx");
    }

    ngOnInit() {

        if (this.widgetService.getProperty("ismailpage") == "True") {
            this.service.get('/dashboard/database?filter_by=IsMailFile&filter_value=true&order_by=FileName&order_type=asc')
                .subscribe(
                (data) => {
                    this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                    this.data.pageSize = this.currentPageSize;
                    var groupDesc = new wijmo.collections.PropertyGroupDescription('device_name');
                    this.data.groupDescriptions.push(groupDesc);
                },
                (error) => this.errorMessage = <any>error
                );
        }
        else if (this.widgetService.getProperty("ismailpage") == "False") {
            if (this.widgetService.getProperty("exceptions") == "True") {
                this.service.get('/dashboard/database?order_by=FileName&order_type=asc&exceptions=true')
                    .subscribe(
                    (data) => {
                        this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                        this.data.pageSize = this.currentPageSize;
                        //var flex = new wijmo.grid.FlexGrid('#flex');
                    },
                    (error) => this.errorMessage = <any>error
                    );
            }
            else {
                if (this.widgetService.getProperty("istemplate") == "True") {
                    this.service.get('/dashboard/database?order_by=FileName&order_type=asc')
                        .subscribe(
                        (data) => {
                            this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                            this.data.pageSize = this.currentPageSize;
                            var groupDesc = new wijmo.collections.PropertyGroupDescription('design_template_name');
                            this.data.groupDescriptions.push(groupDesc);
                        },
                        (error) => this.errorMessage = <any>error
                        );
                }
                else {
                    this.service.get('/dashboard/database?order_by=FileName&order_type=asc')
                        .subscribe(
                        (data) => {
                            this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                            this.data.pageSize = this.currentPageSize;
                        },
                        (error) => this.errorMessage = <any>error
                        );

                }
            }
        }
        this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("OverallDatabaseGrid", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error
            );
        this.toolTip.getTooltip(this.flex, 0, 6);
        
    }

}