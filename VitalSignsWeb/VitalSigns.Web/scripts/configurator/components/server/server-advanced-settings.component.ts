import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {RESTService} from '../../../core/services';
import {Router, ActivatedRoute} from '@angular/router';
import {AppComponentService} from '../../../core/services';

@Component({
    selector: 'servder-form',
    templateUrl: '/app/configurator/components/server/server-advanced-settings.component.html'

})
export class ServerAdvancedSettings implements OnInit {
    advancedSettingsForm: FormGroup;
    errorMessage: string;
    deviceId: any;
    deviceCredentialData: any;
    ConnectionsCredentialData: any;
    selectedTpe: string;
    deviceType: any;
    selectedIbmDb2Credential: string;
    selectedCredential: string;
    modal = true;
    addCredentialForm: FormGroup;
    serverType: string;
    Types: string;
    websphereplatform: string;
    platform: string;
 appComponentService: AppComponentService;
    db2CredentialsId : any;

    constructor(
        private formBuilder: FormBuilder,
        private dataProvider: RESTService,
        private route: ActivatedRoute,
        appComponentService: AppComponentService)
        { 
        this.advancedSettingsForm = this.formBuilder.group({
            'memory_threshold': [''],
            'cpu_threshold': [''],
            'server_days_alert': [''],
            'cluster_replication_delay_threshold': [''],
            'proxy_server_type': [''],
            'proxy_server_protocol': [''],
            'dbms_host_name': [''],
            'dbms_name': [''],
            'dbms_port': [''],
            'collect_extended_statistics': [''],
            'collect_meeting_statistics': [''],
            'extended_statistics_port': [''],
            'meeting_port': [''],
            'meeting_host_name': [''],
            'meeting_require_ssl': [''],
            'conference_host_name': [''],
            'conference_port': [''],
            'conference_require_ssl': [''],
            'database_settings_host_name': [''],
            'database_settings_credentials_id': [''],
            'database_settings_port': [''],
            'device_type': [''],
            'cluster_replication_queue_threshold': [''],
            'db2_settings_credentials_id': [''],
            'collect_conference_statistics': ['']

        });

        this.addCredentialForm = this.formBuilder.group({
            'alias': ['', Validators.required],
            'user_id': ['', Validators.required],
            'password': ['', Validators.required],
            'device_type': [''],
            'confirm_password': [''],
            'id': [''],
            'is_modified': ['']

        });
        this.appComponentService = appComponentService;
    }

    ngOnInit() {

        this.route.params.subscribe(params => {
            this.deviceId = params['service'];
        });
        this.dataProvider.get('/Configurator/get_advanced_settings/' + this.deviceId)
            .subscribe(
            (response) => {
               
                this.advancedSettingsForm.setValue(response.data.results);
                this.db2CredentialsId = response.data.results.db2_settings_credentials_id;
                this.deviceType = response.data.results.device_type;
                this.deviceCredentialData = response.data.credentialsData;
                this.ConnectionsCredentialData = response.data.credentialsData;
                this.route.queryParams.subscribe(params => {

                    this.websphereplatform = params['platform'];
                    if (this.websphereplatform == "undefined" || this.websphereplatform == null)
                    {
                        this.websphereplatform = response.data.platform;
                    }
                    else {
                        this.websphereplatform = this.websphereplatform;
                    }
                });

            },
            (error) => {
                this.errorMessage = <any>error
                this.appComponentService.showErrorMessage(this.errorMessage);
            }

            );
    }
    onSubmit(advancedSettings: any): void {
        this.dataProvider.put('/Configurator/save_advanced_settings/' + this.deviceId, advancedSettings)
            .subscribe(
            response => {

                if (response.status == "Success") {

                    this.appComponentService.showSuccessMessage(response.message);

                } else {

                    this.appComponentService.showErrorMessage(response.message);
                }
            });
    }
    addIbmCredentials(dlg: wijmo.input.Popup) {
        if (dlg) {
            dlg.modal = this.modal;
            dlg.hideTrigger = dlg.modal ? wijmo.input.PopupTrigger.None : wijmo.input.PopupTrigger.Blur;
            dlg.show();
            this.serverType = "IBM Connections"
        }
    }
    addSametimeCredentials(dlg: wijmo.input.Popup) {
        if (dlg) {
            dlg.modal = this.modal;
            dlg.hideTrigger = dlg.modal ? wijmo.input.PopupTrigger.None : wijmo.input.PopupTrigger.Blur;
            dlg.show();
            this.serverType = "Sametime"
        }
    }
    SaveCredential(addCrdential: any, dialog: wijmo.input.Popup) {
        if (this.serverType == "IBM Connections")
            addCrdential.device_type = "IBM Connections";
        else if (this.serverType = "Sametime")
            addCrdential.device_type = "Sametime";

        addCrdential.confirm_password = "";
        addCrdential.id = null;
        addCrdential.is_modified = true;
        this.dataProvider.put('/Configurator/save_credentials', addCrdential)
            .subscribe(

            response => {

                if (response.status == "Success") {

                    this.appComponentService.showSuccessMessage(response.message);
                    this.addCredentialForm.reset();
                } else {

                    this.appComponentService.showErrorMessage(response.message);
                }
            });
        this.dataProvider.get('/Configurator/get_advanced_settings/' + this.deviceId)
            .subscribe(
            (response) => {

               
                this.deviceCredentialData = response.data.credentialsData;
                this.ConnectionsCredentialData = response.data.credentialsData;
            

            },
            (error) => {
                this.errorMessage = <any>error
                this.appComponentService.showErrorMessage(this.errorMessage);
            }

            );
    
        dialog.hide();
    }
}

    