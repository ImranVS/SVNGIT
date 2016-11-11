import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, FormsModule, Validators} from '@angular/forms';
import { CommonModule } from '@angular/common';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {AppNavigator} from '../../../navigation/app.navigator.component';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as wjCoreModule from 'wijmo/wijmo.angular2.core';
import {GridBase} from '../../../core/gridBase';

@Component({
    selector: 'vs-notification-definitions',
    templateUrl: '/app/configurator/components/alert/alert-definitions.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class AlertDefinitions extends GridBase implements OnInit  {    
    devices: string;
    errorMessage: string;
    eventsdata: any;

    get pageSize(): number {
        return this.data.pageSize;
    }

    constructor(service: RESTService) {
        super(service);
        this.formName = "Notification Definition";
    }

    ngOnInit() {
        this.service.get('/configurator/notifications_list')
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data[0]));
                this.data.pageSize = 20;
                var groupDesc = new wijmo.collections.PropertyGroupDescription('notification_name');
                this.data.groupDescriptions.push(groupDesc);
            },
            (error) => this.errorMessage = <any>error
        );
        this.service.get('/configurator/events_master_list')
            .subscribe(
            (data) => {
                this.eventsdata = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data));
                this.eventsdata.pageSize = 10;
                var groupDesc = new wijmo.collections.PropertyGroupDescription('DeviceType');
                this.eventsdata.groupDescriptions.push(groupDesc);
            },
            (error) => this.errorMessage = <any>error
            );
    }
 
    eventCheck(value, event) {
        if (event.target.checked) {
            this.eventsdata.push(value);
        }
        else {
            this.eventsdata.splice(this.eventsdata.indexOf(value), 1);
        }
    }

    changeInDevices(devices: string) {
        this.devices = devices;
    }

    deleteDefinition() {
        this.delteGridRow('/configurator/delete_notification_definition/');
    }

    saveDefinition(dlg: wijmo.input.Popup) {
        this.saveGridRow('/configurator/save_notification_definition', dlg);
    }
}