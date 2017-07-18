import {Component, Input, Output, OnInit, EventEmitter, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services';

import {IBMConnectionsGrid} from './ibm-connections-grid.component';
import {ActivatedRoute} from '@angular/router';
import * as gridHelpers from '../../../core/services/helpers/gridutils';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';

@Component({
    selector: 'vs-connections-communities-detail-grid',
    templateUrl: './app/dashboards/components/ibm-connections/ibm-connections-communities-detail-grid.component.html',
    providers: [
        HttpModule,
        RESTService,
        gridHelpers.CommonUtils
    ]
})
export class IBMConnectionsCommunitiesDetailGrid implements WidgetComponent, OnInit {
    @Input() settings: any;

    @Output() select: EventEmitter<string> = new EventEmitter<string>();
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    data: wijmo.collections.CollectionView;
    errorMessage: string;
    _serviceId: string;
    
    get serviceId(): string {

        return this._serviceId

    }

    set serviceId(id: string) {
        this._serviceId = id;
        this.widgetService.setProperty('serviceId', id);

        this.select.emit(this.widgetService.getProperty('serviceId'));

    }


    constructor(private service: RESTService, private widgetService: WidgetService, private route: ActivatedRoute, protected gridHelpers: gridHelpers.CommonUtils) { }

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
        this.route.params.subscribe(params => {
            if (params['service'])
                this.serviceId = params['service'];
            else {

                this.serviceId = this.widgetService.getProperty('serviceId');
            }
        });

        var displayDate = (new Date()).toISOString().slice(0, 10);
        this.service.get(`/reports/community_users?deviceId=${this.serviceId}`)
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                this.data.pageSize = 500;
            },
            (error) => this.errorMessage = <any>error
            );

    }

    onPropertyChanged(key: string, value: any) {
        if (key === 'serviceId') {
            var url = '';
            this.service.get(`/reports/community_users?deviceId=${this.serviceId}`)
                .subscribe(
                (data) => {
                    this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                    this.data.pageSize = 10;
                },
                (error) => this.errorMessage = <any>error
                );

        }

    }

    
    ExportExcel(event) {
        this.gridHelpers.ExportExcel(this.flex, "Communities.xlsx")
    }

}