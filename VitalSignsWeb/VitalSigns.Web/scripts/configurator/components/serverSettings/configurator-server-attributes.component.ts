import {Component, OnInit, ViewChild, AfterViewInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import {HTTP_PROVIDERS}    from '@angular/http';
import {RESTService} from '../../../core/services';
import {GridBase} from '../../../core/gridBase';

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
    constructor(service: RESTService) {
        super(service, '/Configurator/get_device_attributes');
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
}



