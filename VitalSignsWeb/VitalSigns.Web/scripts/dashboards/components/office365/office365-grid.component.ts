﻿import {Component, Input, Output, OnInit, EventEmitter, ViewChild} from '@angular/core';
import {HttpModule}    from '@angular/http';

import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {RESTService} from '../../../core/services';

import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as helpers from '../../../core/services/helpers/helpers';

@Component({
    selector: 'vs-office365-grid',
    templateUrl: './app/dashboards/components/office365/office365-grid.component.html',
    providers: [
        HttpModule,
        RESTService,
        helpers.GridTooltip
    ]
})
export class Office365Grid implements WidgetComponent, OnInit {
    @Input() settings: any;
    @Output() select: EventEmitter<string> = new EventEmitter<string>();
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;  
    data: wijmo.collections.CollectionView;
    errorMessage: string;
    
    get serviceId(): string {

        return this.widgetService.getProperty('serviceId');

    }

    set serviceId(id: string) {

        this.widgetService.setProperty('serviceId', id);

        this.select.emit(this.widgetService.getProperty('serviceId'));

    }


    constructor(private service: RESTService, private widgetService: WidgetService, protected toolTip: helpers.GridTooltip) { }

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
        this.service.get('/services/status_list?type=Office365')
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                this.data.pageSize = 10;
                this.data.moveCurrentToPosition(0);
                this.serviceId = this.data.currentItem.device_id + ';' + this.data.currentItem.category;
            },
            (error) => this.errorMessage = <any>error
            );
        this.toolTip.getTooltip(this.flex, 0, 3);
    }

    

    onSelectionChanged(event: wijmo.grid.CellRangeEventArgs) {
        this.serviceId = event.panel.grid.selectedItems[0].device_id + ';' + event.panel.grid.selectedItems[0].category;
    }

    refreshChart(event: wijmo.grid.CellRangeEventArgs) {
        var nodeName = event.panel.grid.selectedItems[0].category;
        var deviceid = event.panel.grid.selectedItems[0].device_id;

        this.widgetService.refreshWidget('mailServices', `/services/statistics?deviceId=${deviceid}&statName=[POP@` + nodeName + `,IMAP@` + nodeName + `,SMTP@` + nodeName + `]$operation=HOURLY&isChart=true`)
            .catch(error => console.log(error));
    }
}