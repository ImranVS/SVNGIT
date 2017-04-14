import {Component, OnInit, AfterViewInit, ViewChild} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {HttpModule}    from '@angular/http';
import {GridBase} from '../../../core/gridBase';
import {RESTService} from '../../../core/services';
import {DiskSttingsValue} from '../../models/server-disk-settings';
import {AppComponentService} from '../../../core/services';
import {ServersLocationService} from '../serverSettings/serverattributes-view.service';
@Component({
    templateUrl: '/app/configurator/components/serverSettings/server-settings-disk-settings.html',
    providers: [
        HttpModule,
        RESTService,
        ServersLocationService
    ]
})
//export class ServerDiskSettings implements OnInit, AfterViewInit {
export class ServerDiskSettings implements OnInit {
    @ViewChild('flexDisks') flexDisks: wijmo.grid.FlexGrid;
    data: wijmo.collections.CollectionView;
    errorMessage: any;
    deviceLocationData: any;
    deviceCredentialData: any;
    devicebusinessHourData: any;
    diskSettingsForm: FormGroup;
    selectedDiskSetting: any;
    selectedDiskSettingValue: any;
    devices: string;
    checkedDevices: any;
    diskByPercentage: string;
    diskByGB: string;
    selectedDisks: string;
    noDiskAlerts: string;
    postData: any;
    diskValues: any;
    protected appComponentService: AppComponentService;
    deviceTypes: string = "Domino,Exchange,Active Directory,Windows";
    selectedDeviceTypes: string = "Domino,Exchange,Active Directory,Windows";
    thresholdTypes: string[];
    formObject: any = {
        id: null,
        threshold_type: null,
        free_space_threshold: null
    };

    constructor(    
        private dataProvider: RESTService,
        private formBuilder: FormBuilder,
        appComponentService: AppComponentService) {

        this.dataProvider.get('/Configurator/get_disk_names')
            .subscribe(
            response => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
                this.data.pageSize = 10;

            }); 
      
        this.diskSettingsForm = this.formBuilder.group({
            'setting': [''],
            'value': [''],
            'devices': ['']


        });
        this.thresholdTypes = ["Percent", "GB"];
        this.appComponentService = appComponentService;
    }
    ngOnInit() {
    }
    ngOnAfterInit() {
        this.formObject.threshold_type = this.flexDisks.collectionView.currentItem.threshold_type;
        this.formObject.free_space_threshold = this.flexDisks.collectionView.currentItem.freespace_threshold;
    }

    itemsSourceChangedHandler() {
      
        var flex = this.flexDisks;
      
        if (flex) {
            var colThresholdType = flex.columns.getColumn('threshold_type');

            if (colThresholdType) {

                colThresholdType.showDropDown = true; // or colors (just to show how)
                var unitsData = [{ unit: "Percent", code: "Percent" }, { unit: "GB", code: "GB" }];
              
                var unitsDataMap = new wijmo.grid.DataMap(unitsData, 'unit', 'code');
             
                colThresholdType.dataMap = unitsDataMap;
            }
            else {
                colThresholdType.dataMap = null;
            }
        }
    }
    
    applySetting(nameValue: any): void{
       // alert(this.flex);
       
        if (this.selectedDiskSetting == "allDisksBypercentage")
        {
          //  alert(this.diskByPercentage);
            this.selectedDiskSettingValue = this.diskByPercentage;
        }
        else if (this.selectedDiskSetting == "allDisksByGB")
            this.selectedDiskSettingValue = this.diskByGB;
        else if (this.selectedDiskSetting == "selectedDisks")
        {
            this.selectedDiskSettingValue = this.selectedDisks;
            var slectedDiskSettingValues: DiskSttingsValue[] = [];
            for (var _i = 0; _i < this.flexDisks.collectionView.sourceCollection.length; _i++) {

                var item = (<wijmo.collections.CollectionView>this.flexDisks.collectionView.sourceCollection)[_i];

                if (item.is_selected) {

                    var dominoserverObject = new DiskSttingsValue();
                    dominoserverObject.is_selected = item.is_selected;
                    dominoserverObject.disk_name = item.disk_name;
                    dominoserverObject.freespace_threshold = item.freespace_threshold;
                    dominoserverObject.threshold_type = item.threshold_type;

                    slectedDiskSettingValues.push(dominoserverObject);
                }

            }
            this.diskValues = slectedDiskSettingValues;
        }
        else if (this.selectedDiskSetting == "noDiskAlerts")
            this.selectedDiskSettingValue = this.noDiskAlerts;  
        if (this.selectedDiskSetting == "selectedDisks")
        {

            this.postData = {
                "setting": this.selectedDiskSetting,
                "value": this.diskValues,
                "devices": this.devices
            };
        }
        else if (this.selectedDiskSetting == "noDiskAlerts")
        {
            this.postData = {
                "setting": this.selectedDiskSetting,
                "value":null,
                "devices": this.devices
            };
        }
        else {
            this.postData = {
                "setting": this.selectedDiskSetting,
                "value": this.selectedDiskSettingValue,
                "devices": this.devices
            };
        }
      
        this.diskSettingsForm.setValue(this.postData);
        this.dataProvider.put('/Configurator/save_disk_settings', this.postData)
            .subscribe(
            response => {

                if (response.status == "Success") {

                    this.appComponentService.showSuccessMessage(response.message);

                } else {

                    this.appComponentService.showErrorMessage(response.message);
                }
            });
    }
    changeInDevices(devices: any) {
        this.devices = devices;
        this.checkedDevices = devices;
    }
    get pageSize(): number {
        return this.data.pageSize;
    }
    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            if (this.flexDisks) {
                (<wijmo.collections.IPagedCollectionView>this.flexDisks.collectionView).pageSize = value;
            }
        }
    }
}