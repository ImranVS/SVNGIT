import {Component, Input, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent, WidgetService} from '../../../core/widgets';
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

    @Input() settings: any;

    data: wijmo.collections.CollectionView;
    errorMessage: string;
    _serviceId: string;

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

        this.service.get('/services/summarystats?statName=[Mail.Delivered,Mail.TotalRouted]&startDate=2016-08-01&endDate=2016-08-19')
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                this.data.pageSize = 20;
                this.data.moveCurrentToPosition(0);
                this._serviceId = this.data.currentItem.device_id;
            },
            (error) => this.errorMessage = <any>error
            );
    }

}