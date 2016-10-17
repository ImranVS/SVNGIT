import {Component, OnInit, ViewChild, AfterViewInit, Output, EventEmitter} from '@angular/core';
import { CommonModule } from '@angular/common';
import {HTTP_PROVIDERS}    from '@angular/http';
import {RESTService} from '../../core/services';
import {GridBase} from '../../core/gridBase';

import {AppNavigator} from '../../navigation/app.navigator.component';
import * as wjFlexGrid from 'wijmo/wijmo.angular2.grid';
import * as wjFlexGridFilter from 'wijmo/wijmo.angular2.grid.filter';
import * as wjFlexGridGroup from 'wijmo/wijmo.angular2.grid.grouppanel';
import * as wjFlexInput from 'wijmo/wijmo.angular2.input';
import * as wjCoreModule from 'wijmo/wijmo.angular2.core';;


@Component({
    selector: 'server-location-list',
    templateUrl: '/app/configurator/components/server-list-location.component.html',
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
export class ServersLocation extends GridBase {
    @Output() checkedDevices = new EventEmitter();   

    devices: string[]=[];
    constructor(service: RESTService) {
        super(service, '/Configurator/device_list');
        this.formName = "Server Location Grid";        
    }
    saveBusinessHour() {
        this.saveGridRow('/Configurator/save_business_hours');
    }
    delteBusinessHour() {
        this.delteGridRow('/Configurator/delete_business_hours/');
    }
  
    serverCheck(value,event) {
     
        if (event.target.checked)
            this.devices.push(value);
        else {          
            this.devices.splice(this.devices.indexOf(value), 1);
        }
        this.checkedDevices.emit(this.devices);
    }
}



