import {Component, OnInit, AfterViewInit, ViewChild,Output, EventEmitter} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {GridBase} from '../../../core/gridBase';
import {RESTService} from '../../../core/services';
import {DiskSttingsValue} from '../../models/server-disk-settings';
import {WidgetComponent, WidgetService} from '../../../core/widgets';
@Component({
    selector: 'servder-form',
    templateUrl: '/app/configurator/components/server/server-disk-settings.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
//export class ServerDiskSettings implements OnInit, AfterViewInit {
export class DominoServerDiskSettings  implements OnInit {
    deviceId: any;
    data: wijmo.collections.CollectionView;
    errorMessage: any;
    deviceLocationData: any;
    deviceCredentialData: any;
    devicebusinessHourData: any;
    diskSettingsForm: FormGroup;
    selectedDiskSetting: any;
    selectedDiskSettingValue: any;
   
    diskByPercentage: string;
    diskByGB: string;
    selectedDisks: string;
    noDiskAlerts: string;
    postData: any;
    diskValues: any;
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;

    constructor(    
        private dataProvider: RESTService,
        private formBuilder: FormBuilder,
       
        private route: ActivatedRoute) {
       
      
      
        this.diskSettingsForm = this.formBuilder.group({
            'setting': [''],
            'value': [''],
            'devices': ['']


        });
   }

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.deviceId = params['service'];

        });
        this.dataProvider.get('/Configurator/get_server_disk_info/' + this.deviceId)
            .subscribe(
            response => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
                this.data.pageSize = 10;

            }); 
    }
    itemsSourceChangedHandler() {

        var flex = this.flex;

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
    
    applySetting(nameValue: any){
     
      
        if (this.selectedDiskSetting == "allDisksBypercentage") {
          
            this.selectedDiskSettingValue = this.diskByPercentage;
        }
        else if (this.selectedDiskSetting == "allDisksByGB")
            this.selectedDiskSettingValue = this.diskByGB;
        else if (this.selectedDiskSetting == "selectedDisks") 
        {
            this.selectedDiskSettingValue = this.selectedDisks;
            var slectedDiskSettingValues: DiskSttingsValue[] = [];
            for (var _i = 0; _i < this.flex.collectionView.sourceCollection.length; _i++) {

                var item = (<wijmo.collections.CollectionView>this.flex.collectionView.sourceCollection)[_i];

                if (item.is_selected) {
                    

                    var dominoserverObject = new DiskSttingsValue();
                    dominoserverObject.is_selected = item.is_selected;
                    dominoserverObject.disk_name = item.disk_name;

                    dominoserverObject.freespace_threshold = item.freespace_threshold;
                    dominoserverObject.threshold_type = item.threshold_type;
                   // alert(item.is_selected)
                    slectedDiskSettingValues.push(dominoserverObject);
                }
            }
            this.diskValues = slectedDiskSettingValues;
        
        }
          
        else if (this.selectedDiskSetting == "noDiskAlerts")
            this.selectedDiskSettingValue = this.noDiskAlerts;
       

     
        if (this.selectedDiskSetting == "selectedDisks") {

            this.postData = {
                "setting": this.selectedDiskSetting,
                "value": this.diskValues,
                "devices": this.deviceId
            };
           
        }
        else if (this.selectedDiskSetting == "noDiskAlerts")
        {
            this.postData = {
                "setting": this.selectedDiskSetting,
                "value": null,
                "devices": this.deviceId
            };

        }
        else {
            this.postData = {
                "setting": this.selectedDiskSetting,
                "value": this.selectedDiskSettingValue,
                "devices": this.deviceId
            };
        }
        this.dataProvider.put('/Configurator/save_server_disk_settings', this.postData)
            .subscribe(
            response => {

            });
    }
   
    get pageSize(): number {
        return this.data.pageSize;
    }
    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            if (this.flex) {
                (<wijmo.collections.IPagedCollectionView>this.flex.collectionView).pageSize = value;
            }
        }
    }
}