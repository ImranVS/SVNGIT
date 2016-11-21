import {Component, Input, Output, OnInit, EventEmitter, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent, WidgetService} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import * as helpers from '../../../core/services/helpers/helpers';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';

@Component({
    selector: 'vs-connections-stats-grid',
    templateUrl: './app/reports/components/servers/any-statistic-report-grid.component.html',
    providers: [
        HttpModule,
        RESTService,
        helpers.DateTimeHelper
    ]
})
export class AnyStatisticReportGrid implements WidgetComponent, OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @Input() settings: any;

    @Output() select: EventEmitter<string> = new EventEmitter<string>();

    data: wijmo.collections.CollectionView;
    errorMessage: string;

    constructor(private service: RESTService, private widgetService: WidgetService, protected datetimeHelpers: helpers.DateTimeHelper) { }

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

        var displayDate = (new Date()).toISOString().slice(0, 10);
        var startDate = new Date(2016, 10, 5).toISOString();
        var endDate = new Date(2016, 10, 20).toISOString();
        this.service.get(`/reports/summarystats_aggregation?type=IBM Connections&aggregationType=sum&statName=NUM_OF_FORUMS_FORUMS&startDate=${startDate}&endDate=${endDate}`)
            .subscribe(
            (data) => {
                var newData = this.datetimeHelpers.toLocalDate(data);




                newData.data.forEach(function (entity) {
                    var colName = Object.keys(entity)[1];
                    var colValue = entity[colName];
                    var colDesc = Object.getOwnPropertyDescriptor(entity, colName);

                    delete entity[colName];
                    Object.defineProperty(entity, colName, colDesc);
                    
                });



                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(newData.data));
                //this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.datetimeHelpers.toLocalDate(data.data)));
                this.data.pageSize = 10;
            },
            (error) => this.errorMessage = <any>error
            );


    }

    itemsSourceChangedHandler() {
        this.flex.autoSizeColumns();
    }
    

}