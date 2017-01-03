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
import {AppComponentService} from '../../../core/services';

@Component({
    selector: 'vs-escalation',
    templateUrl: '/app/configurator/components/alert/escalation.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class Escalation extends GridBase implements OnInit  {    
    devices: string;
    errorMessage: string;
    sendvia: any;
    isSMS: boolean;
    isEmail: boolean;
    isScript: boolean;
    key: string;
    formObject: any = {
        id: null,
        send_to: null,
        script_name: null,
        interval: null,
        send_via: null
    };

    get pageSize(): number {
        return this.data.pageSize;
    }

    constructor(service: RESTService, appComponentService: AppComponentService) {
        super(service, appComponentService);
        this.formName = "Escalation";
        this.sendvia = ["E-mail", "Script", "SMS"];
        this.isEmail = true;
    }

    ngOnInit() {
        this.service.get('/configurator/notifications_list')
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data[2]));
            },
            (error) => this.errorMessage = <any>error
        );
    }

    showEditForm(dlg: wijmo.input.Popup) {

        this.formTitle = "Edit " + this.formName;

        this.formObject.id = this.flex.collectionView.currentItem.id;
        this.formObject.send_to = this.flex.collectionView.currentItem.send_to;
        this.formObject.script_name = this.flex.collectionView.currentItem.script_name;
        this.formObject.send_via = this.flex.collectionView.currentItem.send_via;
        this.formObject.interval = this.flex.collectionView.currentItem.interval;

        (<wijmo.collections.CollectionView>this.flex.collectionView).editItem(this.flex.collectionView.currentItem);
        this.showDialog(dlg);

    }

    showAddForm(dlg: wijmo.input.Popup) {

        this.formTitle = "Add " + this.formName;

        this.formObject.id = "";
        this.formObject.send_to = "";
        this.formObject.script_name = "";
        this.formObject.send_via = "E-mail";
        this.formObject.interval = "";
        this.getSendVia(this.formObject.send_via);

        this.showDialog(dlg);

    }

    saveEscalation(dlg: wijmo.input.Popup) {
        var saveUrl = '/configurator/save_escalation';
        if (this.formObject.send_via == "E-mail") {
            this.flex.collectionView.currentItem.send_via = this.formObject.send_via;
            this.flex.collectionView.currentItem.send_to = this.formObject.send_to;
            this.flex.collectionView.currentItem.interval = this.formObject.interval;
        }
        else if (this.formObject.send_via == "SMS") {
            this.flex.collectionView.currentItem.send_via = this.formObject.send_via;
            this.flex.collectionView.currentItem.send_to = this.formObject.send_to;
            this.flex.collectionView.currentItem.interval = this.formObject.interval;
        }
        else if (this.formObject.send_via == "Script") {
            this.flex.collectionView.currentItem.send_via = this.formObject.send_via;
            this.flex.collectionView.currentItem.script_name = this.formObject.script_name;
            this.flex.collectionView.currentItem.interval = this.formObject.interval;
            //add script name and location
        }
        if (this.formObject.id == "") {
            this.service.put(saveUrl, this.formObject)
                .subscribe(
                response => {
                    if (response.status == "Success") {
                        this.data = response.data;
                        this.appComponentService.showSuccessMessage(response.message);
                    }
                    else {
                        this.appComponentService.showErrorMessage(response.message);
                    }
                }
            );
            this.flex.refresh();
        }
        else {
            this.service.put(saveUrl, this.formObject)
                .subscribe(
                response => {
                    if (response.status == "Success") {
                        this.flex.collectionView.currentItem.id = response.data;
                        this.appComponentService.showSuccessMessage(response.message);
                    }
                    else {
                        this.appComponentService.showErrorMessage(response.message);
                    }
                }
            );
            (<wijmo.collections.CollectionView>this.flex.collectionView).commitEdit();
        }
        dlg.hide();
    }

    changeInDevices(devices: string) {
        this.devices = devices;
    }

    onSendViaSelectedIndexChanged(event: wijmo.EventArgs) {

        this.getSendVia(this.formObject.send_via);

    }

    
    getSendVia(combotxt: string) {
        if (combotxt == "E-mail") {
            this.isEmail = true;
            this.isScript = false;
            this.isSMS = false;
        }
        else if (combotxt == "Script") {
            this.isEmail = false;
            this.isScript = true;
            this.isSMS = false;
        }
        else if (combotxt == "SMS") {
            this.isEmail = false;
            this.isScript = false;
            this.isSMS = true;
        }
    }

    deleteEscalate() {
        let deleteUrl = '/configurator/delete_hours_destinations/';
        this.key = this.flex.collectionView.currentItem.id;
        if (confirm("Are you sure want to delete this record?")) {
            this.service.delete(deleteUrl + this.key)
                .subscribe(
                response => {
                    if (response.status == "Success") {
                        this.appComponentService.showSuccessMessage(response.message);
                    } else {
                        this.appComponentService.showErrorMessage(response.message);
                    }

                }, error => {
                    var errorMessage = <any>error;
                    this.appComponentService.showErrorMessage(errorMessage);
                });
            (<wijmo.collections.CollectionView>this.flex.collectionView).remove(this.flex.collectionView.currentItem);
        }
    }
}