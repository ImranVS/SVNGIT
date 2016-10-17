import {Component, OnInit, ViewChild, AfterViewInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import {HTTP_PROVIDERS}    from '@angular/http';
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


@Component({
    templateUrl: '/app/configurator/components/serverSettings/configurator-windows-services .component.html',
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
export class WindowsServices extends GridBase  {  
    devices: string;
    selectedSetting: any;
    deviceTypeData: any;
    errorMessage: any;
    selectedSettingValue: any;
    selectedServerType: string;
    currentForm: FormGroup;
    postData: any;

    constructor(service: RESTService, private formBuilder: FormBuilder) {
        super(service, '/Configurator/windows_services');
      
        this.currentForm = this.formBuilder.group({
            'setting': [''],
            'value': [''],
            'devices': ['']


        });
    }   

    ngOnInit() {
        this.service.get('/configurator/get_Device_type__list')
            .subscribe(
            (response) => {
                this.deviceTypeData = response.data;

            },
            (error) => this.errorMessage = <any>error
            );
    }

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
        this.service.put('/Configurator/save_windows_services', postData);
    }


    changeInDevices(devices: string) {
        this.devices = devices;
    }
}



