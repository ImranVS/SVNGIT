import {Component, OnInit, AfterViewInit, ViewChildren,Output, EventEmitter} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {GridBase} from '../../../core/gridBase';
import {RESTService} from '../../../core/services';

@Component({
    selector: 'servder-form',
    templateUrl: '/app/configurator/components/serverSettings/server-disk-settings.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
//export class ServerDiskSettings implements OnInit, AfterViewInit {
export class ServerDiskSettings extends GridBase implements OnInit  {
    
    @ViewChildren('name') inputName; 
    errorMessage: any;
    deviceLocationData: any;
    deviceCredentialData: any;
    devicebusinessHourData: any;
    diskSettingsForm: FormGroup;
    selectedDiskSetting: any;
    selectedDiskSettingValue: any;
    devices: string;
    diskByPercentage: string;
    diskByGB: string;
    selectedDisks: string;
    noDiskAlerts: string;
    postData: any;
    
    constructor(    
        private dataProvider: RESTService,
        private formBuilder: FormBuilder) {

        super(dataProvider, '/Configurator/get_disk_names');
      
       // super(dataProvider, '/Configurator/get_disk_names');
       this.formName = "Disk Information";
        this.diskSettingsForm = this.formBuilder.group({
            'setting': [''],
            'value': [''],
            'devices': ['']


        });
   }

    ngOnInit() {
      
    }

    
    applySetting(nameValue: any): void{

        if (this.selectedDiskSetting == "allDisksBypercentage")
        {
            alert(this.diskByPercentage);
            this.selectedDiskSettingValue = this.diskByPercentage;
        }
        else if (this.selectedDiskSetting == "allDisksByGB")
            this.selectedDiskSettingValue = this.diskByGB;
        else if (this.selectedDiskSetting == "selectedDisks")
            this.selectedDiskSettingValue = this.selectedDisks;  
        else if (this.selectedDiskSetting == "noDiskAlerts")
            this.selectedDiskSettingValue = this.noDiskAlerts;  
        this.postData = {
            "setting": this.selectedDiskSetting,
            "value": this.selectedDiskSettingValue,
            "devices": this.devices
        }; 
      
        this.diskSettingsForm.setValue(this.postData);
        this.dataProvider.put(
            '/Configurator/save_disk_settings',
            this.postData);
    }
    changeInDevices(server: string) {
        this.devices = server;
    }
   
}