import {Component, OnInit, ViewChild, Input, Output} from '@angular/core';
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
import { AppComponentService } from '../../../core/services';
import { ServersLocationService } from '../serverSettings/serverattributes-view.service';

@Component({
    selector: 'vs-notification-definitions',
    templateUrl: '/app/configurator/components/alert/alert-definitions.component.html',
    providers: [
        HttpModule,
        RESTService,
        ServersLocationService
    ]
})
export class AlertDefinitions extends GridBase implements OnInit  {  
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;  
    @ViewChild('flex3') flex3: wijmo.grid.FlexGrid;  
    @ViewChild('flex4') flex4: wijmo.grid.FlexGrid;  
    @ViewChild('flex5') flex5: wijmo.grid.FlexGrid;  
    @ViewChild('flex6') flex6: wijmo.grid.FlexGrid;  
    errorMessage: string;
    serversdata: wijmo.collections.CollectionView;
    eventsdata: wijmo.collections.CollectionView;
    hoursdata: wijmo.collections.CollectionView;
    escalationdata: wijmo.collections.CollectionView;
    devicedata: wijmo.collections.CollectionView;
    selected_hours: string[] = [];
    selected_escalation: string[] = [];
    selected_events: string[] = [];
    devices: string[] = [];
    _deviceList: any;
    checkedDevices: any;
    selDeviceTypes: string = "Domino,Sametime,URL,WebSphere,IBM Connections";

    formObject: any = {
        id: null,
        notification_name: null,
        send_to: null,
        business_hours_ids: null,
        event_ids: null,
        server_ids: null,
        is_selected_hour: null,
        is_selected_event: null,
        is_selected_server: null
    };

    refreshCheckedEvents() {
        if (this.flex3.collectionView) {
            if (this.flex3.collectionView.items.length > 0) {
                //(<wijmo.collections.CollectionView>this.flex3.collectionView.sourceCollection).moveToFirstPage();
                for (var _i = 0; _i < this.flex3.collectionView.sourceCollection.length; _i++) {
                    var item = (<wijmo.collections.CollectionView>this.flex3.collectionView.sourceCollection)[_i];
                    var val = this.selected_events.filter((record) => record == item.id);
                    if (item.is_selected_event && val.length == 0) {
                        this.selected_events.push(item.id);
                    }
                    if (this.flex.collectionView.items.length > 0) {
                        var val2 = this.flex.collectionView.currentItem.event_ids.filter((record) => record == item.id);
                        if (val2.length != 0) {
                            var ind2 = this.flex.collectionView.currentItem.event_ids.indexOf(val2[0]);
                            if (ind2 != -1) {
                                this.flex.collectionView.currentItem.is_selected_event[ind2] = item.is_selected_event;
                            }
                        }
                    }
                }
            }
        }
    }

    onPageClickPrev() {
        this.eventsdata.moveToPreviousPage();
        this.flex3.refresh();
    }

    onPageClickNext() {
        this.eventsdata.moveToNextPage();
        this.flex3.refresh();
    }

    refreshCheckedHours() {
        if (this.flex4.collectionView) {
            if (this.flex4.collectionView.items.length > 0) {
                //(<wijmo.collections.CollectionView>this.flex4.collectionView.sourceCollection).moveToFirstPage();
                for (var _i = 0; _i < this.flex4.collectionView.sourceCollection.length; _i++) {
                    var item = (<wijmo.collections.CollectionView>this.flex4.collectionView.sourceCollection)[_i];
                    var val = this.selected_hours.filter((record) => record == item.id);
                    if (item.is_selected_hour && val.length == 0) {
                        this.selected_hours.push(item.id);
                    }
                    if (this.flex.collectionView.items.length > 0) {
                        var val2 = this.flex.collectionView.currentItem.business_hours_ids.filter((record) => record == item.id);
                        if (val2.length != 0) {
                            var ind2 = this.flex.collectionView.currentItem.business_hours_ids.indexOf(val2[0]);
                            if (ind2 != -1) {
                                this.flex.collectionView.currentItem.is_selected_hour[ind2] = item.is_selected_hour;
                            }
                        }
                    }  
                }
            }
        }
    }

    refreshCheckedEscalation() {
        if (this.flex5.collectionView) {
            if (this.flex5.collectionView.items.length > 0) {
                //(<wijmo.collections.CollectionView>this.flex5.collectionView.sourceCollection).moveToFirstPage();
                for (var _i = 0; _i < this.flex5.collectionView.sourceCollection.length; _i++) {
                    var item = (<wijmo.collections.CollectionView>this.flex5.collectionView.sourceCollection)[_i];
                    var ind = this.selected_escalation.filter((record) => record == item.id);
                    if (item.is_selected_hour && ind.length == 0) {
                        this.selected_escalation.push(item.id)
                    }
                    if (this.flex.collectionView.items.length > 0) {
                        var val2 = this.flex.collectionView.currentItem.business_hours_ids.filter((record) => record == item.id);
                        if (val2.length != 0) {
                            var ind2 = this.flex.collectionView.currentItem.business_hours_ids.indexOf(val2[0]);
                            if (ind2 != -1) {
                                this.flex.collectionView.currentItem.is_selected_hour[ind2] = item.is_selected_hour;
                            }
                        }
                    }
                }
            }
        }
    }

    refreshCheckedDevices() {
        for (var _i = 0; _i < this.formObject.server_ids.length; _i++) {
            this.formObject.is_selected_server[_i] = false;
        }
        for (var _j = 0; _j < this.devices.length; _j++) {
            var item = this.devices[_j];
            var ind2 = this.formObject.server_ids.indexOf(item);
            if (ind2 != -1) {
                this.formObject.is_selected_server[ind2] = true;
            }
        }
    }

    get pageSize(): number {
        return this.data.pageSize;
    }

    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
        }
    }

    get eventsPageSize(): number {
        return this.eventsdata.pageSize;
    }

    set eventsPageSize(value: number) {
        if (this.eventsdata.pageSize != value) {
            this.eventsdata.pageSize = value;
            this.eventsdata.refresh();
        }
    }

    constructor(service: RESTService, appComponentService: AppComponentService) {
        super(service, appComponentService);
        this.formName = "Notification Definition";
    }

    ngOnInit() {
        this.service.get('/configurator/notifications_list')
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data[0]));
                //var groupDesc = new wijmo.collections.PropertyGroupDescription('notification_name');
                //this.data.groupDescriptions.push(groupDesc);
                this.hoursdata = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data[1]));
                this.escalationdata = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data[2]));
                this.eventsdata = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data[3]));
                this.serversdata = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data[4]));
                var groupDesc = new wijmo.collections.PropertyGroupDescription('device_type');
                this.eventsdata.groupDescriptions.push(groupDesc);
                //this.eventsdata.pageSize = 10;
            },
            (error) => this.errorMessage = <any>error
        );
    }
 

    deleteDefinition() {
        this.delteGridRow('/configurator/delete_notification_definition/');
    }

    showEditForm(dlg: wijmo.input.Popup) {
        this.errorMessage = "";
        this.formTitle = "Edit " + this.formName;
        this.formObject.id = this.flex.collectionView.currentItem.id;
        this.formObject.notification_name = this.flex.collectionView.currentItem.notification_name;
        this.formObject.send_to = this.flex.collectionView.currentItem.send_to;
        this.formObject.business_hours_ids = this.flex.collectionView.currentItem.business_hours_ids;
        this.formObject.event_ids = this.flex.collectionView.currentItem.event_ids;
        this.devices = [];
        if (this.flex3.collectionView) {
            if (this.flex3.collectionView.items.length > 0) {
                //(<wijmo.collections.CollectionView>this.flex3.collectionView.sourceCollection).moveToFirstPage();
                for (var _i = 0; _i < this.flex3.collectionView.sourceCollection.length; _i++) {
                    var item = (<wijmo.collections.CollectionView>this.flex3.collectionView.sourceCollection)[_i];
                    if (this.formObject.event_ids) {
                        var value = this.formObject.event_ids.filter((record) => record == item.id);
                        if (value.length > 0) {
                            var ind = this.formObject.event_ids.indexOf(value[0]);
                            if (ind != -1) {
                                item.is_selected_event = this.flex.collectionView.currentItem.is_selected_event[ind];
                            }
                        }
                        else {
                            item.is_selected_event = false;
                        }
                    }
                    else {
                        item.is_selected_event = false;
                    }

                }
            }
        }
        this.formObject.is_selected_event = this.flex.collectionView.currentItem.is_selected_event;
        if (this.flex4.collectionView) {
            if (this.flex4.collectionView.items.length > 0) {
                //(<wijmo.collections.CollectionView>this.flex4.collectionView.sourceCollection).moveToFirstPage();
                for (var _i = 0; _i < this.flex4.collectionView.sourceCollection.length; _i++) {
                    var item = (<wijmo.collections.CollectionView>this.flex4.collectionView.sourceCollection)[_i];
                    if (this.formObject.business_hours_ids) {
                        var value = this.formObject.business_hours_ids.filter((record) => record == item.id);
                        if (value.length > 0) {
                            var ind = this.formObject.business_hours_ids.indexOf(value[0]);
                            if (ind != -1) {
                                item.is_selected_hour = this.flex.collectionView.currentItem.is_selected_hour[ind];
                            }
                        }
                        else {
                            item.is_selected_hour = false;
                        }
                    }
                    else {
                        item.is_selected_hour = false;
                    }

                }
            }
        }
        if (this.flex5.collectionView) {
            if (this.flex5.collectionView.items.length > 0) {
                //(<wijmo.collections.CollectionView>this.flex5.collectionView.sourceCollection).moveToFirstPage();
                for (var _i = 0; _i < this.flex5.collectionView.sourceCollection.length; _i++) {
                    var item = (<wijmo.collections.CollectionView>this.flex5.collectionView.sourceCollection)[_i];
                    if (this.formObject.business_hours_ids) {
                        var value = this.formObject.business_hours_ids.filter((record) => record == item.id);
                        if (value.length > 0) {
                            var ind = this.formObject.business_hours_ids.indexOf(value[0]);
                            if (ind != -1) {
                                item.is_selected_hour = this.flex.collectionView.currentItem.is_selected_hour[ind];
                            }
                        }
                        else {
                            item.is_selected_hour = false;
                        }
                    }
                    else {
                        item.is_selected_hour = false;
                    }

                }
            }
        }
        this.formObject.is_selected_hour = this.flex.collectionView.currentItem.is_selected_hour;
        this.formObject.server_ids = this.flex.collectionView.currentItem.server_ids;
        this.formObject.is_selected_server = this.flex.collectionView.currentItem.is_selected_server;
        for (var _i = 0; _i < this.flex.collectionView.currentItem.is_selected_server.length; _i++) {
            var item = this.formObject.is_selected_server[_i];
            if (item) {
                var item2 = this.formObject.server_ids[_i];
                this.devices.push(item2);
            }
            else {
            }
        }
        this._deviceList = this.devices;
        this.checkedDevices = this._deviceList;

        this.showDialog(dlg);

    }

    showAddForm(dlg: wijmo.input.Popup) {
        this.errorMessage = "";
        this.formTitle = "Add " + this.formName;
        this.formObject.id = "";
        //this.flex.collectionView.currentItem.notification_name = "";
        this.formObject.notification_name = "";
        if (this.flex.collectionView.items.length != 0) {
            this.formObject.send_to = this.flex.collectionView.currentItem.send_to;
            this.formObject.business_hours_ids = this.flex.collectionView.currentItem.business_hours_ids;
            this.formObject.is_selected_hour = this.flex.collectionView.currentItem.is_selected_hour;
            this.formObject.event_ids = this.flex.collectionView.currentItem.event_ids;
            this.formObject.is_selected_event = this.flex.collectionView.currentItem.is_selected_event;
            this.formObject.server_ids = this.flex.collectionView.currentItem.server_ids;
            this.formObject.is_selected_server = this.flex.collectionView.currentItem.is_selected_server;
        }
        else {
            if (this.flex6.collectionView) {
                if (this.flex6.collectionView.items.length > 0) {
                    this.formObject.business_hours_ids = this.flex6.collectionView.currentItem.business_hours_ids;
                    this.formObject.is_selected_hour = this.flex6.collectionView.currentItem.is_selected_hour;
                    this.formObject.event_ids = this.flex6.collectionView.currentItem.event_ids;
                    this.formObject.is_selected_event = this.flex6.collectionView.currentItem.is_selected_event;
                    this.formObject.server_ids = this.flex6.collectionView.currentItem.server_ids;
                    this.formObject.is_selected_server = this.flex6.collectionView.currentItem.is_selected_server;
                }
            }
        }
        this.devices = [];
        if (this.flex3.collectionView) {
            if (this.flex3.collectionView.items.length > 0) {
                //(<wijmo.collections.CollectionView>this.flex3.collectionView.sourceCollection).moveToFirstPage();
                for (var _i = 0; _i < this.flex3.collectionView.sourceCollection.length; _i++) {
                    var item = (<wijmo.collections.CollectionView>this.flex3.collectionView.sourceCollection)[_i];
                    item.is_selected_event = false;
                }
            }
        }
        if (this.flex4.collectionView) {
            if (this.flex4.collectionView.items.length > 0) {
                //(<wijmo.collections.CollectionView>this.flex4.collectionView.sourceCollection).moveToFirstPage();
                for (var _i = 0; _i < this.flex4.collectionView.sourceCollection.length; _i++) {
                    var item = (<wijmo.collections.CollectionView>this.flex4.collectionView.sourceCollection)[_i];
                    item.is_selected_hour = false;
                }
            }
        }
        if (this.flex5.collectionView) {
            if (this.flex5.collectionView.items.length > 0) {
                //(<wijmo.collections.CollectionView>this.flex5.collectionView.sourceCollection).moveToFirstPage();
                for (var _i = 0; _i < this.flex5.collectionView.sourceCollection.length; _i++) {
                    var item = (<wijmo.collections.CollectionView>this.flex5.collectionView.sourceCollection)[_i];
                    item.is_selected_hour = false;
                }
            }
        }
        this.devices = [];
        this._deviceList = this.devices;
        this.checkedDevices = this._deviceList;
        this.showDialog(dlg);

    }

    changeInDevices(devices: any) {
        this.devices = devices;
        this.checkedDevices = devices;
    }

    saveDefinition(dlg: wijmo.input.Popup) {
        this.errorMessage = "";
        this.refreshCheckedHours();
        this.refreshCheckedEscalation();
        this.refreshCheckedEvents();
        this.refreshCheckedDevices();
        var selected_servers = this.checkedDevices;
        if (this.selected_events.length == 0 || this.selected_hours.length == 0 || selected_servers.length == 0) {
            this.errorMessage = "No selection made. Please select at least one Hours and Destinations entry, one Events entry, and one Devices entry.";
        }
        if (!this.errorMessage) {
            if (this.formObject.id == "") {
                this.service.put('/configurator/save_notification_definition', this.formObject)
                    .subscribe(
                    response => {
                        if (response.status == "Success") {
                            this.data = response.data[0];
                            this.appComponentService.showSuccessMessage(response.message);
                        }
                        else {
                            this.appComponentService.showErrorMessage(response.message);
                        }
                    });
                (<wijmo.collections.CollectionView>this.flex.collectionView).commitNew();
            }
            else {
                this.service.put('/configurator/save_notification_definition', this.formObject)
                    .subscribe(
                    response => {
                        if (response.status == "Success") {
                            this.data = response.data[0];
                            this.appComponentService.showSuccessMessage(response.message);
                        }
                        else {
                            this.appComponentService.showErrorMessage(response.message);
                        }
                    });
                (<wijmo.collections.CollectionView>this.flex.collectionView).commitEdit();
            }
            //(<wijmo.collections.CollectionView>this.flex3.collectionView.sourceCollection).moveToFirstPage();           
            this.flex.refresh();
            this._deviceList = [];
            dlg.hide();
        }
    }

    selectAll() {
        for (var _i = 0; _i < this.flex3.collectionView.sourceCollection.length; _i++) {
            var item = (<wijmo.collections.CollectionView>this.flex3.collectionView.sourceCollection)[_i];
            item.is_selected_event = true;
            //var val = this.selected_events.filter((record) => record == item.id);
            //if (val.length == 0) {
            //    this.selected_events.push(item.id);
            //}
        }
        this.flex3.refresh();
    }

    deselectAll() {
        for (var _i = 0; _i < this.flex3.collectionView.sourceCollection.length; _i++) {
            var item = (<wijmo.collections.CollectionView>this.flex3.collectionView.sourceCollection)[_i];
            item.is_selected_event = false;
            //var val = this.selected_events.filter((record) => record == item.id);
            //if (val.length != 0) {
            //    this.selected_events.splice(this.selected_events.indexOf(item.id), 1);
            //}
        }
        this.flex3.refresh();
    }

    collapse(flex) {
        flex.collapseGroupsToLevel(0);
    }

    expand(flex) {
        var rows = flex.rows;
        for (var rowIdx = 0; rowIdx < rows.length; rowIdx++) {
            var rootRow = rows[rowIdx];
            if (rootRow.hasChildren) { rootRow.isCollapsed = false; }
        }
    }
}
