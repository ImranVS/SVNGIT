import {Component, Input, Output, OnInit, EventEmitter, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services';

import * as helpers from '../../../core/services/helpers/helpers';
import * as gridHelpers from '../../../core/services/helpers/gridutils'
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
        helpers.DateTimeHelper,
        gridHelpers.CommonUtils
    ]
})
export class AnyStatisticReportGrid implements WidgetComponent, OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @Input() settings: any;

    baseUrl: string = `/reports/summarystats_aggregation`
    gridUrl: string = this.baseUrl +`?type=Domino&aggregationType=sum&statName=[Mail.Transferred,Mail.TotalRouted,Mail.Delivered]`;

    @Output() select: EventEmitter<string> = new EventEmitter<string>();
    isLoading: boolean = true;
    data: wijmo.collections.CollectionView;
    errorMessage: string;
    columns: any;

    constructor(private service: RESTService, private widgetService: WidgetService, protected datetimeHelpers: helpers.DateTimeHelper, protected gridHelpers: gridHelpers.CommonUtils) { }

    get pageSize(): number {
        return this.data.pageSize;
    }

    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
        }
    }
     
  
    ExportExcel(event) {
        this.gridHelpers.ExportExcel(this.flex, "Mail Volume..xlsx")
    }
    ngOnInit() {
       
        //this.gridUrl = this.widgetService.getProperty('gridUrl');
        if (this.settings & this.settings.url)
            this.gridUrl = this.settings.url;
        var displayDate = (new Date()).toISOString().slice(0, 10);    
        this.loaddata();
    }
    loaddata() {
        this.isLoading = true;
        this.service.get(this.gridUrl)
            .finally(() => this.isLoading = false)
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
                //this.data.pageSize = 10;
            },
            (error) => this.errorMessage = <any>error
            );

    }
    refresh(url) {
        this.gridUrl = url;
        this.loaddata();
    }
    //itemsSourceChangedHandler() {
    //    var flex = this.flex;
    //    var aggregateRow = new wijmo.grid.GroupRow();
    //    aggregateRow.cssClass = 'wj-aggregate-row';
    //    flex.rows.push(aggregateRow);
    //    var colType: string = "";
    //    // update totals now and whenever the data changes
    //    if (aggregateRow) {
    //        for (var i = 0; i < flex.columns.length; i++) {
    //            var colName: string = flex.columns[i].Name;
    //            if (colName == "avg") {
    //                colType = "Average"
    //                break;
    //            } else if (colName == "sum") {
    //                colType = "Total"
    //                break;
    //            }
    //        }
    //        for (var i = 0; i < flex.columns.length; i++) {
    //            var col = flex.columns[i];
    //            if (colType === "Average") {
    //                col.aggregate = "Sum";
    //            } else if (colType === "Sum") {
    //                col.aggregate = "Sum"
    //            }

    //            if (col.binding && col.aggregate) {
    //                var value = wijmo.getAggregate(col.aggregate, flex.collectionView.items, col.binding)
    //                if (value > 0) {
    //                    flex.setCellData(aggregateRow.index, col.index, value, false);
    //                }
    //                if (i == 0) {
    //                    flex.setCellData(aggregateRow.index, col.index, colType, false);
    //                }
    //            }
    //        }
    //    }

    //    this.flex.autoSizeColumns();
    //}
    itemsSourceChangedHandler() {
        var flex = this.flex;
        var aggregateRow = new wijmo.grid.GroupRow();
        aggregateRow.cssClass = 'wj-aggregate-row';
        flex.rows.push(aggregateRow);
        var colType: string = "";
        // update totals now and whenever the data changes
        if (aggregateRow) {
            for (var i = 0; i < flex.columns.length; i++) {
                var colName: string = flex.columns[i].name;
                console.log(colName);
                if (colName == "Average") {
                    colType = "Average"
                    break;
                } else if (colName == "Total") {
                    colType = "Total"
                    break;
                }
            }
            for (var i = 0; i < flex.columns.length; i++) {
                var col = flex.columns[i];
                if (colType === "Average") {
                    col.aggregate = "Avg";
                } else if (colType === "Total") {
                    col.aggregate = "Sum"
                }

                if (col.binding && col.aggregate) {
                    var value = wijmo.getAggregate(col.aggregate, flex.collectionView.items, col.binding)
                    if (value > 0) {
                        flex.setCellData(aggregateRow.index, col.index, value, false);
                    }
                    if (i == 0) {
                        flex.setCellData(aggregateRow.index, col.index, colType, false);
                    }
                }
            }
        }
    }


}