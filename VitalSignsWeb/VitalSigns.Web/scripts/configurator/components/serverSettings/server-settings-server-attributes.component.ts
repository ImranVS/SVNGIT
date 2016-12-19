import {Component, OnInit, EventEmitter, Input, Output, ViewChild, AfterViewInit} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import { CommonModule } from '@angular/common';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import {DeviceAttributeValue} from '../../models/device-attribute';
import {AppComponentService} from '../../../core/services';


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
    @ViewChild('attributeGrid') attributeGrid: wijmo.grid.FlexGrid;
    @ViewChild('combo') combo: wijmo.input.ComboBox;
    @Output() type = new EventEmitter();  
    @Input() currentDeviceType: string;
    devices: string="";
    deviceTypeData: any;
    errorMessage: any;
    selectedDeviceType: any;
    currentForm: FormGroup;
    //data: wijmo.collections.CollectionView;
    constructor(service: RESTService,
        private formBuilder: FormBuilder, appComponentService: AppComponentService) {
        super(service, appComponentService);
        this.currentForm = this.formBuilder.group({
            'setting': [''],
            'value': [''],
            'devices': ['']


        });
    }  
  
    changeInDevices(devices: string) {
        this.devices = devices;
       
    }

    onCellEditEnding(grid: wijmo.grid.FlexGrid, e: wijmo.grid.CellEditEndingEventArgs) {
    
        let isPercent: boolean = grid.selectedRows[0].dataItem.unit_of_measurement === 'Percentage Used (eg:- for 90% = 0.90)';
        let newValue: number = parseFloat(grid.activeEditor.value);
        
        if (isPercent && (isNaN(newValue) || newValue < 0 || newValue > 1)) {
            e.cancel = true;
            e.stayInEditMode = true;
        }
            
    }

    onDeviceTypeIndexChanged(event: wijmo.EventArgs) {
        this.currentDeviceType = this.combo.selectedItem.Text;
        this.type.emit(this.currentDeviceType);
        this.selectedDeviceType = this.selectedDeviceType;
        this.service.get('/configurator/get_device_attributes?type='+ this.selectedDeviceType)
            .subscribe(
            (response) => {
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));               
                this.data.pageSize = 10;
            },
            (error) => this.errorMessage = <any>error
            );



    }
    //loadData() {
    //    this.service.get('/configurator/get_device_attributes?type=' + this.selectedDeviceType)
    //        .subscribe(
    //        (response) => {
    //            this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
    //            this.data.pageSize = 10;
    //        },
    //        (error) => this.errorMessage = <any>error
    //        );
    //}
    ngOnInit()
    {
        this.initialGridBind('/Configurator/get_device_attributes?type='+this.selectedDeviceType);
        this.service.get('/configurator/get_device_type_list')
            .subscribe(
            (response) => {
                this.deviceTypeData = response.data;                
            },
            (error) => this.errorMessage = <any>error
        );
       // this.loadData();
    }
    applySetting() { 
        var slectedAttributeValues: DeviceAttributeValue[] = [];
        for (var _i = 0; _i < this.attributeGrid.collectionView.sourceCollection.length; _i++) {
            var item = (<wijmo.collections.CollectionView>this.attributeGrid.collectionView.sourceCollection)[_i];
            if (item.is_selected) {
                var deviceAttrObject=new DeviceAttributeValue();
                deviceAttrObject.value = item.default_value;
                deviceAttrObject.field_name = item.field_name;
                deviceAttrObject.datatype = item.datatype;
                deviceAttrObject.defaultboolvalue = item.defaultboolvalue;
                slectedAttributeValues.push(deviceAttrObject);
            }

       }



        var postData = {
            "setting": "",
            "value": slectedAttributeValues,
            "devices": this.devices
        };  
        this.currentForm.setValue(postData);
        this.service.put('/Configurator/save_device_attributes', postData)
            .subscribe(
            response => {
                if (response.status == "Success") {

                    this.appComponentService.showSuccessMessage(response.message);                  

                } else {

                    this.appComponentService.showErrorMessage(response.message);
                }

            });
    }
  

    }




