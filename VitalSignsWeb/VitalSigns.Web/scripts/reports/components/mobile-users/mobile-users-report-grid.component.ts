import {Component, Input, Output, OnInit, EventEmitter, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';
import { ActivatedRoute, Router } from '@angular/router';

import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services';
import * as gridHelpers from '../../../core/services/helpers/gridutils';
import * as helpers from '../../../core/services/helpers/helpers';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';

@Component({
    selector: 'vs-connections-stats-grid',
    templateUrl: './app/reports/components/mobile-users/mobile-users-report-grid.component.html',
    providers: [
        HttpModule,
        RESTService,
        helpers.DateTimeHelper,
        helpers.UrlHelperService,
        gridHelpers.CommonUtils
    ]
})
export class MobileUsersReportGrid implements WidgetComponent, OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @Input() settings: any;

    gridUrl: string = `/dashboard/mobile_user_devices`
    isLoading: boolean = true;
    @Output() select: EventEmitter<string> = new EventEmitter<string>();

    data: wijmo.collections.CollectionView;
    errorMessage: string;
    columns: any;
    paramtype: string;

    constructor(private service: RESTService, private widgetService: WidgetService, private router: Router, private route: ActivatedRoute,
        protected datetimeHelpers: helpers.DateTimeHelper,
        protected urlHelpers: helpers.UrlHelperService, protected gridHelpers: gridHelpers.CommonUtils) { }

    get pageSize(): number {
        return this.data.pageSize;
    }

    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
        }
    }
     
    ngOnInit() {
        //this.gridUrl = this.widgetService.getProperty('gridUrl');
        this.route.queryParams.subscribe(params => {
            this.paramtype = params['isinactive'];
        });
        this.gridUrl = `/dashboard/mobile_user_devices?isInactive=${this.paramtype}`;
        var displayDate = (new Date()).toISOString().slice(0, 10);
        this.isLoading = true;
        this.service.get(this.gridUrl)
            .finally(() => this.isLoading = false)
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDate(data.data)));
                //this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDate(data.data)));
                //this.data.pageSize = 10;
            },
            (error) => this.errorMessage = <any>error
            );
    }

    itemsSourceChangedHandler() {
        this.flex.autoSizeColumns();
    }

    onPropertyChanged(key: string, value: any) {

        if (key === 'gridUrl') {
			this.isLoading = true;
            this.gridUrl = value;

            this.service.get(this.gridUrl)
            	.finally(() => this.isLoading = false)
                .subscribe(
                (data) => {
                    this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDate(data.data)));
                },
                (error) => this.errorMessage = <any>error
                );

        }
    }

    ExportExcel(event) {
        this.gridHelpers.ExportExcel(this.flex, "MobileUsersReport.xlsx")
    }
}