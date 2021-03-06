﻿import {Component, OnInit, AfterViewInit, ViewChildren,Output, EventEmitter} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {ServersLocationService} from '../serverSettings/serverattributes-view.service';
import {RESTService} from '../../../core/services';
import {AppComponentService} from '../../../core/services';

@Component({
    selector: 'servder-form',
    templateUrl: '/app/configurator/components/serverSettings/server-settings-locations-credentials-businesshours.component.html',
    providers: [
        HttpModule,
        RESTService,
        ServersLocationService
    ]
})
export class ServerLocations implements OnInit, AfterViewInit {
    @ViewChildren('name') inputName; 
    errorMessage: any;
    deviceLocationData: any;
    deviceCredentialData: any;
    devicebusinessHourData: any;
    serverLocationsBusinessHoursCredentialsForm: FormGroup;
    selectedSetting: any;
    selectedSettingValue: any;
    devices: string = "";
    checkedDevices: any;
    selectedLocation: string=null;
    selectedCredential: string = null;
    selectedBusinessHour: string = null;
    postData: any;
    protected appComponentService: AppComponentService;
    constructor(    
        private dataProvider: RESTService,
        private formBuilder: FormBuilder,
        appComponentService: AppComponentService) {
        this.serverLocationsBusinessHoursCredentialsForm = this.formBuilder.group({
            'setting': [''],
            'value': [''],
            'devices': ['']
        });
        this.appComponentService = appComponentService;
   }

    ngOnInit() {
        this.dataProvider.get('/Configurator/get_server_credentials_businesshours')
            .subscribe(
            (response) => {

                this.deviceLocationData = response.data.locationsData;
                this.deviceCredentialData = response.data.credentialsData;
                this.devicebusinessHourData = response.data.businessHoursData;
            },
            (error) => this.errorMessage = <any>error
            );
    }

    ngAfterViewInit() {

    }
    applySetting(nameValue: any): void{

        if (this.selectedSetting == "locations")
            this.selectedSettingValue = this.selectedLocation;
        else if (this.selectedSetting == "credentials")
            this.selectedSettingValue = this.selectedCredential;
        else if (this.selectedSetting == "businessHours")
            this.selectedSettingValue = this.selectedBusinessHour;  
        this.postData = {
            "setting": this.selectedSetting,
            "value": this.selectedSettingValue,
            "devices": this.devices
        }; 
        this.serverLocationsBusinessHoursCredentialsForm.setValue(this.postData);
        this.dataProvider.put('/Configurator/save_server_credentials_businesshours', this.postData)
            .subscribe(
            response => {

                if (response.status == "Success") {

                    this.appComponentService.showSuccessMessage(response.message);

                } else {

                    this.appComponentService.showErrorMessage(response.message);
                }
            });
       //alert(this.postData);


    }
    changeInDevices(devices: any) {
        this.devices = devices;
        this.checkedDevices = devices;
    }
   
}