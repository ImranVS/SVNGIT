import { Component, OnInit, OnDestroy, ViewChild} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, FormsModule, Validators} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpModule } from '@angular/http';
import { NgForm } from '@angular/forms';
import {RESTService} from '../../../core/services';
import { AppNavigator } from '../../../navigation/app.navigator.component';
import { AuthenticationService } from '../../../profiles/services/authentication.service';
import * as gridHelpers from '../../../core/services/helpers/gridutils';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as wjCoreModule from 'wijmo/wijmo.angular2.core';
import {GridBase} from '../../../core/gridBase';
import { AppComponentService } from '../../../core/services';
import { Subscription } from 'rxjs';


@Component({
    selector: 'vs-notesmail-probes',
    templateUrl: 'app/configurator/components/ibmDomino/notesmail-probes.component.html',
    providers: [
        HttpModule,
        RESTService,
        gridHelpers.CommonUtils
    ]
})
export class NotesMailProbes extends GridBase implements OnInit, OnDestroy {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    serverNames: any;
    categories: any;
    errorMessage: string;
    currentPageSize: any = 20;
    credentialCollectionView: wijmo.collections.CollectionView;
    getCredentialsSubscription: Subscription;
    getNameValueSubscrition: Subscription;
    getDominoServersSubscription: Subscription;
    saveCredentialsSubscription: Subscription;
    setNameValueSubscription: Subscription;


    get pageSize(): number {
        return this.data.pageSize;
    }
    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
            var obj = {
                name: this.gridHelpers.getGridPageName("NotesMailProbes", this.authService.CurrentUser.email),
                value: value
            };

            this.setNameValueSubscription = this.service.put(`/services/set_name_value`, obj)
                .subscribe(
                (data) => {

                },
                (error) => console.log(error)
                );

        }
    }

    constructor(service: RESTService, appComponentService: AppComponentService, protected gridHelpers: gridHelpers.CommonUtils, private authService: AuthenticationService
) {
        super(service, appComponentService);
        this.formName = "NotesMail Probe";
        this.categories = ["Domino", "Inbound", "International", "Internet Round Trip", "Domestic", "Between Hubs"];
        this.getDominoServersSubscription = this.service.get('/Configurator/get_domino_servers')
            .subscribe(
            (response) => {
                this.serverNames = response.data.serversData;
            },
            (error) => this.errorMessage = <any>error
            );
    }

    ngOnInit() {
        this.initialGridBind('/configurator/notesmail_probes_list');
        this.getNameValueSubscrition = this.service.get(`/services/get_name_value?name=${this.gridHelpers.getGridPageName("NotesMailProbes", this.authService.CurrentUser.email)}`)
            .subscribe(
            (data) => {
                this.currentPageSize = Number(data.data.value);
                this.data.pageSize = this.currentPageSize;
                this.data.refresh();
            },
            (error) => this.errorMessage = <any>error);
        
        this.getCredentialData();
    }

    ngOnDestroy() {
        if (this.getCredentialsSubscription) {
            this.getCredentialsSubscription.unsubscribe();
        }
        if (this.getNameValueSubscrition) {
            this.getNameValueSubscrition.unsubscribe();
        }
        if (this.saveCredentialsSubscription) {
            this.saveCredentialsSubscription.unsubscribe();
        }
        if (this.getDominoServersSubscription) {
            this.getCredentialsSubscription.unsubscribe();
        }
        if (this.setNameValueSubscription) {
            this.setNameValueSubscription.unsubscribe();
        }
        
    }

 
    getCredentialData() {
        this.getCredentialsSubscription = this.service.get('/configurator/get_credentials')
            .subscribe(
            response => {
                const credentialData = response.data;
                this.credentialCollectionView = new wijmo.collections.CollectionView(credentialData, {currentItem:null});              
            },
            error => this.errorMessage = <any>error
        );

    }
    onSelectIndexChanged(combo: wijmo.input.ComboBox) {
        console.log('inside onSelectIndexChanged');
        console.log(combo);
    }

    saveNotesMailProbes(dlg: wijmo.input.Popup) {
        this.saveGridRow('/configurator/save_notesmail_probes', dlg);
    }

    addCredential(dlg: wijmo.input.Popup) {
        if (dlg) {
           dlg.show();
        }
    }

    SaveCredential(addCredentialForm: NgForm, dialog: wijmo.input.Popup) {
        const credential = addCredentialForm.value;
        credential.confirm_password = "";
        credential.id = null;
        credential.is_modified = false;
        this.saveCredentialsSubscription = this.service.put('/Configurator/save_credentials', credential)
            .subscribe( response => {
                if (response.status == "Success") {
                    this.appComponentService.showSuccessMessage(response.message);
                    addCredentialForm.reset();
                    this.getCredentialData();
                } else {
                    this.appComponentService.showErrorMessage(response.message);
                }
                    
        });
        dialog.hide();
    }

    showAddForm(dlg: wijmo.input.Popup) {

        this.addGridRow(dlg);
        this.currentEditItem.id = "";
        this.currentEditItem.name = "";
        this.currentEditItem.scan_interval = "";
        this.currentEditItem.off_hours_interval = "";
        this.currentEditItem.retry_interval = "";
        this.currentEditItem.threshold = "";
        this.currentEditItem.send_to = "";
        this.currentEditItem.reply_to = "";
        this.currentEditItem.destination_database = "";
        this.currentEditItem.use_imap = false;
        this.currentEditItem.imap_host_name = "";
        this.currentEditItem.credentials_id = null;
    }

    deleteNotesMailProbes() {
        this.deleteGridRow('/configurator/delete_server/');
    }

    onItemsSourceChanged() {
        var row = this.flex.columnHeaders.rows[0];
        row.wordWrap = true;
        // autosize first header row
        this.flex.autoSizeRow(0, true);

    }
}