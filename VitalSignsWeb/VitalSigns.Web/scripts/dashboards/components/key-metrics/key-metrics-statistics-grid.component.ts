import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';

@Component({
    selector: 'vs-keymetrics-alpha-grid',
    templateUrl: '/app/dashboards/components/key-metrics/key-metrics-statistics-grid.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class KeyMetricsStatisticsGrid implements WidgetComponent, OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @Input() settings: any;

    data: wijmo.collections.CollectionView;
    errorMessage: string;
    _serviceId: string;
    filterDate: string;

    get serviceId(): string {
        return this._serviceId;
    }
    constructor(private service: RESTService, private widgetService: WidgetService) { }

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

        var displayDate = (new Date()).toISOString().slice(0, 7);

        this.service.get(`/dashboard/mail_health?month=${displayDate}`)
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                this.data.pageSize = 20;
            },
            (error) => this.errorMessage = <any>error
            );
        var today = new Date();
        this.filterDate = today.toISOString().substr(0, 7);
    }

    onItemsSourceChanged() {
        var row = this.flex.columnHeaders.rows[0];
        row.wordWrap = true;
        // autosize first header row
        this.flex.autoSizeRow(0, true);

    }

    filterStats() {
        var dt = new Date(this.filterDate);
        this.service.get(`/dashboard/mail_health?month=` + dt.toISOString().substr(0, 7))
            .subscribe(
            (response) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
                this.data.pageSize = 20;
            },
            (error) => this.errorMessage = <any>error
            );

    }
}