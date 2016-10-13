import {Component, Input, Output, OnInit, EventEmitter} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent, WidgetService} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import {IBMConnectionsGrid} from './ibm-connections-grid.component';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';

@Component({
    selector: 'vs-connections-activities-grid',
    templateUrl: './app/dashboards/components/ibm-connections/ibm-connections-activities-grid.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class IBMConnectionsActivitiesGrid implements WidgetComponent, OnInit {
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

        this.serviceId = this.widgetService.getProperty('serviceId');

        var displayDate = (new Date()).toISOString().slice(0, 10);
        console.log(`/services/summarystats?statName=NUM_OF_ACTIVITIES_*&deviceId=${this.serviceId}&isChart=false&startDate=${displayDate}&endDate=${displayDate}`)
        this.service.get(`/services/summarystats?statName=NUM_OF_ACTIVITIES_*&deviceId=${this.serviceId}&isChart=false&startDate=${displayDate}&endDate=${displayDate}`)
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                this.data.pageSize = 10;
            },
            (error) => this.errorMessage = <any>error
            );

    }
}