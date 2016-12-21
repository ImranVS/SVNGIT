import {Component, Input, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';

@Component({
    selector: 'vs-overall-database-grid',
    templateUrl: '/app/dashboards/components/key-metrics/overall-database-grid.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class OverallDatabaseGrid implements WidgetComponent, OnInit {

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

        if (this.widgetService.getProperty("ismailpage") == "True") {
            this.service.get('/dashboard/database?filter_by=IsMailFile&filter_value=true&order_by=FileName&order_type=asc')
                .subscribe(
                (data) => {
                    this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                    this.data.pageSize = 10;
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
                        this.data.pageSize = 20;
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
                            this.data.pageSize = 20;
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
                            this.data.pageSize = 20;
                        },
                        (error) => this.errorMessage = <any>error
                        );
                }
            }
        }
        
    }

}