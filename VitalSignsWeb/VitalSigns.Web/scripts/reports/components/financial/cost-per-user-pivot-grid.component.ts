import {Component, Input, Output, OnInit, EventEmitter} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent, WidgetService} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
//import * as wjFlexOLAP from 'wijmo/wijmo.angular2.olap';

@Component({
    selector: 'vs-cost-per-user-grid',
    templateUrl: './app/reports/components/financial/cost-per-user-pivot-grid.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class CostPerUserPivotGrid implements WidgetComponent, OnInit {
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
        this.service.get('/reports/cost_per_user?statName=Server.Users&isChart=false')
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                this.data.pageSize = 10;
            },
            (error) => this.errorMessage = <any>error
            );

    }

    //initPanel = function (sender, e) {
        //var ng = sender.engine;
        //ng.rowFields.push('device_name', 'cost_per_user');
        //ng.valueFields.push('stat_value');
        //ng.showRowTotals = wijmo.olap.ShowTotals.Subtotals;
        //ng.showColTotals = wijmo.olap.ShowTotals.Subtotals;
    //}

}