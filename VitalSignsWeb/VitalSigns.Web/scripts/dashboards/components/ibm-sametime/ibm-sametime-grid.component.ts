﻿import {Component, Input, OnInit} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent, WidgetService} from '../../../core/widgets';
import {RESTService} from '../../../core/services';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';

@Component({
    selector: 'vs-sametime-grid',
    templateUrl: './app/dashboards/components/ibm-sametime/ibm-sametime-grid.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class IBMSametimeGrid implements WidgetComponent, OnInit {
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

        
        
    }

    ngAfterViewInit() {
        //console.log('after init) device id is: ' + this.serviceId);
        //this.widgetService.refreshWidget('responseTimes', `/services/statistics?statName=ResponseTime&deviceid=${this.serviceId}&operation=hourly`)
        //    .catch(error => console.log(error));
        this.service.get('/services/status_list?type=Sametime')
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                this.data.pageSize = 10;
                this.data.moveCurrentToPosition(0);
                this._serviceId = this.data.currentItem.device_id;
                //let overall: IBMSametimeDetails = <IBMSametimeDetails>(this.widgetService.findWidget('sametimeDetails').component);
            },
            (error) => this.errorMessage = <any>error
            );
        
    }

    getAccessColor(access: string) {

        switch (access) {
            case 'Allow':
                return 'green';
            case 'Blocked':
                return 'red';
            default:
                return '';
        }

    }

    refreshChart(event: wijmo.grid.CellRangeEventArgs) {

        //console.log(`/services/statistics?statName=ResponseTime&deviceid=${event.panel.grid.selectedItems[0].device_id}&operation=hourly`);
        this.widgetService.refreshWidget('responseTimes', `/services/statistics?statName=ResponseTime&deviceid=${event.panel.grid.selectedItems[0].device_id}&operation=hourly`)
            .catch(error => console.log(error));
        this.widgetService.refreshWidget('dailyUserLogins', `/services/statistics?statName=Users&deviceid=${event.panel.grid.selectedItems[0].device_id}&operation=hourly`)
            .catch(error => console.log(error));
        //this.widgetService.refreshWidget('oneOnOneCalls', `/services/statistics?statName=Totalcountofall1x1calls&deviceid=${event.panel.grid.selectedItems[0].device_id}&operation=hourly`)
        //    .catch(error => console.log(error));
        
    }
}