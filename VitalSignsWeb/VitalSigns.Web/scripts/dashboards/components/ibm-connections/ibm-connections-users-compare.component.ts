import {Component, Input, Output, OnInit, EventEmitter, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';
import {ActivatedRoute} from '@angular/router';
import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services';

import {IBMConnectionsGrid} from './ibm-connections-grid.component';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';

@Component({
    selector: 'vs-connections-stats-grid',
    templateUrl: './app/dashboards/components/ibm-connections/ibm-connections-users-compare.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class IBMConnectionsUserComparison implements WidgetComponent, OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @ViewChild('user_selector_1') user_selector_1: wijmo.input.ComboBox;
    @ViewChild('user_selector_2') user_selector_2: wijmo.input.ComboBox;
    @Input() settings: any;

    @Output() select: EventEmitter<string> = new EventEmitter<string>();

    data: wijmo.collections.CollectionView;
    errorMessage: string;
    _serviceId: string;
    url: string;
    userData: any;
    comparisonData: wijmo.collections.CollectionView;

    get serviceId(): string {

        return this._serviceId;

    }

    set serviceId(id: string) {
        this._serviceId = id;
        this.widgetService.setProperty('serviceId', id);

        this.select.emit(this.widgetService.getProperty('serviceId'));

    }


    constructor(private service: RESTService, private widgetService: WidgetService, private route: ActivatedRoute) { }

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
        this.url = `/dashboard/connections/users?deviceid=${this.serviceId}`;
        this.service.get(this.url)
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                this.data.pageSize = 10;
                this.userData = data.data;
                this.comparisonData = null;
            },
            (error) => this.errorMessage = <any>error
            );

    }

    compareUsers() {
        this.route.params.subscribe(params => {
            if (params['service'])
                this.serviceId = params['service'];
            else {
                this.serviceId = this.widgetService.getProperty('serviceId');
            }
        });

        this.url = `/dashboard/connections/compare_users?deviceid=${this.serviceId}&user1=${this.user_selector_1.selectedValue}&user2=${this.user_selector_2.selectedValue}&username1=${this.user_selector_1.selectedItem.name}&username2=${this.user_selector_2.selectedItem.name}`;
        this.service.get(this.url)
            .subscribe(
            (data) => {
                this.comparisonData = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                var groupDesc = new wijmo.collections.PropertyGroupDescription('category');
                this.comparisonData.groupDescriptions.push(groupDesc);
                this.comparisonData.pageSize = 10;
            },
            (error) => this.errorMessage = <any>error
            );
    }
}