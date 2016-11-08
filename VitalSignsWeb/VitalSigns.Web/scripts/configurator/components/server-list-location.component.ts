﻿import {Component, OnInit, ViewChild, AfterViewInit, Output, Input, EventEmitter} from '@angular/core';
import { CommonModule } from '@angular/common';
import {HttpModule}    from '@angular/http';
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
    providers: [
        HttpModule,
        RESTService
    ]
})
export class ServersLocation implements OnInit {
   
    @Output() checkedDevices = new EventEmitter();  
    @Input() deviceList: any; 
    @Input() deviceType: any; 
    @ViewChild('flex') flex: wijmo.grid.FlexGrid;
    data: wijmo.collections.CollectionView;
    devices: string[]=[];
    constructor(private service: RESTService) {
       
    } 
    serverCheck(value,event) {
     
        if (event.target.checked)
            this.devices.push(value);
        else {          
            this.devices.splice(this.devices.indexOf(value), 1);
        }
        this.checkedDevices.emit(this.devices);
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
    onDeviceListChange() {
        console.log(this.deviceList);
    }

    ngOnInit() {       
        console.log(this.deviceList);
        this.service.get("/Configurator/device_list")
            .subscribe(
            response => {
                var resultData:any = [];
                for (var item of response.data) {
                    if (this.deviceList) {
                        var value = this.deviceList.filter((record) => record.toLocaleLowerCase().indexOf(item.id.toLocaleLowerCase()) !== -1);                       
                        if (value.length > 0) {
                            item.is_selected = true;
                        }
                    } 
                    resultData.push(item);
                }                
                if (this.deviceType) {
                    resultData = resultData.filter((record) => record.device_type == this.deviceType);

                }               
                this.data = new wijmo.collections.CollectionView(new wijmo.collections.ObservableArray(resultData));
                this.data.groupDescriptions.push(new wijmo.collections.PropertyGroupDescription("location_name"));
                this.data.pageSize = 10;               
            });        
    }  
}



