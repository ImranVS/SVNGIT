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
    dbthreshold: number;
    postData: any;
    appComponentService: AppComponentService;
    thresholdTypes: string[];

    copyThreshold: number;
    replayThreshold: number;

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
        this.dataProvider.get('/Configurator/get_dag_database_info/' + this.deviceId)
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
        this.dataProvider.get('/Configurator/get_dag_database_settings_data/' + this.deviceId)
            .subscribe(
            (response) => {


                this.selectedDbSetting = response.data.database_name;
                this.copyThreshold = response.data.copy_queue_threshold;
                this.replayThreshold = response.data.replay_queue_threshold;
            },

            (error) => {
                this.errorMessage = <any>error
                this.appComponentService.showErrorMessage(this.errorMessage);
            }

            );


    }

    applySetting(nameValue: any) {

        interface DBSettings {
            is_selected: boolean;
            database_name: string;
            server_name: string;
            replay_queue_threshold: number;
            copy_queue_threshold: number;
        }
        let payload:DBSettings[] = [];

        if (this.selectedDbSetting == "allDatabases") {
            let dbSettings: DBSettings = {
                is_selected: true,
                copy_queue_threshold: this.copyThreshold,
                database_name: "allDatabases",
                replay_queue_threshold: this.replayThreshold,
                server_name: "allDatabases"
            };
            payload.push(dbSettings);
        }
        else if (this.selectedDbSetting == "selectedDatabases") {
            for (var _i = 0; _i < this.flex.collectionView.sourceCollection.length; _i++) {
                var item = (<wijmo.collections.CollectionView>this.flex.collectionView.sourceCollection)[_i];
                if (item.is_selected) {
                    let dbSetting: DBSettings = {
                        is_selected: item.is_selected,
                        copy_queue_threshold: item.copy_queue_threshold,
                        database_name: item.database_name,
                        replay_queue_threshold: item.replay_queue_threshold,
                        server_name: item.server_name
                    };
                    payload.push(dbSetting);
                }
            }
        }

        else if (this.selectedDbSetting == "noAlerts") {
            let dbSettings: DBSettings = {
                is_selected: true,
                copy_queue_threshold: 0,
                database_name: "noAlerts",
                replay_queue_threshold: 0,
                server_name: "noAlerts"
            };
            payload.push(dbSettings);
        }

        this.dataProvider.put(`/Configurator/save_dag_database_settings?id=${this.deviceId}`, payload)
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