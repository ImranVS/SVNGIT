import {Component, OnInit, AfterViewInit, ViewChildren,Output, EventEmitter} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';

import {RESTService} from '../../../core/services';

@Component({
    selector: 'servder-form',
    templateUrl: '/app/configurator/components/serverSettings/server-disk-settings.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class ServerDiskSettings implements OnInit, AfterViewInit {

    @ViewChildren('name') inputName; 
    errorMessage: any;
    deviceLocationData: any;
    deviceCredentialData: any;
    devicebusinessHourData: any;
    serverLocationsBusinessHoursCredentialsForm: FormGroup;
    selectedDiskSetting: any;
    selectedDiskSettingValue: any;
    devices: string;
    selectedDiskByPercentage: string;
    selectedDiskByGB: string;
    selectedDisks: string;
    selectedNoDiskAlerts: string;
    postData: any;
    constructor(    
        private dataProvider: RESTService,
        private formBuilder: FormBuilder) {

        this.serverLocationsBusinessHoursCredentialsForm = this.formBuilder.group({
            'setting': [''],
            'value': [''],
            'devices': ['']
           

        });
   }

    ngOnInit() {
      
    }

    ngAfterViewInit() {

    }
    applySetting(nameValue: any): void{

        if (this.selectedDiskSetting == "allDisksBypercentage")
            
            this.selectedDiskSettingValue = this.selectedDiskByPercentage;
        else if (this.selectedDiskSetting == "allDisksByGB")
            this.selectedDiskSettingValue = this.selectedDiskByGB;
        else if (this.selectedDiskSetting == "selectedDisks")
            this.selectedDiskSettingValue = this.selectedDisks;  
        else if (this.selectedDiskSetting == "noDiskAlerts")
            this.selectedDiskSettingValue = this.selectedNoDiskAlerts;  
        this.postData = {
            "setting": this.selectedDiskSetting,
            "value": this.selectedDiskSettingValue,
            "devices": this.devices
        }; 
      

    }
    changeInDevices(server: string) {
        this.devices = server;
    }
   
}