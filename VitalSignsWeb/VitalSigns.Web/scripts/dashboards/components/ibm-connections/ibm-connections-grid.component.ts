import {Component, Input, Output, OnInit, EventEmitter} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent, WidgetService} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';

@Component({
    selector: 'vs-connections-grid',
    templateUrl: './app/dashboards/components/ibm-connections/ibm-connections-grid.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class IBMConnectionsGrid implements WidgetComponent, OnInit {
    @Input() settings: any;

    @Output() select: EventEmitter<string> = new EventEmitter<string>();

    data: wijmo.collections.CollectionView;
    errorMessage: string;
    
    get serviceId(): string {

        return this.widgetService.getProperty('serviceId');

    }

    set serviceId(id: string) {

        this.widgetService.setProperty('serviceId', id);

        this.select.emit(this.widgetService.getProperty('serviceId'));

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
        this.service.get('/services/status_list?type=IBM%20Connections')
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                this.data.pageSize = 10;
                this.data.moveCurrentToPosition(0);
                this.serviceId = this.data.currentItem.device_id;
            },
            (error) => this.errorMessage = <any>error
            );

    }

    onSelectionChanged(event: wijmo.grid.CellRangeEventArgs) {

        this.serviceId = event.panel.grid.selectedItems[0].device_id;

    }
    refreshChart(event: wijmo.grid.CellRangeEventArgs) {

        this.widgetService.refreshWidget('dailyActivities', `/services/summarystats?statName=[BLOGS_CREATED_LAST_DAY,COMMENT_CREATED_LAST_DAY,ENTRY_CREATED_LAST_DAY]&deviceid=${event.panel.grid.selectedItems[0].device_id}`)
            .catch(error => console.log(error));
        this.widgetService.refreshWidget('top5Tags', `/services/top_tags?deviceId=${event.panel.grid.selectedItems[0].device_id}`)
            .catch(error => console.log(error));
    }
}