import {Component, OnInit, ViewChild, AfterViewInit} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import { CommonModule } from '@angular/common';
import {HTTP_PROVIDERS}    from '@angular/http';
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
    templateUrl: '/app/configurator/components/serverSettings/configurator-server-attributes.component.html',
    directives: [
        wjFlexGrid.WjFlexGrid,
        wjFlexGrid.WjFlexGridColumn,
        wjFlexGrid.WjFlexGridCellTemplate,
        wjFlexGridFilter.WjFlexGridFilter,
        wjFlexGridGroup.WjGroupPanel,
        wjFlexInput.WjMenu,
        wjFlexInput.WjMenuItem,
        AppNavigator
    ],  
    providers: [
        HTTP_PROVIDERS,
        RESTService
    ]
})
export class DeviceAttributes extends GridBase implements OnInit {  
    devices: string;
    deviceTypeData: any;
    errorMessage: any;
    selectedDeviceType: any;
    slectedAttributeValues: DeviceAttributeValue[] = [];
    currentForm: FormGroup;
    constructor(service: RESTService,
        private formBuilder: FormBuilder) {
        super(service, '/Configurator/get_device_attributes');
        this.currentForm = this.formBuilder.group({
            'setting': [''],
            'value': [''],
            'devices': ['']


        });
    }  
     
    changeInDevices(devices: string) {
        this.devices = devices;
       
    }

    ngOnInit()
    {
        this.service.get('/configurator/get_Device_type__list')
            .subscribe(
            (response) => {
                this.deviceTypeData = response.data;                
            },
            (error) => this.errorMessage = <any>error
            );
    }
    applySetting() { 
        for (var _i = 0; _i < this.flex.collectionView.sourceCollection.length; _i++) {
            var item = (<wijmo.collections.CollectionView>this.flex.collectionView.sourceCollection)[_i];
            if (item.is_selected) {
                var deviceAttrObject=new DeviceAttributeValue();
                deviceAttrObject.value = item.default_value;
                deviceAttrObject.field_name = item.field_name;
                this.slectedAttributeValues.push(deviceAttrObject);
            }

        }
        var postData = {
            "setting": "",
            "value": this.slectedAttributeValues,
            "devices": this.devices
        };   
        this.currentForm.setValue(postData);
        this.service.put('/Configurator/save_device_attributes', postData);
    }
}



