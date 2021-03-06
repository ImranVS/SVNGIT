﻿import {Component, OnInit, ViewChildren} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {RESTService} from '../../../core/services';
import {Router, ActivatedRoute} from '@angular/router';
import {AppComponentService} from '../../../core/services';

@Component({
    selector: 'servder-form',
    templateUrl: '/app/configurator/components/server/server-advanced-settings.component.html'

})
export class ServerAdvancedSettings implements OnInit {
    casCredentialsId: string;
    advancedSettingsForm: FormGroup;
    errorMessage: string;
    deviceId: any;
    deviceCredentialData: any;
    wsDeviceCredentialData: any;
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
    db2CredentialsId: any;
    credentials_id: any;
    webSphereServerNodeData: any;
    websphereSettingsForm: FormGroup;
    nodes: FormGroup;
    limitsChecked: boolean;
    websphereData: any;
    database_settings_credentials_id: any;

    checkboxGroup: FormGroup;

    cas_tests: any = [
        { name: "SMTP", value: true },
        { name: "Outlook Anywhere", value: false },
        { name: "OWA", value: false },
        { name: "POP3", value: false },
        { name: "Auto Discovery", value: false },
        { name: "Outlook Native RPC", value: false },
        { name: "IMAP", value: false },
        { name: "Active Sync", value: false }
    ];
    site_collections: any = [
        { name: "Conflicting Content Types", value: true },
        { name: "Customized Files", value: false },
        { name: "Missing Galleries", value: false },
        { name: "Missing Parent Content Types", value: false },
        { name: "Missing Site Templates", value: false },
        { name: "Unsupported MUI References", value: false },
        { name: "Unsupported Language Pack References", value: false }
    ];

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
            'domino_server_name': [''],
            'device_type': [''],
            'cluster_replication_queue_threshold': [''],
            'db2_settings_credentials_id': [''],
            'collect_conference_statistics': [''],
            'id': [''],
            'cell_id': [''],
            'cell_name': [''],
            'name': [''],
            'host_name': [''],
            'connection_type': [''],
            'port_no': [''],
            'global_security': [''],
            'credentials_id': [''],
            'credentials_name': [''],
            'realm': [''],
            'user_name': [''],
            'password': [''],
            'nodes_data': [''],
            'simulation_tests': [''],
            'cas_credentials_id': ['']
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
     
        this.nodes = this.formBuilder.group({
            'selected_servers': [''],
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
                this.advancedSettingsForm.valueChanges.subscribe(websphereobject => {
                    this.limitsChecked = websphereobject['global_security'];
                });
                if (response.data.results != null) {
                    this.webSphereServerNodeData = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data.results.nodes_data));
                    this.webSphereServerNodeData.groupDescriptions.push(new wijmo.collections.PropertyGroupDescription("node_name"));
                    this.webSphereServerNodeData.pageSize = 10;
                }
                this.database_settings_credentials_id = response.data.results.database_settings_credentials_id;
                this.wsDeviceCredentialData = response.data.wsCredentialsData;
                this.websphereData = response.data.websphereData;
                this.db2CredentialsId = response.data.results.db2_settings_credentials_id;
                this.credentials_id = response.data.results.credentials_id;
                this.limitsChecked = response.data.results.global_security;
                this.deviceType = response.data.results.device_type;
                this.deviceCredentialData = response.data.credentialsData;
                this.ConnectionsCredentialData = response.data.credentialsData;
                this.casCredentialsId = response.data.results.cas_credentials_id;
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
                if (response.data.results.simulation_tests && this.deviceType == "Exchange") {
                    this.advancedSettingsForm.patchValue({
                        'simulation_tests': [this.cas_tests],
                    })
                    for (var i = 0; i < response.data.results.simulation_tests.length; i++) {
                        let temp = this.cas_tests.find(x => x.name == response.data.results.simulation_tests[i].name);
                        if (temp)
                            temp.value = true;
                    }

                }
                if (response.data.results.simulation_tests && this.deviceType == "SharePoint") {
                    this.advancedSettingsForm.patchValue({
                        'simulation_tests': [this.site_collections],
                    });
                    for (var i = 0; i < response.data.results.simulation_tests.length; i++) {
                        let temp = this.site_collections.find(x => x.name == response.data.results.simulation_tests[i].name);
                        if (temp)
                            temp.value = true;
                    }

                }
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

    addCredentials(dlg: wijmo.input.Popup) {
        if (dlg) {
            dlg.modal = this.modal;
            dlg.hideTrigger = dlg.modal ? wijmo.input.PopupTrigger.None : wijmo.input.PopupTrigger.Blur;
            dlg.show();
            this.serverType = this.deviceType
        }
    }

    SaveCredential(addCrdential: any, dialog: wijmo.input.Popup) {
        if (this.deviceType == "IBM Connections")
            addCrdential.device_type = "IBM Connections";
        else if (this.deviceType == "Sametime")
            addCrdential.device_type = "Sametime"
        else
            addCrdential.device_type = this.deviceType;

        addCrdential.confirm_password = "";
        addCrdential.id = null;
        addCrdential.is_modified = false;
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

    isLimitsChecked(ischecked: boolean) {
        this.limitsChecked = ischecked;
    }

    onClickRefresh(advancedSettings: any) {
        this.dataProvider.put('/Configurator/get_sametime_websphere_nodes/' + this.deviceId, advancedSettings)
            .subscribe(
            response => {
                if (response.status == "Success") {
                    if (response.data.results != null) {
                        this.webSphereServerNodeData = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data.results.nodes_data));
                        this.webSphereServerNodeData.groupDescriptions.push(new wijmo.collections.PropertyGroupDescription("node_name"));
                        this.webSphereServerNodeData.pageSize = 10;
                    }
                }
                else {
                    this.appComponentService.showErrorMessage(response.message);
                }
            });
    }
}

    