import {Component, Input, OnInit, EventEmitter, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';
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
    templateUrl: './app/dashboards/components/ibm-connections/ibm-connections-community-user.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class IBMConnectionsCommunityUser implements WidgetComponent, OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @Input() settings: any;

    data: wijmo.collections.CollectionView;
    errorMessage: string;
    serviceId: string;
    url: string;
    userData: any;
    comparisonData: wijmo.collections.CollectionView;

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
        this.serviceId = this.widgetService.getProperty('serviceId');
        this.loadData();
    }

    loadData() {
        this.url = `/dashboard/connections/community_user?deviceid=${this.serviceId}`;
        this.service.get(this.url)
            .subscribe(
                (data) => {
                    this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                    var groupDesc = new wijmo.collections.PropertyGroupDescription('community');
                    this.data.groupDescriptions.push(groupDesc);
                    this.data.pageSize = 10;
                },
                (error) => this.errorMessage = <any>error
            );
    }

    onPropertyChanged(key: string, value: any) {

        if (key === 'serviceId') {
            this.serviceId = value;
            this.loadData()
        }

    }

}