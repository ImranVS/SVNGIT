import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {RESTService} from '../../../core/services';
import {Router, ActivatedRoute} from '@angular/router';
@Component({
    selector: 'servder-form',
    templateUrl: '/app/configurator/components/server/server-advanced-settings.component.html'

})
//export class ServerDiskSettings implements OnInit, AfterViewInit {
export class ServerAdvancedSettings implements OnInit {
    advancedSettingsForm: FormGroup;
    errorMessage: string;
    deviceId: any;
    deviceCredentialData: any;
    // selectedCredential: string;
    selectedTpe: string;
    deviceType: any;
    selectedIbmDb2Credential: string;
    selectedCredential: string;
    modal = true;
    addCredentialForm: FormGroup;
    serverType: string;
    constructor(
        private formBuilder: FormBuilder,
        private dataProvider: RESTService,
        private route: ActivatedRoute
    ) {

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
    }

    ngOnInit() {

        this.route.params.subscribe(params => {
            this.deviceId = params['service'];

        });
        this.dataProvider.get('/Configurator/get_advanced_settings/' + this.deviceId)
            .subscribe(
            (response) => {
                console.log(response.data);

                this.advancedSettingsForm.setValue(response.data);
                this.deviceType = response.data.device_type;
                console.log(this.deviceType);
            },

            (error) => this.errorMessage = <any>error

            );
        // this.selectedTpe = this.advancedSettingsForm.memory_threshold;

        this.dataProvider.get('/Configurator/get_server_credentials_businesshours')
            .subscribe(
            (response) => {

                this.deviceCredentialData = response.data.credentialsData;

            },
            (error) => this.errorMessage = <any>error
            );
        // alert(this.deviceId);
    }

    onSubmit(advancedSettings: any): void {

        this.dataProvider.put('/Configurator/save_advanced_settings/' + this.deviceId, advancedSettings)
            .subscribe(
            response => {

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
        else if (addCrdential.device_type = "Sametime")
            addCrdential.device_type = "Sametime";

        addCrdential.confirm_password = "";
        addCrdential.id = null;
        addCrdential.is_modified = true;
        this.dataProvider.put('/Configurator/save_credentials', addCrdential)
            .subscribe(
            response => {
                this.addCredentialForm.reset();
            });

        dialog.hide();

    }
}