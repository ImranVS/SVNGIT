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
import {AppComponentService} from '../../../core/services';


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
    scripts: any;
    urls: any;
    isSMS: boolean;
    isSNMP: boolean;
    isEmail: boolean;
    isScript: boolean;
    isUrl: boolean;
    isWinLog: boolean;
    currentRow: any;
    key: string;
    formObject: any = {
        id: null,
        business_hours_type: null,
        send_to: null,
        script_name: null,
        send_via: null,
        copy_to: null,
        persistent_notification: null,
        blind_copy_to: null,
        scripts: null,
        urls: null
    };

    get pageSize(): number {
        return this.data.pageSize;
    }

    constructor(service: RESTService, appComponentService: AppComponentService) {
        super(service, appComponentService);
        this.formName = "Hours and Destinations";
        this.sendvia = ["E-mail", "Script", "SMS", "SNMP Trap", "URL", "Windows Log"];
        this.isEmail = true;
    }

    ngOnInit() {
        this.service.get('/configurator/notifications_list')
            .subscribe(
            (data) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(data.data[1]));
            },
            (error) => this.errorMessage = <any>error
        );
        this.service.get('/configurator/get_business_hours?nameonly=true')
            .subscribe(
            (response) => {
                this.businesshours = response.data[0];
            },
            (error) => this.errorMessage = <any>error
        );
        this.service.get('/configurator/get_scripts?isCombo=true')
            .subscribe(
            (response) => {
                this.scripts = response.data;
            },
            (error) => this.errorMessage = <any>error
            );
        this.service.get('/configurator/get_alert_urls?isCombo=true')
            .subscribe(
            (response) => {
                this.urls = response.data;
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
        if (this.flex.collectionView) {
            if (this.flex.collectionView.items.length > 0) {
                if (this.formObject.send_via == "E-mail") {
                    this.flex.collectionView.currentItem.business_hours_type = this.formObject.business_hours_type;
                    this.flex.collectionView.currentItem.send_via = this.formObject.send_via;
                    this.flex.collectionView.currentItem.send_to = this.formObject.send_to;
                    this.flex.collectionView.currentItem.copy_to = this.formObject.copy_to;
                    this.flex.collectionView.currentItem.blind_copy_to = this.formObject.blind_copy_to;
                    this.flex.collectionView.currentItem.persistent_notification = this.formObject.persistent_notification;
                }
                else if (this.formObject.send_via == "SMS") {
                    this.flex.collectionView.currentItem.business_hours_type = this.formObject.business_hours_type;
                    this.flex.collectionView.currentItem.send_via = this.formObject.send_via;
                    this.flex.collectionView.currentItem.send_to = this.formObject.send_to;
                }
                else if (this.formObject.send_via == "SNMP Trap" || this.formObject.send_via == "Windows Log") {
                    this.flex.collectionView.currentItem.business_hours_type = this.formObject.business_hours_type;
                    this.flex.collectionView.currentItem.send_via = this.formObject.send_via;
                }
                else if (this.formObject.send_via == "Script") {
                    this.flex.collectionView.currentItem.business_hours_type = this.formObject.business_hours_type;
                    this.flex.collectionView.currentItem.send_via = this.formObject.send_via;
                    this.flex.collectionView.currentItem.send_to = this.formObject.send_to;
                }
                else if (this.formObject.send_via == "URL") {
                    this.flex.collectionView.currentItem.business_hours_type = this.formObject.business_hours_type;
                    this.flex.collectionView.currentItem.send_via = this.formObject.send_via;
                    this.flex.collectionView.currentItem.send_to = this.formObject.send_to;
                }
            }
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
            //(<wijmo.collections.CollectionView>this.flex.collectionView).commitNew();
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

    onSelectionChanged(event: wijmo.grid.CellRangeEventArgs) {
        if (event.panel.grid.selectedItems.length > 0) {
            this.getSendVia(event.panel.grid.selectedItems[0].send_via);
        }
    }

    showEditForm(dlg: wijmo.input.Popup) {

        this.formTitle = "Edit " + this.formName;

        this.formObject.id = this.flex.collectionView.currentItem.id;
        this.formObject.scripts = this.scripts;
        this.formObject.urls = this.urls;
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

    showAddForm(dlg: wijmo.input.Popup) {

        this.formTitle = "Add " + this.formName;

        this.formObject.id = "";
        this.formObject.business_hours_type = this.businesshours[0];
        this.formObject.send_to = "";
        this.formObject.script_name = "";
        this.formObject.send_via = "E-mail";
        this.formObject.copy_to = "";
        this.formObject.persistent_notification = "";
        this.formObject.blind_copy_to = "";
        this.getSendVia(this.formObject.send_via);

        this.showDialog(dlg);

    }

    onSendViaSelectedIndexChanged(event: wijmo.EventArgs) {
        
        this.getSendVia(this.formObject.send_via);

    }

    getSendVia(combotxt: string) {
        if (combotxt == "E-mail") {
            this.isUrl = false;
            this.isEmail = true;
            this.isScript = false;
            this.isSMS = false;
            this.isSNMP = false;
            this.isWinLog = false;
        }
        else if (combotxt == "Script") {
            this.isUrl = false;
            this.isEmail = false;
            this.isScript = true;
            this.isSMS = false;
            this.isSNMP = false;
            this.isWinLog = false;
            
        }
        else if (combotxt == "SMS") {
            this.isUrl = false;
            this.isEmail = false;
            this.isScript = false;
            this.isSMS = true;
            this.isSNMP = false;
            this.isWinLog = false;
        }
        else if (combotxt == "SNMP Trap") {
            this.isUrl = false;
            this.isEmail = false;
            this.isScript = false;
            this.isSMS = false;
            this.isSNMP = false;
            this.isWinLog = false;
        }
        else if (combotxt == "Windows Log") {
            this.isUrl = false;
            this.isEmail = false;
            this.isScript = false;
            this.isSMS = false;
            this.isSNMP = false;
            this.isWinLog = false;
        }
        else if (combotxt == "URL") {
            this.isUrl = true;
            this.isEmail = false;
            this.isScript = false;
            this.isSMS = false;
            this.isSNMP = false;
            this.isWinLog = false;
        }
    }

    deleteHoursDestinations() {
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