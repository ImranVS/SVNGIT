import {Component, OnInit, ViewChild} from '@angular/core';
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
    selector: 'vs-hours-destinations',
    templateUrl: '/app/configurator/components/alert/hours-destinations.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class HoursDestinations extends GridBase implements OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    devices: string;
    errorMessage: string;
    sendtodata: any;
    escalatetodata: any;
    eventsdata: any;
    businesshours: any;
    sendvia: any;
    isSMS: boolean;
    isSNMP: boolean;
    isEmail: boolean;
    isScript: boolean;
    isWinLog: boolean;
    currentRow: any;

    formObject: any = {
        id: null,
        hours_destinations_id: null,
        business_hours_type: null,
        send_to: null,
        script_name: null,
        send_via: null,
        copy_to: null,
        persistent_notification: null,
        blind_copy_to: null
    };

    get pageSize(): number {
        return this.data.pageSize;
    }

    constructor(service: RESTService) {
        super(service);
        this.formName = "Hours and Destinations";
        this.sendvia = ["E-mail", "Script", "SMS", "SNMP Trap", "Windows Log"];
        this.isEmail = true;
    }

    ngOnInit() {
        this.service.get('/configurator/notifications_list')
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data[1]));
                //console.log(this.data);
            },
            (error) => this.errorMessage = <any>error
        );
        this.service.get('/configurator/get_business_hours?nameonly=true')
            .subscribe(
            (response) => {
                this.businesshours = response.data;
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

    saveHoursDestinations(dlg: wijmo.input.Popup) {
        var saveUrl = '/configurator/save_hours_destinations';
        if (this.formObject.hours_destinations_id == "") {
            //console.log('empty');
        }
        else {
            this.flex.collectionView.currentItem.business_hours_type = this.formObject.business_hours_type;
            this.flex.collectionView.currentItem.send_via = this.formObject.send_via;
            this.flex.collectionView.currentItem.send_to = this.formObject.send_to;
            this.flex.collectionView.currentItem.persistent_notification = this.formObject.persistent_notification;
            this.service.put(saveUrl, this.formObject)
                .subscribe(
                response => {
                    this.flex.collectionView.currentItem.id = response.data;
                }
            );
            (<wijmo.collections.CollectionView>this.flex.collectionView).commitEdit();
        }
        dlg.hide();
    }

    onSelectionChanged(event: wijmo.grid.CellRangeEventArgs) {

        this.getSendVia(event.panel.grid.selectedItems[0].send_via);

    }

    showEditForm(dlg: wijmo.input.Popup) {

        this.formTitle = "Edit " + this.formName;

        this.formObject.hours_destinations_id = this.flex.collectionView.currentItem.hours_destinations_id;
        this.formObject.business_hours_type = this.flex.collectionView.currentItem.business_hours_type;
        this.formObject.send_to = this.flex.collectionView.currentItem.send_to;
        this.formObject.script_name = this.flex.collectionView.currentItem.script_name;
        this.formObject.send_via = this.flex.collectionView.currentItem.send_via;
        this.formObject.copy_to = this.flex.collectionView.currentItem.copy_to;
        this.formObject.persistent_notification = this.flex.collectionView.currentItem.persistent_notification;
        this.formObject.blind_copy_to = this.flex.collectionView.currentItem.blind_copy_to;

        (<wijmo.collections.CollectionView>this.flex.collectionView).editItem(this.flex.collectionView.currentItem);
        this.showDialog(dlg);

    }

    onSendViaSelectedIndexChanged(event: wijmo.EventArgs) {
        
        this.getSendVia(this.formObject.send_via);

    }

    getSendVia(combotxt: string) {
        if (combotxt == "E-mail") {
            this.isEmail = true;
            this.isScript = false;
            this.isSMS = false;
            this.isSNMP = false;
            this.isWinLog = false;
        }
        else if (combotxt == "Script") {
            this.isEmail = false;
            this.isScript = true;
            this.isSMS = false;
            this.isSNMP = false;
            this.isWinLog = false;
            
        }
        else if (combotxt == "SMS") {
            this.isEmail = false;
            this.isScript = false;
            this.isSMS = true;
            this.isSNMP = false;
            this.isWinLog = false;
        }
        else if (combotxt == "SNMP Trap") {
            this.isEmail = false;
            this.isScript = false;
            this.isSMS = false;
            this.isSNMP = false;
            this.isWinLog = false;
        }
        else if (combotxt == "Windows Log") {
            this.isEmail = false;
            this.isScript = false;
            this.isSMS = false;
            this.isSNMP = false;
            this.isWinLog = false;
        }
    }
}