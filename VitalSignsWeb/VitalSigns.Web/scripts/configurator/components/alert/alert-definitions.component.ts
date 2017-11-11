import { Component, OnInit, ViewChild, Input, Output, ElementRef} from '@angular/core';
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
    @ViewChild('serverFlex') serverFlex: wijmo.grid.FlexGrid;  
    @ViewChild('myModal') myModal: ElementRef;  
    errorMessage: string;
    serversdata: wijmo.collections.CollectionView;
    eventsdata: wijmo.collections.CollectionView;
    hoursdata: wijmo.collections.CollectionView;
    escalationdata: wijmo.collections.CollectionView;
    devicedata: wijmo.collections.CollectionView;
    serverGridData: wijmo.collections.CollectionView;
    selected_hours: string[] = [];
    selected_escalation: string[] = [];
    selected_events: string[] = [];
    devices: string[] = [];
    _deviceList: any;
    checkedDevices: any;

    formObject: any = {
        id: null,
        notification_name: null,
        send_to: null,
        business_hours_ids: null,
        event_ids: null,
        server_ids: null,
        is_selected_hour: null,
        is_selected_event: null,
        servers: null,
        is_selected_server: null,
        collection_names: null,
        server_objects: null
    };

    refreshCheckedEvents() {
        if (this.flex3.collectionView) {
            if (this.flex3.collectionView.items.length > 0) {
                for (var _i = 0; _i < this.flex3.collectionView.sourceCollection.length; _i++) {
                    var item = (<wijmo.collections.CollectionView>this.flex3.collectionView.sourceCollection)[_i];
                    if (item.is_selected_event)
                        this.formObject.event_ids.push(item.id);
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
                for (var _i = 0; _i < this.flex4.collectionView.sourceCollection.length; _i++) {
                    var item = (<wijmo.collections.CollectionView>this.flex4.collectionView.sourceCollection)[_i];
                    console.log(item);
                    if (item.is_selected_hour) {
                        this.formObject.selected_hours.push(item.id);
                    }
                }
            }
        }
    }

    refreshCheckedEscalation() {
        if (this.flex5.collectionView) {
            if (this.flex5.collectionView.items.length > 0) {
                for (var _i = 0; _i < this.flex5.collectionView.sourceCollection.length; _i++) {
                    var item = (<wijmo.collections.CollectionView>this.flex5.collectionView.sourceCollection)[_i];
                    if (item.is_selected_hour)
                        this.formObject.selected_hours.push(item.id);
                }
            }
        }
    }

    refreshCheckedDevices() {

        if (this.serverFlex.collectionView) {
            if (this.serverFlex.collectionView.items.length > 0) {
                for (var _i = 0; _i < this.serverFlex.collectionView.sourceCollection.length; _i++) {
                    var item = (<wijmo.collections.CollectionView>this.serverFlex.collectionView.sourceCollection)[_i];
                    console.log(item);
                    if (item.is_selected)
                        this.formObject.server_objects.push({ device_id: item.device_id, collection_name: item.collection_name });
                }
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
        this.service.get(`/configurator/notifications_list`)
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data[0]));
                this.hoursdata = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data[1]));
                this.escalationdata = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data[2]));
                this.eventsdata = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data[3]));
                this.serverGridData = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data[4]));
                this.serverGridData.groupDescriptions.push(new wijmo.collections.PropertyGroupDescription('location_name'));
                var groupDesc = new wijmo.collections.PropertyGroupDescription('device_type');
                this.eventsdata.groupDescriptions.push(groupDesc);
            },
            (error) => this.errorMessage = <any>error
        );
    }
 

    deleteDefinition() {
        this.deleteGridRow('/configurator/delete_notification_definition/');
    }

    showEditForm(dlg: wijmo.input.Popup) {
        this.errorMessage = "";
        this.formTitle = "Edit " + this.formName;
        this.formObject.id = this.flex.collectionView.currentItem.id;
        this.formObject.notification_name = this.flex.collectionView.currentItem.notification_name;
        this.formObject.server_objects = this.flex.collectionView.currentItem.server_objects;
        this.formObject.selected_hours = this.flex.collectionView.currentItem.selected_hours;
        this.formObject.event_ids = this.flex.collectionView.currentItem.event_ids;
        if (this.flex3.collectionView) {
            if (this.flex3.collectionView.items.length > 0) {
                for (var _i = 0; _i < this.flex3.collectionView.sourceCollection.length; _i++) {
                    var item = (<wijmo.collections.CollectionView>this.flex3.collectionView.sourceCollection)[_i];
                    item.is_selected_event = this.formObject.event_ids.indexOf(item.id) != -1;
                }
            }
        }
        this.formObject.is_selected_event = this.flex.collectionView.currentItem.is_selected_event;
        if (this.flex4.collectionView) {
            if (this.flex4.collectionView.items.length > 0) {
                for (var _i = 0; _i < this.flex4.collectionView.sourceCollection.length; _i++) {
                    var item = (<wijmo.collections.CollectionView>this.flex4.collectionView.sourceCollection)[_i];
                    item.is_selected_hour = this.formObject.selected_hours.indexOf(item.id) != -1;
                }
            }
        }
        if (this.flex5.collectionView) {
            if (this.flex5.collectionView.items.length > 0) {
                for (var _i = 0; _i < this.flex5.collectionView.sourceCollection.length; _i++) {
                    var item = (<wijmo.collections.CollectionView>this.flex5.collectionView.sourceCollection)[_i];
                    item.is_selected_hour = this.formObject.selected_hours.indexOf(item.id) != -1;
                }
            }
        }

        if (this.serverFlex.collectionView) {
            if (this.serverFlex.collectionView.items.length > 0) {
                for (var _i = 0; _i < this.serverFlex.collectionView.sourceCollection.length; _i++) {
                    var item = (<wijmo.collections.CollectionView>this.serverFlex.collectionView.sourceCollection)[_i];
                    item.is_selected = this.formObject.server_objects.filter(x => x.device_id == item.device_id && x.collection_name == item.collection_name).length >= 1;
                }
            }
        }
        
        this.showDialog(dlg);

    }

    showAddForm(dlg: wijmo.input.Popup) {
        this.errorMessage = "";
        this.formTitle = "Add " + this.formName;
        this.formObject.id = "";
        //this.flex.collectionView.currentItem.notification_name = "";
        this.formObject.notification_name = "";
        //if (this.flex.collectionView.items.length != 0) {
        //    console.log("Wes 1")
        //    this.formObject.send_to = this.flex.collectionView.currentItem.send_to;
        //    this.formObject.business_hours_ids = this.flex.collectionView.currentItem.business_hours_ids;
        //    this.formObject.is_selected_hour = this.flex.collectionView.currentItem.is_selected_hour;
        //    this.formObject.event_ids = this.flex.collectionView.currentItem.event_ids;
        //    this.formObject.is_selected_event = this.flex.collectionView.currentItem.is_selected_event;
        //    this.formObject.server_ids = this.flex.collectionView.currentItem.server_ids;
        //    this.formObject.is_selected_server = this.flex.collectionView.currentItem.is_selected_server;
        //}
        //else {
        console.log("Wes 2")
        this.formObject.business_hours_ids = [];
        this.formObject.is_selected_hour = [];
        this.formObject.event_ids = [];
        this.formObject.is_selected_event = [];
        this.formObject.server_ids = [];
        this.formObject.is_selected_server = [];
        //if (this.flex6.collectionView) {
        //    console.log("Wes 3")
        //    if (this.flex6.collectionView.items.length > 0) {
        //        console.log("Wes 4")
        //        this.formObject.business_hours_ids = this.flex6.collectionView.currentItem.business_hours_ids;
        //        this.formObject.is_selected_hour = this.flex6.collectionView.currentItem.is_selected_hour;
        //        this.formObject.event_ids = this.flex6.collectionView.currentItem.event_ids;
        //        this.formObject.is_selected_event = this.flex6.collectionView.currentItem.is_selected_event;
        //        this.formObject.server_ids = this.flex6.collectionView.currentItem.server_ids;
        //        this.formObject.is_selected_server = this.flex6.collectionView.currentItem.is_selected_server;
        //    }
        //}
        //}
        this.devices = [];
        if (this.flex3.collectionView) {
            console.log("Wes 5")
            if (this.flex3.collectionView.items.length > 0) {
                console.log("Wes 6")
                //(<wijmo.collections.CollectionView>this.flex3.collectionView.sourceCollection).moveToFirstPage();
                for (var _i = 0; _i < this.flex3.collectionView.sourceCollection.length; _i++) {
                    console.log("Wes 7")
                    var item = (<wijmo.collections.CollectionView>this.flex3.collectionView.sourceCollection)[_i];
                    item.is_selected_event = false;
                }
            }
        }
        if (this.flex4.collectionView) {
            console.log("Wes 8")
            if (this.flex4.collectionView.items.length > 0) {
                console.log("Wes 9")
                //(<wijmo.collections.CollectionView>this.flex4.collectionView.sourceCollection).moveToFirstPage();
                for (var _i = 0; _i < this.flex4.collectionView.sourceCollection.length; _i++) {
                    console.log("Wes 10")
                    var item = (<wijmo.collections.CollectionView>this.flex4.collectionView.sourceCollection)[_i];
                    item.is_selected_hour = false;
                }
            }
        }
        if (this.flex5.collectionView) {
            console.log("Wes 11")
            if (this.flex5.collectionView.items.length > 0) {
                console.log("Wes 12")
                //(<wijmo.collections.CollectionView>this.flex5.collectionView.sourceCollection).moveToFirstPage();
                for (var _i = 0; _i < this.flex5.collectionView.sourceCollection.length; _i++) {
                    console.log("Wes 13")
                    var item = (<wijmo.collections.CollectionView>this.flex5.collectionView.sourceCollection)[_i];
                    item.is_selected_hour = false;
                }
            }
        }

        if (this.serverFlex.collectionView) {
            console.log("Wes 14")
            if (this.serverFlex.collectionView.items.length > 0) {
                console.log("Wes 15")
                //(<wijmo.collections.CollectionView>this.flex5.collectionView.sourceCollection).moveToFirstPage();
                for (var _i = 0; _i < this.serverFlex.collectionView.sourceCollection.length; _i++) {
                    console.log("Wes 16")
                    var item = (<wijmo.collections.CollectionView>this.serverFlex.collectionView.sourceCollection)[_i];
                    item.is_selected = false;
                }
            }
        }

        //if (this.flex.collectionView.items.length != 0) {
        //    this.serverGridData = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(this.flex.collectionView.currentItem.server_objects));
        //    this.serverGridData.groupDescriptions.push(new wijmo.collections.PropertyGroupDescription('location_name'));

        //    this.formObject.server_objects = this.serverGridData;
        //}
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
        this.formObject.server_objects = [];
        this.formObject.selected_hours = [];
        this.formObject.event_ids = [];
        this.refreshCheckedHours();
        this.refreshCheckedEscalation();
        this.refreshCheckedEvents();
        this.refreshCheckedDevices();
        
        console.log(this.formObject.server_objects.length);
        console.log(this.formObject.selected_hours.length);
        console.log(this.formObject.event_ids.length);
        if (this.formObject.server_objects.length == 0 || this.formObject.selected_hours.length == 0 || this.formObject.event_ids.length == 0) {
            this.errorMessage = "No selection made. Please select at least one Hours and Destinations entry, one Events entry, and one Devices entry.";
        }
        if (!this.errorMessage) {
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
            if (this.formObject.id == "") {
                (<wijmo.collections.CollectionView>this.flex.collectionView).commitNew();
            } else {
                (<wijmo.collections.CollectionView>this.flex.collectionView).commitEdit();
            }        
            this.flex.refresh();
            dlg.hide();
        }
    }

    selectAll() {
        for (var _i = 0; _i < this.flex3.collectionView.sourceCollection.length; _i++) {
            var item = (<wijmo.collections.CollectionView>this.flex3.collectionView.sourceCollection)[_i];
            item.is_selected_event = true;
            //if (item.hasClass("wijmo-wijgrid-datarow")) {
                
            //}
            
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

    cancelEditAdd() {
        //this.myModal.nativeElement.className = 'modal fade';
    }

    selectAllServers() {
        for (var _i = 0; _i < this.serverFlex.collectionView.sourceCollection.length; _i++) {
            var item = (<wijmo.collections.CollectionView>this.serverFlex.collectionView.sourceCollection)[_i];
            item.is_selected = true;
        }
        this.serverFlex.refresh();
    }

    deselectAllServers() {
        for (var _i = 0; _i < this.serverFlex.collectionView.sourceCollection.length; _i++) {
            var item = (<wijmo.collections.CollectionView>this.serverFlex.collectionView.sourceCollection)[_i];
            item.is_selected = false;
        }
        this.serverFlex.refresh();
    }
}
