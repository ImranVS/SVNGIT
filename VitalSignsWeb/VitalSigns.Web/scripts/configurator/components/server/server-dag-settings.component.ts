import {Component, OnInit, AfterViewInit, ViewChild, Output, EventEmitter} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {GridBase} from '../../../core/gridBase';
import {RESTService} from '../../../core/services';
import {DiskSttingsValue} from '../../models/server-disk-settings';
import {WidgetComponent} from '../../../core/widgets';
import {WidgetService} from '../../../core/widgets/services/widget.service';
import {AppComponentService} from '../../../core/services';
import { ServersLocationService } from '../serverSettings/serverattributes-view.service';
import * as gridHelpers from '../../../core/services/helpers/gridutils';
import * as helpers from '../../../core/services/helpers/helpers';

@Component({
    selector: 'servder-form',
    templateUrl: '/app/configurator/components/server/server-dag-settings.component.html',
    providers: [
        HttpModule,
        RESTService,
        ServersLocationService,
        gridHelpers.CommonUtils
    ]
})
//export class ServerDiskSettings implements OnInit, AfterViewInit {
export class DatabaseSettings implements OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    deviceId: any;
    data: wijmo.collections.CollectionView;
    errorMessage: any;
    deviceLocationData: any;
    deviceCredentialData: any;
    devicebusinessHourData: any;
    diskSettingsForm: FormGroup;
    currentPageSize: any = 20;
    selectedDbSetting: any;
    selectedDbSettingValue: any;
    selecteddatabases: string;
    selectedDisks: string;
    noDbAlertss: string;
    dbthreshold: string;
    postData: any;
    appComponentService: AppComponentService;
    thresholdTypes: string[];

    constructor(
        private dataProvider: RESTService,
        private formBuilder: FormBuilder,
        private service: RESTService,
        private route: ActivatedRoute,
        protected gridHelpers: gridHelpers.CommonUtils,
       
        appComponentService: AppComponentService) {



        this.diskSettingsForm = this.formBuilder.group({
            'setting': [''],
            'value': [''],
            'devices': ['']


        });

        //this.diskSettingsDataForm = this.formBuilder.group({
        //    'disk_name': [''],
        //    'freespace_threshold': [''],
        //    'threshold_type': [''],
        //    'is_selected': [''],
        //     'percent_free': [''],
        //    'disk_size': [''],
        //    'disk_free': [''],

        //});
        this.thresholdTypes = ["Percent", "GB"];
        this.appComponentService = appComponentService;
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

        //this.dataProvider.get('/Configurator/get_server_disk_settings_data/' + this.deviceId)
        //    .subscribe(
        //    (data) => this.diskSettingsDataForm.setValue(data.data),
        //    (error) => this.errorMessage = <any>error

        //    );
        this.dataProvider.get('/Configurator/get_server_disk_settings_data/' + this.deviceId)
            .subscribe(
            (response) => {


                //this.selectedDiskSetting = response.data.disk_name;
                //this.diskThreshold = response.data.freespace_threshold;
            },

            (error) => {
                this.errorMessage = <any>error
                this.appComponentService.showErrorMessage(this.errorMessage);
            }

            );


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

    applySetting(nameValue: any) {


        //if (this.selectedDiskSetting == "allDisksBypercentage") {

        //    this.selectedDiskSettingValue = this.diskThreshold;
        //}
        //else if (this.selectedDiskSetting == "allDisksByGB")
        //    this.selectedDiskSettingValue = this.diskThreshold;
        //else if (this.selectedDiskSetting == "selectedDisks") {
        //    this.selectedDiskSettingValue = this.selectedDisks;
        //    var slectedDiskSettingValues: DiskSttingsValue[] = [];
        //    for (var _i = 0; _i < this.flex.collectionView.sourceCollection.length; _i++) {

        //        var item = (<wijmo.collections.CollectionView>this.flex.collectionView.sourceCollection)[_i];

        //        if (item.is_selected) {


        //            var dominoserverObject = new DiskSttingsValue();
        //            dominoserverObject.is_selected = item.is_selected;
        //            dominoserverObject.disk_name = item.disk_name;

        //            dominoserverObject.freespace_threshold = item.freespace_threshold;
        //            dominoserverObject.threshold_type = item.threshold_type;
        //            // alert(item.is_selected)
        //            slectedDiskSettingValues.push(dominoserverObject);
        //        }
        //    }
        //    this.diskValues = slectedDiskSettingValues;

        //}

        //else if (this.selectedDiskSetting == "noDiskAlerts")
        //    this.selectedDiskSettingValue = this.diskThreshold;



        //if (this.selectedDiskSetting == "selectedDisks") {

        //    this.postData = {
        //        "setting": this.selectedDiskSetting,
        //        "value": this.diskValues,
        //        "devices": this.deviceId
        //    };

        //}
        //else if (this.selectedDiskSetting == "noDiskAlerts") {
        //    this.postData = {
        //        "setting": this.selectedDiskSetting,
        //        "value": null,
        //        "devices": this.deviceId
        //    };

        //}
        //else {
        //    this.postData = {
        //        "setting": this.selectedDiskSetting,
        //        "value": this.selectedDiskSettingValue,
        //        "devices": this.deviceId
        //    };
        //}
        this.dataProvider.put('/Configurator/save_server_disk_settings', this.postData)
            .subscribe(
            response => {

                if (response.status == "Success") {

                    this.appComponentService.showSuccessMessage(response.message);

                } else {

                    this.appComponentService.showErrorMessage(response.message);
                }
            });
    }


    get pageSize(): number {
        return this.data.pageSize;
    }

    //set pageSize(value: number) {
    //    if (this.data.pageSize != value) {
    //        this.data.pageSize = value;
    //        this.data.refresh();
    //        var obj = {
    //            name: this.gridHelpers.getGridPageName("DominoServerDiskSettings", this.authService.CurrentUser.email),
    //            value: value
    //        };

    //        this.service.put(`/services/set_name_value`, obj)
    //            .subscribe(
    //            (data) => {

    //            },
    //            (error) => console.log(error)
    //            );
    //    }
    //}
}