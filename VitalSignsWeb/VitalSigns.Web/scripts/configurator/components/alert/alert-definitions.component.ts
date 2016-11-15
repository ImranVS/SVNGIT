import {Component, OnInit, ViewChild, Input} from '@angular/core';
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
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;  
    @ViewChild('flex3') flex3: wijmo.grid.FlexGrid;  
    @ViewChild('flex4') flex4: wijmo.grid.FlexGrid;  
    @ViewChild('flex5') flex5: wijmo.grid.FlexGrid;  
    devices: string;
    errorMessage: string;
    eventsdata: any;
    hoursdata: any;
    escalationdata: any;
    selected_hours: string[] = [];
    selected_escalation: string[] = [];
    selected_events: string[] = [];
    formObject: any = {
        id: null,
        alert_name: null,
        hours_destinations_ids: null,
        escalation_ids: null,
        event_ids: null,
        server_ids: null
    };
    @Input() public set hoursList(val: string) {
        this.selected_hours = [];
    }
    @Input() public set escalationList(val: string) {
        this.selected_escalation = [];
    }
    @Input() public set eventsList(val: string) {
        this.selected_events = [];
    }

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
                //console.log(data.data[0]);
                this.data.pageSize = 20;
                var groupDesc = new wijmo.collections.PropertyGroupDescription('notification_name');
                this.data.groupDescriptions.push(groupDesc);
                this.hoursdata = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data[1]));
                this.escalationdata = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data[2]));
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
            this.selected_events.push(value);
        }
        else {
            this.selected_events.splice(this.selected_events.indexOf(value), 1);
        }
    }

    escalationCheck(value, event) {
        if (event.target.checked) {
            this.selected_escalation.push(value);
        }
        else {
            this.selected_escalation.splice(this.selected_escalation.indexOf(value), 1);
        }
    }

    hoursCheck(value, event) {
        if (event.target.checked) {
            this.selected_hours.push(value);
        }
        else {
            this.selected_hours.splice(this.selected_hours.indexOf(value), 1);
        }
    }

    changeInDevices(devices: string) {
        this.devices = devices;
    }

    deleteDefinition() {
        this.delteGridRow('/configurator/delete_notification_definition/');
    }

    showEditForm(dlg: wijmo.input.Popup) {

        this.formTitle = "Edit " + this.formName;
        
        this.formObject.alert_name = this.flex.collectionView.currentItem.notification_name;
        this.formObject.hours_destinations_ids = this.flex.collectionView.currentItem.hours_destinations_ids;
        this.formObject.escalation_ids = this.flex.collectionView.currentItem.escalation_ids;
        this.formObject.event_ids = this.flex.collectionView.currentItem.event_ids;
        this.formObject.server_ids = this.flex.collectionView.currentItem.server_ids;

        (<wijmo.collections.CollectionView>this.flex.collectionView).editItem(this.flex.collectionView.currentItem);
        this.showDialog(dlg);

    }

    saveDefinition(dlg: wijmo.input.Popup) {
        this.errorMessage = "";
        var selected_hours = this.selected_hours;
        var selected_escalation = this.selected_escalation;
        var selected_events = this.selected_events;
        //var selected_servers: Server[] = [];
        //for (var _i = 0; _i < this.flex.collectionView.sourceCollection.length; _i++) {
        //    var item = (<wijmo.collections.CollectionView>this.flex.collectionView.sourceCollection)[_i];
        //    if (item.is_selected) {
        //        var serverObject = new Server();
        //        serverObject.device_type = item.device_type;
        //        serverObject.device_name = item.device_name;
        //        selected_servers.push(serverObject);
        //    }
        //}
        console.log(this.selected_events);
        if (this.selected_events.length == 0 || this.selected_hours.length == 0) {
            this.errorMessage = "No selection made.";
        }
        
        if (!this.errorMessage) {

            console.log("saving");

            //this.service.put('/configurator/save_notification_definition', { selected_hours, selected_escalation, selected_events })
            //    .subscribe(
            //    response => {
            //        this.flex.collectionView.currentItem.id = response.data;
            //    });
        }
    }
}
