import {Component, OnInit, EventEmitter, Input, Output, ViewChild, AfterViewInit} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import { CommonModule } from '@angular/common';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import {DeviceAttributeValue} from '../../models/device-attribute';


import {AppNavigator} from '../../../navigation/app.navigator.component';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as wjCoreModule from 'wijmo/wijmo.angular2.core';;


@Component({
    templateUrl: '/app/configurator/components/serverSettings/server-settings-server-attributes.component.html',
    providers: [
        HttpModule,
        RESTService
    ]
})
export class DeviceAttributes extends GridBase implements OnInit {
    @ViewChild('combo') combo: wijmo.input.ComboBox;
    @Output() type = new EventEmitter();  
    @Input() currentDeviceType: string;
    devices: string;
    deviceTypeData: any;
    attributedata: any;
    errorMessage: any;
    selectedDeviceType: any;
    currentForm: FormGroup;
    constructor(service: RESTService,
        private formBuilder: FormBuilder) {
        super(service);
        this.currentForm = this.formBuilder.group({
            'setting': [''],
            'value': [''],
            'devices': ['']


        });
    }  
  
    changeInDevices(devices: string) {
        this.devices = devices;
       
    }

    onDeviceTypeIndexChanged(event: wijmo.EventArgs) {
        this.currentDeviceType = this.combo.selectedItem.Text;
        this.type.emit(this.currentDeviceType);
        this.selectedDeviceType = this.selectedDeviceType;
        this.service.get('/configurator/get_device_attributes/'+ this.selectedDeviceType)
            .subscribe(
            (response) => {
                this.attributedata = response.data;
                console.log(this.attributedata)
            },
            (error) => this.errorMessage = <any>error
            );



    }

    ngOnInit()
    {
       // this.initialGridBind('/Configurator/get_device_attributes');
        this.service.get('/configurator/get_device_type_list')
            .subscribe(
            (response) => {
                this.deviceTypeData = response.data;                
            },
            (error) => this.errorMessage = <any>error
            );
    }
    applySetting() { 
       var slectedAttributeValues: DeviceAttributeValue[] = [];
        for (var _i = 0; _i < this.flex.collectionView.sourceCollection.length; _i++) {
            var item = (<wijmo.collections.CollectionView>this.flex.collectionView.sourceCollection)[_i];
            if (item.is_selected) {
                var deviceAttrObject=new DeviceAttributeValue();
                deviceAttrObject.value = item.default_value;
                deviceAttrObject.field_name = item.field_name;
                deviceAttrObject.datatype = item.datatype;
                slectedAttributeValues.push(deviceAttrObject);
            }

        }
        var postData = {
            "setting": "",
            "value": slectedAttributeValues,
            "devices": this.devices
        };   
        console.log(postData);
        this.currentForm.setValue(postData);
        this.service.put('/Configurator/save_device_attributes', postData)
            .subscribe(
            response => {

            });
    }
}



