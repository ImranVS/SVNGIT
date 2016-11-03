import {Component, Input, Output, OnInit, EventEmitter, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent, WidgetService} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';

@Component({
    selector: 'vs-connections-stats-grid',
    templateUrl: './app/reports/components/servers/any-statistic-report-grid.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class AnyStatisticReportGrid implements WidgetComponent, OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @Input() settings: any;

    @Output() select: EventEmitter<string> = new EventEmitter<string>();

    data: wijmo.collections.CollectionView;
    errorMessage: string;

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

        var displayDate = (new Date()).toISOString().slice(0, 10);

        this.service.get(`/reports/summarystats?type=IBM Connections&aggregationType=sum&statName=LIBRARIES_TOTAL_NUM_OF_FILES&startDate=2016-10-21&endDate=2016-10-26`)
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                this.data.pageSize = 10;
            },
            (error) => this.errorMessage = <any>error
            );


    }

    itemsSourceChangedHandler() {
        this.flex.autoSizeColumns();
    }
    

}