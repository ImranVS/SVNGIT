﻿//import {Component, OnInit, ViewChild, AfterViewInit} from '@angular/core';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import { CommonModule } from '@angular/common';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import {AppNavigator} from '../../../navigation/app.navigator.component';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as wjCoreModule from 'wijmo/wijmo.angular2.core';
import {WindowsServicesValue} from '../../models/windows-services';
import {WidgetComponent} from '../../../core/widgets';


@Component({
    templateUrl: '/app/configurator/components/serverSettings/configurator-windows-services .component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})



export class WindowsServices implements WidgetComponent, OnInit {
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    @Input() settings: any;

    data: wijmo.collections.CollectionView;
    devices: string;
    selectedSetting: any;
    deviceTypeData: any;
    errorMessage: any;
    selectedSettingValue: any;
    selectedServerType: string;
    currentForm: FormGroup;
    postData: any;


    constructor(private service: RESTService,
        private formBuilder: FormBuilder) {
        this.currentForm = this.formBuilder.group({
            'setting': [''],
            'value': [''],
            'devices': ['']


        });
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
 
    

    ngOnInit() {        
        this.service.get('/Configurator/get_windows_services')
            .subscribe(
            response => {
                this.deviceTypeData = response.data.ComboBoxData;
                console.log(response.data);
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data.GridData));
                this.data.pageSize = 10;
               
            }); 

 
    }
    getServices()
    {
        alert("get Services");
    }
        //this.service.get('/configurator/windows_services')
        //    .subscribe(
        //    (response) => {
        //        this.deviceTypeData = response.data;

        //    },
        //    (error) => this.errorMessage = <any>error
        //);

    
    applySetting() {
        var slectedWindowsServicesValues: WindowsServicesValue[] = [];
        for (var _i = 0; _i < this.flex.collectionView.sourceCollection.length; _i++) {
            var item = (<wijmo.collections.CollectionView>this.flex.collectionView.sourceCollection)[_i];
            if (item.is_selected) {
                var windowsservicesObject = new WindowsServicesValue();
               
                windowsservicesObject.service_name = item.service_name;
                slectedWindowsServicesValues.push(windowsservicesObject);
            }

        }
        var postData = {
            "setting": "",
            "value": slectedWindowsServicesValues,
            "devices": this.devices
        };
        console.log(postData);
        this.currentForm.setValue(postData);
        this.service.put('/Configurator/save_windows_services', postData)
            .subscribe(
            response => {

            });
    }


    changeInDevices(devices: string) {
        this.devices = devices;
    }
}



