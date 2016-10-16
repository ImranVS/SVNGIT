import {Component, OnInit, AfterViewInit, ViewChildren,Output, EventEmitter} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';

import {RESTService} from '../../../core/services';

@Component({
    selector: 'servder-form',
    templateUrl: '/app/configurator/components/serverSettings/server-locations-credentials-businesshours.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class ServerLocations implements OnInit, AfterViewInit {

    @ViewChildren('name') inputName; 
    errorMessage: any;
    deviceLocationData: any;
    deviceCredentialData: any;
    devicebusinessHourData: any;
    selectedSetting: any;
    selectedSettingValue: any;
    devices: string;
    selectedLocation: string;
    selectedCredential: string;
    selectedBusinessHour: string;
    postData: any;
    constructor(    
        private dataProvider: RESTService) {
   }

    ngOnInit() {
        this.dataProvider.get('/Configurator/get-server-credentials-businesshours')
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
    applySetting() {
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
    }
    changeInDevices(servers: string) {
        this.devices = servers;
    }
   
}