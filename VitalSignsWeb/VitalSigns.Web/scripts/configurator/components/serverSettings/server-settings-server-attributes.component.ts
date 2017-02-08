import {Component, OnInit, EventEmitter, Input, Output, ViewChild, AfterViewInit} from '@angular/core';
import {FormBuilder, FormGroup, FormControl, Validators} from '@angular/forms';
import { CommonModule } from '@angular/common';
import {HttpModule}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';
import {DeviceAttributeValue} from '../../models/device-attribute';
import {AppComponentService} from '../../../core/services';
import {ServersLocationService} from './serverattributes-view.service';
import { ServersLocation } from '../server-list-location.component';



@Component({
    templateUrl: '/app/configurator/components/serverSettings/server-settings-server-attributes.component.html',
    providers: [
        HttpModule,
        RESTService,
        ServersLocationService
    ]
})
export class DeviceAttributes implements OnInit {
   // @ViewChild('attributeGrid') attributeGrid: wijmo.grid.FlexGrid;
    @ViewChild('combo') combo: wijmo.input.ComboBox;
    @Output() type = new EventEmitter();  
    @Input() currentDeviceType: string;
    devices: string = "";
    attributes: string[] = [];
    deviceTypeData: any;
    errorMessage: any;
    selectedDeviceType: any;
    checkedDevices: any;
    currentForm: FormGroup;
    protected service: RESTService;
    protected appComponentService: AppComponentService;
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    data: wijmo.collections.CollectionView;
    public serversLocationsService: ServersLocationService;
    //data: wijmo.collections.CollectionView;
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
    constructor(service: RESTService, serversLocationsService: ServersLocationService,
        private formBuilder: FormBuilder, appComponentService: AppComponentService) {
       
        this.service = service;
        this.appComponentService = appComponentService; 

        this.serversLocationsService = serversLocationsService;
       
        this.currentForm = this.formBuilder.group({
            'setting': [''],
            'value': [''],
            'devices': ['']

           
        });
       this.serversLocationsService = serversLocationsService;
     
        
    }  

    changeInDevices(devices: any) {
        this.devices = devices;
        this.checkedDevices = devices;
    }

    onCellEditEnding(grid: wijmo.grid.FlexGrid, e: wijmo.grid.CellEditEndingEventArgs) {
    
        //let isPercent: boolean = grid.selectedRows[0].dataItem.unit_of_measurement === 'Percentage Used (eg:- for 90% = 0.90)';
        let isPercent: boolean = grid.selectedRows[0].dataItem.is_percentage === true;
        let newValue: number = parseFloat(grid.activeEditor.value);
        
        if (isPercent && (isNaN(newValue) || newValue < 0 || newValue > 100)) {
            e.cancel = true;
            e.stayInEditMode = true;
        }
            
    }

    onDeviceTypeIndexChanged(event: wijmo.EventArgs) {
        this.currentDeviceType = this.combo.text;
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
        this.serversLocationsService.refreshServerLocations(this.selectedDeviceType);
    }
 
    ngOnInit()
    {     
        this.service.get('/Configurator/get_device_attributes?type=' + this.selectedDeviceType)
            .subscribe(
            response => {
                if (response.status == "Success") {
                    this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(response.data));
                    this.data.pageSize = 10;
                } else {
                    this.appComponentService.showErrorMessage(response.message);
                }

            }, error => {
                var errorMessage = <any>error;
                this.appComponentService.showErrorMessage(errorMessage);
            });
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
          //  var value = this.attributes.filter((record) => record == item.id);
           // if (value.length > 0){
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

    serverCheck(value, event) {

        if (event.target.checked) {
           // this.attributes.push(value);
             this.flex.collectionView.currentItem.is_selected = true;
        }
        else {
           // this.attributes.splice(this.devices.indexOf(value), 1);
            this.flex.collectionView.currentItem.is_selected = false;
        }     
    }

    }




