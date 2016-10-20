import {Component, OnInit, AfterViewInit, ViewChild, Output, EventEmitter} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {Router, ActivatedRoute} from '@angular/router';
import {HttpModule}    from '@angular/http';
import {GridBase} from '../../../core/gridBase';
import {RESTService} from '../../../core/services';
import {DiskSttingsValue} from '../../models/server-disk-settings';

@Component({
    selector: 'servder-form',
    templateUrl: '/app/configurator/components/server/server-maintenance-windows.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
//export class ServerDiskSettings implements OnInit, AfterViewInit {
export class MaintenanceWindows implements OnInit {

    data: wijmo.collections.CollectionView;
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
    diskValues: any;
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;

    constructor(
        private dataProvider: RESTService,
        private formBuilder: FormBuilder) {

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
    }



    ngOnInit() {
        alert("hi");
    }

   
    changeInDevices(server: string) {
        this.devices = server;
    }

    get pageSize(): number {
        return this.data.pageSize;
    }

    set pageSize(value: number) {
        if (this.data.pageSize != value) {
            this.data.pageSize = value;
            this.data.refresh();
        }
    }
   
}